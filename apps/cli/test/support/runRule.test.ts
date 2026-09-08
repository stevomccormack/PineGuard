import { readFileSync, readdirSync, statSync } from "node:fs";
import { join, relative } from "node:path";

import { describe, expect, it } from "vitest";

import type { Finding, Rule, RuleContext } from "../../src/audit/types.js";
import { expectInvalid, expectValid, runBoundary } from "./runRule.js";

/**
 * Self-test proving the P1.4 harness plumbing works end-to-end (fixture
 * convention corrected to VIBE by plan §4.6), using a trivial demo rule and
 * demo fixtures — NOT a real audit rule. Real rules (P2.1-P2.14) live in
 * `src/audit/rules/<slug>.ts` with tests in `test/rules/<slug>.test.ts`; see
 * `test/README.md` for that convention.
 */

/** Recursively lists every file under `dir`. */
function* walk(dir: string): Generator<string> {
    for (const entry of readdirSync(dir)) {
        const full = join(dir, entry);
        if (statSync(full).isDirectory()) {
            yield* walk(full);
        } else {
            yield full;
        }
    }
}

/**
 * Demo rule: flags any file under `ctx.rootDir` containing the literal,
 * case-sensitive substring "BADWORD" — a plain `line.includes("BADWORD")`,
 * with no word-boundary awareness and no code/comment distinction. Exists
 * only to prove `expectValid`/`expectInvalid`/`runBoundary` wire a `Rule`
 * up to its VIBE fixtures correctly; its very naivety is what the
 * `boundary/` fixtures below are chosen to probe.
 */
const demoRule: Rule = {
    slug: "_demo-harness",
    run(ctx: RuleContext): Finding[] {
        const findings: Finding[] = [];
        for (const file of walk(ctx.rootDir)) {
            const lines = readFileSync(file, "utf8").split(/\r?\n/);
            const relPath = relative(ctx.rootDir, file);
            lines.forEach((line, index) => {
                if (line.includes("BADWORD")) {
                    findings.push({
                        rule: demoRule.slug,
                        file: relPath,
                        line: index + 1,
                        message: "found literal 'BADWORD'",
                        key: `${demoRule.slug}:${relPath}:${index + 1}`,
                    });
                }
            });
        }
        return findings;
    },
};

describe("runRule test harness (P1.4 self-test, VIBE convention per §4.6)", () => {
    it("expectValid resolves without throwing against the valid/ fixture", async () => {
        await expect(
            expectValid(demoRule, "_demo-harness"),
        ).resolves.toBeUndefined();
    });

    it("expectInvalid resolves with >=1 finding against the invalid/ fixture", async () => {
        const findings = await expectInvalid(demoRule, "_demo-harness");
        expect(findings.length).toBeGreaterThanOrEqual(1);
        expect(findings[0]?.rule).toBe("_demo-harness");
    });

    it("expectInvalid throws when the invalid/ fixture doesn't actually violate the rule (proving it isn't vacuous)", async () => {
        // "_demo-harness-vacuous" is a harness-only fixture whose invalid/
        // directory contains no BADWORD at all — i.e. content that belongs
        // in a valid/ fixture, filed under invalid/ by mistake. This is the
        // exact shape of bug that let the old tool's Rules 03/04/05/07 pass
        // silently for months, and is the case expectInvalid must catch.
        await expect(
            expectInvalid(demoRule, "_demo-harness-vacuous"),
        ).rejects.toThrow(/produced zero findings/);
    });

    it("runBoundary flags the substring-in-identifier case but not the case-variant case", async () => {
        const findings = await runBoundary(demoRule, "_demo-harness");
        const flaggedFiles = findings.map((finding) => finding.file);

        // Boundary probe #1 (IdentifierSubstring.cs): "BADWORD" appears only
        // as a substring of a longer identifier, in code, not a comment.
        // The rule's `includes` check has no word-boundary awareness, so it
        // is STILL flagged — this fixture is a boundary case that comes out
        // still-invalid.
        expect(flaggedFiles).toContain("IdentifierSubstring.cs");

        // Boundary probe #2 (CaseVariant.cs): the word appears in a comment
        // but lowercase ("badword"). The rule's check is case-sensitive, so
        // it is NOT flagged — this fixture is a boundary case that comes
        // out still-valid.
        expect(flaggedFiles).not.toContain("CaseVariant.cs");

        expect(findings).toHaveLength(1);
    });

    it("expectValid throws a clear, distinct error for a nonexistent fixture path", async () => {
        await expect(
            expectValid(demoRule, "_demo-harness-does-not-exist"),
        ).rejects.toThrow(/fixture directory does not exist/);
    });

    it("expectInvalid throws a clear, distinct error for a nonexistent fixture path", async () => {
        await expect(
            expectInvalid(demoRule, "_demo-harness-does-not-exist"),
        ).rejects.toThrow(/fixture directory does not exist/);
    });
});
