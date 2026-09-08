import type { CatalogEntry } from "../catalog.js";
import type { AuditRunResult } from "../engine.js";
import type { Finding } from "../types.js";

/**
 * The `sarif` reporter (plan §4.2/§7 point 3, §9.2 P3.3): emits a SARIF
 * 2.1.0 document (schema: https://json.schemastore.org/sarif-2.1.0.json) so
 * a consumer such as GitHub Code Scanning can ingest `pineguard audit`'s
 * findings. Deliberately minimal, per the plan's "Optional; last" framing —
 * a single `runs[0]`, no `fixes`, `codeFlows`, or `partialFingerprints`.
 *
 * ## `rules[]` derivation
 *
 * One entry per rule *actually selected for this run* (`result.outcomes`),
 * not only the ones that produced ≥1 finding, and not the whole registered
 * catalog. This keeps the document self-describing for exactly what this
 * invocation audited: `pineguard audit --gate --format sarif` lists only the
 * gate rules (whether or not each one happened to be clean this time),
 * while `pineguard audit --format sarif` (no selection) lists every
 * registered rule. A rule this invocation never ran gets no `rules[]` entry.
 *
 * ## Level mapping
 *
 * `entry.gate` (`true` = merge-blocking, same flag the `pretty` reporter's
 * summary table and `--list` already surface) decides severity:
 * `gate: true` → `"error"`, `gate: false` → `"warning"`. Every finding from
 * a given rule shares that rule's level — SARIF's `results[].level` and the
 * matching `rules[].defaultConfiguration.level` are set from the same
 * mapping, so a consumer reading either one agrees on how serious the rule
 * is.
 */

const SARIF_SCHEMA = "https://json.schemastore.org/sarif-2.1.0.json";
const SARIF_VERSION = "2.1.0";
const TOOL_NAME = "pineguard";

/** SARIF's two levels this reporter ever emits — see the module doc comment's "Level mapping" section. */
export type SarifLevel = "error" | "warning";

export interface SarifRule {
    id: string;
    shortDescription: { text: string };
    defaultConfiguration: { level: SarifLevel };
}

export interface SarifLocation {
    physicalLocation: {
        artifactLocation: { uri: string };
        region?: { startLine: number };
    };
}

export interface SarifResult {
    ruleId: string;
    level: SarifLevel;
    message: { text: string };
    locations: SarifLocation[];
}

export interface SarifRun {
    tool: {
        driver: {
            name: string;
            rules: SarifRule[];
        };
    };
    results: SarifResult[];
}

/** The document shape this reporter builds — a single-run SARIF 2.1.0 log. */
export interface SarifLog {
    $schema: string;
    version: "2.1.0";
    runs: SarifRun[];
}

function levelFor(gate: boolean): SarifLevel {
    return gate ? "error" : "warning";
}

function toSarifRule(entry: CatalogEntry): SarifRule {
    return {
        id: entry.slug,
        shortDescription: { text: entry.description },
        defaultConfiguration: { level: levelFor(entry.gate) },
    };
}

function toSarifLocation(finding: Finding): SarifLocation {
    return {
        physicalLocation: {
            artifactLocation: { uri: finding.file },
            ...(finding.line === undefined
                ? {}
                : { region: { startLine: finding.line } }),
        },
    };
}

function toSarifResult(finding: Finding, gate: boolean): SarifResult {
    return {
        ruleId: finding.rule,
        level: levelFor(gate),
        message: { text: finding.message },
        locations: [toSarifLocation(finding)],
    };
}

/**
 * Builds a {@link SarifLog} from an {@link AuditRunResult}. Pure — does no
 * I/O; pair with `JSON.stringify` (see {@link renderSarif}) to serialise it.
 */
export function buildSarifLog(result: AuditRunResult): SarifLog {
    const rules = result.outcomes.map(({ entry }) => toSarifRule(entry));
    const results = result.outcomes.flatMap(({ entry, findings }) =>
        findings.map((finding) => toSarifResult(finding, entry.gate)),
    );

    return {
        $schema: SARIF_SCHEMA,
        version: SARIF_VERSION,
        runs: [
            {
                tool: {
                    driver: {
                        name: TOOL_NAME,
                        rules,
                    },
                },
                results,
            },
        ],
    };
}

/**
 * Renders `result` as a SARIF 2.1.0 JSON document and returns it as a plain
 * string — same convention as `pretty.ts`'s `renderPretty`: the caller
 * (`src/commands/audit.ts`) is the one that actually `console.log`s it.
 */
export function renderSarif(result: AuditRunResult): string {
    return JSON.stringify(buildSarifLog(result), null, 2);
}
