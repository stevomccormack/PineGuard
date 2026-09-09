import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import "../../src/audit/rules/surface-parity.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for the `surface-parity` rule (plan
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3, §8, §2.4, P2.9) — the port of
 * the legacy PowerShell tool's Rule12 ("Agent <-> adapter-surface command
 * parity"). See `src/audit/rules/surface-parity.ts` for the full write-up of
 * what it parses and the two behaviour fixes over the legacy rule.
 *
 * The fixtures under `test/fixtures/surface-parity/` are a tiny, fully fake
 * `adapter-surfaces.md`-shaped policy doc — three fake surfaces
 * (`.fakeclaude/`, `.fakepi/`, `.fakecopilot/`), a fake agent roster, and a
 * fake §4 exceptions table with one agent-specific exemption ("Solo family")
 * and one Copilot-style subset policy ("FakeCopilot subset", families
 * `alpha`/`gamma`) — never the real repo's `docs/ai/meta/adapter-surfaces.md`
 * or `docs/ai/agents/`.
 */

const rule = getRule("surface-parity");
if (!rule) {
    throw new Error(
        "surface-parity did not register itself — check the registerRule() call in src/audit/rules/surface-parity.ts",
    );
}

describe("surface-parity", () => {
    it("is registered under its slug and the legacy id Rule12, in the docs scope, as a gate rule", () => {
        expect(rule.slug).toBe("surface-parity");
        expect(rule.legacyId).toBe("Rule12");
        expect(rule.scope).toBe("docs");
        expect(rule.gate).toBe(true);
    });

    it("passes when every agent is represented consistently across every surface and palette, per the declared exceptions", async () => {
        await expectValid(rule, "surface-parity");
    });

    it("fails on both a genuine parity gap and a missing parity-policy section", async () => {
        const findings = await expectInvalid(rule, "surface-parity");

        // Proof #1: the rule still catches a real parity gap — `great-gamma`
        // is present in docs/ai/agents/ and on two surfaces but missing from
        // `.fakepi/prompts/`, with no declared exception covering it. This
        // is the "can it still fail on an ordinary gap" check — the fix
        // must not have traded away the rule's actual job.
        const parityGapFinding = findings.find(
            (finding) =>
                finding.file === ".fakepi/prompts" &&
                finding.message.includes(
                    "Missing adapter for agent 'great-gamma'",
                ),
        );
        expect(
            parityGapFinding,
            `expected a "Missing adapter for agent 'great-gamma'" finding on .fakepi/prompts; got: ${JSON.stringify(findings, null, 2)}`,
        ).toBeDefined();

        // Proof #2: the loud-failure fix. The legacy Rule12 silently treated
        // a missing/malformed §4 exceptions section as "zero exceptions" and
        // carried on — this rule must instead report a real finding the
        // moment the section can't be found, rather than passing quietly.
        const missingSectionFinding = findings.find(
            (finding) =>
                finding.key === "surface-parity:exceptions-section-missing",
        );
        expect(
            missingSectionFinding,
            `expected a "surface-parity:exceptions-section-missing" finding; got: ${JSON.stringify(findings, null, 2)}`,
        ).toBeDefined();
        expect(missingSectionFinding?.message).toContain(
            "Missing parity-policy",
        );
    });

    it("boundary: the Copilot one-representative-per-family policy's own decision boundary (zero reps and two reps)", async () => {
        const findings = await runBoundary(rule, "surface-parity");

        // Every other surface and palette is deliberately kept fully
        // parity-consistent in this fixture, so the only findings should be
        // the two family-count boundary cases below.
        expect(findings).toHaveLength(2);

        const missingAlpha = findings.find(
            (finding) =>
                finding.key ===
                "surface-parity:.fakecopilot/prompts:copilot-family-missing:alpha",
        );
        expect(
            missingAlpha,
            "expected a missing-representative finding for family 'alpha' (zero present)",
        ).toBeDefined();
        expect(missingAlpha?.message).toContain(
            "no representative present for command family 'alpha'",
        );

        const ambiguousGamma = findings.find(
            (finding) =>
                finding.key ===
                "surface-parity:.fakecopilot/prompts:copilot-family-ambiguous:gamma",
        );
        expect(
            ambiguousGamma,
            "expected an ambiguous-representative finding for family 'gamma' (two present)",
        ).toBeDefined();
        expect(ambiguousGamma?.message).toContain(
            "2 representatives present for command family 'gamma'",
        );
        expect(ambiguousGamma?.message).toContain("great-gamma");
        expect(ambiguousGamma?.message).toContain("mega-gamma");
    });
});
