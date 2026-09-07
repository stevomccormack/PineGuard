import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import "../../src/audit/rules/test-records.js";
import {
    expectInvalid,
    expectValid,
    runBoundary,
    runRuleOnFixture,
} from "../support/runRule.js";

/**
 * Tests for the `test-records` rule (plan
 * docs/ai/plans/audit-cli-rebuild.md §4.3, §8, §2.4, P2.12) — the AST-based,
 * per-layer-scoped rebuild of the legacy tool's Rule52. See
 * `src/audit/rules/test-records.ts` for the full write-up of the old rule's
 * two bugs (layers that forbid custom case records outright vs. ones that
 * merely require the right base) and where the legitimate bare-record
 * convention is actually documented.
 */

const rule = getRule("test-records");
if (!rule) {
    throw new Error(
        "test-records did not register itself — check the registerRule() call in src/audit/rules/test-records.ts",
    );
}

const HERE = dirname(fileURLToPath(import.meta.url));
const FIXTURES_ROOT = join(HERE, "..", "fixtures", "test-records");

describe("test-records", () => {
    it("is registered under its slug and the shared legacy id Rule52", () => {
        expect(rule.slug).toBe("test-records");
        expect(rule.legacyId).toBe("Rule52");
        expect(rule.scope).toBe("testing");
        expect(rule.gate).toBe(false);
    });

    it("passes on a conforming tree spanning a base-record layer, a forbidding layer, and the documented DataAnnotations exception", async () => {
        await expectValid(rule, "test-records");
    });

    it("does not flag a ThrowsCase<>-derived record in a layer that forbids custom case records (root §4.2/§8.2 throws shape; the Fluent prohibition is scoped to ReturnCase<T, bool> replacements)", async () => {
        const fluentValidDir = join(
            FIXTURES_ROOT,
            "valid",
            "tests",
            "PineGuard.FluentValidation.UnitTests",
        );

        await expect(runRuleOnFixture(rule, fluentValidDir)).resolves.toEqual(
            [],
        );
    });

    it("proves the old blanket-scoping bug is fixed: a bare value-object record in a layer that forbids custom CASE records is not itself flagged", async () => {
        // Widget (no base at all) lives in the Guard fixture below — Guard is
        // one of the layers that forbids locally-declared *Case* records
        // outright. The old Rule52 line-scanner's "must inherit a real base"
        // check was scoped by name (`\w+Case\b`), but this proves the AST
        // rewrite draws exactly the same line deliberately, not by accident:
        // a bare record that is not itself a Case record must never be
        // flagged, in ANY layer, forbidding or not.
        const guardFixtureDir = join(
            FIXTURES_ROOT,
            "valid",
            "tests",
            "PineGuard.GuardClauses.UnitTests",
        );
        const findings = await runRuleOnFixture(rule, guardFixtureDir);

        expect(findings).toHaveLength(0);
        expect(findings.some((f) => f.message.includes("Widget"))).toBe(false);
    });

    it("fails on a layer where the base-record convention applies but the record has no base at all (Core)", async () => {
        const findings = await expectInvalid(rule, "test-records");

        const coreFinding = findings.find((f) =>
            f.file.includes("PineGuard.Core.UnitTests"),
        );
        expect(coreFinding).toBeDefined();
        expect(coreFinding?.rule).toBe("test-records");
        expect(coreFinding?.message).toContain("ValidCase");
        expect(coreFinding?.message).toContain("no base clause at all");
        expect(coreFinding?.key).toBe(
            "test-records:missing-base:tests/PineGuard.Core.UnitTests/BadRulesTestData.cs:ValidCase",
        );
    });

    it("fails on a layer that forbids custom case records outright, even when the record's base is textbook-correct (Fluent)", async () => {
        const findings = await expectInvalid(rule, "test-records");

        const fluentFinding = findings.find((f) =>
            f.file.includes("PineGuard.FluentValidation.UnitTests"),
        );
        expect(fluentFinding).toBeDefined();
        expect(fluentFinding?.message).toContain("Fluent");
        expect(fluentFinding?.message).toContain("FluentCase");
        expect(fluentFinding?.message).toContain("explicitly forbids");
        expect(fluentFinding?.key).toBe(
            "test-records:forbidden:tests/PineGuard.FluentValidation.UnitTests/FluentBadExtensionsTestData.cs:ValidCase",
        );
    });

    it("boundary: a Core case record with a base clause that is the raw BaseCase infrastructure type is NOT flagged (P4.3 finding #4 — the ban had no spec basis and was removed)", async () => {
        const findings = await runBoundary(rule, "test-records");

        expect(findings).toEqual([]);
    });
});
