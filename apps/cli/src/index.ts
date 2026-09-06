#!/usr/bin/env node
import { Command } from "commander";

import { readVersion, renderBanner } from "./banner.js";
import { registerAuditCommand } from "./commands/audit.js";
import { registerBannerCommand } from "./commands/banner.js";

const program = new Command();

program
    .name("pineguard")
    .description(
        "PineGuard engineering CLI - cross-layer audit rules for the validation stack",
    )
    .version(readVersion());

registerAuditCommand(program);
registerBannerCommand(program);

// Bare `pineguard` (no subcommand, no flags at all): print the banner, then
// let commander's own default behaviour take over — with no matching command
// and no root action, commander already prints its usual help listing (and
// exits non-zero, same as e.g. bare `git`) from inside parseAsync below, so
// nothing further is needed here. This is the only case that prints the
// banner unprompted: `--help`/`-h` is handled entirely by commander and
// deliberately never sees it (most CLIs keep `--help` output clean/parseable
// for scripting), and `pineguard banner` prints it on its own via
// registerBannerCommand above.
if (process.argv.length <= 2) {
    console.log(renderBanner());
}

await program.parseAsync(process.argv);
