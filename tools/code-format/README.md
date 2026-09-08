# Code Format

Wraps `dotnet format` with named scope support and verification mode.

Uses `.editorconfig` rules automatically (`dotnet format` reads them by default).

## Usage

Run from the repository root.

### Format by scope

```powershell
# Format Core project only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-format/Run-Format.ps1" -Scope Core

# Format all projects (full solution)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-format/Run-Format.ps1" -Scope All
```

### Format specific project or solution

```powershell
# Specific project
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-format/Run-Format.ps1" -Project src/PineGuard.Core/PineGuard.Core.csproj

# Specific solution
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-format/Run-Format.ps1" -Solution ./PineGuard.slnx
```

### Verification mode (CI / dry-run)

```powershell
# Fail if any files would be changed (useful for CI pipelines)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-format/Run-Format.ps1" -Scope All -VerifyNoChanges
```

### Additional options

```powershell
# Only format warnings and above
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-format/Run-Format.ps1" -Scope Core -Severity warn

# Skip the implicit restore phase
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-format/Run-Format.ps1" -Scope Core -NoRestore
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Project` | string | — | Path to a specific `.csproj` file |
| `-Solution` | string | — | Path to a specific `.sln`/`.slnx` file |
| `-Scope` | string | — | Named scope: `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Options`, `DependencyInjection`, `AspNetCore`, `ErrorOr`, `FluentResults`, `OneOf`, `MediatR`, `Analyzers`, `Testing`, `All` |
| `-VerifyNoChanges` | switch | `$false` | Verification mode (`--verify-no-changes`). Exits non-zero if changes needed |
| `-Severity` | string | — | Minimum severity: `info`, `warn`, `error` |
| `-NoRestore` | switch | `$false` | Skip the implicit restore phase (`--no-restore`) |
| `-Verbosity` | string | — | MSBuild verbosity: `quiet`, `minimal`, `normal`, `detailed`, `diagnostic` |
| `-Configuration` | string | `Debug` | Build configuration |

## Scope Mapping

| Scope | Target |
|-------|--------|
| `Core` | `src/PineGuard.Core/PineGuard.Core.csproj` |
| `MustClauses` | `src/PineGuard.MustClauses/PineGuard.MustClauses.csproj` |
| `GuardClauses` | `src/PineGuard.GuardClauses/PineGuard.GuardClauses.csproj` |
| `FluentValidation` | `src/PineGuard.FluentValidation/PineGuard.FluentValidation.csproj` |
| `DataAnnotations` | `src/PineGuard.DataAnnotations/PineGuard.DataAnnotations.csproj` |
| `Options` | `src/PineGuard.Extensions.Options/PineGuard.Extensions.Options.csproj` |
| `DependencyInjection` | `src/PineGuard.Extensions.DependencyInjection/PineGuard.Extensions.DependencyInjection.csproj` |
| `AspNetCore` | `src/PineGuard.AspNetCore/PineGuard.AspNetCore.csproj` |
| `ErrorOr` | `src/PineGuard.ErrorOr/PineGuard.ErrorOr.csproj` |
| `FluentResults` | `src/PineGuard.FluentResults/PineGuard.FluentResults.csproj` |
| `OneOf` | `src/PineGuard.OneOf/PineGuard.OneOf.csproj` |
| `MediatR` | `src/PineGuard.MediatR/PineGuard.MediatR.csproj` |
| `Analyzers` | `src/PineGuard.Analyzers/PineGuard.Analyzers.csproj` **and** `src/PineGuard.Analyzers.CodeFixes/PineGuard.Analyzers.CodeFixes.csproj` (one scope, two projects — each gets its own `dotnet format` pass) |
| `Testing` | `tests/PineGuard.Testing/PineGuard.Testing.csproj` |
| `All` | `PineGuard.slnx` (src + tests) |

## Notes

- Specify either `-Project`, `-Solution`, or `-Scope` — not multiple.
- `All` scope targets the full solution including test projects.
- A transcript of each run is written under `logs/code-format/<yyyyMMdd-HHmmss>.log`.
