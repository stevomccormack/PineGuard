import { existsSync, readdirSync } from "node:fs";
import { join, relative } from "node:path";

import type { Node } from "web-tree-sitter";

import { registerRule } from "../catalog.js";
import {
    findInvocations,
    getModifiers,
    parseFile as parseCsharpFile,
} from "../parsing/csharp.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `rules-usage` — source-only rebuild of the legacy tool's Rule02 (plan
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3 row "Rule02 → `rules-usage`",
 * §2.2, P2.1).
 *
 * ## What the old rule did, and why this rebuild changes its granularity
 *
 * `tools/audit-cli/helpers/Find-UnusedRules.ps1` built `PineGuard.Core` for
 * a hardcoded `bin/Release/net8.0` TFM, loaded the DLL via reflection to list
 * every public, sealed, abstract (i.e. `static`) class named `*Rules` in the
 * `PineGuard.Rules` namespace, then regex-scanned `src/PineGuard.MustClauses`
 * for the literal text `\b([A-Za-z_][A-Za-z0-9_]*)Rules\.` and flagged a
 * *class* with zero textual hits anywhere in that scan. It was one of the
 * old tool's trustworthy rules (PASS 39/39) — but its unit of detection was
 * the whole class, and its "usage" signal was a bare substring match with no
 * awareness of what kind of C# construct the match sat inside.
 *
 * This rebuild answers a strictly more precise question — is every
 * individual **method**, not just every class, actually called? — using a
 * real parse tree instead of a build + reflection + regex pipeline. This is
 * not "fixing a bug" in the old rule (class-level PASS 39/39 was a correct
 * answer to the old rule's own, coarser question); it is deliberately tighter
 * per this rule's own written brief, and it removes one way the old
 * substring approach could be *wrong in the permissive direction*: a
 * `nameof(FixtureRules.Bar)` reference is a real regex hit for
 * `FixtureRules\.` (the old tool would have silently counted it as "used"),
 * but it is not a call — this rule's `boundary/` fixture proves the
 * AST-based version does not repeat that false negative (see below).
 *
 * ## The check
 *
 * 1. Enumerate every top-level `public static` class ending in `Rules` under
 *    `src/PineGuard.Core/Rules/` (mirrors the old tool's class filter,
 *    reimplemented from syntax: name + modifiers, not namespace + assembly
 *    reflection — partial fragments of the same class, e.g. `StringRules`
 *    split across a dozen files, all contribute to the same logical class
 *    without any special-casing, since each fragment is walked independently
 *    and methods are matched by their fully-qualified name text alone).
 * 2. Within each such class (including nested `public static` helper classes
 *    some Rules files use for grouping, e.g. `StringRules.Bool`,
 *    `StringRules.Graphemes` — confirmed by inspecting
 *    `src/PineGuard.Core/Rules/StringRules.Bool.cs` and
 *    `src/PineGuard.MustClauses/MustStringGraphemesClauses.cs` before writing
 *    this rule), collect every `public static` method, keyed by its full
 *    dotted path from the outermost Rules class down through any nested
 *    class to the method name (`StringRules.IsExactLength`,
 *    `StringRules.Bool.IsTrue`, `OwaspRules.IsXssSafe`, …). A class not
 *    ending in `Rules` under this tree (`Owasp/OwaspRegex.cs`'s `OwaspRegex`
 *    and its nested pattern-holder classes) is out of scope — the same
 *    filter the old tool's `Name.EndsWith('Rules')` reflection check applied,
 *    confirmed empirically: `OwaspRegex` is referenced only from
 *    `OwaspRules.cs`/`OwaspUtility.cs`, never from `MustClauses`, so folding
 *    it in would manufacture ~30 findings for a helper that was never meant
 *    to be called from that layer directly.
 * 3. Collect every `invocation_expression`'s dotted call-target text
 *    (`memberPath`, from `parsing/csharp.ts`'s `findInvocations`) across
 *    every `*.cs` file under `src/PineGuard.MustClauses/`. Confirmed by
 *    inspection: the calling convention is a plain static member-access
 *    chain (`StringRules.IsBetween(...)`, `StringRules.Graphemes.HasMinCount(...)`,
 *    `BoolRules.IsTrue(...)`) — no `using static` imports of any `*Rules`
 *    class exist anywhere in `src/PineGuard.MustClauses` today, so no alias
 *    handling is needed.
 * 4. A Core Rules method whose full dotted name does not appear as an exact
 *    `memberPath` anywhere in that set is reported. Overloads of the same
 *    name share one dotted path (member-access text carries no type
 *    information), so this rule — like the old one, like every other rule
 *    in this plan — is syntax-only: it cannot distinguish "no overload is
 *    called" from "some other overload is called", and is documented as such
 *    rather than guessing from argument shapes.
 */

const CORE_RULES_DIR = "src/PineGuard.Core/Rules";
const MUST_CLAUSES_DIR = "src/PineGuard.MustClauses";

interface CandidateFile {
    readonly absolutePath: string;
    /** Repo-relative (or fixture-relative) path, forward-slash normalised. */
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
 * Resolves every `*.cs` file under `${ctx.rootDir}/${dirPrefix}`: filtered
 * from `ctx.trackedFiles` on a real `pineguard audit` run, or a direct
 * directory walk when running against a fixture tree (the test harness in
 * `test/support/runRule.ts` never populates `trackedFiles` — see that
 * module's header comment). Fixtures mirror the real repo-relative directory
 * shape (`test/fixtures/rules-usage/valid/src/PineGuard.Core/Rules/...`) so
 * the same `dirPrefix` constants resolve correctly in both cases.
 */
function collectFilesUnder(
    ctx: RuleContext,
    dirPrefix: string,
): CandidateFile[] {
    if (ctx.trackedFiles) {
        return ctx.trackedFiles
            .filter(
                (file) =>
                    file.startsWith(`${dirPrefix}/`) && file.endsWith(".cs"),
            )
            .map((file) => ({
                absolutePath: join(ctx.rootDir, file),
                relativePath: file,
            }));
    }

    const dirAbsolute = join(ctx.rootDir, dirPrefix);
    if (!existsSync(dirAbsolute)) {
        return [];
    }

    return [...walkCsFiles(dirAbsolute)].map((absolutePath) => ({
        absolutePath,
        relativePath: `${dirPrefix}/${relative(dirAbsolute, absolutePath).replaceAll("\\", "/")}`,
    }));
}

/** True when `node` (a `class_declaration`) is not itself nested inside another `class_declaration`. */
function isTopLevelClass(node: Node): boolean {
    let parent = node.parent;
    while (parent) {
        if (parent.type === "class_declaration") {
            return false;
        }
        parent = parent.parent;
    }
    return true;
}

interface CoreMethod {
    /** Full dotted path from the outermost Rules class to the method, e.g. `"StringRules.Bool.IsTrue"`. */
    readonly qualifiedName: string;
    readonly file: string;
    readonly line: number;
}

/**
 * Walks `classNode` (and every nested `class_declaration` inside it,
 * regardless of that nested class's own modifiers — a nested helper class
 * such as `StringRules.Bool` groups related methods, not visibility) and
 * collects every `public static` method as a {@link CoreMethod}, qualified by
 * `chain` (the dotted path of enclosing class names collected so far).
 */
function collectPublicStaticMethods(
    classNode: Node,
    chain: readonly string[],
    relativePath: string,
    results: CoreMethod[],
): void {
    const className = classNode.childForFieldName("name")?.text ?? "";
    const nextChain = [...chain, className];

    const body = classNode.childForFieldName("body");
    if (!body) {
        return;
    }

    for (const member of body.namedChildren) {
        if (member.type === "class_declaration") {
            collectPublicStaticMethods(
                member,
                nextChain,
                relativePath,
                results,
            );
            continue;
        }

        if (member.type !== "method_declaration") {
            continue;
        }

        const modifiers = getModifiers(member);
        if (!modifiers.includes("public") || !modifiers.includes("static")) {
            continue;
        }

        const methodName = member.childForFieldName("name")?.text ?? "";
        results.push({
            qualifiedName: [...nextChain, methodName].join("."),
            file: relativePath,
            line: member.startPosition.row + 1,
        });
    }
}

/** Every `public static` method on every top-level `public static` class ending in `Rules` under `file`. */
function findCoreRulesMethods(root: Node, relativePath: string): CoreMethod[] {
    const results: CoreMethod[] = [];

    for (const classNode of root.descendantsOfType("class_declaration")) {
        if (!isTopLevelClass(classNode)) {
            continue;
        }

        const modifiers = getModifiers(classNode);
        const name = classNode.childForFieldName("name")?.text ?? "";
        if (
            !modifiers.includes("public") ||
            !modifiers.includes("static") ||
            !name.endsWith("Rules")
        ) {
            continue;
        }

        collectPublicStaticMethods(classNode, [], relativePath, results);
    }

    return results;
}

registerRule({
    slug: "rules-usage",
    legacyId: "Rule02",
    scope: "library",
    gate: false,
    description:
        "Every public static method on a Core Rules class (src/PineGuard.Core/Rules/*.cs) has at least one call site in src/PineGuard.MustClauses/*.cs.",
    async run(ctx: RuleContext): Promise<Finding[]> {
        const parse = ctx.parseFile ?? parseCsharpFile;

        const coreFiles = collectFilesUnder(ctx, CORE_RULES_DIR);
        const mustFiles = collectFilesUnder(ctx, MUST_CLAUSES_DIR);

        const coreMethods: CoreMethod[] = [];
        for (const file of coreFiles) {
            const { root } = await parse(file.absolutePath);
            coreMethods.push(...findCoreRulesMethods(root, file.relativePath));
        }

        const usedMemberPaths = new Set<string>();
        for (const file of mustFiles) {
            const { root } = await parse(file.absolutePath);
            for (const invocation of findInvocations(root)) {
                if (invocation.memberPath !== "") {
                    usedMemberPaths.add(invocation.memberPath);
                }
            }
        }

        const byQualifiedName = new Map<string, CoreMethod[]>();
        for (const method of coreMethods) {
            const group = byQualifiedName.get(method.qualifiedName);
            if (group) {
                group.push(method);
            } else {
                byQualifiedName.set(method.qualifiedName, [method]);
            }
        }

        const findings: Finding[] = [];
        for (const [qualifiedName, declarations] of [
            ...byQualifiedName.entries(),
        ].sort(([a], [b]) => a.localeCompare(b))) {
            if (usedMemberPaths.has(qualifiedName)) {
                continue;
            }

            const sorted = [...declarations].sort((a, b) => {
                if (a.file !== b.file) {
                    return a.file.localeCompare(b.file);
                }
                return a.line - b.line;
            });
            const first = sorted[0];
            if (!first) {
                continue;
            }

            const locations = sorted
                .map(
                    (declaration) =>
                        `${declaration.file}:${String(declaration.line)}`,
                )
                .join(", ");

            findings.push({
                rule: "rules-usage",
                file: first.file,
                line: first.line,
                message:
                    `Core Rules method '${qualifiedName}' has no call site anywhere in ` +
                    `${MUST_CLAUSES_DIR}/*.cs (declared at ${locations}).`,
                key: `rules-usage:${qualifiedName}`,
            });
        }

        return findings;
    },
});
