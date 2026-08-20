<!-- metadata_header
type: agent
id: agent-commit-fluent
version: 1.0
-->

# Agent: Run Scoped Git Commits (FluentValidation) (tools/git)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at `docs/ai/workflows/commit.md`.
2. Limit execution strictly to the `-FluentValidation` scope; add `-IncludeTests` to include the paired `*.UnitTests` project in the same commit.
3. Dry-run the plan first, then create the commits:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -FluentValidation -DryRun
   pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -FluentValidation -AutoMessage
   ```

4. `.vscode/tasks.json` carries equivalent tasks for human runs; an agent must invoke the PowerShell commands above directly.
