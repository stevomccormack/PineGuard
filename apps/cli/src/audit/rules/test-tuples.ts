import { readdirSync, type Dirent } from "node:fs";
import { basename, join, relative } from "node:path";

import { registerRule } from "../catalog.js";
import {
    findMethodDeclarations,
    findRecordDeclarations,
    parseFile as parseCsharpFile,
    toTupleTypeInfo,
} from "../parsing/csharp.js";
import type {
    MethodDeclarationInfo,
    ParameterInfo,
    ParsedFile,
    RecordDeclarationInfo,
    TupleTypeInfo,
} from "../parsing/csharp.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `test-tuples` — AST-based rebuild of the legacy tool's Rule54 (plan
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3, §8, §2.4, P2.14).
 *
 * ## The old rule, and the bug this rewrite fixes
 *
 * `tools/audit-cli/rules/Test-Rule54-UnitTestTupleConventions.ps1`'s "tuple
 * type" regex (`\(\s*(?<inner>...)\s*\)\s+(?<var>[A-Za-z_][A-Za-z0-9_]*)`) is
 * not anchored to a tuple's own parentheses — it matches *any*
 * `(...) identifier` shape, which is exactly what a record's own
 * primary-constructor parameter list looks like when the record is followed
 * by anything (a base clause, a semicolon read as trailing text, etc.):
 * `record ValidCase(string Name, (string? Value, int Length) Value, bool
 * Expected)` gets its *outer* parameter list spliced into the "tuple element"
 * list right alongside the real, inner tuple, so `Name`/`Value`/`Expected`
 * (legitimately PascalCase record property names) get flagged as camelCase
 * violations they are not (plan §2.4). This rewrite never runs a regex over
 * C# source text for detection: it walks `tuple_type` nodes found via
 * `parsing/csharp.ts`'s `findTupleTypes` — already proven in
 * `test/audit/parsing/csharp.test.ts` to distinguish a real `tuple_type` node
 * from a record's unrelated `parameter_list`/`parameter` node shape — and
 * correlates each one to the specific record parameter it types, rather than
 * pattern-matching parentheses.
 *
 * ## The check (§4.3 of `docs/ai/specs/testing/unit-test.md`)
 *
 * ### Which tuples are in scope (widened in P4.3)
 *
 * §4.3's rule is about the tuple that carries a multi-parameter method's
 * *inputs*. That tuple appears in the repo in two shapes, and the rule
 * originally saw only the first:
 *
 * 1. The declared type of a `*Case`-suffixed record's primary-constructor
 *    parameter literally named `Value` (PascalCase — the property name §4.3
 *    mandates). ~239 sites, mostly in the "(Other)" adapter packages that
 *    still hand-roll their own case records.
 * 2. The **first type argument of a `*Case`/`*Scenario` generic** —
 *    `TheoryData<RuleCase<(int value, int min, int max)>>`,
 *    `RuleScenario<(string? value, int threshold)>`,
 *    `ReturnCase<(DateTimeOffset start, DateTimeOffset end), TimeSpan>`. This
 *    is the shape the scenario architecture actually uses, and it is what
 *    every §4.3 example is written in (`unit-test.md` §4.3's two canonical
 *    examples, §8.1's `IsBaz`, `fixture.md` §11.4). ~1,533 sites in
 *    `*TestData.cs` plus ~685 in the fixtures — all of them invisible to the
 *    rule before P4.3, so a PR introducing
 *    `RuleCase<(string? val, int len)>` was never flagged.
 *
 * Only tuples in a *declaration's* type position count — a record's
 * `base_list`, a property body, an object-creation's type arguments and every
 * other expression position is skipped. See
 * {@link EXPRESSION_POSITION_ANCESTORS} for why each one is excluded.
 *
 * ### Which files are in scope (widened in P4.3)
 *
 * Every `*TestData.cs` under `tests/`, **plus** every `.cs` file under
 * `tests/PineGuard.Testing/Fixtures/`. §9.3 of the root spec states the
 * fixture field convention in the same terms and points back at §4.3 for it
 * ("Tuple element names: **camelCase**, **exact parameter names** from the
 * method under test (per §4.3)"), and `fixture.md` §11.4 repeats it verbatim.
 * Fixtures are where the tuple is *defined* — a wrong element name there
 * propagates into every layer's TestData — so leaving them unchecked was the
 * larger half of the gap.
 *
 * ### What is checked
 *
 * 1. **Every named tuple element must be camelCase.** An element with no
 *    name at all (a positional-only tuple element) is also flagged — the
 *    spec requires every element to carry the exact source parameter name,
 *    which is impossible without a name.
 * 2. **Where the paired source method is resolvable, tuple element names
 *    must exactly match its real parameter names**, in order — no rename, no
 *    abbreviation, no shorthand. Resolution is deliberately best-effort and
 *    self-contained (it does not depend on `test-orphans`' own resolution
 *    logic, per this rule's brief, though the shape rhymes): the tuple's
 *    innermost enclosing `class_declaration` name is read as the "operation
 *    group" (the repo's nested-per-method TestData/fixture convention, e.g.
 *    `public static class ExactLength { … }`; `fixture.md` §9.2: "Inner class
 *    | Matches the Core Rules method name"), the outermost enclosing class's
 *    name has its trailing `TestData`/`Fixtures` stripped to get a candidate
 *    source file basename (`MustStringClausesTestData` -> `MustStringClauses`;
 *    `StringRulesFixtures` -> `StringRules`),
 *    and — if exactly one `.cs` file under `src/` has that basename, and
 *    exactly one of its methods is named after the operation group with a
 *    caller-supplied value-parameter count matching the tuple's arity — that
 *    method's parameter names are the expected names. Any step failing to
 *    resolve to exactly one answer (no nesting, no matching source file, no
 *    matching method, an ambiguous arity match) skips check 2 entirely for
 *    that tuple, silently and without a finding — check 1 still applies
 *    regardless, since it needs no source resolution at all.
 *
 * "Caller-supplied value parameters" excludes the extension-method receiver
 * (a parameter with the `this` modifier) and any parameter carrying a
 * `[CallerArgumentExpression]`/`[CallerMemberName]`/etc. attribute — a call
 * site can never pass those explicitly, so they play no part in the tuple
 * the *caller* constructs.
 */

const CAMEL_CASE = /^[a-z][a-zA-Z0-9]*$/;
const CALLER_ATTRIBUTE_PREFIX = "Caller";
const TEST_DATA_SUFFIX = "TestData";
const TEST_DATA_FILE_SUFFIX = `${TEST_DATA_SUFFIX}.cs`;
const FIXTURES_SUFFIX = "Fixtures";
/** `fixture.md` §10 / root spec §9.2: the one folder fixtures live in. */
const FIXTURES_DIR = "tests/PineGuard.Testing/Fixtures/";
/** Outer-class suffixes that name a source class once stripped (see check 2 in the header). */
const OUTER_CLASS_SUFFIXES = [TEST_DATA_SUFFIX, FIXTURES_SUFFIX] as const;
/** Generic type names whose FIRST type argument is the input tuple: `RuleCase<>`, `MustCase<>`, `ReturnCase<,>`, `ThrowsCase<>`, `RuleScenario<>`, … */
const TUPLE_BEARING_GENERIC = /(?:Case|Scenario)$/;

function toForwardSlash(path: string): string {
    return path.replaceAll("\\", "/");
}

function tryReaddir(dir: string): Dirent[] {
    try {
        return readdirSync(dir, { withFileTypes: true });
    } catch {
        return [];
    }
}

/** Recursively lists every `*.cs` file under `dir`, skipping `obj`/`bin` build output. Returns `[]` if `dir` does not exist. */
function* walkCsFiles(dir: string): Generator<string> {
    for (const entry of tryReaddir(dir)) {
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

interface CandidateFile {
    readonly absolutePath: string;
    readonly relativePath: string;
}

/**
 * Resolves the `*TestData.cs` files this rule scans: every tracked file
 * under `tests/` ending in `TestData.cs` on a real `pineguard audit` run
 * (`ctx.trackedFiles`), or every such file under `ctx.rootDir` when running
 * against a fixture tree (the test harness in `test/support/runRule.ts`
 * never populates `trackedFiles` — mirrors `test-records`' own
 * `collectTestDataFiles`, this rule's closest sibling).
 */
function isInScopeFile(relativePath: string): boolean {
    return (
        (relativePath.startsWith("tests/") &&
            relativePath.endsWith(TEST_DATA_FILE_SUFFIX)) ||
        (relativePath.startsWith(FIXTURES_DIR) &&
            relativePath.endsWith(".cs")) ||
        // Fixture-tree fallback: a VIBE fixture root is not the repo root, so
        // its paths carry neither the `tests/` nor the `PineGuard.Testing`
        // prefix. Match the folder name alone there.
        relativePath.includes("/Fixtures/")
    );
}

function collectTestDataFiles(ctx: RuleContext): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles.filter(isInScopeFile).map((file) => ({
            absolutePath: join(ctx.rootDir, file),
            relativePath: file,
        }));
    }

    return [...walkCsFiles(ctx.rootDir)]
        .map((absolutePath) => ({
            absolutePath,
            relativePath: toForwardSlash(relative(ctx.rootDir, absolutePath)),
        }))
        .filter(
            (file) =>
                file.relativePath.endsWith(TEST_DATA_FILE_SUFFIX) ||
                file.relativePath.includes("/Fixtures/"),
        );
}

/**
 * Resolves every `.cs` file under `src/` — the candidate set for the
 * best-effort source-method lookup. Same tracked-files-else-walk shape as
 * {@link collectTestDataFiles}; a fixture tree with no `src/` directory at
 * all yields `[]` (via {@link walkCsFiles}'s missing-directory tolerance),
 * which is exactly "no source resolvable" for every record in that fixture.
 */
function collectSourceFiles(ctx: RuleContext): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles
            .filter((file) => file.startsWith("src/") && file.endsWith(".cs"))
            .map((file) => ({
                absolutePath: join(ctx.rootDir, file),
                relativePath: file,
            }));
    }

    return [...walkCsFiles(join(ctx.rootDir, "src"))].map((absolutePath) => ({
        absolutePath,
        relativePath: toForwardSlash(relative(ctx.rootDir, absolutePath)),
    }));
}

/** Groups `sourceFiles` by their `.cs`-stripped basename, e.g. `"MustStringClauses"`. */
function indexSourceFilesByBaseName(
    sourceFiles: readonly CandidateFile[],
): Map<string, CandidateFile[]> {
    const index = new Map<string, CandidateFile[]>();
    for (const file of sourceFiles) {
        const base = basename(file.relativePath, ".cs");
        const existing = index.get(base);
        if (existing) {
            existing.push(file);
        } else {
            index.set(base, [file]);
        }
    }
    return index;
}

/**
 * True for a parameter the compiler supplies on the caller's behalf (e.g.
 * `[CallerArgumentExpression]`, `[CallerMemberName]`) — a call site can never
 * pass an explicit argument for one of these, so it is never part of the
 * tuple a caller constructs.
 */
function isCompilerSupplied(parameter: ParameterInfo): boolean {
    return parameter.attributes.some((attribute) =>
        attribute.name.startsWith(CALLER_ATTRIBUTE_PREFIX),
    );
}

/** A method's parameters a caller actually supplies: no `this` receiver, no compiler-supplied parameter. */
function valueParametersOf(method: MethodDeclarationInfo): ParameterInfo[] {
    return method.parameters.filter(
        (parameter) =>
            !parameter.modifiers.includes("this") &&
            !isCompilerSupplied(parameter),
    );
}

/**
 * Walks up from `node` collecting every enclosing `class_declaration`'s name,
 * innermost first — e.g. `["ExactLength", "MustStringClausesTestData"]` for a
 * record declared inside `public static class ExactLength { … }` inside the
 * outer `MustStringClausesTestData` class.
 */
function enclosingClassChain(node: RecordDeclarationInfo["node"]): string[] {
    const chain: string[] = [];
    let current = node.parent;
    while (current) {
        if (current.type === "class_declaration") {
            const name = current.childForFieldName("name")?.text;
            if (name) {
                chain.push(name);
            }
        }
        current = current.parent;
    }
    return chain;
}

/**
 * Ancestors that put a node in *expression* position rather than in a
 * declaration's type position:
 *
 * - `base_list` — a two-line record restates its `Value` parameter's type in
 *   its `: Base(...)` clause; reporting both doubles every finding.
 * - the rest — the body of a property/method, an object-creation's type
 *   arguments, an argument list, an initializer. A tuple there is an
 *   intermediate in a projection, not a declared dataset shape. The repo has
 *   70 such sites (`.Select(s => new RuleScenario<(TimeSpan, TimeSpan,
 *   Inclusion)>(…))` in `FluentTimeSpanExtensionsTestData` and
 *   `GuardTimeSpanClausesTestData`), whose *declared* dataset type on the
 *   very next line up is correctly named — §4.3 governs the declaration, so
 *   flagging the projection intermediate would be a false positive.
 */
const EXPRESSION_POSITION_ANCESTORS = new Set([
    "base_list",
    "arrow_expression_clause",
    "block",
    "argument_list",
    "object_creation_expression",
    "equals_value_clause",
    "initializer_expression",
    "collection_expression",
]);

function isInExpressionPosition(node: RecordDeclarationInfo["node"]): boolean {
    let current = node.parent;
    while (current) {
        if (EXPRESSION_POSITION_ANCESTORS.has(current.type)) return true;
        current = current.parent;
    }
    return false;
}

/** One tuple this rule checks, plus the label its findings are reported under. */
interface TupleSite {
    readonly tuple: TupleTypeInfo;
    /** The declaration the tuple belongs to, for the finding message (`"IsBetween.Cases"`, `"ValidCase"`, …). */
    readonly owner: string;
    /** Any node inside the declaration — used to walk up to the enclosing Operation Group / outer class. */
    readonly anchor: RecordDeclarationInfo["node"];
}

/** The nearest enclosing property/field/record declaration's name, for a finding's `owner` label. */
function ownerLabelOf(node: RecordDeclarationInfo["node"]): string {
    let current = node.parent;
    while (current) {
        if (
            current.type === "property_declaration" ||
            current.type === "record_declaration"
        ) {
            const name = current.childForFieldName("name")?.text;
            if (name) return name;
        }
        if (current.type === "field_declaration") {
            const declarator = current
                .descendantsOfType("variable_declarator")
                .at(0);
            const name = declarator?.childForFieldName("name")?.text;
            if (name) return name;
        }
        current = current.parent;
    }
    return "tuple";
}

/**
 * Every §4.3 input tuple in `root`: the first type argument of any
 * `*Case`/`*Scenario` generic, plus any `*Case` record's `Value` parameter
 * whose type is a tuple (which the generic form usually also covers via the
 * record's base clause — deliberately skipped there to avoid double
 * reporting). See this module's header, "Which tuples are in scope".
 */
function collectTupleSites(
    root: RecordDeclarationInfo["node"],
    records: readonly RecordDeclarationInfo[],
): TupleSite[] {
    const sites = new Map<string, TupleSite>();

    // `generic_name` has no named fields in this grammar: its children are
    // exactly `identifier` then `type_argument_list` (verified against
    // tree-sitter-c-sharp 0.23.5).
    for (const generic of root.descendantsOfType("generic_name")) {
        const identifier = generic.children.find(
            (child) => child.type === "identifier",
        );
        if (!identifier || !TUPLE_BEARING_GENERIC.test(identifier.text)) {
            continue;
        }
        const argumentList = generic.children.find(
            (child) => child.type === "type_argument_list",
        );
        const first = argumentList?.namedChild(0);
        if (!first || first.type !== "tuple_type") continue;
        if (isInExpressionPosition(first)) continue;

        sites.set(tupleTypeKey(first), {
            tuple: toTupleTypeInfo(first),
            owner: `${identifier.text} in '${ownerLabelOf(generic)}'`,
            anchor: generic,
        });
    }

    // Fixture tuple *fields* — `public static readonly (string? value, int
    // length) Matching = …` (root spec §9.3, `fixture.md` §11.4). Their
    // `tuple_type` sits under `variable_declaration`, not under any generic.
    for (const tuple of root.descendantsOfType("tuple_type")) {
        if (tuple.parent?.type !== "variable_declaration") continue;
        if (sites.has(tupleTypeKey(tuple))) continue;

        sites.set(tupleTypeKey(tuple), {
            tuple: toTupleTypeInfo(tuple),
            owner: `field '${ownerLabelOf(tuple)}'`,
            anchor: tuple,
        });
    }

    for (const record of records) {
        if (!record.parameters || !record.name.endsWith("Case")) continue;
        const valueParameter = record.parameters.find(
            (p) => p.name === "Value",
        );
        const typeNode = valueParameter?.node.childForFieldName("type");
        if (!typeNode || typeNode.type !== "tuple_type") continue;

        sites.set(tupleTypeKey(typeNode), {
            tuple: toTupleTypeInfo(typeNode),
            owner: `${record.name}.Value`,
            anchor: record.node,
        });
    }

    return [...sites.values()];
}

interface ResolvedSourceMethod {
    readonly className: string;
    readonly methodName: string;
    readonly parameterNames: readonly string[];
}

/**
 * Best-effort resolution of the source method a test-case record's `Value`
 * tuple describes. See this file's header comment for the exact algorithm
 * and every point at which it gives up gracefully (returns `null`) rather
 * than guessing.
 */
async function resolveSourceMethod(
    site: TupleSite,
    tupleArity: number,
    sourceIndex: ReadonlyMap<string, CandidateFile[]>,
    parse: (filePath: string) => Promise<ParsedFile>,
): Promise<ResolvedSourceMethod | null> {
    const chain = enclosingClassChain(site.anchor);
    if (chain.length < 2) {
        return null; // No distinct operation-group nesting to name a method after.
    }

    const operationGroupName = chain[0];
    const outerClassName = chain[chain.length - 1];
    if (operationGroupName === undefined || outerClassName === undefined) {
        return null;
    }

    const suffix = OUTER_CLASS_SUFFIXES.find((candidate) =>
        outerClassName.endsWith(candidate),
    );
    if (suffix === undefined) {
        return null;
    }

    const sourceBaseName = outerClassName.slice(0, -suffix.length);
    if (sourceBaseName.length === 0) {
        return null;
    }

    const candidates = sourceIndex.get(sourceBaseName);
    if (!candidates || candidates.length !== 1) {
        return null; // No source file found, or the basename is ambiguous.
    }
    const [sourceFile] = candidates;
    if (!sourceFile) {
        return null;
    }

    const { root } = await parse(sourceFile.absolutePath);
    const methods = findMethodDeclarations(root).filter(
        (method) => method.name === operationGroupName,
    );
    if (methods.length === 0) {
        return null; // No method named after the operation group.
    }

    const arityMatches = methods
        .map((method) => valueParametersOf(method))
        .filter((parameters) => parameters.length === tupleArity);
    if (arityMatches.length !== 1) {
        return null; // No overload matches the tuple's arity, or more than one does.
    }
    const [parameters] = arityMatches;
    if (!parameters) {
        return null;
    }

    return {
        className: sourceBaseName,
        methodName: operationGroupName,
        parameterNames: parameters.map((parameter) => parameter.name),
    };
}

function tupleTypeKey(node: { startIndex: number; endIndex: number }): string {
    return `${String(node.startIndex)}:${String(node.endIndex)}`;
}

async function checkTupleSite(
    site: TupleSite,
    sourceIndex: ReadonlyMap<string, CandidateFile[]>,
    parse: (filePath: string) => Promise<ParsedFile>,
    relFile: string,
): Promise<Finding[]> {
    const { tuple, owner } = site;
    const line = tuple.node.startPosition.row + 1;
    const findings: Finding[] = [];

    tuple.elements.forEach((element, index) => {
        if (element.name === null) {
            findings.push({
                rule: "test-tuples",
                file: relFile,
                line,
                message:
                    `${owner}: tuple element #${String(index + 1)} has no name — every ` +
                    `element must be a camelCase name matching the source method's exact parameter name.`,
                key: `test-tuples:unnamed:${relFile}:${String(line)}:${String(index)}`,
            });
        } else if (!CAMEL_CASE.test(element.name)) {
            findings.push({
                rule: "test-tuples",
                file: relFile,
                line,
                message: `${owner}: tuple element '${element.name}' must be camelCase (unit-test.md §4.3).`,
                key: `test-tuples:camel-case:${relFile}:${String(line)}:${element.name}`,
            });
        }
    });

    const resolved = await resolveSourceMethod(
        site,
        tuple.elements.length,
        sourceIndex,
        parse,
    );
    if (resolved) {
        tuple.elements.forEach((element, index) => {
            if (element.name === null) {
                return; // Already reported above; nothing to compare.
            }
            const expected = resolved.parameterNames[index];
            if (expected !== undefined && element.name !== expected) {
                findings.push({
                    rule: "test-tuples",
                    file: relFile,
                    line,
                    message:
                        `${owner}: tuple element '${element.name}' does not match the exact ` +
                        `source parameter name '${expected}' of ${resolved.className}.${resolved.methodName} ` +
                        `(unit-test.md §4.3 — no shorthand, no renaming, no abbreviation).`,
                    key: `test-tuples:exact-name:${relFile}:${String(line)}:${element.name}`,
                });
            }
        });
    }

    return findings;
}

registerRule({
    slug: "test-tuples",
    legacyId: "Rule54",
    scope: "testing",
    gate: false,
    description:
        "Every input tuple in a *TestData.cs or fixture file — a *Case/*Scenario generic's first type argument, a case record's Value property, or a fixture tuple field — has camelCase element names that, where the paired source method is resolvable, match its exact parameter names.",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const parse = ctx.parseFile ?? parseCsharpFile;
        const testDataFiles = collectTestDataFiles(ctx);
        const sourceIndex = indexSourceFilesByBaseName(collectSourceFiles(ctx));

        const findings: Finding[] = [];
        for (const file of testDataFiles) {
            const { root } = await parse(file.absolutePath);
            const sites = collectTupleSites(root, findRecordDeclarations(root));

            for (const site of sites) {
                findings.push(
                    ...(await checkTupleSite(
                        site,
                        sourceIndex,
                        parse,
                        file.relativePath,
                    )),
                );
            }
        }

        findings.sort((a, b) => a.key.localeCompare(b.key));
        return findings;
    },
});
