import { existsSync, readFileSync, readdirSync } from "node:fs";
import { dirname, join, relative, resolve } from "node:path";

import { registerRule } from "../catalog.js";
import { extractPathReferences } from "../parsing/markdown.js";
import type { Finding, Rule, RuleContext } from "../types.js";

/**
 * `doc-links` (legacy Rule11, plan §4.3/§8): every backticked path and
 * markdown link inside a tracked `.md` file, plus every script-path-shaped
 * string inside `.vscode/tasks.json`, must resolve to a real file. This is a
 * **hard CI gate at cutover** (plan §4.3: `gate: yes`), not a
 * baseline-eligible rule like most other P2 rules — see this rule's header
 * comment for why that still doesn't require zero false positives *today*
 * (P3.1's baseline ratchet absorbs current findings regardless of `gate`;
 * `gate` only controls whether `pineguard audit --gate` selects a rule, per
 * `engine.ts`'s `resolveSelection`).
 *
 * Deliberately widened versus legacy Rule11 (plan §2.4, §8): *any* extension
 * and *any* root, not only `.md`/`.ps1` under the docs tree — source-tree
 * references (`src/**`, `tests/**`, `.claude/**`, …) are in scope too. This
 * is expected to surface **more** real drift than the old tool, not less.
 *
 * ## Path resolution strategy
 *
 * For every extracted reference, two candidate bases are tried, in order:
 *
 * 1. **repo-root-relative** — `resolve(ctx.rootDir, path)`. Markdown links
 *    and backticked paths in this repo are conventionally written
 *    root-relative (`docs/ai/rules/must.md`, `src/PineGuard.Core/…`), and
 *    `.vscode/tasks.json`'s `./tools/…` arguments are resolved by VS Code
 *    against the workspace root.
 * 2. **directory-relative** — `resolve(ctx.rootDir, dirname(referencingFile),
 *    path)`, i.e. relative to the file that contains the reference. Some
 *    docs do use `./sibling.md`-style relative links.
 *
 * A reference is only reported when it resolves under *neither* base. See
 * `test/fixtures/doc-links/boundary/` for a fixture that only resolves via
 * base 2, proving both are actually attempted (not just the first).
 *
 * For each candidate absolute path, existence is checked two ways, and
 * either is sufficient:
 *
 * - `existsSync(absolutePath)` — plain filesystem existence. This is what
 *   makes fixtures work in isolation (see below) and is also a deliberate
 *   fallback for real runs: a target that exists on disk but is gitignored
 *   (e.g. a generated file that is still real and expected to exist) should
 *   not be flagged just because it isn't in the git index.
 * - `ctx.trackedFiles?.includes(repoRootRelativePath)` — membership in the
 *   engine's tracked-file list (`repo.ts`'s `listTrackedFiles`, git-index
 *   backed). This is the primary signal on a real `pineguard audit` run: a
 *   reference resolving only to *untracked* build output should still count
 *   as broken, which a pure `existsSync` check alone would miss.
 *
 * ## Fixture isolation (why no extra injectable option was needed)
 *
 * The task brief asked for a way to run fixtures without depending on the
 * real repo's tracked-file list. `RuleContext.trackedFiles` (declared in
 * `types.ts`) is already optional for exactly this reason — the test harness
 * (`test/support/runRule.ts`) builds a bare `{ rootDir: fixtureDir }` context
 * with no `trackedFiles` at all. This rule reads that as "no git index is
 * available here" and falls back to `existsSync` (for individual reference
 * resolution, above) and a plain recursive filesystem walk (for discovering
 * which `.md` files to scan in the first place, see `listMarkdownFiles`
 * below) instead of calling `listTrackedFiles`. No new constructor-style
 * option was introduced: the existing optional field already *is* the
 * injection point, and using it this way means a fixture's "valid" targets
 * are judged purely by what's on disk under the fixture directory, never by
 * whether the outer repo's git index happens to track them.
 *
 * ## Bare-prose paths (a deliberate widening beyond `extractPathReferences`)
 *
 * `extractPathReferences` only extracts markdown link targets and backticked
 * code spans (see `parsing/markdown.ts`) — used here unmodified, exactly as
 * instructed. But real, known drift in this repo (plan §2.2/§2.4: eight
 * package-level `AGENTS.md` files pointing at nonexistent per-package rules
 * files) is written as *plain prose*, with no backticks and no link syntax
 * at all — e.g. `src/PineGuard.Analyzers/AGENTS.md` reads literally "Read
 * docs/ai/rules/analyzers.md before …". `extractPathReferences` cannot see
 * this (by design — it is a link/code-span extractor, not a prose scanner),
 * so this rule adds a small, local, deliberately conservative supplementary
 * regex pass (`collectBareProseReferences`) that only fires on a path-shaped
 * token (contains `/`, ends in a short dotted extension) appearing outside
 * any fenced or inline code span. This mirrors the legacy Rule11 PowerShell
 * tool's own `Get-MarkdownBodyReference` bare-path regex (see
 * `tools/audit-cli/rules/Test-Rule11-DocLinks.ps1`), so it is a port of an
 * existing, working check rather than new scope invented here. Fenced code
 * blocks and inline code spans are blanked out (space-for-character, so line
 * numbers stay correct) before this pass runs, both to avoid re-flagging
 * illustrative code examples and — critically — to avoid re-extracting a
 * `` `+ planned/path` `` reference from inside its own backticks, which would
 * bypass the `planned` tag `extractPathReferences` already computed for it.
 */

const TASKS_JSON_RELATIVE = ".vscode/tasks.json";
const ARCHIVED_PLANS_PREFIX = "docs/ai/plans/completed/";
const EXCLUDED_WALK_DIRS = new Set([
    "node_modules",
    ".git",
    "dist",
    "bin",
    "obj",
]);

/** Glob/placeholder punctuation that never appears in a literal file path. */
const PLACEHOLDER_PUNCTUATION = /[*?<>{}[\]|$%"'`]/;

const BARE_PATH_PATTERN =
    /(?<![\w./-])((?:[A-Za-z0-9_.-]+\/)+[A-Za-z0-9_.-]+\.[A-Za-z0-9]{1,10})(?![\w])/g;

function isSkippableReference(raw: string): boolean {
    const trimmed = raw.trim();
    if (trimmed.length === 0) return true;
    if (trimmed.startsWith("#")) return true;
    if (trimmed.startsWith("/")) return true;
    if (trimmed.startsWith("~")) return true;
    if (trimmed.startsWith("@")) return true; // npm-scoped package name (e.g. @pineguard/cli), not a path
    if (trimmed.includes("://")) return true;
    if (trimmed.startsWith("mailto:") || trimmed.startsWith("tel:"))
        return true;
    return false;
}

/** Drops a `#fragment` and/or `?query` suffix, mirroring the legacy tool's own normalisation. */
function stripFragmentAndQuery(raw: string): string {
    const withoutHash = raw.split("#")[0] ?? "";
    const withoutQuery = withoutHash.split("?")[0] ?? "";
    return withoutQuery;
}

/** Illustrative placeholder text (`path/to/x`, `PineGuard.X`, `xxx`, glob syntax, `...`) that is never a real reference. */
function looksLikePlaceholder(path: string): boolean {
    if (PLACEHOLDER_PUNCTUATION.test(path)) return true;
    if (/\s/.test(path)) return true;
    if (path.includes("...")) return true;
    if (/(^|\/)path\/to(\/|$)/i.test(path)) return true;
    if (/(^|\/)PineGuard\.X(\.|\/|$)/i.test(path)) return true;
    if (/(xxx|yyy|zzz|nnn|placeholder)/i.test(path)) return true;
    return false;
}

/**
 * Does `path` look like a genuine, checkable file reference? This is the
 * rule's actual "any extension, any root" widening (plan §8) — rather than
 * legacy Rule11's hardcoded allowlist of known top-level directories (which
 * is exactly what made it blind to `apps/cli` — plan §2.4's "skips
 * references rooted under the source tree"), any first path segment is
 * accepted, so a brand-new top-level tree needs no allowlist update.
 *
 * The trade-off, found empirically by sanity-checking against this repo's
 * own `docs/ai/plans/audit-cli-rebuild.md` (which mentions dozens of *bare*
 * filenames — `unit-test.md`, `vocabulary.json`, `catalog.ts`, `ci.yml` —
 * and directory/glob fragments — `valid/`, `Core/Rules`, `tools/**`,
 * `*Tests.cs`, `src/PineGuard.MustClauses/*.cs` — in backticks purely as
 * illustration, not as literal paths to resolve): a candidate is only
 * checkable when it (a) contains a `/` (a bare filename with no directory
 * is *not* checkable — this matches legacy Rule11's own body-text behaviour,
 * which only allows bare filenames in front-matter, a case this rule does
 * not scan) and (b) ends in a real extension (`\.[A-Za-z0-9]{1,10}$`), which
 * excludes directory-only mentions (`valid/`, `Codes/`) and namespace/glob
 * fragments (`Core/Rules`, `tools/**`, `src/PineGuard.MustClauses/*.cs` —
 * the last is additionally caught by {@link looksLikePlaceholder}'s glob
 * check). A bare filename ending in a recognised extension but with no `/`
 * at all (`README.md` mentioned on its own) is deliberately still excluded:
 * it is indistinguishable, by shape alone, from the dozens of bare
 * `unit-test.md`/`vocabulary.json`-style illustrative mentions above.
 *
 * One further guard: the trailing "extension" must contain at least one
 * letter. Without it, a dual-TFM mention like `net8.0/net10.0` or a GitHub
 * Action pin like `JetBrains/qodana-action@v2026.2` reads as a path ending
 * in the "extension" `.0`/`.2` — no real file in this repo ever ends in a
 * purely numeric extension, so this costs nothing against genuine paths.
 */
function looksLikeFileReference(path: string): boolean {
    if (!path.includes("/")) return false;
    const match = /\.([A-Za-z0-9]{1,10})$/.exec(path);
    const extension = match?.[1];
    return extension !== undefined && /[A-Za-z]/.test(extension);
}

/** Blanks fenced (```) and inline (`) code spans to spaces, preserving length/line numbers, so the bare-prose pass never re-scans code. */
function stripCodeSpans(markdown: string): string {
    const noFences = markdown.replace(/```[\s\S]*?```/g, (block) =>
        block.replaceAll(/[^\n]/g, " "),
    );
    return noFences.replace(/`[^`\n]*`/g, (span) => " ".repeat(span.length));
}

interface BareProseReference {
    readonly path: string;
    readonly line: number;
}

/** See the "Bare-prose paths" section of this file's header comment. */
function collectBareProseReferences(markdown: string): BareProseReference[] {
    const prose = stripCodeSpans(markdown);
    const refs: BareProseReference[] = [];

    for (const match of prose.matchAll(BARE_PATH_PATTERN)) {
        const value = match[1];
        if (value === undefined) continue;
        const line = prose.slice(0, match.index).split("\n").length;
        refs.push({ path: value, line });
    }

    return refs;
}

function walkForMarkdownFiles(rootDir: string): string[] {
    const results: string[] = [];
    const stack: string[] = [rootDir];

    while (stack.length > 0) {
        const dir = stack.pop();
        if (dir === undefined) continue;

        let entries;
        try {
            entries = readdirSync(dir, { withFileTypes: true });
        } catch {
            continue;
        }

        for (const entry of entries) {
            if (entry.isDirectory()) {
                if (!EXCLUDED_WALK_DIRS.has(entry.name)) {
                    stack.push(join(dir, entry.name));
                }
                continue;
            }
            if (entry.isFile() && entry.name.endsWith(".md")) {
                results.push(
                    relative(rootDir, join(dir, entry.name)).replaceAll(
                        "\\",
                        "/",
                    ),
                );
            }
        }
    }

    return results;
}

/** See the "Fixture isolation" section of this file's header comment. */
function listMarkdownFiles(ctx: RuleContext): string[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles.filter((file) => file.endsWith(".md"));
    }
    return walkForMarkdownFiles(ctx.rootDir);
}

function findTasksJson(ctx: RuleContext): string | undefined {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles.includes(TASKS_JSON_RELATIVE)
            ? TASKS_JSON_RELATIVE
            : undefined;
    }
    return existsSync(join(ctx.rootDir, TASKS_JSON_RELATIVE))
        ? TASKS_JSON_RELATIVE
        : undefined;
}

/**
 * Walks a parsed `.vscode/tasks.json` document (any shape — `label`,
 * `command`, `args`, `dependsOn`, …) and pulls out path-shaped tokens from
 * every string value. Splitting each string on whitespace before testing
 * handles both a plain array element (`"./tools/testing/Run-Tests.ps1"`) and
 * a compound `"-Command"` string (`"dotnet test ./PineGuard.slnx -c Debug"`)
 * with one pass, without needing to special-case either shape by key name.
 */
function collectTasksJsonReferences(raw: string): string[] {
    let parsed: unknown;
    try {
        parsed = JSON.parse(raw);
    } catch {
        return [];
    }

    const refs: string[] = [];
    const seen = new Set<string>();

    const visit = (value: unknown): void => {
        if (typeof value === "string") {
            for (const token of value.split(/\s+/)) {
                if (looksLikeTasksJsonPath(token) && !seen.has(token)) {
                    seen.add(token);
                    refs.push(token);
                }
            }
            return;
        }
        if (Array.isArray(value)) {
            for (const item of value) visit(item);
            return;
        }
        if (value !== null && typeof value === "object") {
            for (const entry of Object.values(value)) visit(entry);
        }
    };

    visit(parsed);
    return refs;
}

function looksLikeTasksJsonPath(token: string): boolean {
    const cleaned = token.trim();
    if (cleaned.length === 0 || !cleaned.includes("/")) return false;
    return /\.[A-Za-z0-9]{1,10}$/.test(cleaned);
}

/**
 * Tries both resolution bases (see the header comment's "Path resolution
 * strategy") and both existence checks for each. Returns `true` as soon as
 * any combination resolves.
 */
function referenceResolves(
    ctx: RuleContext,
    referencingFile: string,
    refPath: string,
): boolean {
    const bases = [ctx.rootDir, join(ctx.rootDir, dirname(referencingFile))];

    for (const base of bases) {
        const absolute = resolve(base, refPath);
        if (existsSync(absolute)) return true;

        if (ctx.trackedFiles) {
            const relativeToRoot = relative(ctx.rootDir, absolute).replaceAll(
                "\\",
                "/",
            );
            if (
                !relativeToRoot.startsWith("..") &&
                ctx.trackedFiles.includes(relativeToRoot)
            ) {
                return true;
            }
        }
    }

    return false;
}

function makeFinding(
    file: string,
    refPath: string,
    line: number | undefined,
): Finding {
    return {
        rule: "doc-links",
        file,
        line,
        message:
            `Reference "${refPath}" does not resolve to a tracked or existing file ` +
            `(tried repo-root-relative and "${file}"-directory-relative resolution).`,
        key: `doc-links:${file}:${refPath}`,
    };
}

function checkFileReferences(
    ctx: RuleContext,
    file: string,
    text: string,
): Finding[] {
    const findings: Finding[] = [];
    const seen = new Set<string>();

    // `seen` is populated for *every* extracted reference — including ones
    // skipped as planned/placeholder/unresolved-on-purpose — before any
    // skip check runs. This is what stops the bare-prose pass (below) from
    // re-evaluating a path that a `+ path` code span already tagged
    // `planned` and chose not to check: the marker-stripped path is added to
    // `seen` regardless of the `planned` skip, so the later, marker-blind
    // bare-prose match of the same text is dropped as a duplicate instead of
    // being independently (and wrongly) flagged. See this file's "Bare-prose
    // paths" header comment.
    const evaluate = (rawPath: string, line: number | undefined): void => {
        const stripped = stripFragmentAndQuery(rawPath).trim();
        if (seen.has(stripped)) return;
        seen.add(stripped);

        if (isSkippableReference(stripped)) return;
        if (!looksLikeFileReference(stripped)) return;
        if (looksLikePlaceholder(stripped)) return;

        if (!referenceResolves(ctx, file, stripped)) {
            findings.push(makeFinding(file, stripped, line));
        }
    };

    for (const ref of extractPathReferences(text)) {
        if (ref.planned) {
            // Still record it in `seen` (via the marker-stripped path) so a
            // later bare-prose match of the same literal text is deduped,
            // not independently flagged — see the comment above `evaluate`.
            seen.add(stripFragmentAndQuery(ref.path).trim());
            continue;
        }
        evaluate(ref.path, ref.line);
    }

    for (const bare of collectBareProseReferences(text)) {
        evaluate(bare.path, bare.line);
    }

    return findings;
}

function collectDocLinksFindings(ctx: RuleContext): Finding[] {
    const findings: Finding[] = [];

    const markdownFiles = listMarkdownFiles(ctx).filter(
        (file) => !file.startsWith(ARCHIVED_PLANS_PREFIX),
    );

    for (const file of markdownFiles) {
        let text: string;
        try {
            text = readFileSync(join(ctx.rootDir, file), "utf8");
        } catch {
            continue;
        }
        findings.push(...checkFileReferences(ctx, file, text));
    }

    const tasksJson = findTasksJson(ctx);
    if (tasksJson !== undefined) {
        let raw: string | undefined;
        try {
            raw = readFileSync(join(ctx.rootDir, tasksJson), "utf8");
        } catch {
            raw = undefined;
        }

        if (raw !== undefined) {
            const seen = new Set<string>();
            for (const candidate of collectTasksJsonReferences(raw)) {
                const stripped = stripFragmentAndQuery(candidate).trim();
                if (seen.has(stripped)) continue;
                seen.add(stripped);

                if (isSkippableReference(stripped)) continue;
                if (looksLikePlaceholder(stripped)) continue;

                if (!referenceResolves(ctx, tasksJson, stripped)) {
                    findings.push(makeFinding(tasksJson, stripped, undefined));
                }
            }
        }
    }

    return findings;
}

/** The bare `Rule` object, exported so tests can run it directly against fixtures (see `test/README.md`). */
export const docLinksRule: Rule = {
    slug: "doc-links",
    run(ctx: RuleContext): Finding[] {
        return collectDocLinksFindings(ctx);
    },
};

registerRule({
    ...docLinksRule,
    legacyId: "Rule11",
    scope: "docs",
    gate: true,
    description:
        "Every backticked path, markdown link, and .vscode/tasks.json script path in tracked docs resolves to a real file.",
});
