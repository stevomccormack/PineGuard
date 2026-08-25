<!-- metadata_header
type: agent
id: agent-commit-solution
version: 1.0
-->

# Agent: Commit Solution

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/commit.md`.
2. Limit execution strictly to the `-Solution` scope. `-IncludeTests` has no effect on this scope.
3. Dry-run the plan first, then create the commits:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Solution -DryRun
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Solution -AutoMessage
   ```
