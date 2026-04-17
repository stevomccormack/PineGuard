# Workflow: Fix Roslyn Compiler Diagnostics

> [!NOTE]
> Fetches Roslyn compiler warnings by scope and fixes them in-place using idiomatic C#.

## Context

- **Role**: [Senior Engineer](../roles/owner.md)
- **Reference**: `tools/code-diagnostics/Run-CompilerDiagnostics.ps1`
- **Spec**: `docs/ai/specs/tools/code-diagnostics/spec.md`

## Parameters

- **Scope**: (`All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Testing`)
- **Filter**: (optional) Regex pattern to filter warning codes (e.g. `CS86` for nullability)

## Auto-Approval

- **Gemini**: `// turbo-all`
- **Claude**: `Project Rules` allow scripts.
- **Cursor**: `cmd: powershell` allowed.

## Steps

// turbo-all

1. **Run diagnostics**

   ```powershell
   pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope [SCOPE] -OutputFormat Json
   ```

   If a Filter is provided, add `-Filter [FILTER]`.

   Parse the JSON output. Each warning contains: `File`, `Line`, `Column`, `Code`, `Message`, `Project`.

2. **Fix warnings (one file at a time)**

   For each unique file in the warning list:
   1. Read the affected file.
   2. Understand the warning from `Code` and `Message`. Investigate root cause.
   3. Apply an idiomatic C# fix following `docs/ai/specs/coding-standard.md`.
   4. **Never suppress warnings** — fix the root cause.

3. **Verify build after each file**

   ```powershell
   dotnet build PineGuard.slnx --no-incremental
   ```

   If the build fails, revert the last change and investigate.

4. **Report**

   Summarize:
   - Total warnings found
   - Warnings fixed (with file, code, line)
   - Warnings skipped (with reason)
