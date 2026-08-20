---
name: core-test-structure
description: Baseline Test Class + TestData structural rules (naming, ordering, MemberData wiring) per unit-test.md §4-5.
metadata:
  type: feedback
---

### Test Class Structure (per `docs/ai/rules/fixture-conventions.md` §4 and `docs/ai/specs/testing/fixture.md`)
- `sealed class` inheriting the layer's `BaseXxxUnitTest` via primary constructor
- Namespace mirrors source: `PineGuard.X.UnitTests` for `PineGuard.X`
- Naming: `[SubjectClassName]Tests` (e.g., `MustBoolClausesTests`)
- Flat class — no nested `public static class` Operation Groups in the Tests file; the method name carries the grouping
- Method naming: `MethodName_BehavesAsExpected(XxxCase<T> tc)`
- Place `XxxTests.cs` and `XxxTestData.cs` side-by-side in mirrored folder
- Note: `unit-test.md` §5.1 still describes the older nested Operation Group pattern for Tests files; `fixture-conventions.md` §4 supersedes it (see [[fixture-architecture-v2]])

### Test Data Pattern (Nested Operation Groups, per §4)
- Outer class: `public static class [SubjectClassName]TestData`
- Nested class per operation: `[SubjectClassName]TestData.[MethodName]`
- Element ordering within each group (§4.4): datasets first (`ValidCases` → `InvalidCases`), then records (`ValidCase`/`Case` → `InvalidCase`)
- Record type per scenario: inherits from `RuleCase<T>`, `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase` (v2 case records)
- Access expected value via `testCase.Expected` (NOT `testCase.ExpectedReturn`)
- Dataset properties return `TheoryData<T>`; edge cases live inside `ValidCases`/`InvalidCases` via the fixture's `ValidEdge`/`InvalidEdge` scenario arrays — no separate `EdgeCases` dataset
- Outer TestData class ordering (§4.6): shared fields → Op Groups → helper methods (at bottom)
- Structural correspondence (§4.5): Tests methods mirror TestData Op Groups in same order
- Tuple property MUST be named `Value` (PascalCase) — matches `ValueCase<TValue>.Value`. NEVER use `Input`.
- Tuple element names MUST be camelCase and MUST be the **exact parameter names** from the method under test (§4.3)
- DO NOT hardcode data arrays in test methods
- DO NOT use named arguments in test case records — use named tuples for multi-input
- Use Test Fixtures (`PineGuard.Testing.Fixtures/`) for shared input constants (§9 "Test Fixtures")
- Alias: `using F = PineGuard.Testing.Fixtures.[Class]Fixtures;`
- Use `nameof(F.OpGroup.Field)` for test case Name — zero magic strings
- Fixtures = raw values ONLY (no records, no datasets) — each project defines its own

### MemberData Pattern
```csharp
[Theory]
[MemberData(nameof(MustBoolClausesTestData.True.ValidCases), MemberType = typeof(MustBoolClausesTestData.True))]
[MemberData(nameof(MustBoolClausesTestData.True.InvalidCases), MemberType = typeof(MustBoolClausesTestData.True))]
public void True_BehavesAsExpected(MustCase<bool?> tc)
```

### Full Canonical Examples
- See §8 "Full Canonical Examples" of `docs/ai/specs/testing/unit-test.md` for a complete file-level TestData + Tests pair
- Shows predicate tests, tuple input tests, value+throws tests with all structural rules demonstrated
