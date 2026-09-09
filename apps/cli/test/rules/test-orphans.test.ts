import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

import { describe, expect, it } from "vitest";

import { getRule } from "../../src/audit/catalog.js";
import "../../src/audit/rules/test-orphans.js";
import type { RuleContext } from "../../src/audit/types.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for `test-orphans` (legacy `Rule53`) — plan §9.2 P2.13.
 *
 * See `src/audit/rules/test-orphans.ts`'s own doc comment for the full
 * "what this fixes" writeup; this file only asserts against the VIBE
 * fixtures under `test/fixtures/test-orphans/` (plan §4.6).
 */

const rule = getRule("test-orphans");
if (!rule) {
    throw new Error(
        '"test-orphans" did not register — check src/audit/rules/test-orphans.ts is imported above.',
    );
}

describe("test-orphans", () => {
    it("does not flag any *Tests.cs whose subject is resolvable (same project, cross-project via ProjectReference, partial-class split, a family-per-file source name, a dotted partial file name, or an enum)", async () => {
        await expectValid(rule, "test-orphans");
    });

    it("flags a *Tests.cs whose subject is declared nowhere — neither the same-named project nor any referenced project (real drift, not a false positive)", async () => {
        const findings = await expectInvalid(rule, "test-orphans");

        expect(findings).toHaveLength(1);
        expect(findings[0]?.file).toBe("tests/Qux.UnitTests/QuxTests.cs");
        expect(findings[0]?.message).toContain("'Qux'");
        expect(findings[0]?.message).toContain("Qux.UnitTests");
    });

    it("boundary: a singular/plural near-miss (WidgetTests.cs vs. class Widgets) is still flagged — no fuzzy/plural-tolerant matching", async () => {
        const findings = await runBoundary(rule, "test-orphans");

        expect(findings).toHaveLength(1);
        expect(findings[0]?.file).toBe("tests/Widget.UnitTests/WidgetTests.cs");
        expect(findings[0]?.message).toContain("'Widget'");
    });

    it("resolves a family-per-file source name (GroupedAttributesTests.cs -> GroupedAttributes.cs, the DataAnnotations addendum's own canonical pairing), a dotted partial file (SplitBoolTests.cs -> Split.Bool.cs) and an enum (ModeTests.cs -> enum Mode in Shapes.cs)", async () => {
        const here = dirname(fileURLToPath(import.meta.url));
        const ctx: RuleContext = {
            rootDir: join(here, "..", "fixtures", "test-orphans", "valid"),
        };
        const findings = await rule.run(ctx);

        expect(findings.map((f) => f.file)).toEqual([]);
    });

    it("suppresses a real orphan finding when ctx.exceptions carries a matching test-orphans entry (path-based exception, plan §8)", async () => {
        const here = dirname(fileURLToPath(import.meta.url));
        const invalidFixtureDir = join(
            here,
            "..",
            "fixtures",
            "test-orphans",
            "invalid",
        );
        const ctx: RuleContext = {
            rootDir: invalidFixtureDir,
            exceptions: { "test-orphans": ["Qux.UnitTests/QuxTests.cs"] },
        };

        await expect(rule.run(ctx)).resolves.toEqual([]);
    });
});
