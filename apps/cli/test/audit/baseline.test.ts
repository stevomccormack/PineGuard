import { mkdtempSync, readFileSync, rmSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";

import { afterEach, describe, expect, it } from "vitest";

import {
    type CatalogEntry,
    clearCatalog,
    registerRule,
} from "../../src/audit/catalog.js";
import {
    applyBaseline,
    computeBaselineSnapshot,
    resolveSelection,
    runSelectedRules,
    writeBaselineFile,
} from "../../src/audit/engine.js";
import type {
    BaselineConfig,
    Finding,
    RuleContext,
} from "../../src/audit/types.js";

/**
 * Proves the P3.1 baseline ratchet (plan §4.4): `applyBaseline`'s exact-key
 * suppression, its composition with `--gate`/`--no-baseline` via
 * `runSelectedRules` (the exact per-rule pipeline `runAudit` runs — see that
 * function's doc comment in `engine.ts` for why the composition is tested
 * against it directly instead of the real filesystem-backed `runAudit`), and
 * the `--update-baseline` machinery (`computeBaselineSnapshot` +
 * `writeBaselineFile`). Engine-level plumbing, like `engine.test.ts` — plain
 * fakes throughout, no VIBE fixture harness needed.
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

describe("applyBaseline", () => {
    it("suppresses a finding whose key is already accepted in the baseline for that rule", () => {
        const debt = findingFor(
            "noisy-rule",
            "src/Foo.cs",
            "old, accepted debt",
        );
        const baseline: BaselineConfig = { "noisy-rule": [debt.key] };

        expect(applyBaseline([debt], "noisy-rule", baseline)).toEqual([]);
    });

    it("still surfaces a NEW finding for an already-baselined rule (anti-regression property, plan §11)", () => {
        const debt = findingFor(
            "noisy-rule",
            "src/Foo.cs",
            "old, accepted debt",
        );
        const regression = findingFor(
            "noisy-rule",
            "src/Foo.cs",
            "a brand-new problem",
        );
        const baseline: BaselineConfig = { "noisy-rule": [debt.key] };

        const result = applyBaseline(
            [debt, regression],
            "noisy-rule",
            baseline,
        );

        expect(result).toEqual([regression]);
    });

    it("matches on the exact key only — a baselined finding does not suppress a different finding in the same file", () => {
        // Same rule, same file, different message => different key. If this
        // ever came back empty, the baseline would have degraded into the
        // coarse per-file suppression plan §11 explicitly warns against.
        const baselined = findingFor(
            "noisy-rule",
            "src/Foo.cs",
            "known issue A",
        );
        const unrelated = findingFor(
            "noisy-rule",
            "src/Foo.cs",
            "an unrelated issue B",
        );
        const baseline: BaselineConfig = { "noisy-rule": [baselined.key] };

        expect(applyBaseline([unrelated], "noisy-rule", baseline)).toEqual([
            unrelated,
        ]);
    });

    it("is a pass-through when no baseline config is given at all (matches the original P1.5 no-op default)", () => {
        const findings = [findingFor("r1", "Foo.cs", "bad thing")];

        expect(applyBaseline(findings, "r1")).toEqual(findings);
    });

    it("is a pass-through when the baseline has no entry for that slug", () => {
        const findings = [findingFor("r1", "Foo.cs", "bad thing")];

        expect(
            applyBaseline(findings, "r1", { "other-rule": ["x:y:z"] }),
        ).toEqual(findings);
    });

    it("is a pass-through when the slug's baseline entry is an empty array", () => {
        const findings = [findingFor("r1", "Foo.cs", "bad thing")];

        expect(applyBaseline(findings, "r1", { r1: [] })).toEqual(findings);
    });
});

describe("runSelectedRules (--gate + baseline composition)", () => {
    it("a gate rule with only baselined findings passes (empty findings after the ratchet)", async () => {
        const debt = findingFor(
            "gate-rule",
            "src/Foo.cs",
            "old, accepted debt",
        );
        registerRule(
            fakeEntry({ slug: "gate-rule", gate: true, run: () => [debt] }),
        );
        registerRule(fakeEntry({ slug: "ungated-rule", gate: false }));

        const { entries } = resolveSelection([], { gate: true });
        expect(entries.map((entry) => entry.slug)).toEqual(["gate-rule"]);

        const ctx: RuleContext = {
            rootDir: "/fixture",
            exceptions: {},
            baseline: { "gate-rule": [debt.key] },
        };
        const outcomes = await runSelectedRules(ctx, entries, true);

        expect(outcomes).toHaveLength(1);
        expect(outcomes[0]?.findings).toEqual([]);
    });

    it("the same gate rule fails once a genuinely new finding joins the baselined debt", async () => {
        const debt = findingFor(
            "gate-rule",
            "src/Foo.cs",
            "old, accepted debt",
        );
        const fresh = findingFor(
            "gate-rule",
            "src/Foo.cs",
            "a brand-new regression",
        );
        registerRule(
            fakeEntry({
                slug: "gate-rule",
                gate: true,
                run: () => [debt, fresh],
            }),
        );

        const { entries } = resolveSelection([], { gate: true });
        const ctx: RuleContext = {
            rootDir: "/fixture",
            exceptions: {},
            baseline: { "gate-rule": [debt.key] },
        };
        const outcomes = await runSelectedRules(ctx, entries, true);

        expect(outcomes[0]?.findings).toEqual([fresh]);
    });

    it("--no-baseline (applyRatchet: false) bypasses suppression entirely, showing the full debt", async () => {
        const debt = findingFor(
            "noisy-rule",
            "src/Foo.cs",
            "old, accepted debt",
        );
        registerRule(fakeEntry({ slug: "noisy-rule", run: () => [debt] }));

        const { entries } = resolveSelection(["noisy-rule"]);
        const ctx: RuleContext = {
            rootDir: "/fixture",
            exceptions: {},
            baseline: { "noisy-rule": [debt.key] },
        };

        const withRatchet = await runSelectedRules(ctx, entries, true);
        const withoutRatchet = await runSelectedRules(ctx, entries, false);

        expect(withRatchet[0]?.findings).toEqual([]);
        expect(withoutRatchet[0]?.findings).toEqual([debt]);
    });
});

describe("computeBaselineSnapshot", () => {
    it("collects every current (unsuppressed) finding's key, grouped by slug, deduplicated and sorted", async () => {
        registerRule(
            fakeEntry({
                slug: "b-rule",
                run: () => [
                    findingFor("b-rule", "Two.cs", "z issue"),
                    findingFor("b-rule", "One.cs", "a issue"),
                    findingFor("b-rule", "One.cs", "a issue"), // duplicate, must collapse
                ],
            }),
        );
        registerRule(fakeEntry({ slug: "a-rule", run: () => [] })); // clean rule => no entry at all
        registerRule(
            fakeEntry({
                slug: "c-rule",
                run: () => [findingFor("c-rule", "Three.cs", "c issue")],
            }),
        );

        const ctx: RuleContext = { rootDir: "/fixture", exceptions: {} };
        const snapshot = await computeBaselineSnapshot(ctx);

        expect(snapshot).toEqual({
            "b-rule": [
                findingFor("b-rule", "One.cs", "a issue").key,
                findingFor("b-rule", "Two.cs", "z issue").key,
            ],
            "c-rule": [findingFor("c-rule", "Three.cs", "c issue").key],
        });
        expect(Object.keys(snapshot)).not.toContain("a-rule");
    });

    it("does not snapshot findings already suppressed by config/exceptions.json (only genuine debt becomes baseline floor)", async () => {
        registerRule(
            fakeEntry({
                slug: "excepted-rule",
                run: () => [
                    findingFor(
                        "excepted-rule",
                        "Known.cs",
                        "known false positive",
                    ),
                ],
            }),
        );

        const ctx: RuleContext = {
            rootDir: "/fixture",
            exceptions: { "excepted-rule": ["known false positive"] },
        };
        const snapshot = await computeBaselineSnapshot(ctx);

        expect(snapshot).toEqual({});
    });

    it("ignores the current baseline entirely — a snapshot always reflects the full, unsuppressed finding set", async () => {
        const debt = findingFor(
            "noisy-rule",
            "src/Foo.cs",
            "old, accepted debt",
        );
        registerRule(fakeEntry({ slug: "noisy-rule", run: () => [debt] }));

        // Even with an existing baseline already accepting this exact
        // finding, --update-baseline's snapshot must still include it: the
        // whole point is to (re)compute the floor from what's really there,
        // not from what a stale baseline already believed.
        const ctx: RuleContext = {
            rootDir: "/fixture",
            exceptions: {},
            baseline: { "noisy-rule": [debt.key] },
        };
        const snapshot = await computeBaselineSnapshot(ctx);

        expect(snapshot).toEqual({ "noisy-rule": [debt.key] });
    });
});

describe("writeBaselineFile", () => {
    it("writes a stably-ordered, prettified JSON snapshot to apps/cli/config/baseline.json under rootDir", () => {
        const rootDir = mkdtempSync(join(tmpdir(), "pineguard-baseline-test-"));
        try {
            const baseline: BaselineConfig = {
                "z-rule": ["z:key:2", "z:key:1"],
                "a-rule": ["a:key:1", "a:key:1"], // duplicate, must collapse
                "empty-rule": [], // must be dropped entirely
            };

            writeBaselineFile(baseline, rootDir);

            const written = readFileSync(
                join(rootDir, "apps", "cli", "config", "baseline.json"),
                "utf8",
            );

            expect(JSON.parse(written)).toEqual({
                "a-rule": ["a:key:1"],
                "z-rule": ["z:key:1", "z:key:2"],
            });
            // Key order in the raw text matters too, for a clean git diff —
            // JSON.parse alone would not catch an accidental re-ordering.
            expect(written.indexOf('"a-rule"')).toBeLessThan(
                written.indexOf('"z-rule"'),
            );
            expect(written.endsWith("\n")).toBe(true);
        } finally {
            rmSync(rootDir, { recursive: true, force: true });
        }
    });

    it("writes an empty object when every slug's entry is empty", () => {
        const rootDir = mkdtempSync(join(tmpdir(), "pineguard-baseline-test-"));
        try {
            writeBaselineFile({ "clean-rule": [] }, rootDir);

            const written = readFileSync(
                join(rootDir, "apps", "cli", "config", "baseline.json"),
                "utf8",
            );
            expect(JSON.parse(written)).toEqual({});
        } finally {
            rmSync(rootDir, { recursive: true, force: true });
        }
    });
});
