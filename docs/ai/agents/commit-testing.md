<!-- metadata_header
type: agent
id: agent-commit-testing
version: 1.0
-->

# Agent: Run Scoped Git Commits (Testing) (tools/git)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/commit.md`.
2. Limit execution strictly to the `-Testing` scope; add `-IncludeTests` to include the paired `*.UnitTests` project in the same commit.
3. Dry-run the plan first, then create the commits:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Testing -DryRun
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Testing -AutoMessage
   ```

4. `.vscode/tasks.json` carries equivalent tasks for human runs; an agent must invoke the PowerShell commands above directly.
