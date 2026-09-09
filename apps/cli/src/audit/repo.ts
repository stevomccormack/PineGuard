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

/** Result of {@link listChangedFiles}. */
export interface ChangedFilesResult {
    /**
     * Files that differ from the resolved base ref, forward-slash-normalised
     * and relative to the repo root. Empty (with `baseRef` undefined) when no
     * base ref could be resolved at all.
     */
    files: string[];
    /** The ref actually used as the comparison base: `"main"` or `"origin/main"`. Undefined when neither resolved. */
    baseRef?: string;
    /** Set alongside an undefined `baseRef`, explaining why — surfaced by the engine as a non-fatal warning (plan §4.2 `--changed`: "fast pre-commit feedback, not a hard CI mechanism"). */
    warning?: string;
}

function gitRefExists(ref: string, cwd: string): boolean {
    const result = spawnSync("git", ["rev-parse", "--verify", "--quiet", ref], {
        cwd,
        encoding: "utf8",
    });
    return result.status === 0;
}

/**
 * Lists files that differ between a base ref and the current working tree,
 * for `pineguard audit --changed` (plan §4.2, P3.2).
 *
 * ## Ref resolution
 * Tries the local `main` branch first, then the `origin/main` remote-tracking
 * ref; if neither resolves (e.g. a fresh clone whose default branch is not
 * `main`, or a repo with no `origin` remote), this returns
 * `{ files: [], warning: "..." }` rather than throwing — `--changed` is a
 * convenience for fast local feedback, not a mechanism CI depends on, so a
 * repo shaped unusually should not hard-fail an otherwise-working audit run;
 * the caller decides how loudly to surface `warning`.
 *
 * ## Comparison point
 * Diffs from `git merge-base <baseRef> HEAD` to the **working tree**
 * (`git diff --name-only <merge-base>`, the two-dot form with no second
 * ref, which compares directly against what is on disk right now) rather
 * than either of the two options named in this task's brief on their own:
 * - `git diff --name-only <baseRef>` (two-dot, straight against `baseRef`'s
 *   tip) would also report files that changed on `main` itself after this
 *   branch forked from it, even though this branch never touched them —
 *   wrong for "what have I changed".
 * - `git diff --name-only <baseRef>...HEAD` (three-dot) already excludes
 *   that upstream drift by diffing from the merge-base, but a bare
 *   three-dot range diffs merge-base→HEAD only — it does not see anything
 *   staged or unstaged in the working tree, which is most of the point of a
 *   fast pre-commit check.
 *
 * Computing the merge-base explicitly and then doing a plain two-dot diff
 * against it gets both properties at once: no upstream-drift noise, and
 * uncommitted changes included. If `git merge-base` itself fails (e.g.
 * unrelated histories in an unusual checkout), this falls back to diffing
 * directly against `baseRef`.
 */
export function listChangedFiles(startPath?: string): ChangedFilesResult {
    const root = findRepoRoot(startPath);

    let baseRef: string | undefined;
    if (gitRefExists("refs/heads/main", root)) {
        baseRef = "main";
    } else if (gitRefExists("refs/remotes/origin/main", root)) {
        baseRef = "origin/main";
    }

    if (baseRef === undefined) {
        return {
            files: [],
            warning:
                'pineguard audit --changed: neither "main" nor "origin/main" could be resolved as a base ref in this checkout; running without --changed\'s file-scope narrowing.',
        };
    }

    const mergeBase = spawnSync("git", ["merge-base", baseRef, "HEAD"], {
        cwd: root,
        encoding: "utf8",
    });
    const comparisonPoint =
        mergeBase.status === 0 ? mergeBase.stdout.trim() : baseRef;

    const args = ["diff", "--name-only", comparisonPoint];
    const diff = spawnSync("git", args, {
        cwd: root,
        encoding: "utf8",
        maxBuffer: 1024 * 1024 * 64,
    });

    if (diff.error) {
        throw new Error(
            `Failed to spawn "git ${args.join(" ")}" in "${root}": ${diff.error.message}`,
        );
    }
    if (diff.status !== 0) {
        throw new Error(
            `"git ${args.join(" ")}" exited ${String(diff.status)} in "${root}":\n${diff.stderr}`,
        );
    }

    const files = diff.stdout
        .split(/\r?\n/)
        .filter((entry) => entry.length > 0)
        .map((entry) => entry.replaceAll("\\", "/"));

    return { files, baseRef };
}
