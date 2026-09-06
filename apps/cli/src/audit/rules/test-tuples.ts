import { readdirSync, type Dirent } from "node:fs";
import { basename, join, relative } from "node:path";

import { registerRule } from "../catalog.js";
import {
    findMethodDeclarations,
    findRecordDeclarations,
    findTupleTypes,
    parseFile as parseCsharpFile,
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
 * ## The check (plan §4.3 of `docs/ai/specs/testing/unit-test.md`)
 *
 * For every `*TestData.cs` file, for every `*Case`-suffixed record with a
 * primary-constructor parameter literally named `Value` (PascalCase — the
 * spec's required property name) whose declared type is a `tuple_type`:
 *
 * 1. **Every named tuple element must be camelCase.** An element with no
 *    name at all (a positional-only tuple element) is also flagged — the
 *    spec requires every element to carry the exact source parameter name,
 *    which is impossible without a name.
 * 2. **Where the paired source method is resolvable, tuple element names
 *    must exactly match its real parameter names**, in order — no rename, no
 *    abbreviation, no shorthand. Resolution is deliberately best-effort and
 *    self-contained (it does not depend on `test-orphans`' own resolution
 *    logic, per this rule's brief, though the shape rhymes): the record's
 *    innermost enclosing `class_declaration` name is read as the "operation
 *    group" (the repo's nested-per-method TestData convention, e.g.
 *    `public static class ExactLength { … }`), the outermost enclosing
 *    class's name has its trailing `TestData` stripped to get a candidate
 *    source file basename (`MustStringClausesTestData` -> `MustStringClauses`),
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
function collectTestDataFiles(ctx: RuleContext): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles
            .filter(
                (file) =>
                    file.startsWith("tests/") &&
                    file.endsWith(TEST_DATA_FILE_SUFFIX),
            )
            .map((file) => ({
                absolutePath: join(ctx.rootDir, file),
                relativePath: file,
            }));
    }

    return [...walkCsFiles(ctx.rootDir)]
        .filter((absolutePath) => absolutePath.endsWith(TEST_DATA_FILE_SUFFIX))
        .map((absolutePath) => ({
            absolutePath,
            relativePath: toForwardSlash(relative(ctx.rootDir, absolutePath)),
        }));
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
    record: RecordDeclarationInfo,
    tupleArity: number,
    sourceIndex: ReadonlyMap<string, CandidateFile[]>,
    parse: (filePath: string) => Promise<ParsedFile>,
): Promise<ResolvedSourceMethod | null> {
    const chain = enclosingClassChain(record.node);
    if (chain.length < 2) {
        return null; // No distinct operation-group nesting to name a method after.
    }

    const operationGroupName = chain[0];
    const outerClassName = chain[chain.length - 1];
    if (
        operationGroupName === undefined ||
        outerClassName === undefined ||
        !outerClassName.endsWith(TEST_DATA_SUFFIX)
    ) {
        return null;
    }

    const sourceBaseName = outerClassName.slice(0, -TEST_DATA_SUFFIX.length);
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

async function checkRecord(
    record: RecordDeclarationInfo,
    tupleByKey: ReadonlyMap<string, TupleTypeInfo>,
    sourceIndex: ReadonlyMap<string, CandidateFile[]>,
    parse: (filePath: string) => Promise<ParsedFile>,
    relFile: string,
): Promise<Finding[]> {
    if (!record.parameters || !record.name.endsWith("Case")) {
        return [];
    }

    const valueParameter = record.parameters.find((p) => p.name === "Value");
    if (!valueParameter) {
        return [];
    }

    const typeNode = valueParameter.node.childForFieldName("type");
    if (!typeNode || typeNode.type !== "tuple_type") {
        return []; // Single-input Value (or unresolvable type) — not this rule's concern.
    }

    const tuple = tupleByKey.get(tupleTypeKey(typeNode));
    if (!tuple) {
        return []; // Should not happen — every tuple_type under root was indexed.
    }

    const line = typeNode.startPosition.row + 1;
    const findings: Finding[] = [];

    tuple.elements.forEach((element, index) => {
        if (element.name === null) {
            findings.push({
                rule: "test-tuples",
                file: relFile,
                line,
                message:
                    `${record.name}: tuple element #${String(index + 1)} in Value has no name — every ` +
                    `element must be a camelCase name matching the source method's exact parameter name.`,
                key: `test-tuples:unnamed:${relFile}:${String(line)}:${String(index)}`,
            });
        } else if (!CAMEL_CASE.test(element.name)) {
            findings.push({
                rule: "test-tuples",
                file: relFile,
                line,
                message: `${record.name}: tuple element '${element.name}' in Value must be camelCase (unit-test.md §4.3).`,
                key: `test-tuples:camel-case:${relFile}:${String(line)}:${element.name}`,
            });
        }
    });

    const resolved = await resolveSourceMethod(
        record,
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
                        `${record.name}: tuple element '${element.name}' in Value does not match the exact ` +
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
        "Every named tuple element in a *TestData.cs case record's Value property is camelCase and, where the paired source method is resolvable, matches its exact parameter names.",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const parse = ctx.parseFile ?? parseCsharpFile;
        const testDataFiles = collectTestDataFiles(ctx);
        const sourceIndex = indexSourceFilesByBaseName(collectSourceFiles(ctx));

        const findings: Finding[] = [];
        for (const file of testDataFiles) {
            const { root } = await parse(file.absolutePath);
            const tupleByKey = new Map(
                findTupleTypes(root).map((tuple) => [
                    tupleTypeKey(tuple.node),
                    tuple,
                ]),
            );

            for (const record of findRecordDeclarations(root)) {
                findings.push(
                    ...(await checkRecord(
                        record,
                        tupleByKey,
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
