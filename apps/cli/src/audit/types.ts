/**
 * Shared type shapes for PineGuard audit rules and their findings.
 *
 * This file is deliberately **type declarations only** — no logic, no
 * runtime code, no side-effecting imports. It exists so the test harness
 * (`test/support/runRule.ts`, plan P1.4) and the rule engine (plan P1.5:
 * `src/audit/engine.ts` + `src/audit/catalog.ts`) agree on the same `Rule` /
 * `Finding` shape from day one, instead of P1.5 reconciling a shape invented
 * independently after 14 rule-fixture tests already exist against it.
 *
 * P1.5 is expected to *extend* this file (e.g. wrap `Rule` with catalog
 * metadata — slug/legacyId/scope/gate/description, per plan §4.1
 * `catalog.ts` — and add fields to `RuleContext`). Extensions should be
 * additive so rules written against today's shape keep working unchanged.
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
     * §4.4) to distinguish already-known debt from a new regression.
     * Typically derived from rule + file + a normalised message.
     */
    key: string;
}

/**
 * The context a rule runs against.
 *
 * The test harness (`test/support/runRule.ts`) only ever populates
 * `rootDir`, pointed at a fixture directory instead of the real repo root.
 * The real engine (plan P1.5) extends this with the shared, precomputed
 * context described in plan §7.2 — tracked file listings, the C# parse
 * cache, `vocabulary.json` data, `config/exceptions.json`, and the
 * baseline — as additional (optional, unless a rule opts in) fields.
 */
export interface RuleContext {
    /** Absolute path to the root of the tree this run should scan. */
    rootDir: string;
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
