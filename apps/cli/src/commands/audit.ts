import type { Command } from "commander";

import { getAllRules, isRuleScope, type RuleScope } from "../audit/catalog.js";
import {
    runAudit,
    runUpdateBaseline,
    type AuditRunOptions,
} from "../audit/engine.js";
import { renderGithub } from "../audit/reporters/github.js";
import { buildJsonReport, writeJsonReport } from "../audit/reporters/json.js";
import { renderPretty } from "../audit/reporters/pretty.js";
import { renderSarif } from "../audit/reporters/sarif.js";
import { findRepoRoot } from "../audit/repo.js";
// Side-effect import: every rule module under src/audit/rules/*.ts registers
// itself into the catalog (see src/audit/catalog.ts's header comment) by
// calling registerRule() at import time, reached transitively through this
// barrel. This is the one place that import chain runs for a real
// `pineguard audit` invocation; empty until plan P2 lands the first rule.
import "../audit/rules/index.js";

const SUPPORTED_FORMATS = ["pretty", "json", "github", "sarif"] as const;
type SupportedFormat = (typeof SUPPORTED_FORMATS)[number];

function isSupportedFormat(value: string): value is SupportedFormat {
    return (SUPPORTED_FORMATS as readonly string[]).includes(value);
}

interface AuditOptions {
    scope?: string;
    gate?: boolean;
    list?: boolean;
    format?: string;
    /** `--changed` (plan P3.2): restrict `ctx.trackedFiles` to files that differ from `main`/`origin/main`. See `engine.ts`'s `AuditRunOptions.changed` doc comment for exactly which rules this reaches. */
    changed?: boolean;
    /** `--update-baseline` (plan §4.4): handled entirely by {@link runUpdateBaselineCommand}. */
    updateBaseline?: boolean;
    /** `--no-baseline` (plan §4.4). commander maps it to `baseline: false`; absent = `true` (apply the ratchet). */
    baseline?: boolean;
}

/** `pineguard audit --list`: prints slug/legacyId/scope/gate/description for every registered rule, or a clear "nothing here yet" message. */
function printCatalog(): void {
    const rules = getAllRules();
    if (rules.length === 0) {
        console.log(
            "No rules registered yet — the catalog is empty. Rules land in plan P2; " +
                "see docs/ai/plans/audit-cli-rebuild.md §9.2.",
        );
        return;
    }

    const header = ["slug", "legacyId", "scope", "gate", "description"];
    const rows = rules.map((rule) => [
        rule.slug,
        rule.legacyId ?? "-",
        rule.scope,
        String(rule.gate),
        rule.description,
    ]);
    const widths = header.map((title, index) =>
        Math.max(title.length, ...rows.map((row) => row[index]?.length ?? 0)),
    );
    const renderRow = (cells: string[]): string =>
        cells
            .map((cell, index) => cell.padEnd(widths[index] ?? 0))
            .join("  ")
            .trimEnd();

    console.log(renderRow(header));
    for (const row of rows) {
        console.log(renderRow(row));
    }
}

/**
 * `pineguard audit --update-baseline` (plan §4.4): a maintenance operation,
 * not a pass/fail check. Snapshots every registered rule's current,
 * unsuppressed findings into `apps/cli/config/baseline.json` via
 * {@link runUpdateBaseline}, then exits `0` — regardless of how many
 * findings were baselined — unless something actually went wrong building
 * the context or running a rule, in which case it exits `2` like any other
 * usage/config error.
 */
async function runUpdateBaselineCommand(): Promise<void> {
    const result = await runUpdateBaseline();

    if (!result.written) {
        console.error(
            `pineguard audit --update-baseline: ${result.error ?? "failed to write apps/cli/config/baseline.json"}`,
        );
        process.exitCode = 2;
        return;
    }

    const slugs = Object.keys(result.baseline).sort((a, b) =>
        a.localeCompare(b),
    );
    const totalFindings = slugs.reduce(
        (sum, slug) => sum + (result.baseline[slug]?.length ?? 0),
        0,
    );
    console.log(
        `pineguard audit: wrote apps/cli/config/baseline.json — ` +
            `${String(totalFindings)} finding(s) accepted as pre-existing debt across ${String(slugs.length)} rule(s).`,
    );
    for (const slug of slugs) {
        console.log(`  ${slug}: ${String(result.baseline[slug]?.length ?? 0)}`);
    }
    process.exitCode = 0;
}

async function runAuditCommand(
    rules: string[],
    options: AuditOptions,
): Promise<void> {
    if (options.list) {
        printCatalog();
        return;
    }

    if (options.updateBaseline) {
        // A separate maintenance flow (plan §4.4) — always snapshots the
        // whole catalog, so any rule-selection/--scope/--gate arguments also
        // given alongside --update-baseline are deliberately ignored rather
        // than narrowing what gets snapshotted (see runUpdateBaseline's doc
        // comment in engine.ts for why: a partial snapshot would silently
        // drift the ratchet floor out of sync for the rules not selected).
        await runUpdateBaselineCommand();
        return;
    }

    const format = options.format ?? "pretty";
    if (!isSupportedFormat(format)) {
        console.error(
            `pineguard audit: --format ${format} is not supported — ` +
                `use --format pretty, --format json, --format github, or --format sarif.`,
        );
        process.exitCode = 2;
        return;
    }

    if (options.scope !== undefined && !isRuleScope(options.scope)) {
        console.error(
            `pineguard audit: unknown --scope "${options.scope}" (expected library|testing|docs).`,
        );
        process.exitCode = 2;
        return;
    }

    const runOptions: AuditRunOptions = {
        rules,
        scope: options.scope as RuleScope | undefined,
        gate: options.gate,
        // commander maps --no-baseline to `baseline: false`; absent (true)
        // applies the ratchet as normal (plan §4.4).
        baseline: options.baseline,
        changed: options.changed,
    };

    const result = await runAudit(runOptions);

    if (result.exitCode === 2) {
        console.error(`pineguard audit: ${result.error ?? "usage error"}`);
        process.exitCode = 2;
        return;
    }

    if (result.warnings) {
        for (const warning of result.warnings) {
            console.error(warning);
        }
    }

    if (format === "json") {
        const report = buildJsonReport(result);
        writeJsonReport(report, findRepoRoot());
        console.log(JSON.stringify(report, null, 2));
    } else if (format === "github") {
        console.log(renderGithub(result));
    } else if (format === "sarif") {
        console.log(renderSarif(result));
    } else {
        console.log(renderPretty(result));
    }

    process.exitCode = result.exitCode;
}

export function registerAuditCommand(program: Command): void {
    program
        .command("audit")
        .description("Run PineGuard's cross-layer audit rules")
        .argument(
            "[rules...]",
            "rule slugs or legacy RuleNN ids to run (default: all)",
        )
        .option("--scope <scope>", "restrict to a scope: library|testing|docs")
        .option("--gate", "only run merge-blocking rules")
        .option("--list", "print the rule catalog and exit")
        .option(
            "--format <format>",
            "output format: pretty|json|github|sarif",
            "pretty",
        )
        .option(
            "--changed",
            "restrict to files changed vs main/origin-main (fast pre-commit feedback)",
        )
        .option(
            "--update-baseline",
            "accept current findings as the new ratchet floor (plan §4.4); a maintenance operation, always exits 0",
        )
        .option(
            "--no-baseline",
            "show the full debt, ignoring the ratchet (plan §4.4)",
        )
        .action(async (rules: string[], options: AuditOptions) => {
            await runAuditCommand(rules, options);
        });
}
