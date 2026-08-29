---
spec:
  id: pineguard.ai.must-clauses.unit-test
  title: "PineGuard.MustClauses Unit Tests (Addendum)"
  version: 3
  template:
    - ../../meta/template-unit-test.md
  parent:
    - ../spec.md
    - ../testing/unit-test.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.MustClauses/**"
  - "tests/PineGuard.MustClauses.UnitTests/**"
---

# PineGuard.MustClauses Unit Tests (Addendum)

This file is a **MustClauses-specific addendum** to the global unit test spec:

- Global unit test rules: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`

---

## Must-Specific Patterns

### Base Class

Must tests inherit **`BaseMustUnitTest`** (not `BaseUnitTest`):

```csharp
public sealed class MustCsvClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
```

`BaseMustUnitTest` provides:

```csharp
protected static void AssertResult<TValue, TResult>(MustCase<TValue> testCase, MustResult<TResult> result)
```

This checks `IsValid`, `Message` (if not null in Expected), and `ParamName` (if not null in Expected).

### Case Type

Must tests use **`MustCase<TValue>`** — no custom record definitions:

```csharp
public sealed record MustCase<TValue>(string Name, TValue Value, MustExpected Expected)
    : ReturnCase<TValue, MustExpected>(Name, Value, Expected);
```

### Expected Type: `MustExpected`

```csharp
public sealed record MustExpected(bool IsValid, string? Message = null, string? ParamName = null, string? Code = null)
    : ReturnExpected(IsValid, Message);
```

- `new MustExpected(true)` — valid case, no message/paramName checked
- `new MustExpected(false, "value must be a valid CSV line.")` — failure, message checked, no paramName
- `new MustExpected(false, "value must not be null.", "value")` — failure, message AND paramName checked

### Required Imports

TestData and Tests files require the MustClauses sub-namespace:

```csharp
using PineGuard.Testing.UnitTests.MustClauses;
```

### TestData Pattern

Two datasets per Op Group — **`ValidCases`** and **`InvalidCases`** (no `EdgeCases`):

```csharp
public static TheoryData<MustCase<string?>> ValidCases  => F.IsCsvLine.ValidScenarios.ToMustCases();
public static TheoryData<MustCase<string?>> InvalidCases => F.IsCsvLine.InvalidScenarios.ToMustCases(s => s.Name switch
{
    nameof(F.IsCsvLine.NullValue) => new MustExpected(false, "csvLine must not be null.", "csvLine"),
    _                             => new MustExpected(false, "csvLine must be a valid CSV line.")
});
```

**All Must TestData must be fixture-backed.** If a fixture is missing scenario arrays (`ValidScenarios`/`InvalidScenarios`/`AllScenarios`), the fixture must be completed first — never use inline hardcoded test data as a workaround.

### Test Structure

- **Flat** — test methods live directly in the outer class, no nested `public static class` per Op Group.
- **Instance methods** — `public void` (not `public static void`).
- **Two `[MemberData]` attributes** per test method — one for `ValidCases`, one for `InvalidCases`.
- **`// Act` and `// Assert`** section markers required in every test method.
- **`// Arrange`** required when tuple deconstruction or local assignment is needed.

### Testing Null Inputs

When dealing with nullable reference types (`string?`), always let null reach the `Must.Be.*` method — do NOT guard inside the test method. MustClauses handle nulls and return appropriate failure results.

---

## Canonical Example (CsvLine — Fixture-Backed)

**TestData**:

```csharp
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustCsvClausesTestData
{
    public static class CsvLine
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsCsvLine.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsCsvLine.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new MustExpected(false, "line must not be null.", "line"),
            _                             => new MustExpected(false, "line must be a valid CSV line.")
        });
    }
}
```

**Tests**:

```csharp
using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustCsvClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustCsvClausesTestData.CsvLine.ValidCases), MemberType = typeof(MustCsvClausesTestData.CsvLine))]
    [MemberData(nameof(MustCsvClausesTestData.CsvLine.InvalidCases), MemberType = typeof(MustCsvClausesTestData.CsvLine))]
    public void CsvLine_BehavesAsExpected(MustCase<string?> tc)
    {
        // Act
        var result = Must.Be.CsvLine(tc.Value, paramName: "line");

        // Assert
        AssertResult(tc, result);
    }
}
```

---

## Scope

Prefer testing MustClauses via the public surface: `Must.Be.*`

## Default Test Project

- `tests/PineGuard.MustClauses.UnitTests/PineGuard.MustClauses.UnitTests.csproj`

## References

- Global unit test spec: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`
