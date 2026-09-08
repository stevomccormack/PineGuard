import { describe, expect, it } from "vitest";

import {
    extractFrontMatter,
    extractPathReferences,
} from "../../../src/audit/parsing/markdown.js";

describe("extractFrontMatter", () => {
    it("parses a simple leading front-matter block into a plain object", () => {
        const markdown = [
            "---",
            "type: plan",
            "id: audit-cli-rebuild",
            "version: 1",
            "---",
            "",
            "# Heading",
        ].join("\n");

        expect(extractFrontMatter(markdown)).toEqual({
            type: "plan",
            id: "audit-cli-rebuild",
            version: 1,
        });
    });

    it("parses nested maps and lists (real shape used by docs/ai/meta/template-coverage.md)", () => {
        const markdown = [
            "---",
            "spec:",
            "  id: pineguard.ai.templates.coverage",
            "  parent:",
            "    - ../specs/testing/coverage.md",
            "applies_to:",
            '  - "docs/ai/specs/**/coverage.md"',
            "---",
            "",
            "# Body",
        ].join("\n");

        expect(extractFrontMatter(markdown)).toEqual({
            spec: {
                id: "pineguard.ai.templates.coverage",
                parent: ["../specs/testing/coverage.md"],
            },
            applies_to: ["docs/ai/specs/**/coverage.md"],
        });
    });

    it("returns an empty object when there is no front matter", () => {
        expect(extractFrontMatter("# Just a heading\n\nSome text.")).toEqual(
            {},
        );
    });

    it("returns an empty object for an empty front-matter block", () => {
        expect(extractFrontMatter("---\n---\n\n# Heading")).toEqual({});
    });

    it("does not treat a later --- thematic break as front matter", () => {
        const markdown = "# Heading\n\nSome text.\n\n---\n\nMore text.";

        expect(extractFrontMatter(markdown)).toEqual({});
    });
});

describe("extractPathReferences", () => {
    const markdown = [
        "See the [plan](docs/ai/plans/audit-cli-rebuild.md) for details.",
        "",
        "Read `apps/cli/package.json` for the dependency list.",
        "",
        "The engine will also live at `+ apps/cli/src/audit/engine.ts` once P1.5 lands.",
        "",
        "This is not a path: `IsValid`.",
    ].join("\n");

    const refs = extractPathReferences(markdown);

    it("extracts a markdown link target as not planned", () => {
        expect(refs).toContainEqual(
            expect.objectContaining({
                path: "docs/ai/plans/audit-cli-rebuild.md",
                planned: false,
                kind: "link",
            }),
        );
    });

    it("extracts a plain backticked path as not planned", () => {
        expect(refs).toContainEqual(
            expect.objectContaining({
                path: "apps/cli/package.json",
                planned: false,
                kind: "code",
            }),
        );
    });

    it("tags a `+ path` code span as planned and strips the marker", () => {
        expect(refs).toContainEqual(
            expect.objectContaining({
                path: "apps/cli/src/audit/engine.ts",
                planned: true,
                kind: "code",
            }),
        );
    });

    it("does not treat a bare identifier code span as a path reference", () => {
        expect(refs.some((ref) => ref.path === "IsValid")).toBe(false);
    });

    it("records source line numbers for each reference", () => {
        expect(refs.every((ref) => typeof ref.line === "number")).toBe(true);
    });
});
