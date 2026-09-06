import { spawnSync } from "node:child_process";
import { readFileSync } from "node:fs";
import { createRequire } from "node:module";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

import figlet from "figlet";
import { describe, expect, it } from "vitest";

import {
    BANNER_FONT,
    BANNER_TAGLINE,
    renderBanner,
    renderWordmark,
    shouldUseColor,
} from "../src/banner.js";

const HERE = dirname(fileURLToPath(import.meta.url));
const CLI_ROOT = join(HERE, "..");
const ENTRY = join(CLI_ROOT, "src", "index.ts");
const require = createRequire(import.meta.url);
const TSX_CLI = require.resolve("tsx/cli");

const PACKAGE_VERSION = (
    JSON.parse(readFileSync(join(CLI_ROOT, "package.json"), "utf8")) as {
        version: string;
    }
).version;

/** A standard terminal; the wordmark must never wrap in one. */
const MAX_COLUMNS = 80;

// Built from the char code (not a `\x1b` regex literal) so eslint's
// no-control-regex stays quiet and the ESC byte can't be mangled in transit.
const ESC = String.fromCharCode(27);
const ANSI_SGR = new RegExp(`${ESC}\\[[0-9;]*m`, "g");
const stripAnsi = (text: string): string => text.replace(ANSI_SGR, "");

/** Terminal columns of a row: code points, since the font uses non-ASCII block glyphs. */
const columns = (row: string): number => [...row].length;

/**
 * Spawns the real CLI entry point (via the `tsx` runtime, same as
 * `pnpm dev`/`pnpm exec tsx src/index.ts`) rather than importing
 * `src/index.ts` in-process — that module runs `program.parseAsync` against
 * the live `process.argv` as a side effect of being imported at all, so it
 * can't safely be exercised in-process with different argv per test.
 */
function runCli(args: readonly string[]): {
    stdout: string;
    stderr: string;
    /** stdout+stderr concatenated — commander writes its default "no command"
     * help to stderr (exit 1, like bare `git`) but explicit `--help` output
     * to stdout (exit 0); tests that don't care which stream use this. */
    combined: string;
    status: number | null;
} {
    const result = spawnSync(process.execPath, [TSX_CLI, ENTRY, ...args], {
        cwd: CLI_ROOT,
        encoding: "utf8",
        timeout: 20_000,
    });
    return {
        stdout: result.stdout,
        stderr: result.stderr,
        combined: result.stdout + result.stderr,
        status: result.status,
    };
}

describe("renderWordmark", () => {
    it("is figlet's own rendering of the wordmark in BANNER_FONT, trimmed", () => {
        const expected = figlet
            .textSync("PineGuard", { font: BANNER_FONT })
            .split("\n")
            .map((row) => row.trimEnd());
        while (expected.at(-1) === "") {
            expected.pop();
        }
        expect(renderWordmark()).toEqual(expected);
    });

    it("is multi-row block art that fits a standard 80-column terminal", () => {
        const rows = renderWordmark();
        expect(rows.length).toBeGreaterThanOrEqual(5);
        expect(rows.some((row) => row.includes("█"))).toBe(true);
        for (const row of rows) {
            expect(columns(row)).toBeLessThanOrEqual(MAX_COLUMNS);
        }
    });
});

describe("shouldUseColor", () => {
    it("is on only for a TTY with neither NO_COLOR nor CI set", () => {
        expect(shouldUseColor({}, true)).toBe(true);
        expect(shouldUseColor({}, false)).toBe(false);
    });

    it("honours NO_COLOR on any value, including an empty string", () => {
        expect(shouldUseColor({ NO_COLOR: "" }, true)).toBe(false);
        expect(shouldUseColor({ NO_COLOR: "1" }, true)).toBe(false);
    });

    it("is off under CI even on a TTY", () => {
        expect(shouldUseColor({ CI: "true" }, true)).toBe(false);
    });
});

describe("renderBanner", () => {
    it("carries the wordmark, the tagline and the package version, with no ANSI when colour is off", () => {
        const banner = renderBanner({ color: false });
        expect(banner.length).toBeGreaterThan(0);
        expect(banner).not.toContain(ESC);
        for (const row of renderWordmark()) {
            expect(banner).toContain(row);
        }
        expect(banner).toContain(BANNER_TAGLINE);
        expect(banner.toLowerCase()).toContain("pineguard");
        expect(banner).toContain(`v${PACKAGE_VERSION}`);
        for (const line of banner.split("\n")) {
            expect(columns(line)).toBeLessThanOrEqual(MAX_COLUMNS);
        }
    });

    it("colour only adds ANSI styling — stripping it gives the plain banner back", () => {
        const plain = renderBanner({ color: false });
        const coloured = renderBanner({ color: true });
        expect(coloured).toContain(ESC);
        expect(stripAnsi(coloured)).toBe(plain);
    });
});

describe("pineguard CLI (integration)", () => {
    // Each case spawns a real `tsx` process (transpile + run), so give it
    // more headroom than vitest's 5s default.
    const SPAWN_TIMEOUT = 20_000;
    const [FIRST_WORDMARK_ROW] = renderWordmark();

    it(
        "bare invocation prints the banner then falls through to commander's help listing",
        () => {
            // The banner (our own console.log) lands on stdout; commander's
            // own "no command given" help — like bare `git` — goes to stderr
            // with a non-zero exit, so this checks the banner on stdout and
            // the listing on the combined stream.
            const { stdout, combined } = runCli([]);
            expect(stdout).toContain(FIRST_WORDMARK_ROW);
            expect(stdout).toContain(BANNER_TAGLINE);
            // A pipe is not a TTY, so no escape codes may leak into it.
            expect(stdout).not.toContain(ESC);
            expect(combined).toMatch(/Usage: pineguard/);
            expect(combined).toContain("audit");
            expect(combined).toContain("banner");
        },
        SPAWN_TIMEOUT,
    );

    it(
        "--help prints commander's clean help listing without the banner",
        () => {
            const { stdout, status } = runCli(["--help"]);
            expect(status).toBe(0);
            expect(stdout).not.toContain(FIRST_WORDMARK_ROW);
            expect(stdout).not.toContain(BANNER_TAGLINE);
            expect(stdout).toMatch(/Usage: pineguard/);
            expect(stdout).toContain("audit");
            expect(stdout).toContain("banner");
        },
        SPAWN_TIMEOUT,
    );

    it(
        "`pineguard banner` prints only the banner",
        () => {
            const { stdout, status } = runCli(["banner"]);
            expect(status).toBe(0);
            expect(stdout).toContain(FIRST_WORDMARK_ROW);
            expect(stdout).toContain(BANNER_TAGLINE);
            expect(stdout).not.toContain(ESC);
            expect(stdout).not.toMatch(/Usage: pineguard/);
        },
        SPAWN_TIMEOUT,
    );
});
