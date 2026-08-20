# Workflow: Format Code

> [!NOTE]
> Standard workflow for enforcing .editorconfig formatting rules via dotnet format.

## Context

- **Role**: [Software Engineer](../roles/builder.md)
- **Skill**: [Format Code](../skills/format-code/SKILL.md)

## Parameters

- **Scope**: (Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Testing, All)

## Auto-Approval

- **Antigravity**: `// turbo-all` in `.agent/workflows/`.
- **Claude Code**: `Project Rules` allow formatting.
- **Cursor**: `cmd: dotnet format` allowed.

See [Adapter Surfaces](../meta/adapter-surfaces.md) for the full surface inventory.

## Steps

// turbo-all

1. **Execute Formatter**
   Run the code formatter for the specified scope.

   **Command Template**:

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-formatter/Run-Format.ps1" -Scope [SCOPE]
   ```

   **Scope map** (resolves to source projects):
   - Core: `src/PineGuard.Core/PineGuard.Core.csproj`
   - MustClauses: `src/PineGuard.MustClauses/PineGuard.MustClauses.csproj`
   - GuardClauses: `src/PineGuard.GuardClauses/PineGuard.GuardClauses.csproj`
   - FluentValidation: `src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj`
   - DataAnnotations: `src/PineGuard.DataAnnotations/PineGuard.DataAnnotations.csproj`
   - Testing: `tests/PineGuard.Testing/PineGuard.Testing.csproj`
   - All: `PineGuard.slnx` (entire solution including tests)

   **Verify mode** (CI/dry-run):

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-formatter/Run-Format.ps1" -Scope [SCOPE] -VerifyNoChanges
   ```

2. **Check Results**
   Ensure dotnet format exited with code 0 (no violations).
