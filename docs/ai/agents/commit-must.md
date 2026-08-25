<!-- metadata_header
type: agent
id: agent-commit-must
version: 1.0
-->

# Agent: Commit MustClauses

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/commit.md`.
2. Limit execution strictly to the `-MustClauses` scope; add `-IncludeTests` to include the paired `*.UnitTests` project in the same commit.
3. Dry-run the plan first, then create the commits:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -MustClauses -DryRun
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -MustClauses -AutoMessage
   ```
