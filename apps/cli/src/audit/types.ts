import type { ParsedFile } from "./parsing/csharp.js";

/**
 * Shared type shapes for PineGuard audit rules and their findings.
 *
 * This file is deliberately **type declarations only** — no logic, no
 * runtime code, no side-effecting imports (the `ParsedFile` import below is
 * `import type`, so it is erased at compile time and never runs the
 * `parsing/csharp.ts` module). It exists so the test harness
 * (`test/support/runRule.ts`, plan P1.4) and the rule engine (plan P1.5:
 * `src/audit/engine.ts` + `src/audit/catalog.ts`) agree on the same `Rule` /
 * `Finding` shape from day one, instead of P1.5 reconciling a shape invented
 * independently after 14 rule-fixture tests already exist against it.
 *
 * P1.5 (landed) extends this file additively: `catalog.ts` wraps `Rule` with
 * catalog metadata (`CatalogEntry`: slug/legacyId/scope/gate/description —
 * see that file, not here, since the wrapping is a catalog concern), and
 * `RuleContext` below gained optional `trackedFiles`/`parseFile`/
 * `vocabulary`/`exceptions` fields. Every field P1.4 depended on
 * (`rootDir`, plain `{ rootDir }` context construction) is untouched — the
 * new fields are additive and optional, so `test/support/runRule.ts`'s
 * `{ rootDir: fixtureDir }` literal still type-checks unchanged.
 *
 * See docs/ai/plans/audit-cli-rebuild.md §4.1, §4.2, §7.
 */

/**
 * One violation reported by a rule.
 */
export interface Finding {
    /** The rule slug that produced this finding (e.g. `"must-usage"`). */
    rule: string;
    /** Repo-relative (or fixture-relative) path to the offending file. */
    file: string;
    /** 1-based line number, when the rule can pinpoint one. */
    line?: number;
    /** Human-readable description of the violation. */
    message: string;
    /**
     * Stable identity for this finding, used by the baseline ratchet (plan
     * §4.4, `applyBaseline` in `engine.ts`) to distinguish already-known debt
     * from a new regression. Typically derived from rule + file + a
     * normalised message.
     *
     * **Convention** (until a rule needs something else — the engine treats
     * `key` as an opaque string and never parses it, so a rule is free to
     * deviate if it has a good reason to, as long as it stays *stable* across
     * runs): `` `${rule}:${file}:${normalisedMessage}` ``, e.g.
     * `"must-usage:src/PineGuard.MustClauses/MustStringClauses.cs:Must.Be.NullOrWhiteSpace has no Guard caller"`.
     * This matches the pattern the P1.5 engine tests already use
     * (`test/audit/engine.test.ts`'s `findingFor` helper) and satisfies plan
     * §11's anti-regression requirement: two runs against unchanged input
     * must produce byte-identical keys (no timestamps, no line-number
     * churn if the rule can avoid it, no non-deterministic ordering baked
     * into the message), and two *different* violations — even in the same
     * file, even for the same rule — must produce different keys, so a new
     * finding in an already-baselined file still surfaces instead of being
     * masked by a coarser per-file suppression.
     */
    key: string;
}

/**
 * Parsed shape of `docs/ai/specs/language/vocabulary.json` (plan §4.1, §8).
 * The engine (`src/audit/engine.ts`) loads and parses this file once per
 * invocation and exposes it on `RuleContext.vocabulary`, so every rule reads
 * the same parsed object instead of each re-reading/re-parsing the file.
 * `concepts`/`opposites` are carried through unvalidated — D9 (plan §5) is
 * still open on whether they get wired up or removed; loading them here does
 * not decide that.
 */
export interface VocabularyConfig {
    readonly version: number;
    readonly stripPrefixes: readonly string[];
    readonly ignoreMethods: readonly string[];
    readonly aliases: Readonly<Record<string, string>>;
    readonly concepts: readonly unknown[];
    readonly opposites: readonly {
        readonly a: string;
        readonly b: string;
        readonly omitNegationsInParity?: boolean;
    }[];
}

/**
 * Parsed shape of `apps/cli/config/exceptions.json` (plan §4.1, §7
 * "Exceptions application"). Keyed by rule slug; each value is a list of
 * substrings. A finding is suppressed by the engine when one of its rule's
 * entries is a substring of either the finding's `file` or its `message`.
 * Deliberately simple — this is the load-and-apply mechanism P1.5 owes P2/P5,
 * not the final shape ported from `tools/audit-cli/test-audit-exceptions.json`
 * (that port is P5's job; today's file is still the placeholder `{}` from
 * P1.1).
 */
export type ExceptionsConfig = Readonly<Record<string, readonly string[]>>;

/**
 * Parsed shape of `apps/cli/config/baseline.json` (plan §4.4 "Baseline
 * ratchet"). Keyed by rule slug; each value is the list of `Finding.key`s
 * accepted as pre-existing debt for that rule. A rule with a clean baseline
 * (zero accepted findings) has **no entry at all** rather than an empty
 * array — `--update-baseline` (`engine.ts`'s `computeBaselineSnapshot`)
 * omits empty entries, and once a rule's entry is gone it is a hard gate
 * (plan §4.4: "when a rule's baseline entry count hits zero the entry is
 * deleted and the rule is a hard gate").
 *
 * The engine (`applyBaseline` in `engine.ts`) never inspects a key's
 * internal structure — matching is exact-string-equality only, on purpose:
 * a coarser per-file suppression would defeat the anti-regression property
 * plan §11 calls out ("a new finding in an already-failing file still
 * fails"). See `Finding.key`'s doc comment above for the key convention.
 */
export type BaselineConfig = Readonly<Record<string, readonly string[]>>;

/**
 * The context a rule runs against.
 *
 * The test harness (`test/support/runRule.ts`) only ever populates
 * `rootDir`, pointed at a fixture directory instead of the real repo root —
 * every field below is optional for exactly that reason: a rule that only
 * reads `rootDir` (or walks it directly) keeps working against that harness
 * unchanged. The real engine (`src/audit/engine.ts`'s `buildContext`)
 * populates every field on a real `pineguard audit` run: tracked file
 * listings (`trackedFiles`), a bound, already-cached C# parse function
 * (`parseFile` — rules may also import `parsing/csharp.ts`'s `parseFile`
 * directly if that's simpler; both resolve to the same cache),
 * `vocabulary.json` (`vocabulary`), and `config/exceptions.json`
 * (`exceptions`, though rules do not need to apply exceptions themselves —
 * the engine does that after `run()` returns, per rule slug).
 */
export interface RuleContext {
    /** Absolute path to the root of the tree this run should scan. */
    rootDir: string;
    /**
     * Every file `git` tracks under `rootDir`, forward-slash-normalised and
     * relative to it (see `repo.ts`'s `listTrackedFiles`). Absent in the
     * P1.4 fixture harness — a rule that needs this list against a fixture
     * tree should walk `rootDir` directly instead (see the `_demo-harness`
     * rule in `test/support/runRule.test.ts` for that pattern).
     */
    trackedFiles?: readonly string[];
    /** Bound to `parsing/csharp.ts`'s `parseFile`; parses (and caches) one C# file. */
    parseFile?: (filePath: string) => Promise<ParsedFile>;
    /** Parsed `docs/ai/specs/language/vocabulary.json`. */
    vocabulary?: VocabularyConfig;
    /** Parsed `apps/cli/config/exceptions.json`, keyed by rule slug. */
    exceptions?: ExceptionsConfig;
    /**
     * Parsed `apps/cli/config/baseline.json`, keyed by rule slug (plan §4.4).
     * Rules do not need to apply this themselves — same as `exceptions`, the
     * engine applies it (via `applyBaseline` in `engine.ts`) after `run()`
     * returns, per rule slug.
     */
    baseline?: BaselineConfig;
}

/**
 * A single audit rule.
 *
 * The full catalog entry (slug, legacyId, scope, gate, description — plan
 * §4.1 `catalog.ts`) wraps a `Rule`; this interface is only the part every
 * rule module and the test harness need to agree on.
 */
export interface Rule {
    /** Stable, kebab-case identifier (e.g. `"must-usage"`). Matches the catalog slug. */
    slug: string;
    /** Runs the rule against `ctx` and returns every finding (empty array = clean). */
    run(ctx: RuleContext): Finding[] | Promise<Finding[]>;
}
