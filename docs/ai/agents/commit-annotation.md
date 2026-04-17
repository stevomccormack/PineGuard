<!-- metadata_header
type: agent
id: agent-commit-annotation
version: 1.0
-->

# Agent: Run Scoped Git Commits (DataAnnotations) (tools/git)

> [!IMPORTANT]
> business unit: engineering ([../business-units/engineering.md](../business-units/engineering.md))
> roles: shipper ([../roles/shipper.md](../roles/shipper.md))

## Steps

1. Read the master workflow at "docs/ai/workflows/commit.md".
2. Limit execution strictly to the $(System.Collections.Hashtable.arg) scope (e.g., -DataAnnotations -AutoMessage and -IncludeTests if applicable).
3. Prefer a dry-run first (-DryRun) before making commits.
4. Run the matching VS Code task if available (preferred), otherwise run the PowerShell command.
