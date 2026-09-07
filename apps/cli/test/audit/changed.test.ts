import { spawnSync } from "node:child_process";
import { mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { tmpdir } from "node:os";
import { join } from "node:path";

import { afterEach, describe, expect, it } from "vitest";

import { buildContext } from "../../src/audit/engine.js";
import {
    findRepoRoot,
    listChangedFiles,
    listTrackedFiles,
} from "../../src/audit/repo.js";

/**
 * Proves `--changed`'s (plan §4.2, P3.2) file-list computation in isolation:
 * ref resolution (`main` first, `origin/main` fallback, a clean "neither
 * resolved" outcome), the merge-base comparison point (excludes upstream-only
 * drift on `main`, includes uncommitted working-tree changes), and the
 * `buildContext` intersection that is the actual propagation mechanism into
 * `ctx.trackedFiles` — see `engine.ts`'s `BuildContextOptions.changedFiles`
 * doc comment for exactly which rules that reaches.
 *
 * `listChangedFiles` tests use a small, real, disposable git repo per test
 * (mirroring `repo.test.ts`'s own "walk a real filesystem, not a mock" style)
 * rather than mocking `child_process`, so the actual `git` ref-resolution and
 * `merge-base` behaviour is what gets exercised.
 */

function git(args: readonly string[], cwd: string): string {
    const result = spawnSync("git", args, { cwd, encoding: "utf8" });
    if (result.status !== 0) {
        throw new Error(
            `"git ${args.join(" ")}" failed in "${cwd}":\n${result.stderr}`,
        );
    }
    return result.stdout;
}

function commitFile(dir: string, relativePath: string, content: string): void {
    writeFileSync(join(dir, relativePath), content);
    git(["add", relativePath], dir);
    git(["commit", "--quiet", "-m", `commit ${relativePath}`], dir);
}

/** A fresh, disposable git repo with one commit on a branch literally named `main` and a clean working tree. */
function initRepoOnMain(): string {
    const dir = mkdtempSync(join(tmpdir(), "pineguard-changed-test-"));
    git(["init", "--quiet"], dir);
    // Force the branch name regardless of this machine's init.defaultBranch
    // config — deterministic across CI runners and local machines alike.
    git(["symbolic-ref", "HEAD", "refs/heads/main"], dir);
    git(["config", "user.email", "test@example.com"], dir);
    git(["config", "user.name", "Test"], dir);
    commitFile(dir, "base.txt", "base\n");
    return dir;
}

let tempDirs: string[] = [];

function tempRepo(init: () => string): string {
    const dir = init();
    tempDirs.push(dir);
    return dir;
}

afterEach(() => {
    for (const dir of tempDirs) {
        rmSync(dir, { recursive: true, force: true });
    }
    tempDirs = [];
});

describe("listChangedFiles", () => {
    it("resolves the local main branch and reports an empty diff when nothing differs from it", () => {
        const dir = tempRepo(initRepoOnMain);

        const result = listChangedFiles(dir);

        expect(result.baseRef).toBe("main");
        expect(result.warning).toBeUndefined();
        expect(result.files).toEqual([]);
    });

    it("includes uncommitted working-tree changes, not just committed diffs", () => {
        const dir = tempRepo(initRepoOnMain);
        writeFileSync(join(dir, "base.txt"), "modified, not committed\n");

        const result = listChangedFiles(dir);

        expect(result.baseRef).toBe("main");
        expect(result.files).toEqual(["base.txt"]);
    });

    it("includes a new file committed on this branch since it forked from main", () => {
        const dir = tempRepo(initRepoOnMain);
        git(["checkout", "--quiet", "-b", "feature"], dir);
        commitFile(dir, "feature.txt", "feature work\n");

        const result = listChangedFiles(dir);

        expect(result.baseRef).toBe("main");
        expect(result.files).toEqual(["feature.txt"]);
    });

    it("excludes files that changed on main after this branch forked (merge-base, not a plain diff against main's tip)", () => {
        const dir = tempRepo(initRepoOnMain);
        git(["checkout", "--quiet", "-b", "feature"], dir);
        commitFile(dir, "feature.txt", "feature work\n");

        git(["checkout", "--quiet", "main"], dir);
        commitFile(dir, "mainonly.txt", "main moved on without this branch\n");

        git(["checkout", "--quiet", "feature"], dir);
        const result = listChangedFiles(dir);

        expect(result.baseRef).toBe("main");
        expect(result.files).toContain("feature.txt");
        expect(result.files).not.toContain("mainonly.txt");
    });

    it("falls back to origin/main when there is no local main branch", () => {
        const dir = mkdtempSync(join(tmpdir(), "pineguard-changed-test-"));
        tempDirs.push(dir);
        git(["init", "--quiet"], dir);
        git(["symbolic-ref", "HEAD", "refs/heads/trunk"], dir);
        git(["config", "user.email", "test@example.com"], dir);
        git(["config", "user.name", "Test"], dir);
        commitFile(dir, "base.txt", "base\n");
        // Simulate a remote-tracking ref without needing a real remote.
        const headSha = git(["rev-parse", "HEAD"], dir).trim();
        git(["update-ref", "refs/remotes/origin/main", headSha], dir);

        commitFile(dir, "trunk-work.txt", "trunk work\n");

        const result = listChangedFiles(dir);

        expect(result.baseRef).toBe("origin/main");
        expect(result.files).toEqual(["trunk-work.txt"]);
    });

    it("returns an empty file list with a warning, not a crash, when neither main nor origin/main resolves", () => {
        const dir = mkdtempSync(join(tmpdir(), "pineguard-changed-test-"));
        tempDirs.push(dir);
        git(["init", "--quiet"], dir);
        git(["symbolic-ref", "HEAD", "refs/heads/trunk"], dir);
        git(["config", "user.email", "test@example.com"], dir);
        git(["config", "user.name", "Test"], dir);
        commitFile(dir, "base.txt", "base\n");

        const result = listChangedFiles(dir);

        expect(result.baseRef).toBeUndefined();
        expect(result.files).toEqual([]);
        expect(result.warning).toMatch(/neither "main" nor "origin\/main"/);
    });
});

describe("buildContext's --changed propagation (BuildContextOptions.changedFiles)", () => {
    it("leaves trackedFiles as the full git ls-files listing when changedFiles is omitted", () => {
        const root = findRepoRoot();
        const allTracked = listTrackedFiles(undefined, root);

        const ctx = buildContext(root);

        expect(ctx.trackedFiles).toEqual(allTracked);
    });

    it("narrows trackedFiles to the intersection of git ls-files and the given changedFiles list", () => {
        const root = findRepoRoot();
        const allTracked = listTrackedFiles(undefined, root);
        const realFile = allTracked[0];
        expect(realFile).toBeDefined();

        const ctx = buildContext(root, {
            changedFiles: [
                realFile as string,
                "definitely/not/a/real/tracked/file.txt",
            ],
        });

        expect(ctx.trackedFiles).toEqual([realFile]);
    });

    it("produces an empty trackedFiles list, not a crash, for an empty changedFiles array", () => {
        const root = findRepoRoot();

        const ctx = buildContext(root, { changedFiles: [] });

        expect(ctx.trackedFiles).toEqual([]);
    });
});
