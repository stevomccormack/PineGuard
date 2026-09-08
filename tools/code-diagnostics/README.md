# Code Diagnostics (Roslyn Compiler Warnings)

Captures and reports Roslyn compiler warnings (CS-prefixed codes) from `dotnet build`.

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
| `-Scope` | ValidateSet | `All` | `All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Options`, `Testing` |
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
  "TotalWarnings": 2,
  "ByCode": [{ "Code": "CS8604", "Count": 1 }, { "Code": "CS8619", "Count": 1 }],
  "ByFile": [{ "File": "...", "Count": 2 }],
  "Warnings": [{ "File": "...", "Line": 56, "Column": 31, "Code": "CS8604", "Message": "..." }]
}
```

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
