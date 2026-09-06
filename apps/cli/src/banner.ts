import { readFileSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

import figlet from "figlet";
import pc from "picocolors";

/**
 * The wordmark's figlet font. "ANSI Shadow" is the block-capital face behind
 * most modern CLI wordmarks — Hermes Agent, create-t3-app and Gemini CLI all
 * ship it (pre-rendered and pasted into source); rendering it live through
 * figlet gives the same look without hand-drawn glyphs living in this file.
 * "PineGuard" comes out at 6 rows x 71 columns, comfortably inside a standard
 * 80-column terminal (~30 bundled fonts were measured before settling on this
 * one — see the P1.6 note in docs/ai/plans/audit-cli-rebuild.md).
 */
export const BANNER_FONT = "ANSI Shadow";

/**
 * The wordmark is rendered as two figlet blocks — "Pine" and "Guard" — and
 * zipped back together row by row, so each word can be painted on its own
 * (pine green for "Pine", the terminal's default for "Guard") without having
 * to locate a column offset inside one rendered block. The font only has
 * capitals, so the casing here is for readers of the source.
 */
const WORDMARK = { pine: "Pine", guard: "Guard" } as const;

/** Plain-text line under the wordmark. Stable, and never part of commander's own `--help` output. */
export const BANNER_TAGLINE =
    "Guarding every layer of the PineGuard validation stack";

/** Two-space left margin so the banner sits off the terminal's left edge. */
const MARGIN = "  ";

type Palette = ReturnType<typeof pc.createColors>;
type Paint = (text: string) => string;

/** Options for {@link renderBanner}. */
export interface BannerOptions {
    /** Emit ANSI colour. Defaults to {@link shouldUseColor}. */
    color?: boolean;
}

/** One row of the wordmark, split at the word boundary so each half can be painted independently. */
export interface WordmarkRow {
    /** The "Pine" half, padded to a uniform width so the "Guard" half lines up beneath itself on every row. */
    pine: string;
    /** The "Guard" half, right edge trimmed (it is the end of the line). */
    guard: string;
}

/**
 * Colour is a pure nicety: gate it on a real TTY and the informal NO_COLOR /
 * CI conventions so piping `pineguard`'s output to a file or a CI log never
 * captures raw escape codes. The inputs are parameters (defaulting to the
 * live process) so the policy is unit-testable without stubbing `process`.
 */
export function shouldUseColor(
    env: NodeJS.ProcessEnv = process.env,
    isTTY: boolean = Boolean(process.stdout.isTTY),
): boolean {
    // NO_COLOR (https://no-color.org) disables colour on *any* value,
    // including an empty string, so check presence rather than truthiness.
    if ("NO_COLOR" in env || "CI" in env) {
        return false;
    }
    return isTTY;
}

/**
 * One word as figlet rows, every row padded to the block's widest row so a
 * block rendered next to it stays column-aligned.
 *
 * figlet reads the `.flf` font from its own `fonts/` directory at run time.
 * That is fine here because tsup keeps `dependencies` external, so `figlet`
 * is always on disk next to the built CLI. (Should the build ever inline
 * dependencies, switch to importing `figlet/fonts/ANSI Shadow` and
 * registering it with `figlet.parseFont` instead.)
 */
function renderBlock(text: string): string[] {
    const rows = figlet.textSync(text, { font: BANNER_FONT }).split("\n");
    const width = Math.max(...rows.map((row) => row.length));
    return rows.map((row) => row.padEnd(width));
}

function isBlank(row: WordmarkRow | undefined): boolean {
    return row === undefined || (row.pine + row.guard).trim() === "";
}

/**
 * The wordmark as rows split at the "Pine" | "Guard" boundary, with figlet's
 * trailing whitespace and any blank leading/trailing rows removed. Joining
 * `pine + guard` on each row gives the plain wordmark (see
 * {@link renderWordmark}).
 */
export function renderWordmarkRows(): WordmarkRow[] {
    const pine = renderBlock(WORDMARK.pine);
    const guard = renderBlock(WORDMARK.guard);
    const rows: WordmarkRow[] = [];
    for (let i = 0; i < Math.max(pine.length, guard.length); i += 1) {
        const guardRow = (guard[i] ?? "").trimEnd();
        const pineRow = pine[i] ?? "";
        // The pine half keeps its padding so the guard half lines up — unless
        // there is no guard half on this row, in which case the padding would
        // just be trailing whitespace.
        rows.push({
            pine: guardRow === "" ? pineRow.trimEnd() : pineRow,
            guard: guardRow,
        });
    }
    let end = rows.length;
    while (end > 0 && isBlank(rows[end - 1])) {
        end -= 1;
    }
    let start = 0;
    while (start < end && isBlank(rows[start])) {
        start += 1;
    }
    return rows.slice(start, end);
}

/** The wordmark as plain rows: both halves joined, no colour. */
export function renderWordmark(): string[] {
    return renderWordmarkRows().map((row) => row.pine + row.guard);
}

/**
 * ANSI Shadow draws each glyph as solid `█` blocks with a drop shadow of
 * box-drawing strokes (`╗ ═ ╝ …`). The strokes are dimmed so the shadow
 * reads as a shadow rather than a second outline, and `body` then paints the
 * whole half (identity for the terminal-default "Guard"). With colour
 * disabled every function here is an identity, so the row comes back as-is.
 */
function paintHalf(half: string, body: Paint, palette: Palette): string {
    return body(half.replace(/[^█\s]+/g, (stroke) => palette.dim(stroke)));
}

/** Resolves the package version straight from `apps/cli/package.json` so the banner can never drift from `pineguard --version`. */
export function readVersion(): string {
    try {
        const here = path.dirname(fileURLToPath(import.meta.url));
        const pkgPath = path.join(here, "..", "package.json");
        const pkg: unknown = JSON.parse(readFileSync(pkgPath, "utf8"));
        if (
            typeof pkg === "object" &&
            pkg !== null &&
            "version" in pkg &&
            typeof (pkg as { version: unknown }).version === "string"
        ) {
            return (pkg as { version: string }).version;
        }
        return "0.0.0";
    } catch {
        return "0.0.0";
    }
}

/**
 * Builds the `pineguard` startup banner as a plain string (no `console.log`
 * inside, so it stays trivially testable and reusable from both the bare
 * `pineguard` invocation and the explicit `pineguard banner` command).
 */
export function renderBanner(options: BannerOptions = {}): string {
    const palette = pc.createColors(options.color ?? shouldUseColor());
    const plain: Paint = (text) => text;
    const wordmark = renderWordmarkRows().map(
        (row) =>
            MARGIN +
            paintHalf(row.pine, palette.green, palette) +
            paintHalf(row.guard, plain, palette),
    );
    const version = palette.dim(`v${readVersion()}`);

    return [
        "",
        ...wordmark,
        "",
        `${MARGIN}${BANNER_TAGLINE}  ${version}`,
        "",
    ].join("\n");
}
