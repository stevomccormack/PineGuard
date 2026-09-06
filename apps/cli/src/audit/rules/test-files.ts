import { existsSync, readFileSync, readdirSync, statSync } from "node:fs";
import { basename, dirname, join, relative } from "node:path";

import { registerRule, type CatalogEntry } from "../catalog.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `test-files` — faithful, exact-parity port of the legacy PowerShell tool's
 * `Rule50` (`tools/audit-cli/rules/Test-Rule50-UnitTestFileStructureNormalization.ps1`).
 *
 * This is **the one rule that currently gates CI** (plan
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3, §9.2 P2.10) — the mandate here is
 * parity, not improvement. See that PowerShell file and
 * `tools/audit-cli/test-audit-exceptions.json` for the source of truth this
 * file was ported from; every check below is annotated with the exact legacy
 * line/behaviour it reproduces.
 *
 * Two independent checks, both scoped to files physically on disk under
 * `<rootDir>/tests/**`, excluding any path with a `bin` or `obj` path
 * segment (legacy `Test-IsInScopeTestsFile`):
 *
 * 1. **Pairing** — every `*Tests.cs` has a sibling `*TestData.cs` in the same
 *    directory (`[MissingTestData]`), and every `*TestData.cs` has a sibling
 *    `*Tests.cs` (`[OrphanTestData]`). The sibling name is derived exactly as
 *    the legacy script does: strip the `.cs` extension, replace a trailing
 *    `Tests`/`TestData` (anchored at the end of the base name, per the
 *    legacy `-replace 'Tests$', 'TestData'` / `-replace 'TestData$', 'Tests'`)
 *    with the other suffix, and re-append `.cs`. Sibling existence is a
 *    **physical filesystem check** (legacy `Test-Path`), not a
 *    git-tracked-files check — deliberately: the legacy tool walks
 *    `Get-ChildItem -Recurse` over the physical `tests/` tree, so an
 *    untracked scratch file would still satisfy (or fail to satisfy) pairing
 *    exactly as it does today. This rule mirrors that by walking
 *    `<rootDir>/tests` with `node:fs` directly rather than consulting
 *    `ctx.trackedFiles`.
 * 2. **Theory-only** — a `*Tests.cs` file containing `[Fact]` or
 *    `[Fact(...)]` is flagged (`[FactNotAllowed]`). The match is the legacy
 *    regex verbatim, `/\[\s*Fact(\s*\(|\s*\])/` (PowerShell's `(?s)` modifier
 *    on the original only affects `.`, which the pattern never uses, and
 *    JavaScript's `\s` already matches newlines, so no flags are needed for
 *    parity) — this is a **known, intentionally-preserved** blind spot: it
 *    does not catch the verbose `[FactAttribute]` alias, and does not catch
 *    `[Fact, Trait(...)]` multi-attribute-in-one-bracket forms. Both are
 *    ported as-is (see `test/fixtures/test-files/boundary/`), not "fixed" —
 *    per this task's brief, an unexplained behaviour delta here is a bigger
 *    problem than a faithfully-reproduced legacy gap.
 *
 * **Exceptions**: the legacy tool reads three per-key allowlists from
 * `tools/audit-cli/test-audit-exceptions.json` (`Rule50.AllowMissingTestData`,
 * `Rule50.AllowOrphanTestData`, `Rule50.AllowFact`), each an exact,
 * case-insensitive repo-relative path list. This rule does **not**
 * reimplement that lookup itself — per `catalog.ts`'s "adding a new rule"
 * recipe and `engine.ts`'s own doc comment, exception suppression is an
 * **engine-level** concern applied uniformly to every rule's raw findings
 * after `run()` returns (`applyExceptions`, keyed by rule slug against
 * `apps/cli/config/exceptions.json`). Porting the legacy JSON's actual two
 * live entries into that config file is P5's cascade job, not this rule's;
 * `test/fixtures/test-files/boundary/` demonstrates the mechanism (a
 * genuinely pre-existing, allowlist-worthy orphan, exactly like the real
 * repo's `ErrorMessageAttributesTests.cs` case) using the engine's existing
 * `applyExceptions` directly, without duplicating per-key parsing here.
 */

const FACT_ATTRIBUTE_PATTERN = /\[\s*Fact(\s*\(|\s*\])/;
const BIN_OR_OBJ_SEGMENT = /(^|\/)(bin|obj)(\/|$)/i;

function toForwardSlash(path: string): string {
    return path.replaceAll("\\", "/");
}

/** Recursively lists every file under `dir`. Returns `[]` if `dir` doesn't exist (mirrors an empty/absent `tests/` tree gracefully; the real repo always has one). */
function* walkFiles(dir: string): Generator<string> {
    let entries: string[];
    try {
        entries = readdirSync(dir);
    } catch {
        return;
    }
    for (const entry of entries) {
        const full = join(dir, entry);
        const stats = statSync(full);
        if (stats.isDirectory()) {
            yield* walkFiles(full);
        } else if (stats.isFile()) {
            yield full;
        }
    }
}

/** Strips a trailing `.cs` extension from a filename (legacy `.BaseName`). */
function baseNameNoExt(filePath: string): string {
    return basename(filePath, ".cs");
}

function findMissingTestDataFindings(
    testsFiles: readonly string[],
    rootDir: string,
): Finding[] {
    const findings: Finding[] = [];
    for (const file of testsFiles) {
        const relFromRoot = toForwardSlash(relative(rootDir, file));
        if (BIN_OR_OBJ_SEGMENT.test(relFromRoot)) {
            continue;
        }

        const expectedBase = baseNameNoExt(file).replace(/Tests$/, "TestData");
        const expectedPath = join(dirname(file), `${expectedBase}.cs`);
        const expectedRel = toForwardSlash(relative(rootDir, expectedPath));

        if (!existsSync(expectedPath)) {
            findings.push({
                rule: "test-files",
                file: relFromRoot,
                message: `[MissingTestData] ${relFromRoot} is missing sibling ${expectedRel}`,
                key: `test-files:missing-test-data:${relFromRoot}`,
            });
        }

        const text = readFileSync(file, "utf8");
        if (FACT_ATTRIBUTE_PATTERN.test(text)) {
            findings.push({
                rule: "test-files",
                file: relFromRoot,
                message: `[FactNotAllowed] ${relFromRoot} contains [Fact]; only [Theory] is allowed`,
                key: `test-files:fact-not-allowed:${relFromRoot}`,
            });
        }
    }
    return findings;
}

function findOrphanTestDataFindings(
    testDataFiles: readonly string[],
    rootDir: string,
): Finding[] {
    const findings: Finding[] = [];
    for (const file of testDataFiles) {
        const relFromRoot = toForwardSlash(relative(rootDir, file));
        if (BIN_OR_OBJ_SEGMENT.test(relFromRoot)) {
            continue;
        }

        const expectedBase = baseNameNoExt(file).replace(/TestData$/, "Tests");
        const expectedPath = join(dirname(file), `${expectedBase}.cs`);
        const expectedRel = toForwardSlash(relative(rootDir, expectedPath));

        if (!existsSync(expectedPath)) {
            findings.push({
                rule: "test-files",
                file: relFromRoot,
                message: `[OrphanTestData] ${relFromRoot} is missing sibling ${expectedRel}`,
                key: `test-files:orphan-test-data:${relFromRoot}`,
            });
        }
    }
    return findings;
}

export function runTestFiles(ctx: RuleContext): Finding[] {
    const testsRoot = join(ctx.rootDir, "tests");
    const allFiles = [...walkFiles(testsRoot)];

    const testsFiles = allFiles.filter((f) => basename(f).endsWith("Tests.cs"));
    const testDataFiles = allFiles.filter((f) =>
        basename(f).endsWith("TestData.cs"),
    );

    const findings = [
        ...findMissingTestDataFindings(testsFiles, ctx.rootDir),
        ...findOrphanTestDataFindings(testDataFiles, ctx.rootDir),
    ];

    // Legacy script sorts findings before writing its report (`$findings |
    // Sort-Object`); mirrored here for deterministic, diffable output.
    findings.sort((a, b) => a.message.localeCompare(b.message));
    return findings;
}

export const testFilesRule: CatalogEntry = {
    slug: "test-files",
    legacyId: "Rule50",
    scope: "testing",
    gate: true,
    description:
        "Every *Tests.cs has a paired *TestData.cs (and vice versa); *Tests.cs files use [Theory] only, never [Fact].",
    run: runTestFiles,
};

registerRule(testFilesRule);
