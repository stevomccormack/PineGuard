---
spec:
  id: pineguard.ai.specs.testing.gold-standard
  title: "GOLD-STANDARD Compliance Index"
  version: 2
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
5. 100% line and branch coverage confirmed via Coverlet (the xplat collector), enforced by the xplat analyzer with `-Enforce100`

## Project Summary

| Project | TestData Files | Op Groups | Fixture-Based Files | Status |
|---------|---------------|-----------|--------------------|--------|
| PineGuard.Core.UnitTests | 87 | 486 | 49 (56%) | SILVER |
| PineGuard.MustClauses.UnitTests | 50 | 541 | 31 (62%) | SILVER |
| PineGuard.GuardClauses.UnitTests | 49 | 538 | 41 (84%) | SILVER |
| PineGuard.FluentValidation.UnitTests | 52 | 660 | 40 (77%) | SILVER |
| PineGuard.DataAnnotations.UnitTests | 50 | 355 | 18 (36%) | SILVER |
| PineGuard.Testing.UnitTests | 15 | 57 | 0 (n/a) | SILVER |

**Counting method** (rerun these to reproduce the table):

- **TestData Files** — files matching `*TestData*.cs` under `tests/<Project>/`, excluding `bin/` and `obj/`.
- **Op Groups** — lines in those files matching `^\s+public static (partial )?class \w+`, i.e. nested Operation Groups, excluding the outer `XxxTestData` class.
- **Fixture-Based Files** — TestData files containing at least one call to the project's scenario extension: `.ToRuleCases(` for Core and Testing, `.ToMustCases(`, `.ToGuardCases(`, `.ToFluentCases(`, `.ToDataAnnotationCases(` for the other layers.

**All projects are SILVER** — structure is correct and no `=> [];` scaffolding remains in any TestData file, but coverage has not been verified against the 100% target. Promotion to GOLD requires a coverage run per the section below.

## Fixture-Based Testing Adoption

TestData files using the `RuleScenario<T>` → `.ToXxxCases()` fixture pattern (counted per the method above):

| Project | Fixture-Based Files | TestData Files | Adoption |
|---------|--------------------|----------------|----------|
| GuardClauses | 41 | 49 | 84% |
| FluentValidation | 40 | 52 | 77% |
| MustClauses | 31 | 50 | 62% |
| Core | 49 | 87 | 56% |
| DataAnnotations | 18 | 50 | 36% |
| Testing | 0 | 15 | N/A — tests the framework itself, no cross-layer validation inputs (`unit-test.md` §9.6) |

**Note**: DataAnnotations is now the only layer with materially low adoption. Migrating its remaining TestData files to the pattern defined in `fixture.md` is the outstanding gap.

## Coverage Status

Coverage has not been re-verified since the last audit, so every project is still graded SILVER on
criterion 5. Verify per `docs/ai/specs/testing/coverage.md`:

- run the scope's coverage agent — `/coverage-core`, `/coverage-must`, `/coverage-guard`, `/coverage-fluent`, `/coverage-annotation`, `/coverage-testing`, `/coverage-all`; or
- run the script directly:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File "./tools/code-coverage/Run-CodeCoverage.ps1" -Mode GenerateAndAnalyze -Scope <Scope> -SkipHtml -Enforce100
```

When a scope comes back green, promote that project's status from SILVER → GOLD.

## Next Steps

1. **Run coverage analysis** per `coverage.md` to identify remaining gaps
2. **Promote to GOLD** as each project achieves 100% line+branch coverage
3. **Increase fixture adoption** in the DataAnnotations project
4. **Maintain this index** — recompute the tables (per the counting method above) when new test classes are added

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
last_verified: 2026-08-20
-->
