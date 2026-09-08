import { existsSync, readdirSync, statSync } from "node:fs";
import { join, relative } from "node:path";

import type { Node } from "web-tree-sitter";

import { registerRule } from "../catalog.js";
import {
    findInvocations,
    findMethodDeclarations,
    getModifiers,
    parseFile,
} from "../parsing/csharp.js";
import type { Finding, Rule, RuleContext } from "../types.js";

/**
 * `ordering` (legacy Rule08): cross-layer method ordering parity across
 * Must/Guard/FluentValidation/DataAnnotations/Rules.
 *
 * Faithful TypeScript port of `tools/audit-cli/solution/MethodOrderingAudit.cs`
 * (invoked by `tools/audit-cli/helpers/Test-SpecOrdering.ps1`, itself a thin
 * `dotnet run -- --audit ordering` wrapper — all the real logic lived in the
 * C# file). Plan `docs/ai/plans/audit-cli-rebuild.md` §4.3 (Rule08 row), §8
 * ("ordering" technical note), §9.2 P2.6.
 *
 * ## What "ordering" means here
 *
 * Every `Must<Tail>Clauses` type in `src/PineGuard.MustClauses` is a
 * "family". Its public static method *declaration order*, once each method
 * name is normalised down to a domain-and-affix-free concept key (see
 * {@link normalizeOperationName}/{@link normalizeBaseKey} below), is the
 * family's canonical order. Each sibling layer — Guard, FluentValidation,
 * DataAnnotations, Rules (Core) — is expected to declare/implement the same
 * concepts in the same relative order. A sibling's concept order is compared
 * against Must's only over the *intersection* of concepts both sides share
 * (a concept only one side has doesn't participate in the order check).
 *
 * ## Two deliberate behaviour changes from the legacy tool (plan §4.3/§8)
 *
 * 1. **A missing sibling layer is now a finding, not a warning.** The old
 *    tool printed "Missing GuardClauses sibling (skipped)" as a warning and
 *    kept its exit code green — real gaps were downgraded to noise (plan
 *    §2.2: "4 violations, 46 warnings"). Here, a family with no
 *    corresponding Guard/Fluent/DataAnnotations/Rules file (or an empty one)
 *    produces a `missing-layer` finding.
 * 2. **Guard ordering is keyed by the Must clause each Guard method
 *    delegates to, and an ambiguous delegation is itself a finding.** The old
 *    tool found the *first* `Must.`-rooted invocation in a Guard method's
 *    body and used it silently, even when a method invoked more than one
 *    distinct Must target (plan §2.2: "identifier string-matching, first
 *    match wins"). Here, the first distinct target is still used for the
 *    order comparison (so ordering keeps working), but a Guard method
 *    invoking **more than one** distinct `Must.` target now also produces an
 *    `ambiguous-delegation` finding — a Guard method should delegate to
 *    exactly one Must clause.
 *
 * ## Simplifications versus the C# original (documented, not silent)
 *
 * - The old tool's separate "concept set differs from Must (missing/extra)"
 *   warnings are dropped rather than ported as findings — porting them
 *   as blocking findings would reintroduce exactly the noise the rebuild
 *   plan calls Rule08 out for (plan §2.2: "Works; ... noisy"); the
 *   intersection-only order comparison already surfaces genuine reordering,
 *   and whole-layer absence is now its own finding.
 * - The `IsStringFamily`/nested-`StringRules` domain special case (Must
 *   families whose tail starts with `String`, e.g. `MustStringGuidClauses`)
 *   is not ported — it is a file-*discovery* quirk for one specific Rules
 *   layout, not part of the name-normalisation table itself, which is what
 *   this port is required to keep faithful. `domainName` here is simply the
 *   family's `tail`.
 * - Guard's alternate `MustXxxClauses.Yyy(Must.Be, ...)` delegation shape
 *   (passing `Must.Be` as an argument rather than calling `Must.Be.Xxx(...)`
 *   directly) is not recognised — every Guard method in the current codebase
 *   uses the direct-call shape (see any `Guard*Clauses.cs`), and
 *   {@link findInvocations} already covers it via its `"Must"` prefix match.
 * - The nested-class top-level check on DataAnnotations attribute types
 *   (`cls.Parent is BaseNamespaceDeclarationSyntax` in the original) is not
 *   ported; every `*Attributes.cs` file in the repo declares its attribute
 *   classes at namespace scope, not nested.
 *
 * See `test/rules/ordering.test.ts` and `test/fixtures/ordering/` for the
 * VIBE fixtures (plan §4.6) proving both behaviour changes and the
 * word-boundary-safe normalisation.
 */

const MUST_DIR = "src/PineGuard.MustClauses";
const GUARD_DIR = "src/PineGuard.GuardClauses";
const FLUENT_DIR = "src/PineGuard.FluentValidation";
const DATA_ANNOTATIONS_DIR = "src/PineGuard.DataAnnotations";
const RULES_DIR = "src/PineGuard.Core/Rules";

type SiblingLayer = "guard" | "fluent" | "dataAnnotations" | "rules";
type Layer = "must" | SiblingLayer;

const LAYER_LABELS: Record<SiblingLayer, string> = {
    guard: "GuardClauses",
    fluent: "FluentValidation",
    dataAnnotations: "DataAnnotations",
    rules: "Rules",
};

// ---------------------------------------------------------------------------
// Name normalisation table (ported from MethodOrderingAudit.cs, faithfully)
// ---------------------------------------------------------------------------

/**
 * The narrow whitelist that makes the Rules-layer `In`-prefix strip
 * word-boundary-safe: `InPast`/`InFuture`-shaped names mean "the Xxx
 * concept", but only for these four exact remainders — never a blind
 * "strip the substring `In`" (which would wrongly mangle a name like
 * `IsInstance`, see `test/fixtures/ordering/boundary/`).
 */
const IN_PREFIX_REMAINDERS = new Set([
    "Past",
    "PastOrPresent",
    "Future",
    "FutureOrPresent",
]);

/**
 * Strips layer-specific and domain-specific affixes from one raw member
 * name, word-boundary-safe throughout (every strip is a `startsWith`/
 * `endsWith` check with a length guard, never a substring replace).
 * Mirrors `MethodOrderingAudit.NormalizeOperationName` exactly, including
 * its two independent (not mutually exclusive) domain-affix checks.
 */
export function normalizeOperationName(
    memberName: string,
    domainName: string,
    layer: Layer,
): string {
    let name = memberName;

    if (layer === "rules" && name.startsWith("Is")) {
        name = name.slice(2);
    }

    // Rules occasionally use 'InXxx' to mean the 'Xxx' concept (e.g.
    // InPast). Only strip 'In' when the remainder is one of the known
    // date-shaped concepts — this is what keeps a name like `IsInstance`
    // (normalised to `Instance` by the `Is` strip above) intact instead of
    // being further mangled into `stance`.
    if (layer === "rules" && name.startsWith("In") && name.length > 2) {
        const remainder = name.slice(2);
        if (IN_PREFIX_REMAINDERS.has(remainder)) {
            name = remainder;
        }
    }

    if (layer === "dataAnnotations") {
        if (name.endsWith("StringAttribute")) {
            name = name.slice(0, -"StringAttribute".length);
        } else if (name.endsWith("Attribute")) {
            name = name.slice(0, -"Attribute".length);
        }
    }

    // Remove the domain name when it appears as a prefix (common in
    // DataAnnotations, e.g. CharControl) — and, independently, when it
    // appears as a suffix. Both checks run unconditionally in sequence
    // (not else-if), matching the original.
    if (
        domainName.length > 0 &&
        name.startsWith(domainName) &&
        name.length > domainName.length
    ) {
        name = name.slice(domainName.length);
    }

    if (domainName.length > 0 && name.endsWith(domainName)) {
        name = name.slice(0, -domainName.length);
    }

    return name;
}

/** Result of {@link tryStripNegativePrefix}. */
export interface NegativePrefixResult {
    readonly isNegative: boolean;
    readonly baseName: string;
}

/**
 * Recognises the three "this name is the forbidden/negative complement of
 * the real concept" prefixes — `Not`, `Non`, `Invalid` — each guarded so a
 * bare `"Invalid"` (or `"Not"`/`"Non"`) is left untouched rather than
 * stripped down to an empty string. Mirrors
 * `MethodOrderingAudit.TryStripNegativePrefix` exactly.
 */
export function tryStripNegativePrefix(
    operationName: string,
): NegativePrefixResult {
    if (operationName.startsWith("Not") && operationName.length > 3) {
        return { isNegative: true, baseName: operationName.slice(3) };
    }

    if (operationName.startsWith("Non") && operationName.length > 3) {
        return { isNegative: true, baseName: operationName.slice(3) };
    }

    if (operationName.startsWith("Invalid") && operationName.length > 7) {
        return { isNegative: true, baseName: operationName.slice(7) };
    }

    return { isNegative: false, baseName: operationName };
}

/** Mirrors `MethodOrderingAudit.NormalizeBaseKey`: the negative-complement name and its positive counterpart share one ordering key. */
export function normalizeBaseKey(operationName: string): string {
    const { isNegative, baseName } = tryStripNegativePrefix(operationName);
    return isNegative ? baseName : operationName;
}

/**
 * Normalises a raw, declaration-ordered member-name list down to a
 * deduplicated, order-preserving list of concept keys. Mirrors
 * `MethodOrderingAudit.BuildGroupOrder`.
 */
export function buildGroupOrder(
    memberNames: readonly string[],
    domainName: string,
    layer: Layer,
): string[] {
    const seen = new Set<string>();
    const ordered: string[] = [];

    for (const name of memberNames) {
        const operation = normalizeOperationName(name, domainName, layer);
        const key = normalizeBaseKey(operation);
        if (!seen.has(key)) {
            seen.add(key);
            ordered.push(key);
        }
    }

    return ordered;
}

function sequenceEqual(a: readonly string[], b: readonly string[]): boolean {
    return (
        a.length === b.length && a.every((value, index) => value === b[index])
    );
}

// ---------------------------------------------------------------------------
// File / tree discovery
//
// `ctx.trackedFiles` is populated on a real `pineguard audit` run (git
// ls-files-backed) but absent in the fixture harness (`{ rootDir }` only —
// see types.ts's RuleContext doc comment) — this rule works against both by
// falling back to a plain recursive walk of `rootDir` when trackedFiles is
// unavailable, same pattern as the P1.4 harness self-test's demo rule.
// ---------------------------------------------------------------------------

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

function listCsFilesUnder(ctx: RuleContext, relDir: string): string[] {
    if (ctx.trackedFiles) {
        const prefix = `${relDir}/`;
        return ctx.trackedFiles
            .filter((file) => file.startsWith(prefix) && file.endsWith(".cs"))
            .map((file) => join(ctx.rootDir, file));
    }

    const absoluteDir = join(ctx.rootDir, relDir);
    if (!existsSync(absoluteDir)) {
        return [];
    }
    return [...walk(absoluteDir)].filter((file) => file.endsWith(".cs"));
}

interface ClassInfo {
    readonly node: Node;
    readonly name: string;
    readonly modifiers: readonly string[];
}

function findClassDeclarations(root: Node): ClassInfo[] {
    return root.descendantsOfType("class_declaration").map((node) => {
        const nameNode = node.childForFieldName("name");
        return {
            node,
            name: nameNode?.text ?? "",
            modifiers: getModifiers(node),
        };
    });
}

function isPublicStatic(modifiers: readonly string[]): boolean {
    return modifiers.includes("public") && modifiers.includes("static");
}

function distinctPublicStaticMethodNames(classNode: Node): string[] {
    const seen = new Set<string>();
    const names: string[] = [];

    for (const method of findMethodDeclarations(classNode)) {
        if (!isPublicStatic(method.modifiers)) continue;
        if (!seen.has(method.name)) {
            seen.add(method.name);
            names.push(method.name);
        }
    }

    return names;
}

// ---------------------------------------------------------------------------
// Must family discovery
// ---------------------------------------------------------------------------

interface Family {
    readonly tail: string;
    readonly domainName: string;
    readonly mustTypeName: string;
    /** Repo/fixture-relative path (forward slashes), for findings. */
    readonly mustFilePath: string;
    readonly mustMemberNames: readonly string[];
}

async function discoverMustFamilies(ctx: RuleContext): Promise<Family[]> {
    const families: Family[] = [];

    for (const file of listCsFilesUnder(ctx, MUST_DIR)) {
        const parsed = await parseFile(file);

        for (const cls of findClassDeclarations(parsed.root)) {
            if (!isPublicStatic(cls.modifiers)) continue;
            if (!cls.name.startsWith("Must") || !cls.name.endsWith("Clauses")) {
                continue;
            }

            const tail = cls.name.slice(
                "Must".length,
                cls.name.length - "Clauses".length,
            );
            if (tail.length === 0) continue;

            const members = distinctPublicStaticMethodNames(cls.node);
            if (members.length === 0) continue;

            families.push({
                tail,
                domainName: tail,
                mustTypeName: cls.name,
                mustFilePath: relative(ctx.rootDir, file).replaceAll("\\", "/"),
                mustMemberNames: members,
            });
        }
    }

    return families;
}

// ---------------------------------------------------------------------------
// Findings
// ---------------------------------------------------------------------------

function missingLayerFinding(
    family: Family,
    layer: SiblingLayer,
    expectedPathRel: string,
): Finding {
    return {
        rule: "ordering",
        file: family.mustFilePath,
        message:
            `${family.mustTypeName}: missing ${LAYER_LABELS[layer]} sibling ` +
            `(expected ${expectedPathRel}) — cross-layer ordering cannot be ` +
            `verified for this family.`,
        key: `ordering:${family.mustTypeName}:${layer}:missing-layer`,
    };
}

function orderMismatchFinding(
    family: Family,
    layer: SiblingLayer,
    filePathRel: string,
    expectedFiltered: readonly string[],
    actualFiltered: readonly string[],
): Finding {
    return {
        rule: "ordering",
        file: filePathRel,
        message:
            `${family.mustTypeName}: ${LAYER_LABELS[layer]} order mismatch — ` +
            `expected [${expectedFiltered.join(", ")}], actual ` +
            `[${actualFiltered.join(", ")}] (relative to ${family.mustTypeName}'s ` +
            `declaration order).`,
        key: `ordering:${family.mustTypeName}:${layer}:order-mismatch`,
    };
}

function compareOrder(
    family: Family,
    layer: SiblingLayer,
    filePathRel: string,
    expected: readonly string[],
    expectedSet: ReadonlySet<string>,
    actual: readonly string[],
    findings: Finding[],
): void {
    const actualSet = new Set(actual);
    const expectedFiltered = expected.filter((key) => actualSet.has(key));
    const actualFiltered = actual.filter((key) => expectedSet.has(key));

    if (!sequenceEqual(expectedFiltered, actualFiltered)) {
        findings.push(
            orderMismatchFinding(
                family,
                layer,
                filePathRel,
                expectedFiltered,
                actualFiltered,
            ),
        );
    }
}

// ---------------------------------------------------------------------------
// Per-layer checks
// ---------------------------------------------------------------------------

/** Guard, Fluent and Rules share this shape: one file, one public static class, method names compared directly (no invocation resolution). DataAnnotations and Guard each need their own check (see below). */
async function checkMemberNameLayer(
    ctx: RuleContext,
    family: Family,
    expected: readonly string[],
    expectedSet: ReadonlySet<string>,
    findings: Finding[],
    layer: SiblingLayer,
    dir: string,
    className: string,
): Promise<void> {
    const filePathRel = `${dir}/${className}.cs`;
    const filePathAbs = join(ctx.rootDir, filePathRel);

    if (!existsSync(filePathAbs)) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const parsed = await parseFile(filePathAbs);
    const cls = findClassDeclarations(parsed.root).find(
        (candidate) =>
            candidate.name === className && isPublicStatic(candidate.modifiers),
    );
    if (!cls) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const memberNames = distinctPublicStaticMethodNames(cls.node);
    if (memberNames.length === 0) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const actual = buildGroupOrder(memberNames, family.domainName, layer);
    compareOrder(
        family,
        layer,
        filePathRel,
        expected,
        expectedSet,
        actual,
        findings,
    );
}

async function checkDataAnnotationsLayer(
    ctx: RuleContext,
    family: Family,
    expected: readonly string[],
    expectedSet: ReadonlySet<string>,
    findings: Finding[],
): Promise<void> {
    const layer: SiblingLayer = "dataAnnotations";
    const filePathRel = `${DATA_ANNOTATIONS_DIR}/${family.tail}Attributes.cs`;
    const filePathAbs = join(ctx.rootDir, filePathRel);

    if (!existsSync(filePathAbs)) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const parsed = await parseFile(filePathAbs);
    const seen = new Set<string>();
    const names: string[] = [];

    for (const cls of findClassDeclarations(parsed.root)) {
        if (!cls.modifiers.includes("public")) continue;
        if (!cls.name.endsWith("Attribute")) continue;
        if (!seen.has(cls.name)) {
            seen.add(cls.name);
            names.push(cls.name);
        }
    }

    if (names.length === 0) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const actual = buildGroupOrder(names, family.domainName, layer);
    compareOrder(
        family,
        layer,
        filePathRel,
        expected,
        expectedSet,
        actual,
        findings,
    );
}

/**
 * Guard ordering is compared by the Must clause each Guard method invokes,
 * not by the Guard method's own name (behaviour change #2, see file header).
 * A Guard method invoking more than one distinct `Must.`-rooted target is an
 * `ambiguous-delegation` finding; the first distinct target found (in source
 * order) is still used as that method's comparable name either way, so
 * ordering keeps working even while the ambiguity is flagged.
 */
async function checkGuardLayer(
    ctx: RuleContext,
    family: Family,
    expected: readonly string[],
    expectedSet: ReadonlySet<string>,
    findings: Finding[],
): Promise<void> {
    const layer: SiblingLayer = "guard";
    const className = `Guard${family.tail}Clauses`;
    const filePathRel = `${GUARD_DIR}/${className}.cs`;
    const filePathAbs = join(ctx.rootDir, filePathRel);

    if (!existsSync(filePathAbs)) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const parsed = await parseFile(filePathAbs);
    const cls = findClassDeclarations(parsed.root).find(
        (candidate) =>
            candidate.name === className && isPublicStatic(candidate.modifiers),
    );
    if (!cls) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const seenComparable = new Set<string>();
    const comparableMembers: string[] = [];

    for (const method of findMethodDeclarations(cls.node)) {
        if (!isPublicStatic(method.modifiers)) continue;

        const mustInvocations = findInvocations(method.node, "Must");
        const seenTargets = new Set<string>();
        const distinctTargets: string[] = [];
        for (const invocation of mustInvocations) {
            const invoked = invocation.memberPath.split(".").pop();
            if (invoked && !seenTargets.has(invoked)) {
                seenTargets.add(invoked);
                distinctTargets.push(invoked);
            }
        }

        if (distinctTargets.length > 1) {
            findings.push({
                rule: "ordering",
                file: filePathRel,
                line: method.node.startPosition.row + 1,
                message:
                    `${className}.${method.name}: ambiguous Must delegation — ` +
                    `invokes ${String(distinctTargets.length)} distinct Must targets ` +
                    `(${distinctTargets.join(", ")}); a Guard method should delegate ` +
                    `to exactly one Must clause.`,
                key: `ordering:${className}:${method.name}:ambiguous-delegation`,
            });
        }

        const comparable = distinctTargets[0] ?? method.name;
        if (!seenComparable.has(comparable)) {
            seenComparable.add(comparable);
            comparableMembers.push(comparable);
        }
    }

    if (comparableMembers.length === 0) {
        findings.push(missingLayerFinding(family, layer, filePathRel));
        return;
    }

    const actual = buildGroupOrder(comparableMembers, family.domainName, layer);
    compareOrder(
        family,
        layer,
        filePathRel,
        expected,
        expectedSet,
        actual,
        findings,
    );
}

// ---------------------------------------------------------------------------
// Rule
// ---------------------------------------------------------------------------

async function run(ctx: RuleContext): Promise<Finding[]> {
    const findings: Finding[] = [];
    const families = await discoverMustFamilies(ctx);
    families.sort((a, b) =>
        a.mustTypeName < b.mustTypeName
            ? -1
            : a.mustTypeName > b.mustTypeName
              ? 1
              : 0,
    );

    for (const family of families) {
        const expected = buildGroupOrder(
            family.mustMemberNames,
            family.domainName,
            "must",
        );
        const expectedSet = new Set(expected);

        await checkGuardLayer(ctx, family, expected, expectedSet, findings);
        await checkMemberNameLayer(
            ctx,
            family,
            expected,
            expectedSet,
            findings,
            "fluent",
            FLUENT_DIR,
            `Fluent${family.tail}Extensions`,
        );
        await checkDataAnnotationsLayer(
            ctx,
            family,
            expected,
            expectedSet,
            findings,
        );
        await checkMemberNameLayer(
            ctx,
            family,
            expected,
            expectedSet,
            findings,
            "rules",
            RULES_DIR,
            `${family.tail}Rules`,
        );
    }

    return findings;
}

/** The bare `Rule` — imported directly by `test/rules/ordering.test.ts` per the VIBE harness convention (`test/README.md`). */
export const orderingRule: Rule = {
    slug: "ordering",
    run,
};

registerRule({
    ...orderingRule,
    legacyId: "Rule08",
    scope: "library",
    gate: false,
    description:
        "Must/Guard/FluentValidation/DataAnnotations/Rules declare the same validation concepts in the same relative order.",
});
