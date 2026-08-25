---
spec:
  id: pineguard.ai.data-annotations.unit-test
  title: "PineGuard.DataAnnotations Unit Tests (Addendum)"
  version: 6
  template:
    - ../../meta/template-unit-test.md
  parent:
    - ../spec.md
    - ../testing/unit-test.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.DataAnnotations/**"
  - "tests/PineGuard.DataAnnotations.UnitTests/**"
---

# PineGuard.DataAnnotations Unit Tests (Addendum)

This file is a **DataAnnotations-specific addendum** to the global unit test spec:

- Global unit test rules: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`

Only DataAnnotations-specific guidance should live here.

---

## Purpose

Document additional DataAnnotations-specific testing guidance; treat the global unit test spec as the baseline.

## Scope

All global unit test rules (framework, TestData shape, formatting, nested Operation Groups, determinism, throws patterns, etc.) are defined in:

- `docs/ai/specs/testing/unit-test.md`

---

## DA-Specific Patterns

### Base Class

DA tests inherit **`BaseDataAnnotationUnitTest`** with the primary constructor output parameter:

```csharp
public sealed class StringBoolAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
```

`BaseDataAnnotationUnitTest` provides:

```csharp
protected static void AssertResult(DataAnnotationCase testCase, ValidationResult? result)
protected static void AssertReturn(ReturnExpected expected, bool actualIsValid, string? actualMessage)
```

`AssertResult` is the standard assertion. `AssertReturn` is used when the test body manually extracts the validation result (e.g., for inline parameterised validators not using `DataAnnotationCase` directly).

### Required Imports

TestData files require:

```csharp
using PineGuard.Testing.UnitTests.DataAnnotations;
```

Tests files require:

```csharp
using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;
```

### Case Type: `DataAnnotationCase`

`DataAnnotationCase` is the **only** allowed case type. No custom record definitions in TestData files:

```csharp
public sealed record DataAnnotationCase(string Name, object? Value, DataAnnotationExpected Expected)
    : ReturnCase<object?, DataAnnotationExpected>(Name, Value, Expected);
```

### Expected Type: `DataAnnotationExpected`

`DataAnnotationExpected` is the **only** allowed expected type:

```csharp
public sealed record DataAnnotationExpected(bool IsValid, string? Message = null, string? MemberName = null)
    : ReturnExpected(IsValid, Message);
```

- `new DataAnnotationExpected(true)` — valid (null is also valid in DA by default)
- `new DataAnnotationExpected(false, "Value must be true.")` — invalid, message checked

### TestData — Unified `Cases` Property

Each Op Group has a single **`Cases`** property. Never use `ValidCases / EdgeCases / InvalidCases` split:

```csharp
public static TheoryData<DataAnnotationCase> Cases => F.BoolIsTrue.AllScenarios.ToDataAnnotationCases(s => s.Name switch
{
    nameof(F.BoolIsTrue.NullValue) => new DataAnnotationExpected(true),
    _ when s.IsValid               => new DataAnnotationExpected(true),
    _                              => new DataAnnotationExpected(false, "Value must be true.")
});
```

DA treats null as valid (standard behavior; null is valid unless `[Required]` is applied).

### Test Structure

- **`BaseDataAnnotationUnitTest(output)`** — required on every Tests class.
- **Flat** — test methods live directly in the outer class, no nested `public static class`.
- **Instance methods** — `public void` (not `public static void`).
- **Single `[MemberData]`** per test method — never stack `[MemberData]` attributes.
- **`// Arrange`**, **`// Act`**, **`// Assert`** section markers required in every test method body.
- **Allman braces** — always block bodies in test methods; never `=> expr` expression bodies.

---

## Pattern A — No-Param Attribute

For attributes with no constructor parameters. Input values come from fixture scenarios.

**TestData:**

```csharp
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.FooRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class FooAttributesTestData
{
    public static class FooAttr
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsFoo.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsFoo.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid          => new DataAnnotationExpected(true),
            _                         => new DataAnnotationExpected(false, "Value must be a valid foo.")
        });
    }
}
```

**When value extractor needed** (fixture type is not `string?` — e.g., `double`, `Guid`, `byte[]`):

```csharp
public static TheoryData<DataAnnotationCase> Cases => F.IsBar.AllScenarios.ToDataAnnotationCases(v => (object?)v, s => s.Name switch
{
    nameof(F.IsBar.NullValue) => new DataAnnotationExpected(true),
    _ when s.IsValid          => new DataAnnotationExpected(true),
    _                         => new DataAnnotationExpected(false, "Value must be a valid bar.")
});
```

**Tests:**

```csharp
using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class FooAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FooAttributesTestData.FooAttr.Cases), MemberType = typeof(FooAttributesTestData.FooAttr))]
    public void FooAttr_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FooAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
```

---

## Pattern B — Typed-Bool Attribute

For attributes that accept `bool?` and throw `InvalidOperationException` on non-bool CLR types
(e.g., `TrueAttribute`, `FalseAttribute`).

**TestData:**

```csharp
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class TrueAttributeTestData
{
    public static class TrueAttr
    {
        public static TheoryData<DataAnnotationCase> Cases => F.BoolIsTrue.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.BoolIsTrue.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid               => new DataAnnotationExpected(true),
            _                              => new DataAnnotationExpected(false, "Value must be true.")
        });
    }

    public static class TrueAttrTypeMismatch
    {
        private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
            : ThrowsCase<Action>(Name, Value, ExpectedException);

        public static TheoryData<IThrowsCase> Cases =>
        [
            new ActionThrowsCase(
                "string-value",
                () => new TrueAttribute().GetValidationResult("not a bool", new ValidationContext(new object())),
                new ExpectedException(typeof(InvalidOperationException)))
        ];
    }
}
```

**Tests:**

```csharp
[Theory]
[MemberData(nameof(TrueAttributeTestData.TrueAttr.Cases), MemberType = typeof(TrueAttributeTestData.TrueAttr))]
public void TrueAttr_BehavesAsExpected(DataAnnotationCase tc)
{
    // Arrange
    var attr = new TrueAttribute();
    var ctx = new ValidationContext(new object()) { MemberName = "Value" };

    // Act
    var result = attr.GetValidationResult(tc.Value, ctx);

    // Assert
    AssertResult(tc, result);
}

[Theory]
[MemberData(nameof(TrueAttributeTestData.TrueAttrTypeMismatch.Cases), MemberType = typeof(TrueAttributeTestData.TrueAttrTypeMismatch))]
public void TrueAttr_TypeMismatch_ThrowsExpected(IThrowsCase tc)
{
    // Arrange
    var action = ((ThrowsCase<Action>)tc).Value;

    // Act
    var ex = Assert.Throws(tc.ExpectedException.Type, action);

    // Assert
    ThrowsCaseAssert.Expected(ex, tc);
}
```

---

## Pattern C — Parameterised Attribute

For attributes with constructor parameters that are fixed per Op Group
(e.g., `InCidrRangeAttribute(cidr)`, `HasSchemeAttribute(scheme)`, `CustomPhoneNumberAttribute(min, max)`).

Store constructor params as `public static readonly` fields on the Op Group class, accessed in the test method:

**TestData:**

```csharp
public static class InCidrRange
{
    public static readonly string Cidr = "192.168.1.0/24";

    public static TheoryData<DataAnnotationCase> Cases => F.IsInCidr.AllScenarios
        .Except(nameof(F.IsInCidr.InvalidCidr))
        .ToDataAnnotationCases(inputs => inputs.ip, s => s.Name switch
        {
            nameof(F.IsInCidr.NullIp) => new DataAnnotationExpected(true),
            _ when s.IsValid          => new DataAnnotationExpected(true),
            _                         => new DataAnnotationExpected(false, "Value must be within the specified CIDR range.")
        });
}
```

**When no matching fixture scenario group exists**, use inline `DataAnnotationCase` entries:

```csharp
public static class CustomPhoneNumber
{
    public static readonly int Min = 3;
    public static readonly int Max = 5;

    public static TheoryData<DataAnnotationCase> Cases =>
    [
        new("min",   "123",    new DataAnnotationExpected(true)),
        new("max",   "12345",  new DataAnnotationExpected(true)),
        new("null",  null,     new DataAnnotationExpected(true)),
        new("short", "12",     new DataAnnotationExpected(false, "Value must be a valid phone number.")),
        new("long",  "123456", new DataAnnotationExpected(false, "Value must be a valid phone number.")),
    ];
}
```

**Tests:**

```csharp
[Theory]
[MemberData(nameof(NetworkAttributesTestData.InCidrRange.Cases), MemberType = typeof(NetworkAttributesTestData.InCidrRange))]
public void InCidrRange_BehavesAsExpected(DataAnnotationCase tc)
{
    // Arrange
    var attr = new InCidrRangeAttribute(NetworkAttributesTestData.InCidrRange.Cidr);
    var ctx = new ValidationContext(new object()) { MemberName = "Value" };

    // Act
    var result = attr.GetValidationResult(tc.Value, ctx);

    // Assert
    AssertResult(tc, result);
}
```

---

## Pattern D — Temporal / Range Attribute

For attributes parameterised with reference date/time/timespan values
(e.g., `BetweenTimeOnlyAttribute(min, max)`, `BeforeDateOnlyAttribute(cutoff)`).
Use `private static readonly` fields sourced from fixture named fields or stable hardcoded values.

**TestData:**

```csharp
public static class BetweenTimeOnly
{
    private static readonly TimeOnly Min = F.IsKnownTimes.T1000!.Value;
    private static readonly TimeOnly Max = F.IsKnownTimes.T1200!.Value;

    public static TimeOnly MinParam => Min;
    public static TimeOnly MaxParam => Max;

    public static TheoryData<DataAnnotationCase> Cases =>
    [
        new("in-range", F.IsKnownTimes.T1100!.Value, new DataAnnotationExpected(true)),
        new("out-range", F.IsKnownTimes.T1300!.Value, new DataAnnotationExpected(false, "Value must be within the expected range.")),
        new("null", null, new DataAnnotationExpected(true)),
    ];
}
```

**Tests:**

```csharp
[Theory]
[MemberData(nameof(TimeOnlyAttributesTestData.BetweenTimeOnly.Cases), MemberType = typeof(TimeOnlyAttributesTestData.BetweenTimeOnly))]
public void BetweenTimeOnly_BehavesAsExpected(DataAnnotationCase tc)
{
    // Arrange
    var attr = new BetweenTimeOnlyAttribute(
        TimeOnlyAttributesTestData.BetweenTimeOnly.MinParam,
        TimeOnlyAttributesTestData.BetweenTimeOnly.MaxParam);
    var ctx = new ValidationContext(new object()) { MemberName = "Value" };

    // Act
    var result = attr.GetValidationResult(tc.Value, ctx);

    // Assert
    AssertResult(tc, result);
}
```

---

## Pattern E — TypeMismatch Throws

For Op Groups that verify `InvalidOperationException` when the wrong CLR type is passed.
The `ActionThrowsCase` private record is defined **inside the Op Group**, not at the outer class level.

**TestData:**

```csharp
public static class XxxAttributeTypeMismatch
{
    private sealed record ActionThrowsCase(string Name, Action Value, ExpectedException ExpectedException)
        : ThrowsCase<Action>(Name, Value, ExpectedException);

    public static TheoryData<IThrowsCase> Cases =>
    [
        new ActionThrowsCase(
            "int-value",
            () => new XxxAttribute().GetValidationResult(42, new ValidationContext(new object())),
            new ExpectedException(typeof(InvalidOperationException)))
    ];
}
```

**Tests:**

```csharp
[Theory]
[MemberData(nameof(XxxAttributesTestData.XxxAttributeTypeMismatch.Cases), MemberType = typeof(XxxAttributesTestData.XxxAttributeTypeMismatch))]
public void XxxAttribute_TypeMismatch_ThrowsExpected(IThrowsCase tc)
{
    // Arrange
    var action = ((ThrowsCase<Action>)tc).Value;

    // Act
    var ex = Assert.Throws(tc.ExpectedException.Type, action);

    // Assert
    ThrowsCaseAssert.Expected(ex, tc);
}
```

---

## Canonical Example (StringBoolAttributes)

**TestData** (`tests/PineGuard.DataAnnotations.UnitTests/StringBoolAttributesTestData.cs`):

```csharp
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class StringBoolAttributesTestData
{
    public static class TrueString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.BoolIsTrue.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.BoolIsTrue.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid               => new DataAnnotationExpected(true),
            _                              => new DataAnnotationExpected(false, "Value must be true.")
        });
    }

    public static class FalseString
    {
        public static TheoryData<DataAnnotationCase> Cases => F.BoolIsFalse.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.BoolIsFalse.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid                => new DataAnnotationExpected(true),
            _                               => new DataAnnotationExpected(false, "Value must be false.")
        });
    }
}
```

**Tests** (`tests/PineGuard.DataAnnotations.UnitTests/StringBoolAttributesTests.cs`):

```csharp
using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringBoolAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringBoolAttributesTestData.TrueString.Cases), MemberType = typeof(StringBoolAttributesTestData.TrueString))]
    public void TrueString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new TrueStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringBoolAttributesTestData.FalseString.Cases), MemberType = typeof(StringBoolAttributesTestData.FalseString))]
    public void FalseString_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FalseStringAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
```

---

## Prohibited Patterns

The following patterns are **explicitly forbidden** and must never appear in DA test files:

| Forbidden | Correct Alternative |
|-----------|---------------------|
| Custom `ValidCase` records in TestData | `DataAnnotationCase` (universal) |
| `ReturnCase<T, bool>` as expected base | `DataAnnotationExpected` only |
| `ValidCases / EdgeCases / InvalidCases` split | Single `Cases` property per Op Group |
| `CommonEdgeCases()` helper methods | Switch expression in `Cases` |
| `Func<object?>` lazy value pattern | Values boxed at construction time |
| `DateTime.UtcNow` in TestData | Fixture static fields |
| `Must.Be.*` in test bodies | Hardcoded literal messages in `DataAnnotationExpected` |
| Private `Validate<TAttribute>` helper | Inline `// Arrange / // Act / // Assert` body |
| `Assert.Equal(bool, ...)` | `AssertResult(tc, result)` |
| `BaseUnitTest` on Tests class | `BaseDataAnnotationUnitTest(output)` |
| `=> AssertResult(...)` expression bodies | Allman block bodies in test methods |
| `get { ... }` block properties in TestData | `=> expr` expression bodies |
| Multiple `[MemberData]` stacks on one method | Single `[MemberData]` pointing to `Cases` |
| `AdHocCases` with standalone null entry | Null handled in `Cases` switch expression |
| Non-AAA comments in any test file | `// Arrange`, `// Act`, `// Assert` only |
| `#pragma warning disable` suppressions | Proper test design |
| `using PineGuard.Testing.UnitTests;` | `using PineGuard.Testing.UnitTests.DataAnnotations;` |
| `ActionThrowsCase` at outer TestData class level | Private sealed record inside TypeMismatch Op Group |
| `TheoryData<object?>` for TypeMismatch | `TheoryData<IThrowsCase>` |
| `ThrowsCase` as method parameter type | `IThrowsCase` |

---

## References

- Global unit test spec: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`
