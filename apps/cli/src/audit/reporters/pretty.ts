import type { AuditRunResult } from "../engine.js";

/**
 * The default `pineguard audit` reporter: a per-rule header + first-N
 * findings, then a summary table (plan §4.2/§7 point 3).
 */

const DEFAULT_MAX_FINDINGS_PER_RULE = 20;

export interface PrettyOptions {
    /** How many findings to print per rule before collapsing to "...and N more". Default 20. */
    maxFindingsPerRule?: number;
}

function renderLocation(finding: { file: string; line?: number }): string {
    return finding.line === undefined
        ? finding.file
        : `${finding.file}:${String(finding.line)}`;
}

function padColumns(rows: string[][]): string[] {
    const widths: number[] = [];
    for (const row of rows) {
        row.forEach((cell, index) => {
            widths[index] = Math.max(widths[index] ?? 0, cell.length);
        });
    }
    return rows.map((row) =>
        row
            .map((cell, index) => cell.padEnd(widths[index] ?? 0))
            .join("  ")
            .trimEnd(),
    );
}

/**
 * Renders `result` as the pretty (human-readable) report and returns it as a
 * plain string — the caller (`src/commands/audit.ts`) is the one that
 * actually `console.log`s it, same convention as `src/banner.ts`'s
 * `renderBanner`.
 */
export function renderPretty(
    result: AuditRunResult,
    options: PrettyOptions = {},
): string {
    const max = options.maxFindingsPerRule ?? DEFAULT_MAX_FINDINGS_PER_RULE;
    const lines: string[] = [];

    if (result.outcomes.length === 0) {
        lines.push(
            "pineguard audit: no rules selected (empty catalog, or every rule was filtered out).",
        );
        return lines.join("\n");
    }

    for (const { entry, findings } of result.outcomes) {
        const label = entry.legacyId
            ? `${entry.slug} (${entry.legacyId})`
            : entry.slug;
        lines.push("");
        lines.push(`${label} — ${String(findings.length)} finding(s)`);
        for (const finding of findings.slice(0, max)) {
            lines.push(`  ${renderLocation(finding)}: ${finding.message}`);
        }
        if (findings.length > max) {
            lines.push(`  ...and ${String(findings.length - max)} more`);
        }
    }

    const totalFindings = result.outcomes.reduce(
        (sum, o) => sum + o.findings.length,
        0,
    );
    const header = ["rule", "count", "gate"];
    const rows = result.outcomes.map(({ entry, findings }) => [
        entry.slug,
        String(findings.length),
        String(entry.gate),
    ]);

    lines.push("");
    lines.push("Summary:");
    for (const row of padColumns([header, ...rows])) {
        lines.push(row);
    }
    lines.push(
        `total: ${String(totalFindings)} finding(s) across ${String(result.outcomes.length)} rule(s)`,
    );

    return lines.join("\n");
}
