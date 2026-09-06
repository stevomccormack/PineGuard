import { describe, expect, it } from "vitest";

import { testStructureRule } from "../../src/audit/rules/test-structure.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

const SLUG = "test-structure";

/**
 * `test-structure` (legacy `Rule51`) — v11 §4-5 TestData/Tests structural
 * conformance. See `src/audit/rules/test-structure.ts` for the full rule
 * doc comment (what it checks and why it inverts the old Rule51's logic).
 */
describe("test-structure", () => {
    it("produces zero findings against a real, conforming TestData/Tests pair (NullRulesTests.cs / NullRulesTestData.cs, copied verbatim from tests/PineGuard.Core.UnitTests/Rules/)", async () => {
        await expectValid(testStructureRule, SLUG);
    });

    it("flags a nested `public static class` inside the Tests file (flatness violation, v11 §5.1)", async () => {
        const findings = await expectInvalid(testStructureRule, SLUG);
        expect(
            findings.some((f) =>
                /nests 'public static class NotAllowedHere'/.test(f.message),
            ),
        ).toBe(true);
    });

    it("flags an empty `=> [];` dataset (v11 §4.1)", async () => {
        const findings = await expectInvalid(testStructureRule, SLUG);
        expect(
            findings.some((f) =>
                /declares an empty dataset 'ValidCases => \[\];'/.test(
                    f.message,
                ),
            ),
        ).toBe(true);
    });

    it("flags InvalidCases declared before ValidCases within an Operation Group (v11 §4.4)", async () => {
        const findings = await expectInvalid(testStructureRule, SLUG);
        expect(
            findings.some((f) =>
                /declares 'InvalidCases' before 'ValidCases'/.test(f.message),
            ),
        ).toBe(true);
    });

    it("boundary: a single-Cases-rollup Operation Group and a fully-split Valid/Edge/Invalid group are both valid — neither is falsely flagged (v11 §4.1)", async () => {
        const findings = await runBoundary(testStructureRule, SLUG);
        expect(findings).toEqual([]);
    });
});
