import { existsSync, readdirSync } from "node:fs";
import path from "node:path";

import { registerRule } from "../catalog.js";
import {
    findInvocations,
    findMethodDeclarations,
    parseFile,
} from "../parsing/csharp.js";
import type { Finding, Rule, RuleContext } from "../types.js";

/**
 * `must-usage` — plan `docs/ai/plans/audit-cli-rebuild.md` §4.3 row
 * "Rule03/04/05 → `must-usage`", §8 technical note, §2.2 bug history, §9.2
 * P2.2.
 *
 * Merges the three legacy PowerShell rules — Rule03 (Must → Guard usage),
 * Rule04 (Must → Fluent usage), Rule05 (Must → DataAnnotations usage) —
 * into one rule that checks all three layers and reports the union of their
 * findings, one finding per (layer, unused Must method) pair.
 *
 * ## The bug this rule exists to not repeat (plan §2.2)
 *
 * The legacy helpers (`tools/audit-cli/helpers/Find-UnusedMustGuard.ps1`
 * and its `Find-UnusedMustFluent.ps1`/`Find-UnusedMustData.ps1` siblings)
 * filtered candidate Must-clause types with the single-quoted regex literal
 * `'^PineGuard\\.MustClauses'`. PowerShell single-quoted strings do not
 * process backslash escapes, so the doubled backslash reached the regex
 * engine unchanged: `\\` there means "match one literal backslash
 * character" — a byte sequence no real .NET namespace ever contains. The
 * type filter therefore matched **zero** types, every run, so
 * "Total Must Methods Found: 0" and all three rules printed PASS
 * unconditionally for months.
 *
 * This rule sidesteps that entire bug *category* by never using a regex to
 * identify a Must clause or a call site: it walks a real tree-sitter parse
 * tree (`findMethodDeclarations` / `findInvocations` from
 * `../parsing/csharp.ts`) and matches on parsed AST shapes — a method's
 * modifiers and its first parameter's type text; a call site's parsed
 * member-access chain — never on a hand-escaped pattern string. The
 * `invalid/` and `boundary/` fixtures (`test/fixtures/must-usage/`) exist
 * specifically to prove this produces a nonzero, *correct* count, unlike
 * the tool it replaces.
 *
 * ## What counts as a "Must clause"
 *
 * Every `public static` method declared anywhere in
 * `src/PineGuard.MustClauses/*.cs` whose first parameter carries the `this`
 * modifier and has type text exactly `IMustClause` — the extension-method
 * shape every real Must clause uses (`public static MustResult<T> Xxx(this
 * IMustClause _, ...)`). Verified empirically against the real project
 * before writing this rule: all 607 public static methods across the 56
 * files in `src/PineGuard.MustClauses` share this exact first-parameter
 * shape (`grep -c 'public static' src/PineGuard.MustClauses/*.cs` sums to
 * 663 = 607 methods + 56 `public static class` declarations; every one of
 * the 607 methods reads `this IMustClause _`). Generic Must methods (e.g.
 * `BitwiseEqualTo<T>`) still resolve to their simple name via
 * `findMethodDeclarations` — the grammar's `name` field excludes the
 * `<T>` type-parameter list.
 *
 * ## What counts as "used", per layer
 *
 * A real `invocation_expression` anywhere under a layer's own source
 * directory whose dotted call target is `Must.Be.<Name>` (via
 * `findInvocations(root, "Must.Be")`) — never a textual mention. A method
 * name that appears only inside a comment or a string literal is not an
 * `invocation_expression` node at all (tree-sitter never parses comment or
 * string contents as expressions), so it is correctly never counted as a
 * call site — see `test/fixtures/must-usage/boundary/`.
 *
 * Note on the plan's own prose: §8 describes call targets as `Must.Be.<Name>`
 * / `Must.Not.Be.<Name>`, but the real API has no `Must.Not` member at all —
 * confirmed via `grep -rn "Must\.Not\b" src/ tests/` (zero hits) before
 * writing this rule. Negation lives in the Must method's own *name*
 * (`NotXxx`, e.g. `Must.Be.NotBitwiseEqualTo(...)`), not in a separate
 * `Must.Not` chain, so only `Must.Be.<Name>` is ever searched for.
 *
 * Layer directories (confirmed via `git ls-files` against the real repo
 * before writing this rule, since the plan's own prose only guesses at two
 * of the three and hedges on the third):
 *
 * - `guard`       → `src/PineGuard.GuardClauses/`
 * - `fluent`       → `src/PineGuard.FluentValidation/`
 * - `annotations` → `src/PineGuard.DataAnnotations/`
 *
 * ## Deliberately dropped from the legacy logic: the complement heuristic
 *
 * `Find-UnusedMustGuard.ps1` additionally treated a `NotXxx`/`Xxx` pair as
 * mutually "covering" each other ("Guard often uses complements"). Plan
 * §8's technical note for `must-usage` does not carry that heuristic
 * forward — it says only "report each Must name with zero call sites", with
 * no mention of complement forgiveness — so this rule checks each Must
 * method name for its own literal call site, per layer, with no
 * complement-pair leniency. This is a stricter, simpler, and unambiguous
 * replacement for the legacy heuristic; it will surface real (if
 * previously-hidden) debt when run against the live repo, which is exactly
 * what the baseline ratchet (plan §4.4) exists to absorb rather than block
 * on immediately (`gate: false` below).
 *
 * ## `--layer` CLI flag (plan §4.3) — not wired here
 *
 * The plan names a future `--layer guard|fluent|annotations` CLI flag
 * (default: all three). Neither `RuleContext` nor `CatalogEntry` (plan
 * P1.5, `../types.ts` / `../catalog.ts`) has a mechanism yet for a
 * rule-specific CLI option to reach `run(ctx)`, and `src/commands/audit.ts`
 * has no per-rule option plumbing either — adding that would mean editing
 * shared engine/catalog/command files outside this task's scope (P2.2 owns
 * only its own new files, and 13 sibling agents are editing in parallel).
 * This rule therefore always checks all three layers unconditionally; a
 * real `--layer` flag is left as a follow-up once the engine grows
 * per-rule option support.
 *
 * ## Diagnostic finding when zero Must methods are found (P4.1 remediation)
 *
 * `collectMustMethods` returning an empty array should never happen against
 * the real repo — 607 methods match today (see above) — but if it ever did
 * (a tree-sitter grammar change, a moved `MustClauses` directory, a future
 * refactor of `isMustExtensionMethod`'s shape check), the previous code
 * returned `[]` immediately, silently reporting a clean audit. That is
 * structurally the *exact same failure shape* as the legacy Rule03/04/05 bug
 * this module exists to not repeat (see the module header above): "I found
 * zero Must clauses, therefore zero are unused, PASS" — even though the
 * *cause* would be different (a real detection failure, not a regex
 * quoting bug). P4.1's parity diff flagged this at what was then
 * `must-usage.ts:249` (`docs/ai/plans/audit-cli-rebuild.md` §9.2 row P4.1,
 * §3.1 item 2). A rule that cannot find its own subject population must emit
 * a diagnostic finding instead of returning clean — see `run()` below and
 * `test/fixtures/must-usage/no-subjects-found/` for the regression fixture.
 */

const MUST_CLAUSES_DIR = "src/PineGuard.MustClauses";

interface LayerSpec {
    readonly id: "guard" | "fluent" | "annotations";
    readonly dir: string;
    readonly label: string;
}

const LAYERS: readonly LayerSpec[] = [
    { id: "guard", dir: "src/PineGuard.GuardClauses", label: "GuardClauses" },
    {
        id: "fluent",
        dir: "src/PineGuard.FluentValidation",
        label: "FluentValidation",
    },
    {
        id: "annotations",
        dir: "src/PineGuard.DataAnnotations",
        label: "DataAnnotations",
    },
];

interface MustMethodInfo {
    readonly name: string;
    readonly file: string;
    readonly line: number;
}

/** Recursively lists every `.cs` file under `dir`, skipping `bin`/`obj` build output. */
function* walkCsFiles(dir: string): Generator<string> {
    for (const entry of readdirSync(dir, { withFileTypes: true })) {
        if (entry.name === "bin" || entry.name === "obj") continue;
        const full = path.join(dir, entry.name);
        if (entry.isDirectory()) {
            yield* walkCsFiles(full);
        } else if (entry.name.endsWith(".cs")) {
            yield full;
        }
    }
}

/**
 * Absolute paths of every `.cs` file under the repo-relative `relDir`. Uses
 * `ctx.trackedFiles` when the engine populated it (a real `pineguard audit`
 * run — see `engine.ts`'s `buildContext`); falls back to a direct
 * filesystem walk otherwise, which is what the test harness's fixture
 * `RuleContext` needs (it only ever sets `rootDir` — see
 * `test/support/runRule.ts`).
 */
function listCsFiles(ctx: RuleContext, relDir: string): string[] {
    if (ctx.trackedFiles) {
        const prefix = relDir.endsWith("/") ? relDir : `${relDir}/`;
        return ctx.trackedFiles
            .filter((file) => file.startsWith(prefix) && file.endsWith(".cs"))
            .map((file) => path.join(ctx.rootDir, file));
    }

    const absoluteDir = path.join(ctx.rootDir, relDir);
    if (!existsSync(absoluteDir)) {
        return [];
    }
    return [...walkCsFiles(absoluteDir)];
}

function toRelativePath(ctx: RuleContext, absoluteFile: string): string {
    return path.relative(ctx.rootDir, absoluteFile).replaceAll("\\", "/");
}

/**
 * True for a `public static` method whose first parameter is `this
 * IMustClause <anything>` — the shape every real Must-clause extension
 * method shares (verified against all 607 real methods before writing this
 * rule; see the module header comment).
 */
function isMustExtensionMethod(method: {
    readonly modifiers: readonly string[];
    readonly parameters: readonly {
        readonly modifiers: readonly string[];
        readonly typeText: string | null;
    }[];
}): boolean {
    if (
        !method.modifiers.includes("public") ||
        !method.modifiers.includes("static")
    ) {
        return false;
    }
    const first = method.parameters[0];
    return (
        first !== undefined &&
        first.modifiers.includes("this") &&
        first.typeText === "IMustClause"
    );
}

async function collectMustMethods(ctx: RuleContext): Promise<MustMethodInfo[]> {
    const files = listCsFiles(ctx, MUST_CLAUSES_DIR);
    const methods: MustMethodInfo[] = [];

    for (const file of files) {
        const parsed = await parseFile(file);
        for (const method of findMethodDeclarations(parsed.root)) {
            if (!isMustExtensionMethod(method)) continue;
            methods.push({
                name: method.name,
                file: toRelativePath(ctx, file),
                line: method.node.startPosition.row + 1,
            });
        }
    }

    return methods;
}

/** Every distinct Must method name invoked as `Must.Be.<Name>(...)` anywhere under `relDir`. */
async function collectCalledNames(
    ctx: RuleContext,
    relDir: string,
): Promise<Set<string>> {
    const files = listCsFiles(ctx, relDir);
    const called = new Set<string>();

    for (const file of files) {
        const parsed = await parseFile(file);
        for (const invocation of findInvocations(parsed.root, "Must.Be")) {
            const name = invocation.memberPath.split(".").pop();
            if (name) called.add(name);
        }
    }

    return called;
}

/**
 * Emitted instead of an empty array when {@link collectMustMethods} finds no
 * subjects at all — see this module's "Diagnostic finding when zero Must
 * methods are found" header note. A rule that cannot find its own subject
 * population must say so loudly, not report a vacuous PASS.
 */
function noSubjectsFoundFinding(): Finding {
    return {
        rule: "must-usage",
        file: MUST_CLAUSES_DIR,
        message:
            "must-usage found zero Must extension methods to check — this likely means the rule's " +
            "detection logic is broken, not that there is nothing to audit.",
        key: "must-usage:no-subjects-found",
    };
}

async function run(ctx: RuleContext): Promise<Finding[]> {
    const mustMethods = await collectMustMethods(ctx);
    if (mustMethods.length === 0) {
        return [noSubjectsFoundFinding()];
    }

    const findings: Finding[] = [];
    for (const layer of LAYERS) {
        const calledNames = await collectCalledNames(ctx, layer.dir);
        for (const method of mustMethods) {
            if (calledNames.has(method.name)) continue;
            findings.push({
                rule: "must-usage",
                file: method.file,
                line: method.line,
                message:
                    `Must.Be.${method.name} (declared at ${method.file}:${String(method.line)}) has no ` +
                    `call site anywhere in ${layer.label} (${layer.dir}/).`,
                key: `must-usage:${layer.id}:${method.name}`,
            });
        }
    }

    return findings;
}

/** The plain `Rule` — exported directly so `test/rules/must-usage.test.ts` can run it against fixtures without going through the catalog (per `test/README.md`'s harness convention). */
export const mustUsageRule: Rule = { slug: "must-usage", run };

registerRule({
    ...mustUsageRule,
    // Only Rule03 fits CatalogEntry's single `legacyId?: string` field.
    // Rule04 (Fluent) and Rule05 (DataAnnotations) are ALSO superseded by
    // this same slug — see this module's header comment and the commit
    // report for the many-to-one mapping this creates; flagged loudly there
    // for whoever wires legacy-id lookups next, since `types.ts`/
    // `catalog.ts` were deliberately left unchanged (13 sibling P2 agents
    // depend on today's single-`legacyId` shape).
    legacyId: "Rule03",
    scope: "library",
    gate: false,
    description:
        "Every public Must clause (src/PineGuard.MustClauses) is invoked from Guard, Fluent, and DataAnnotations. Supersedes legacy Rule03 (Guard), Rule04 (Fluent), and Rule05 (DataAnnotations) — see this module's header comment.",
});
