import { remark } from "remark";
import remarkFrontmatter from "remark-frontmatter";
import { visit } from "unist-util-visit";
import { parse as parseYaml } from "yaml";

/**
 * The plan's own convention: a leading `+ ` inside a code span (e.g.
 * `` `+ apps/cli/package.json` ``) marks a file the plan *creates*, so a
 * `doc-links` rule should not flag it as a broken reference. See
 * docs/ai/plans/audit-cli-rebuild.md, "Path convention" note.
 */
const PLANNED_PREFIX = "+ ";

/** A single path-shaped reference pulled out of a markdown document. */
export interface PathReference {
    /** The referenced path, with any `+ ` planned-file marker already stripped. */
    readonly path: string;
    /** `true` when the reference used the `+ path` "planned/future file" convention. */
    readonly planned: boolean;
    /** Where the reference came from: a markdown link target, or a backticked code span. */
    readonly kind: "link" | "code";
    /** 1-based source line, when the parser retained position info. */
    readonly line?: number;
    /** 1-based source column, when the parser retained position info. */
    readonly column?: number;
}

/**
 * A minimal, structurally-typed view of an mdast frontmatter node
 * (`{ type: "yaml", value: "..." }`). Declared locally — rather than
 * imported from `mdast`/`@types/mdast` — because those packages are only
 * *transitive* dependencies under this workspace's isolated pnpm
 * `node_modules` layout and are not reliably resolvable from here; the
 * cast through `unknown` below is intentionally defensive so this keeps
 * working regardless of whether `remark-frontmatter`'s own ambient type
 * augmentation is visible in this compilation.
 */
interface FrontMatterNode {
    readonly type: unknown;
    readonly value: unknown;
}

function createProcessor() {
    // remark-frontmatter teaches the parser to treat a leading `---`
    // block as one opaque "yaml" node instead of misreading it as a
    // thematic break plus paragraph text — needed by both functions
    // below so front matter never leaks into the link/code-span scan.
    return remark().use(remarkFrontmatter, ["yaml"]);
}

function parseTree(markdown: string) {
    return createProcessor().parse(markdown);
}

function isPlainObject(value: unknown): value is Record<string, unknown> {
    return typeof value === "object" && value !== null && !Array.isArray(value);
}

/**
 * Parse a markdown document's leading `---` YAML front-matter block (if
 * any) into a plain object. Returns `{}` when there is no front matter,
 * the block is empty, or it does not parse to a YAML mapping. Schema is
 * not validated here — callers own that.
 */
export function extractFrontMatter(markdown: string): Record<string, unknown> {
    const tree = parseTree(markdown);
    const first = tree.children[0] as unknown as FrontMatterNode | undefined;

    if (!first || first.type !== "yaml" || typeof first.value !== "string") {
        return {};
    }

    const raw = first.value;
    if (raw.trim().length === 0) {
        return {};
    }

    const parsed: unknown = parseYaml(raw);
    return isPlainObject(parsed) ? parsed : {};
}

/**
 * Does `text` (a code span's contents, with any `+ ` marker already
 * stripped) "look like" a file path rather than an identifier, a method
 * call, or a member-access chain? No whitespace, no code punctuation, and
 * either a path separator or a dotted extension.
 *
 * This is a heuristic, not a resolver: it is deliberately permissive
 * (e.g. `Must.Be.Guid` also matches, since it ends in a short dotted
 * suffix). The `doc-links` rule (plan P2.8) is expected to narrow
 * further — e.g. by checking whether the candidate actually resolves
 * against the tracked-file list from `repo.ts`.
 */
function looksLikePath(text: string): boolean {
    if (text.length === 0 || /\s/.test(text)) {
        return false;
    }

    if (/[()<>{}[\]"'`]/.test(text)) {
        return false;
    }

    return text.includes("/") || /\.[A-Za-z0-9]{1,10}$/.test(text);
}

function toReference(
    rawTarget: string,
    kind: PathReference["kind"],
    position: { line: number; column: number } | undefined,
): PathReference {
    const planned = rawTarget.startsWith(PLANNED_PREFIX);
    const path = planned ? rawTarget.slice(PLANNED_PREFIX.length) : rawTarget;

    return {
        path,
        planned,
        kind,
        line: position?.line,
        column: position?.column,
    };
}

/**
 * Walk a markdown document's mdast tree and collect every path-shaped
 * reference:
 *
 * - markdown link targets, `[text](path)`;
 * - backticked inline-code spans that look like a file path (e.g.
 *   `` `apps/cli/package.json` ``), per {@link looksLikePath}.
 *
 * A leading `+ ` inside a code span (`` `+ apps/cli/engine.ts` ``) marks a
 * planned/future file per the plan's own convention: such matches are
 * tagged `planned: true` with the marker stripped from `path`, so a
 * `doc-links` rule can skip them instead of reporting them as broken.
 */
export function extractPathReferences(markdown: string): PathReference[] {
    const tree = parseTree(markdown);
    const references: PathReference[] = [];

    visit(tree, "link", (node) => {
        const url = node.url;
        if (typeof url === "string" && url.length > 0) {
            references.push(toReference(url, "link", node.position?.start));
        }
    });

    visit(tree, "inlineCode", (node) => {
        const raw = node.value;
        if (typeof raw !== "string") {
            return;
        }

        const candidate = raw.startsWith(PLANNED_PREFIX)
            ? raw.slice(PLANNED_PREFIX.length)
            : raw;

        if (looksLikePath(candidate)) {
            references.push(toReference(raw, "code", node.position?.start));
        }
    });

    return references;
}
