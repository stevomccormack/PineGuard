import { existsSync, statSync } from "node:fs";

import type { Finding, Rule, RuleContext } from "../../src/audit/types.js";

/**
 * Test harness for PineGuard audit rules (plan P1.4).
 *
 * ## Fixture convention
 *
 * Every rule slug gets a fixture pair under `test/fixtures/<slug>/`:
 *
 * ```
 * test/fixtures/<slug>/pass/...   minimal, realistic source tree the rule must NOT flag
 * test/fixtures/<slug>/fail/...   minimal, realistic source tree the rule MUST flag (>=1 finding)
 * ```
 *
 * Keep fixtures minimal (only what's needed to trip, or not trip, the rule)
 * and realistic (valid C#/Markdown/JSON shaped like the repo's own code, not
 * contrived syntax the rule would never see in production). `pass/` must be
 * a case the rule genuinely accepts — an empty directory proves nothing; it
 * can't distinguish "the rule correctly ignores this" from "the rule never
 * ran".
 *
 * Pair every fixture with a `test/rules/<slug>.test.ts` that:
 *   1. asserts `runRuleOnFixture(rule, ".../pass")` returns `[]`
 *   2. asserts `expectRuleCanFail(rule, ".../fail")` does not throw
 *
 * `expectRuleCanFail` is **mandatory**, not optional, for every rule's
 * fail-fixture assertion. Do not replace it with a bare
 * `expect(findings.length).toBeGreaterThan(0)` — that reads the same at a
 * glance, but (a) produces a generic assertion failure instead of the
 * explicit "this is the old tool's exact bug" message below, and (b) is easy
 * to accidentally weaken (e.g. to `.toBeGreaterThanOrEqual(0)`, which always
 * passes) without anyone noticing in review.
 *
 * See docs/ai/plans/audit-cli-rebuild.md §2.2-2.3 (why this exists), §4.1
 * (layout), §9.2 P1.4/P4.2 (this harness and the later "can it fail" audit
 * it exists to make easy), and `apps/cli/test/README.md` for the full
 * convention writeup.
 */

/** Runs `rule` against every file under `fixtureDir` and returns its findings. */
export async function runRuleOnFixture(
    rule: Rule,
    fixtureDir: string,
): Promise<Finding[]> {
    assertFixtureDirExists(rule, fixtureDir);
    const ctx: RuleContext = { rootDir: fixtureDir };
    return await rule.run(ctx);
}

/**
 * Runs `rule` against a fixture directory and throws a clear, descriptive
 * error if it produces zero findings.
 *
 * This is the direct fix for the old `tools/audit-cli` tool's exact failure
 * mode: Rules 03, 04, 05 and 07 all "passed" for months because a quoting
 * bug silently produced zero findings, forever, and nothing ever asserted
 * otherwise. Call this with the rule's `fail/` fixture in every rule test.
 *
 * @returns the findings, so callers can make further assertions on them.
 * @throws if `findings.length === 0`, or if `fixtureDir` does not exist.
 */
export async function expectRuleCanFail(
    rule: Rule,
    fixtureDir: string,
): Promise<Finding[]> {
    const findings = await runRuleOnFixture(rule, fixtureDir);
    if (findings.length === 0) {
        throw new Error(
            `rule '${rule.slug}' produced zero findings against its own fail ` +
                `fixture (${fixtureDir}) — this is exactly how the old audit tool's ` +
                `Rules 03/04/05/07 went silently vacuous for months. Check that the ` +
                `fixture actually violates the rule, or that the rule itself has a bug.`,
        );
    }
    return findings;
}

function assertFixtureDirExists(rule: Rule, fixtureDir: string): void {
    if (!existsSync(fixtureDir) || !statSync(fixtureDir).isDirectory()) {
        throw new Error(
            `rule '${rule.slug}': fixture directory does not exist: '${fixtureDir}'. ` +
                `A missing or mistyped fixture path also produces zero findings, ` +
                `which can look identical to a vacuous rule at a glance — check the ` +
                `path before suspecting the rule.`,
        );
    }
}
