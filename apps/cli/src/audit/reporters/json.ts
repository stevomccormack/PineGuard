import { mkdirSync, writeFileSync } from "node:fs";
import { join } from "node:path";

import type { AuditRunResult } from "../engine.js";
import type { Finding } from "../types.js";

/**
 * The `json` reporter (plan §4.2/§7 point 3): writes one file per rule plus
 * a `summary.json` under `artifacts/audit/`, and also returns the same data
 * so the caller (`src/commands/audit.ts`) can print it to stdout — so
 * `pineguard audit --format json | jq …` works without reading the files
 * back off disk.
 *
 * `github` (annotations) and `sarif` are P3.2/P3.3 — not built here.
 */

/** One rule's report, as written to `artifacts/audit/<slug>.json`. */
export interface JsonRuleReport {
    slug: string;
    legacyId?: string;
    scope: string;
    gate: boolean;
    description: string;
    findingCount: number;
    findings: Finding[];
}

/** The row shape inside `summary.json`'s `rules` array — the per-rule count/gate columns, without repeating every finding. */
export interface JsonSummaryRuleRow {
    slug: string;
    findingCount: number;
    gate: boolean;
}

/** `artifacts/audit/summary.json`'s shape. */
export interface JsonSummaryReport {
    generatedAt: string;
    exitCode: number;
    totalFindings: number;
    rules: JsonSummaryRuleRow[];
}

/** The full report this module builds: every rule's findings plus the summary — what gets printed to stdout. */
export interface JsonReport {
    generatedAt: string;
    exitCode: number;
    totalFindings: number;
    rules: JsonRuleReport[];
}

/** Builds a {@link JsonReport} from an {@link AuditRunResult}. Pure — does no I/O; pair with {@link writeJsonReport} to also persist it. */
export function buildJsonReport(result: AuditRunResult): JsonReport {
    const generatedAt = new Date().toISOString();
    const rules: JsonRuleReport[] = result.outcomes.map(
        ({ entry, findings }) => ({
            slug: entry.slug,
            legacyId: entry.legacyId,
            scope: entry.scope,
            gate: entry.gate,
            description: entry.description,
            findingCount: findings.length,
            findings,
        }),
    );

    return {
        generatedAt,
        exitCode: result.exitCode,
        totalFindings: rules.reduce((sum, rule) => sum + rule.findingCount, 0),
        rules,
    };
}

function toSummary(report: JsonReport): JsonSummaryReport {
    return {
        generatedAt: report.generatedAt,
        exitCode: report.exitCode,
        totalFindings: report.totalFindings,
        rules: report.rules.map((rule) => ({
            slug: rule.slug,
            findingCount: rule.findingCount,
            gate: rule.gate,
        })),
    };
}

/**
 * Writes `artifacts/audit/<slug>.json` for every rule in `report`, plus
 * `artifacts/audit/summary.json`, under `rootDir` (creating the directory if
 * needed).
 */
export function writeJsonReport(report: JsonReport, rootDir: string): void {
    const dir = join(rootDir, "artifacts", "audit");
    mkdirSync(dir, { recursive: true });

    for (const rule of report.rules) {
        writeFileSync(
            join(dir, `${rule.slug}.json`),
            `${JSON.stringify(rule, null, 2)}\n`,
            "utf8",
        );
    }

    writeFileSync(
        join(dir, "summary.json"),
        `${JSON.stringify(toSummary(report), null, 2)}\n`,
        "utf8",
    );
}
