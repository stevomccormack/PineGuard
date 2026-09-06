import { readFileSync, readdirSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

import { afterEach, describe, expect, it } from "vitest";

import {
    type CatalogEntry,
    clearCatalog,
    registerRule,
} from "../../src/audit/catalog.js";
import {
    applyBaseline,
    applyExceptions,
    buildContext,
    resolveSelection,
    runAudit,
} from "../../src/audit/engine.js";
import type { Finding, Rule } from "../../src/audit/types.js";
import { runRuleOnFixture } from "../support/runRule.js";

/**
 * Proves the P1.5 engine's plumbing: selection combination semantics,
 * exceptions filtering, the (no-op) baseline pass-through, real-context
 * loading, and the overall exit-code contract. This is engine plumbing, not
 * a real audit rule — most fakes below are plain, hand-registered
 * `CatalogEntry`s; one test reuses the P1.4 harness against a real fixture
 * where that fits more naturally than a hand-typed `Finding`.
 */

function fakeEntry(overrides: Partial<CatalogEntry> = {}): CatalogEntry {
    return {
        slug: "fake-rule",
        scope: "library",
        gate: false,
        description: "A fake rule, used only by this test file.",
        run: () => [],
        ...overrides,
    };
}

function findingFor(rule: string, file: string, message: string): Finding {
    return { rule, file, message, key: `${rule}:${file}:${message}` };
}

afterEach(() => {
    clearCatalog();
});

describe("resolveSelection", () => {
    it("selects every registered rule for an empty selection ('all' implicit)", () => {
        registerRule(fakeEntry({ slug: "a" }));
        registerRule(fakeEntry({ slug: "b" }));

        const { entries, error } = resolveSelection([]);

        expect(error).toBeUndefined();
        expect(entries.map((entry) => entry.slug)).toEqual(["a", "b"]);
    });

    it("selects every registered rule for the explicit 'all' token", () => {
        registerRule(fakeEntry({ slug: "a" }));
        registerRule(fakeEntry({ slug: "b" }));

        const { entries } = resolveSelection(["all"]);

        expect(entries.map((entry) => entry.slug)).toEqual(["a", "b"]);
    });

    it("narrows the implicit 'all' set by --scope", () => {
        registerRule(fakeEntry({ slug: "lib-rule", scope: "library" }));
        registerRule(fakeEntry({ slug: "doc-rule", scope: "docs" }));

        const { entries } = resolveSelection([], { scope: "docs" });

        expect(entries.map((entry) => entry.slug)).toEqual(["doc-rule"]);
    });

    it("narrows the implicit 'all' set by --gate", () => {
        registerRule(fakeEntry({ slug: "gated", gate: true }));
        registerRule(fakeEntry({ slug: "ungated", gate: false }));

        const { entries } = resolveSelection([], { gate: true });

        expect(entries.map((entry) => entry.slug)).toEqual(["gated"]);
    });

    it("ANDs --scope and --gate together", () => {
        registerRule(
            fakeEntry({ slug: "match", scope: "testing", gate: true }),
        );
        registerRule(
            fakeEntry({ slug: "wrong-scope", scope: "docs", gate: true }),
        );
        registerRule(
            fakeEntry({ slug: "wrong-gate", scope: "testing", gate: false }),
        );

        const { entries } = resolveSelection([], {
            scope: "testing",
            gate: true,
        });

        expect(entries.map((entry) => entry.slug)).toEqual(["match"]);
    });

    it("resolves an explicit slug", () => {
        registerRule(fakeEntry({ slug: "must-usage" }));
        registerRule(fakeEntry({ slug: "other" }));

        const { entries } = resolveSelection(["must-usage"]);

        expect(entries.map((entry) => entry.slug)).toEqual(["must-usage"]);
    });

    it("resolves an explicit legacy id", () => {
        registerRule(fakeEntry({ slug: "test-files", legacyId: "Rule50" }));

        const { entries } = resolveSelection(["Rule50"]);

        expect(entries.map((entry) => entry.slug)).toEqual(["test-files"]);
    });

    it("resolves multiple explicit identifiers, mixing slugs and legacy ids", () => {
        registerRule(fakeEntry({ slug: "test-files", legacyId: "Rule50" }));
        registerRule(fakeEntry({ slug: "doc-links", legacyId: "Rule11" }));
        registerRule(fakeEntry({ slug: "unrelated" }));

        const { entries } = resolveSelection(["Rule50", "doc-links"]);

        expect(entries.map((entry) => entry.slug).sort()).toEqual([
            "doc-links",
            "test-files",
        ]);
    });

    it("lets explicit identifiers override --scope/--gate rather than being narrowed by them", () => {
        registerRule(
            fakeEntry({ slug: "must-usage", scope: "library", gate: false }),
        );

        const { entries, error } = resolveSelection(["must-usage"], {
            scope: "docs",
            gate: true,
        });

        expect(error).toBeUndefined();
        expect(entries.map((entry) => entry.slug)).toEqual(["must-usage"]);
    });

    it("reports a clear usage error for an unknown slug", () => {
        const { entries, error } = resolveSelection(["nonexistent-slug"]);

        expect(entries).toEqual([]);
        expect(error).toMatch(/unknown rule "nonexistent-slug"/);
    });

    it("treats 'all' combined with another token as an unknown-rule error, not as 'run everything'", () => {
        registerRule(fakeEntry({ slug: "a" }));

        const { error } = resolveSelection(["all", "a"]);

        expect(error).toMatch(/unknown rule "all"/);
    });
});

describe("applyExceptions", () => {
    it("passes every finding through when no exceptions are configured", () => {
        const findings = [findingFor("r1", "Foo.cs", "bad thing")];

        expect(applyExceptions(findings)).toEqual(findings);
    });

    it("passes every finding through when the rule's slug has no exception entry", () => {
        const findings = [findingFor("r1", "Foo.cs", "bad thing")];

        expect(applyExceptions(findings, { "other-rule": ["Foo.cs"] })).toEqual(
            findings,
        );
    });

    it("filters a finding whose file matches an exception substring for its rule", () => {
        const findings = [findingFor("r1", "src/Foo.cs", "bad thing")];

        expect(applyExceptions(findings, { r1: ["Foo.cs"] })).toEqual([]);
    });

    it("filters a finding whose message matches an exception substring for its rule", () => {
        const findings = [
            findingFor("r1", "src/Foo.cs", "known false positive"),
        ];

        expect(
            applyExceptions(findings, { r1: ["known false positive"] }),
        ).toEqual([]);
    });

    it("leaves other rules' findings untouched by one rule's exceptions", () => {
        const findings = [
            findingFor("r1", "src/Foo.cs", "bad thing"),
            findingFor("r2", "src/Foo.cs", "bad thing"),
        ];

        expect(applyExceptions(findings, { r1: ["Foo.cs"] })).toEqual([
            findingFor("r2", "src/Foo.cs", "bad thing"),
        ]);
    });

    it("filters a real, fixture-produced finding via a matching exception entry", async () => {
        // Reuses P1.4's own harness fixture (test/fixtures/_demo-harness/invalid/)
        // instead of hand-typing a Finding, so this exercises applyExceptions
        // against output actually produced by a rule's run() against a real
        // file on disk, via the P1.4 harness's lower-level runRuleOnFixture.
        const here = path.dirname(fileURLToPath(import.meta.url));
        const fixtureDir = path.join(
            here,
            "..",
            "fixtures",
            "_demo-harness",
            "invalid",
        );

        // Trimmed reimplementation of test/support/runRule.test.ts's own
        // (unexported) demoRule: flags any file containing the literal
        // "BADWORD". Not a real audit rule — engine-plumbing test fixture only.
        const badWordRule: Rule = {
            slug: "_demo-harness",
            run(ctx): Finding[] {
                return readdirSync(ctx.rootDir)
                    .filter((entry) =>
                        readFileSync(
                            path.join(ctx.rootDir, entry),
                            "utf8",
                        ).includes("BADWORD"),
                    )
                    .map((entry) =>
                        findingFor(
                            "_demo-harness",
                            entry,
                            "found literal 'BADWORD'",
                        ),
                    );
            },
        };

        const findings = await runRuleOnFixture(badWordRule, fixtureDir);
        expect(findings.length).toBeGreaterThan(0);

        expect(
            applyExceptions(findings, { "_demo-harness": ["Greeter.cs"] }),
        ).toEqual([]);
    });
});

describe("applyBaseline", () => {
    it("is a no-op pass-through (the baseline ratchet is P3.1's job, not P1.5's)", () => {
        const findings = [findingFor("r1", "Foo.cs", "bad thing")];

        expect(applyBaseline(findings, "r1")).toEqual(findings);
    });

    it("does not drop or reorder findings regardless of slug", () => {
        const findings = [
            findingFor("r1", "Foo.cs", "bad thing"),
            findingFor("r1", "Bar.cs", "another thing"),
        ];

        expect(applyBaseline(findings, "some-other-slug")).toEqual(findings);
    });
});

describe("buildContext", () => {
    it("builds a real context: rootDir, trackedFiles, parseFile, parsed vocabulary, and parsed exceptions", () => {
        const ctx = buildContext();

        expect(ctx.rootDir.length).toBeGreaterThan(0);
        expect(ctx.trackedFiles?.length ?? 0).toBeGreaterThan(0);
        expect(typeof ctx.parseFile).toBe("function");
        // docs/ai/specs/language/vocabulary.json's top-level "version" is 1.
        expect(ctx.vocabulary?.version).toBe(1);
        // apps/cli/config/exceptions.json is still the P1.1 placeholder `{}`.
        expect(ctx.exceptions).toEqual({});
    });
});

describe("runAudit", () => {
    it("exits 0 clean when every selected rule produces no findings", async () => {
        registerRule(fakeEntry({ slug: "clean-rule", run: () => [] }));

        const result = await runAudit({ rules: ["clean-rule"] });

        expect(result.exitCode).toBe(0);
        expect(result.outcomes).toHaveLength(1);
        expect(result.outcomes[0]?.findings).toEqual([]);
    });

    it("exits 1 when any selected rule produces a finding", async () => {
        registerRule(
            fakeEntry({
                slug: "dirty-rule",
                run: () => [findingFor("dirty-rule", "Foo.cs", "bad thing")],
            }),
        );

        const result = await runAudit({ rules: ["dirty-rule"] });

        expect(result.exitCode).toBe(1);
        expect(result.outcomes[0]?.findings).toHaveLength(1);
    });

    it("exits 2 with a clear error for an unknown rule (usage error)", async () => {
        const result = await runAudit({ rules: ["nonexistent-slug"] });

        expect(result.exitCode).toBe(2);
        expect(result.error).toMatch(/unknown rule "nonexistent-slug"/);
        expect(result.outcomes).toEqual([]);
    });

    it("exits 2 when a rule throws while running, instead of rejecting", async () => {
        registerRule(
            fakeEntry({
                slug: "crashing-rule",
                run: () => {
                    throw new Error("boom");
                },
            }),
        );

        const result = await runAudit({ rules: ["crashing-rule"] });

        expect(result.exitCode).toBe(2);
        expect(result.error).toMatch(/boom/);
    });

    it("exits 0 cleanly when nothing is selected at all (empty catalog)", async () => {
        const result = await runAudit({});

        expect(result.exitCode).toBe(0);
        expect(result.outcomes).toEqual([]);
    });

    it("runs findings through exceptions before computing the exit code", async () => {
        registerRule(
            fakeEntry({
                slug: "excepted-rule",
                run: () => [
                    findingFor("excepted-rule", "Foo.cs", "known issue"),
                ],
            }),
        );

        // The real apps/cli/config/exceptions.json is still the P1.1
        // placeholder `{}`, so nothing is configured for "excepted-rule" —
        // this proves the exceptions step runs without over-filtering when
        // there is genuinely nothing to except.
        const result = await runAudit({ rules: ["excepted-rule"] });

        expect(result.exitCode).toBe(1);
        expect(result.outcomes[0]?.findings).toHaveLength(1);
    });
});
