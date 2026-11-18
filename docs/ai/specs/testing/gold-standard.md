---
spec:
  id: pineguard.ai.specs.testing.gold-standard
  title: "GOLD-STANDARD Compliance Index"
  version: 3
  parent:
    - unit-test.md
applies_to:
  - "tests/**"
---

# GOLD-STANDARD Compliance Index

Tracks compliance of every test project against the GOLD-STANDARD testing pattern defined in `unit-test.md`.

## Status Legend

| Status | Meaning |
|--------|---------|
| **GOLD** | Only populated datasets exist (no empty scaffolding), 100% line+branch coverage confirmed |
| **SILVER** | Structure correct, no empty scaffolding, coverage not yet verified |
| **BRONZE** | Structure correct, some empty scaffolding remains |
| **SCAFFOLD** | Incomplete test data |

## Compliance Criteria

A test operation group reaches **GOLD** when:

1. Only datasets with actual test cases are present (§4.1 of unit-test spec)
2. No empty arrays (`=> [];`) exist — omit datasets that have no cases
3. EdgeCases reference Core constants/statics where applicable (boundary values, min/max, enum ranges)
4. Fixtures are the source of truth for cross-layer validation inputs (§9)
5. 100% line and branch coverage confirmed via Coverlet, enforced by `tools/code-coverage/Test-Coverage.ps1` with `-Enforce100`

## Project Summary

| Project | TestData Files | Op Groups | Fixture-Based Files | Status |
|---------|---------------|-----------|--------------------|--------|
| PineGuard.Core.UnitTests | 115 | 702 | 59 (51%) | GOLD |
| PineGuard.MustClauses.UnitTests | 59 | 641 | 48 (81%) | GOLD |
| PineGuard.GuardClauses.UnitTests | 58 | 601 | 49 (84%) | GOLD |
| PineGuard.FluentValidation.UnitTests | 67 | 805 | 49 (73%) | GOLD |
| PineGuard.DataAnnotations.UnitTests | 64 | 493 | 34 (53%) | GOLD |
| PineGuard.Testing.UnitTests | 20 | 82 | 0 (n/a) | GOLD |
| PineGuard.Testing.UnitTests.Rules | 1 | 1 | 1 (100%) | GOLD |
| PineGuard.Extensions.Options.UnitTests | 2 | 7 | 0 (n/a) | GOLD |
| PineGuard.Extensions.DependencyInjection.UnitTests | 2 | 8 | 0 (n/a) | GOLD |
| PineGuard.AspNetCore.UnitTests | 14 | 30 | 0 (n/a) | GOLD |
| PineGuard.MediatR.UnitTests | 2 | 2 | 0 (n/a) | GOLD |
| PineGuard.ErrorOr.UnitTests | 1 | 4 | 0 (n/a) | GOLD |
| PineGuard.FluentResults.UnitTests | 2 | 6 | 0 (n/a) | GOLD |
| PineGuard.OneOf.UnitTests | 1 | 2 | 0 (n/a) | GOLD |
| PineGuard.Xml.UnitTests | 8 | 25 | 0 (n/a) | GOLD |
| PineGuard.Analyzers.UnitTests | 3 | 12 | 0 (n/a) | GOLD |

**Counting method** (rerun these to reproduce the table):

- **TestData Files** — files matching `*TestData*.cs` under `tests/<Project>/`, excluding `bin/` and `obj/`.
- **Op Groups** — lines in those files matching `^\s+public static (partial )?class \w+`, i.e. nested Operation Groups, excluding the outer `XxxTestData` class.
- **Fixture-Based Files** — TestData files containing at least one call to the project's scenario extension: `.ToRuleCases(` for Core and Testing, `.ToMustCases(`, `.ToGuardCases(`, `.ToFluentCases(`, `.ToDataAnnotationCases(` for the other layers, and the project-local `.ToConformanceCases(` for PineGuard.Testing.UnitTests.Rules. The nine adapter and tooling projects (Options to Analyzers) take no cross-layer validation inputs and use no scenario extension, so their column reads `0 (n/a)`, as it does for Testing.

**All projects are GOLD** — structure is correct, no `=> [];` scaffolding remains in any TestData file, and coverage is verified at the 100% target (see below). PineGuard.Testing.UnitTests.Rules meets criteria 1–4; criterion 5 does not apply to it, because it owns no source assembly and so has no coverage scope of its own. Locally its results feed only the `All` and `Testing` scopes, whose project filter is the broad ordinary-test family (`Get-DefaultTestProjectFilter` in `tools/.shared/dotnet-projects.ps1`); every other scope's `DefaultProjectFilter` is that scope's own test project. In CI its results merge into the coverage report only for the assemblies whose own matrix entry ran.

## Coverage Verification (2026-11-18)

`tools/code-coverage/Test-Coverage.ps1 -Enforce100` exits 0 for `All` and for every individual
scope, against a full `tools/code-coverage/New-CoverageReport.ps1 -Scope All -Configuration Release`
run covering net8.0, net10.0 and net11.0 (71,158 passing executions: 23,700 on net8.0, 23,725 on
net10.0 and 23,733 on net11.0; 0 failed, 0 skipped), on commit `55f050e` plus the documentation-only
commit `2c3cd92`:

| Scope | Line | Branch |
|-------|------|--------|
| All | 100.00% (20678/20678) | 100.00% (8134/8134) |
| Core | 100.00% (3783/3783) | 100.00% (3162/3162) |
| MustClauses | 100.00% (3067/3067) | 100.00% (1986/1986) |
| GuardClauses | 100.00% (2359/2359) | 100.00% (1192/1192) |
| DataAnnotations | 100.00% (2285/2285) | 100.00% (824/824) |
| FluentValidation | 100.00% (1748/1748) | 100.00% (514/514) |
| Testing | 100.00% (6613/6613) | 100.00% (120/120) |
| Options | 100.00% (30/30) | 100.00% (8/8) |
| DependencyInjection | 100.00% (35/35) | 100.00% (24/24) |
| AspNetCore | 100.00% (157/157) | 100.00% (70/70) |
| ErrorOr | 100.00% (15/15) | 100.00% (4/4) |
| FluentResults | 100.00% (26/26) | 100.00% (6/6) |
| OneOf | 100.00% (8/8) | 100.00% (4/4) |
| MediatR | 100.00% (25/25) | 100.00% (14/14) |
| Xml | 100.00% (145/145) | 100.00% (62/62) |
| Analyzers | 100.00% (382/382) | 100.00% (144/144) |

Per-scope figures apply their own class filters and are not expected to sum to the `All` row.
`Test-Coverage.ps1` reads the newest Cobertura file per test project, so each figure comes from one
target framework per project and can differ by a few framework-specific lines between runs; CI
merges all three frameworks and enforces the same 100% target on the merged report. Every scope
was read from the one `-Scope All` run through `-ResultsRoot`. No `[ExcludeFromCodeCoverage]`
attributes exist in `src/`.

Where a branch was genuinely unreachable through the public API, the enclosing private
helper was widened to `internal` and tested directly rather than deleted or suppressed
— see `NetworkUtility.IsValidIpv4Segment`, `NetworkUtility.IsValidHostnameLabel`,
`CultureInfoUtility.IsIsoAlpha2RegionCode`, and the `StringUtility.NumberTypes` span
helpers.

## Fixture-Based Testing Adoption

TestData files using the `RuleScenario<T>` → `.ToXxxCases()` fixture pattern (counted per the method above):

| Project | Fixture-Based Files | TestData Files | Adoption |
|---------|--------------------|----------------|----------|
| Testing.UnitTests.Rules | 1 | 1 | 100% |
| GuardClauses | 49 | 58 | 84% |
| MustClauses | 48 | 59 | 81% |
| FluentValidation | 49 | 67 | 73% |
| DataAnnotations | 34 | 64 | 53% |
| Core | 59 | 115 | 51% |
| Testing | 0 | 20 | N/A — tests the framework itself, no cross-layer validation inputs (`unit-test.md` §9.6) |
| Options, DependencyInjection, AspNetCore, MediatR, ErrorOr, FluentResults, OneOf, Xml, Analyzers | 0 | 35 | N/A — adapter and tooling projects with no cross-layer validation inputs (`unit-test.md` §9.6) |

**Note**: Core (51%) and DataAnnotations (53%) are now the layers with the lowest adoption. Migrating their remaining TestData files to the pattern defined in `fixture.md` is the outstanding gap.

## Re-verifying Coverage

GOLD is only as fresh as its last verification (see the dated section above). After any
substantive test or source change, re-verify per `docs/ai/specs/testing/coverage.md`:

- run the scope's coverage agent — `/coverage-core`, `/coverage-must`, `/coverage-guard`, `/coverage-fluent`, `/coverage-annotation`, `/coverage-testing`, `/coverage-all`; or
- run the script directly:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope <Scope> -SkipHtml -Enforce100
```

If a scope comes back below 100%, demote that project to SILVER here, close the gap
(`/fix-coverage-<scope>`), then restore GOLD with a new dated verification section.

## Next Steps

1. **Increase fixture adoption** in the Core and DataAnnotations projects (51% and 53% — the outstanding gap)
2. **Maintain this index** — recompute the tables (per the counting method above) and re-date the
   coverage verification whenever new test classes are added

## History

### Empty Array Audit (2026-03-04)

#### Summary

| Category | Count | Action Taken |
|----------|-------|-------------|
| Removed (legitimately no cases) | 86 | Deleted empty properties entirely |
| Populated (were empty, now have data) | 4 | Added test cases |
| **Total audited** | **115** | **All resolved** |

#### Arrays Populated

| File | Operation Group | Dataset | Cases Added |
|------|----------------|---------|-------------|
| `TimeOnlyRangeTestData.cs` | Overlaps | ValidCases | 5 (partial overlap, contained, identical, no overlap, inclusive vs exclusive) |
| `StringAttributesTestData.cs` | NullOrWhiteSpaceString | EdgeCases | 5 (tab, newline, CRLF, multi-space, mixed whitespace) |
| `BaseUnitTestTestData.cs` | CreateDeterministicRandom | EdgeCases | 3 (negative seed, int.MaxValue, int.MinValue) |
| `ThrowsCaseAssertTestData.cs` | Expected | EdgeCases | 2 (case-insensitive match, empty messageContains) |

#### Removed Empty Datasets (by pattern)

| Pattern | Count | Reason for removal |
|---------|-------|--------------------|
| Null-return methods (no throws) | 28 | Methods return null/false, never throw |
| Pure boolean validators | 8 | `IsValid()`, `IsDigitsOnly()` return false |
| Immutable records (no validation) | 22 | Record constructors are pure data holders |
| Enum exhaustive | 6 | All enum values tested |
| Marker/sentinel | 2 | `Guard.Against` — zero behavior |
| LINQ extensions | 8 | Empty + populated inputs in ValidCases cover all paths |
| Pure type conversions | 6 | Implicit operators with no throw pathway |
| Constructor validation in InvalidCases | 3 | EdgeCases empty because InvalidCases covers boundaries |
| Parameterless/no-logic methods | 3 | `CreateCancelledToken()`, `OnDispose()`, etc. |

<!-- footer
last_verified: 2026-11-18
-->
