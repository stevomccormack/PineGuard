# Reference Files: implement-unit-tests

> Read the exemplar files listed below before implementing. Do NOT copy them — follow the same patterns.

## Core Rules Tests

| Role | Exemplar File |
|------|---------------|
| Test class | `tests/PineGuard.Core.UnitTests/Rules/JsonRulesTests.cs` |
| Test data | `tests/PineGuard.Core.UnitTests/Rules/JsonRulesTestData.cs` |
| Fixtures | `tests/PineGuard.Testing/Fixtures/JsonRulesFixtures.cs` |

## Gold Standard Index

Consult `docs/ai/specs/testing/gold-standard.md` for a complete list of reference test implementations that have been verified at 100% coverage.

## Patterns

- **Always** `[Theory]` + `TheoryData<T>` — never `[Fact]`
- **Nested Operation Groups** in TestData: `public static class IsJson { public static TheoryData<...> Data => ... }`
- **Side-by-side** placement: `XxxTests.cs` + `XxxTestData.cs` in the same folder
- **Composite expected types**: `MustExpected(bool IsValid, string? Message?, string? ParamName?)`, `FluentExpected(bool IsValid, string? Message?)`
