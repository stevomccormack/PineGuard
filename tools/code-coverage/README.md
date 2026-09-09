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

Coverage collection is engine-pluggable behind a delegating front door (`New-CoverageReport.ps1
-Engine`):

| Engine | Folder | Status |
|---|---|---|
| Coverlet | `coverlet/` | Authoritative. CI and the 100% gate use it. Collects via `dotnet test --collect:"XPlat Code Coverage"` on both test TFMs (`net8.0`, `net10.0`), rendered to HTML + Cobertura by ReportGenerator. |
| JetBrains dotCover | `dotcover/` | Local, snapshot-only. Collects a Rider-native `.dcvr` snapshot via `dotCover cover --snapshot-output` on both test TFMs. Not gated, not converted to HTML/Cobertura — see below for why. |

The engine folder is named after the tool, never after a collector's display string:
`coverlet.collector` registers the data collector `friendlyName` "XPlat Code Coverage" — that is
where the old `xplat/` folder name came from, but the tool is Coverlet.

Both engines share one parameter contract (`coverlet/New-CoverageReport.ps1` and
`dotcover/New-CoverageReport.ps1`) and one output root:
`artifacts/code-coverage/<engine>/<scope>/`. The front door (`New-CoverageReport.ps1`) validates
`-Engine` and delegates only — it holds no collection logic of its own. `Test-Coverage.ps1` gates
on Cobertura output, so it only works for `-Engine Coverlet`; `-Engine DotCover` throws (see
below).

### Why dotCover is snapshot-only, not full-featured

The plan originally scoped a full-featured dotCover engine: XML collection via
`--xml-report-output`, rendered to Html/HtmlSummary/Cobertura by ReportGenerator, gateable by
`Test-Coverage.ps1 -Engine DotCover` exactly like Coverlet. Spike T3.10
(`docs/ai/plans/tools-review-and-standardisation.md` `## Baselines`, "T3.10 — dotCover spike")
found that path genuinely broken in dotCover 2025.3.3 on **both** `net8.0` and `net10.0`: `cover
--xml-report-output` either hangs indefinitely against a persistent `VBCSCompiler` (Roslyn
compiler-server) process, or — once that hang is avoided — throws `Unhandled exception: Snapshot
container is not initialized` from `ReportBuilder.BuildReports` and produces no XML at all. This
was reproduced four times, including a `--no-build` run that rules out the build step as the
cause; it is a real, documented upstream JetBrains bug, not a local misconfiguration.

What T3.10 proved DOES work reliably on both TFMs is `dotCover cover --snapshot-output
<path>.dcvr` (omitting `--xml-report-output` entirely) — a plain coverage snapshot in JetBrains
Rider's own native format. `dotcover/New-CoverageReport.ps1` (T3.11) implements exactly that: it
collects `.dcvr` snapshot(s) under `artifacts/code-coverage/dotcover/<scope>/snapshots/` and
prints where they landed, but never invokes ReportGenerator and never produces HTML or Cobertura
— there is no dotCover XML to feed it. Open the `.dcvr` file(s) directly in Rider's coverage
viewer for visual, file-by-file exploration. `Test-Coverage.ps1 -Engine DotCover` throws a clear,
accurate error explaining this rather than pretending to gate on numbers that do not exist;
Coverlet (§3.5: "Coverlet is authoritative … dotCover is the local second opinion, reported not
gated") remains the only engine this repo's automated 100% gate can enforce against.

## Prerequisites

- PowerShell 7+ (`pwsh`). (Windows PowerShell 5.1 may work for some commands, but the scripts are written/tested with `pwsh`.)
- .NET 10 SDK — see [tools/README.md](../README.md#prerequisites).

The HTML report uses ReportGenerator via `dotnet-reportgenerator-globaltool`, pinned to `5.4.18` in
`.config/dotnet-tools.json` and restored with `dotnet tool restore` (a local tool manifest, not a
global install); the scripts invoke it as `dotnet reportgenerator`.

## Scripts

### Run-CodeCoverage.ps1

Single entry-point for local usage. Defaults to `-Engine Coverlet`.

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
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope Custom -IncludeClassNameRegex "^PineGuard\.Testing\." -Top 30 -Enforce100
```

### New-CoverageReport.ps1 (D-9 front door)

Validates `-Engine Coverlet|DotCover` and delegates to the matching engine folder's own
`New-CoverageReport.ps1`, forwarding every other parameter unchanged. Calling it directly with
`-Engine Coverlet` behaves identically to calling `coverlet/New-CoverageReport.ps1` directly;
`-Engine DotCover` behaves identically to calling `dotcover/New-CoverageReport.ps1` directly (the
snapshot-only fallback — see "Why dotCover is snapshot-only" above).

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/New-CoverageReport.ps1" -Engine Coverlet -Scope Core -NoOpen
```

### coverlet/New-CoverageReport.ps1

Generates fresh coverage output by:

- discovering runnable unit test projects under `tests/**/*.UnitTests.csproj` (or narrowed to that scope's own default test project, for speed, whenever `-Scope` names a single registry scope rather than `All`)
- running `dotnet test` with `--collect:"XPlat Code Coverage"`
- generating a scope-specific runsettings file under `artifacts/code-coverage/coverlet/<scope>/coverlet.runsettings`
- producing HTML (plus a merged Cobertura.xml) under `artifacts/code-coverage/coverlet/<scope>/report/`

Run from repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1"
```

Common variants:

```powershell
# Debug (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Configuration Debug

# Release
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Configuration Release

# Clean the generated output folder first
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Clean

# CI / non-interactive (don't try to open the browser)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Configuration Release -Clean -NoOpen

# Scope to MustClauses
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Scope MustClauses

# Override which test projects run
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Scope Core -ProjectFilter "*.UnitTests.csproj"

# Fast path: collect Cobertura XML only (skip HTML generation)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Scope Core -SkipHtml

# Clean + Debug
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Configuration Debug -Clean

# Run in isolated mode (separate test results per run)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Scope All -Isolated

# Filter tests (dotnet test --filter expression)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Scope Core -Filter "FullyQualifiedName~SomeTests"

# Use OpenCover format instead of Cobertura (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Scope Core -Format opencover
```

Notes:

- The script skips `*.UnitTests.csproj` projects that contain no `*.cs` files (outside `bin/`/`obj/`) to avoid misleading "No test is available" runs.
- Coverage collection is occasionally intermittent (empty/invalid Cobertura output). The script detects that and automatically retries once.
- When not `-NoOpen`, the browser opens `report/index.html` directly — there is no intermediate redirect page.

### Test-Coverage.ps1

Reads the newest Cobertura XML file per test project under `artifacts/code-coverage/coverlet/<scope>/testresults/**/coverage.cobertura.xml`, filters coverage to a scope (by default `Core`), and prints:

- filtered line + branch totals
- lowest-covered classes list

Accepts `-Engine Coverlet|DotCover` (default `Coverlet`), but only `Coverlet` actually gates:
`-Engine DotCover` throws a clear "not supported" error, because dotCover's snapshot-only fallback
(T3.10/T3.11 — see "Why dotCover is snapshot-only" above) produces no Cobertura output for this
script to read. Coverlet remains the only engine this repo's automated 100% gate can enforce
against.

Run from repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1"
```

Common variants:

```powershell
# Show more/less rows
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Top 10
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Top 50

# Open the HTML report after printing the summary
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -OpenHtml

# Fail the command if the filtered scope is not 100% line+branch
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Enforce100

# Threshold gates (accept 0..1 or 0..100)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope Core -FailCoverageBelow 95 -FailBranchBelow 95

# Preset scoping
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope MustClauses
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope GuardClauses
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope Testing
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope All

# Print a formatted table (may truncate depending on console width)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -AsTable
```

Changing the filtered scope (matches Cobertura `class filename` values):

```powershell
# Include only PineGuard.Core (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" `
  -IncludeFileRegex '^src[\\/]+PineGuard\.Core[\\/]+'


# Exclude build artifacts (default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" `
  -ExcludeFileRegex '^src[\\/]+PineGuard\.Core[\\/]obj[\\/]+'


# Example: analyze a different project folder under src
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" `
  -IncludeFileRegex '^src[\\/]+PineGuard\.GuardClauses[\\/]+'


```

### dotcover/New-CoverageReport.ps1

Collects a JetBrains dotCover snapshot only — see "Why dotCover is snapshot-only" above for the
T3.10 finding this implements. Same parameter contract as `coverlet/New-CoverageReport.ps1`
(`-Scope`, `-Configuration`, `-Clean`, `-Framework`, `-ProjectFilter`, `-Filter`, `-Isolated`);
`-NoOpen`, `-SkipHtml`, `-Format` are accepted for contract parity with Coverlet but have no
effect (there is no HTML/Cobertura step here to open, skip, or format — each prints a warning
when passed explicitly).

Run from repo root:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/dotcover/New-CoverageReport.ps1"

# Collect only one TFM (faster than the both-TFM default)
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/dotcover/New-CoverageReport.ps1" -Scope Core -Framework net8.0

# Clean this scope's previous snapshot(s) first
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/dotcover/New-CoverageReport.ps1" -Scope Core -Clean
```

After collection, open the printed `.dcvr` file(s) directly in Rider's coverage viewer.

## Outputs

- Coverlet (`coverlet/New-CoverageReport.ps1`):
  - HTML report:
    - `artifacts/code-coverage/coverlet/<scope>/report/index.html`
    - `artifacts/code-coverage/coverlet/<scope>/report/summary.html`
    - `artifacts/code-coverage/coverlet/<scope>/report/Cobertura.xml` (merged, per-scope)
  - Raw test results + Cobertura XML (per test project run):
    - `artifacts/code-coverage/coverlet/<scope>/testresults/<ProjectName>/<RunId>/coverage.cobertura.xml`
- dotCover (`dotcover/New-CoverageReport.ps1`):
  - Raw snapshot(s) only (T3.10: no HTML/Cobertura is ever produced):
    - `artifacts/code-coverage/dotcover/<scope>/snapshots/<ProjectName>.<tfm>.dcvr`

Notes about the structure:

- `<scope>` is the lower-cased `-Scope` value (e.g. `core`, `mustclauses`, `all`) — each scope gets its own physically separate folder, so cleaning or reading one scope's output can never touch another's (F-19).
- `<ProjectName>` comes from the test project file name (e.g., `PineGuard.Core.UnitTests`).
- `<RunId>` is a GUID created by `dotnet test` for the run.
- The scripts always pick the newest Cobertura file per test project folder.

## coverlet.runsettings (what gets measured)

Coverage collection is configured by a generated runsettings file that is created per run, and by
CI's own step directly. Both derive from a single static template (F-35) — there used to be two
independently-typed copies (the file below, and an inline heredoc in `Write-CoverletRunSettings`),
which could drift; the generator now reads the same file CI uses and patches only `<Include>` (and
`<Format>`, if a non-default format is requested) onto a copy of it.

The generator writes:

- `artifacts/code-coverage/coverlet/<scope>/coverlet.runsettings`

The template — and what CI's own coverage step passes to `dotnet test --settings` directly — lives at:

- `tools/code-coverage/coverlet.runsettings`

Key settings:

- `<Include>` controls which assemblies are included in collection. The generator overwrites this per scope; the static file's own value (`[PineGuard.*]*`) is what CI uses.
- `<ExcludeByFile>` excludes build artifacts and generated sources (e.g., `**/obj/**`, `**/bin/**`).
- `<ExcludeByAttribute>` excludes compiler/source-generated code (including `GeneratedRegex` output) so the report stays stable.
- `<Format>` is absent from the static file (Coverlet's "XPlat Code Coverage" collector defaults to Cobertura); the generator inserts it only when `-Format opencover` is requested.

### When you need coverage for other projects

Use the `-Scope` parameter on the Coverlet generator to change which assemblies are included.

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
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Configuration Debug -Scope Core

# 2) analyze and pick targets
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope Core -Top 30

# 3) (after adding tests) repeat
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/coverlet/New-CoverageReport.ps1" -Configuration Debug -Scope Core
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Test-Coverage.ps1" -Scope Core -Top 30
```
