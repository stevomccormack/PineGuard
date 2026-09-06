import { readFileSync } from "node:fs";
import path from "node:path";
import { fileURLToPath } from "node:url";

/**
 * A tiny 5x5 block font, ASCII-only (`#` ink / space background), covering
 * only the letters `renderBanner` actually needs. Every glyph is exactly 5
 * rows of 5 columns so the wordmark lines up without a hand-eyeballed fudge —
 * `PINEGUARD`'s letters are all distinct, so this is the whole alphabet the
 * banner requires.
 */
const FONT: Readonly<Record<string, readonly string[]>> = {
    P: ["#### ", "#   #", "#### ", "#    ", "#    "],
    I: ["#####", "  #  ", "  #  ", "  #  ", "#####"],
    N: ["#   #", "##  #", "# # #", "#  ##", "#   #"],
    E: ["#####", "#    ", "#### ", "#    ", "#####"],
    G: [" ####", "#    ", "# ###", "#   #", " ####"],
    U: ["#   #", "#   #", "#   #", "#   #", " ### "],
    A: [" ### ", "#   #", "#####", "#   #", "#   #"],
    R: ["#### ", "#   #", "#### ", "#  # ", "#   #"],
    D: ["#### ", "#   #", "#   #", "#   #", "#### "],
};

const GLYPH_ROWS = 5;
const WORDMARK = "PINEGUARD";
const TAGLINE = "The PineGuard engineering CLI - guarding every layer.";

/** Unique to the rendered banner's ASCII art; never appears in commander's own `--help` listing. */
export const BANNER_ART_MARKER = "#####";

// Built from the char code (rather than a literal escape sequence in source)
// so the ESC control character can't get mangled by an editor, git, or a
// copy/paste round-trip.
const ESC = String.fromCharCode(27);
const ANSI_GREEN = `${ESC}[32m`;
const ANSI_DIM = `${ESC}[2m`;
const ANSI_RESET = `${ESC}[0m`;

/**
 * Colour is a pure nicety: gate it on a real TTY and the informal NO_COLOR /
 * CI conventions so piping `pineguard`'s output to a file or a CI log never
 * captures raw escape codes.
 */
function shouldUseColor(): boolean {
    // NO_COLOR (https://no-color.org) disables colour on *any* value,
    // including an empty string, so check presence rather than truthiness.
    if ("NO_COLOR" in process.env || "CI" in process.env) {
        return false;
    }
    return Boolean(process.stdout.isTTY);
}

function colorize(text: string, code: string): string {
    return shouldUseColor() ? `${code}${text}${ANSI_RESET}` : text;
}

function renderWordmarkRows(word: string): string[] {
    const rows: string[] = [];
    for (let row = 0; row < GLYPH_ROWS; row += 1) {
        rows.push(
            word
                .split("")
                .map((letter) => FONT[letter]?.[row] ?? "     ")
                .join(" "),
        );
    }
    return rows;
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
export function renderBanner(): string {
    const wordmarkRows = renderWordmarkRows(WORDMARK);
    const width = Math.max(
        ...wordmarkRows.map((row) => row.length),
        TAGLINE.length,
    );
    const rule = "-".repeat(width);
    const version = `v${readVersion()}`;

    const lines = [
        "",
        ...wordmarkRows.map((row) => colorize(row, ANSI_GREEN)),
        rule,
        TAGLINE,
        colorize(version, ANSI_DIM),
        "",
    ];
    return lines.join("\n");
}
