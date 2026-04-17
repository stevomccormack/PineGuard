---
spec:
  id: pineguard.ai.fluent-validation.unit-test
  title: "PineGuard.FluentValidation Unit Tests (Addendum)"
  version: 4
  template:
    - ../../meta/template-unit-test.md
  parent:
    - ../../spec.md
    - ../../testing/unit-test.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.FluentValidation/**"
  - "tests/PineGuard.FluentValidation.UnitTests/**"
---

# PineGuard.FluentValidation Unit Tests (Addendum)

This file is a **FluentValidation-specific addendum** to the global unit test spec:

- Global unit test rules: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`

---

## Purpose

Document FluentValidation-specific unit testing guidance only; treat the global unit test spec as the baseline.

---

## Fluent-Specific Patterns

### Base Class

Fluent tests inherit **`BaseFluentUnitTest`** (not `BaseUnitTest`):

```csharp
public sealed class FluentBoolExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
```

`BaseFluentUnitTest` provides:

```csharp
protected static void AssertResult<TValue>(FluentCase<TValue> testCase, ValidationResult result)
```

This checks `IsValid`, `Message` (if not null in Expected), and `PropertyName` (if not null in Expected).

### Case Type

Fluent tests use **`FluentCase<TValue>`** — no custom record definitions:

```csharp
public sealed record FluentCase<TValue>(string Name, TValue Value, FluentExpected Expected)
    : ReturnCase<TValue, FluentExpected>(Name, Value, Expected);
```

### Expected Type: `FluentExpected`

```csharp
public sealed record FluentExpected(bool IsValid, string? Message = null, string? PropertyName = null)
    : ReturnExpected(IsValid, Message);
```

- `new FluentExpected(true)` — valid, no message checked
- `new FluentExpected(false, "Value must be true.")` — invalid, message checked

### TestData Pattern

Single **`Cases`** property per Op Group using `AllScenarios.ToFluentCases(switch)`:

```csharp
public static TheoryData<FluentCase<bool?>> Cases => F.IsTrue.AllScenarios.ToFluentCases(s => s.Name switch
{
    nameof(F.IsTrue.Null)  => new FluentExpected(true),
    _ when s.IsValid       => new FluentExpected(true),
    _                      => new FluentExpected(false, "Value must be true.")
});
```

### Null Handling

FluentValidation has two distinct null behaviors:

1. **Default (passthrough)** — FluentValidation skips the rule when the value is `null`, treating null as implicitly valid. Map these to `new FluentExpected(true)`:
   ```csharp
   nameof(F.IsTrue.Null) => new FluentExpected(true)
   ```

2. **Explicit null-is-error** — Some rules actively validate that null is disallowed (e.g. Owasp XSS requires non-null). These map to `new FluentExpected(false, "Value must not be null.")`.

The `_ when s.IsValid => new FluentExpected(true)` arm covers all other valid scenarios. The null override arm must appear **before** the generic valid arm in the switch.

### Model + Validator Pattern

- `private sealed record Model` at the **outer Tests class level** (shared across all test methods).
- `private sealed class XxxValidator : AbstractValidator<Model>` at the **outer Tests class level** (one per extension method under test).

```csharp
public sealed class FluentBoolExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public bool? Value { get; init; } }

    private sealed class TrueValidator : AbstractValidator<Model>
    {
        public TrueValidator() => RuleFor(x => x.Value).True();
    }

    private sealed class FalseValidator : AbstractValidator<Model>
    {
        public FalseValidator() => RuleFor(x => x.Value).False();
    }

    // test methods follow...
}
```

### Test Structure

- **Flat** — test methods live directly in the outer class, no nested `public static class`.
- **Instance methods** — `public void` (not `public static void`).
- **Single `[MemberData]`** per test method pointing to `Cases`.
- **`// Act` and `// Assert`** section markers required.
- Comments inline: `// FluentXxxExtensions.MethodName` above each test method.

---

## Explicit Prohibitions

The following patterns are **strictly forbidden**. Any file that contains them must be migrated:

- **No custom case records**: only `FluentCase<TValue>`. Never define `ValidCase`, `NullCase`, `Args`, or any other local record extending `ReturnCase<T, bool>`.
- **No split datasets**: only a single `Cases` property per Op Group. Never `ValidCases`, `EdgeCases`, `NullCases`, `InvalidCases` as separate `TheoryData` properties.
- **No `InlineValidator<T>`**: always use `private sealed class XxxValidator : AbstractValidator<Model>`.
- **No static test methods**: always `public void` instance methods.
- **No nested `public static class` inside Tests file**: flat structure only — Models and Validators at outer class level, test methods directly in the outer class.
- **No custom assertion helpers**: only `AssertResult(tc, result)` from `BaseFluentUnitTest`.
- **No `[Fact]` or `[InlineData]`**: always `[Theory]` + `[MemberData]`.
- **No inline magic values in TestData**: all scenarios must come from fixture classes (`tests/PineGuard.Testing/Fixtures/`).

---

## Parameterized Validators

When a rule takes parameters (e.g. `InRange(min, max)`, `HasAllBits(mask)`, `ExactLength(n)`), the fixture uses a **named tuple** input that bundles both the value-under-test and the rule parameters. The validator is instantiated with the parameters from `tc.Value`:

**TestData:**
```csharp
public static class InRange
{
    public static TheoryData<FluentCase<(int? value, int min, int max)>> Cases =>
        F.IsInRange.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsInRange.NullValue) => new FluentExpected(true),
            _ when s.IsValid              => new FluentExpected(true),
            _                             => new FluentExpected(false, "Value must be within the specified range.")
        });
}
```

**Tests:**
```csharp
private sealed class InRangeValidator(int min, int max) : AbstractValidator<NullableModel>
{
    public InRangeValidator(int min, int max) => RuleFor(x => x.Value).InRange(min, max);
}

// FluentNumberExtensions.InRange
[Theory]
[MemberData(nameof(FluentNumberExtensionsTestData.InRange.Cases), MemberType = typeof(FluentNumberExtensionsTestData.InRange))]
public void InRange_BehavesAsExpected(FluentCase<(int? value, int min, int max)> tc)
{
    // Act
    var result = new InRangeValidator(tc.Value.min, tc.Value.max).Validate(new NullableModel { Value = tc.Value.value });

    // Assert
    AssertResult(tc, result);
}
```

Tuple element names must be **camelCase** and match the parameter names in the source method exactly.

---

## Nullable vs Non-Nullable Variants

When a rule applies to both `T?` (nullable) and `T` (non-nullable):

- **Nullable variant**: standard `AllScenarios.ToFluentCases(switch)` including the null override arm.
- **Non-nullable variant**: exclude the null scenario using `.Except(nameof(F.X.NullValue)).Project(v => v!.Value)`, then use a separate nested class and test method.

```csharp
// Non-nullable variant in TestData
public static class EvenNonNullable
{
    public static TheoryData<FluentCase<int>> Cases =>
        F.IsEven.AllScenarios
            .Except(nameof(F.IsEven.NullValue))
            .Project(v => v!.Value)
            .ToFluentCases(s => s.IsValid
                ? new FluentExpected(true)
                : new FluentExpected(false, "Value must be even."));
}
```

```csharp
// Non-nullable variant in Tests
private sealed record NonNullableModel { public int Value { get; init; } }

private sealed class EvenNonNullableValidator : AbstractValidator<NonNullableModel>
{
    public EvenNonNullableValidator() => RuleFor(x => x.Value).Even();
}

// FluentNumberExtensions.Even (non-nullable)
[Theory]
[MemberData(nameof(FluentNumberExtensionsTestData.EvenNonNullable.Cases), MemberType = typeof(FluentNumberExtensionsTestData.EvenNonNullable))]
public void Even_NonNullable_BehavesAsExpected(FluentCase<int> tc)
{
    // Act
    var result = new EvenNonNullableValidator().Validate(new NonNullableModel { Value = tc.Value });

    // Assert
    AssertResult(tc, result);
}
```

**Forbidden**: `if (testCase.Value is null) return;` inside any test method body. This silently skips cases and masks coverage gaps.

---

## Missing Fixture Classes

If a rule op group requires test scenarios but no fixture class exists in `tests/PineGuard.Testing/Fixtures/`, add the fixture class **before** writing the TestData. Never inline scenarios in TestData files.

Reference the existing fixture files for the correct shape. All new fixture classes follow the standard pattern:
```csharp
public static class IsXxx
{
    public static readonly T ValidValue = ...;
    public static readonly T InvalidValue = ...;
    public static RuleScenario<T>[] ValidScenarios => [...];
    public static RuleScenario<T>[] InvalidScenarios => [...];
    public static RuleScenario<T>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
}
```

---

## Canonical Example (BoolRules Fluent)

**TestData** (`tests/PineGuard.FluentValidation.UnitTests/FluentBoolExtensionsTestData.cs`):

```csharp
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.BoolRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentBoolExtensionsTestData
{
    public static class True
    {
        public static TheoryData<FluentCase<bool?>> Cases => F.IsTrue.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsTrue.Null) => new FluentExpected(true),
            _ when s.IsValid      => new FluentExpected(true),
            _                     => new FluentExpected(false, "Value must be true.")
        });
    }
    
    public static class False
    {
        public static TheoryData<FluentCase<bool?>> Cases => F.IsFalse.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsFalse.Null) => new FluentExpected(true),
            _ when s.IsValid       => new FluentExpected(true),
            _                      => new FluentExpected(false, "Value must be false.")
        });
    }
}
```

**Tests** (`tests/PineGuard.FluentValidation.UnitTests/FluentBoolExtensionsTests.cs`):

```csharp
using FluentValidation;
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.FluentValidation.UnitTests;

public sealed class FluentBoolExtensionsTests(ITestOutputHelper output) : BaseFluentUnitTest(output)
{
    private sealed record Model { public bool? Value { get; init; } }

    private sealed class TrueValidator : AbstractValidator<Model>
    {
        public TrueValidator() => RuleFor(x => x.Value).True();
    }

    private sealed class FalseValidator : AbstractValidator<Model>
    {
        public FalseValidator() => RuleFor(x => x.Value).False();
    }
    
    [Theory]
    [MemberData(nameof(FluentBoolExtensionsTestData.True.Cases), MemberType = typeof(FluentBoolExtensionsTestData.True))]
    public void True_BehavesAsExpected(FluentCase<bool?> tc)
    {
        // Act
        var result = new TrueValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
    
    [Theory]
    [MemberData(nameof(FluentBoolExtensionsTestData.False.Cases), MemberType = typeof(FluentBoolExtensionsTestData.False))]
    public void False_BehavesAsExpected(FluentCase<bool?> tc)
    {
        // Act
        var result = new FalseValidator().Validate(new Model { Value = tc.Value });

        // Assert
        AssertResult(tc, result);
    }
}
```

---

## Default Test Project

- `tests/PineGuard.FluentValidation.UnitTests/PineGuard.FluentValidation.UnitTests.csproj`

## References

- Global unit test spec: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`
