import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import "../../src/audit/rules/layer-parity.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for the `layer-parity` rule (plan
 * docs/ai/plans/audit-cli-rebuild.md §4.3, §8, §2.2, P2.3) — the from-source
 * rebuild of the legacy tool's `Rule06`. See
 * `src/audit/rules/layer-parity.ts` for the full write-up of how concepts
 * are extracted per layer and normalised via `vocabulary.json`.
 */

const rule = getRule("layer-parity");
if (!rule) {
    throw new Error(
        "layer-parity did not register itself — check the registerRule() call in src/audit/rules/layer-parity.ts",
    );
}

describe("layer-parity", () => {
    it("is registered under its slug and the legacy id Rule06", () => {
        expect(rule.slug).toBe("layer-parity");
        expect(rule.legacyId).toBe("Rule06");
        expect(rule.scope).toBe("library");
        expect(rule.gate).toBe(false);
    });

    it("passes when all four layers implement the same concept", async () => {
        await expectValid(rule, "layer-parity");
    });

    it("fails when Guard and DataAnnotations both never wire up a concept Must implements", async () => {
        const findings = await expectInvalid(rule, "layer-parity");

        expect(findings).toHaveLength(2);
        const layersNamed = findings.map((finding) => finding.file).sort();
        expect(layersNamed).toEqual(
            [
                "src/PineGuard.GuardClauses",
                "src/PineGuard.DataAnnotations",
            ].sort(),
        );

        for (const finding of findings) {
            expect(finding.rule).toBe("layer-parity");
            expect(finding.message).toContain("'NullOrEmpty'");
            expect(finding.message).toContain("Must");
        }
    });

    it("boundary: alias- and prefix-stripped names resolve to the same concept, but a truly unimplemented concept still flags", async () => {
        const findings = await runBoundary(rule, "layer-parity");

        // "NullOrEmpty"/"NotBlank" (alias) and "Null"/"NotNull" (stripPrefixes)
        // both resolve across Must <-> Guard via this fixture's own
        // vocabulary.json, so neither produces a finding.
        expect(findings).toHaveLength(1);

        const [finding] = findings;
        expect(finding?.message).toContain("'Extra'");
        expect(finding?.message).toContain("Guard");
        expect(finding?.file).toBe("src/PineGuard.GuardClauses");

        const messages = findings.map((f) => f.message);
        expect(
            messages.some((message) => message.includes("'NullOrEmpty'")),
        ).toBe(false);
        expect(messages.some((message) => message.includes("'Null'"))).toBe(
            false,
        );
    });
});
