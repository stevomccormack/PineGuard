import { readdirSync, readFileSync, statSync } from "node:fs";
import { dirname, join, relative, resolve } from "node:path";

import { registerRule } from "../catalog.js";
import { findTypeDeclarations, parseFile } from "../parsing/csharp.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `test-orphans` (legacy `Rule53`) — plan §4.3, §8, §2.2, §2.4.
 *
 * Every `FooTests.cs` under `tests/<Pkg>.UnitTests/…` must refer to a real
 * `class|record|struct|interface Foo` declared somewhere in `<Pkg>`'s own
 * source project (`src/<Pkg>`) **or any project that test project
 * references** — resolved from the test project's own `.csproj`
 * (`<ProjectReference Include="…">` items), not assumed from the package
 * name alone.
 *
 * ## Why this rule exists, and what it fixes
 *
 * The old PowerShell `Rule53` (`tools/audit-cli/rules/Test-Rule53-TestOrphans.ps1`)
 * used a pure filename heuristic: build a set of source file *basenames*
 * (plus a "strip the dots" normalised form for `Xxx.Yyy.cs` partial-file
 * splits) from `src/<Pkg>` only, then check whether `FooTests.cs`'s
 * `Foo` appears in that set. Two problems fall out of that:
 *
 * 1. **False positives** (plan §2.2/§2.4): a type that moved to a
 *    *different* project (`MustResult` now lives in Core, not MustClauses),
 *    a partial-class split whose file names don't happen to normalise to
 *    the type name (`Baz.Core.cs`/`Baz.Formatting.cs` for `class Baz`, where
 *    dot-stripping gives `BazCore`/`BazFormatting`, neither of which is
 *    `Baz`), and cross-project consumer tests (`MustResultTests` living in
 *    `MustClauses.UnitTests` because that's where the *usage* is tested,
 *    even though the type itself is declared in `Core`).
 * 2. **Real drift the old rule did genuinely catch** and this rewrite must
 *    keep catching: `MustStringNumberClausesTests` vs. the actual
 *    `MustStringNumbersClauses.cs` (singular/plural mismatch — no fuzzy or
 *    plural-tolerant matching here, on purpose), and
 *    `FluentDateExtensionsTests` whose subject moved to
 *    `FluentSqlDateTimeExtensions.cs` without a rename anywhere.
 *
 * The fix (plan §8) is to stop pattern-matching *filenames* altogether and
 * instead parse every `.cs` file in the *candidate project set* — `src/<Pkg>`
 * plus every project resolved from `<ProjectReference>` — for its actual
 * `class`/`record`/`struct`/`interface` declarations (`findTypeDeclarations`,
 * added to `parsing/csharp.ts` for this rule; it generalises the
 * record-only `findRecordDeclarations` the same module already had), and
 * check whether `Foo` is declared *anywhere* in that set. A type declared
 * across multiple partial files is a hit the moment any one of those files'
 * declarations matches by name — no merging of the parts is needed, and no
 * assumption is made about which file (if any) is named after the type.
 *
 * ## Exceptions
 *
 * `ctx.exceptions["test-orphans"]` (plan §7 "Exceptions application",
 * `apps/cli/config/exceptions.json`) is a list of substrings matched against
 * the offending test file's path — the direct equivalent of the old rule's
 * `AllowOrphanTest` allowlist (`tools/audit-cli/test-audit-exceptions.json`).
 * The engine (`src/audit/engine.ts`'s `applyExceptions`) already applies the
 * same substring check generically to every rule's findings after `run()`
 * returns, so this is redundant with that generic mechanism in a real
 * `pineguard audit` run — applying it here too is deliberate belt-and-braces
 * (plan §8 calls it out explicitly for this rule) and is a no-op the second
 * time it runs, never a double-exclusion bug: filtering an already-filtered
 * list by the same predicate changes nothing.
 */

const SLUG = "test-orphans";

/** Matches a path segment `tests/<pkg>.UnitTests/`, capturing `<pkg>`. */
const TEST_PROJECT_RE = /(?:^|\/)tests\/([^/]+)\.UnitTests\//;

function isBuildOutput(path: string): boolean {
    return path.includes("/bin/") || path.includes("/obj/");
}

/** Recursively lists every file under `dir`, as absolute paths. */
function* walk(dir: string): Generator<string> {
    for (const entry of readdirSync(dir)) {
        const full = join(dir, entry);
        if (statSync(full).isDirectory()) {
            yield* walk(full);
        } else {
            yield full;
        }
    }
}

/**
 * Every file under `ctx.rootDir`, forward-slash-normalised and relative to
 * it. Prefers `ctx.trackedFiles` (the real engine's `git ls-files` listing)
 * when present; falls back to walking the filesystem directly for the test
 * harness, which only ever populates `rootDir` (see `test/support/runRule.ts`
 * and the `_demo-harness` rule in `test/support/runRule.test.ts` for the same
 * pattern) — a fixture tree is not itself a committed, self-contained repo,
 * so `git ls-files` is not the right tool for it even though `rootDir` sits
 * inside this repo's own working tree.
 */
function listAllFiles(ctx: RuleContext): string[] {
    if (ctx.trackedFiles) {
        return [...ctx.trackedFiles];
    }
    return [...walk(ctx.rootDir)].map((absolutePath) =>
        relative(ctx.rootDir, absolutePath).replaceAll("\\", "/"),
    );
}

interface TestProject {
    readonly pkg: string;
    readonly projectDir: string;
    readonly testFiles: string[];
}

/** Groups every `*Tests.cs` file under a `tests/<pkg>.UnitTests/` tree by package. */
function discoverTestProjects(
    files: readonly string[],
): Map<string, TestProject> {
    const projects = new Map<string, TestProject>();
    for (const file of files) {
        if (isBuildOutput(file)) continue;
        const match = TEST_PROJECT_RE.exec(file);
        if (!match) continue;
        const pkg = match[1];
        if (pkg === undefined) continue;

        let project = projects.get(pkg);
        project ??= {
            pkg,
            projectDir: `tests/${pkg}.UnitTests`,
            testFiles: [],
        };
        projects.set(pkg, project);

        const fileName = file.slice(file.lastIndexOf("/") + 1);
        if (fileName.endsWith("Tests.cs")) {
            project.testFiles.push(file);
        }
    }
    return projects;
}

/** Finds the one `.csproj` sitting directly inside `projectDir` (not nested further), if any. */
function findProjectCsproj(
    files: readonly string[],
    projectDir: string,
): string | undefined {
    const prefix = `${projectDir}/`;
    return files.find((file) => {
        if (!file.startsWith(prefix) || !file.endsWith(".csproj")) return false;
        return !file.slice(prefix.length).includes("/");
    });
}

const PROJECT_REFERENCE_RE =
    /<ProjectReference\b[^>]*\bInclude\s*=\s*"([^"]+)"/gi;

/**
 * Reads `<ProjectReference Include="…">` items out of a `.csproj`'s raw
 * text and resolves each one to a project directory, repo-root-relative and
 * forward-slash-normalised. A small regex over the raw XML rather than a
 * full XML parser — `apps/cli` has no XML dependency, and MSBuild project
 * references are always a single, unnested `Include="…"` attribute, so a
 * regex is not fighting the grammar the way it would for arbitrary XML.
 */
function resolveProjectReferences(
    csprojAbsolutePath: string,
    csprojContent: string,
    rootDir: string,
): string[] {
    const dirs: string[] = [];
    const re = new RegExp(PROJECT_REFERENCE_RE);
    let match: RegExpExecArray | null;
    while ((match = re.exec(csprojContent)) !== null) {
        const include = match[1];
        if (include === undefined) continue;
        const resolvedCsprojPath = resolve(
            dirname(csprojAbsolutePath),
            include.replaceAll("\\", "/"),
        );
        const relativeCsprojPath = relative(
            rootDir,
            resolvedCsprojPath,
        ).replaceAll("\\", "/");
        const lastSlash = relativeCsprojPath.lastIndexOf("/");
        if (lastSlash === -1) continue;
        dirs.push(relativeCsprojPath.slice(0, lastSlash));
    }
    return dirs;
}

/**
 * Parses every `.cs` file under any of `sourceDirs` and returns the set of
 * `class`/`record`/`struct`/`interface` names declared anywhere among them.
 * A type split across partial files is a member of this set the moment any
 * one of its declaring files is parsed — the caller never needs to know how
 * many files, or which one, contributed the match.
 */
async function collectDeclaredTypeNames(
    files: readonly string[],
    sourceDirs: readonly string[],
    rootDir: string,
): Promise<Set<string>> {
    const prefixes = sourceDirs.map((dir) => `${dir}/`);
    const candidateFiles = files.filter(
        (file) =>
            file.endsWith(".cs") &&
            !isBuildOutput(file) &&
            prefixes.some((prefix) => file.startsWith(prefix)),
    );

    const names = new Set<string>();
    for (const relativePath of candidateFiles) {
        const parsed = await parseFile(join(rootDir, relativePath));
        for (const declaration of findTypeDeclarations(parsed.root)) {
            if (declaration.name.length > 0) {
                names.add(declaration.name);
            }
        }
    }
    return names;
}

registerRule({
    slug: SLUG,
    legacyId: "Rule53",
    scope: "testing",
    gate: false,
    description:
        "Every *Tests.cs file's subject type is declared as a class/record/struct/interface in its package's source project or a project it references.",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const files = listAllFiles(ctx);
        const projects = discoverTestProjects(files);
        const exceptionPatterns = ctx.exceptions?.[SLUG] ?? [];
        const findings: Finding[] = [];

        for (const project of projects.values()) {
            const sourceDirs = new Set<string>([`src/${project.pkg}`]);

            const csprojRelativePath = findProjectCsproj(
                files,
                project.projectDir,
            );
            if (csprojRelativePath !== undefined) {
                const csprojAbsolutePath = join(
                    ctx.rootDir,
                    csprojRelativePath,
                );
                const csprojContent = readFileSync(csprojAbsolutePath, "utf8");
                for (const dir of resolveProjectReferences(
                    csprojAbsolutePath,
                    csprojContent,
                    ctx.rootDir,
                )) {
                    sourceDirs.add(dir);
                }
            }

            const declaredNames = await collectDeclaredTypeNames(
                files,
                [...sourceDirs],
                ctx.rootDir,
            );

            for (const testFile of project.testFiles) {
                const fileName = testFile.slice(testFile.lastIndexOf("/") + 1);
                const baseName = fileName.endsWith(".cs")
                    ? fileName.slice(0, -".cs".length)
                    : fileName;
                if (!baseName.endsWith("Tests")) continue;
                const targetType = baseName.slice(0, -"Tests".length);
                if (targetType.length === 0) continue;
                if (declaredNames.has(targetType)) continue;
                if (
                    exceptionPatterns.some((pattern) =>
                        testFile.includes(pattern),
                    )
                ) {
                    continue;
                }

                findings.push({
                    rule: SLUG,
                    file: testFile,
                    message:
                        `'${testFile}' refers to type '${targetType}', which is not declared ` +
                        `as a class/record/struct/interface in '${project.pkg}' (src/${project.pkg}) ` +
                        `or any project '${project.pkg}.UnitTests' references`,
                    key: `${SLUG}:${testFile}:${targetType}`,
                });
            }
        }

        return findings;
    },
});
