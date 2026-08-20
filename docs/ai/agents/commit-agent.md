<!-- metadata_header
type: agent
id: agent-commit-agent
version: 1.0
-->

# Agent: Run Scoped Git Commits (Agent) (tools/git)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/commit.md`.
2. Limit execution strictly to the `-Agent` scope. `-IncludeTests` has no effect on this scope.
3. Dry-run the plan first, then create the commits:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Agent -DryRun
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Agent -AutoMessage
   ```

4. `.vscode/tasks.json` carries equivalent tasks for human runs; an agent must invoke the PowerShell commands above directly.
