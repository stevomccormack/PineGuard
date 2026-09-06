import { readFileSync, readdirSync, statSync } from "node:fs";
import { dirname, join, relative } from "node:path";
import { fileURLToPath } from "node:url";

import { describe, expect, it } from "vitest";

import type { Finding, Rule, RuleContext } from "../../src/audit/types.js";
import { expectRuleCanFail, runRuleOnFixture } from "./runRule.js";

/**
 * Self-test proving the P1.4 harness plumbing works end-to-end, using a
 * trivial demo rule and demo fixtures — NOT a real audit rule. Real rules
 * (P2.1-P2.14) live in `src/audit/rules/<slug>.ts` with tests in
 * `test/rules/<slug>.test.ts`; see `test/README.md` for that convention.
 */

const HERE = dirname(fileURLToPath(import.meta.url));
const FIXTURE_ROOT = join(HERE, "..", "fixtures", "_demo-harness");
const PASS_DIR = join(FIXTURE_ROOT, "pass");
const FAIL_DIR = join(FIXTURE_ROOT, "fail");

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
 * Demo rule: flags any file under `ctx.rootDir` containing the literal
 * string "BADWORD". Exists only to prove `runRuleOnFixture` /
 * `expectRuleCanFail` wire a `Rule` up to its fixtures correctly.
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

describe("runRule test harness (P1.4 self-test)", () => {
    it("runRuleOnFixture returns no findings for the pass fixture", async () => {
        const findings = await runRuleOnFixture(demoRule, PASS_DIR);
        expect(findings).toEqual([]);
    });

    it("runRuleOnFixture returns at least one finding for the fail fixture", async () => {
        const findings = await runRuleOnFixture(demoRule, FAIL_DIR);
        expect(findings.length).toBeGreaterThanOrEqual(1);
        expect(findings[0]?.rule).toBe("_demo-harness");
    });

    it("expectRuleCanFail passes for the real fail fixture", async () => {
        const findings = await expectRuleCanFail(demoRule, FAIL_DIR);
        expect(findings.length).toBeGreaterThanOrEqual(1);
    });

    it("expectRuleCanFail throws when pointed at the pass fixture (proving it isn't vacuous)", async () => {
        await expect(expectRuleCanFail(demoRule, PASS_DIR)).rejects.toThrow(
            /produced zero findings/,
        );
    });

    it("runRuleOnFixture throws a clear error for a missing fixture directory", async () => {
        await expect(
            runRuleOnFixture(demoRule, join(FIXTURE_ROOT, "does-not-exist")),
        ).rejects.toThrow(/fixture directory does not exist/);
    });
});
