import type { Command } from "commander";

/**
 * Shape a real catalog entry will have once `src/audit/catalog.ts` lands
 * (plan P1.5). This stub only needs enough of the shape to render an
 * (empty) `--list` table — it intentionally does not import from
 * `src/audit/*` so the P1 foundation agents can land those files without
 * racing this one.
 */
interface CatalogEntry {
    slug: string;
    legacyId: string;
    scope: "library" | "testing" | "docs";
    gate: boolean;
    description: string;
}

// No rules ship in this phase (P1.1). P2 rule agents populate the real
// catalog in src/audit/catalog.ts; this stays empty on purpose until then.
const EMPTY_CATALOG: readonly CatalogEntry[] = [];

interface AuditOptions {
    scope?: string;
    gate?: boolean;
    list?: boolean;
    format?: string;
    changed?: boolean;
    updateBaseline?: boolean;
    // commander maps `--no-baseline` to `baseline: false`; absent = true.
    baseline?: boolean;
}

function printCatalog(catalog: readonly CatalogEntry[]): void {
    console.log("slug\tlegacyId\tscope\tgate\tdescription");
    if (catalog.length === 0) {
        console.log(
            "(no rules registered yet — scaffold only; see plan P1.5/P2)",
        );
        return;
    }
    // TODO(P1.5): real column alignment / table rendering once rules exist.
    for (const rule of catalog) {
        console.log(
            `${rule.slug}\t${rule.legacyId}\t${rule.scope}\t${rule.gate}\t${rule.description}`,
        );
    }
}

function runAudit(rules: readonly string[], options: AuditOptions): void {
    if (options.list) {
        printCatalog(EMPTY_CATALOG);
        return;
    }

    // TODO(P1.5): resolve selection (all / slugs / legacy ids / --scope / --gate),
    // build the shared context (tracked files, C# parse cache, vocabulary,
    // exceptions, baseline), run rules, apply exceptions + baseline, hand
    // findings to a reporter, and return the real exit code (0/1/2).
    // TODO(P3): --changed, --update-baseline, --no-baseline, github/sarif formats.
    const requested = rules.length > 0 ? rules.join(", ") : "all";
    console.log(
        `pineguard audit: engine not implemented yet (scaffold only — see ` +
            `docs/ai/plans/audit-cli-rebuild.md P1.5). Requested rules: ${requested}; ` +
            `scope=${options.scope ?? "(none)"} gate=${Boolean(options.gate)} ` +
            `format=${options.format ?? "pretty"} changed=${Boolean(options.changed)} ` +
            `updateBaseline=${Boolean(options.updateBaseline)} ` +
            `baseline=${options.baseline === false ? "disabled" : "enabled"}`,
    );
}

export function registerAuditCommand(program: Command): void {
    program
        .command("audit")
        .description(
            "Run PineGuard's cross-layer audit rules (scaffold — no rules registered yet)",
        )
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
        .option("--changed", "restrict to files changed vs main")
        .option(
            "--update-baseline",
            "accept current findings as the new ratchet floor",
        )
        .option("--no-baseline", "show the full debt, ignoring the ratchet")
        .action((rules: string[], options: AuditOptions) => {
            runAudit(rules, options);
        });
}
