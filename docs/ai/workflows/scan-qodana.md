# Workflow: Run Qodana

> [!NOTE]
> Runs JetBrains Qodana locally via the repo wrapper under `tools/code-inspection/qodana/`.

## Context

- **Role**: [Code Reviewer](../roles/reviewer.md)
- **Reference**: `tools/code-inspection/Run-Qodana.ps1`
- **Docs**: `docs/ai/specs/tools/code-inspection/qodana.md`

## Parameters

- **Scope**: (`All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Testing`)

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow scripts.
- **Cursor**: `cmd: powershell` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Run Qodana (recommended wrapper)**

   **Command Template**:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-inspection/Run-Qodana.ps1" -Scope [SCOPE] -Clean
   ```

   Notes:
   - The wrapper defaults to non-interactive runs (suppresses Qodana CLI prompts).
     - To allow prompts for a single run, pass: `-NonInteractive:$false`
   - A hard timeout is enabled by default (`-TimeoutMinutes 30`). Override if needed.

   Optional: open the generated HTML report automatically:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-inspection/Run-Qodana.ps1" -Scope [SCOPE] -Clean -OpenReport
   ```

2. **Inspect outputs**
   - Results are written under `artifacts/qodana/<scope>/`
   - Look for:
     - SARIF report: `*.sarif.json` (used for Code Scanning)
     - HTML report: `report/index.html`
