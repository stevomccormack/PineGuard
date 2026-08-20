# Code Coverage (PowerShell)

This folder contains the repository's standard scripts for collecting coverage locally and for analyzing "what to fix next".

For the repo's coverage playbook / agent instructions, see:

- `docs/ai/specs/testing/coverage.md`

The workflow is:

1. generate coverage (Cobertura XML + HTML)
2. analyze filtered coverage (e.g. `Core`, `MustClauses`) and pick the lowest-covered targets
3. add deterministic tests / remove unreachable branches
4. repeat

**PineGuard.Testing** (`tests/PineGuard.Testing/`) is the shared test-infrastructure library and a shipped package. It has its own runner, `tests/PineGuard.Testing.UnitTests` — use `-Scope Testing` to generate and analyze its coverage like any other project.

## Engines

These scripts wrap exactly one collector: coverlet's `XPlat Code Coverage`, which is why every generator and
analyzer lives under `xplat/`. It collects on both test TFMs (`net8.0`, `net10.0`).

There is deliberately no `dotcover/` folder here — JetBrains dotCover is driven from its own tooling, not from
this directory, so its absence is not a missing file.

## Prerequisites

- PowerShell 7+ (`pwsh`). (Windows PowerShell 5.1 may work for some commands, but the scripts are written/tested with `pwsh`.)
- .NET 10 SDK — see [tools/README.md](../README.md#prerequisites).

The HTML report uses ReportGenerator via `dotnet-reportgenerator-globaltool` installed as a repo-local tool under `.dotnet/tools` (ignored by git).

## Scripts

### Run-CodeCoverage.ps1

Single entry-point for local usage.

```powershell
# Generate + analyze Core
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope Core -Enforce100

# Generate + analyze MustClauses
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope MustClauses -Top 30

# Override which test projects run
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope Core -ProjectFilter "*.UnitTests.csproj"

# Generate + analyze PineGuard.Testing
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope Testing -Enforce100
```

For ad-hoc slicing that no preset covers, generate with the widest scope you need and then re-filter the
existing Cobertura files with `Custom`:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Custom -IncludeClassNameRegex "^PineGuard\.Testing\." -Top 30 -Enforce100
```

### xplat/Gen-CoverageReport.ps1

Generates fresh coverage output by:

- discovering runnable unit test projects under `tests/**/*.UnitTests.csproj` (or narrowed to a single project for `-Scope Core|MustClauses|GuardClauses|DataAnnotations|FluentValidation|Testing` for speed)
- running `dotnet test` with `--collect:"XPlat Code Coverage"`
- generating a scope-specific runsettings file under `artifacts/code-coverage/xplat/coverlet.<Scope>.runsettings`
- producing HTML under `artifacts/code-coverage/xplat/html`

Run from repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1"
```

Common variants:

```powershell
# Debug (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Configuration Debug

# Release
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Configuration Release

# Clean the generated output folder first
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Clean

# CI / non-interactive (don't try to open the browser)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Configuration Release -Clean -NoOpen

# Scope to MustClauses
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Scope MustClauses

# Override which test projects run
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Scope Core -ProjectFilter "*.UnitTests.csproj"

# Fast path: collect Cobertura XML only (skip HTML generation)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Scope Core -SkipHtml

# Clean + Debug
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Configuration Debug -Clean

# Run in isolated mode (separate test results per run)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Scope All -Isolated

# Filter tests (dotnet test --filter expression)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Scope Core -Filter "FullyQualifiedName~SomeTests"

# Use OpenCover format instead of Cobertura (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Scope Core -Format opencover
```

Notes:

- The script skips `*.UnitTests.csproj` projects that contain no `*.cs` files (outside `bin/`/`obj/`) to avoid misleading "No test is available" runs.
- Coverage collection is occasionally intermittent (empty/invalid Cobertura output). The script detects that and automatically retries once.

### xplat/Test-CoverageAnalysis.ps1

Reads the newest Cobertura XML file per test project under `artifacts/code-coverage/xplat/testresults/**/coverage.cobertura.xml`, filters coverage to a scope (by default `Core`), and prints:

- filtered line + branch totals
- lowest-covered classes list

Run from repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1"
```

Common variants:

```powershell
# Show more/less rows
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Top 10
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Top 50

# Open the HTML report after printing the summary
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -OpenHtml

# Fail the command if the filtered scope is not 100% line+branch
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Enforce100

# Threshold gates (accept 0..1 or 0..100)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Core -FailCoverageBelow 95 -FailBranchBelow 95

# Preset scoping
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope MustClauses
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope GuardClauses
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Testing
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope All

# Print a formatted table (may truncate depending on console width)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -AsTable
```

Changing the filtered scope (matches Cobertura `class filename` values):

```powershell
# Include only PineGuard.Core (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" `
  -IncludeFileRegex '^src[\\/]+PineGuard\.Core[\\/]+'


# Exclude build artifacts (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" `
  -ExcludeFileRegex '^src[\\/]+PineGuard\.Core[\\/]obj[\\/]+'


# Example: analyze a different project folder under src
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" `
  -IncludeFileRegex '^src[\\/]+PineGuard\.GuardClauses[\\/]+'


```

## Outputs

- HTML report:
  - `artifacts/code-coverage/xplat/html/index.html`
  - `artifacts/code-coverage/xplat/html/summary.html`
- Stable redirect (always points at the latest HTML):
  - `artifacts/code-coverage/xplat-report.html`
- Raw test results + Cobertura XML (per test project run):
  - `artifacts/code-coverage/xplat/testresults/<ProjectName>/<RunId>/coverage.cobertura.xml`

Notes about the structure:

- `<ProjectName>` comes from the test project file name (e.g., `PineGuard.Core.UnitTests`).
- `<RunId>` is a GUID created by `dotnet test` for the run.
- The scripts always pick the newest Cobertura file per test project folder.

## coverlet.runsettings (what gets measured)

Coverage collection is configured by a generated runsettings file that is created per run.

The generator writes:

- `artifacts/code-coverage/xplat/coverlet.<Scope>.runsettings`

The template lives at:

- `tools/code-coverage/coverlet.runsettings`

Key settings:

- `<Include>` controls which assemblies are included in collection. Today it's intentionally set to the main library to avoid collector regressions that only report helper/test assemblies.
- `<ExcludeByFile>` excludes build artifacts and generated sources (e.g., `**/obj/**`, `**/bin/**`).
- `<ExcludeByAttribute>` excludes compiler/source-generated code (including `GeneratedRegex` output) so the report stays stable.

### When you need coverage for other projects

Use the `-Scope` parameter on the xplat generator to change which assemblies are included.

Examples:

```xml
<!-- Only PineGuard.Core (default) -->
<Include>[PineGuard.Core]*</Include>

<!-- Multiple assemblies -->
<Include>[PineGuard.Core]*;[PineGuard.GuardClauses]*;[PineGuard.MustClauses]*</Include>

<!-- Everything that starts with PineGuard. (use cautiously; will pull in more generated/build artifacts) -->
<Include>[PineGuard.*]*</Include>
```

After changing scope, regenerate coverage and re-run the analyzer (and consider also updating `-IncludeFileRegex` if you're using `-Scope Custom`).

## Troubleshooting

### "Coverage output looked invalid … Retrying once…"

This is expected occasionally. The generator script validates the Cobertura file and retries once when it detects an empty/invalid output.

### ReportGenerator warnings about missing RegexGenerator.g.cs

If you see warnings about missing `RegexGenerator.g.cs`, that's typically source-generated code paths moving between builds. The scripts exclude `obj/bin` and exclude generated code by attribute to keep reports stable.

### "No test is available" warnings

Some test projects are intentionally empty placeholders. The generator script skips test projects that contain no real `*.cs` sources.

## Typical workflow (copy/paste)

```powershell
# 1) generate coverage
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Configuration Debug -Scope Core

# 2) analyze and pick targets
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Core -Top 30

# 3) (after adding tests) repeat
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Gen-CoverageReport.ps1" -Configuration Debug -Scope Core
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/xplat/Test-CoverageAnalysis.ps1" -Scope Core -Top 30
```
