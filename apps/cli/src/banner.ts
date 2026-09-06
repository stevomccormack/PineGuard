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

/** The font only has capitals, so this casing is for readers of the source. */
const WORDMARK = "PineGuard";

/** Plain-text line under the wordmark. Stable, and never part of commander's own `--help` output. */
export const BANNER_TAGLINE =
    "Guarding every layer of the PineGuard validation stack";

/** Two-space left margin so the banner sits off the terminal's left edge. */
const MARGIN = "  ";

type Palette = ReturnType<typeof pc.createColors>;

/** Options for {@link renderBanner}. */
export interface BannerOptions {
    /** Emit ANSI colour. Defaults to {@link shouldUseColor}. */
    color?: boolean;
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
 * The wordmark as plain rows: figlet's output with trailing whitespace and
 * any blank leading/trailing rows removed.
 *
 * figlet reads the `.flf` font from its own `fonts/` directory at run time.
 * That is fine here because tsup keeps `dependencies` external, so `figlet`
 * is always on disk next to the built CLI. (Should the build ever inline
 * dependencies, switch to importing `figlet/fonts/ANSI Shadow` and
 * registering it with `figlet.parseFont` instead.)
 */
export function renderWordmark(): string[] {
    const rows = figlet
        .textSync(WORDMARK, { font: BANNER_FONT })
        .split("\n")
        .map((row) => row.trimEnd());
    while (rows.length > 0 && rows.at(-1) === "") {
        rows.pop();
    }
    while (rows.length > 0 && rows[0] === "") {
        rows.shift();
    }
    return rows;
}

/**
 * ANSI Shadow draws each glyph as solid `█` blocks with a drop shadow of
 * box-drawing strokes (`╗ ═ ╝ …`). Painting the blocks green and the strokes
 * dim green makes the shadow read as a shadow rather than a second outline.
 * With colour disabled the palette functions are identities, so the row
 * comes back untouched.
 */
function paintRow(row: string, palette: Palette): string {
    return palette.green(
        row.replace(/[^█\s]+/g, (stroke) => palette.dim(stroke)),
    );
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
    const wordmark = renderWordmark().map(
        (row) => MARGIN + paintRow(row, palette),
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
