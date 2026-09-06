import { describe, expect, it } from "vitest";

import {
    buildGroupOrder,
    normalizeBaseKey,
    normalizeOperationName,
    orderingRule,
    tryStripNegativePrefix,
} from "../../src/audit/rules/ordering.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * `ordering` (legacy Rule08) tests. See
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3/§8/§9.2 P2.6 and
 * `src/audit/rules/ordering.ts`'s header comment for the two deliberate
 * behaviour changes from the legacy PowerShell/Roslyn tool this proves:
 * a missing sibling layer is now a finding (not a warning), and a Guard
 * method delegating to more than one distinct Must target is now an
 * `ambiguous-delegation` finding.
 */

describe("ordering rule (Rule08 port)", () => {
    it("passes on a conforming tree: Widget family, all five layers declare Empty/Duplicate in the same order", async () => {
        await expectValid(orderingRule, "ordering");
    });

    it("fails on a violating tree, proving both behaviour changes at once", async () => {
        const findings = await expectInvalid(orderingRule, "ordering");

        // Family Alpha: GuardAlphaClauses declares Duplicate-then-Empty, the
        // reverse of MustAlphaClauses' Empty-then-Duplicate — an
        // order-mismatch finding, isolated to the GuardClauses layer only
        // (Fluent/DataAnnotations/Rules for Alpha are kept in Must's order).
        const alphaFindings = findings.filter((finding) =>
            finding.message.includes("MustAlphaClauses"),
        );
        expect(alphaFindings).toHaveLength(1);
        expect(alphaFindings[0]?.message).toContain(
            "GuardClauses order mismatch",
        );
        expect(alphaFindings[0]?.message).toContain(
            "expected [Empty, Duplicate]",
        );
        expect(alphaFindings[0]?.message).toContain(
            "actual [Duplicate, Empty]",
        );

        // Family Bravo: GuardBravoClauses.Archived invokes both
        // Must.Be.NotStale and Must.Be.NotArchived — an ambiguous
        // delegation. This is the direct proof of behaviour change #2: the
        // old tool silently used the first match; this rule now also
        // reports the ambiguity as a finding.
        const bravoFindings = findings.filter((finding) =>
            finding.message.includes("GuardBravoClauses.Archived"),
        );
        expect(bravoFindings).toHaveLength(1);
        expect(bravoFindings[0]?.message).toContain(
            "ambiguous Must delegation",
        );
        expect(bravoFindings[0]?.message).toContain("NotStale, NotArchived");

        // Family Charlie: MustCharlieClauses has no GuardClauses,
        // FluentValidation, DataAnnotations, or Rules sibling anywhere in
        // the fixture tree. This is the direct proof of behaviour change
        // #1: the old tool only warned "Missing X sibling (skipped)" here
        // and stayed green; this rule now reports one missing-layer finding
        // per absent sibling.
        const charlieFindings = findings.filter((finding) =>
            finding.message.includes("MustCharlieClauses"),
        );
        expect(charlieFindings).toHaveLength(4);
        const charlieLayers = charlieFindings
            .map((finding) => finding.message)
            .join("\n");
        expect(charlieLayers).toContain("missing GuardClauses sibling");
        expect(charlieLayers).toContain("missing FluentValidation sibling");
        expect(charlieLayers).toContain("missing DataAnnotations sibling");
        expect(charlieLayers).toContain("missing Rules sibling");

        // Total: 1 (Alpha order-mismatch) + 1 (Bravo ambiguous-delegation)
        // + 4 (Charlie missing-layer × 4) — nothing else should have
        // slipped in from these three families.
        expect(findings).toHaveLength(6);
    });

    it("boundary: word-boundary-safe Is/In stripping keeps a family with an 'Instance' concept clean", async () => {
        // DeltaRules.IsInstance must normalise to "Instance" (not further
        // mangled by the Rules-layer "In"-prefix strip into "stance"),
        // while DeltaRules.InPast correctly normalises to "Past". Both
        // align with MustDeltaClauses' own Instance/Past order across every
        // layer, so a correct, word-boundary-safe implementation produces
        // zero findings here — this is the "still valid" outcome for this
        // boundary case (plan §4.6: a boundary case can legitimately come
        // out either way).
        const findings = await runBoundary(orderingRule, "ordering");
        expect(findings).toEqual([]);
    });

    describe("normalizeOperationName / normalizeBaseKey (word-boundary-safe normalisation table)", () => {
        it("strips the Rules-layer 'Is' prefix", () => {
            expect(normalizeOperationName("IsEmpty", "", "rules")).toBe(
                "Empty",
            );
        });

        it("strips 'In' only for the whitelisted date-shaped remainder (InPast -> Past)", () => {
            expect(normalizeOperationName("InPast", "", "rules")).toBe("Past");
            expect(
                normalizeOperationName("InFutureOrPresent", "", "rules"),
            ).toBe("FutureOrPresent");
        });

        it("does NOT strip 'In' out of the middle of a longer word (word-boundary safety)", () => {
            // "IsInstance" -> Is-strip -> "Instance" -> starts with "In" but
            // the remainder "stance" is not a whitelisted concept, so it
            // must survive untouched.
            expect(normalizeOperationName("IsInstance", "", "rules")).toBe(
                "Instance",
            );
            // Same check without the leading "Is", directly on "Invalid":
            // remainder "valid" is not whitelisted either.
            expect(normalizeOperationName("IsInvalid", "", "rules")).toBe(
                "Invalid",
            );
        });

        it("strips the DataAnnotations 'Attribute'/'StringAttribute' suffix", () => {
            expect(
                normalizeOperationName(
                    "NotEmptyWidgetAttribute",
                    "",
                    "dataAnnotations",
                ),
            ).toBe("NotEmptyWidget");
            expect(
                normalizeOperationName(
                    "NotEmptyStringAttribute",
                    "",
                    "dataAnnotations",
                ),
            ).toBe("NotEmpty");
        });

        it("strips a domain name appearing as a prefix or a suffix", () => {
            expect(
                normalizeOperationName("WidgetControl", "Widget", "must"),
            ).toBe("Control");
            expect(
                normalizeOperationName("NotEmptyWidget", "Widget", "must"),
            ).toBe("NotEmpty");
        });

        it("leaves a bare negative-prefix word untouched (guards against stripping to an empty string)", () => {
            // "Invalid" alone (length 7) is not > 7, so the Invalid-prefix
            // strip must not fire; ditto "Not"/"Non" at exactly length 3.
            expect(tryStripNegativePrefix("Invalid")).toEqual({
                isNegative: false,
                baseName: "Invalid",
            });
            expect(tryStripNegativePrefix("Not")).toEqual({
                isNegative: false,
                baseName: "Not",
            });
            expect(normalizeBaseKey("Invalid")).toBe("Invalid");
        });

        it("unifies a negative-complement name with its positive counterpart's key", () => {
            expect(normalizeBaseKey("NotEmpty")).toBe("Empty");
            expect(normalizeBaseKey("NonCompliant")).toBe("Compliant");
            expect(normalizeBaseKey("InvalidToken")).toBe("Token");
            expect(normalizeBaseKey("Empty")).toBe("Empty");
        });

        it("buildGroupOrder deduplicates and preserves first-seen declaration order", () => {
            expect(
                buildGroupOrder(["NotEmpty", "NotDuplicate"], "", "must"),
            ).toEqual(["Empty", "Duplicate"]);
            // A second method that normalises to an already-seen key does
            // not add a duplicate entry.
            expect(
                buildGroupOrder(
                    ["NotEmpty", "Empty", "NotDuplicate"],
                    "",
                    "must",
                ),
            ).toEqual(["Empty", "Duplicate"]);
        });
    });
});
