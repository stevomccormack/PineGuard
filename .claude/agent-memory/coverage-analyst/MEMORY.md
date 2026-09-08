# Coverage Analyst Memory

> **Role:** `docs/ai/roles/planner.md` (Planner)
> Directives: Risk-based Testing, Test Data Design, Clarity, Coverage with Intent.
> Constraints: No implementation-detail tests. No time/environment/network dependent tests.

## Learned Patterns

### Coverage Tool Usage
- Default engine: Coverlet (`-Engine Coverlet`, the default of `-Engine Coverlet|DotCover` on `Run-CodeCoverage.ps1`)
- Command: `pwsh -NoProfile -ExecutionPolicy Bypass -Command "cd '...'; ./tools/code-coverage/Run-CodeCoverage.ps1 -Mode GenerateAndAnalyze -Scope [ProjectName] -Top 30 -SkipHtml -Format cobertura"`
- Note: `-Engine` parameter DOES exist on `Run-CodeCoverage.ps1` (`Coverlet` | `DotCover`, default `Coverlet`), forwarded through the D-9 front door (`New-CoverageReport.ps1`) and to the gate (`Test-Coverage.ps1`). `-Engine DotCover` is snapshot-only and cannot be gated — do not remove a stored `-Engine` flag.
- dotCover: blocker resolved (Mar 2026) by adding Webroot AV exclusions — 2025.3.3 verified on net8.0 and net10.0, as is Coverlet. The dotCover wrapper ships at `tools/code-coverage/dotcover/New-CoverageReport.ps1`, but it is snapshot-only (writes a `.dcvr` file into `artifacts/code-coverage/dotcover/<scope>/snapshots/`, opened in Rider's coverage viewer; never gated). Coverlet remains the sole CI/gate-authority engine — use it for any result that needs to be gated.
- Valid scopes: Core, MustClauses, GuardClauses, FluentValidation, DataAnnotations, Options, DependencyInjection, AspNetCore, ErrorOr, FluentResults, OneOf, MediatR, Analyzers, Testing, All
- Reports land in: `artifacts/code-coverage/coverlet/`

### Common Gap Patterns
- **Null check branches**: Method accepts `string?` but tests only pass non-null
- **Config param null checks**: `if (length < 0)` branch untested
- **Guard return paths**: Guard returns typed value on success — test the return value
- **Partial branches**: `&&` / `||` conditions where only one side tested
- **Edge values**: `int.MinValue`, `int.MaxValue`, empty collections, single-element
- **String edge cases**: Empty string `""`, whitespace `" "`, very long strings

### Analysis Strategy
1. Run scope-specific coverage (not All — isolates failures)
2. Focus on classes < 100% first
3. Check branch coverage separately from line coverage (can have 100% line but < 100% branch)
4. Look for yellow diamonds in HTML report = partial branches

### Known Limitations
- FluentValidation scope compiled and ran cleanly as of 2026-03-17 — previously noted compile errors are resolved
- `-SkipHtml` flag avoids HTML generation overhead during iterative analysis
- `-Isolated` (dotnet publish mode) fails for multi-targeting projects (Core, MustClauses, GuardClauses all confirmed) — always drop `-Isolated`; non-isolated run is equally authoritative

## Projects Analyzed
- **MustClauses** (2026-03-22): 100% line (3712/3712), 100% branch (1316/1316) across all 66 classes. 3142 tests passed, 0 failures (net8.0 + net10.0).
- **GuardClauses** (2026-03-22): 100% line (3569/3569), 100% branch (2344/2344) across all 66 classes. 2720 tests passed, 0 failures (net8.0 + net10.0).
- **Core** (2026-03-22): 100% line (16188/16188), 100% branch (3217/3217). 5342 tests passed, 0 failures (net8.0 + net10.0). 201 classes in filtered scope.
- **Testing** (2026-03-19): 100% line (5444/5444), 100% branch (88/88). 613 tests passed, 0 failures.
- **DataAnnotations** (2026-03-22): 100% line (2300/2300), 100% branch (182/182). 1685 tests passed, 0 failures (net8.0 + net10.0).
- **FluentValidation** (2026-03-22): 100% line (1540/1540), 100% branch (1036/1036) across all 57 classes. 3249 tests passed, 0 failures. Requires `-Framework net10.0` to avoid net8.0 DLL lock when another process holds the file.


### Analyzer "Lowest-covered" List Artifact Warning
- `tools/code-coverage/Test-Coverage.ps1`'s "Lowest-covered classes" table may include classes that are actually 100% in the raw Cobertura XML. This happens when the analyzer merges multiple XML runs. Always verify by reading the raw Cobertura XML before reporting gaps.

### Inverted Guard Return Value Assertion Bug (GuardClauses)
- **Resolved (2026-03-17)**: All GuardClauses tests pass — 0 failures.

## Topic Files
- (none yet — will grow organically)
