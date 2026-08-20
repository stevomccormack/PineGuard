# Workflow: Run Roslyn Compiler Diagnostics

> [!NOTE]
> Runs Roslyn compiler diagnostics via the repo wrapper under `tools/code-diagnostics/`.

## Context

- **Role**: [Code Reviewer](../roles/reviewer.md)
- **Reference**: `tools/code-diagnostics/Run-CompilerDiagnostics.ps1`
- **Docs**: `docs/ai/specs/tools/code-diagnostics/spec.md`

## Parameters

- **Scope**: (`All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Testing`)

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow scripts.
- **Cursor**: `cmd: powershell` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Run the diagnostics script**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope [SCOPE]
   ```

   Notes:
   - No Docker or external tool dependencies.
   - The script builds the scoped project and captures all `warning CS\d+` output.

2. **Review findings**

   - Text summary printed to stdout (grouped by warning code and by file).
   - JSON report written to `artifacts/code-diagnostics/<scope>/diagnostics.json`.
