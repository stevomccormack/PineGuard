import { existsSync, statSync } from "node:fs";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

import type { Finding, Rule, RuleContext } from "../../src/audit/types.js";

/**
 * Test harness for PineGuard audit rules (plan P1.4, fixture convention
 * corrected to VIBE by plan §4.6).
 *
 * ## Fixture convention — VIBE (Valid / Invalid / Boundary-Edge)
 *
 * Every rule slug gets a fixture triad under `test/fixtures/<slug>/`:
 *
 * ```
 * test/fixtures/<slug>/valid/      minimal, realistic source tree the rule must NOT flag (always [])
 * test/fixtures/<slug>/invalid/    minimal, realistic source tree the rule MUST flag (always >=1 finding)
 * test/fixtures/<slug>/boundary/   edge-condition source trees that probe the rule's own decision
 *                                  boundary; each case is asserted individually — a boundary case
 *                                  can legitimately turn out still-valid or still-invalid
 * ```
 *
 * `valid/` and `invalid/` are mandatory for every rule (mirrors the repo's
 * C# test spec `docs/ai/specs/testing/unit-test.md` v11 §4.1/§4.4:
 * `ValidCases`/`EdgeCases`/`InvalidCases`). `boundary/` is optional — add it
 * only when the rule actually has a meaningful edge to probe; don't
 * scaffold an empty placeholder directory (plan §4.6, §10.3's "no empty
 * dataset" policy).
 *
 * Keep fixtures minimal (only what's needed to trip, or not trip, the rule)
 * and realistic (valid C#/Markdown/JSON shaped like the repo's own code, not
 * contrived syntax the rule would never see in production). `valid/` must
 * be a case the rule genuinely accepts — an empty directory proves nothing;
 * it can't distinguish "the rule correctly ignores this" from "the rule
 * never ran".
 *
 * Pair every fixture with a `test/rules/<slug>.test.ts` that:
 *   1. asserts `expectValid(rule, slug)` resolves without throwing
 *   2. asserts `expectInvalid(rule, slug)` resolves without throwing
 *   3. if `boundary/` exists, asserts on `runBoundary(rule, slug)`'s
 *      findings case by case, for whatever each boundary fixture should
 *      specifically produce
 *
 * `expectInvalid` is **mandatory**, not optional, for every rule's
 * invalid-fixture assertion. Do not replace it with a bare
 * `expect(findings.length).toBeGreaterThan(0)` — that reads the same at a
 * glance, but (a) produces a generic assertion failure instead of the
 * explicit "this is the old tool's exact bug" message below, and (b) is easy
 * to accidentally weaken (e.g. to `.toBeGreaterThanOrEqual(0)`, which always
 * passes) without anyone noticing in review.
 *
 * See docs/ai/plans/audit-cli-rebuild.md §2.2-2.3 (why this exists), §4.1
 * (layout), §4.6 (the VIBE fixture convention this harness implements),
 * §9.2 P1.4/P4.2 (this harness and the later "can it fail" audit it exists
 * to make easy), and `apps/cli/test/README.md` for the full convention
 * writeup.
 */

const HERE = dirname(fileURLToPath(import.meta.url));
const FIXTURES_ROOT = join(HERE, "..", "fixtures");

type FixtureKind = "valid" | "invalid" | "boundary";

function fixtureDirFor(slug: string, kind: FixtureKind): string {
    return join(FIXTURES_ROOT, slug, kind);
}

/**
 * Runs `rule` against every file under `fixtureDir` and returns its
 * findings. This is the generic, lower-level building block the three
 * `expectValid`/`expectInvalid`/`runBoundary` helpers are built on; kept
 * exported because it's still useful directly for an ad-hoc fixture path
 * (e.g. asserting on one specific file inside `boundary/` in isolation).
 */
export async function runRuleOnFixture(
    rule: Rule,
    fixtureDir: string,
): Promise<Finding[]> {
    assertFixtureDirExists(rule, fixtureDir);
    const ctx: RuleContext = { rootDir: fixtureDir };
    return await rule.run(ctx);
}

/**
 * Runs `rule` against its `valid/` fixture (`test/fixtures/<slug>/valid/`)
 * and asserts it produces zero findings.
 *
 * @throws if the fixture directory does not exist, or if the rule reports
 * any finding at all — a `valid/` fixture that trips the rule means either
 * the fixture doesn't actually belong in `valid/`, or the rule has a
 * false-positive bug.
 */
export async function expectValid(rule: Rule, slug: string): Promise<void> {
    const fixtureDir = fixtureDirFor(slug, "valid");
    const findings = await runRuleOnFixture(rule, fixtureDir);
    if (findings.length > 0) {
        throw new Error(
            `rule '${rule.slug}' produced ${findings.length} finding(s) against its own ` +
                `valid fixture (${fixtureDir}) — a valid/ fixture must be a genuine, ` +
                `unflagged accept case. Check that the fixture doesn't actually violate ` +
                `the rule, or that the rule doesn't have a false-positive bug. ` +
                `Findings: ${JSON.stringify(findings)}`,
        );
    }
}

/**
 * Runs `rule` against its `invalid/` fixture (`test/fixtures/<slug>/invalid/`)
 * and asserts it produces at least one finding.
 *
 * This is the renamed/refocused form of P1.4's original `expectRuleCanFail`,
 * corrected from the plain `pass`/`fail` convention to VIBE (plan §4.6) —
 * same check, same message, new directory name.
 *
 * This is the direct fix for the old `tools/audit-cli` tool's exact failure
 * mode: Rules 03, 04, 05 and 07 all "passed" for months because a quoting
 * bug silently produced zero findings, forever, and nothing ever asserted
 * otherwise. Call this with the rule's `invalid/` fixture in every rule test.
 *
 * @returns the findings, so callers can make further assertions on them.
 * @throws if `findings.length === 0`, or if the fixture directory does not
 * exist.
 */
export async function expectInvalid(
    rule: Rule,
    slug: string,
): Promise<Finding[]> {
    const fixtureDir = fixtureDirFor(slug, "invalid");
    const findings = await runRuleOnFixture(rule, fixtureDir);
    if (findings.length === 0) {
        throw new Error(
            `rule '${rule.slug}' produced zero findings against its own invalid ` +
                `fixture (${fixtureDir}) — this is exactly how the old audit tool's ` +
                `Rules 03/04/05/07 went silently vacuous for months. Check that the ` +
                `fixture actually violates the rule, or that the rule itself has a bug.`,
        );
    }
    return findings;
}

/**
 * Runs `rule` against its `boundary/` fixture (`test/fixtures/<slug>/boundary/`)
 * and returns the raw findings for the calling test to assert against
 * explicitly, case by case.
 *
 * Unlike `expectValid`/`expectInvalid`, this makes no built-in pass/fail
 * assumption: per plan §4.6, a boundary case can legitimately turn out
 * still-valid (no finding) or still-invalid (a finding) — that's the point
 * of probing a decision boundary rather than a clear accept/reject case.
 *
 * @throws if the fixture directory does not exist.
 */
export async function runBoundary(
    rule: Rule,
    slug: string,
): Promise<Finding[]> {
    return await runRuleOnFixture(rule, fixtureDirFor(slug, "boundary"));
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
