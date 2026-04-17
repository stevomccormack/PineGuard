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
4. Fixtures are the source of truth for cross-layer validation inputs (§10)
5. 100% line and branch coverage confirmed via Coverlet

## Project Summary

| Project | TestData Files | Op Groups | Fixture-Based | Status |
|---------|---------------|-----------|--------------|--------|
| PineGuard.Core.UnitTests | 129 | 751 | 346 (46%) | SILVER |
| PineGuard.MustClauses.UnitTests | 59 | 592 | 196 (33%) | SILVER |
| PineGuard.GuardClauses.UnitTests | 53 | 620 | 112 (18%) | SILVER |
| PineGuard.FluentValidation.UnitTests | 54 | 597 | 13 (2%) | SILVER |
| PineGuard.DataAnnotations.UnitTests | 45 | 353 | 0 (0%) | SILVER |
| PineGuard.Testing.UnitTests | 15 | 52 | 0 (0%) | SILVER |

**All projects are SILVER** — structure is correct, empty scaffolding removed, but coverage has not been verified against the 100% target.

## Empty Array Audit (2026-03-04)

### Summary

| Category | Count | Action Taken |
|----------|-------|-------------|
| Removed (legitimately no cases) | 86 | Deleted empty properties entirely |
| Populated (were empty, now have data) | 4 | Added test cases |
| **Total audited** | **115** | **All resolved** |

### Arrays Populated

| File | Operation Group | Dataset | Cases Added |
|------|----------------|---------|-------------|
| `TimeOnlyRangeTestData.cs` | Overlaps | ValidCases | 5 (partial overlap, contained, identical, no overlap, inclusive vs exclusive) |
| `StringAttributesTestData.cs` | NullOrWhiteSpaceString | EdgeCases | 5 (tab, newline, CRLF, multi-space, mixed whitespace) |
| `BaseUnitTestTestData.cs` | CreateDeterministicRandom | EdgeCases | 3 (negative seed, int.MaxValue, int.MinValue) |
| `ThrowsCaseAssertTestData.cs` | Expected | EdgeCases | 2 (case-insensitive match, empty messageContains) |

### Removed Empty Datasets (by pattern)

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

## Fixture-Based Testing Adoption

Projects using the `RuleScenario<T>` → `.ToXxxCases()` fixture pattern:

| Project | Fixture-Based Groups | Total Groups | Adoption |
|---------|---------------------|-------------|----------|
| Core | 346 | 751 | 46% |
| MustClauses | 196 | 592 | 33% |
| GuardClauses | 112 | 620 | 18% |
| FluentValidation | 13 | 597 | 2% |
| DataAnnotations | 0 | 353 | 0% |
| Testing | 0 | 52 | 0% (N/A — tests framework itself) |

**Note**: FluentValidation and DataAnnotations have low fixture adoption. This is a known gap — these layers can benefit from migrating to the fixture-based pattern defined in `fixture.md`.

## Coverage Status

Coverage verification is **deferred** — to be run in a separate session using:

```bash
dotnet test --collect:"XPlat Code Coverage" -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
```

When coverage is verified, update each project's status from SILVER → GOLD.

## Next Steps

1. **Run coverage analysis** per `coverage.md` to identify remaining gaps
2. **Promote to GOLD** as each project achieves 100% line+branch coverage
3. **Increase fixture adoption** in FluentValidation and DataAnnotations projects
4. **Maintain this index** — update when new test classes are added

<!-- footer
last_verified: 2026-03-04
-->
