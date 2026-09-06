import { spawnSync } from "node:child_process";
import { existsSync, readFileSync } from "node:fs";
import { dirname, join, resolve } from "node:path";

/**
 * Thrown by {@link findRepoRoot} when no `.git` entry can be found walking
 * up from the given starting path.
 */
export class RepoRootNotFoundError extends Error {
    public constructor(startPath: string) {
        super(
            `Could not find a git repository root walking up from "${startPath}" ` +
                `(no .git entry found in any ancestor directory).`,
        );
        this.name = "RepoRootNotFoundError";
    }
}

/**
 * Walk up from `startPath` (default: `process.cwd()`) looking for a `.git`
 * entry and return the first directory that has one.
 *
 * A `.git` entry is a directory in a normal checkout, but a *file* in a
 * git worktree checkout (it contains a `gitdir:` pointer back to the main
 * repo's `.git/worktrees/<name>`) — this repo's own
 * `.claude/worktrees/<name>` checkouts are exactly that shape, so both
 * forms count.
 *
 * Resolving the root this way — walking up from wherever we are actually
 * running, rather than assuming a fixed location — is what makes
 * {@link listTrackedFiles} immune to the bug that shipped in the old
 * PowerShell audit tool's Rule10: that rule walked the filesystem from a
 * hardcoded root and got its counts inflated by nested
 * `.claude/worktrees/` checkouts sitting on disk. `git ls-files`, scoped
 * to whichever repo root we resolve to here, only ever reports what
 * *that* repo's index tracks.
 */
export function findRepoRoot(startPath: string = process.cwd()): string {
    let dir = resolve(startPath);

    for (;;) {
        if (existsSync(join(dir, ".git"))) {
            return dir;
        }

        const parent = dirname(dir);
        if (parent === dir) {
            throw new RepoRootNotFoundError(startPath);
        }

        dir = parent;
    }
}

/**
 * List every file `git` tracks in the repository containing `startPath`
 * (default: the repo containing `process.cwd()`), normalised to
 * forward-slash paths relative to the repo root.
 *
 * Backed by `git ls-files` — never a filesystem walk — precisely so it
 * cannot be polluted by nested worktree checkouts, build output, or any
 * other untracked clutter sitting on disk inside the repo.
 *
 * @param patterns optional pathspecs / globs passed straight through to
 *   `git ls-files` (e.g. `["*.md"]`); omit to list every tracked file.
 * @param startPath passed through to {@link findRepoRoot} to resolve the
 *   repo root `git ls-files` runs against.
 */
export function listTrackedFiles(
    patterns?: readonly string[],
    startPath?: string,
): string[] {
    const root = findRepoRoot(startPath);
    const args = ["ls-files", "--", ...(patterns ?? [])];
    const result = spawnSync("git", args, {
        cwd: root,
        encoding: "utf8",
        maxBuffer: 1024 * 1024 * 64,
    });

    if (result.error) {
        throw new Error(
            `Failed to spawn "git ${args.join(" ")}" in "${root}": ${result.error.message}`,
        );
    }

    if (result.status !== 0) {
        throw new Error(
            `"git ${args.join(" ")}" exited ${String(result.status)} in "${root}":\n${result.stderr}`,
        );
    }

    return result.stdout
        .split(/\r?\n/)
        .filter((entry) => entry.length > 0)
        .map((entry) => entry.replaceAll("\\", "/"));
}

/**
 * Read the UTF-8 contents of a file given a path relative to the repo
 * root (forward slashes, as returned by {@link listTrackedFiles}).
 */
export function readRepoFile(relativePath: string, startPath?: string): string {
    const root = findRepoRoot(startPath);
    return readFileSync(join(root, relativePath), "utf8");
}
