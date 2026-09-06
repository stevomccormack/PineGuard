import type { Rule } from "./types.js";

/**
 * The audit rule catalog: a single, process-wide registry every rule module
 * registers itself into, plus lookup helpers the engine (`engine.ts`) and
 * the `audit` command (`src/commands/audit.ts`) use to resolve a run.
 *
 * ## Adding a new rule (read this first if you are a P2 rule-implementer agent)
 *
 * 1. Create `src/audit/rules/<slug>.ts`. It exports nothing required by the
 *    catalog — it just calls {@link registerRule} once, at module load time,
 *    for its side effect:
 *
 *    ```ts
 *    // src/audit/rules/must-usage.ts
 *    import { registerRule } from "../catalog.js";
 *    import type { Finding, RuleContext } from "../types.js";
 *
 *    registerRule({
 *        slug: "must-usage",
 *        legacyId: "Rule03", // omit if the rule has no legacy PowerShell-tool id
 *        scope: "library", // "library" | "testing" | "docs"
 *        gate: false, // true only for rules that block merges today
 *        description: "Every Must clause is invoked from Guard, Fluent, or DataAnnotations.",
 *        run(ctx: RuleContext): Finding[] {
 *            // ... real logic; see src/audit/parsing/csharp.ts and repo.ts ...
 *            return [];
 *        },
 *    });
 *    ```
 *
 * 2. Add one line to the barrel `src/audit/rules/index.ts`:
 *    `import "./must-usage.js";` — this is what actually triggers step 1's
 *    `registerRule` call for a real `pineguard audit` run (the barrel is
 *    imported once, for its side effects, by `src/commands/audit.ts`). A
 *    rule file that exists but is never imported from the barrel silently
 *    never registers — if `--list` doesn't show your rule, check this first.
 *
 * 3. Add `test/rules/<slug>.test.ts` plus
 *    `test/fixtures/<slug>/{valid,invalid}/` (and `boundary/` when
 *    meaningful) per the VIBE convention in `test/README.md` and
 *    `test/support/runRule.ts`.
 *
 * `registerRule` throws immediately on a duplicate `slug` or a `legacyId`
 * already claimed by a different slug — a collision is a bug in the rule
 * module (or in the barrel importing the same file twice), not something to
 * paper over silently. This subsumes what the old PowerShell tool's Rule09
 * ("catalog self-consistency") checked at runtime; here it is a load-time
 * invariant instead (see `test/audit/catalog.test.ts`).
 */

/** The three audit scopes rules are grouped into (plan §4.1/§4.3). */
export const RULE_SCOPES = ["library", "testing", "docs"] as const;

/** A rule's scope: which part of the repo it audits. */
export type RuleScope = (typeof RULE_SCOPES)[number];

/** Narrows an arbitrary string (e.g. a raw `--scope` CLI argument) to a {@link RuleScope}. */
export function isRuleScope(value: string): value is RuleScope {
    return (RULE_SCOPES as readonly string[]).includes(value);
}

/**
 * One registered audit rule: a {@link Rule} (slug + `run`) plus the catalog
 * metadata every rule needs (plan §4.1 `catalog.ts`, §7.1).
 */
export interface CatalogEntry extends Rule {
    /** The old PowerShell tool's numeric id (e.g. `"Rule03"`), kept as an alias so existing Brain citations keep resolving. Omit for a rule with no legacy counterpart. */
    legacyId?: string;
    /** Which part of the repo this rule audits. */
    scope: RuleScope;
    /** `true` for a merge-blocking rule (`pineguard audit --gate` includes it; today's CI gate). */
    gate: boolean;
    /** One-line, human-readable summary shown by `pineguard audit --list`. */
    description: string;
}

/** Thrown by {@link registerRule} on a duplicate `slug` or `legacyId`. */
export class DuplicateRuleError extends Error {
    public constructor(message: string) {
        super(message);
        this.name = "DuplicateRuleError";
    }
}

const bySlug = new Map<string, CatalogEntry>();
const slugByLegacyId = new Map<string, string>();

/**
 * Registers `entry` into the catalog. Throws {@link DuplicateRuleError} if
 * `entry.slug` is already registered, or if `entry.legacyId` is already
 * registered under a *different* slug.
 */
export function registerRule(entry: CatalogEntry): void {
    if (bySlug.has(entry.slug)) {
        throw new DuplicateRuleError(
            `a rule with slug "${entry.slug}" is already registered`,
        );
    }

    if (entry.legacyId !== undefined) {
        const existingSlug = slugByLegacyId.get(entry.legacyId);
        if (existingSlug !== undefined) {
            throw new DuplicateRuleError(
                `legacy id "${entry.legacyId}" is already registered to rule "${existingSlug}" ` +
                    `(cannot also register it to "${entry.slug}")`,
            );
        }
    }

    bySlug.set(entry.slug, entry);
    if (entry.legacyId !== undefined) {
        slugByLegacyId.set(entry.legacyId, entry.slug);
    }
}

/** Every registered rule, sorted by slug (stable, deterministic `--list` / `all` order). */
export function getAllRules(): CatalogEntry[] {
    return [...bySlug.values()].sort((a, b) => a.slug.localeCompare(b.slug));
}

/** Looks up a rule by its slug or its legacy `RuleNN` id. Returns `undefined` if neither matches. */
export function getRule(slugOrLegacyId: string): CatalogEntry | undefined {
    const bySlugMatch = bySlug.get(slugOrLegacyId);
    if (bySlugMatch !== undefined) {
        return bySlugMatch;
    }

    const slug = slugByLegacyId.get(slugOrLegacyId);
    return slug === undefined ? undefined : bySlug.get(slug);
}

/**
 * Clears every registered rule. Test isolation only — a real `pineguard`
 * process never calls this; the catalog is meant to accumulate every rule
 * module's registration for the lifetime of the process.
 */
export function clearCatalog(): void {
    bySlug.clear();
    slugByLegacyId.clear();
}
