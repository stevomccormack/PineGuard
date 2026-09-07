import type { AuditRunResult } from "../engine.js";
import type { Finding } from "../types.js";

/**
 * The `github` reporter (plan §4.2/§7 point 3): emits one GitHub Actions
 * workflow-command annotation per finding, so a `pineguard audit --format
 * github` step in a PR workflow gets its findings surfaced as inline
 * check-run annotations, the same way `dotnet build`/eslint's own GitHub
 * formatters do.
 *
 * `sarif` (P3.3) is the other P3 reporter — not built here.
 *
 * ## `error` vs `warning`
 *
 * A finding from a `gate: true` rule (plan §4.3's "Gate at cutover" column —
 * `doc-links`, `surface-parity`, `test-files` today) is emitted as
 * `::error`: these are the rules that already block merges, so their
 * findings should render as failing, red annotations. Every other rule's
 * findings (still accumulating debt behind the P3.1 baseline ratchet, plan
 * §4.4) are emitted as `::warning` — visible on the PR without turning the
 * check red for debt nobody has asked to gate on yet. This mirrors
 * `pretty`/`json`'s own per-rule `entry.gate` field; no new classification
 * is invented here.
 *
 * ## Format
 *
 * `::error file={file},line={line},title={rule}::{message}` (or `::warning`
 * for a non-gate rule); `,line={line}` is omitted for a finding with no line
 * number, matching `pretty.ts`'s `renderLocation` convention for the same
 * case. `title` is set to the rule's slug — one of the workflow command's
 * documented optional properties — so a PR annotation is traceable back to
 * the rule that raised it without parsing the message text. `file`, `line`,
 * and `title` are percent-escaped per GitHub's documented property-escaping
 * rules (`%` → `%25`, CR → `%0D`, LF → `%0A`, plus `:` → `%3A` and
 * `,` → `%2C` for property values specifically); `message` gets the same
 * three data-escapes but not the property-only two, since colons and commas
 * are unremarkable inside free-text messages.
 *
 * Every finding gets a line, uncapped — unlike `pretty`'s human-facing
 * truncation to the first N per rule, a GitHub annotation is consumed by the
 * Actions UI/API, not scrolled past by a human, and capping it would silently
 * hide real findings from the PR.
 */

function escapeData(value: string): string {
    return value
        .replaceAll("%", "%25")
        .replaceAll("\r", "%0D")
        .replaceAll("\n", "%0A");
}

function escapeProperty(value: string): string {
    return escapeData(value).replaceAll(":", "%3A").replaceAll(",", "%2C");
}

function renderAnnotation(
    level: "error" | "warning",
    finding: Finding,
): string {
    const properties = [`file=${escapeProperty(finding.file)}`];
    if (finding.line !== undefined) {
        properties.push(`line=${escapeProperty(String(finding.line))}`);
    }
    properties.push(`title=${escapeProperty(finding.rule)}`);

    return `::${level} ${properties.join(",")}::${escapeData(finding.message)}`;
}

/**
 * Renders `result` as GitHub Actions workflow-command annotations and
 * returns them as a plain string (one annotation per line) — the caller
 * (`src/commands/audit.ts`) is the one that actually `console.log`s it, same
 * convention as `pretty.ts`'s `renderPretty`.
 */
export function renderGithub(result: AuditRunResult): string {
    const lines: string[] = [];

    for (const { entry, findings } of result.outcomes) {
        const level = entry.gate ? "error" : "warning";
        for (const finding of findings) {
            lines.push(renderAnnotation(level, finding));
        }
    }

    return lines.join("\n");
}
