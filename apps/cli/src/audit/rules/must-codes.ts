import { readFileSync, readdirSync, statSync } from "node:fs";
import { join, relative } from "node:path";

import { registerRule, type CatalogEntry } from "../catalog.js";
import type { Finding, RuleContext } from "../types.js";

/**
 * `must-codes` — port of the legacy PowerShell tool's Rule13
 * (`tools/audit-cli/rules/Test-Rule13-MustCodes.ps1`), plan
 * `docs/ai/plans/audit-cli-rebuild.md` §4.3 row "Rule13 → `must-codes`" and
 * §8's technical note.
 *
 * ## The bug this rewrite fixes (plan §2.2)
 *
 * The legacy script hardcodes a `$domainMap` (clause-file key → catalogue
 * domain class name) that a human has to remember to update every time a new
 * clause family is added — three of Rule13's six live findings on
 * 2026-09-06 were just that map being stale (missing Cron/Token/Version,
 * added 2026-08-31), not real drift.
 *
 * This rewrite **derives** the map instead. Every domain partial file under
 * `src/PineGuard.Core/Codes/` (`MustCodes.<Domain>.cs`) carries a `// Serves:
 * <ClauseFile1>.cs, <ClauseFile2>.cs, …` comment immediately above its one
 * top-level nested domain class (verified against all 33 files in that
 * directory, 2026-09-06 — every domain file has exactly this shape; only the
 * root `MustCodes.cs` partial-declaration file has neither). Reading that
 * comment plus the domain class name it precedes is enough to reconstruct
 * the whole clause-file → domain map with no hardcoded list at all: add a
 * new `MustCodes.Whatever.cs` file with its own `// Serves:` comment and the
 * next `pineguard audit must-codes` run picks it up automatically, with zero
 * changes to this file.
 *
 * The same "derive it from a doc comment instead of hardcoding a list"
 * approach is used for check (b)'s "reserved, not yet used" exemption (see
 * below) — the legacy script hardcoded `Value.Argument.Invalid` by exact
 * name; this rewrite instead reads the actual doc comment already sitting on
 * that constant's containing class (`MustCodes.Value.cs`'s `Argument` class:
 * "Reserved for adapters that map an argument exception PineGuard did not
 * itself throw … No clause emits this directly.") and treats any class or
 * constant whose XML doc comment contains the word "reserved" as exempt.
 * Same derivation principle, same zero-maintenance property, one less
 * hardcoded list.
 *
 * ## The seven checks (source of truth: the PowerShell script, not the plan
 * summary — the plan's §8 note says "checks (a)-(f)" but the script itself
 * documents seven, labelled (a) through (g) in its own header comment; all
 * seven are ported here)
 *
 * - **(a)** every public Must clause method's `.Fail(`/`.FromBool(` call
 *   passes exactly one `MustCodes` constant.
 * - **(b)** every catalogue constant (other than `Prefix`) is referenced by
 *   at least one clause, DataAnnotations attribute, or
 *   Core/AspNetCore/Extensions.Options call site — unless its own or its
 *   containing class's doc comment says it's reserved for later.
 * - **(c)** no code string literal (e.g. `"email.address.invalid"`)
 *   duplicates a catalogue domain outside `src/PineGuard.Core/Codes/`.
 * - **(d)** every DataAnnotations attribute's declared code matches a code
 *   actually produced by the Must clause method(s) it dispatches to (direct
 *   `Must.Be.*` calls and `nameof(MustXClauses.Y)` reflective dispatch
 *   alike).
 * - **(e)** every Guard clause passes its `IMustResult` (never a string) as
 *   `GuardFailure.Throw`'s first argument.
 * - **(f)** every clause file only references constants from its own mapped
 *   domain (and every clause file resolves to *some* domain in the first
 *   place — an unmapped file is itself a finding).
 * - **(g)** no `using PineGuard…` line appears under
 *   `src/PineGuard.Core/Codes/` (the catalogue stays a dependency-free leaf).
 *
 * ## Parsing approach: plain text/regex, not tree-sitter
 *
 * Every other check ported in this plan reaches for `parsing/csharp.ts`'s
 * tree-sitter helpers. This rule deliberately does not: the domain-map
 * derivation is fundamentally a *comment* convention (`// Serves: …`), not an
 * AST shape, and the rest of the checks (nested `const string` fields inside
 * nested classes, `Fail(`/`FromBool(`/`GuardFailure.Throw(` call-site
 * argument counting, a DataAnnotations attribute's base-constructor
 * argument) are exactly what the legacy script already validated works via
 * regex plus hand-rolled balanced-paren/brace scanning — ported faithfully
 * (with one correctness fix: the method-signature regex now tolerates
 * `async` between `static` and the return type, e.g.
 * `public static async ValueTask<MustResult<T>> …`, which the legacy
 * pattern could never match at all).
 */

/**
 * Exported (not just registered) so `test/rules/must-codes.test.ts` can run
 * it directly against fixtures via the VIBE harness, per the pattern
 * documented in `test/README.md`.
 */
export const mustCodesRule: CatalogEntry = {
    slug: "must-codes",
    legacyId: "Rule13",
    scope: "library",
    gate: false,
    description:
        "Must error-code catalogue integrity: every clause resolves to a domain, every constant is used, DataAnnotations codes match their dispatch, Guard passes IMustResult not a string — domain map derived from Codes/*.cs 'Serves:' comments, never hardcoded.",
    run(ctx: RuleContext): Finding[] {
        return runMustCodes(ctx);
    },
};

registerRule(mustCodesRule);

// ---------------------------------------------------------------------------
// Regexes (module-level `const`s reused across many `matchAll` calls — safe:
// `String.prototype.matchAll` always operates on a fresh internal copy of a
// global regex, so a shared `g`-flagged instance never leaks `lastIndex`
// state between calls; non-`g` regexes below are only ever used with `.test`
// / `.match`, which are likewise stateless).
// ---------------------------------------------------------------------------

const CLASS_DECL_RE =
    /^\s*(?:public|internal)\s+static\s+(?:partial\s+)?class\s+(\w+)/;
const CONST_FIELD_RE = /^\s*(?:public|internal)\s+const\s+string\s+(\w+)\s*=/;
const SERVES_LINE_RE = /^\s*\/\/\s*Serves:\s*(.+)$/m;
const USING_PINEGUARD_RE = /^\s*using\s+PineGuard/;

const CODE_REF_RE = /MustCodes(?:\.\w+)+/g;
// Fix vs. the legacy PowerShell pattern: allow an optional `async` between
// `static` and the return type, so async predicate clauses
// (`public static async ValueTask<MustResult<T>> SatisfiesAsync<T>(…)`) are
// scanned too, instead of silently falling outside check (a)'s coverage.
const METHOD_SIG_RE =
    /public\s+static\s+(?:async\s+)?(?:MustResult<|ValueTask<MustResult<)[^\r\n]*?\s(\w+)\s*(?:<[^>]*>)?\s*\(/g;
const CALL_SITE_RE =
    /MustResult<[^>]*(?:<[^>]*>)?[^>]*>\.(Fail|FromBool)\s*\(/g;
const CLASS_NAME_ATTR_RE = /\bclass\s+(\w*Attribute)\b/g;
const DIRECT_CALL_RE = /Must\.Be\.(\w+)\s*\(/g;
const REFLECTIVE_CALL_RE = /nameof\(Must\w+Clauses\.(\w+)\)/g;
const GUARD_THROW_RE = /GuardFailure\.Throw\s*\(\s*(.)/g;

// ---------------------------------------------------------------------------
// File discovery — works against both a real repo (`ctx.trackedFiles`
// populated) and a fixture tree (`ctx.trackedFiles` undefined, so this rule
// walks `ctx.rootDir` itself, exactly like the `_demo-harness` rule in
// `test/support/runRule.test.ts` does).
// ---------------------------------------------------------------------------

function listAllRelativeFiles(ctx: RuleContext): string[] {
    if (ctx.trackedFiles) {
        return [...ctx.trackedFiles];
    }

    const results: string[] = [];
    const walk = (dir: string): void => {
        for (const entry of readdirSync(dir)) {
            const full = join(dir, entry);
            if (statSync(full).isDirectory()) {
                walk(full);
            } else {
                results.push(relative(ctx.rootDir, full).replaceAll("\\", "/"));
            }
        }
    };
    walk(ctx.rootDir);
    return results;
}

interface FilesUnderOptions {
    readonly recursive?: boolean;
    readonly namePattern: RegExp;
    readonly excludeSegments?: readonly string[];
}

/** Filters `allFiles` (repo/fixture-relative, forward-slash paths) to those directly or recursively under `dir`. */
function filesUnder(
    allFiles: readonly string[],
    dir: string,
    opts: FilesUnderOptions,
): string[] {
    const prefix = `${dir}/`;
    return allFiles.filter((file) => {
        if (!file.startsWith(prefix)) return false;
        const rest = file.slice(prefix.length);
        if (!opts.recursive && rest.includes("/")) return false;
        const name = rest.slice(rest.lastIndexOf("/") + 1);
        if (!opts.namePattern.test(name)) return false;
        if (opts.excludeSegments?.some((seg) => file.includes(seg)))
            return false;
        return true;
    });
}

/** Index one past a balanced `open`/`close` span starting at `openIndex` (which must hold `open`). Mirrors the legacy script's `Get-BalancedSpan`. */
function getBalancedSpan(
    content: string,
    openIndex: number,
    open: string,
    close: string,
): number {
    let depth = 1;
    let i = openIndex + 1;
    while (depth > 0 && i < content.length) {
        if (content[i] === open) depth++;
        else if (content[i] === close) depth--;
        i++;
    }
    return i;
}

function lineNumberAt(content: string, index: number | undefined): number {
    return content.slice(0, Math.max(index ?? 0, 0)).split("\n").length;
}

function escapeRegExp(value: string): string {
    return value.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
}

function stripPrefix(codeRef: string): string {
    return codeRef.replace(/^MustCodes\./, "");
}

// ---------------------------------------------------------------------------
// Codes/ scan: declared constants + the derived clause-file -> domain map.
// ---------------------------------------------------------------------------

interface DeclaredConstant {
    /** Dotted path under `MustCodes.`, e.g. `"Boolean.Value.False"`. */
    readonly qualified: string;
    readonly file: string;
    readonly line: number;
    /** True if this constant (or a class it's nested in) is documented as reserved for later use. */
    readonly reserved: boolean;
}

interface CodesScanResult {
    readonly declaredConstants: DeclaredConstant[];
    readonly clauseFileToDomain: ReadonlyMap<string, string>;
    readonly findings: Finding[];
}

function scanCodesFiles(
    ctx: RuleContext,
    codesFiles: readonly string[],
): CodesScanResult {
    const findings: Finding[] = [];
    const declaredConstants: DeclaredConstant[] = [];
    const clauseFileToDomain = new Map<string, string>();

    for (const relPath of codesFiles) {
        const content = readFileSync(join(ctx.rootDir, relPath), "utf8");
        const lines = content.split(/\r?\n/);

        interface StackEntry {
            readonly indent: number;
            readonly name: string;
            readonly reserved: boolean;
        }
        const stack: StackEntry[] = [];
        let docBuffer: string[] = [];
        let domainName: string | null = null;

        lines.forEach((line, index) => {
            if (USING_PINEGUARD_RE.test(line)) {
                findings.push({
                    rule: "must-codes",
                    file: relPath,
                    line: index + 1,
                    message: `(g) ${relPath}: 'using PineGuard...' line found under Codes/ — the catalogue must stay a dependency-free leaf (${line.trim()}).`,
                    key: `must-codes:g:${relPath}:${index + 1}`,
                });
            }

            const trimmed = line.trim();

            if (trimmed.startsWith("///")) {
                docBuffer.push(trimmed.replace(/^\/\/\/\s?/, ""));
                return;
            }
            if (trimmed.startsWith("[") || trimmed === "") {
                // Attribute line or blank line between a doc-comment block and
                // the declaration it documents — keep accumulating.
                return;
            }

            const indent = line.length - line.replace(/^ +/, "").length;
            const classMatch = line.match(CLASS_DECL_RE);
            const className = classMatch?.[1];
            if (className !== undefined && className !== "MustCodes") {
                while (
                    stack.length > 0 &&
                    (stack.at(-1)?.indent ?? -1) >= indent
                ) {
                    stack.pop();
                }
                const parentReserved = stack.at(-1)?.reserved ?? false;
                const ownReserved = /reserved/i.test(docBuffer.join(" "));
                if (stack.length === 0 && domainName === null) {
                    domainName = className;
                }
                stack.push({
                    indent,
                    name: className,
                    reserved: parentReserved || ownReserved,
                });
                docBuffer = [];
                return;
            }

            const fieldMatch = line.match(CONST_FIELD_RE);
            const fieldName = fieldMatch?.[1];
            if (fieldName !== undefined && stack.length > 0) {
                if (fieldName !== "Prefix") {
                    const qualified = `${stack.map((s) => s.name).join(".")}.${fieldName}`;
                    const ownReserved = /reserved/i.test(docBuffer.join(" "));
                    const reserved =
                        ownReserved || stack.some((s) => s.reserved);
                    declaredConstants.push({
                        qualified,
                        file: relPath,
                        line: index + 1,
                        reserved,
                    });
                }
                docBuffer = [];
                return;
            }

            docBuffer = [];
        });

        const servesMatch = content.match(SERVES_LINE_RE);
        const servesList = servesMatch?.[1];
        if (servesList !== undefined && domainName !== null) {
            const entries = servesList
                .split(",")
                .map((s) => s.trim())
                .filter((s) => s.length > 0);
            for (const entry of entries) {
                clauseFileToDomain.set(entry, domainName);
            }
        }
    }

    return { declaredConstants, clauseFileToDomain, findings };
}

// ---------------------------------------------------------------------------
// Must clause file scan: checks (a), (c), (f); also populates
// `usedConstants` and `methodCodesByName` (consumed by (b) and (d)).
// ---------------------------------------------------------------------------

interface ClauseScanResult {
    readonly findings: Finding[];
    readonly usedConstants: ReadonlySet<string>;
    readonly methodCodesByName: ReadonlyMap<string, ReadonlySet<string>>;
}

function scanClauseFiles(
    ctx: RuleContext,
    clauseFiles: readonly string[],
    clauseFileToDomain: ReadonlyMap<string, string>,
    domainTokens: readonly string[],
): ClauseScanResult {
    const findings: Finding[] = [];
    const usedConstants = new Set<string>();
    const methodCodesByName = new Map<string, Set<string>>();

    for (const relPath of clauseFiles) {
        const fileName = relPath.slice(relPath.lastIndexOf("/") + 1);
        const domain = clauseFileToDomain.get(fileName);

        if (!domain) {
            findings.push({
                rule: "must-codes",
                file: relPath,
                message: `(f) ${fileName}: no domain mapping found — no MustCodes.<Domain>.cs file's "// Serves:" comment lists this clause file. Add it to the appropriate domain file's Serves comment (or create a new domain file), so the catalogue stays derived, not hardcoded.`,
                key: `must-codes:f:missing-domain:${relPath}`,
            });
            continue;
        }

        const content = readFileSync(join(ctx.rootDir, relPath), "utf8");

        // (c) hardcoded code-string literal instead of the MustCodes constant.
        for (const token of domainTokens) {
            const litRe = new RegExp(
                `"${escapeRegExp(token.toLowerCase())}\\.[a-z][a-z0-9-]*\\.[a-z][a-z0-9-]*"`,
                "g",
            );
            for (const m of content.matchAll(litRe)) {
                findings.push({
                    rule: "must-codes",
                    file: relPath,
                    line: lineNumberAt(content, m.index),
                    message: `(c) ${fileName}: hardcoded code string literal ${m[0]} — use the MustCodes constant instead.`,
                    key: `must-codes:c:${relPath}:${m.index}`,
                });
            }
        }

        // (f) every MustCodes.<X>... reference in this file must belong to
        // this file's own mapped domain; also feeds usedConstants for (b).
        for (const m of content.matchAll(CODE_REF_RE)) {
            const qualified = stripPrefix(m[0]);
            usedConstants.add(qualified);
            const referencedDomain = qualified.split(".")[0];
            if (referencedDomain !== undefined && referencedDomain !== domain) {
                findings.push({
                    rule: "must-codes",
                    file: relPath,
                    line: lineNumberAt(content, m.index),
                    message: `(f) ${fileName}: references MustCodes.${referencedDomain}.* but is mapped to domain '${domain}' — clause files must only use their own domain's constants.`,
                    key: `must-codes:f:cross-domain:${relPath}:${qualified}:${m.index}`,
                });
            }
        }

        // (a) every public clause method's Fail(/FromBool( call passes
        // exactly one MustCodes constant.
        for (const sigMatch of content.matchAll(METHOD_SIG_RE)) {
            const methodName = sigMatch[1];
            if (methodName === undefined || sigMatch.index === undefined)
                continue;

            const sigEnd = sigMatch.index + sigMatch[0].length;
            const parenOpen = content.indexOf("(", sigEnd - 1);
            if (parenOpen < 0) continue;
            const afterParams = getBalancedSpan(content, parenOpen, "(", ")");
            const braceOpen = content.indexOf("{", afterParams);
            if (braceOpen < 0) continue;
            const braceClose = getBalancedSpan(content, braceOpen, "{", "}");
            const body = content.slice(braceOpen, braceClose);

            const codesInBody =
                methodCodesByName.get(methodName) ?? new Set<string>();
            for (const m of body.matchAll(CODE_REF_RE)) {
                codesInBody.add(stripPrefix(m[0]));
            }
            methodCodesByName.set(methodName, codesInBody);

            for (const callMatch of body.matchAll(CALL_SITE_RE)) {
                if (callMatch.index === undefined) continue;
                const callOpenParen = body.indexOf(
                    "(",
                    callMatch.index + callMatch[0].length - 1,
                );
                if (callOpenParen < 0) continue;
                const callEnd = getBalancedSpan(body, callOpenParen, "(", ")");
                const argText = body.slice(callOpenParen + 1, callEnd - 1);
                const codeRefs = [...argText.matchAll(CODE_REF_RE)];
                const callKind = callMatch[1] ?? "Fail";
                const absoluteIndex = braceOpen + callMatch.index;
                if (codeRefs.length === 0) {
                    findings.push({
                        rule: "must-codes",
                        file: relPath,
                        line: lineNumberAt(content, absoluteIndex),
                        message: `(a) ${fileName}: ${methodName} -> .${callKind}(...) call passes no MustCodes constant.`,
                        key: `must-codes:a:${relPath}:${methodName}:${callMatch.index}`,
                    });
                } else if (codeRefs.length > 1) {
                    findings.push({
                        rule: "must-codes",
                        file: relPath,
                        line: lineNumberAt(content, absoluteIndex),
                        message: `(a) ${fileName}: ${methodName} -> .${callKind}(...) call passes ${codeRefs.length} MustCodes constants, expected exactly one.`,
                        key: `must-codes:a:${relPath}:${methodName}:${callMatch.index}:multi`,
                    });
                }
            }
        }
    }

    return { findings, usedConstants, methodCodesByName };
}

// ---------------------------------------------------------------------------
// Guard clause file scan: check (e).
// ---------------------------------------------------------------------------

function scanGuardClauseFiles(
    ctx: RuleContext,
    guardClauseFiles: readonly string[],
): Finding[] {
    const findings: Finding[] = [];

    for (const relPath of guardClauseFiles) {
        const content = readFileSync(join(ctx.rootDir, relPath), "utf8");
        for (const m of content.matchAll(GUARD_THROW_RE)) {
            const firstChar = m[1];
            if (firstChar === '"' || firstChar === "'") {
                const lineNumber = lineNumberAt(content, m.index);
                findings.push({
                    rule: "must-codes",
                    file: relPath,
                    line: lineNumber,
                    message: `(e) ${relPath}:${lineNumber}: GuardFailure.Throw(...) is called with a string literal as its first argument — pass the IMustResult itself, e.g. GuardFailure.Throw(result, message, exceptionCreator).`,
                    key: `must-codes:e:${relPath}:${lineNumber}`,
                });
            }
        }
    }

    return findings;
}

// ---------------------------------------------------------------------------
// DataAnnotations attribute file scan: check (d).
// ---------------------------------------------------------------------------

function scanAttributeFiles(
    ctx: RuleContext,
    attributeFiles: readonly string[],
    methodCodesByName: ReadonlyMap<string, ReadonlySet<string>>,
): Finding[] {
    const findings: Finding[] = [];

    for (const relPath of attributeFiles) {
        const content = readFileSync(join(ctx.rootDir, relPath), "utf8");

        for (const classMatch of content.matchAll(CLASS_NAME_ATTR_RE)) {
            if (classMatch.index === undefined) continue;
            const className = classMatch[1];
            if (className === undefined) continue;

            let pos = classMatch.index + classMatch[0].length;
            const isWhitespace = (): boolean =>
                pos < content.length && /\s/.test(content[pos] ?? "");
            while (isWhitespace()) pos++;
            if (pos < content.length && content[pos] === "(") {
                pos = getBalancedSpan(content, pos, "(", ")");
            }
            while (isWhitespace()) pos++;

            let declaredCode: string | null = null;
            if (pos < content.length && content[pos] === ":") {
                pos++;
                while (isWhitespace()) pos++;
                const baseNameMatch = content.slice(pos).match(/^\w+/);
                pos += baseNameMatch ? baseNameMatch[0].length : 0;
                while (isWhitespace()) pos++;
                if (pos < content.length && content[pos] === "(") {
                    const baseCallEnd = getBalancedSpan(content, pos, "(", ")");
                    const baseCallArgs = content.slice(
                        pos + 1,
                        baseCallEnd - 1,
                    );
                    const codeMatches = [...baseCallArgs.matchAll(CODE_REF_RE)];
                    const firstMatch = codeMatches[0];
                    if (firstMatch) {
                        declaredCode = stripPrefix(firstMatch[0]);
                    }
                    pos = baseCallEnd;
                }
            }
            if (declaredCode === null) continue;

            while (pos < content.length && content[pos] !== "{") pos++;
            if (pos >= content.length) continue;
            const bodyEnd = getBalancedSpan(content, pos, "{", "}");
            const classBody = content.slice(pos, bodyEnd);

            const dispatched = new Set<string>();
            for (const m of classBody.matchAll(DIRECT_CALL_RE)) {
                if (m[1] !== undefined) dispatched.add(m[1]);
            }
            for (const m of classBody.matchAll(REFLECTIVE_CALL_RE)) {
                if (m[1] !== undefined) dispatched.add(m[1]);
            }
            if (dispatched.size === 0) continue;

            let matchesAny = false;
            for (const methodName of dispatched) {
                const codes = methodCodesByName.get(methodName);
                if (codes?.has(declaredCode)) {
                    matchesAny = true;
                    break;
                }
            }
            if (!matchesAny) {
                findings.push({
                    rule: "must-codes",
                    file: relPath,
                    line: lineNumberAt(content, classMatch.index),
                    message: `(d) ${relPath}: ${className} declares code ${declaredCode} but its Must clause dispatch (${[...dispatched].join(", ")}) doesn't produce that code.`,
                    key: `must-codes:d:${relPath}:${className}`,
                });
            }
        }
    }

    return findings;
}

// ---------------------------------------------------------------------------
// Main entry point.
// ---------------------------------------------------------------------------

const EXTRA_USAGE_ROOTS = [
    "src/PineGuard.Core",
    "src/PineGuard.DataAnnotations",
    "src/PineGuard.AspNetCore",
    "src/PineGuard.Extensions.Options",
];

function runMustCodes(ctx: RuleContext): Finding[] {
    const allFiles = listAllRelativeFiles(ctx);

    const codesFiles = filesUnder(allFiles, "src/PineGuard.Core/Codes", {
        namePattern: /\.cs$/,
    });
    const clauseFiles = filesUnder(allFiles, "src/PineGuard.MustClauses", {
        namePattern: /^Must.*Clauses\.cs$/,
    });
    const guardClauseFiles = filesUnder(
        allFiles,
        "src/PineGuard.GuardClauses",
        {
            namePattern: /^Guard.*Clauses\.cs$/,
        },
    );
    const attributeFiles = filesUnder(
        allFiles,
        "src/PineGuard.DataAnnotations",
        {
            recursive: true,
            namePattern: /\.cs$/,
            excludeSegments: ["/Common/"],
        },
    );
    const usageFiles = EXTRA_USAGE_ROOTS.flatMap((root) =>
        filesUnder(allFiles, root, {
            recursive: true,
            namePattern: /\.cs$/,
            excludeSegments: ["/bin/", "/obj/"],
        }),
    ).filter((file) => !file.includes("/Codes/"));

    const findings: Finding[] = [];

    const {
        declaredConstants,
        clauseFileToDomain,
        findings: codesFindings,
    } = scanCodesFiles(ctx, codesFiles);
    findings.push(...codesFindings);

    const domainTokens = [
        ...new Set(declaredConstants.map((c) => c.qualified.split(".")[0])),
    ].filter((token): token is string => token !== undefined);

    const {
        findings: clauseFindings,
        usedConstants,
        methodCodesByName,
    } = scanClauseFiles(ctx, clauseFiles, clauseFileToDomain, domainTokens);
    findings.push(...clauseFindings);

    findings.push(...scanGuardClauseFiles(ctx, guardClauseFiles));
    findings.push(
        ...scanAttributeFiles(ctx, attributeFiles, methodCodesByName),
    );

    // (b) usage scan across the extra roots (Core/DataAnnotations/AspNetCore/
    // Extensions.Options, excluding Codes/ itself) feeds the same
    // usedConstants set the clause-file scan already populated.
    const usedConstantsMutable = new Set(usedConstants);
    for (const filePath of usageFiles) {
        const content = readFileSync(join(ctx.rootDir, filePath), "utf8");
        for (const m of content.matchAll(CODE_REF_RE)) {
            usedConstantsMutable.add(stripPrefix(m[0]));
        }
    }

    for (const constant of declaredConstants) {
        if (constant.reserved) continue;
        if (!usedConstantsMutable.has(constant.qualified)) {
            findings.push({
                rule: "must-codes",
                file: constant.file,
                line: constant.line,
                message: `(b) MustCodes.${constant.qualified} is declared but never referenced by any clause, DataAnnotations attribute, or Core/AspNetCore call site.`,
                key: `must-codes:b:${constant.qualified}`,
            });
        }
    }

    return findings;
}
