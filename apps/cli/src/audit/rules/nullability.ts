import { readdirSync } from "node:fs";
import { join, relative } from "node:path";

import type { Node } from "web-tree-sitter";

import { registerRule } from "../catalog.js";
import {
    findMethodDeclarations,
    parseFile as parseCsharpFile,
} from "../parsing/csharp.js";
import type {
    MethodDeclarationInfo,
    ParameterInfo,
} from "../parsing/csharp.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `nullability` — the hybrid nullability policy on the *primary parameter*
 * of every Must/Guard clause (plan `docs/ai/plans/audit-cli-rebuild.md`
 * §4.3, §8, §2.2, P2.4). Replaces the legacy tool's Rule07 (and absorbs the
 * nullability half of its Rule01 — the overload-collision half is the
 * sibling rule `must-collisions`, see that file's header for the shared
 * legacy-id note).
 *
 * ## What the old rule tried to do, and why it always reported zero
 *
 * `tools/audit-cli/helpers/Test-SpecNullability.ps1` hand-rolled a
 * regex-based C# signature parser — `\\w`, `\\s`, `\\.` written inside
 * *single-quoted* PowerShell strings, so every escape was doubled and the
 * resulting pattern required a literal backslash in the source it was
 * matching. No real parameter token ever matched `Try-ParseParameter`'s
 * regex, so the rule silently skipped every method it scanned and printed
 * "Violations: 0" unconditionally, for months (plan §2.2). This is the same
 * escaping-bug family that hit the legacy Rule03/04/05 (`must-usage`'s
 * history). It is structurally impossible here: this rule reads real
 * tree-sitter parameter and type-parameter-constraint nodes, never regex
 * over source text.
 *
 * Even discounting the escaping bug, the old helper's *policy* was narrower
 * than what this rule enforces: it only special-cased a literal `string`
 * primary parameter and a `Nullable<T>`/`T?` wrapper around one of a fixed
 * value-type list — every other shape (a generic primary parameter, a
 * `where T : struct` constraint) was never handled at all. This rewrite
 * implements the full hybrid policy from plan §8, including the generic
 * case.
 *
 * ## The check
 *
 * Every `public static` extension method under `src/PineGuard.MustClauses/`
 * or `src/PineGuard.GuardClauses/` (first parameter carries the `this`
 * modifier — the same target shape `must-collisions` and `must-usage`
 * enumerate) has its **primary parameter** identified: the first parameter
 * after the `this` receiver that the compiler does not supply on the
 * caller's behalf (i.e. not `[CallerArgumentExpression]`/`[CallerMemberName]`/
 * etc.) — in every real Must/Guard clause in this codebase that is simply
 * the second parameter (plan §8: "second parameter of a Must extension" /
 * "first value parameter of a Guard method" describe the same position,
 * since both families share the `this IMustClause`/`this IGuardClause`
 * receiver-first convention).
 *
 * The primary parameter's declared type must follow PineGuard's hybrid
 * nullability convention:
 *
 * - a **reference type** (`string`, `object`, a custom class/interface/
 *   record, a generic collection like `IEnumerable<T>`, or an unconstrained/
 *   reference-constrained generic type parameter) must be declared
 *   **nullable** (`T?`) — flagged if it is not;
 * - a **value type** (a built-in numeric/`bool`/`char` primitive, `Guid`,
 *   `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, `TimeSpan`, or a
 *   generic type parameter constrained `where T : struct`) must be declared
 *   **non-nullable** — flagged if it is declared `T?`/`Nullable<T>`.
 *
 * A generic primary parameter's bucket is decided entirely by its own
 * method's `where` clause (plan §8): `where T : struct` (with or without
 * additional constraints, e.g. `where T : struct, IBinaryInteger<T>`) is the
 * *only* constraint shape this rule treats as a value type — every other
 * shape (no constraint at all, `where T : class`, an interface constraint,
 * `notnull`, `unmanaged`, `new()`) defaults to the reference-type bucket,
 * exactly as plan §8 states ("otherwise (unconstrained or
 * reference-constrained) → treat as reference type"). A real method whose
 * generic constraint makes that default wrong for a legitimate reason (e.g.
 * `where T : notnull`, which in fact forbids writing `T?` at all) is exactly
 * the kind of case plan §8's "documented exemption path" exists for — add
 * the method name to `nullability`'s array in `apps/cli/config/
 * exceptions.json` rather than special-casing more constraint shapes here.
 *
 * ## Exemptions
 *
 * Plan §8: "Exemptions by method name in `config/exceptions.json`." This
 * rule does not read `ctx.exceptions` itself — per `src/audit/types.ts`'s
 * `RuleContext.exceptions` doc comment and `src/audit/engine.ts`'s
 * `applyExceptions`, the engine already suppresses any finding whose rule
 * slug (`"nullability"`) has a configured substring matching the finding's
 * `file` or `message` *after* `run()` returns — the same centralized
 * mechanism `must-collisions` relies on. Every finding's `message` below
 * includes the offending method's name specifically so a
 * `{"nullability": ["MethodName"]}` entry is enough to exempt it via plain
 * substring matching; `test/rules/nullability.test.ts` proves this by
 * calling `applyExceptions` directly against this rule's own findings.
 */

const MUST_CLAUSES_DIR = "src/PineGuard.MustClauses";
const GUARD_CLAUSES_DIR = "src/PineGuard.GuardClauses";

/**
 * C# predefined-type keywords that are value types (the primitive numeric
 * types, `bool`, `char`). `string`, `object`, and `dynamic` are the
 * predefined-type keywords that are reference types instead — see
 * {@link PREDEFINED_REFERENCE_TYPES}.
 */
const PREDEFINED_VALUE_TYPES = new Set<string>([
    "bool",
    "byte",
    "sbyte",
    "char",
    "decimal",
    "double",
    "float",
    "int",
    "uint",
    "long",
    "ulong",
    "short",
    "ushort",
    "nint",
    "nuint",
]);

/** The predefined-type keywords that are reference types, not value types. */
const PREDEFINED_REFERENCE_TYPES = new Set<string>([
    "string",
    "object",
    "dynamic",
]);

/**
 * Named (non-keyword) BCL value types this codebase's Must/Guard clauses
 * parameterise on today — the same set `must-collisions` maintains for the
 * same reason (see that rule's `KNOWN_VALUE_TYPES` doc comment): a
 * syntax-only rule cannot resolve an arbitrary type name's kind, so anything
 * not in this set (or {@link PREDEFINED_VALUE_TYPES}) defaults to the
 * reference-type bucket. A real custom `struct` primary parameter this list
 * misses (e.g. one of the small value-object structs under
 * `src/PineGuard.Core/Common/`) produces a false-positive finding rather
 * than a false negative — the safer direction for a merge-blocking-adjacent
 * rule to err in, and fixable either by extending this list or by an
 * exemption entry (see the module doc comment above).
 */
const KNOWN_NAMED_VALUE_TYPES = new Set<string>([
    "Guid",
    "DateTime",
    "DateTimeOffset",
    "DateOnly",
    "TimeOnly",
    "TimeSpan",
]);

const CALLER_ATTRIBUTE_PREFIX = "Caller";

/**
 * True for a parameter the compiler supplies on the caller's behalf (e.g.
 * `[CallerArgumentExpression]`, `[CallerMemberName]`) — never a candidate
 * primary parameter. Mirrors `must-collisions`'s helper of the same name.
 */
function isCompilerSupplied(parameter: ParameterInfo): boolean {
    return parameter.attributes.some((attribute) =>
        attribute.name.startsWith(CALLER_ATTRIBUTE_PREFIX),
    );
}

/**
 * True for a `public static` extension method whose first parameter carries
 * the `this` modifier — the same target shape `must-collisions` enumerates:
 * a real `Must.Be.Xxx`/`Guard.Against.Xxx` clause.
 */
function isCandidateExtensionMethod(method: MethodDeclarationInfo): boolean {
    if (
        !method.modifiers.includes("public") ||
        !method.modifiers.includes("static")
    ) {
        return false;
    }
    const receiver = method.parameters[0];
    return receiver !== undefined && receiver.modifiers.includes("this");
}

/** The first non-compiler-supplied parameter after the `this` receiver (plan §8's "primary parameter"). */
function primaryParameterOf(
    method: MethodDeclarationInfo,
): ParameterInfo | undefined {
    return method.parameters
        .slice(1)
        .find((parameter) => !isCompilerSupplied(parameter));
}

// ---------------------------------------------------------------------------
// Generic type-parameter constraints — the one place this rule needs real
// tree-sitter node access rather than plain type-text string manipulation
// (every real Must/Guard clause's `where` clauses are read from
// `type_parameter_constraints_clause` nodes, never regexed from source text
// — see the module doc comment for why that distinction is the whole point).
// ---------------------------------------------------------------------------

/** Names of a method's own generic type parameters, and which of those carry a `where T : struct` constraint. */
interface GenericInfo {
    readonly names: ReadonlySet<string>;
    readonly structConstrained: ReadonlySet<string>;
}

const EMPTY_GENERIC_INFO: GenericInfo = {
    names: new Set<string>(),
    structConstrained: new Set<string>(),
};

function collectGenericInfo(methodNode: Node): GenericInfo {
    const typeParameterList = methodNode.childForFieldName("type_parameters");
    if (!typeParameterList) {
        return EMPTY_GENERIC_INFO;
    }

    const names = new Set<string>();
    for (const typeParameter of typeParameterList.namedChildren) {
        if (typeParameter.type !== "type_parameter") continue;
        const nameNode = typeParameter.childForFieldName("name");
        if (nameNode) names.add(nameNode.text);
    }

    const structConstrained = new Set<string>();
    const constraintsClauses = methodNode.children.filter(
        (child) => child.type === "type_parameter_constraints_clause",
    );
    for (const clause of constraintsClauses) {
        const targetName = clause.namedChildren.find(
            (child) => child.type === "identifier",
        );
        if (!targetName) continue;

        // Grammar: `type_parameter_constraint: choice(seq('class', optional('?')), 'struct', 'notnull', 'unmanaged', constructor_constraint, field('type', $.type))`.
        // The bare `struct` constraint is its own `type_parameter_constraint`
        // node whose text is exactly "struct" (never combined with the
        // interface/base-type constraints that may follow it in the same
        // clause, e.g. `where T : struct, IBinaryInteger<T>`).
        const hasStructConstraint = clause.namedChildren.some(
            (child) =>
                child.type === "type_parameter_constraint" &&
                child.text === "struct",
        );
        if (hasStructConstraint) {
            structConstrained.add(targetName.text);
        }
    }

    return { names, structConstrained };
}

// ---------------------------------------------------------------------------
// Primary-parameter type classification
// ---------------------------------------------------------------------------

type TypeBucket = "value" | "reference";

/** Splits a declared parameter type into its nullable-annotation flag and base type text (`"string?"` → `{ base: "string", nullable: true }`; `"Nullable<Guid>"` → `{ base: "Guid", nullable: true }`). */
function splitNullable(typeText: string): { base: string; nullable: boolean } {
    const trimmed = typeText.trim();
    if (trimmed.endsWith("?")) {
        return { base: trimmed.slice(0, -1).trim(), nullable: true };
    }
    const wrapperMatch = /^Nullable<\s*(.+?)\s*>$/.exec(trimmed);
    if (wrapperMatch) {
        return { base: (wrapperMatch[1] ?? trimmed).trim(), nullable: true };
    }
    return { base: trimmed, nullable: false };
}

/** Strips generic type arguments and namespace qualification down to a type's simple name (`"System.Collections.Generic.IEnumerable<T>"` → `"IEnumerable"`). */
function simpleTypeName(base: string): string {
    const angleIndex = base.indexOf("<");
    const withoutGenerics =
        angleIndex === -1 ? base : base.slice(0, angleIndex);
    const segments = withoutGenerics.split(".");
    return (segments[segments.length - 1] ?? withoutGenerics).trim();
}

interface PrimaryClassification {
    readonly bucket: TypeBucket;
    readonly isNullableSyntax: boolean;
    readonly expectedTypeText: string;
    /** `undefined` for a non-generic (concrete) primary parameter type. */
    readonly genericConstraintNote?: string;
}

/**
 * Classifies a Must/Guard clause's primary parameter against the hybrid
 * nullability policy (plan §8). Returns `undefined` when the parameter has
 * no resolvable declared type at all (the grammar failed to produce one) —
 * conservatively skipped rather than guessed at, same posture as
 * `must-collisions`.
 */
function classifyPrimaryParameter(
    parameter: ParameterInfo,
    generics: GenericInfo,
): PrimaryClassification | undefined {
    if (parameter.typeText === null) {
        return undefined;
    }

    const { base, nullable } = splitNullable(parameter.typeText);
    const simple = simpleTypeName(base);

    if (simple === base && generics.names.has(simple)) {
        const isStructConstrained = generics.structConstrained.has(simple);
        const bucket: TypeBucket = isStructConstrained ? "value" : "reference";
        return {
            bucket,
            isNullableSyntax: nullable,
            expectedTypeText: bucket === "value" ? base : `${base}?`,
            genericConstraintNote: isStructConstrained
                ? `generic parameter '${simple}' is constrained 'where ${simple} : struct', treated as a value type`
                : `generic parameter '${simple}' has no 'struct' constraint, treated as a reference type`,
        };
    }

    if (PREDEFINED_REFERENCE_TYPES.has(simple)) {
        return {
            bucket: "reference",
            isNullableSyntax: nullable,
            expectedTypeText: `${base}?`,
        };
    }

    const isValueType =
        PREDEFINED_VALUE_TYPES.has(simple) ||
        KNOWN_NAMED_VALUE_TYPES.has(simple);
    const bucket: TypeBucket = isValueType ? "value" : "reference";
    return {
        bucket,
        isNullableSyntax: nullable,
        expectedTypeText: bucket === "value" ? base : `${base}?`,
    };
}

/** A classification violates the hybrid policy when its bucket and its nullable-annotation flag disagree. */
function violates(classification: PrimaryClassification): boolean {
    return classification.bucket === "value"
        ? classification.isNullableSyntax
        : !classification.isNullableSyntax;
}

// ---------------------------------------------------------------------------
// File discovery — identical shape to `must-collisions`'s
// `collectCandidateFiles`/`walkCsFiles` (see that rule for the full doc
// comment on why the two code paths exist): tracked files under both target
// directories on a real run, or every `*.cs` file under `ctx.rootDir` against
// a fixture tree (the harness never populates `trackedFiles`).
// ---------------------------------------------------------------------------

interface CandidateFile {
    readonly absolutePath: string;
    readonly relativePath: string;
}

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

function collectCandidateFiles(ctx: RuleContext): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles
            .filter(
                (file) =>
                    file.endsWith(".cs") &&
                    (file.startsWith(`${MUST_CLAUSES_DIR}/`) ||
                        file.startsWith(`${GUARD_CLAUSES_DIR}/`)),
            )
            .map((file) => ({
                absolutePath: join(ctx.rootDir, file),
                relativePath: file,
            }));
    }

    return [...walkCsFiles(ctx.rootDir)].map((absolutePath) => ({
        absolutePath,
        relativePath: relative(ctx.rootDir, absolutePath).replaceAll("\\", "/"),
    }));
}

registerRule({
    slug: "nullability",
    legacyId: "Rule07",
    scope: "library",
    gate: false,
    description:
        "Every Must/Guard clause's primary parameter follows the hybrid nullability policy: reference types are declared nullable, value types are not (a generic parameter is judged by its own 'where' constraint).",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const parse = ctx.parseFile ?? parseCsharpFile;
        const files = collectCandidateFiles(ctx);

        const findings: Finding[] = [];
        for (const file of files) {
            const { root } = await parse(file.absolutePath);
            for (const method of findMethodDeclarations(root)) {
                if (!isCandidateExtensionMethod(method)) {
                    continue;
                }

                const primary = primaryParameterOf(method);
                if (!primary) {
                    continue;
                }

                const generics = collectGenericInfo(method.node);
                const classification = classifyPrimaryParameter(
                    primary,
                    generics,
                );
                if (!classification || !violates(classification)) {
                    continue;
                }

                const line = primary.node.startPosition.row + 1;
                const bucketNoun =
                    classification.bucket === "value"
                        ? "value type"
                        : "reference type";
                const requirement =
                    classification.bucket === "value"
                        ? "must be declared non-nullable"
                        : "must be declared nullable";
                const constraintClause = classification.genericConstraintNote
                    ? ` (${classification.genericConstraintNote})`
                    : "";

                findings.push({
                    rule: "nullability",
                    file: file.relativePath,
                    line,
                    message:
                        `${method.name}'s primary parameter '${primary.name}: ${primary.typeText ?? "?"}' ` +
                        `is a ${bucketNoun}${constraintClause}, which ${requirement} under PineGuard's ` +
                        `hybrid nullability policy — expected '${primary.name}: ${classification.expectedTypeText}'.`,
                    key: `nullability:${file.relativePath}:${method.name}:${String(line)}`,
                });
            }
        }

        return findings;
    },
});
