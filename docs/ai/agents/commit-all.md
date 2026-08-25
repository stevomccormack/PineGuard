<!-- metadata_header
type: agent
id: agent-commit-all
version: 1.0
-->

# Agent: Commit All

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/commit.md`.
2. Limit execution strictly to the `-All` scope. `-All` expands to every scope and implicitly sets `-IncludeTests`.
3. Dry-run the plan first, then create the commits:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -DryRun
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -All -AutoMessage
   ```
