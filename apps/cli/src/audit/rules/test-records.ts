import { readdirSync } from "node:fs";
import { join, relative } from "node:path";

import { registerRule } from "../catalog.js";
import {
    findRecordDeclarations,
    parseFile as parseCsharpFile,
} from "../parsing/csharp.js";
import type { RecordDeclarationInfo } from "../parsing/csharp.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `test-records` — the rebuild of the legacy tool's Rule52 (plan
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3, §8, §2.4, P2.12).
 *
 * ## The old rule, and the bug this rewrite fixes
 *
 * `tools/audit-cli/rules/Test-Rule52-UnitTestCaseRecordConventions.ps1`
 * line-scanned every `*TestData.cs` file with one blanket rule: a
 * `ValidCase` record must have *some* base clause, and that base must not be
 * `BaseCase`/`ValueCase<>` directly. That rule was applied identically
 * everywhere in `tests/**`, regardless of which layer the file belonged to.
 *
 * Two independent problems follow from that:
 *
 * 1. **Not every layer allows a custom `*Case` record at all.** The
 *    per-layer addenda (`docs/ai/specs/{must-clauses,guard-clauses,
 *    fluent-validation,data-annotations}/unit-test.md`) are explicit that
 *    Must/Guard/Fluent/DataAnnotations tests use the shared per-layer case
 *    type (`MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`,
 *    `DataAnnotationCase`) *directly* — a locally-declared case record is
 *    forbidden outright, independent of what it inherits from. Fluent's
 *    addendum spells this out under "Explicit Prohibitions": "No custom case
 *    records: only `FluentCase<TValue>`. Never define `ValidCase`,
 *    `NullCase`, `Args`, or any other local record extending
 *    `ReturnCase<T, bool>`." DataAnnotations' addendum has an identical
 *    "Prohibited Patterns" entry. A record with a textbook-correct base
 *    (`ValidCase : ReturnCase<DateOnly, bool>(...)`) satisfied the old rule's
 *    "has a real base" check while still violating these two layers' actual
 *    convention — real, current drift this rule now catches (see
 *    `test/fixtures/test-records/invalid/tests/PineGuard.FluentValidation.UnitTests/`).
 * 2. **Not every record in a `*TestData.cs` file is a case record at all.**
 *    TestData files also declare plain value-object/domain-model records
 *    that get used as a case's `Value` (e.g. real repo precedent:
 *    `GuardValidatorClausesTestData.cs`'s `Widget`,
 *    `RuleBuilderExtensionTestData.cs`'s `Address`/`Order`,
 *    `MustValidatorTestData.cs`'s `FailureExpectation`/`ItemsHolder`) or
 *    other auxiliary shapes. None of these are named with the `Case` suffix
 *    and none of them need — or are ever documented anywhere as needing — a
 *    base type; they carry domain data, not scenario metadata, and are never
 *    instantiated into a `TheoryData<T>` row. The base-record convention
 *    exists only for the records that *are* per-scenario row types, and
 *    every real example of that in the repo (and every one shown in
 *    `docs/ai/specs/testing/unit-test.md` §4.2/§8, and each layer addendum)
 *    is named with the exact suffix `Case` — `Case`, `ValidCase`,
 *    `InvalidCase`, `InvalidActionCase`, DataAnnotations'
 *    `ActionThrowsCase`. This rule flags only records matching that suffix;
 *    a `Widget`/`Order`/`FailureExpectation`-shaped record is never even
 *    considered, in any layer. This is the direct fix for "conflates
 *    positional records and inheritance forms" (plan §2.2): the old
 *    line-scanner had no way to reliably tell a tuple-typed parameter's
 *    closing paren from a base clause's opening one, or a multi-line base
 *    clause from a plain positional record spanning the same number of
 *    lines; `findRecordDeclarations`'s `base_list` extraction is AST-exact
 *    about both.
 *
 * ## Where the legitimate "no base at all" shape is actually documented
 *
 * The plan's review notes (§2.4) describe the root spec's own example as "a
 * bare `record ValidCase(string Name, …)`" used legitimately by 82 TestData
 * files. Read directly, `unit-test.md` §4.2's "Standard Definitions" and
 * every one of its full canonical examples (§8.2's `FooRulesTestData.Parse`)
 * *do* inherit `ReturnCase<,>`/`ThrowsCase<>` — as does every layer
 * addendum's "Case Type" section (`docs/ai/specs/fixture.md` §3's six sealed
 * case types are all `ReturnCase<,>`/`ThrowExpected`-derived too). None of
 * the five authoritative specs shows a `*Case`-suffixed record with
 * literally no base as sanctioned. What *is* real, current, and repeated
 * across dozens of TestData files repo-wide is the value-object/auxiliary
 * shape described in point 2 above — a record that is not a `*Case` at all.
 * That is the legitimate "bare record" convention this rule encodes: the
 * fault line is the `Case` name suffix (which is also, empirically, a
 * distinction the old rule already drew correctly via its own regex scope —
 * its real bugs were point 1 above, and the line-scanning fragility AST
 * parsing now removes), not "is a `TestData.cs` file" in general.
 *
 * ## Per-layer policy (plan §8: "Scope by test project, not repo-wide")
 *
 * - **`PineGuard.Testing.UnitTests`** (the meta project that tests the
 *   shared `BaseCase`/`ValueCase<>` infrastructure itself, e.g.
 *   `BaseCasesTestData.cs`) is exempt entirely — inheriting those types
 *   directly is exactly what it is there to test.
 * - **Must / Guard / Fluent / DataAnnotations**: any `*Case`-suffixed record
 *   locally declared in that layer's TestData is a finding — the addendum
 *   requires the shared per-layer case type used directly, full stop. The
 *   one documented exception is DataAnnotations' `ActionThrowsCase` (its
 *   addendum's "Pattern E — TypeMismatch Throws"), which is explicitly
 *   sanctioned as a `private sealed record ... : ThrowsCase<Action>(...)`
 *   declared inside a `*TypeMismatch` Op Group.
 * - **Core** and **"(Other)" packages** (`docs/ai/specs/testing/unit-test.md`
 *   §2.1 — a package outside the five layers, e.g. `PineGuard.AspNetCore`):
 *   a `*Case`-suffixed record is legitimate (Core's "Value/Result Tests
 *   (e.g. Converters, Parsers)" per §4.2, and the "(Other)" convention's own
 *   project-local `XxxCase : ReturnCase<,>`), but it must actually inherit
 *   `ReturnCase<,>`/`ThrowsCase<>`/one of the shared layer case types — never
 *   bare, and never `BaseCase`/`ValueCase<>` directly (those skip the
 *   `Expected`-carrying shape the assertion helpers rely on).
 */

type Layer =
    | "core"
    | "must"
    | "guard"
    | "fluent"
    | "data-annotations"
    | "other"
    | "testing-infra";

const LAYER_BY_PROJECT: Readonly<Record<string, Layer>> = {
    "PineGuard.Core.UnitTests": "core",
    "PineGuard.MustClauses.UnitTests": "must",
    "PineGuard.GuardClauses.UnitTests": "guard",
    "PineGuard.FluentValidation.UnitTests": "fluent",
    "PineGuard.DataAnnotations.UnitTests": "data-annotations",
    "PineGuard.Testing.UnitTests": "testing-infra",
};

/** Layers whose addendum forbids a locally-declared `*Case` record outright, regardless of what it inherits from. */
const FORBIDS_CUSTOM_CASE_RECORDS: ReadonlySet<Layer> = new Set([
    "must",
    "guard",
    "fluent",
    "data-annotations",
]);

interface LayerInfo {
    readonly label: string;
    readonly caseType: string;
    readonly addendum: string;
}

const LAYER_INFO: Readonly<Record<Layer, LayerInfo>> = {
    core: {
        label: "Core",
        caseType: "RuleCase<T>",
        addendum: "docs/ai/specs/core/unit-test.md",
    },
    must: {
        label: "Must",
        caseType: "MustCase<T>",
        addendum: "docs/ai/specs/must-clauses/unit-test.md",
    },
    guard: {
        label: "Guard",
        caseType: "GuardCase<T>",
        addendum: "docs/ai/specs/guard-clauses/unit-test.md",
    },
    fluent: {
        label: "Fluent",
        caseType: "FluentCase<T>",
        addendum: "docs/ai/specs/fluent-validation/unit-test.md",
    },
    "data-annotations": {
        label: "DataAnnotations",
        caseType: "DataAnnotationCase",
        addendum: "docs/ai/specs/data-annotations/unit-test.md",
    },
    other: {
        label: "(Other)",
        caseType: "a project-local XxxCase : ReturnCase<,>",
        addendum: "docs/ai/specs/testing/unit-test.md §2.1",
    },
    "testing-infra": {
        label: "PineGuard.Testing (infra)",
        caseType: "n/a",
        addendum: "n/a",
    },
};

/** DataAnnotations' one documented exception (its addendum's "Pattern E — TypeMismatch Throws"). */
const DA_TYPE_MISMATCH_EXCEPTION_NAME = "ActionThrowsCase";
const DA_TYPE_MISMATCH_EXCEPTION_BASE = "ThrowsCase<Action>";

/** Bases that bypass the `Expected`-carrying shape entirely — never a valid direct base for a case record. */
const RAW_INFRASTRUCTURE_BASES: ReadonlySet<string> = new Set([
    "BaseCase",
    "ValueCase",
]);

function detectLayer(relativePath: string): Layer | null {
    const projectSegment = relativePath
        .split("/")
        .find((segment) => segment.endsWith(".UnitTests"));
    if (projectSegment === undefined) {
        return null;
    }
    return LAYER_BY_PROJECT[projectSegment] ?? "other";
}

/** Reads the leading type name off a `record_declaration`'s raw `base_list` text (e.g. `": ReturnCase<string, bool>(Name, Value, Expected)"` -> `"ReturnCase"`). `null` when there is no base clause at all. */
function extractBaseTypeName(baseListText: string | null): string | null {
    if (baseListText === null) {
        return null;
    }
    const withoutLeadingColon = baseListText.replace(/^\s*:\s*/, "");
    const match = /^([A-Za-z_][A-Za-z0-9_]*)/.exec(withoutLeadingColon);
    return match?.[1] ?? null;
}

function isDocumentedException(
    layer: Layer,
    record: RecordDeclarationInfo,
): boolean {
    return (
        layer === "data-annotations" &&
        record.name === DA_TYPE_MISMATCH_EXCEPTION_NAME &&
        (record.baseListText?.includes(DA_TYPE_MISMATCH_EXCEPTION_BASE) ??
            false)
    );
}

interface CandidateFile {
    readonly absolutePath: string;
    readonly relativePath: string;
}

/** Recursively lists every `*.cs` file under `dir`, skipping `obj`/`bin` build output. */
function* walkCsFiles(dir: string): Generator<string> {
    for (const entry of readdirSync(dir, { withFileTypes: true })) {
        if (entry.name === "obj" || entry.name === "bin") {
            continue;
        }
        const full = join(dir, entry.name);
        if (entry.isDirectory()) {
            yield* walkCsFiles(full);
        } else if (entry.name.endsWith(".cs")) {
            yield full;
        }
    }
}

/**
 * Resolves the `*TestData.cs` files this rule scans: every tracked file
 * under `tests/` ending in `TestData.cs` on a real `pineguard audit` run
 * (`ctx.trackedFiles`), or every such file under `ctx.rootDir` when running
 * against a fixture tree (the test harness in `test/support/runRule.ts`
 * never populates `trackedFiles`).
 */
function collectTestDataFiles(ctx: RuleContext): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles
            .filter(
                (file) =>
                    file.startsWith("tests/") && file.endsWith("TestData.cs"),
            )
            .map((file) => ({
                absolutePath: join(ctx.rootDir, file),
                relativePath: file,
            }));
    }

    return [...walkCsFiles(ctx.rootDir)]
        .filter((absolutePath) => absolutePath.endsWith("TestData.cs"))
        .map((absolutePath) => ({
            absolutePath,
            relativePath: relative(ctx.rootDir, absolutePath).replaceAll(
                "\\",
                "/",
            ),
        }));
}

registerRule({
    slug: "test-records",
    legacyId: "Rule52",
    scope: "testing",
    gate: false,
    description:
        "Every layer's *Case test-data records follow that layer's own base-record convention (root spec §4.2 + the per-layer addenda) instead of one blanket repo-wide rule.",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const parse = ctx.parseFile ?? parseCsharpFile;
        const files = collectTestDataFiles(ctx);
        const findings: Finding[] = [];

        for (const file of files) {
            const layer = detectLayer(file.relativePath);
            if (layer === null || layer === "testing-infra") {
                continue;
            }

            const info = LAYER_INFO[layer];
            const { root } = await parse(file.absolutePath);

            for (const record of findRecordDeclarations(root)) {
                if (!record.name.endsWith("Case")) {
                    continue;
                }
                if (isDocumentedException(layer, record)) {
                    continue;
                }

                const line = record.node.startPosition.row + 1;

                if (FORBIDS_CUSTOM_CASE_RECORDS.has(layer)) {
                    findings.push({
                        rule: "test-records",
                        file: file.relativePath,
                        line,
                        message:
                            `${info.label} tests must use the shared ${info.caseType} directly (${info.addendum}) — ` +
                            `"${record.name}" is a locally-declared case record, which this layer's addendum ` +
                            `explicitly forbids regardless of what it inherits from.`,
                        key: `test-records:forbidden:${file.relativePath}:${record.name}`,
                    });
                    continue;
                }

                const baseTypeName = extractBaseTypeName(record.baseListText);
                if (baseTypeName === null) {
                    findings.push({
                        rule: "test-records",
                        file: file.relativePath,
                        line,
                        message:
                            `"${record.name}" in ${info.label} TestData has no base clause at all — a case ` +
                            `record must inherit ReturnCase<,>/ThrowsCase<>/the shared layer case type ` +
                            `(unit-test.md §4.2 "Standard Definitions"; ${info.addendum}).`,
                        key: `test-records:missing-base:${file.relativePath}:${record.name}`,
                    });
                } else if (RAW_INFRASTRUCTURE_BASES.has(baseTypeName)) {
                    findings.push({
                        rule: "test-records",
                        file: file.relativePath,
                        line,
                        message:
                            `"${record.name}" in ${info.label} TestData inherits ${baseTypeName} directly — ` +
                            `case records must inherit ReturnCase<,>/ThrowsCase<>/the shared layer case type ` +
                            `instead of the raw ${baseTypeName} infrastructure type (unit-test.md §4.2; fixture.md §3).`,
                        key: `test-records:wrong-base:${file.relativePath}:${record.name}`,
                    });
                }
            }
        }

        return findings;
    },
});
