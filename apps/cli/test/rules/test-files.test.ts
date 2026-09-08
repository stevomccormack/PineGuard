import { describe, expect, it } from "vitest";

import { applyExceptions } from "../../src/audit/engine.js";
import { testFilesRule } from "../../src/audit/rules/test-files.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * `test-files` (legacy Rule50) — the exact-parity port of
 * `tools/audit-cli/rules/Test-Rule50-UnitTestFileStructureNormalization.ps1`.
 * See the header comment in `src/audit/rules/test-files.ts` for the ported
 * logic; this is the currently-enforced CI gate, so both independent checks
 * (pairing, Theory-only) must be proven able to fail on their own — not just
 * "some finding came back".
 */
describe("test-files (Rule50)", () => {
    it("passes on a conforming Tests.cs/TestData.cs pair using [Theory] only", async () => {
        await expectValid(testFilesRule, "test-files");
    });

    it("fails distinctly on an orphaned Tests.cs AND on a [Fact]-using paired Tests.cs", async () => {
        const findings = await expectInvalid(testFilesRule, "test-files");

        const missingTestData = findings.filter((f) =>
            f.message.includes("[MissingTestData]"),
        );
        const factNotAllowed = findings.filter((f) =>
            f.message.includes("[FactNotAllowed]"),
        );

        // Violation (a): BarTests.cs has no sibling BarTestData.cs.
        expect(missingTestData).toHaveLength(1);
        expect(missingTestData[0]?.file).toBe(
            "tests/PineGuard.Fake.UnitTests/BarTests.cs",
        );

        // Violation (b): BazTests.cs is properly paired with BazTestData.cs
        // but uses [Fact] instead of [Theory].
        expect(factNotAllowed).toHaveLength(1);
        expect(factNotAllowed[0]?.file).toBe(
            "tests/PineGuard.Fake.UnitTests/BazTests.cs",
        );

        // The two checks are independent: BazTests.cs (paired) must not also
        // report [MissingTestData], and BarTests.cs (Theory-only) must not
        // also report [FactNotAllowed].
        expect(
            missingTestData.some((f) => f.file.endsWith("BazTests.cs")),
        ).toBe(false);
        expect(factNotAllowed.some((f) => f.file.endsWith("BarTests.cs"))).toBe(
            false,
        );
    });

    describe("boundary", () => {
        it("still flags a legitimately-allowlisted orphan when run directly (the rule itself is exception-agnostic)", async () => {
            const findings = await runBoundary(testFilesRule, "test-files");
            const allowlisted = findings.filter((f) =>
                f.file.endsWith("AllowlistedTests.cs"),
            );
            expect(allowlisted).toHaveLength(1);
            expect(allowlisted[0]?.message).toContain("[MissingTestData]");
        });

        it("the engine's exceptions mechanism suppresses that same finding, mirroring the legacy AllowMissingTestData allowlist", async () => {
            const findings = await runBoundary(testFilesRule, "test-files");
            const filtered = applyExceptions(findings, {
                "test-files": [
                    "tests/PineGuard.Fake.UnitTests/AllowlistedTests.cs",
                ],
            });
            expect(
                filtered.some((f) => f.file.endsWith("AllowlistedTests.cs")),
            ).toBe(false);
        });

        it("flags the verbose [FactAttribute] alias (P4.3 finding #1 — widened, no longer a blind spot)", async () => {
            const findings = await runBoundary(testFilesRule, "test-files");
            const flagged = findings.filter((f) =>
                f.file.endsWith("VerboseFactAliasTests.cs"),
            );
            expect(flagged).toHaveLength(1);
            expect(flagged[0]?.message).toContain("[FactNotAllowed]");
        });

        it('still flags the parenthesized [Fact(DisplayName = "...")] form', async () => {
            const findings = await runBoundary(testFilesRule, "test-files");
            const flagged = findings.filter((f) =>
                f.file.endsWith("ParameterizedFactTests.cs"),
            );
            expect(flagged).toHaveLength(1);
            expect(flagged[0]?.message).toContain("[FactNotAllowed]");
        });

        it("flags [Fact, Trait(...)] in both attribute orderings (P4.3 finding #1 — widened, no longer a blind spot)", async () => {
            const findings = await runBoundary(testFilesRule, "test-files");
            const flagged = findings.filter((f) =>
                f.file.endsWith("MultiAttributeFactTests.cs"),
            );
            expect(flagged).toHaveLength(1);
            expect(flagged[0]?.message).toContain("[FactNotAllowed]");
        });
    });
});
