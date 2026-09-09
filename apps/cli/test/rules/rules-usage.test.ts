import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import "../../src/audit/rules/rules-usage.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for the `rules-usage` rule (plan
 * docs/ai/plans/audit-cli-rebuild.md §4.3, §2.2, P2.1) — the source-only
 * rebuild of the legacy tool's Rule02. See
 * `src/audit/rules/rules-usage.ts` for the full write-up of the calling
 * convention this rule assumes and why.
 */

const rule = getRule("rules-usage");
if (!rule) {
    throw new Error(
        "rules-usage did not register itself — check the registerRule() call in src/audit/rules/rules-usage.ts",
    );
}

describe("rules-usage", () => {
    it("is registered under both its slug and the legacy id Rule02", () => {
        expect(rule.slug).toBe("rules-usage");
        expect(rule.legacyId).toBe("Rule02");
        expect(rule.scope).toBe("library");
        expect(rule.gate).toBe(false);
    });

    it("passes when every Core Rules public static method is called from MustClauses", async () => {
        await expectValid(rule, "rules-usage");
    });

    it("fails when a Core Rules public static method has zero call sites in MustClauses", async () => {
        const findings = await expectInvalid(rule, "rules-usage");

        expect(findings).toHaveLength(1);
        const [finding] = findings;
        expect(finding?.rule).toBe("rules-usage");
        expect(finding?.message).toContain("FixtureRules.IsBaz");
        expect(finding?.message).toContain("no call site");
        expect(finding?.key).toBe("rules-usage:FixtureRules.IsBaz");
    });

    it("boundary: a nameof()-only reference does NOT count as a call site (still flagged)", async () => {
        const findings = await runBoundary(rule, "rules-usage");
        const keys = findings.map((finding) => finding.key);

        expect(keys).toContain("rules-usage:FixtureRules.IsBar");
    });

    it("boundary: a nested static helper class invoked via its full qualified chain DOES count as usage (not flagged)", async () => {
        const findings = await runBoundary(rule, "rules-usage");
        const keys = findings.map((finding) => finding.key);

        expect(keys).not.toContain("rules-usage:FixtureRules.Nested.IsQux");
        expect(keys).not.toContain("rules-usage:FixtureRules.IsFoo");
    });

    it("boundary fixture produces exactly one finding (only the nameof()-only method)", async () => {
        const findings = await runBoundary(rule, "rules-usage");
        expect(findings).toHaveLength(1);
    });
});
