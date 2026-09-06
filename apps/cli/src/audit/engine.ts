import { mkdirSync, writeFileSync } from "node:fs";
import { dirname, join } from "node:path";

import { parseFile } from "./parsing/csharp.js";
import { findRepoRoot, listTrackedFiles, readRepoFile } from "./repo.js";
import {
    type CatalogEntry,
    type RuleScope,
    getAllRules,
    getRule,
} from "./catalog.js";
import type {
    BaselineConfig,
    ExceptionsConfig,
    Finding,
    RuleContext,
    VocabularyConfig,
} from "./types.js";

/**
 * The audit engine: resolves which rules to run, builds the shared
 * {@link RuleContext} once, runs them, and applies exceptions + the baseline
 * ratchet. Plan §7 "Functional plan", §9.2 P1.5/P3.1.
 *
 * Pipeline shape (fixed by P1.5; P3.1 filled in {@link applyBaseline}'s body
 * without otherwise touching this shape): `resolveSelection → buildContext →
 * rule.run → applyExceptions → applyBaseline → (reporter, in
 * src/commands/audit.ts)`. `--update-baseline` is a separate maintenance
 * flow ({@link runUpdateBaseline}) that reuses `buildContext` and
 * `applyExceptions` but deliberately runs every registered rule and skips
 * `applyBaseline` — see that function's doc comment.
 */

const VOCABULARY_PATH = "docs/ai/specs/language/vocabulary.json";
const EXCEPTIONS_PATH = "apps/cli/config/exceptions.json";
const BASELINE_PATH = "apps/cli/config/baseline.json";

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

function parseBaseline(raw: string): BaselineConfig {
    const parsed: unknown = JSON.parse(raw);
    if (!isPlainObject(parsed)) {
        throw new Error(`${BASELINE_PATH} did not parse to a JSON object`);
    }
    return parsed as BaselineConfig;
}

/**
 * Builds the shared {@link RuleContext} for one real `pineguard audit`
 * invocation: every tracked file, the cached C# parser, and the parsed
 * vocabulary + exceptions + baseline config. Built once per invocation and
 * handed to every selected rule unchanged.
 *
 * @param rootDir repo root to scan; defaults to {@link findRepoRoot}. Rule
 *   *tests* never call this directly — they build a minimal `{ rootDir }`
 *   pointed at a fixture directory via `test/support/runRule.ts` instead.
 */
export function buildContext(rootDir: string = findRepoRoot()): RuleContext {
    const trackedFiles = listTrackedFiles(undefined, rootDir);
    const vocabulary = parseVocabulary(readRepoFile(VOCABULARY_PATH, rootDir));
    const exceptions = parseExceptions(readRepoFile(EXCEPTIONS_PATH, rootDir));
    const baseline = parseBaseline(readRepoFile(BASELINE_PATH, rootDir));

    return {
        rootDir,
        trackedFiles,
        parseFile,
        vocabulary,
        exceptions,
        baseline,
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
 * The baseline ratchet (plan §4.4): drops any finding whose `key` is already
 * accepted as pre-existing debt for `slug` in `baseline`, so a rule with
 * only known, already-baselined findings reads as clean while a genuinely
 * *new* finding for that same rule still surfaces.
 *
 * `baseline` is optional and, when omitted (or when it has no entry for
 * `slug`, or an empty one), this is a plain pass-through — every finding
 * comes back unchanged. This is what keeps `pineguard audit --no-baseline`
 * simple (the engine just declines to pass a baseline in for that run,
 * rather than threading a separate bypass flag through this function) and
 * is also why this exact 2-argument call shape (no baseline) still behaves
 * the way P1.5's original no-op placeholder did.
 *
 * Matching is **exact string equality on the full key**, never a per-file or
 * per-rule blanket suppression — this is the load-bearing property plan §11
 * calls out ("Baseline masking real regressions... keys include file +
 * message; a new finding in an already-failing file still fails"). Two
 * findings in the same file, even for the same rule, have different `key`s
 * (see `Finding.key`'s doc comment in `types.ts`) whenever they represent
 * different violations, so baselining one never silently baselines the
 * other.
 */
export function applyBaseline(
    findings: readonly Finding[],
    slug: string,
    baseline?: BaselineConfig,
): Finding[] {
    const acceptedKeys = baseline?.[slug];
    if (acceptedKeys === undefined || acceptedKeys.length === 0) {
        return [...findings];
    }

    const accepted = new Set(acceptedKeys);
    return findings.filter((finding) => !accepted.has(finding.key));
}

/**
 * Runs every *registered* rule (deliberately `getAllRules()`, not a
 * selection — plan §4.4's `--update-baseline` snapshots the whole catalog
 * regardless of any `--scope`/`--gate`/explicit-slug arguments also given on
 * the command line, so the ratchet floor never silently drifts out of sync
 * with rules the invocation happened not to select), applies exceptions
 * (but deliberately *not* the baseline — this computes what the *next*
 * baseline should be, from the current, unsuppressed finding set, per plan
 * §4.4 "run all rules with baseline suppression OFF"), and groups the
 * resulting finding keys by rule slug.
 *
 * Pure with respect to the filesystem (`ctx` is the only input; nothing is
 * read or written here) precisely so it can be unit-tested against fake
 * registered rules without touching `config/baseline.json` — see
 * `writeBaselineFile` for the (separate) persistence step, and
 * `runUpdateBaseline` for the two composed.
 *
 * A rule with zero findings gets **no key** in the result, not an empty
 * array — see `BaselineConfig`'s doc comment in `types.ts` for why that
 * distinction matters (an absent entry is what makes a rule a hard gate).
 */
export async function computeBaselineSnapshot(
    ctx: RuleContext,
): Promise<BaselineConfig> {
    const outcomes = await Promise.all(
        getAllRules().map(async (entry) => {
            const raw = await entry.run(ctx);
            const findings = applyExceptions(raw, ctx.exceptions);
            return { slug: entry.slug, keys: findings.map((f) => f.key) };
        }),
    );

    const snapshot: Record<string, string[]> = {};
    for (const { slug, keys } of outcomes) {
        if (keys.length === 0) {
            continue;
        }
        snapshot[slug] = [...new Set(keys)].sort((a, b) => a.localeCompare(b));
    }
    return snapshot;
}

/**
 * Persists `baseline` to `apps/cli/config/baseline.json` under `rootDir`,
 * normalising it first (slugs sorted, each slug's keys deduplicated and
 * sorted, empty entries dropped) so the committed file's diff stays clean
 * across `--update-baseline` runs regardless of what order the caller's
 * `baseline` object happened to be built in — {@link computeBaselineSnapshot}
 * already produces normalised output, but this function does not rely on
 * that (it re-normalises unconditionally), so it is correct as a standalone
 * building block too, e.g. in a future tool that hand-edits a subset of the
 * baseline.
 */
export function writeBaselineFile(
    baseline: BaselineConfig,
    rootDir: string = findRepoRoot(),
): void {
    const normalised: Record<string, string[]> = {};
    for (const slug of Object.keys(baseline).sort((a, b) =>
        a.localeCompare(b),
    )) {
        const keys = baseline[slug];
        if (keys === undefined || keys.length === 0) {
            continue;
        }
        normalised[slug] = [...new Set(keys)].sort((a, b) =>
            a.localeCompare(b),
        );
    }

    const path = join(rootDir, BASELINE_PATH);
    mkdirSync(dirname(path), { recursive: true });
    writeFileSync(path, `${JSON.stringify(normalised, null, 2)}\n`, "utf8");
}

/** Result of {@link runUpdateBaseline}. */
export interface UpdateBaselineResult {
    /** `true` once `apps/cli/config/baseline.json` has been overwritten. `false` only alongside `error`. */
    written: boolean;
    /** The snapshot written, grouped by slug, keys deduplicated and sorted. Empty when `error` is set. */
    baseline: BaselineConfig;
    /** Set when building the context or running a rule failed; mirrors {@link AuditRunResult.error}. */
    error?: string;
}

/**
 * `pineguard audit --update-baseline`'s engine-side implementation (plan
 * §4.4): builds a real context, snapshots every registered rule's current,
 * unsuppressed findings ({@link computeBaselineSnapshot}), and overwrites
 * `apps/cli/config/baseline.json` with the result
 * ({@link writeBaselineFile}).
 *
 * This is a maintenance operation, not a pass/fail check — unlike
 * {@link runAudit}, there is no finding-based exit code here; the caller
 * (`src/commands/audit.ts`) exits `0` on `written: true` regardless of how
 * many findings were baselined, and `2` only if something actually went
 * wrong (mirrors {@link runAudit}'s `exitCode: 2` contract: never throws,
 * reports failure in the result instead).
 */
export async function runUpdateBaseline(
    rootDir: string = findRepoRoot(),
): Promise<UpdateBaselineResult> {
    try {
        const ctx = buildContext(rootDir);
        const baseline = await computeBaselineSnapshot(ctx);
        writeBaselineFile(baseline, rootDir);
        return { written: true, baseline };
    } catch (error) {
        return {
            written: false,
            baseline: {},
            error: error instanceof Error ? error.message : String(error),
        };
    }
}

/** One rule's final findings (post-exceptions, post-baseline-ratchet unless `--no-baseline` bypassed it), paired with its catalog metadata. */
export interface RuleRunOutcome {
    entry: CatalogEntry;
    findings: Finding[];
}

/**
 * Runs every entry in `entries` against `ctx` (in parallel — each rule only
 * reads from the shared context, per plan §7 point 4), applying exceptions
 * unconditionally and the baseline ratchet only when `applyRatchet` is
 * `true`. This is `runAudit`'s per-rule pipeline, extracted so it can be
 * exercised directly against a hand-built `ctx`/`entries` in tests (see
 * `test/audit/baseline.test.ts`) without going through
 * {@link buildContext}'s real filesystem/`git` reads — the exact same code
 * path a real `pineguard audit` invocation runs, just callable without a
 * real repo checkout backing it.
 */
export async function runSelectedRules(
    ctx: RuleContext,
    entries: readonly CatalogEntry[],
    applyRatchet: boolean,
): Promise<RuleRunOutcome[]> {
    return Promise.all(
        entries.map(async (entry): Promise<RuleRunOutcome> => {
            const raw = await entry.run(ctx);
            const afterExceptions = applyExceptions(raw, ctx.exceptions);
            const findings = applyRatchet
                ? applyBaseline(afterExceptions, entry.slug, ctx.baseline)
                : afterExceptions;
            return { entry, findings };
        }),
    );
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
    /**
     * Whether to apply the baseline ratchet (plan §4.4). Defaults to `true`
     * (baselined findings are suppressed) when omitted or `undefined`.
     * `pineguard audit --no-baseline` maps to `false`: the run bypasses
     * `applyBaseline` entirely for every selected rule, showing the full,
     * unfiltered finding set (the total debt) — this composes correctly with
     * `--gate`, since gate selection (which *rules* run) and baseline
     * application (which of a run rule's *findings* count) are independent
     * stages of the same pipeline.
     */
    baseline?: boolean;
    /** Repo root override, for tests. Defaults to {@link findRepoRoot}. */
    rootDir?: string;
}

/**
 * Runs one `pineguard audit` invocation end to end: resolves the selection,
 * builds the shared context, runs every selected rule (in parallel — each
 * rule only reads from the shared context, per plan §7 point 4), applies
 * exceptions then the baseline ratchet (`options.baseline === false` — i.e.
 * `--no-baseline` — bypasses that last step entirely, per rule, so every
 * finding is shown), and returns the overall result.
 *
 * Never throws: a selection error, a context-build failure (e.g. a missing
 * or malformed `vocabulary.json`/`exceptions.json`/`baseline.json`), or a
 * rule throwing during `run()` are all reported as `exitCode: 2` with
 * `error` set, rather than rejecting — so callers (the `audit` command, and
 * this module's own tests) always get a plain result object back.
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
        const outcomes = await runSelectedRules(
            ctx,
            selection.entries,
            options.baseline !== false,
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
