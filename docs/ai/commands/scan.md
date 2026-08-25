<!-- metadata_header
type: command
id: cmd-scan
version: 1.1
-->

# Command: Scan

Runs static analysis using the specified tool. Scanning is read-only and auto-approved.

## Intent Mapping

| Command | Tool / Scope | Agent |
|---------|--------------|-------|
| `/scan-roslyn-all` | Roslyn, whole solution | `docs/ai/agents/scan-roslyn-all.md` |
| `/scan-roslyn-core` | Roslyn, PineGuard.Core | `docs/ai/agents/scan-roslyn-core.md` |
| `/scan-roslyn-must` | Roslyn, MustClauses | `docs/ai/agents/scan-roslyn-must.md` |
| `/scan-roslyn-guard` | Roslyn, GuardClauses | `docs/ai/agents/scan-roslyn-guard.md` |
| `/scan-roslyn-fluent` | Roslyn, FluentValidation | `docs/ai/agents/scan-roslyn-fluent.md` |
| `/scan-roslyn-annotation` | Roslyn, DataAnnotations | `docs/ai/agents/scan-roslyn-annotation.md` |
| `/scan-roslyn-testing` | Roslyn, PineGuard.Testing | `docs/ai/agents/scan-roslyn-testing.md` |
| `/scan-qodana-all` | Qodana, whole solution | `docs/ai/agents/scan-qodana-all.md` |
| `/scan-qodana-core` | Qodana, PineGuard.Core | `docs/ai/agents/scan-qodana-core.md` |
| `/scan-qodana-must` | Qodana, MustClauses | `docs/ai/agents/scan-qodana-must.md` |
| `/scan-qodana-guard` | Qodana, GuardClauses | `docs/ai/agents/scan-qodana-guard.md` |
| `/scan-qodana-fluent` | Qodana, FluentValidation | `docs/ai/agents/scan-qodana-fluent.md` |
| `/scan-qodana-annotation` | Qodana, DataAnnotations | `docs/ai/agents/scan-qodana-annotation.md` |
| `/scan-qodana-testing` | Qodana, PineGuard.Testing | `docs/ai/agents/scan-qodana-testing.md` |
| `/scan-sonar` | SonarQube, whole solution (no per-scope variants) | `docs/ai/agents/scan-sonar.md` |

## Repairing what a scan reports

Contracted in [`fix.md`](fix.md): `/fix-roslyn-all` for Roslyn, `/fix-sonar-{severity}` for
SonarQube. There is **no `fix-qodana` family by design** — Qodana findings largely overlap the
Roslyn and Sonar rule sets, so remediation goes through those commands or conversationally from
the Qodana report.
