---
spec:
  id: pineguard.ai.specs.testing.fixture
  title: "Unified Fixture Architecture v2"
  version: 2
  parent:
    - unit-test.md
applies_to:
  - "tests/**"
---

# Unified Fixture Architecture v2

Authoritative reference for the fixture / scenario architecture implemented in `tests/PineGuard.Testing/`. Where it differs from `unit-test.md` §4, §5 or §8, this file wins.

## 1. Expected Type Hierarchy

```
IExpectedResult { bool IsValid }
├── ReturnExpected(IsValid, Message?)                                        [abstract]
│   ├── MustExpected(IsValid, Message?, ParamName?)                          [sealed]
│   ├── FluentExpected(IsValid, Message?, PropertyName?)                     [sealed]
│   └── DataAnnotationExpected(IsValid, Message?, MemberName?)               [sealed]
├── ThrowExpected(IsValid, ExceptionType?, ParamName?, MessageContains?)     [abstract]
│   └── GuardExpected(IsValid, ExceptionType?, ParamName?, MessageContains?) [sealed]
└── RuleExpected(IsValid)                                                    [sealed]
```

```csharp
public interface IExpectedResult { bool IsValid { get; } }
public abstract record ReturnExpected(bool IsValid, string? Message = null) : IExpectedResult;
public abstract record ThrowExpected(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null) : IExpectedResult;
public sealed record RuleExpected(bool IsValid) : IExpectedResult;
public sealed record MustExpected(bool IsValid, string? Message = null, string? ParamName = null) : ReturnExpected(IsValid, Message);
public sealed record FluentExpected(bool IsValid, string? Message = null, string? PropertyName = null) : ReturnExpected(IsValid, Message);
public sealed record DataAnnotationExpected(bool IsValid, string? Message = null, string? MemberName = null) : ReturnExpected(IsValid, Message);
public sealed record GuardExpected(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null) : ThrowExpected(IsValid, ExceptionType, ParamName, MessageContains);
```

| Layer | Expected | Base | Extra Fields |
|---|---|---|---|
| Rules (Core) | `RuleExpected` | `IExpectedResult` | — |
| Must | `MustExpected` | `ReturnExpected` | `.ParamName` |
| Guard | `GuardExpected` | `ThrowExpected` | `.ExceptionType`, `.ParamName`, `.MessageContains` |
| Fluent | `FluentExpected` | `ReturnExpected` | `.PropertyName` |
| DA | `DataAnnotationExpected` | `ReturnExpected` | `.MemberName` |

Files — one type per file, no `Expected/` folder. The abstract and shared types live under `Common/` (`IExpectedResult.cs`, `ReturnExpected.cs`, `ThrowExpected.cs`); each layer's `Expected`, `Case` and scenario-extension types live under `UnitTests/<Layer>/` (`UnitTests/Rules/RuleExpected.cs`, `UnitTests/MustClauses/MustExpected.cs`, `UnitTests/GuardClauses/GuardExpected.cs`, `UnitTests/FluentValidation/FluentExpected.cs`, `UnitTests/DataAnnotations/DataAnnotationExpected.cs`).

## 2. RuleScenario

```csharp
public sealed record RuleScenario<TInputs>(string Name, TInputs Inputs, bool IsValid)
{
    public bool IsNull => Inputs is null;
}
```

**Format rules** (CSV, Email, URI, JSON — no numeric boundaries):
```
ValidScenarios, InvalidScenarios, AllScenarios = [..Valid, ..Invalid]
```

**Boundary rules** (Length, Range, GeoLocation, Phone, Char, SqlDateTime, Buffer — constants/param boundaries):
```
ValidScenarios, ValidEdgeScenarios, InvalidScenarios, InvalidEdgeScenarios
AllValid = [..ValidScenarios, ..ValidEdgeScenarios]
AllInvalid = [..InvalidScenarios, ..InvalidEdgeScenarios]
AllScenarios = [..AllValid, ..AllInvalid]
```

Rule: If Rule class defines `const`/`static readonly` boundary values OR method has numeric boundary params → 4 arrays + rollups. Otherwise → 2 arrays + `AllScenarios`.

Constants may live in the Rule class itself OR in referenced Utils classes (e.g., `EmailUtility.MaxEmailLength`, `PanAlgorithm.PanMinLength`). Fixtures reference whichever class owns the constant.

## 3. Case Records

```csharp
public sealed record RuleCase<TValue>(string Name, TValue Value, RuleExpected Expected) : ReturnCase<TValue, RuleExpected>(Name, Value, Expected);
public sealed record MustCase<TValue>(string Name, TValue Value, MustExpected Expected) : ReturnCase<TValue, MustExpected>(Name, Value, Expected);
public sealed record GuardCase<TValue>(string Name, TValue Value, GuardExpected Expected) : ReturnCase<TValue, GuardExpected>(Name, Value, Expected);
public sealed record FluentCase<TValue>(string Name, TValue Value, FluentExpected Expected) : ReturnCase<TValue, FluentExpected>(Name, Value, Expected);
public sealed record DataAnnotationCase(string Name, object? Value, DataAnnotationExpected Expected) : ReturnCase<object?, DataAnnotationExpected>(Name, Value, Expected);
```

`IsCase<T>`, `HasCase<T>` are annotated `[Description("Use RuleCase<T> for rules.")]` — soft-deprecated, no compiler warning; do not use in new tests. They are not `[Obsolete]`: `Directory.Build.props` sets `TreatWarningsAsErrors`, so promoting them would break the build at every existing derivation site. A hard deprecation has to be its own migration.

Case files live beside their layer: `UnitTests/Rules/RuleCase.cs`, `UnitTests/MustClauses/MustCase.cs`, `UnitTests/GuardClauses/GuardCase.cs`, `UnitTests/FluentValidation/FluentCase.cs`, `UnitTests/DataAnnotations/DataAnnotationCase.cs`. `IsCase<T>` and `HasCase<T>` remain at `UnitTests/`.

## 4. Extension Methods

| Method | Input | Output |
|---|---|---|
| `.ToRuleCases()` | `RuleScenario<T>[]` | `TheoryData<RuleCase<T>>` |
| `.ToMustCases()` | `RuleScenario<T>[]` | `TheoryData<MustCase<T>>` |
| `.ToMustCases(Func<RuleScenario<T>, MustExpected>)` | `RuleScenario<T>[]` | `TheoryData<MustCase<T>>` |
| `.ToGuardCases()` | `RuleScenario<T>[]` | `TheoryData<GuardCase<T>>` |
| `.ToGuardCases(string paramName)` | `RuleScenario<T>[]` | `TheoryData<GuardCase<T>>` |
| `.ToGuardCases(Func<RuleScenario<T>, GuardExpected>)` | `RuleScenario<T>[]` | `TheoryData<GuardCase<T>>` |
| `.ToFluentCases()` | `RuleScenario<T>[]` | `TheoryData<FluentCase<T>>` |
| `.ToFluentCases(Func<RuleScenario<T>, FluentExpected>)` | `RuleScenario<T>[]` | `TheoryData<FluentCase<T>>` |
| `.ToDataAnnotationCases()` | `RuleScenario<T>[]` | `TheoryData<DataAnnotationCase>` |
| `.ToDataAnnotationCases(Func<RuleScenario<T>, DataAnnotationExpected>)` | `RuleScenario<T>[]` | `TheoryData<DataAnnotationCase>` |
| `.ToDataAnnotationCases(Func<T,object?>)` | `RuleScenario<T>[]` | `TheoryData<DataAnnotationCase>` |
| `.ToDataAnnotationCases(Func<T,object?>, Func<RuleScenario<T>, DataAnnotationExpected>)` | `RuleScenario<T>[]` | `TheoryData<DataAnnotationCase>` |

Auto-logic for `.ToGuardCases(string paramName)`: `IsValid` → valid, `IsNull` → `ArgumentNullException`, else → `ArgumentException`.

## 5. Filter Combinators

```csharp
.WhereValid()                       // scenarios where IsValid == true
.WhereInvalid()                     // scenarios where IsValid == false
.Except(params string[] names)      // exclude by Name
.Only(params string[] names)        // include only by Name
.Project(Func<T, TOut> selector)    // RuleScenario<T>[] -> RuleScenario<TOut>[]
```

`Project` reshapes Core scenarios for a layer that consumes a differently-shaped input — e.g. wrapping a raw value in the model the Fluent or DataAnnotations test binds against. `Name` and `IsValid` carry through unchanged.

## 6. Base Test Classes

```csharp
public abstract class BaseRuleUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
public abstract class BaseMustUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
public abstract class BaseGuardUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
public abstract class BaseFluentUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
public abstract class BaseDataAnnotationUnitTest(ITestOutputHelper output) : BaseUnitTest(output)
```

Each provides `AssertResult(tc, result)` — uniform assertion pattern per layer.

## 7. Canonical Example: CsvRules.IsCsvLine (Format Rule)

### Fixture

```csharp
namespace PineGuard.Testing.Fixtures;

public static class CsvRulesFixtures
{
    public static class IsCsvLine
    {
        public static readonly string? Simple = "a,b";
        public static readonly string? NullValue = null;
        public static readonly string? EmptyString = "";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Simple), Simple, true),
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false),
            new(nameof(EmptyString), EmptyString, false),
        ];

        public static RuleScenario<string?>[] AllScenarios => [..ValidScenarios, ..InvalidScenarios];
    }
}
```

### Rules Layer

```csharp
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

public static class CsvRulesTestData
{
    public static class IsCsvLine
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsCsvLine.AllScenarios.ToRuleCases();
    }
}
```

```csharp
public sealed class CsvRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CsvRulesTestData.IsCsvLine.Cases), MemberType = typeof(CsvRulesTestData.IsCsvLine))]
    public void IsCsvLine_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = CsvRules.IsCsvLine(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
```

### Must Layer

```csharp
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

public static class MustCsvClausesTestData
{
    public static class CsvLine
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsCsvLine.ValidScenarios.ToMustCases();
        public static TheoryData<MustCase<string?>> InvalidCases => F.IsCsvLine.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new MustExpected(false, "csvLine must not be null.", "csvLine"),
            _ => new MustExpected(false, "csvLine must be a valid CSV line.")
        });
    }
}
```

### Guard Layer

```csharp
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

public static class GuardCsvClausesTestData
{
    public static class NotCsvLine
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsCsvLine.ValidScenarios.ToGuardCases();
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsCsvLine.InvalidScenarios.ToGuardCases("csvLine");
    }
}
```

### Fluent Layer

```csharp
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

public static class FluentCsvExtensionsTestData
{
    public static class CsvLine
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsCsvLine.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid CSV line.")
        });
    }
}
```

### DA Layer

```csharp
using F = PineGuard.Testing.Fixtures.CsvRulesFixtures;

public static class CsvAttributesTestData
{
    public static class CsvLine
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsCsvLine.AllScenarios.ToDataAnnotationCases(s => s.Name switch
        {
            nameof(F.IsCsvLine.NullValue) => new DataAnnotationExpected(true),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid CSV line.")
        });
    }
}
```

## 8. Summary

| Layer | Expected | Case | Extension | Base Class |
|---|---|---|---|---|
| Rules | `RuleExpected` | `RuleCase<T>` | `.ToRuleCases()` | `BaseRuleUnitTest` |
| Must | `MustExpected` | `MustCase<T>` | `.ToMustCases()` | `BaseMustUnitTest` |
| Guard | `GuardExpected` | `GuardCase<T>` | `.ToGuardCases()` | `BaseGuardUnitTest` |
| Fluent | `FluentExpected` | `FluentCase<T>` | `.ToFluentCases()` | `BaseFluentUnitTest` |
| DA | `DataAnnotationExpected` | `DataAnnotationCase` | `.ToDataAnnotationCases()` | `BaseDataAnnotationUnitTest` |

Pattern: `Fixture.Scenarios` → `.ToXxxCases()` → `TheoryData<XxxCase>` → `BaseXxxUnitTest.AssertResult()`

## 9. Edge Case Constants

Fixtures should reference boundary constants from Core classes for edge case scenarios. These classes provide the canonical boundary values:

| Source Class | Constants | Use For |
|---|---|---|
| `StringRules` | `MinLength`, `MaxLength` patterns | String length boundary tests |
| `NumberRules` | Numeric range boundaries | Number range edge cases |
| `PanAlgorithm` | `PanMinLength`, `PanMaxLength` | PAN length boundaries |
| `EmailUtility` | `MaxEmailLength` | Email length edge cases |
| `Inclusion` (enum) | `Inclusive`, `Exclusive` | Range inclusion boundary tests |
| `TimeOnlyRange` | `TimeOnly.MinValue`, `TimeOnly.MaxValue` | Time boundary edge cases |

When creating `ValidEdgeScenarios` or `InvalidEdgeScenarios` (§2), reference these constants rather than hardcoding magic numbers:

```csharp
public static RuleScenario<string?>[] ValidEdgeScenarios =>
[
    new(nameof(AtMinLength), AtMinLength, true),  // uses PanAlgorithm.PanMinLength
    new(nameof(AtMaxLength), AtMaxLength, true),  // uses PanAlgorithm.PanMaxLength
];
```

## 10. File Layout & Partial Split

Fixtures live in `tests/PineGuard.Testing/Fixtures/`, namespace `PineGuard.Testing.Fixtures`, one class per Core Rules class: `XxxRules` → `XxxRulesFixtures`.

When the source Rules class is split across partial files, the fixture class mirrors it one-for-one:

| Source | Fixture |
|---|---|
| `src/PineGuard.Core/Rules/StringRules.cs` | `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.cs` |
| `src/PineGuard.Core/Rules/StringRules.Bool.cs` | `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.Bool.cs` |
| `src/PineGuard.Core/Rules/StringRules.Casing.cs` | `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.Casing.cs` |

Each file declares `public static partial class XxxRulesFixtures`. Monolithic per-method fixture files are not used.

Because every partial contributes to the same class, inner class names must be unique across the whole set — prefix them with the partial's sub-scope where a bare method name would collide (`BoolIsTrue`, `BoolIsFalse` in `StringRulesFixtures.Bool.cs`). TestData files still alias the whole class as `F`:

```csharp
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;
```
