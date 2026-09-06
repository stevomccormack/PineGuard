import { describe, expect, it } from "vitest";

import { mustUsageRule } from "../../src/audit/rules/must-usage.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * Tests for the `must-usage` rule (plan P2.2, `docs/ai/plans/audit-cli-rebuild.md`
 * §4.3 row "Rule03/04/05 → must-usage", §8, §2.2). See
 * `src/audit/rules/must-usage.ts`'s header comment for the full design
 * rationale (the bug history, layer directories, and the deliberately
 * dropped complement heuristic).
 */
describe("must-usage", () => {
    it("produces zero findings when every Must clause is called from Guard, Fluent, and DataAnnotations", async () => {
        await expectValid(mustUsageRule, "must-usage");
    });

    it("flags a Must clause with no call site in any layer (the exact 'cannot fail' gap Rule03/04/05 had for months)", async () => {
        const findings = await expectInvalid(mustUsageRule, "must-usage");

        // Baz is never called anywhere in the invalid/ fixture tree, so it
        // must be reported once per layer (guard, fluent, annotations) —
        // three findings, not one, since must-usage checks all three by
        // default and each layer's own call-site check is independent.
        const bazKeys = findings
            .map((finding) => finding.key)
            .filter((key) => key.endsWith(":Baz"))
            .sort();
        expect(bazKeys).toEqual([
            "must-usage:annotations:Baz",
            "must-usage:fluent:Baz",
            "must-usage:guard:Baz",
        ]);

        // Foo IS called everywhere in the invalid/ fixture, so it must
        // never appear in the findings — a false positive here would mean
        // the rule over-flags, not just under-flags.
        expect(findings.some((finding) => finding.key.endsWith(":Foo"))).toBe(
            false,
        );
    });

    it("boundary: a Must method named only in a comment/string literal is NOT treated as a real call site", async () => {
        const findings = await runBoundary(mustUsageRule, "must-usage");

        // Qux is mentioned in an XML doc comment, a line comment, and a
        // string literal across the three layer fixtures, but never
        // actually invoked as Must.Be.Qux(...) anywhere. A naive
        // text/regex search would wrongly treat those mentions as usage;
        // the AST-based invocation search must not, so Qux is still
        // reported as unused in every layer — proving the rule can't be
        // fooled the way a text-search-based tool would be.
        const quxKeys = findings
            .map((finding) => finding.key)
            .filter((key) => key.endsWith(":Qux"))
            .sort();
        expect(quxKeys).toEqual([
            "must-usage:annotations:Qux",
            "must-usage:fluent:Qux",
            "must-usage:guard:Qux",
        ]);
    });
});
