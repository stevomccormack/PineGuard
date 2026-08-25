<!-- metadata_header
type: agent
id: agent-scan-sonar
version: 1.0
-->

# Agent: Run SonarQube Analysis

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: reviewer ([../roles/reviewer.md](../roles/reviewer.md))

> [!NOTE]
> This is the only agent in the scan-sonar family by design — SonarQube analyses the whole
> solution, so there are no per-scope `scan-sonar-*` variants. Severity-based remediation is
> contracted in [`../commands/fix.md`](../commands/fix.md) (`/fix-sonar-{severity}`).

## Steps

1. Read the master workflow at `docs/ai/workflows/scan-sonar.md`.
2. Execute it (no parameters — SonarQube is project-wide).
