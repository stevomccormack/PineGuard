import { parseFile } from "./parsing/csharp.js";
import { findRepoRoot, listTrackedFiles, readRepoFile } from "./repo.js";
import {
    type CatalogEntry,
    type RuleScope,
    getAllRules,
    getRule,
} from "./catalog.js";
import type {
    ExceptionsConfig,
    Finding,
    RuleContext,
    VocabularyConfig,
} from "./types.js";

/**
 * The audit engine: resolves which rules to run, builds the shared
 * {@link RuleContext} once, runs them, and applies exceptions + the (not yet
 * built) baseline. Plan §7 "Functional plan", §9.2 P1.5.
 *
 * Pipeline shape (fixed here; P3.1 only fills in {@link applyBaseline}'s
 * body): `resolveSelection → buildContext → rule.run → applyExceptions →
 * applyBaseline → (reporter, in src/commands/audit.ts)`.
 */

const VOCABULARY_PATH = "docs/ai/specs/language/vocabulary.json";
const EXCEPTIONS_PATH = "apps/cli/config/exceptions.json";

function isPlainObject(value: unknown): value is Record<string, unknown> {
    return typeof value === "object" && value !== null && !Array.isArray(value);
}

function parseVocabulary(raw: string): VocabularyConfig {
    const parsed: unknown = JSON.parse(raw);
    if (!isPlainObject(parsed)) {
        throw new Error(`${VOCABULARY_PATH} did not parse to a JSON object`);
    }
    return parsed as unknown as VocabularyConfig;
}

function parseExceptions(raw: string): ExceptionsConfig {
    const parsed: unknown = JSON.parse(raw);
    if (!isPlainObject(parsed)) {
        throw new Error(`${EXCEPTIONS_PATH} did not parse to a JSON object`);
    }
    return parsed as ExceptionsConfig;
}

/**
 * Builds the shared {@link RuleContext} for one real `pineguard audit`
 * invocation: every tracked file, the cached C# parser, and the parsed
 * vocabulary + exceptions config. Built once per invocation and handed to
 * every selected rule unchanged.
 *
 * @param rootDir repo root to scan; defaults to {@link findRepoRoot}. Rule
 *   *tests* never call this directly — they build a minimal `{ rootDir }`
 *   pointed at a fixture directory via `test/support/runRule.ts` instead.
 */
export function buildContext(rootDir: string = findRepoRoot()): RuleContext {
    const trackedFiles = listTrackedFiles(undefined, rootDir);
    const vocabulary = parseVocabulary(readRepoFile(VOCABULARY_PATH, rootDir));
    const exceptions = parseExceptions(readRepoFile(EXCEPTIONS_PATH, rootDir));

    return {
        rootDir,
        trackedFiles,
        parseFile,
        vocabulary,
        exceptions,
    };
}

/** What {@link resolveSelection} decided to run, or the usage error that stopped it. */
export interface SelectionResult {
    /** The rules to run, in stable slug order. Empty when `error` is set. */
    entries: CatalogEntry[];
    /** Set when a requested slug/legacy id could not be resolved (a usage error, exit code 2). */
    error?: string;
}

/**
 * Resolves which catalog rules a `pineguard audit …` invocation should run.
 *
 * ## Combination semantics (decided here — plan §7 leaves the exact
 * combination unspecified beyond listing the individual selectors)
 *
 * - **Explicit rule identifiers override `--scope`/`--gate`.** Naming rules
 *   by hand (`pineguard audit must-usage Rule50`) means "run exactly these";
 *   a stray `--gate` or `--scope` alongside an explicit list does not
 *   additionally narrow it. This is the least surprising reading for a CLI —
 *   `eslint some-rule.js --quiet` still lints exactly the file you named,
 *   `--quiet` doesn't also filter which file gets linted.
 * - **No explicit identifiers** — an empty `rules` array, or the single
 *   token `"all"` — selects every registered rule, and *then* `--scope` and
 *   `--gate` narrow that set (AND'd together when both are given). This is
 *   `pineguard audit`, `pineguard audit all`, `pineguard audit --gate`,
 *   `pineguard audit --scope docs --gate`, etc.
 * - `"all"` is only recognised as the selection's sole token. `"all"`
 *   combined with anything else (`pineguard audit all must-usage`) falls
 *   into the first bullet instead: every token, `"all"` included, is
 *   resolved as a slug/legacy id — since no rule is ever registered under
 *   the slug `"all"`, that combination reports it as an unknown rule rather
 *   than silently doing something unexpected.
 * - An unresolvable slug/legacy id is a usage error (exit code 2), naming
 *   the offending token so the caller can fix the typo or check `--list`.
 */
export function resolveSelection(
    rules: readonly string[],
    options: { scope?: RuleScope; gate?: boolean } = {},
): SelectionResult {
    const isImplicitAll =
        rules.length === 0 || (rules.length === 1 && rules[0] === "all");

    if (!isImplicitAll) {
        const entries: CatalogEntry[] = [];
        for (const token of rules) {
            const entry = getRule(token);
            if (!entry) {
                return {
                    entries: [],
                    error: `unknown rule "${token}" (not a registered slug or legacy id; run "pineguard audit --list" to see the catalog)`,
                };
            }
            entries.push(entry);
        }
        return { entries };
    }

    let entries = getAllRules();
    if (options.scope !== undefined) {
        entries = entries.filter((entry) => entry.scope === options.scope);
    }
    if (options.gate) {
        entries = entries.filter((entry) => entry.gate);
    }
    return { entries };
}

/**
 * Exception-matching shape (plan §7 "Exceptions application"): given
 * `config/exceptions.json`'s `{ "<slug>": ["<substring>", ...] }`, a finding
 * is suppressed when any of its rule's substrings appears in either the
 * finding's `file` or its `message`. Deliberately simple for P1.5's "wire it
 * up" mandate; the real ported exceptions (from
 * `tools/audit-cli/test-audit-exceptions.json`) are P5's job and can refine
 * the matching shape then if a plain substring proves too coarse.
 */
export function applyExceptions(
    findings: readonly Finding[],
    exceptions?: ExceptionsConfig,
): Finding[] {
    if (!exceptions) {
        return [...findings];
    }

    return findings.filter((finding) => {
        const patterns = exceptions[finding.rule];
        if (!patterns) {
            return true;
        }
        const excepted = patterns.some(
            (pattern) =>
                finding.file.includes(pattern) ||
                finding.message.includes(pattern),
        );
        return !excepted;
    });
}

/**
 * The baseline ratchet (plan §4.4) is not built yet — P3.1's job. This is a
 * clearly-marked no-op pass-through so the pipeline shape
 * (`run rules → exceptions → baseline → reporter`) is already correct today;
 * P3.1 only needs to fill in this function's body (reading
 * `config/baseline.json`, filtering findings whose stable key is already
 * baselined, and wiring `--update-baseline`/`--no-baseline`), not restructure
 * the pipeline around it.
 */
// TODO(P3.1): replace this pass-through with the real baseline ratchet.
export function applyBaseline(
    findings: readonly Finding[],
    slug: string,
): Finding[] {
    void slug; // referenced only to keep the signature P3.1 will need; unused today
    return [...findings];
}

/** One rule's final findings (post-exceptions, post-baseline-passthrough), paired with its catalog metadata. */
export interface RuleRunOutcome {
    entry: CatalogEntry;
    findings: Finding[];
}

/** `0` clean, `1` findings, `2` usage/config error (plan §4.2). */
export type AuditExitCode = 0 | 1 | 2;

/** Result of one {@link runAudit} call. */
export interface AuditRunResult {
    exitCode: AuditExitCode;
    /** Set when `exitCode === 2`: what went wrong (an unknown rule, or an unexpected error building the context / running a rule). */
    error?: string;
    /** Empty when `exitCode === 2`. Otherwise one entry per selected rule, in stable slug order. */
    outcomes: RuleRunOutcome[];
}

/** Options for {@link runAudit}. */
export interface AuditRunOptions {
    /** Rule slugs and/or legacy ids to run; empty (or `["all"]`) runs every registered rule. See {@link resolveSelection} for the exact combination semantics with `scope`/`gate`. */
    rules?: readonly string[];
    scope?: RuleScope;
    gate?: boolean;
    /** Repo root override, for tests. Defaults to {@link findRepoRoot}. */
    rootDir?: string;
}

/**
 * Runs one `pineguard audit` invocation end to end: resolves the selection,
 * builds the shared context, runs every selected rule (in parallel — each
 * rule only reads from the shared context, per plan §7 point 4), applies
 * exceptions then the baseline pass-through, and returns the overall result.
 *
 * Never throws: a selection error, a context-build failure (e.g. a missing
 * or malformed `vocabulary.json`/`exceptions.json`), or a rule throwing
 * during `run()` are all reported as `exitCode: 2` with `error` set, rather
 * than rejecting — so callers (the `audit` command, and this module's own
 * tests) always get a plain result object back.
 */
export async function runAudit(
    options: AuditRunOptions = {},
): Promise<AuditRunResult> {
    const selection = resolveSelection(options.rules ?? [], {
        scope: options.scope,
        gate: options.gate,
    });
    if (selection.error !== undefined) {
        return { exitCode: 2, error: selection.error, outcomes: [] };
    }

    try {
        const ctx = buildContext(options.rootDir);
        const outcomes = await Promise.all(
            selection.entries.map(async (entry): Promise<RuleRunOutcome> => {
                const raw = await entry.run(ctx);
                const afterExceptions = applyExceptions(raw, ctx.exceptions);
                const findings = applyBaseline(afterExceptions, entry.slug);
                return { entry, findings };
            }),
        );

        const hasFindings = outcomes.some(
            (outcome) => outcome.findings.length > 0,
        );
        return { exitCode: hasFindings ? 1 : 0, outcomes };
    } catch (error) {
        return {
            exitCode: 2,
            error: error instanceof Error ? error.message : String(error),
            outcomes: [],
        };
    }
}
