# Code Diagnostics (Roslyn Compiler Warnings)

Captures and reports warning- and error-severity compiler diagnostics from `dotnet build` — any
`<PREFIX><digits>` code the build pipeline emits (Roslyn `CS` today, plus e.g. NuGet audit `NU19xx`,
ApiCompat `CP0xxx`, IL trimmer/AOT `IL2xxx`/`IL3xxx`), not just `CS`-prefixed ones.

## Prerequisites

- .NET SDK (already installed for PineGuard development)
- No Docker or external tools required

## Usage

```powershell
# All projects
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope All

# Single scope
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope Core

# Filter to nullability warnings only
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope All -Code "CS86"

# JSON output
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope All -OutputFormat Json
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Scope` | ValidateSet | `All` | `All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Options`, `DependencyInjection`, `AspNetCore`, `ErrorOr`, `FluentResults`, `OneOf`, `MediatR`, `Analyzers`, `Testing` |
| `-Code` | string | _(none)_ | Regex pattern to filter diagnostic codes (e.g. `CS86`, `CS0618`, `NU19`). Not `-Filter` — that name is reserved for `dotnet test`'s own filter syntax elsewhere in the toolchain |
| `-OutputFormat` | ValidateSet | `Text` | `Text` (human-readable) or `Json` (structured) |
| `-Configuration` | ValidateSet | `Debug` | `Debug` or `Release` |
| `-Clean` | switch | `$false` | Run `dotnet clean` before building |

## Output

### Artifacts

Written to `artifacts/code-diagnostics/<scope>/diagnostics.json`:

```json
{
  "Scope": "All",
  "Configuration": "Debug",
  "Code": null,
  "Timestamp": "2026-09-09T10:14:52.1234567+01:00",
  "BuildSucceeded": true,
  "FailedBuildTargets": [],
  "TotalWarnings": 2,
  "TotalErrors": 0,
  "ByCode": [{ "Code": "CS8604", "Count": 1 }, { "Code": "CS8619", "Count": 1 }],
  "ByFile": [{ "File": "...", "Count": 2 }],
  "Warnings": [
    {
      "File": "...",
      "Line": 56,
      "Column": 31,
      "Severity": "warning",
      "CodePrefix": "CS",
      "CodeNumber": "8604",
      "Code": "CS8604",
      "Message": "...",
      "Project": "...",
      "TargetFrameworks": ["net8.0", "net10.0"],
      "Occurrences": 2
    }
  ],
  "Errors": []
}
```

`Code` echoes the `-Code` filter (`null` when none was given). `BuildSucceeded` and
`FailedBuildTargets` record whether every target actually compiled — a consumer that ignores them
can end up "fixing warnings" against a broken build. `Warnings` and `Errors` are separate arrays
split by `Severity`; entries are deduped across a multi-targeting project's per-TFM build passes,
with `TargetFrameworks` recording which TFMs each diagnostic was seen in and `Occurrences` the raw
pre-dedupe count.

### Exit Codes

- `0` — Build succeeded, no diagnostics matched
- `1` — Build succeeded, warnings found
- `2` — One or more build targets failed to compile

A transcript of each run is written under `logs/code-diagnostics/<yyyyMMdd-HHmmss>.log`.

## Common Warning Categories

| Category | Code Range | Examples |
|----------|-----------|----------|
| Nullability | CS8600-CS8655 | CS8604 (null arg), CS8619 (nullability mismatch) |
| Obsolete | CS0612, CS0618 | Deprecated API usage |
| Unused | CS0168, CS0219, CS8321 | Unused variables, local functions |
| Async | CS1998, CS4014 | Missing await, async without await |

## How It Differs From Other Tools

| Tool | What It Does | External? |
|------|-------------|-----------|
| **Roslyn (this)** | Compiler-native warnings from `dotnet build` | No |
| **SonarQube** | External static analysis + dashboard | Yes (Docker) |
| **Qodana** | JetBrains inspections + SARIF reports | Yes (Docker) |
| **Audit CLI** | Custom Roslyn workspace AST analysis | No (custom tool) |
