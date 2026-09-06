import { expect, test } from "vitest";

import { docLinksRule } from "../../src/audit/rules/doc-links.js";
import { expectInvalid, expectValid, runBoundary } from "../support/runRule.js";

/**
 * `doc-links` (legacy Rule11, plan §4.3/§8/§4.6). See the header comment of
 * `src/audit/rules/doc-links.ts` for the full path-resolution strategy and
 * the fixture-isolation design (no `ctx.trackedFiles` in the harness falls
 * back to plain filesystem checks, which is exactly what the `valid`/
 * `invalid`/`boundary` fixtures below rely on).
 */

test("doc-links: passes on a tree where every reference resolves (and `+ planned` refs are skipped)", async () => {
    await expectValid(docLinksRule, "doc-links");
});

test("doc-links: fails on a tree with an unresolved link and an unresolved backticked path", async () => {
    const findings = await expectInvalid(docLinksRule, "doc-links");
    expect(findings.length).toBeGreaterThanOrEqual(2);
    expect(findings.every((finding) => finding.rule === "doc-links")).toBe(
        true,
    );
    const paths = findings.map((finding) => finding.message);
    expect(paths.some((message) => message.includes("missing.md"))).toBe(true);
    expect(paths.some((message) => message.includes("docs/nope.md"))).toBe(
        true,
    );
});

test("doc-links: boundary — a reference that resolves only relative to the referencing file's own directory, not repo-root, is still accepted", async () => {
    // pkg/AGENTS.md references `sub/target.md`. There is no sub/target.md at
    // the fixture root (repo-root-relative resolution fails), but
    // pkg/sub/target.md does exist (directory-relative resolution
    // succeeds) — proving both resolution bases are actually tried before a
    // reference is reported broken, not just the first one.
    const findings = await runBoundary(docLinksRule, "doc-links");
    expect(findings).toEqual([]);
});
