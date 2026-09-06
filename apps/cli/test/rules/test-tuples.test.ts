import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import "../../src/audit/rules/test-tuples.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for the `test-tuples` rule (plan docs/ai/plans/audit-cli-rebuild.md
 * §4.3, §8, §2.4, P2.14) — the AST-based rebuild of the legacy tool's
 * Rule54. See `src/audit/rules/test-tuples.ts` for the full write-up of the
 * old regex bug this fixes and the best-effort source-method resolution.
 */

const rule = getRule("test-tuples");
if (!rule) {
    throw new Error(
        "test-tuples did not register itself — check the registerRule() call in src/audit/rules/test-tuples.ts",
    );
}

describe("test-tuples", () => {
    it("is registered under its slug and the legacy id Rule54", () => {
        expect(rule.slug).toBe("test-tuples");
        expect(rule.legacyId).toBe("Rule54");
        expect(rule.scope).toBe("testing");
        expect(rule.gate).toBe(false);
    });

    it("passes on a Value tuple whose camelCase elements match the real source parameter names, and never misidentifies an ordinary record's parameter list as a tuple", async () => {
        await expectValid(rule, "test-tuples");
    });

    it("fails on a Value tuple whose elements are PascalCase (the mandatory 'can it fail' fixture)", async () => {
        const findings = await expectInvalid(rule, "test-tuples");

        // Three PascalCase elements (Value, Min, Max); no paired src/ tree in
        // this fixture, so only the camelCase check can fire — one finding
        // per element, none from the (unresolvable) exact-name check.
        expect(findings).toHaveLength(3);
        expect(findings.every((f) => f.rule === "test-tuples")).toBe(true);
        expect(findings.every((f) => f.message.includes("camelCase"))).toBe(
            true,
        );
        expect(findings.some((f) => f.message.includes("'Value'"))).toBe(true);
        expect(findings.some((f) => f.message.includes("'Min'"))).toBe(true);
        expect(findings.some((f) => f.message.includes("'Max'"))).toBe(true);
    });

    it("boundary: a camelCase-but-renamed/abbreviated element against a resolvable source method fails ONLY the exact-name check, not camelCase too", async () => {
        const findings = await runBoundary(rule, "test-tuples");
        const forVal = findings.filter((f) => f.message.includes("'val'"));

        expect(forVal).toHaveLength(1);
        expect(forVal[0]?.message).toContain("does not match the exact");
        expect(forVal[0]?.message).toContain("'value'");
        expect(forVal[0]?.message).not.toContain("camelCase");
    });

    it("boundary: a fully camelCase tuple with no resolvable source method produces zero findings (best-effort skip, not a false positive)", async () => {
        const findings = await runBoundary(rule, "test-tuples");
        const forNoSuchMethod = findings.filter((f) =>
            f.file.includes("FakeClausesTestData"),
        );

        // Only boundary A's single "val" finding should exist; boundary B
        // (NoSuchMethod, fully camelCase) must contribute none.
        expect(forNoSuchMethod).toHaveLength(1);
    });

    it("boundary fixtures produce exactly the one expected finding overall", async () => {
        const findings = await runBoundary(rule, "test-tuples");
        expect(findings).toHaveLength(1);
    });
});
