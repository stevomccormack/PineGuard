import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import { applyExceptions } from "../../src/audit/engine.js";
import "../../src/audit/rules/nullability.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for the `nullability` rule (plan
 * docs/ai/plans/audit-cli-rebuild.md §4.3, §8, §2.2, P2.4) — the hybrid
 * nullability policy on a Must/Guard clause's primary parameter, replacing
 * the legacy tool's Rule07. See `src/audit/rules/nullability.ts` for the
 * full policy write-up and the historical escaping-bug note.
 */

const rule = getRule("nullability");
if (!rule) {
    throw new Error(
        "nullability did not register itself — check the registerRule() call in src/audit/rules/nullability.ts",
    );
}

describe("nullability", () => {
    it("is registered under its slug and the shared legacy id Rule07", () => {
        expect(rule.slug).toBe("nullability");
        expect(rule.legacyId).toBe("Rule07");
        expect(rule.scope).toBe("library");
        expect(rule.gate).toBe(false);
    });

    it("passes on Must/Guard clauses whose primary parameter follows the hybrid policy", async () => {
        await expectValid(rule, "nullability");
    });

    it(
        "fails on a non-nullable reference-type primary parameter and a nullable value-type " +
            "primary parameter — the exact shape the legacy Rule07 escaping bug could never detect",
        async () => {
            const findings = await expectInvalid(rule, "nullability");

            const notNullOrEmpty = findings.find((finding) =>
                finding.message.includes("NotNullOrEmpty"),
            );
            expect(notNullOrEmpty).toBeDefined();
            expect(notNullOrEmpty?.message).toContain("string");
            expect(notNullOrEmpty?.message).toContain("nullable");
            expect(notNullOrEmpty?.message).toContain("string?");

            const notEmpty = findings.find(
                (finding) =>
                    finding.message.includes("NotEmpty") &&
                    !finding.message.includes("NotNullOrEmpty"),
            );
            expect(notEmpty).toBeDefined();
            expect(notEmpty?.message).toContain("Guid?");
            expect(notEmpty?.message).toContain("non-nullable");

            // At least the two documented violations plus the LegacyRawValue
            // fixture reserved for the exemption test below.
            expect(findings.length).toBeGreaterThanOrEqual(3);
        },
    );

    it(
        "exemption: a method-name substring in config/exceptions.json's `nullability` entry " +
            "suppresses that method's finding via the engine's centralized applyExceptions " +
            "(plan §8 'Exemptions by method name'; this rule does not filter itself — see " +
            "src/audit/rules/nullability.ts's module doc comment)",
        async () => {
            const rawFindings = await expectInvalid(rule, "nullability");
            const legacyFinding = rawFindings.find((finding) =>
                finding.message.includes("LegacyRawValue"),
            );
            expect(legacyFinding).toBeDefined();

            const exempted = applyExceptions(rawFindings, {
                nullability: ["LegacyRawValue"],
            });

            expect(
                exempted.some((finding) =>
                    finding.message.includes("LegacyRawValue"),
                ),
            ).toBe(false);
            // Every other real violation is untouched by the exemption.
            expect(exempted.length).toBe(rawFindings.length - 1);
            expect(
                exempted.some((finding) =>
                    finding.message.includes("NotNullOrEmpty"),
                ),
            ).toBe(true);
        },
    );

    it("boundary: a struct-constrained generic primary parameter declared non-nullable is valid", async () => {
        const findings = await runBoundary(rule, "nullability");
        const messages = findings
            .filter((finding) => finding.file === "StructConstrainedValid.cs")
            .map((finding) => finding.message);
        expect(messages).toHaveLength(0);
    });

    it("boundary: a struct-constrained generic primary parameter declared nullable is invalid", async () => {
        const findings = await runBoundary(rule, "nullability");
        const structInvalid = findings.filter(
            (finding) => finding.file === "StructConstrainedInvalid.cs",
        );
        expect(structInvalid).toHaveLength(1);
        expect(structInvalid[0]?.message).toContain("struct");
        expect(structInvalid[0]?.message).toContain("non-nullable");
    });

    it("boundary: a class-constrained generic primary parameter declared nullable is valid", async () => {
        const findings = await runBoundary(rule, "nullability");
        const messages = findings
            .filter((finding) => finding.file === "ClassConstrained.cs")
            .map((finding) => finding.message);
        expect(messages).toHaveLength(0);
    });

    it("boundary: an unconstrained generic primary parameter declared nullable is valid", async () => {
        const findings = await runBoundary(rule, "nullability");
        const messages = findings
            .filter((finding) => finding.file === "UnconstrainedGeneric.cs")
            .map((finding) => finding.message);
        expect(messages).toHaveLength(0);
    });

    it("boundary fixtures produce exactly the one struct-constrained-but-nullable finding", async () => {
        const findings = await runBoundary(rule, "nullability");
        expect(findings).toHaveLength(1);
    });
});
