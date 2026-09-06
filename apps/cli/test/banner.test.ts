import { spawnSync } from "node:child_process";
import { readFileSync } from "node:fs";
import { createRequire } from "node:module";
import { dirname, join } from "node:path";
import { fileURLToPath } from "node:url";

import { describe, expect, it } from "vitest";

import { BANNER_ART_MARKER, renderBanner } from "../src/banner.js";

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

describe("renderBanner", () => {
    it("returns a non-empty string mentioning PineGuard and the current version", () => {
        const banner = renderBanner();
        expect(banner.length).toBeGreaterThan(0);
        expect(banner.toLowerCase()).toContain("pineguard");
        expect(banner).toContain(`v${PACKAGE_VERSION}`);
    });
});

describe("pineguard CLI (integration)", () => {
    // Each case spawns a real `tsx` process (transpile + run), so give it
    // more headroom than vitest's 5s default.
    const SPAWN_TIMEOUT = 20_000;

    it(
        "bare invocation prints the banner then falls through to commander's help listing",
        () => {
            // The banner (our own console.log) lands on stdout; commander's
            // own "no command given" help — like bare `git` — goes to stderr
            // with a non-zero exit, so this checks the banner on stdout and
            // the listing on the combined stream.
            const { stdout, combined } = runCli([]);
            expect(stdout).toContain(BANNER_ART_MARKER);
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
            expect(stdout).not.toContain(BANNER_ART_MARKER);
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
            expect(stdout).toContain(BANNER_ART_MARKER);
            expect(stdout).not.toMatch(/Usage: pineguard/);
        },
        SPAWN_TIMEOUT,
    );
});
