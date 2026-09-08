import { afterEach, describe, expect, it } from "vitest";

import {
    DuplicateRuleError,
    type CatalogEntry,
    clearCatalog,
    getAllRules,
    getRule,
    isRuleScope,
    registerRule,
} from "../../src/audit/catalog.js";

/** Proves the P1.5 catalog registry: registration, slug/legacyId lookup, duplicate detection, and correct zero-entry behaviour. */

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

// Every test starts from a clean catalog — this module is a process-wide
// singleton, and other test files (e.g. engine.test.ts) register their own
// fakes into the same registry.
afterEach(() => {
    clearCatalog();
});

describe("empty catalog", () => {
    it("getAllRules returns an empty array", () => {
        expect(getAllRules()).toEqual([]);
    });

    it("getRule returns undefined for any input", () => {
        expect(getRule("anything")).toBeUndefined();
        expect(getRule("Rule50")).toBeUndefined();
    });
});

describe("registerRule / getRule", () => {
    it("retrieves a registered rule by its slug", () => {
        const entry = fakeEntry({ slug: "test-files", legacyId: "Rule50" });
        registerRule(entry);

        expect(getRule("test-files")).toBe(entry);
    });

    it("retrieves the same registered rule by its legacy id", () => {
        const entry = fakeEntry({ slug: "test-files", legacyId: "Rule50" });
        registerRule(entry);

        expect(getRule("Rule50")).toBe(entry);
    });

    it("returns the rule with no legacyId set as undefined for any legacy-id-shaped lookup", () => {
        registerRule(fakeEntry({ slug: "no-legacy" }));

        expect(getRule("Rule99")).toBeUndefined();
    });

    it("getAllRules returns every registered rule, sorted by slug", () => {
        registerRule(fakeEntry({ slug: "zzz-rule" }));
        registerRule(fakeEntry({ slug: "aaa-rule" }));
        registerRule(fakeEntry({ slug: "mmm-rule" }));

        expect(getAllRules().map((rule) => rule.slug)).toEqual([
            "aaa-rule",
            "mmm-rule",
            "zzz-rule",
        ]);
    });
});

describe("duplicate detection", () => {
    it("throws DuplicateRuleError registering the same slug twice", () => {
        registerRule(fakeEntry({ slug: "dup-slug" }));

        expect(() => registerRule(fakeEntry({ slug: "dup-slug" }))).toThrow(
            DuplicateRuleError,
        );
    });

    it("throws DuplicateRuleError when a legacyId is claimed by a different slug", () => {
        registerRule(fakeEntry({ slug: "first-owner", legacyId: "Rule13" }));

        expect(() =>
            registerRule(
                fakeEntry({ slug: "second-claimant", legacyId: "Rule13" }),
            ),
        ).toThrow(DuplicateRuleError);
    });

    it("does not throw registering two different rules with no legacyId", () => {
        registerRule(fakeEntry({ slug: "a" }));
        expect(() => registerRule(fakeEntry({ slug: "b" }))).not.toThrow();
    });
});

describe("isRuleScope", () => {
    it("accepts every documented scope", () => {
        expect(isRuleScope("library")).toBe(true);
        expect(isRuleScope("testing")).toBe(true);
        expect(isRuleScope("docs")).toBe(true);
    });

    it("rejects an arbitrary string", () => {
        expect(isRuleScope("not-a-scope")).toBe(false);
    });
});

describe("clearCatalog", () => {
    it("resets the catalog to empty", () => {
        registerRule(fakeEntry({ slug: "will-be-cleared" }));
        expect(getAllRules()).toHaveLength(1);

        clearCatalog();

        expect(getAllRules()).toEqual([]);
        expect(getRule("will-be-cleared")).toBeUndefined();
    });
});
