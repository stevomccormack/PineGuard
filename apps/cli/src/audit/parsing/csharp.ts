import { readFileSync, statSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

import { Language, Node, Parser, Tree } from "web-tree-sitter";

/**
 * Thin, typed wrapper over `web-tree-sitter` + the `tree-sitter-c-sharp` wasm
 * grammar (plan `docs/ai/plans/audit-cli-rebuild.md` §4.5 / P1.2 spike).
 *
 * ## Query strategy: walk-then-filter, not `Query` s-expressions
 *
 * `web-tree-sitter` supports two ways to find nodes: the `Query` class
 * (s-expression patterns like `(method_declaration name: (identifier) @name)`,
 * matched via `Query#matches`/`captures`), or plain tree-walking helpers
 * (`Node#descendantsOfType`, `Node#childForFieldName`, `Node#children`).
 *
 * This module deliberately uses the second style everywhere. Every shape the
 * plan's rules need (method/record declarations, tuple types, attributes,
 * invocation member-access chains) is expressible as "find nodes of type X,
 * then read a couple of named fields" — there is no need for a query
 * engine's multi-pattern matching or predicates. Walking is also more robust
 * to grammar drift: a query string is opaque text checked only at runtime,
 * whereas `descendantsOfType("method_declaration")` fails loudly (empty
 * array, and TypeScript still type-checks) if a type name is wrong, and
 * `childForFieldName` failures are `null` rather than a query parse error.
 * `docs/ai/plans/audit-cli-rebuild.md` §8 lists the specific node shapes this
 * was verified against (via `tree-sitter-c-sharp`'s `node-types.json`):
 * `method_declaration`, `record_declaration`, `tuple_type`, `attribute`,
 * `invocation_expression`, `member_access_expression`, `parameter`,
 * `parameter_list`, `attribute_list`, `modifier`.
 *
 * Rules with a genuinely query-shaped need (multiple alternative patterns,
 * predicates) can still reach for `Query` directly against a `ParsedFile`'s
 * `tree` — this module does not hide the underlying tree-sitter API.
 */

// ---------------------------------------------------------------------------
// Grammar loading (once per process) + cached parsing (once per file)
// ---------------------------------------------------------------------------

let languagePromise: Promise<Language> | null = null;

/**
 * Loads and initialises the `tree-sitter-c-sharp` wasm grammar. Cached across
 * calls — `web-tree-sitter`'s wasm module and the parsed `Language` are
 * process-wide singletons; there is no reason to load them twice.
 */
function loadLanguage(): Promise<Language> {
    languagePromise ??= (async () => {
        await Parser.init();
        // `tree-sitter-c-sharp`'s package.json ships no "exports" map, so any
        // subpath (including this top-level .wasm asset) resolves under plain
        // Node ESM resolution — no bundler magic required.
        const wasmUrl = import.meta
            .resolve("tree-sitter-c-sharp/tree-sitter-c_sharp.wasm");
        return Language.load(fileURLToPath(wasmUrl));
    })();
    return languagePromise;
}

let parserPromise: Promise<Parser> | null = null;

function getParser(): Promise<Parser> {
    parserPromise ??= (async () => {
        const language = await loadLanguage();
        const parser = new Parser();
        parser.setLanguage(language);
        return parser;
    })();
    return parserPromise;
}

interface CacheEntry {
    readonly mtimeMs: number;
    readonly size: number;
    readonly text: string;
    readonly tree: Tree;
}

const parseCache = new Map<string, CacheEntry>();

/** A parsed C# file: the source text, the tree-sitter `Tree`, and its root node. */
export interface ParsedFile {
    /** Absolute path of the parsed file. */
    readonly path: string;
    /** Raw file content (UTF-8). */
    readonly text: string;
    /** The tree-sitter parse tree. */
    readonly tree: Tree;
    /** `tree.rootNode` — a `compilation_unit` for a well-formed C# file. */
    readonly root: Node;
}

/**
 * Parses a C# file, reusing the cached `Tree` when the file's mtime/size have
 * not changed since the last call — rules calling this repeatedly for the
 * same path (e.g. one rule per file, fourteen rules per run) pay the parse
 * cost once. Cache key is the resolved absolute path; relative paths resolve
 * against `process.cwd()`.
 */
export async function parseFile(filePath: string): Promise<ParsedFile> {
    const absolute = path.resolve(filePath);
    const stat = statSync(absolute);
    const cached = parseCache.get(absolute);
    if (
        cached &&
        cached.mtimeMs === stat.mtimeMs &&
        cached.size === stat.size
    ) {
        return {
            path: absolute,
            text: cached.text,
            tree: cached.tree,
            root: cached.tree.rootNode,
        };
    }

    const text = readFileSync(absolute, "utf8");
    const parser = await getParser();
    const tree = parser.parse(text);
    if (!tree) {
        throw new Error(
            `tree-sitter failed to produce a parse tree for ${absolute}`,
        );
    }

    parseCache.set(absolute, {
        mtimeMs: stat.mtimeMs,
        size: stat.size,
        text,
        tree,
    });
    return { path: absolute, text, tree, root: tree.rootNode };
}

/** Drops all cached parse trees. Mainly for test isolation. */
export function clearParseCache(): void {
    parseCache.clear();
}

// ---------------------------------------------------------------------------
// Shared node helpers
// ---------------------------------------------------------------------------

/**
 * Modifier keywords attached to a declaration or parameter (`public`,
 * `static`, `sealed`, `this`, `ref`, …). The grammar exposes every modifier
 * keyword as a same-named `modifier` child node (not a field), so this
 * applies equally to `method_declaration`, `record_declaration`, `parameter`,
 * and any other declaration shape that carries modifiers.
 */
export function getModifiers(node: Node): string[] {
    return node.children
        .filter((child) => child.type === "modifier")
        .map((child) => child.text);
}

/**
 * Resolves a name-position node (`identifier`, `generic_name`,
 * `qualified_name`, `alias_qualified_name`) down to its simple (rightmost,
 * non-generic) name — e.g. `System.Runtime.CompilerServices.CallerArgumentExpression`
 * or `CallerArgumentExpression` both resolve to `"CallerArgumentExpression"`.
 */
function simpleName(node: Node): string {
    switch (node.type) {
        case "identifier":
            return node.text;
        case "generic_name": {
            const id = node.namedChildren.find(
                (child) => child.type === "identifier",
            );
            return id ? id.text : node.text;
        }
        case "qualified_name":
        case "alias_qualified_name": {
            const name = node.childForFieldName("name");
            return name ? simpleName(name) : node.text;
        }
        default:
            return node.text;
    }
}

// ---------------------------------------------------------------------------
// Attributes — [CallerArgumentExpression], [GeneratedRegex(...)], etc.
// ---------------------------------------------------------------------------

/** One `[Xxx(...)]` attribute usage, wherever it appears (method, parameter, class, …). */
export interface AttributeInfo {
    readonly node: Node;
    /** Simple name, e.g. `"CallerArgumentExpression"` (namespace prefix, if any, stripped). */
    readonly name: string;
    /** Raw text of the `(...)` argument list, or `null` if the attribute takes none. */
    readonly argumentsText: string | null;
}

function toAttributeInfo(node: Node): AttributeInfo {
    const nameNode = node.childForFieldName("name");
    const argsNode =
        node.children.find(
            (child) => child.type === "attribute_argument_list",
        ) ?? null;
    return {
        node,
        name: nameNode ? simpleName(nameNode) : node.text,
        argumentsText: argsNode?.text ?? null,
    };
}

/**
 * Finds every `attribute` node under `root` (searches the whole subtree, so
 * it catches attributes on methods, parameters, classes, records, etc.
 * uniformly). Pass `name` to filter to one attribute (matched on the simple
 * name, e.g. `findAttributes(root, "CallerArgumentExpression")`).
 */
export function findAttributes(root: Node, name?: string): AttributeInfo[] {
    const all = root.descendantsOfType("attribute").map(toAttributeInfo);
    return name === undefined
        ? all
        : all.filter((attribute) => attribute.name === name);
}

// ---------------------------------------------------------------------------
// Parameters (shared by method and record-primary-constructor parameter lists)
// ---------------------------------------------------------------------------

export interface ParameterInfo {
    readonly node: Node;
    readonly name: string;
    /** Raw text of the declared type, or `null` if the grammar could not resolve one. */
    readonly typeText: string | null;
    readonly modifiers: readonly string[];
    readonly attributes: readonly AttributeInfo[];
}

function toParameterInfo(node: Node): ParameterInfo {
    const nameNode = node.childForFieldName("name");
    const typeNode = node.childForFieldName("type");
    const attributeLists = node.children.filter(
        (child) => child.type === "attribute_list",
    );
    return {
        node,
        name: nameNode?.text ?? node.text,
        typeText: typeNode?.text ?? null,
        modifiers: getModifiers(node),
        attributes: attributeLists.flatMap((list) =>
            list.namedChildren
                .filter((child) => child.type === "attribute")
                .map(toAttributeInfo),
        ),
    };
}

function parametersOf(parameterListNode: Node | null): ParameterInfo[] {
    if (!parameterListNode) return [];
    return parameterListNode.namedChildren
        .filter((child) => child.type === "parameter")
        .map(toParameterInfo);
}

// ---------------------------------------------------------------------------
// Method declarations
// ---------------------------------------------------------------------------

export interface MethodDeclarationInfo {
    readonly node: Node;
    readonly name: string;
    /** e.g. `["public", "static"]`. */
    readonly modifiers: readonly string[];
    readonly parameters: readonly ParameterInfo[];
    /** Attributes on the method itself (not on its parameters). */
    readonly attributes: readonly AttributeInfo[];
    readonly returnTypeText: string | null;
}

/** Finds every `method_declaration` under `root` (does not descend into nested local functions' own bodies twice; those are a distinct node type). */
export function findMethodDeclarations(root: Node): MethodDeclarationInfo[] {
    return root.descendantsOfType("method_declaration").map((node) => {
        const nameNode = node.childForFieldName("name");
        const returnsNode = node.childForFieldName("returns");
        const methodAttributeLists = node.children.filter(
            (child) => child.type === "attribute_list",
        );
        return {
            node,
            name: nameNode?.text ?? "",
            modifiers: getModifiers(node),
            parameters: parametersOf(node.childForFieldName("parameters")),
            attributes: methodAttributeLists.flatMap((list) =>
                list.namedChildren
                    .filter((child) => child.type === "attribute")
                    .map(toAttributeInfo),
            ),
            returnTypeText: returnsNode?.text ?? null,
        };
    });
}

// ---------------------------------------------------------------------------
// Record declarations
// ---------------------------------------------------------------------------

export interface RecordDeclarationInfo {
    readonly node: Node;
    readonly name: string;
    readonly modifiers: readonly string[];
    /**
     * Primary-constructor parameters, e.g. the `(string Name, string? Value,
     * bool Expected)` in `record ValidCase(string Name, ...)`. `null` when the
     * record has no primary constructor (body-only record).
     */
    readonly parameters: readonly ParameterInfo[] | null;
    /** Raw text of the `: Base(...)` clause, or `null` if there is none. */
    readonly baseListText: string | null;
}

/** Finds every `record_declaration` (and `record struct`/`record class`) under `root`. */
export function findRecordDeclarations(root: Node): RecordDeclarationInfo[] {
    return root.descendantsOfType("record_declaration").map((node) => {
        const nameNode = node.childForFieldName("name");
        const parameterListNode =
            node.children.find((child) => child.type === "parameter_list") ??
            null;
        const baseListNode =
            node.children.find((child) => child.type === "base_list") ?? null;
        return {
            node,
            name: nameNode?.text ?? "",
            modifiers: getModifiers(node),
            parameters: parameterListNode
                ? parametersOf(parameterListNode)
                : null,
            baseListText: baseListNode?.text ?? null,
        };
    });
}

// ---------------------------------------------------------------------------
// Tuple types — to distinguish `(string Value, int Length)` from a record's
// own primary-constructor parameter list, which uses the unrelated `parameter`
// node shape even though both look similar in source.
// ---------------------------------------------------------------------------

export interface TupleElementInfo {
    readonly name: string | null;
    readonly typeText: string;
}

export interface TupleTypeInfo {
    readonly node: Node;
    readonly elements: readonly TupleElementInfo[];
}

/** Finds every `tuple_type` node under `root`, e.g. `(string? Value, int Length)`. */
export function findTupleTypes(root: Node): TupleTypeInfo[] {
    return root.descendantsOfType("tuple_type").map((node) => ({
        node,
        elements: node.namedChildren
            .filter((child) => child.type === "tuple_element")
            .map((element) => ({
                name: element.childForFieldName("name")?.text ?? null,
                typeText: element.childForFieldName("type")?.text ?? "",
            })),
    }));
}

// ---------------------------------------------------------------------------
// Invocations / member-access chains — e.g. `Must.Be.NullOrEmpty(value, name)`
// ---------------------------------------------------------------------------

/** Walks a `function`-field expression down to its dotted member path, e.g. `["Must", "Be", "NullOrEmpty"]`. */
function memberPathParts(node: Node): string[] {
    if (node.type === "member_access_expression") {
        const expression = node.childForFieldName("expression");
        const name = node.childForFieldName("name");
        const left = expression ? memberPathParts(expression) : [];
        const right = name ? [simpleName(name)] : [];
        return [...left, ...right];
    }
    return [simpleName(node)];
}

export interface InvocationInfo {
    readonly node: Node;
    /** Dotted call target, e.g. `"Must.Be.NullOrEmpty"`. Empty string if the callee shape is unresolvable (rare; e.g. a parenthesized/cast callee). */
    readonly memberPath: string;
    /** Raw text of the `(...)` argument list. */
    readonly argumentsText: string;
}

/**
 * Finds every `invocation_expression` under `root`. When `memberPath` is
 * given, keeps only invocations whose dotted call target equals it, or is a
 * direct child of it (`findInvocations(root, "Must.Be")` returns every
 * `Must.Be.<Name>(...)` call — the shape `must-usage` (plan §8) needs to
 * enumerate Must clause call sites by layer).
 */
export function findInvocations(
    root: Node,
    memberPath?: string,
): InvocationInfo[] {
    const all = root.descendantsOfType("invocation_expression").map((node) => {
        const functionNode = node.childForFieldName("function");
        const argumentsNode = node.childForFieldName("arguments");
        return {
            node,
            memberPath: functionNode
                ? memberPathParts(functionNode).join(".")
                : "",
            argumentsText: argumentsNode?.text ?? "()",
        };
    });
    if (memberPath === undefined) return all;
    return all.filter(
        (invocation) =>
            invocation.memberPath === memberPath ||
            invocation.memberPath.startsWith(`${memberPath}.`),
    );
}
