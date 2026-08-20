<!-- metadata_header
type: command
id: cmd-scan
version: 1.0
-->

# Command: Scan

Runs static analysis using the specified tool.

## Intent Mapping

| Intent | Agent |
|--------|-------|
| Scan with Roslyn (all) | `docs/ai/agents/scan-roslyn-all.md` |
| Scan with Roslyn ({scope}) | `docs/ai/agents/scan-roslyn-{scope}.md` |
| Scan with Qodana (all) | `docs/ai/agents/scan-qodana-all.md` |
| Scan with Qodana ({scope}) | `docs/ai/agents/scan-qodana-{scope}.md` |
| Scan with SonarQube | `docs/ai/agents/scan-sonar.md` |

Scanning is read-only. Repairing what a scan reports is contracted in [`fix.md`](fix.md)
(`/fix-roslyn-all`, `/fix-sonar-{severity}`).