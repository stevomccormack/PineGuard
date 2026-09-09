import { mkdtempSync, rmSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";

import { describe, expect, it } from "vitest";

import {
    RepoRootNotFoundError,
    findRepoRoot,
    listTrackedFiles,
    readRepoFile,
} from "../../src/audit/repo.js";

describe("findRepoRoot", () => {
    it("finds a repo root walking up from process.cwd()", () => {
        const root = findRepoRoot();
        expect(root.length).toBeGreaterThan(0);
    });

    it("resolves the same root walking up from a deeply nested directory inside the repo", () => {
        const fromDefault = findRepoRoot();
        const nested = join(
            fromDefault,
            "apps",
            "cli",
            "src",
            "audit",
            "parsing",
        );

        expect(findRepoRoot(nested)).toBe(fromDefault);
    });

    it("throws RepoRootNotFoundError when no .git ancestor exists", () => {
        const outside = mkdtempSync(
            join(tmpdir(), "pineguard-repo-root-test-"),
        );

        try {
            expect(() => findRepoRoot(outside)).toThrow(RepoRootNotFoundError);
        } finally {
            rmSync(outside, { recursive: true, force: true });
        }
    });
});

describe("listTrackedFiles", () => {
    it("returns real tracked files from this repo, including the plan driving this task", () => {
        const files = listTrackedFiles();

        expect(files).toContain("docs/ai/plans/audit-cli-rebuild.md");
    });

    it("normalises every entry to forward slashes", () => {
        const files = listTrackedFiles();

        expect(files.length).toBeGreaterThan(0);
        expect(files.every((file) => !file.includes("\\"))).toBe(true);
    });

    // Regression test: the old PowerShell audit tool's Rule10 walked the
    // filesystem from a fixed root and got its counts inflated by nested
    // `.claude/worktrees/` checkouts sitting on disk (docs/ai/plans/
    // audit-cli-rebuild.md §2.2/§2.4). We are, right now, running from
    // inside exactly such a checkout — so if `listTrackedFiles` resolved
    // the wrong repo root (e.g. the outer main checkout instead of this
    // worktree) or ever fell back to a directory walk, this is where it
    // would show up: paths belonging to a nested worktree checkout.
    it("never returns a path under .claude/worktrees/, unlike the old filesystem-walking Rule10", () => {
        const files = listTrackedFiles();

        expect(files.some((file) => file.includes(".claude/worktrees/"))).toBe(
            false,
        );
    });

    it("narrows results when given pathspec patterns", () => {
        const files = listTrackedFiles(["apps/cli/package.json"]);

        expect(files).toEqual(["apps/cli/package.json"]);
    });
});

describe("readRepoFile", () => {
    it("reads a tracked file's contents by repo-relative path", () => {
        const content = readRepoFile("apps/cli/package.json");
        const parsed: unknown = JSON.parse(content);

        expect(parsed).toMatchObject({ name: "@pineguard/cli" });
    });
});
