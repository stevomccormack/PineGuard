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
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope All -Filter "CS86"

# JSON output
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-diagnostics/Run-CompilerDiagnostics.ps1" -Scope All -OutputFormat Json
```

## Parameters

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `-Scope` | ValidateSet | `All` | `All`, `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations`, `Options`, `Testing` |
| `-Filter` | string | _(none)_ | Regex pattern to filter warning codes (e.g. `CS86`, `CS0618`) |
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

- `0` — No warnings found
- `1` — Warnings detected

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
