# Testing

Wraps `dotnet test` with scope/project/solution targeting, filtering, async execution, and
trx logging support.

## Usage

Run from the repository root.

### Run all tests

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope All
```

### Run a registry scope

```powershell
# Core unit tests, resolved from the shared scope registry
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope Core
```

### Run specific project

```powershell
# Core unit tests
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj"

# MustClauses unit tests
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.MustClauses.UnitTests/PineGuard.MustClauses.UnitTests.csproj"
```

### Run with filter

```powershell
# Only tests matching a pattern
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope Core -Filter "FullyQualifiedName~MustBoolClauses"
```

### Run a specific target framework

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope Core -Framework net10.0
```

### Async execution

```powershell
# Run in a separate process (non-blocking)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj" -Async
```

### Export results

```powershell
# Save trx results to a directory; -Scope All writes one distinctly-named trx file per
# project instead of every project overwriting a single shared file
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Scope All -OutputPath "artifacts/test-results"
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Project` | string | — | Path to a specific `.csproj` file |
| `-Solution` | string | — | Path to a specific `.sln`/`.slnx` file |
| `-Scope` | string | — | Registry scope name, or `All` (resolves to `PineGuard.slnx`) |
| `-Filter` | string | — | Test filter expression (e.g., `FullyQualifiedName~Tests`) |
| `-Framework` | string | — | Target framework moniker, passed through as `--framework` |
| `-OutputPath` | string | — | Results directory; adds a trx logger if set. Relative paths resolve against the repo root |
| `-NoBuild` | switch | `$false` | Skip build phase |
| `-Async` | switch | `$false` | Run in a separate process (`Start-Process`) |
| `-Configuration` | string | `Debug` | Build configuration |

## Notes

- Specify exactly one of `-Project`, `-Solution`, or `-Scope`.
- `-Scope` is sourced from the shared registry (`tools/.shared/dotnet-projects.ps1`); `-Scope All`
  resolves to `PineGuard.slnx` rather than a single project.
- `-Async` launches `dotnet test` via `Start-Process` so the current shell is not blocked. Useful for running tests in the background while continuing other work.
- When `-OutputPath` is specified, a trx logger is added for structured test result output. The
  logger uses `LogFilePrefix` (not a fixed `LogFileName`), so a multi-project run (`-Scope All`,
  or a `-Solution` with more than one test project) writes one distinctly-named trx file per
  project instead of every project overwriting a single shared file.
- A transcript of each run is written under `logs/testing/<yyyyMMdd-HHmmss>.log`.
