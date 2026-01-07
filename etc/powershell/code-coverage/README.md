# Code Coverage (PowerShell)

This folder contains the repository’s standard scripts for collecting coverage locally and for analyzing “what to fix next”.

For the repo’s coverage playbook / agent instructions, see:

- `docs/ai/code-coverage-agent-spec.md`

The workflow is:

1. generate coverage (Cobertura XML + HTML)
2. analyze filtered coverage (PineGuard.Core scope) and pick the lowest-covered targets
3. add deterministic tests / remove unreachable branches
4. repeat

## Prerequisites

- PowerShell 7+ (`pwsh`). (Windows PowerShell 5.1 may work for some commands, but the scripts are written/tested with `pwsh`.)
- .NET SDK (repo targets .NET 8+).

The HTML report uses ReportGenerator via `dotnet-reportgenerator-globaltool` installed as a repo-local tool under `.dotnet/tools` (ignored by git).

## Scripts

### GenerateCodeCoverageReport.ps1

Generates fresh coverage output by:

- discovering runnable unit test projects under `tests/**/*.UnitTests.csproj`
- running `dotnet test` with `--collect:"XPlat Code Coverage"`
- using `etc/powershell/code-coverage/coverlet.runsettings`
- producing HTML under `etc/generated/code-coverage/html`

Run from repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1"
```

Common variants:

```powershell
# Debug (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1" -Configuration Debug

# Release
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1" -Configuration Release

# Clean the generated output folder first
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1" -Clean

# Clean + Debug
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1" -Configuration Debug -Clean
```

Notes:

- The script skips `*.UnitTests.csproj` projects that contain no `*.cs` files (outside `bin/`/`obj/`) to avoid misleading “No test is available” runs.
- Coverage collection is occasionally intermittent (empty/invalid Cobertura output). The script detects that and automatically retries once.

### AnalyzeCodeCoverage.ps1

Reads the newest Cobertura XML file per test project under `etc/generated/code-coverage/testresults/**/coverage.cobertura.xml`, filters coverage to a scope (by default `src/PineGuard.Core/**`), and prints:

- filtered line + branch totals
- lowest-covered classes list

Run from repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1"
```

Common variants:

```powershell
# Show more/less rows
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -Top 10
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -Top 50

# Open the HTML report after printing the summary
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -OpenHtml

# Fail the command if the filtered scope is not 100% line+branch
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -Enforce100

# Print a formatted table (may truncate depending on console width)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -AsTable
```

Changing the filtered scope (matches Cobertura `class filename` values):

```powershell
# Include only PineGuard.Core (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" `
  -IncludeFileRegex '^src[\\/]+PineGuard\.Core[\\/]+'

# Exclude build artifacts (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" `
  -ExcludeFileRegex '^src[\\/]+PineGuard\.Core[\\/]obj[\\/]+'

# Example: analyze a different project folder under src
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" `
  -IncludeFileRegex '^src[\\/]+PineGuard\.GuardClauses[\\/]+'
```

## Outputs

After running GenerateCodeCoverageReport.ps1:

- HTML report:
  - `etc/generated/code-coverage/html/index.html`
  - `etc/generated/code-coverage/html/summary.html`
- Raw test results + Cobertura XML (per test project run):
  - `etc/generated/code-coverage/testresults/<ProjectName>/<RunId>/coverage.cobertura.xml`

Notes about the structure:

- `<ProjectName>` comes from the test project file name (e.g., `PineGuard.Core.UnitTests`).
- `<RunId>` is a GUID created by `dotnet test` for the run.
- The scripts always pick the newest Cobertura file per test project folder.

## coverlet.runsettings (what gets measured)

Coverage collection is configured by:

- `etc/powershell/code-coverage/coverlet.runsettings`

Key settings:

- `<Include>` controls which assemblies are included in collection. Today it’s intentionally set to the main library to avoid collector regressions that only report helper/test assemblies.
- `<ExcludeByFile>` excludes build artifacts and generated sources (e.g., `**/obj/**`, `**/bin/**`).
- `<ExcludeByAttribute>` excludes compiler/source-generated code (including `GeneratedRegex` output) so the report stays stable.

### When you need coverage for other projects

If you want to collect coverage for additional assemblies (for example `PineGuard.GuardClauses`, `PineGuard.MustClauses`, etc.), you typically change `<Include>` in `coverlet.runsettings`.

Examples:

```xml
<!-- Only PineGuard.Core (default) -->
<Include>[PineGuard.Core]*</Include>

<!-- Multiple assemblies -->
<Include>[PineGuard.Core]*;[PineGuard.GuardClauses]*;[PineGuard.MustClauses]*</Include>

<!-- Everything that starts with PineGuard. (use cautiously; will pull in more generated/build artifacts) -->
<Include>[PineGuard.*]*</Include>
```

After changing `<Include>`, regenerate coverage and re-run the analyzer (and consider also updating `-IncludeFileRegex` to match the source folder you care about).

## Troubleshooting

### “Coverage output looked invalid … Retrying once…”

This is expected occasionally. The generator script validates the Cobertura file and retries once when it detects an empty/invalid output.

### ReportGenerator warnings about missing RegexGenerator.g.cs

If you see warnings about missing `RegexGenerator.g.cs`, that’s typically source-generated code paths moving between builds. The scripts exclude `obj/bin` and exclude generated code by attribute to keep reports stable.

### “No test is available” warnings

Some test projects are intentionally empty placeholders. The generator script skips test projects that contain no real `*.cs` sources.

## Typical workflow (copy/paste)

```powershell
# 1) generate coverage
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1" -Configuration Debug

# 2) analyze and pick targets
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -Top 30

# 3) (after adding tests) repeat
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/GenerateCodeCoverageReport.ps1" -Configuration Debug
pwsh -NoProfile -ExecutionPolicy Bypass -File "./etc/powershell/code-coverage/AnalyzeCodeCoverage.ps1" -Top 30
```
