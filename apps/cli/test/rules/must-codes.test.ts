import { describe, expect, it } from "vitest";

import { mustCodesRule } from "../../src/audit/rules/must-codes.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * `must-codes` (legacy Rule13) — see `apps/cli/src/audit/rules/must-codes.ts`
 * for the full write-up of the seven checks (a)-(g) ported from
 * `tools/audit-cli/rules/Test-Rule13-MustCodes.ps1`, and of how the
 * clause-file -> domain map is derived from each `MustCodes.<Domain>.cs`
 * file's `// Serves:` comment instead of a hardcoded list.
 *
 * `valid/`'s Widget domain/clause/guard/attribute quartet does not exist in
 * the real repo — it is invented purely for this fixture. The fact that it
 * still produces zero findings is itself the direct proof that the domain
 * map is derived, not hardcoded: if this rule still had a fixed clause-file
 * -> domain lookup table (the legacy `$domainMap` bug, plan §2.2), a brand
 * new domain nobody taught the rule about would immediately fail check (f)
 * ("no domain mapping found") even in a fixture with no other problems.
 */
describe("must-codes (Rule13)", () => {
    it("produces zero findings on a conforming Widget domain/clause/guard/attribute tree", async () => {
        await expectValid(mustCodesRule, "must-codes");
    });

    it("fails on a tree violating all seven checks (a)-(g), with one distinct finding per check", async () => {
        const findings = await expectInvalid(mustCodesRule, "must-codes");
        const messages = findings.map((f) => f.message);

        // (a) MustWidgetClauses.cs: Assembled's Fail/FromBool call passes no
        // MustCodes constant at all — the exact shape of bug that let the
        // legacy tool's Rules 03/04/05/07 go silently vacuous for months,
        // now caught here too.
        expect(
            messages.some((m) =>
                /^\(a\).*Assembled.*passes no MustCodes constant/.test(m),
            ),
        ).toBe(true);

        // (b) MustCodes.Sensor.cs: Reading.Orphaned is declared but never
        // referenced anywhere in the fixture tree, and carries no "reserved"
        // doc comment, so it is not exempt.
        expect(
            messages.some((m) =>
                /^\(b\).*Sensor\.Reading\.Orphaned.*never referenced/.test(m),
            ),
        ).toBe(true);

        // (c) MustGammaClauses.cs: a hardcoded "gamma.level.hacked" literal
        // stands in for the MustCodes.Gamma.Level.High constant.
        expect(
            messages.some((m) =>
                /^\(c\).*hardcoded code string literal "gamma\.level\.hacked"/.test(
                    m,
                ),
            ),
        ).toBe(true);

        // (d) WidgetAttributes.cs: BrokenAttribute declares
        // MustCodes.Widget.State.Broken but dispatches to Must.Be.Broken,
        // whose body actually produces Widget.State.Assembled.
        expect(
            messages.some(
                (m) =>
                    /^\(d\).*BrokenAttribute declares code Widget\.State\.Broken/.test(
                        m,
                    ) && m.includes("doesn't produce that code"),
            ),
        ).toBe(true);

        // (e) GuardWidgetClauses.cs: GuardFailure.Throw's first argument is a
        // string literal instead of the IMustResult from Must.Be.Assembled.
        expect(
            messages.some((m) =>
                /^\(e\).*string literal as its first argument/.test(m),
            ),
        ).toBe(true);

        // (f) — two distinct sub-kinds of the same check letter:
        //   (f-i)  MustDeltaClauses.cs resolves to no domain at all (no
        //          MustCodes.<Domain>.cs file's Serves comment lists it).
        //   (f-ii) MustGammaClauses.cs (mapped to Gamma) references
        //          MustCodes.Sensor.* — a different domain's constant.
        expect(
            messages.some((m) =>
                /^\(f\) MustDeltaClauses\.cs: no domain mapping found/.test(m),
            ),
        ).toBe(true);
        expect(
            messages.some(
                (m) =>
                    /^\(f\) MustGammaClauses\.cs: references MustCodes\.Sensor\.\*/.test(
                        m,
                    ) && m.includes("mapped to domain 'Gamma'"),
            ),
        ).toBe(true);

        // (g) MustCodes.Ledger.cs: a "using PineGuard..." line under Codes/
        // breaks the "dependency-free leaf" invariant.
        expect(
            messages.some((m) =>
                /^\(g\).*using PineGuard\.\.\..*dependency-free leaf/.test(m),
            ),
        ).toBe(true);
    });

    it("boundary: a constant whose containing class doc comment says 'Reserved for ...' is exempt from check (b), but a plain unused constant next to it is not", async () => {
        const findings = await runBoundary(mustCodesRule, "must-codes");
        const messages = findings.map((f) => f.message);

        // Beacon.Uplink.Pending is unused but Uplink's doc comment says
        // "Reserved for a future satellite-uplink adapter ..." — same
        // derivation principle as the domain map itself (read it off an
        // existing doc comment instead of hardcoding an exemption list, as
        // the legacy script did for MustCodes.Value.Argument.Invalid). This
        // boundary case comes out still-valid: no finding.
        expect(messages.some((m) => m.includes("Beacon.Uplink.Pending"))).toBe(
            false,
        );

        // Beacon.Diagnostics.Stale is unused too, but has no "reserved"
        // wording anywhere on it or its containing class — it is genuine,
        // unexempted drift. This boundary case comes out still-invalid: a
        // finding, same as any other unused constant.
        expect(
            messages.some(
                (m) =>
                    m.includes("Beacon.Diagnostics.Stale") &&
                    m.startsWith("(b)"),
            ),
        ).toBe(true);

        // Beacon.Signal.Lost is actually used (by MustBeaconClauses.cs's
        // Online method) and is not part of this boundary probe at all.
        expect(messages.some((m) => m.includes("Beacon.Signal.Lost"))).toBe(
            false,
        );
    });
});
