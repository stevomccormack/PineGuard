import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import "../../src/audit/rules/must-collisions.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for the `must-collisions` rule (plan
 * docs/ai/plans/audit-cli-rebuild.md §4.3, §8, §2.2, P2.5) — the
 * overload-collision half of the legacy tool's Rule01. See
 * `src/audit/rules/must-collisions.ts` for the full heuristic write-up.
 */

const rule = getRule("must-collisions");
if (!rule) {
    throw new Error(
        "must-collisions did not register itself — check the registerRule() call in src/audit/rules/must-collisions.ts",
    );
}

describe("must-collisions", () => {
    it("is registered under both its slug and the shared legacy id Rule01", () => {
        expect(rule.slug).toBe("must-collisions");
        expect(rule.legacyId).toBe("Rule01");
        expect(rule.scope).toBe("library");
        expect(rule.gate).toBe(false);
    });

    it("passes on overloads that are safely distinguishable by reachability or by null-acceptance", async () => {
        await expectValid(rule, "must-collisions");
    });

    it("fails on two single-argument-reachable overloads whose first parameter types both accept a null literal", async () => {
        const findings = await expectInvalid(rule, "must-collisions");

        expect(findings).toHaveLength(1);
        const [finding] = findings;
        expect(finding?.rule).toBe("must-collisions");
        expect(finding?.message).toContain("Must.Be.Foo");
        expect(finding?.message).toContain("string?");
        expect(finding?.message).toContain("int?");
        expect(finding?.message).toContain("ambiguous");
        expect(finding?.key).toMatch(
            /^must-collisions:MustFixtureClauses\.cs:/,
        );
        expect(finding?.key).not.toMatch(/:\d+\s*\(/); // no line numbers baked into the key
    });

    it("boundary: an overload requiring a second argument is NOT flagged, even sharing a first-parameter type with a single-argument overload", async () => {
        const findings = await runBoundary(rule, "must-collisions");
        const messages = findings.map((finding) => finding.message);

        expect(messages.some((message) => message.includes("Between"))).toBe(
            false,
        );
    });

    it("boundary: a generic overload whose first parameter depends on its own type parameter is NOT flagged alongside a concrete overload", async () => {
        const findings = await runBoundary(rule, "must-collisions");
        const messages = findings.map((finding) => finding.message);

        expect(messages.some((message) => message.includes("Custom"))).toBe(
            false,
        );
    });

    it("boundary: two same-shape overloads of an unrecognised custom type name are NOT flagged (the real DateOnlyRange-family false positive this rule was tuned against)", async () => {
        const findings = await runBoundary(rule, "must-collisions");
        const messages = findings.map((finding) => finding.message);

        expect(
            messages.some((message) => message.includes("Chronological")),
        ).toBe(false);
    });

    it("boundary fixtures produce zero findings overall", async () => {
        const findings = await runBoundary(rule, "must-collisions");
        expect(findings).toHaveLength(0);
    });
});
