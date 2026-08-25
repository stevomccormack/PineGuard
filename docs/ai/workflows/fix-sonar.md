<!-- metadata_header
type: workflow
id: workflow-fix-sonar
version: 1.1
-->

# Workflow: Fix Sonar

> [!NOTE]
> Fetches SonarQube issues by severity and fixes them in-place using idiomatic C#.

## Context

- **Role**: [Senior Engineer](../roles/owner.md)
- **Skill**: [Fix Sonar Issues](../skills/fix-sonar/SKILL.md) — the canonical procedure
- **Reference**: `tools/sonar-scanner/Get-SonarIssues.ps1`
- **Spec**: `docs/ai/specs/scan/spec.md`

## Parameters

- **Severity**: (`All`, `Blocker`, `High`, `Medium`, `Low`)

## Auto-Approval

Not auto-approved on any surface — this workflow writes code. The fetch/health-check commands it
uses may be individually approved, but the repair loop requires explicit user intent.
See [`../commands/fix.md`](../commands/fix.md).

## Steps

1. **Execute the canonical procedure** in [`../skills/fix-sonar/SKILL.md`](../skills/fix-sonar/SKILL.md)
   with **Severity = [SEVERITY]**: verify SonarQube is UP, fetch the issues, then fix them one file
   at a time — idiomatic C# per `docs/ai/specs/coding-standard.md`, never suppressing a finding —
   building after each file.

2. **Report**
   - Total issues fetched
   - Issues fixed (with file and rule)
   - Issues skipped (with reason)
