<!-- metadata_header
type: agent
id: agent-commit-guard
version: 1.0
-->

# Agent: Commit GuardClauses

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/commit.md`.
2. Limit execution strictly to the `-Scope GuardClauses` scope; add `-IncludeTests` to include the paired `*.UnitTests` project in the same commit.
3. Dry-run the plan first, then create the commits:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope GuardClauses -WhatIf
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope GuardClauses -AutoMessage
   ```
