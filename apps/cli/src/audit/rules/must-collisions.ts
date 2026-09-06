import { readdirSync } from "node:fs";
import { join, relative } from "node:path";

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
 * `must-collisions` — the overload-collision half of the legacy tool's
 * Rule01 (plan `docs/ai/plans/audit-cli-rebuild.md` §4.3, §8, §2.2, P2.5).
 *
 * ## What the old rule tried to do, and why it crashed
 *
 * `tools/audit-cli/rules/Test-Rule01-Naming.ps1` loaded MSBuildWorkspace and
 * a whole-solution `Compilation`, then read a bootstrap file
 * `artifacts/audit/naming-spec.json` that was never shipped or auto-created —
 * `Program.cs:48` had no existence guard for it, so the rule crashed on every
 * fresh checkout (plan §2.2). Despite loading a full semantic model, the old
 * tool never actually used it for collision analysis (plan §2.4). This rule
 * is a from-source (tree-sitter, syntax-only) rebuild with no MSBuildWorkspace
 * and no bootstrap file — exemptions, if ever needed, live in
 * `apps/cli/config/exceptions.json` (applied centrally by the engine, see
 * `src/audit/engine.ts`'s `applyExceptions` — this rule does not need its own
 * exceptions handling).
 *
 * The nullability half of the old Rule01 (is a Must/Guard primary parameter
 * correctly `T?` vs. non-nullable) is a *different* rule, `nullability`
 * (plan §4.3, §8) — both `nullability` and this rule legitimately answer to
 * the same legacy id `Rule01`, same as `must-usage` answering to the old
 * Rule03/04/05 (see that rule's own note). Whoever wires legacy-id lookups
 * for real needs to handle one legacy id resolving to two new slugs.
 *
 * ## The check
 *
 * Every `public static` Must extension method under `src/PineGuard.MustClauses/`
 * (first parameter carries the `this` modifier — the same target family
 * `must-usage` enumerates) is grouped by method name. For each overload, the
 * "value parameters" are every parameter after the `this` receiver that the
 * compiler does not supply on the caller's behalf (i.e. not
 * `[CallerArgumentExpression]`/`[CallerMemberName]`/etc. — a caller can never
 * pass an explicit argument for those, so they play no part in whether a
 * literal `Must.Be.Xxx(null)` call is ambiguous).
 *
 * An overload is a genuine candidate for a bare, single-argument
 * `Must.Be.Xxx(null)` call only when **every value parameter after the
 * first has a default value** ({@link isReachableWithSingleArgument}) — a
 * real overload pair already in the codebase proves why this matters more
 * than a naive "same parameter count" comparison:
 * `MustStringNumbersClauses.GreaterThan(string? value, decimal min, ...)` and
 * `MustStringTimeSpanClauses.GreaterThan(string? value, TimeSpan threshold, ...)`
 * both take a `string?` first parameter and have the same total parameter
 * count, but `min`/`threshold` have **no default**, so neither overload can
 * ever be invoked with just `Must.Be.GreaterThan(null)` — they are not
 * candidates at all, let alone colliding ones. (`MustStringClauses.DigitsOnly`'s
 * two overloads are excluded from each other for the same underlying reason:
 * the second overload's `allowedNonDigitChars` parameter has no default.)
 *
 * Among the overloads that *are* reachable with a single argument, this rule
 * also excludes any whose first parameter's type refers to **that method's
 * own generic type parameter** ({@link referencesOwnTypeParameter}) — e.g.
 * `MustCollectionClauses.Empty<T>(this IMustClause _, IEnumerable<T>? value, ...)`.
 * A bare `null` literal gives the compiler nothing to infer `T` from, so this
 * overload fails type inference and is never actually an applicable
 * candidate for `Must.Be.Empty(null)` — it looks identical in shape to
 * `MustStringClauses.Empty(this IMustClause _, string? value, ...)` (also
 * named `Empty`, also reachable with one argument, also a reference-type
 * first parameter) but the two do not collide in practice.
 *
 * What remains is classified by {@link classifyNullAcceptance}: a reference
 * type (`string`, an interface — anything named `IXxx` by the pervasive .NET
 * convention, since an interface can never be a `struct` — or an array)
 * accepts `null`; a nullable value type (`int?`, `DateOnly?`, `Nullable<T>`)
 * accepts `null`; a non-nullable {@link KNOWN_VALUE_TYPES} member (`int`,
 * `DateOnly`) does not. Anything else — a bare custom type name with no `I`
 * prefix — is deliberately left **unclassified** rather than guessed at: it
 * could be a `class` (reference, accepts `null`) or a `readonly struct`
 * (value type, rejects `null`), and this rule cannot tell the difference from
 * syntax alone. This is exactly the case that would otherwise misfire on
 * `MustDateOnlyRangeClauses.Chronological(this IMustClause _, DateOnlyRange range, ...)`
 * and its three siblings (`DateTimeRange`/`DateTimeOffsetRange`/`TimeOnlyRange`):
 * `DateOnlyRange` is a `public readonly struct` (`src/PineGuard.Core/Common/DateOnlyRange.cs`),
 * so `range` never accepts `null` — but nothing in its bare name says so.
 *
 * A method name with **two or more** overloads that survive both filters and
 * are classified `"accepts"` is flagged: `Must.Be.<Name>(null)` is genuinely
 * ambiguous (CS0121) or resolves unpredictably at that call site.
 *
 * This is deliberately conservative, not a full reimplementation of C#
 * overload resolution or type inference — every simplification above is
 * biased towards **under-reporting** (skipping a real collision) rather than
 * fabricating one from a type or shape this rule cannot read confidently
 * from syntax alone.
 */

const MUST_CLAUSES_DIR = "src/PineGuard.MustClauses";

/**
 * Value-type keywords whose *bare* (non-`?`) form does not accept a null
 * literal — the built-in numeric/bool/char primitives plus the handful of
 * `struct` types this codebase's Must clauses parameterise on today
 * (`DateOnly`, `DateTime`, `DateTimeOffset`, `TimeSpan`, `TimeOnly`, `Guid`).
 */
const KNOWN_VALUE_TYPES = new Set<string>([
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
    "DateOnly",
    "DateTime",
    "DateTimeOffset",
    "TimeSpan",
    "TimeOnly",
    "Guid",
]);

/**
 * Names this rule is confident are reference types even though they are not
 * spelled with the `I`-prefix interface convention {@link classifyNullAcceptance}
 * otherwise relies on.
 */
const KNOWN_REFERENCE_TYPES = new Set<string>(["string", "object"]);

type NullAcceptance = "accepts" | "rejects" | "unknown";

/**
 * Classifies whether a syntactic parameter type would accept a `null`
 * literal argument. See this file's header comment for the full reasoning
 * and the real Must clause examples that motivate each branch.
 *
 * - An array type (`char[]`, `int?[]`, ...) is always a reference type:
 *   `"accepts"`.
 * - `Nullable<T>` / `T?` where `T` is a {@link KNOWN_VALUE_TYPES} member:
 *   `"accepts"` (a nullable value type).
 * - A bare {@link KNOWN_VALUE_TYPES} member with no `?`: `"rejects"` — a
 *   `null` literal does not convert to a non-nullable value type.
 * - `string`/`object`, or a name starting with `I` followed by an uppercase
 *   letter (the pervasive .NET interface convention — an interface can never
 *   be a `struct`, so it is always a reference type): `"accepts"`, with or
 *   without a trailing `?` or generic arguments.
 * - A bare, unconstrained-looking generic type parameter (`T`, `TValue`,
 *   ...): `"unknown"` — classifying it correctly needs the method's `where`
 *   constraints, which this syntax-only rule does not resolve.
 * - Anything else — a bare custom type name with no recognised shape
 *   (`DateOnlyRange`, a hypothetical custom class) — `"unknown"`: it could be
 *   a `class` or a `readonly struct`, and nothing in the name says which.
 */
function classifyNullAcceptance(rawType: string): NullAcceptance {
    const typeText = rawType.trim();
    if (typeText.endsWith("[]")) {
        return "accepts";
    }

    const nullable = typeText.endsWith("?");
    const bare = (nullable ? typeText.slice(0, -1) : typeText).trim();

    const nullableWrapperMatch = /^Nullable<\s*(.+?)\s*>$/.exec(bare);
    if (nullableWrapperMatch) {
        const inner = nullableWrapperMatch[1] ?? "";
        return KNOWN_VALUE_TYPES.has(inner) ? "accepts" : "unknown";
    }

    if (KNOWN_VALUE_TYPES.has(bare)) {
        return nullable ? "accepts" : "rejects";
    }

    const genericArgsStart = bare.indexOf("<");
    const headName =
        genericArgsStart === -1 ? bare : bare.slice(0, genericArgsStart);
    if (KNOWN_REFERENCE_TYPES.has(headName) || /^I[A-Z]/.test(headName)) {
        return "accepts";
    }

    if (/^T([A-Z][A-Za-z0-9]*)?$/.test(bare)) {
        return "unknown";
    }

    return "unknown";
}

const CALLER_ATTRIBUTE_PREFIX = "Caller";

/**
 * True for a parameter the compiler supplies on the caller's behalf (e.g.
 * `[CallerArgumentExpression]`, `[CallerMemberName]`, `[CallerLineNumber]`,
 * `[CallerFilePath]`) — a call site can never pass an explicit argument for
 * one of these, so it is irrelevant to whether a literal `Must.Be.Xxx(null)`
 * call is ambiguous.
 */
function isCompilerSupplied(parameter: ParameterInfo): boolean {
    return parameter.attributes.some((attribute) =>
        attribute.name.startsWith(CALLER_ATTRIBUTE_PREFIX),
    );
}

/** True when tree-sitter parsed an explicit `= <default>` clause onto `parameter`. */
function hasDefaultValue(parameter: ParameterInfo): boolean {
    return parameter.node.children.some((child) => child.type === "=");
}

/**
 * True when an overload is invocable by a bare, single-argument
 * `Must.Be.Xxx(null)` call — i.e. every value parameter *after* the first
 * (`restParameters`) carries a default value, since a one-argument call never
 * supplies them explicitly. See this file's header comment for the real
 * `GreaterThan`/`DigitsOnly` overloads that motivate this check.
 */
function isReachableWithSingleArgument(
    restParameters: readonly ParameterInfo[],
): boolean {
    return restParameters.every(hasDefaultValue);
}

/**
 * True for a `public static` extension method whose first parameter carries
 * the `this` modifier — the same target shape `must-usage` enumerates
 * (plan §8): a real `Must.Be.Xxx` clause.
 */
function isMustExtensionMethod(method: MethodDeclarationInfo): boolean {
    if (
        !method.modifiers.includes("public") ||
        !method.modifiers.includes("static")
    ) {
        return false;
    }
    const receiver = method.parameters[0];
    return receiver !== undefined && receiver.modifiers.includes("this");
}

/**
 * The method's own generic type parameter names, e.g. `{"T"}` for
 * `Empty<T>(...)` or `{"TKey", "TValue"}` for `HasKey<TKey, TValue>(...)`.
 * Empty for a non-generic method.
 */
function ownTypeParameterNames(method: MethodDeclarationInfo): Set<string> {
    const names = new Set<string>();
    const list = method.node.childForFieldName("type_parameters");
    if (!list) {
        return names;
    }
    for (const child of list.namedChildren) {
        if (child.type !== "type_parameter") {
            continue;
        }
        const nameNode = child.childForFieldName("name");
        if (nameNode) {
            names.add(nameNode.text);
        }
    }
    return names;
}

/**
 * True when `typeText` mentions one of `typeParameterNames` as a whole
 * identifier (e.g. `"IEnumerable<T>"` mentions `"T"`; `"IDictionary<TKey, TValue>"`
 * mentions both `"TKey"` and `"TValue"`). See this file's header comment for
 * why this excludes the overload from candidacy entirely, rather than just
 * affecting its null-acceptance classification.
 */
function referencesOwnTypeParameter(
    typeText: string,
    typeParameterNames: ReadonlySet<string>,
): boolean {
    if (typeParameterNames.size === 0) {
        return false;
    }
    for (const name of typeParameterNames) {
        const pattern = new RegExp(
            `\\b${name.replace(/[.*+?^${}()|[\]\\]/g, "\\$&")}\\b`,
        );
        if (pattern.test(typeText)) {
            return true;
        }
    }
    return false;
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
 * Resolves the C# files this rule scans: every tracked file under
 * `src/PineGuard.MustClauses/` on a real `pineguard audit` run
 * (`ctx.trackedFiles` populated by the engine), or every `*.cs` file under
 * `ctx.rootDir` when running against a fixture tree (the test harness in
 * `test/support/runRule.ts` never populates `trackedFiles` — see that
 * module's header comment).
 */
function collectCandidateFiles(ctx: RuleContext): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles
            .filter(
                (file) =>
                    file.startsWith(`${MUST_CLAUSES_DIR}/`) &&
                    file.endsWith(".cs"),
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

interface OverloadCandidate {
    readonly method: MethodDeclarationInfo;
    readonly relativePath: string;
    readonly firstParameter: ParameterInfo;
}

function locationOf(candidate: OverloadCandidate): string {
    const line = candidate.method.node.startPosition.row + 1;
    const typeText = candidate.firstParameter.typeText ?? "?";
    return `${candidate.relativePath}:${String(line)} (${typeText})`;
}

/**
 * Same information as {@link locationOf}, minus the line number — used for
 * `Finding.key` rather than `Finding.message`. `types.ts`'s `Finding.key` doc
 * comment (plan §4.4/§11) calls out line-number churn as exactly the kind of
 * incidental change a stable key must not react to: shifting an unrelated
 * method up or down in the same file must not look like a new/different
 * baseline entry for a collision that hasn't actually changed.
 */
function stableLocationOf(candidate: OverloadCandidate): string {
    const typeText = candidate.firstParameter.typeText ?? "?";
    return `${candidate.relativePath}(${typeText})`;
}

registerRule({
    slug: "must-collisions",
    legacyId: "Rule01",
    scope: "library",
    gate: false,
    description:
        "Flags public static Must clause overloads invocable with a single argument whose first parameter would all accept a bare `null` literal — an ambiguous or unpredictable Must.Be.Xxx(null) call site.",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const parse = ctx.parseFile ?? parseCsharpFile;
        const files = collectCandidateFiles(ctx);

        const candidates: OverloadCandidate[] = [];
        for (const file of files) {
            const { root } = await parse(file.absolutePath);
            for (const method of findMethodDeclarations(root)) {
                if (!isMustExtensionMethod(method)) {
                    continue;
                }

                const valueParameters = method.parameters
                    .slice(1)
                    .filter((parameter) => !isCompilerSupplied(parameter));
                const firstParameter = valueParameters[0];
                if (firstParameter?.typeText == null) {
                    continue;
                }

                // Every value parameter after the first must be optional, or
                // this overload simply cannot be reached by a one-argument
                // `Must.Be.Xxx(null)` call in the first place (real example:
                // GreaterThan(string? value, decimal min, ...) — `min` has no
                // default, see this file's header comment).
                const restParameters = valueParameters.slice(1);
                if (!isReachableWithSingleArgument(restParameters)) {
                    continue;
                }

                // A first parameter whose type mentions this method's own
                // generic type parameter can never be resolved from a bare
                // `null` argument (type inference has nothing to infer from),
                // so it is not a real candidate either (real example:
                // Empty<T>(this IMustClause _, IEnumerable<T>? value, ...)).
                const typeParameters = ownTypeParameterNames(method);
                if (
                    referencesOwnTypeParameter(
                        firstParameter.typeText,
                        typeParameters,
                    )
                ) {
                    continue;
                }

                if (
                    classifyNullAcceptance(firstParameter.typeText) !==
                    "accepts"
                ) {
                    continue;
                }

                candidates.push({
                    method,
                    relativePath: file.relativePath,
                    firstParameter,
                });
            }
        }

        const grouped = new Map<string, OverloadCandidate[]>();
        for (const candidate of candidates) {
            const name = candidate.method.name;
            const group = grouped.get(name);
            if (group) {
                group.push(candidate);
            } else {
                grouped.set(name, [candidate]);
            }
        }

        const findings: Finding[] = [];
        for (const group of grouped.values()) {
            if (group.length < 2) {
                continue;
            }

            const sorted = [...group].sort((a, b) => {
                if (a.relativePath !== b.relativePath) {
                    return a.relativePath.localeCompare(b.relativePath);
                }
                return (
                    a.method.node.startPosition.row -
                    b.method.node.startPosition.row
                );
            });
            const first = sorted[0];
            if (!first) {
                continue;
            }

            const name = first.method.name;
            const locations = sorted.map(locationOf).join(", ");
            const normalisedMessage =
                `Must.Be.${name} has ${String(sorted.length)} single-argument-reachable overloads ` +
                `whose first parameter would all accept a null literal: ` +
                sorted.map(stableLocationOf).join(", ");

            findings.push({
                rule: "must-collisions",
                file: first.relativePath,
                line: first.method.node.startPosition.row + 1,
                message:
                    `Must.Be.${name} has ${String(sorted.length)} overloads, each invocable with a single ` +
                    `argument, whose first parameter type would all accept a null literal — ` +
                    `Must.Be.${name}(null) is ambiguous or resolves unpredictably: ` +
                    locations,
                // rule:file:normalisedMessage, per the Finding.key convention
                // documented on types.ts's Finding.key (plan §4.4/§11) — see
                // stableLocationOf's doc comment for why this is built from a
                // line-number-free message rather than `finding.message` verbatim.
                key: `must-collisions:${first.relativePath}:${normalisedMessage}`,
            });
        }

        return findings;
    },
});
