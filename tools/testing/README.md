# Testing

Wraps `dotnet test` with filtering, async execution, and trx logging support.

## Usage

Run from the repository root.

### Run all tests

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1"
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
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Filter "FullyQualifiedName~MustBoolClauses"
```

### Async execution

```powershell
# Run in a separate process (non-blocking)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Project "tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj" -Async
```

### Export results

```powershell
# Save trx results to a directory
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/testing/Run-Tests.ps1" -Output "artifacts/test-results"
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Project` | string | — | Path to a specific `.csproj` file |
| `-Solution` | string | — | Path to a specific `.sln`/`.slnx` file |
| `-Filter` | string | — | Test filter expression (e.g., `FullyQualifiedName~Tests`) |
| `-Output` | string | — | Results directory; adds trx logger if set |
| `-NoBuild` | switch | `$false` | Skip build phase |
| `-Async` | switch | `$false` | Run in a separate process (`Start-Process`) |
| `-Configuration` | string | `Debug` | Build configuration |

## Notes

- Specify either `-Project` or `-Solution`, not both.
- `-Async` launches `dotnet test` via `Start-Process` so the current shell is not blocked. Useful for running tests in the background while continuing other work.
- When `-Output` is specified, a trx logger is added for structured test result output.
