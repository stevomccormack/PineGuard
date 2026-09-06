import type { Command } from "commander";

import { renderBanner } from "../banner.js";

/**
 * `pineguard banner` — an explicit, scriptable way to print the banner on
 * its own (onboarding, screenshots, README asciinema-style captures) without
 * needing to invoke the CLI bare and rely on the fall-through help text.
 */
export function registerBannerCommand(program: Command): void {
    program
        .command("banner")
        .description("Print the pineguard ASCII banner and exit")
        .action(() => {
            console.log(renderBanner());
        });
}
