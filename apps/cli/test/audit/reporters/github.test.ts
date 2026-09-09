import { describe, expect, it } from "vitest";

import type { CatalogEntry } from "../../../src/audit/catalog.js";
import type { AuditRunResult } from "../../../src/audit/engine.js";
import { renderGithub } from "../../../src/audit/reporters/github.js";
import type { Finding } from "../../../src/audit/types.js";

/**
 * Proves the P3.2 `github` reporter (plan §4.2/§7, §9.2 P3.2): the exact
 * workflow-command annotation format, the `gate` → `error`/`warning` level
 * mapping, the `,line=...` omission for a line-less finding, and property
 * escaping — same style as `reporters/sarif.test.ts` (plain fakes, no VIBE
 * fixture harness; a reporter is engine-level plumbing, not a rule).
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

function findingFor(
    rule: string,
    file: string,
    message: string,
    line?: number,
): Finding {
    return {
        rule,
        file,
        message,
        line,
        key: `${rule}:${file}:${String(line)}:${message}`,
    };
}

describe("renderGithub", () => {
    it("renders nothing for a run with no outcomes", () => {
        const result: AuditRunResult = { exitCode: 0, outcomes: [] };

        expect(renderGithub(result)).toBe("");
    });

    it("renders nothing for a rule that ran clean (zero findings)", () => {
        const entry = fakeEntry({ slug: "clean-rule", gate: true });
        const result: AuditRunResult = {
            exitCode: 0,
            outcomes: [{ entry, findings: [] }],
        };

        expect(renderGithub(result)).toBe("");
    });

    it("emits ::error for a gate:true rule's finding, with file/line/title and the message", () => {
        const entry = fakeEntry({ slug: "gate-rule", gate: true });
        const finding = findingFor(
            "gate-rule",
            "src/Foo.cs",
            "Foo has no caller",
            42,
        );
        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [{ entry, findings: [finding] }],
        };

        expect(renderGithub(result)).toBe(
            "::error file=src/Foo.cs,line=42,title=gate-rule::Foo has no caller",
        );
    });

    it("emits ::warning for a gate:false rule's finding", () => {
        const entry = fakeEntry({ slug: "warn-rule", gate: false });
        const finding = findingFor(
            "warn-rule",
            "src/Bar.cs",
            "Bar is misordered",
            7,
        );
        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [{ entry, findings: [finding] }],
        };

        expect(renderGithub(result)).toBe(
            "::warning file=src/Bar.cs,line=7,title=warn-rule::Bar is misordered",
        );
    });

    it("omits the line property when the finding has no line number", () => {
        const entry = fakeEntry({ slug: "r", gate: true });
        const finding = findingFor("r", "src/NoLine.cs", "no line here");
        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [{ entry, findings: [finding] }],
        };

        expect(renderGithub(result)).toBe(
            "::error file=src/NoLine.cs,title=r::no line here",
        );
    });

    it("emits one line per finding, across multiple rules, in outcome order", () => {
        const gateEntry = fakeEntry({ slug: "gate-rule", gate: true });
        const warnEntry = fakeEntry({ slug: "warn-rule", gate: false });
        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [
                {
                    entry: gateEntry,
                    findings: [findingFor("gate-rule", "src/A.cs", "a", 1)],
                },
                {
                    entry: warnEntry,
                    findings: [
                        findingFor("warn-rule", "src/B.cs", "b", 2),
                        findingFor("warn-rule", "src/C.cs", "c", 3),
                    ],
                },
            ],
        };

        expect(renderGithub(result)).toBe(
            [
                "::error file=src/A.cs,line=1,title=gate-rule::a",
                "::warning file=src/B.cs,line=2,title=warn-rule::b",
                "::warning file=src/C.cs,line=3,title=warn-rule::c",
            ].join("\n"),
        );
    });

    it("percent-escapes %, CR, and LF in the message, and additionally : and , in file/title properties", () => {
        const entry = fakeEntry({ slug: "r:i,d", gate: true });
        const finding = findingFor(
            "r:i,d",
            "src/A,B:C.cs",
            "100% done\r\nnext line",
        );
        const result: AuditRunResult = {
            exitCode: 1,
            outcomes: [{ entry, findings: [finding] }],
        };

        expect(renderGithub(result)).toBe(
            "::error file=src/A%2CB%3AC.cs,title=r%3Ai%2Cd::100%25 done%0D%0Anext line",
        );
    });
});
