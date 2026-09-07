import { readdirSync, statSync } from "node:fs";
import path from "node:path";

import type { Node } from "web-tree-sitter";

import { registerRule } from "../catalog.js";
import { getModifiers, parseFile } from "../parsing/csharp.js";
import type { Finding, Rule, RuleContext } from "../types.js";

/**
 * `test-structure` (legacy `Rule51`) — v11 §4-5 TestData/Tests structural
 * conformance.
 *
 * ## This is a rewrite, not a port
 *
 * The old PowerShell `Rule51`
 * (`tools/audit-cli/rules/Test-Rule51-UnitTestClassSemanticStructure.ps1`)
 * required every `*Tests.cs` to nest `public static class` "groups" and
 * flagged a *top-level* test method as a violation — that is the *pre-v11*
 * shape. `docs/ai/specs/testing/unit-test.md` v11 §5.1 inverted this: the
 * Tests class is **flat** (test methods declared directly on it); Operation
 * Groups (`public static class` per member under test, §4.1) belong in the
 * TestData file only, and each one is consumed by exactly one
 * `<Group>_BehavesAsExpected` test method (§4.5). Every one of the old
 * rule's 3,961 findings was therefore the rule itself being stale, not real
 * drift (plan `docs/ai/plans/audit-cli-rebuild.md` §2.2/§2.3/§8, row
 * "Rule51 → test-structure": "Rewritten to v11 §4-5, not a port of Rule51").
 * This module does not reuse any of the old script's pass/fail logic —
 * only its general mechanism (line-oriented brace counting, regex-matched
 * `class`/attribute lines) was worth understanding, and this rewrite
 * replaces that with a real AST walk over `web-tree-sitter` instead.
 *
 * ## What this rule checks, per `*Tests.cs` / `*TestData.cs` pair
 *
 * Pairing is re-derived here (same directory, same base name, `Tests.cs` /
 * `TestData.cs` suffix) rather than depended on from the sibling
 * `test-files` rule (plan P2.10) — a pair with no `*TestData.cs` sibling is
 * silently skipped; that gap is `test-files`' (Rule50's) concern.
 *
 * 1. **Flatness** (§5.1): the Tests class nests no `public static class`.
 * 2. **Structural correspondence** (§4.5): every TestData Operation Group
 *    has a `<Group>_BehavesAsExpected` method in Tests, and those methods
 *    appear in the same order as their Operation Groups.
 * 3. **No orphans** (§4.5): every `_BehavesAsExpected` method in Tests maps
 *    back to a real TestData Operation Group.
 * 4. **Dataset ordering + emptiness within an Operation Group** (§4.4,
 *    §4.1): `ValidCases` → `EdgeCases` → `InvalidCases` when more than one
 *    is present in a group; no `=> [];` empty scaffolding for any
 *    recognised dataset (`Cases`, `ValidCases`, `EdgeCases`, `InvalidCases`).
 * 5. **Sealed Tests class** (§5.1): "Test class `XxxTests` must be `sealed`".
 * 6. **Instance test methods** (§5.1): "Test methods must be **instance**
 *    methods declared `public void`" — a `static` test method is a finding.
 *    Every one of the five layer addenda repeats this
 *    ("Instance methods — `public void` (not `public static void`)").
 *
 * A single-dataset rollup (`Cases`, per the Core/Fluent/DataAnnotations
 * layer addenda in §4.1's table) has nothing to order against itself, so it
 * is exempt from check 4's ordering half — but still subject to the
 * emptiness half.
 *
 * ## Two spec-sanctioned shapes checks 2 and 3 must not flag (P4.3)
 *
 * 1. **Underscores inside the member half of a method name.** The
 *    FluentValidation addendum's "Nullable vs Non-Nullable Variants" section
 *    pairs Operation Group `EvenNonNullable` with test method
 *    `Even_NonNullable_BehavesAsExpected`
 *    (`docs/ai/specs/fluent-validation/unit-test.md`) — the group name and the
 *    method's member half are the same identifier, differing only in
 *    underscores. The repo follows it in 35 places
 *    (`IsDefault_Int32_BehavesAsExpected` <-> `IsDefaultInt32`, …). Group and
 *    method are therefore matched with `_` removed from both sides.
 * 2. **A throws-only Operation Group.** §5.1 says "Throwing behaviour is
 *    **not** a separate method" *for the layer bases*, but §8.3's own
 *    canonical example splits `Parse` into a
 *    `_BehavesAsExpected` / `_ThrowsAsExpected` pair, and the DataAnnotations
 *    addendum's "Pattern E — TypeMismatch Throws" defines a group whose only
 *    method is `<Attr>_TypeMismatch_ThrowsExpected`. A group is therefore
 *    satisfied by a `<Group>…Throws[As]Expected` method as well as by
 *    `<Group>_BehavesAsExpected`. Nothing else satisfies it: a group whose
 *    only method is `<Group>_ShouldReturnExpected` is still a finding, which
 *    is exactly what §5.1's "Do **NOT** use `ShouldReturnExpected`,
 *    `ShouldThrowExpected`, `ReturnsExpected`, or any other naming pattern"
 *    requires.
 *
 * ## `NullCases` / `AdHocCases` — conditionally recognised dataset names (P4.3 finding #5)
 *
 * `unit-test.md` §4.1 states dataset names are `ValidCases`/`EdgeCases`/
 * `InvalidCases` "never any other names", but `fixture.md` — which its own
 * header says wins where the two differ — uses two more itself:
 * `NullCases` (§12.2: an inverted-guard nullable variant splits its null
 * scenario into its own dataset because it needs a distinct
 * `ArgumentNullException` expectation, and only ever appears *alongside* an
 * explicit `ValidCases`/`InvalidCases` pair — never on its own) and
 * `AdHocCases` (§11.6: "layer-specific cases not derivable from
 * RuleScenarios", which by definition *supplements* cases that already are
 * RuleScenario-derived, i.e. a `Cases`/`ValidCases`/`InvalidCases` property
 * in the same group). Check 4's dataset-name recognition (below) now accepts
 * both names, but only under the co-occurrence condition each spec section
 * actually shows — never unconditionally, and never as a replacement for the
 * mandatory split. A `NullCases`/`AdHocCases` property that appears without
 * its required companion is simply ignored, exactly as any other
 * unrecognised name is today (no "unknown dataset name" check exists to flag
 * it) — this closes the *false-negative* gap (an empty `NullCases => [];`
 * silently passing check 4) without introducing a new false-positive.
 *
 * See `apps/cli/test/fixtures/test-structure/` for VIBE fixtures (plan
 * §4.6) and `apps/cli/test/rules/test-structure.test.ts` for the assertions.
 */

const SLUG = "test-structure";
const BEHAVES_SUFFIX = "_BehavesAsExpected";
const DATASET_ORDER = ["ValidCases", "EdgeCases", "InvalidCases"] as const;
const RECOGNISED_DATASET_NAMES = new Set<string>(["Cases", ...DATASET_ORDER]);

/**
 * `fixture.md`-sanctioned supplemental dataset names — see this module's
 * header note "`NullCases` / `AdHocCases` — conditionally recognised dataset
 * names". Each is recognised only under the condition named there, never
 * unconditionally.
 */
function isRecognisedDataset(
    name: string,
    groupPropertyNames: ReadonlySet<string>,
): boolean {
    if (RECOGNISED_DATASET_NAMES.has(name)) return true;

    if (name === "NullCases") {
        // fixture.md §12.2: only ever appears alongside an explicit
        // ValidCases/InvalidCases pair (the inverted-guard nullable-variant
        // shape), never replacing it.
        return (
            groupPropertyNames.has("ValidCases") &&
            groupPropertyNames.has("InvalidCases")
        );
    }

    if (name === "AdHocCases") {
        // fixture.md §11.6: supplements cases that ARE RuleScenario-derived
        // — i.e. it never appears in a group with no derived dataset at all.
        return (
            groupPropertyNames.has("Cases") ||
            groupPropertyNames.has("ValidCases") ||
            groupPropertyNames.has("InvalidCases")
        );
    }

    return false;
}

/** The spec-sanctioned throws-companion suffix (`_ThrowsAsExpected` per §8.3, `_ThrowsExpected` per the DataAnnotations addendum's Pattern E). */
const THROWS_SUFFIX = /Throws(As)?Expected$/;

/** Compares group names to method names ignoring `_` — see this module's header, note 1 (the FluentValidation addendum's `EvenNonNullable` <-> `Even_NonNullable_BehavesAsExpected` pairing). */
function squash(name: string): string {
    return name.replaceAll("_", "");
}

/** Rank of a dataset name in the §4.4 canonical order, or `-1` if it isn't one of the three ordered names (e.g. the single-rollup `Cases`). */
function datasetRank(name: string): number {
    return DATASET_ORDER.indexOf(name as (typeof DATASET_ORDER)[number]);
}

/**
 * Recursively lists every file under `dir`, relative to `rootDir` and
 * forward-slash-normalised. Fallback for the fixture harness, which only
 * populates `ctx.rootDir` — a real `pineguard audit` run always has
 * `ctx.trackedFiles` (via `git ls-files`, see `src/audit/repo.ts`) and this
 * function is never called there.
 */
function listFilesRecursively(dir: string, rootDir: string): string[] {
    const out: string[] = [];
    for (const entry of readdirSync(dir)) {
        const full = path.join(dir, entry);
        if (statSync(full).isDirectory()) {
            out.push(...listFilesRecursively(full, rootDir));
        } else {
            out.push(path.relative(rootDir, full).replaceAll("\\", "/"));
        }
    }
    return out;
}

interface TestPair {
    readonly testsRel: string;
    readonly testDataRel: string;
}

/**
 * Pairs every `*Tests.cs` with a same-directory, same-base-name
 * `*TestData.cs` among `relFiles`. Simple by design (per this rule's own
 * brief: "re-derive the pairing lookup yourself, it's simple") — a
 * `*Tests.cs` with no such sibling is silently skipped.
 */
function findTestPairs(relFiles: readonly string[]): TestPair[] {
    const relSet = new Set(relFiles);
    const pairs: TestPair[] = [];
    for (const rel of relFiles) {
        if (!rel.endsWith("Tests.cs")) continue;
        if (rel.includes("/bin/") || rel.includes("/obj/")) continue;

        const dir = path.posix.dirname(rel);
        const base = path.posix.basename(rel).slice(0, -"Tests.cs".length);
        const testDataRel =
            dir === "." ? `${base}TestData.cs` : `${dir}/${base}TestData.cs`;

        if (relSet.has(testDataRel)) {
            pairs.push({ testsRel: rel, testDataRel });
        }
    }
    return pairs;
}

function isTopLevelClass(node: Node): boolean {
    let parent = node.parent;
    while (parent !== null) {
        if (parent.type === "class_declaration") return false;
        parent = parent.parent;
    }
    return true;
}

/** The single outermost `class_declaration` in a file (`XxxTests` / `XxxTestData`) — works for both block-scoped and file-scoped namespace style. */
function findOuterClass(root: Node): Node | undefined {
    return root.descendantsOfType("class_declaration").find(isTopLevelClass);
}

/** Direct (non-recursive) children of `classNode`'s body matching `type`. */
function directChildrenOfType(classNode: Node, type: string): Node[] {
    const body = classNode.childForFieldName("body");
    if (body === null) return [];
    return body.namedChildren.filter((child) => child.type === type);
}

function nameOf(node: Node): string {
    return node.childForFieldName("name")?.text ?? "";
}

function lineOf(node: Node): number {
    return node.startPosition.row + 1;
}

function isPublicStaticClass(node: Node): boolean {
    const modifiers = getModifiers(node);
    return modifiers.includes("public") && modifiers.includes("static");
}

/** `true` for a dataset property whose whole body is the empty collection expression `[]` (the `=> [];` scaffolding §4.1 forbids). */
function isEmptyDataset(property: Node): boolean {
    const value = property.childForFieldName("value");
    if (value === null || value.type !== "arrow_expression_clause") {
        return false;
    }
    const expr = value.namedChild(0);
    return (
        expr !== null &&
        expr.type === "collection_expression" &&
        expr.namedChildCount === 0
    );
}

/** Consecutive `[item, next]` pairs of `items`, skipping any `undefined` a `noUncheckedIndexedAccess` read could produce (never happens in practice here; kept defensive). */
function adjacentPairs<T>(items: readonly T[]): [T, T][] {
    const pairs: [T, T][] = [];
    for (let i = 0; i + 1 < items.length; i++) {
        const current = items[i];
        const next = items[i + 1];
        if (current !== undefined && next !== undefined) {
            pairs.push([current, next]);
        }
    }
    return pairs;
}

function analyzePair(
    pair: TestPair,
    testDataRoot: Node,
    testsRoot: Node,
): Finding[] {
    const findings: Finding[] = [];

    const testDataClass = findOuterClass(testDataRoot);
    const testsClass = findOuterClass(testsRoot);
    if (testDataClass === undefined || testsClass === undefined) {
        return findings;
    }

    // ---- 1. Flatness: Tests nests no `public static class` anywhere in its body ----
    const testsBody = testsClass.childForFieldName("body");
    const nestedGroups = (
        testsBody?.descendantsOfType("class_declaration") ?? []
    ).filter(isPublicStaticClass);
    for (const nested of nestedGroups) {
        findings.push({
            rule: SLUG,
            file: pair.testsRel,
            line: lineOf(nested),
            message: `Tests class '${nameOf(testsClass)}' nests 'public static class ${nameOf(nested)}' — the Tests class must be flat; Operation Groups belong in TestData only (v11 §5.1).`,
            key: `${SLUG}:flat:${pair.testsRel}:${lineOf(nested)}`,
        });
    }

    // ---- 5. The Tests class must be `sealed` (§5.1) ----
    if (!getModifiers(testsClass).includes("sealed")) {
        findings.push({
            rule: SLUG,
            file: pair.testsRel,
            line: lineOf(testsClass),
            message: `Test class '${nameOf(testsClass)}' is not sealed — v11 §5.1 requires 'XxxTests' to be sealed.`,
            key: `${SLUG}:not-sealed:${pair.testsRel}`,
        });
    }

    // ---- Operation Groups (TestData) vs. `_BehavesAsExpected` methods (Tests) ----
    const groupNodes = directChildrenOfType(testDataClass, "class_declaration");
    const groupNames = groupNodes.map(nameOf);
    const squashedGroupNames = new Set(groupNames.map(squash));

    const methodNodes = directChildrenOfType(testsClass, "method_declaration");
    const allMethods = methodNodes.map((node) => ({
        node,
        name: nameOf(node),
    }));
    const behavesMethods = allMethods.filter((m) =>
        m.name.endsWith(BEHAVES_SUFFIX),
    );

    // ---- 6. Test methods must be instance methods, never `static` (§5.1) ----
    for (const m of allMethods) {
        if (
            m.name.includes("_") &&
            getModifiers(m.node).includes("static") &&
            getModifiers(m.node).includes("public")
        ) {
            findings.push({
                rule: SLUG,
                file: pair.testsRel,
                line: lineOf(m.node),
                message: `Test method '${m.name}' is 'public static' — v11 §5.1 requires instance 'public void' methods so the layer base's AssertResult is in scope.`,
                key: `${SLUG}:static-method:${pair.testsRel}:${m.name}`,
            });
        }
    }

    const indexByBase = new Map<string, number>();
    behavesMethods.forEach((m, i) => {
        const base = squash(m.name.slice(0, -BEHAVES_SUFFIX.length));
        if (!indexByBase.has(base)) {
            indexByBase.set(base, i);
        }
    });

    /** Names of the throws-companion methods that satisfy `group` (header note 2). */
    function throwsCompanionsFor(group: string): string[] {
        const prefix = squash(group);
        return allMethods
            .filter(
                (m) =>
                    THROWS_SUFFIX.test(m.name) &&
                    squash(m.name).startsWith(prefix),
            )
            .map((m) => m.name);
    }

    // ---- 2. Missing test methods ----
    for (const group of groupNames) {
        if (indexByBase.has(squash(group))) continue;
        if (throwsCompanionsFor(group).length > 0) continue;

        findings.push({
            rule: SLUG,
            file: pair.testsRel,
            line: lineOf(testsClass),
            message: `TestData Operation Group '${group}' has no corresponding '${group}${BEHAVES_SUFFIX}' method in '${nameOf(testsClass)}' (v11 §4.5).`,
            key: `${SLUG}:missing:${pair.testsRel}:${group}`,
        });
    }

    // ---- 3. Orphan test methods ----
    for (const m of behavesMethods) {
        const base = m.name.slice(0, -BEHAVES_SUFFIX.length);
        if (!squashedGroupNames.has(squash(base))) {
            findings.push({
                rule: SLUG,
                file: pair.testsRel,
                line: lineOf(m.node),
                message: `Test method '${m.name}' has no corresponding 'public static class ${base}' Operation Group in TestData — orphaned (v11 §4.5).`,
                key: `${SLUG}:orphan:${pair.testsRel}:${lineOf(m.node)}`,
            });
        }
    }

    // ---- 2b. Ordering: matched methods must appear in the same order as their Operation Groups ----
    const presentPairs: { group: string; index: number }[] = [];
    for (const group of groupNames) {
        const index = indexByBase.get(squash(group));
        if (index !== undefined) {
            presentPairs.push({ group, index });
        }
    }
    for (const [current, next] of adjacentPairs(presentPairs)) {
        if (current.index > next.index) {
            const offending = behavesMethods[next.index];
            if (offending !== undefined) {
                findings.push({
                    rule: SLUG,
                    file: pair.testsRel,
                    line: lineOf(offending.node),
                    message: `Test method '${offending.name}' is out of order: TestData declares Operation Group '${current.group}' before '${next.group}', but '${next.group}${BEHAVES_SUFFIX}' appears before '${current.group}${BEHAVES_SUFFIX}' in Tests (v11 §4.5).`,
                    key: `${SLUG}:order:${pair.testsRel}:${lineOf(offending.node)}`,
                });
            }
        }
    }

    // ---- 4. Dataset ordering + emptiness, per Operation Group ----
    for (const group of groupNodes) {
        const groupName = nameOf(group);
        const allGroupProperties = directChildrenOfType(
            group,
            "property_declaration",
        ).map((node) => ({ node, name: nameOf(node) }));
        const groupPropertyNames = new Set(
            allGroupProperties.map((p) => p.name),
        );
        const properties = allGroupProperties.filter((p) =>
            isRecognisedDataset(p.name, groupPropertyNames),
        );

        for (const p of properties) {
            if (isEmptyDataset(p.node)) {
                findings.push({
                    rule: SLUG,
                    file: pair.testDataRel,
                    line: lineOf(p.node),
                    message: `Operation Group '${groupName}' declares an empty dataset '${p.name} => [];' — omit datasets with no cases instead of scaffolding an empty one (v11 §4.1).`,
                    key: `${SLUG}:empty:${pair.testDataRel}:${lineOf(p.node)}`,
                });
            }
        }

        const ordered = properties.filter((p) => datasetRank(p.name) >= 0);
        for (const [a, b] of adjacentPairs(ordered)) {
            if (datasetRank(a.name) > datasetRank(b.name)) {
                findings.push({
                    rule: SLUG,
                    file: pair.testDataRel,
                    line: lineOf(a.node),
                    message: `Operation Group '${groupName}' declares '${a.name}' before '${b.name}' — v11 §4.4 requires ValidCases → EdgeCases → InvalidCases.`,
                    key: `${SLUG}:dataset-order:${pair.testDataRel}:${lineOf(a.node)}`,
                });
            }
        }
    }

    return findings;
}

async function run(ctx: RuleContext): Promise<Finding[]> {
    const relFiles =
        ctx.trackedFiles ?? listFilesRecursively(ctx.rootDir, ctx.rootDir);
    const pairs = findTestPairs(relFiles);
    const findings: Finding[] = [];

    for (const pair of pairs) {
        const testDataParsed = await parseFile(
            path.join(ctx.rootDir, pair.testDataRel),
        );
        const testsParsed = await parseFile(
            path.join(ctx.rootDir, pair.testsRel),
        );
        findings.push(
            ...analyzePair(pair, testDataParsed.root, testsParsed.root),
        );
    }

    return findings;
}

/** Exported directly (in addition to being registered below) so `test/rules/test-structure.test.ts` can run it against fixtures via `test/support/runRule.ts`, per the convention in `test/README.md`. */
export const testStructureRule: Rule = { slug: SLUG, run };

registerRule({
    ...testStructureRule,
    legacyId: "Rule51",
    scope: "testing",
    gate: false,
    description:
        "TestData/Tests structural conformance to v11 §4-5: sealed, flat Tests class with instance test methods, one <Group>_BehavesAsExpected per Operation Group in order, no orphans, ValidCases→EdgeCases→InvalidCases dataset ordering with no empty scaffolding.",
});
