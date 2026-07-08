---
name: core-test-structure
description: Baseline Test Class + TestData structural rules (naming, ordering, MemberData wiring) per unit-test.md §4-5.
metadata:
  type: feedback
---

### Test Class Structure (per `docs/ai/specs/testing/unit-test.md` §5)
- `sealed class` inheriting `BaseUnitTest` via primary constructor
- Namespace mirrors source: `PineGuard.X.UnitTests` for `PineGuard.X`
- Naming: `[SubjectClassName]Tests` (e.g., `MustBoolClausesTests`)
- Outer class must NOT contain test methods — use nested `public static class` per Operation Group (§5.1)
- Test methods must be `public static void`
- Method naming (strict §5.1): `Valid_BehavesAsExpected`, `ValidAndEdge_BehavesAsExpected`, `Invalid_ThrowsAsExpected`
- Place `XxxTests.cs` and `XxxTestData.cs` side-by-side in mirrored folder
- Note: several projects have drifted to a flatter "v2" pattern with no nested Op Groups in the Tests file — check sibling test methods in the same file first (see [[fact-to-theory-conversion]])

### Test Data Pattern (Nested Operation Groups, per §4)
- Outer class: `public static class [SubjectClassName]TestData`
- Nested class per operation: `[SubjectClassName]TestData.[MethodName]`
- Element ordering within each group (§4.4): datasets first (`ValidCases` → `EdgeCases` → `InvalidCases`), then records (`ValidCase`/`Case` → `InvalidCase`)
- Record type per scenario: inherits from `RuleCase<T>`, `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase` (v2 case records)
- Access expected value via `testCase.Expected` (NOT `testCase.ExpectedReturn`)
- Dataset properties return `TheoryData<T>` — all three must exist even if empty (`=> []`)
- Outer TestData class ordering (§4.6): shared fields → Op Groups → helper methods (at bottom)
- Structural correspondence (§4.5): Tests file nested classes mirror TestData in same order
- Tuple property MUST be named `Value` (PascalCase) — matches `ValueCase<TValue>.Value`. NEVER use `Input`.
- Tuple element names MUST be camelCase and MUST be the **exact parameter names** from the method under test (§4.3)
- DO NOT hardcode data arrays in test methods
- DO NOT use named arguments in test case records — use named tuples for multi-input
- Use Test Fixtures (`PineGuard.Testing.Fixtures/`) for shared input constants (§10)
- Alias: `using F = PineGuard.Testing.Fixtures.[Class]Fixtures;`
- Use `nameof(F.OpGroup.Field)` for test case Name — zero magic strings
- Fixtures = raw values ONLY (no records, no datasets) — each project defines its own

### MemberData Pattern
```csharp
[Theory]
[MemberData(nameof(MustBoolClausesTestData.True.ValidCases), MemberType = typeof(MustBoolClausesTestData.True))]
[MemberData(nameof(MustBoolClausesTestData.True.EdgeCases), MemberType = typeof(MustBoolClausesTestData.True))]
public static void ValidAndEdge_BehavesAsExpected(MustBoolClausesTestData.True.ValidCase testCase)
```

### Full Canonical Examples
- See §9 of `docs/ai/specs/testing/unit-test.md` for complete file-level TestData + Tests pair
- Shows predicate tests, tuple input tests, value+throws tests with all structural rules demonstrated
