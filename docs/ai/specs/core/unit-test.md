---
spec:
  id: pineguard.ai.core.unit-test
  title: "PineGuard.Core Unit Tests (Addendum)"
  version: 5
  template:
    - ../../meta/template-unit-test.md
  parent:
    - ../spec.md
    - ../testing/unit-test.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.Core/**"
  - "tests/PineGuard.Core.UnitTests/**"
---

# PineGuard.Core Unit Tests (Addendum)

> This file is a **Core-specific addendum** to the global unit test spec.
> Only Core-specific guidance should live here.

## References

- Global unit test rules: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`

---

## Core-Specific Patterns

### §1 Base Class

Core Rules tests inherit **`BaseRuleUnitTest`** (not `BaseUnitTest`):

```csharp
public sealed class BoolRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
```

`BaseRuleUnitTest` provides:

```csharp
protected static void AssertResult<TValue>(RuleCase<TValue> testCase, bool result)
```

### §2 Case Type

Core tests use **`RuleCase<TValue>`** — no custom record definitions needed:

```csharp
public sealed record RuleCase<TValue>(string Name, TValue Value, RuleExpected Expected)
    : ReturnCase<TValue, RuleExpected>(Name, Value, Expected);
```

`IsCase<T>` and `HasCase<T>` are **obsolete** (`[Obsolete("Use RuleCase<T> instead.")]`). Do not use them.

### §3 Expected Type

Core tests validate pure logic. `Expected` is always `RuleExpected`:

| Method pattern | Case type | Expected type |
|:---|:---|:---|
| `Is*` / `Has*` predicates | `RuleCase<T>` | `RuleExpected(bool IsValid)` |
| Multi-param rules | `RuleCase<(T1 x, T2 y)>` | `RuleExpected(bool IsValid)` |

No message or paramName testing — Core is a pure logic layer.

### §4 TestData Pattern

Each Operation Group has a **single `Cases` property** — no `ValidCases`/`EdgeCases`/`InvalidCases` split:

```csharp
public static TheoryData<RuleCase<T>> Cases => F.Op.AllScenarios.ToRuleCases();
```

`AllScenarios` is defined in the fixture and combines valid + invalid scenarios.

**No custom record definitions** in TestData — `RuleCase<T>` is used directly.

**No `ValidCases`/`EdgeCases` split** — all scenarios are combined via `AllScenarios`.

#### Exception: ThrowIfNull Preconditions

When a Core Rule method has `ArgumentNullException.ThrowIfNull(param)` preconditions, the Op Group adds an `InvalidCases` dataset alongside `Cases`:

```csharp
public static class HasScheme
{
    public static TheoryData<RuleCase<(string? value, string scheme)>> Cases => F.HasScheme.AllScenarios.ToRuleCases();

    public static TheoryData<IThrowsCase> InvalidCases =>
    [
        new ThrowsCase<(string?, string)>("null scheme", ("https://example.com", null!), new ExpectedException(typeof(ArgumentNullException), "scheme"))
    ];
}
```

This applies to methods with `ThrowIfNull` in: `PredicateRules.Satisfies`, `CollectionRules.HasAny/HasAll`, `StringRules.IsMatch`, `StringRules.ContainsAllowedOnly/ContainsDisallowed`, `UriRules.HasScheme`.

### §5 Test Structure

- **Flat** — test methods live directly in the outer class, no nested `public static class` per Op Group.
- **Instance methods** — `public void` (not `public static void`) because `AssertResult` is inherited from `BaseRuleUnitTest`.
- **Single `[MemberData]`** per test method pointing to `Cases` (plus separate method for `InvalidCases` when applicable).
- **`// Act` and `// Assert`** section markers required in every test method.
- **`// Arrange`** is required when tuple deconstruction or local assignment is needed before the method call.

### §6 Documented Exceptions

| Exception | Scope | Reason |
|:---|:---|:---|
| Time-relative ops | `IsInPast`, `IsInFuture`, `IsWithinDaysFromNow` in DateTime/DateTimeOffset | Values must be computed relative to `UtcNow` at test execution time. Use `get` accessor in TestData, not fixtures. |
| Func/Action inputs | `Func<T,bool>` predicates, `Action` delegates | Runtime closures cannot be `static readonly` fixture fields. Stay inline in TestData. |
| Test doubles | `ReadOnlyCollectionOnly<T>`, `Enumerate<T>()` | Implementation-variant types testing branch coverage. Define in TestData shared fields (§4.6 of global spec). |
| Constants | Do NOT create test Op Groups for constants (e.g., `CharRules.AsciiMinValue`). | Tautological — testing `constant == hardcoded same value`. Fixtures should reference constants for boundary values instead. |

---

## Canonical Example

### Single-Param Rule (BoolRules)

**Fixture** (`tests/PineGuard.Testing/Fixtures/BoolRulesFixtures.cs`):

```csharp
namespace PineGuard.Testing.Fixtures;

public static class BoolRulesFixtures
{
    public static class IsTrue
    {
        public static readonly bool? Valid       = true;
        public static readonly bool? NullValue   = null;
        public static readonly bool? False       = false;

        public static RuleScenario<bool?>[] ValidScenarios =>
        [
            new(nameof(Valid), Valid, true),
        ];

        public static RuleScenario<bool?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(False), False, false),
        ];

        public static RuleScenario<bool?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
```

**TestData** (`tests/PineGuard.Core.UnitTests/Rules/BoolRulesTestData.cs`):

```csharp
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.BoolRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class BoolRulesTestData
{
    public static class IsTrue
    {
        public static TheoryData<RuleCase<bool?>> Cases => F.IsTrue.AllScenarios.ToRuleCases();
    }
    
    public static class IsFalse
    {
        public static TheoryData<RuleCase<bool?>> Cases => F.IsFalse.AllScenarios.ToRuleCases();
    }
}
```

**Tests** (`tests/PineGuard.Core.UnitTests/Rules/BoolRulesTests.cs`):

```csharp
using PineGuard.Rules;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class BoolRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsTrue.Cases), MemberType = typeof(BoolRulesTestData.IsTrue))]
    public void IsTrue_BehavesAsExpected(RuleCase<bool?> tc)
    {
        // Act
        var result = BoolRules.IsTrue(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
    
    [Theory]
    [MemberData(nameof(BoolRulesTestData.IsFalse.Cases), MemberType = typeof(BoolRulesTestData.IsFalse))]
    public void IsFalse_BehavesAsExpected(RuleCase<bool?> tc)
    {
        // Act
        var result = BoolRules.IsFalse(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
```

---

### Multi-Param Rule (ExactLength)

**TestData** (excerpt):

```csharp
public static class IsExactLength
{
    public static TheoryData<RuleCase<(string? value, int length)>> Cases =>
        F.IsExactLength.AllScenarios.ToRuleCases();
}
```

**Tests** (excerpt):

```csharp
[Theory]
[MemberData(nameof(StringRulesTestData.IsExactLength.Cases), MemberType = typeof(StringRulesTestData.IsExactLength))]
public void IsExactLength_BehavesAsExpected(RuleCase<(string? value, int length)> tc)
{
    // Arrange
    var (value, length) = tc.Value;

    // Act
    var result = StringRules.IsExactLength(value, length);

    // Assert
    AssertResult(tc, result);
}
```

> **Note**: `// Arrange` is included when tuple deconstruction is needed. Omit `// Arrange` for single-value inputs where no local assignment is needed before the Act.

---

## Work Ordering (Core Coverage Workflow)

When doing incremental migrations across the repo:

- Progress alphabetically through root namespaces/projects.
- Within `PineGuard.Rules`, start with the deepest child namespaces first:
  - `PineGuard.Rules.Owasp.*`
  - then other `PineGuard.Rules.*`

---

## Default Test Project

- `tests/PineGuard.Core.UnitTests/PineGuard.Core.UnitTests.csproj`

## Scope

Prefer testing Core Rules via the public static methods: `XxxRules.MethodName(...)`.

## References

- Global unit test spec: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`
