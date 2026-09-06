import { describe, expect, it } from "vitest";

import type { CatalogEntry } from "../../../src/audit/catalog.js";
import type { AuditRunResult } from "../../../src/audit/engine.js";
import {
    buildSarifLog,
    renderSarif,
} from "../../../src/audit/reporters/sarif.js";
import type { Finding } from "../../../src/audit/types.js";

/**
 * Proves the P3.3 `sarif` reporter (plan §4.2/§7, §9.2 P3.3): a valid SARIF
 * 2.1.0 document, the right shape/counts/fields, and the `gate` → level
 * mapping this reporter is built around — same style as `baseline.test.ts`
 * (plain fakes, no VIBE fixture harness; a reporter is engine-level plumbing,
 * not a rule).
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

describe("buildSarifLog", () => {
    it("produces the expected top-level SARIF shape", () => {
        const result: AuditRunResult = {
            exitCode: 0,
            outcomes: [],
        };

        const log = buildSarifLog(result);

        expect(log.$schema).toBe(
            "https://json.schemastore.org/sarif-2.1.0.json",
        );
        expect(log.version).toBe("2.1.0");
        expect(log.runs).toHaveLength(1);
        expect(log.runs[0]?.tool.driver.name).toBe("pineguard");
        expect(log.runs[0]?.tool.driver.rules).toEqual([]);
        expect(log.runs[0]?.results).toEqual([]);
    });

    it("emits one rules[] entry per selected rule and one results[] entry per finding, with the right fields", () => {
        const gateFinding = findingFor(
            "gate-rule",
            "src/Foo.cs",
            "Foo has no caller",
        );
        const warnFinding1 = findingFor(
            "warn-rule",
            "src/Bar.cs",
            "Bar is misordered",
        );
        const warnFinding2 = findingFor(
            "warn-rule",
            "src/Baz.cs",
            "Baz is misordered",
        );

        const gateEntry = fakeEntry({
            slug: "gate-rule",
            gate: true,
            description: "Every gate-rule thing must have a caller.",
        });
        const warnEntry = fakeEntry({
            slug: "warn-rule",
            gate: false,
            description: "Methods should be ordered consistently.",
        });

        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [
                { entry: gateEntry, findings: [gateFinding] },
                { entry: warnEntry, findings: [warnFinding1, warnFinding2] },
            ],
        };

        const log = buildSarifLog(result);
        const run = log.runs[0];

        expect(run?.tool.driver.rules).toEqual([
            {
                id: "gate-rule",
                shortDescription: {
                    text: "Every gate-rule thing must have a caller.",
                },
                defaultConfiguration: { level: "error" },
            },
            {
                id: "warn-rule",
                shortDescription: {
                    text: "Methods should be ordered consistently.",
                },
                defaultConfiguration: { level: "warning" },
            },
        ]);

        expect(run?.results).toHaveLength(3);
        expect(run?.results[0]).toEqual({
            ruleId: "gate-rule",
            level: "error",
            message: { text: "Foo has no caller" },
            locations: [
                {
                    physicalLocation: {
                        artifactLocation: { uri: "src/Foo.cs" },
                    },
                },
            ],
        });
        expect(run?.results[1]).toEqual({
            ruleId: "warn-rule",
            level: "warning",
            message: { text: "Bar is misordered" },
            locations: [
                {
                    physicalLocation: {
                        artifactLocation: { uri: "src/Bar.cs" },
                    },
                },
            ],
        });
    });

    it("includes a rules[] entry for a rule that ran clean (zero findings), so the run stays self-describing", () => {
        const cleanEntry = fakeEntry({ slug: "clean-rule", gate: true });
        const result: AuditRunResult = {
            exitCode: 0,
            outcomes: [{ entry: cleanEntry, findings: [] }],
        };

        const log = buildSarifLog(result);

        expect(log.runs[0]?.tool.driver.rules).toEqual([
            {
                id: "clean-rule",
                shortDescription: {
                    text: "A fake rule, used only by this test file.",
                },
                defaultConfiguration: { level: "error" },
            },
        ]);
        expect(log.runs[0]?.results).toEqual([]);
    });

    it("adds a region.startLine only when the finding has a line number", () => {
        const withLine: Finding = {
            rule: "r",
            file: "src/A.cs",
            line: 42,
            message: "has a line",
            key: "r:src/A.cs:has a line",
        };
        const withoutLine: Finding = {
            rule: "r",
            file: "src/B.cs",
            message: "no line",
            key: "r:src/B.cs:no line",
        };
        const entry = fakeEntry({ slug: "r" });
        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [{ entry, findings: [withLine, withoutLine] }],
        };

        const log = buildSarifLog(result);
        const [first, second] = log.runs[0]?.results ?? [];

        expect(first?.locations[0]?.physicalLocation.region).toEqual({
            startLine: 42,
        });
        expect(second?.locations[0]?.physicalLocation.region).toBeUndefined();
    });
});

describe("renderSarif", () => {
    it("renders valid, parseable JSON matching buildSarifLog's output", () => {
        const entry = fakeEntry({ slug: "gate-rule", gate: true });
        const finding = findingFor("gate-rule", "src/Foo.cs", "bad thing");
        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [{ entry, findings: [finding] }],
        };

        const text = renderSarif(result);
        const parsed: unknown = JSON.parse(text);

        expect(parsed).toEqual(buildSarifLog(result));
    });
});
