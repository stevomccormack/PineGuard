---
spec:
  id: pineguard.ai.specs.testing.unit-tests
  title: "PineGuard Unit Tests (Global Spec)"
  version: 11
  parent:
    - ../../spec.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "tests/**"
  - "src/**" # when adding test-focused hooks/visibility helpers
---

# PineGuard Unit Tests (Global Spec)

This is the **shared, cross-cutting unit test specification** for PineGuard.

All unit test specs (per domain/project) should treat this file as the baseline and only describe what’s different or additional for their scope.

Coverage workflow is documented in `docs/ai/specs/testing/coverage.md`.

## 1. Core Principles (Non-negotiables)

- **Target**: **100% line** and **100% branch** coverage.
- **Reliability**: Tests must be deterministic (independent of machine/culture/time).
- **Structure**: Every test case record must include a non-empty `Name`.
- **Parameterization**: `TheoryData` + `[Theory]` + `[MemberData]` is mandatory; `[Fact]` and `[InlineData]` are disallowed.
- **Comments**: Use `// Arrange`, `// Act`, `// Assert` section markers in test methods. No other inline comments unless they add exceptional value.
- **Brace style**: Test method bodies MUST use Allman brace style — opening `{` on its own line, closing `}` on its own line. Single-line method bodies (`public void Foo() { ... }`) are **strictly forbidden**, even for one-liners. `csharp_preserve_single_line_statements = true` in `.editorconfig` means the formatter will not auto-expand them — violations must be fixed manually.
- **Base class wrapping**: The `: BaseXxxUnitTest(output)` inheritance clause MUST appear on a new line, indented 4 spaces, per `resharper_wrap_extends_list_style = chop_always` and `resharper_wrap_before_extends_colon = true`.

## 2. Repo Conventions

### 2.1 Framework & Base Class

- **Framework**: xUnit.
- **Root Base Class**: `PineGuard.Testing.UnitTests.BaseUnitTest` — forces `InvariantCulture`, provides helpers.
- **Layer-Specific Base Classes**: Each layer inherits from a specialized base that extends `BaseUnitTest`. Use the **layer-specific** base in all test files:

  | Layer | Base Class | `AssertResult` Signature |
  |:---|:---|:---|
  | Core Rules | `BaseRuleUnitTest(output)` | `AssertResult<TValue>(RuleCase<TValue>, bool)` |
  | Must Clauses | `BaseMustUnitTest(output)` | `AssertResult<TValue, TResult>(MustCase<TValue>, MustResult<TResult>)` |
  | Guard Clauses | `BaseGuardUnitTest(output)` | `AssertResult<TValue, TReturn>(GuardCase<TValue>, Func<TReturn>)` |
  | Fluent Validation | `BaseFluentUnitTest(output)` | `AssertResult<TValue>(FluentCase<TValue>, ValidationResult)` |
  | Data Annotations | `BaseDataAnnotationUnitTest(output)` | `AssertResult(DataAnnotationCase, ValidationResult?)` |
  | (Other) | `BaseUnitTest(output)` | — |

  All classes use **primary constructor** syntax:
  ```csharp
  public sealed class BoolRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
  ```

- **`UseCulture(...)`**: Use only if explicitly testing culture-specific behavior.
- **Additional helpers**: `UseEnvironmentVariable(key, value)`, `CreateDeterministicRandom(seed)`, `CreateCancelledToken()`.
- **Layer-specific addenda**: Each layer has its own spec addendum that overrides patterns here. See `docs/ai/specs/{layer}/unit-test.md`.

### 2.2 PineGuard.Testing — Shared Test Infrastructure Library

`PineGuard.Testing` (`tests/PineGuard.Testing/`) is the **shared test helper library** referenced by all `*.UnitTests` projects. It has no own test runner; its code is exercised via all other `*.UnitTests` runs.

**Provided types** (all in `PineGuard.Testing.UnitTests` or `PineGuard.Testing.Common`):

| Type | Purpose |
| :--- | :--- |
| `BaseUnitTest` | Abstract base for all test classes. Enforces `InvariantCulture`. |
| `BaseCase` | Root abstract record; provides `Name` and `ToString()`. |
| `ValueCase<TValue>` | Case with a single `Value` input. |
| `ReturnCase<TValue, TExpected>` | Case for value-returning methods (`Expected`). Bridge property: `Expected => Expected`. |
| `ReturnOutCase<TValue, TExpected, TOut>` | Case for methods with both a return value and an `out` parameter. |
| `MustExpected` | Composite expected type for Must layer: `(bool IsValid, string? Message, string? ParamName)`. |
| `FluentExpected` | Composite expected type for Fluent layer: `(bool IsValid, string? Message)`. |
| `IsCase<TValue>` | Specialisation of `ReturnCase` for `bool`-returning `Is*` predicates. |
| `HasCase<TValue>` | Specialisation of `ReturnCase` for `bool`-returning `Has*` predicates. |
| `TryCase<TValue, TOut>` | Case for Try-pattern methods (`bool` return + `out TOut`). |
| `ThrowsCase<TValue>` | Case for exception-throwing scenarios; supports `TValue = Action` for procedural inputs. |
| `ThrowsCaseAssert` | Asserts `ThrowsCase` expectations against a caught exception. |
| `ExpectedException` | Positional record: `new(typeof(ArgumentException), "paramName", "messageContains")`. |
| `IThrowsCase` | Interface used for `TheoryData<IThrowsCase>` datasets. |
| `IReturnsCase<TExpected>` | Interface for return-value cases. |
| `IReturnsOutCase<TExpected, TOut>` | Interface for out-value cases. |
| `*Fixtures` (in `Fixtures/`) | Shared input constants for cross-layer validations (§9). |

### 2.3 Folder Structure

Mirror the source layout exactly so navigation is obvious:

- Source: `src/<Library>/<Subfolders>/<File>.cs`
- Tests: `tests/<Library>.UnitTests/<Subfolders>/<File>Tests.cs`
- Shared helpers: `tests/PineGuard.Testing/` _(no mirroring needed — not a test runner project)_

## 3. File Structure (Strict)

For each unit under test (e.g., `MyClass`), maintain exactly two files:

1. `MyClassTestData.cs` (Data definitions — records, datasets, references to fixtures)
2. `MyClassTests.cs` (Test execution)

Shared input constants live in `PineGuard.Testing/Fixtures/` (see §10). TestData files reference these fixtures; they never duplicate the raw values.

## 4. Canonical TestData Pattern

### 4.1 Structure

Define `public static class XxxTestData` containing nested **Operation Groups** for each method/feature under test.

Each Operation Group defines up to three datasets:

- `ValidCases` (`TheoryData<ValidCase>`) — Success scenarios.
- `EdgeCases` (`TheoryData<ValidCase>`) — Boundary/Null/Interesting scenarios.
- `InvalidCases` (`TheoryData<IThrowsCase>`) — Exception-throwing scenarios.

Notes:

- **Only include datasets that have test cases.** Omit a dataset entirely when no valid cases exist for that category (e.g., omit `InvalidCases` for pure boolean validators that never throw; omit `EdgeCases` when boundaries are already covered in `ValidCases`).
- Do **not** leave empty scaffolding (`=> [];`). Empty arrays fail static analysis (SonarQube, inspections) and add noise.
- For **non-throwing predicate** methods (e.g., `Is*`, `Has*` rules that return `false` for bad input), omit `InvalidCases` and put “bad-but-not-throwing” inputs (e.g., `null`, whitespace, wrong length, unknown codes) in `EdgeCases`.

### 4.2 Case Records

Define case records inside each Operation Group.

Default rule: **keep TestData declarative** (inputs + expected outputs/exception metadata). The **test method owns the Act step**.

`Action`-based cases (where `Value` is an `Action`) are **allowed only in exceptional circumstances**, such as:

- Overload/generic-selection tests where expressing the _exact call-site_ cleanly would otherwise require large branching in the test method.
- Procedural scenarios where the input is inherently a sequence of steps (setup/mutate/call) and cannot reasonably be represented as a pure value.

Constraints for `Action`-based cases:

- The `Action` must **not contain assertions**.
- Prefer using `Action` only for **throws/void/behavioral** cases; do not default to it for normal return-value tests.

#### Standard Definitions

```csharp
// 1. Value/Result Tests (e.g. Converters, Parsers)
public sealed record ValidCase(string Name, string Value, bool Expected)
    : ReturnCase<string, bool>(Name, Value, Expected);

// 2. Exception Tests (Preferred Declarative Pattern)
public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
    : ThrowsCase<string?>(Name, Value, ExpectedException);

// 3. Exception Tests (Exceptional Pattern using Action)
// Use only when capturing the exact call-site matters (overloads/generics/procedural inputs).
public sealed record InvalidActionCase(string Name, Action Value, ExpectedException ExpectedException)
    : ThrowsCase<Action>(Name, Value, ExpectedException);
```

#### Record Formatting (Strict)

Record declarations MUST use **two-line format**:

- **Line 1**: Full `sealed record` declaration with all positional parameters.
- **Line 2**: Inheritance clause, indented by **4 spaces**.

```csharp
public sealed record ValidCase(string Name, DateOnly Value, DateOnly Expected)
    : ReturnCase<DateOnly, DateOnly>(Name, Value, Expected);

public sealed record InvalidCase(string Name, DateOnly Value, ExpectedException ExpectedException)
    : ThrowsCase<DateOnly>(Name, Value, ExpectedException);
```

Do **NOT** put inheritance on the same line as the record declaration, even for short records.

### 4.3 Parameterization Rules

- **One case per line**.
- **No named arguments** in `new(...)` (except named tuples for inputs).
- **No helper factories**.

#### Tuple Inputs (When a method takes multiple parameters)

When the unit under test takes multiple inputs, represent them as a **single named tuple** in the record's `Value` property.

Rules:

- The record property MUST be named `Value` (PascalCase) — matching the `ValueCase<TValue>.Value` base class. Do **NOT** use `Input`, `Arguments`, or any other name.
- Tuple element names MUST be **camelCase**.
- Tuple element names MUST be the **exact parameter names** from the method under test — no shorthand, no renaming, no abbreviation. If the method signature is `IsExactLength(string? value, int length)`, the tuple is `(string? value, int length)`.
- Do not use tuples when there is only one logical input.

Accessing tuple elements via `testCase.Value.min` is acceptable, but in test methods prefer readability:

- Prefer deconstruction: `var (value, min, max) = testCase.Value;`
- Or assign once: `var input = testCase.Value;` then use `input.min`, `input.max`.

Avoid creating separate tuple layers like `Value` + `Arguments` unless the unit-under-test signature already has that separation (e.g., `Foo(value, options)`). If you want conceptual separation in the test, deconstruct into locals rather than nesting tuples.

#### `nameof` + Fixtures for Test Case Names

When input values come from Test Fixtures (§9), use `nameof` for the test case `Name` — zero magic strings:

```csharp
using F = PineGuard.Testing.Fixtures.FooRulesFixtures;

new(nameof(F.IsBar.Valid), F.IsBar.Valid, true)
```

`nameof(F.IsBar.Valid)` returns `"Valid"` — the fixture field name becomes the test case display name. Standard alias convention: `using F = PineGuard.Testing.Fixtures.[Class]Fixtures;`

When the same fixture input appears in multiple cases within a single dataset, disambiguate with interpolation:

```csharp
new InvalidCase($"{nameof(F.Parse.TooShort)}-CustomMessage", F.Parse.TooShort, ..., "Custom")
```

Canonical example:

```csharp
public static class IsBetween
{
    public sealed record Case(string Name, (int value, int min, int max) Value, bool Expected)
        : IsCase<(int value, int min, int max)>(Name, Value, Expected);

    public static TheoryData<Case> ValidCases =>
    [
        new("inside", (value: 5, min: 0, max: 10), true),
    ];

    public static TheoryData<Case> EdgeCases =>
    [
        new("on min", (value: 0, min: 0, max: 10), true),
        new("on max", (value: 10, min: 0, max: 10), true),
    ];
}
```

Second canonical example (range-style):

```csharp
public static class Constructor
{
    public sealed record Case(string Name, (DateTimeOffset start, DateTimeOffset end) Value, TimeSpan Expected)
        : ReturnCase<(DateTimeOffset start, DateTimeOffset end), TimeSpan>(Name, Value, Expected);

    public static TheoryData<Case> ValidCases =>
    [
        new("simple", (start: new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), end: new DateTimeOffset(2020, 1, 2, 0, 0, 0, TimeSpan.Zero)), TimeSpan.FromDays(1)),
    ];
}
```

#### Example Implementation

```csharp
public static class MyMethod
{
    // Define Records
    public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
        : ThrowsCase<string?>(Name, Value, ExpectedException);

    // Define Data
    public static TheoryData<IThrowsCase> InvalidCases =>
    [
        new InvalidCase("null input", null, new ExpectedException(typeof(ArgumentNullException), "input")),
        new InvalidCase("empty input", "", new ExpectedException(typeof(ArgumentException), "input")),
    ];
}
```

### 4.4 Element Ordering Within an Operation Group

Inside each nested `public static class` (Operation Group), elements MUST appear in this exact order:

1. **Dataset properties** — always in this order:
   a. `ValidCases`
   b. `EdgeCases`
   c. `InvalidCases`
2. **Record definitions** — always in this order:
   a. `ValidCase` (or `Case` when only one record type is needed)
   b. `InvalidCase`
   c. `InvalidActionCase` (only if Action-based throws cases are used)

Datasets first, records last. This keeps the data (what agents and reviewers read first) at the top, and the type definitions at the bottom.

### 4.5 Structural Correspondence (TestData ↔ Tests)

The Tests file MUST mirror the TestData file's Operation Group structure:

- For every `public static class Xxx` in `FooTestData`, there MUST be a corresponding `public static class Xxx` in `FooTests`.
- Operation Groups in the Tests file MUST appear in the **same order** as in the TestData file.
- Each Tests Operation Group consumes ONLY the datasets from its corresponding TestData Operation Group.
- No test method may reference datasets from a different Operation Group.

### 4.6 Outer TestData Class Element Ordering

Inside the outer `public static class XxxTestData`, elements MUST appear in this exact order:

1. **Shared fields** — `private static readonly` fields, test doubles, and construction utilities reused across multiple Operation Groups. Placed at the top because Operation Groups depend on them.
2. **Operation Groups** — nested `public static class` per method/feature under test (per §4.1). Ordered to match the source class method order.
3. **Helper methods** — utility methods (e.g., `Enumerate<T>()`, custom enumerator factories) used by shared fields or Operation Groups. Placed at the bottom.

```csharp
public static class CollectionRulesTestData
{
    // 1. Shared fields (reused across Op Groups)
    private static readonly IEnumerable<int>? NullEnumerable = null;
    private static readonly IEnumerable<int> EmptyCountEnumerable = [];
    private static readonly IEnumerable<int> NonEmptyCountEnumerable = [1, 2, 3];

    // 2. Operation Groups
    public static class IsEmpty { /* ... */ }
    public static class HasItems { /* ... */ }
    public static class Contains { /* ... */ }

    // 3. Helper methods (at bottom)
    public static IEnumerable<T> Enumerate<T>(params T[] items) { /* ... */ }
}
```

When a TestData class has no shared fields or helpers, only Operation Groups appear.

## 5. Test Class Pattern

### 5.1 Structure

- Outer class `XxxTests` must be `sealed` and inherit `BaseUnitTest` via primary constructor.
- Outer class must NOT contain test methods (enforces semantics).
- Use nested `public static class` for each Operation Group.
- Test methods must be `public static void`.

#### Method Naming Convention (Strict)

| Scenario | MemberData Datasets | Method Name |
| :--- | :--- | :--- |
| Valid only | `ValidCases` | `Valid_BehavesAsExpected` |
| Valid + Edge | `ValidCases` + `EdgeCases` | `ValidAndEdge_BehavesAsExpected` |
| Valid + Edge + Invalid (same record type) | `ValidCases` + `EdgeCases` + `InvalidCases` | `ValidEdgeAndInvalid_BehavesAsExpected` |
| Invalid (throws) | `InvalidCases` | `Invalid_ThrowsAsExpected` |

Do **NOT** use `ShouldReturnExpected`, `ShouldThrowExpected`, `ReturnsExpected`, or any other naming pattern.

> **Migration note**: Some existing test files may still use a flat pattern where test methods appear directly in the outer class without nested Operation Groups. This is legacy drift. All **new** test files and all **refactored** files MUST use the nested Operation Group pattern defined above.

### 5.2 Example Implementation

```csharp
public sealed class MyClassTests(ITestOutputHelper output) : BaseUnitTest(output)
{
    public static class MyMethod
    {
        [Theory]
        [MemberData(nameof(MyClassTestData.MyMethod.ValidCases), MemberType = typeof(MyClassTestData.MyMethod))]
        [MemberData(nameof(MyClassTestData.MyMethod.EdgeCases), MemberType = typeof(MyClassTestData.MyMethod))]
        public static void ValidAndEdge_BehavesAsExpected(MyClassTestData.MyMethod.ValidCase testCase)
        {
            // Act
            var result = MyClass.MyMethod(testCase.Value);

            // Assert
            Assert.Equal(testCase.Expected, result);
        }

        [Theory]
        [MemberData(nameof(MyClassTestData.MyMethod.InvalidCases), MemberType = typeof(MyClassTestData.MyMethod))]
        public static void Invalid_ThrowsAsExpected(IThrowsCase testCase)
        {
            // Arrange
            var t = (ThrowsCase<string?>)testCase;
            var value = t.Value;

            // Act & Assert
            var ex = Assert.Throws(testCase.ExpectedException.Type, () => MyClass.MyMethod(value));
            ThrowsCaseAssert.Expected(ex, testCase);
        }
    }
}
```

## 6. Helper Types (Reference)

- **`IsCase<T>` / `HasCase<T>`**: Boolean-returning rules.
- **`ReturnCase<TValue, TExpected>`**: Value-returning methods. Bridge: `Expected => Expected`.
- **`TryCase<TValue, TOut>`**: Try-pattern methods.
- **`ThrowsCase<T>`**: Exception-throwing scenarios (supports `T=Action` for exceptional cases only!!).
- **`ExpectedException`**: Use positional args only: `new(typeof(ArgumentException), "paramName")`.

## 7. Determinism & Best Practices

- Avoid `DateTime.Now` (use passed-in `DateTime`/`TimeProvider` or strict offsets).
- Avoid `Environment.CurrentDirectory`.
- Gate OS-specific tests: `if (!OperatingSystem.IsWindows()) return;`.

## 8. Full Canonical Examples

These examples show a **complete, file-level** Fixtures + TestData + Tests trio for a hypothetical `FooRules` class. Agents MUST produce code that matches this structure exactly.

### 8.1 Complete Fixtures File

Fixtures provide **input constants only** — no records, no datasets. Each project's TestData references these values and wraps them in layer-specific records.

```csharp
namespace PineGuard.Testing.Fixtures;

public static class FooRulesFixtures
{
    public static class IsBar
    {
        public static readonly string? Valid      = "bar";
        public static readonly string? Null       = null;
        public static readonly string? Empty      = "";
        public static readonly string? Whitespace = "   ";
    }

    public static class IsBaz
    {
        public static readonly (string? value, int threshold) AboveThreshold = ("abc", 2);
        public static readonly (string? value, int threshold) AtThreshold    = ("ab", 2);
        public static readonly (string? value, int threshold) BelowThreshold = ("a", 2);
        public static readonly (string? value, int threshold) NullValue      = (null, 0);
    }

    public static class Parse
    {
        public static readonly string? SingleDigit = "5";
        public static readonly string? Negative    = "-3";
        public static readonly string? Zero        = "0";
        public static readonly string? MaxInt      = "2147483647";
        public static readonly string? Null        = null;
        public static readonly string? NonNumeric  = "abc";
    }
}
```

### 8.2 Complete TestData File

References fixtures via `nameof` + alias `F`. Each project defines its **own records and datasets** — fixtures provide input values only.

```csharp
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.FooRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class FooRulesTestData
{
    public static class IsBar
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(F.IsBar.Valid), F.IsBar.Valid, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new(nameof(F.IsBar.Null), F.IsBar.Null, false),
            new(nameof(F.IsBar.Empty), F.IsBar.Empty, false),
            new(nameof(F.IsBar.Whitespace), F.IsBar.Whitespace, false)
        ];

        // InvalidCases omitted — IsBar is a pure boolean predicate, never throws

        public sealed record Case(string Name, string? Value, bool Expected)
            : IsCase<string?>(Name, Value, Expected);
    }

    public static class IsBaz
    {
        public static TheoryData<Case> ValidCases =>
        [
            new(nameof(F.IsBaz.AboveThreshold), F.IsBaz.AboveThreshold, true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new(nameof(F.IsBaz.AtThreshold), F.IsBaz.AtThreshold, true),
            new(nameof(F.IsBaz.BelowThreshold), F.IsBaz.BelowThreshold, false),
            new(nameof(F.IsBaz.NullValue), F.IsBaz.NullValue, false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        public sealed record Case(string Name, (string? value, int threshold) Value, bool Expected)
            : IsCase<(string? value, int threshold)>(Name, Value, Expected);
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new(nameof(F.Parse.SingleDigit), F.Parse.SingleDigit, 5),
            new(nameof(F.Parse.Negative), F.Parse.Negative, -3)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new(nameof(F.Parse.Zero), F.Parse.Zero, 0),
            new(nameof(F.Parse.MaxInt), F.Parse.MaxInt, int.MaxValue)
        ];

        public static TheoryData<IThrowsCase> InvalidCases =>
        [
            new InvalidCase(nameof(F.Parse.Null), F.Parse.Null, new ExpectedException(typeof(ArgumentNullException), "input")),
            new InvalidCase(nameof(F.Parse.NonNumeric), F.Parse.NonNumeric, new ExpectedException(typeof(FormatException)))
        ];

        public sealed record ValidCase(string Name, string? Value, int Expected)
            : ReturnCase<string?, int>(Name, Value, Expected);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);
    }
}
```

### 8.3 Complete Tests File

Tests reference TestData only — never fixtures directly. Unchanged from previous versions.

```csharp
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class FooRulesTests(ITestOutputHelper output) : BaseUnitTest(output)
{
    public static class IsBar
    {
        [Theory]
        [MemberData(nameof(FooRulesTestData.IsBar.ValidCases), MemberType = typeof(FooRulesTestData.IsBar))]
        [MemberData(nameof(FooRulesTestData.IsBar.EdgeCases), MemberType = typeof(FooRulesTestData.IsBar))]
        public static void ValidAndEdge_BehavesAsExpected(FooRulesTestData.IsBar.Case testCase)
        {
            // Act
            var result = FooRules.IsBar(testCase.Value);

            // Assert
            Assert.Equal(testCase.Expected, result);
        }
    }

    public static class IsBaz
    {
        [Theory]
        [MemberData(nameof(FooRulesTestData.IsBaz.ValidCases), MemberType = typeof(FooRulesTestData.IsBaz))]
        [MemberData(nameof(FooRulesTestData.IsBaz.EdgeCases), MemberType = typeof(FooRulesTestData.IsBaz))]
        public static void ValidAndEdge_BehavesAsExpected(FooRulesTestData.IsBaz.Case testCase)
        {
            // Arrange
            var (value, threshold) = testCase.Value;

            // Act
            var result = FooRules.IsBaz(value, threshold);

            // Assert
            Assert.Equal(testCase.Expected, result);
        }
    }

    public static class Parse
    {
        [Theory]
        [MemberData(nameof(FooRulesTestData.Parse.ValidCases), MemberType = typeof(FooRulesTestData.Parse))]
        [MemberData(nameof(FooRulesTestData.Parse.EdgeCases), MemberType = typeof(FooRulesTestData.Parse))]
        public static void ValidAndEdge_BehavesAsExpected(FooRulesTestData.Parse.ValidCase testCase)
        {
            // Act
            var result = FooRules.Parse(testCase.Value!);

            // Assert
            Assert.Equal(testCase.Expected, result);
        }

        [Theory]
        [MemberData(nameof(FooRulesTestData.Parse.InvalidCases), MemberType = typeof(FooRulesTestData.Parse))]
        public static void Invalid_ThrowsAsExpected(IThrowsCase testCase)
        {
            // Arrange
            var t = (FooRulesTestData.Parse.InvalidCase)testCase;

            // Act & Assert
            var ex = Assert.Throws(testCase.ExpectedException.Type, () => FooRules.Parse(t.Value!));
            ThrowsCaseAssert.Expected(ex, testCase);
        }
    }
}
```

### 8.4 Structural Rules Demonstrated

| Rule | Where Demonstrated |
| :--- | :--- |
| Fixtures provide input constants only (§9) | `FooRulesFixtures` — raw values, no records, no datasets |
| Fixture alias convention (§4.3) | `using F = PineGuard.Testing.Fixtures.FooRulesFixtures;` |
| `nameof` for test case Name (§4.3) | `nameof(F.IsBar.Valid)` returns `"Valid"` |
| Zero magic strings in TestData (§9) | Every `new(...)` uses `nameof` + fixture reference |
| Datasets before records (§4.4) | Every Operation Group in TestData |
| Dataset order: Valid → Edge → Invalid (§4.4) | `IsBar`, `IsBaz`, `Parse` |
| Empty dataset syntax (§4.1) | `IsBar.InvalidCases => []` |
| Two-line record format (§4.2) | All record definitions |
| Tuple property named `Value`, not `Input` (§4.3) | `IsBaz.Case` — `(string? value, int threshold) Value` |
| Tuple elements camelCase, exact param names (§4.3) | `IsBaz.Case` — `value`, `threshold` match method signature |
| Tuple deconstruction in test (§4.3) | `IsBaz.ValidAndEdge_BehavesAsExpected` |
| Nested static classes mirror TestData (§4.5) | `IsBar`, `IsBaz`, `Parse` in both files |
| Same group order in both files (§4.5) | `IsBar` → `IsBaz` → `Parse` |
| Method naming convention (§5.1) | `ValidAndEdge_BehavesAsExpected`, `Invalid_ThrowsAsExpected` |
| `public static void` methods (§5.1) | All test methods |
| Outer class sealed + primary constructor (§2.1) | `FooRulesTests(ITestOutputHelper output)` |
| Outer class has NO test methods (§5.1) | All tests live in nested classes |
| `MemberData` with `nameof` + `MemberType` (§5.2) | All `[MemberData]` attributes |
| Throws pattern: cast → extract → assert → verify (§5.2) | `Parse.Invalid_ThrowsAsExpected` |
| AAA section markers (§1) | `// Arrange`, `// Act`, `// Assert`, `// Act & Assert` |

## 9. Test Fixtures (Shared Input Constants)

### 9.1 Purpose

Test Fixtures provide **shared, reusable input constants** for validations tested across multiple layers (Core → Must → Guard → Fluent → Data). Fixtures hold **input values only** — no records, no datasets, no test infrastructure. Each project's TestData owns its records and datasets.

### 9.2 Location & Naming

| Aspect | Convention |
| :--- | :--- |
| **Folder** | `tests/PineGuard.Testing/Fixtures/` |
| **Namespace** | `PineGuard.Testing.Fixtures` |
| **Class name** | `[CoreRulesClassName]Fixtures` — mirrors the Core Rules class where the validation originates |
| **Inner class** | Matches the Core Rules method name (e.g., `IsExactLength`, `IsBetween`, `Parse`) |
| **Alias** | `using F = PineGuard.Testing.Fixtures.[Class]Fixtures;` — standard alias in TestData files |

### 9.3 Field Conventions

- **Declaration**: `public static readonly` — named tuple for multi-param methods, raw type for single-param methods.
- **Tuple element names**: **camelCase**, **exact parameter names** from the method under test (per §4.3).
- **Field names**: **PascalCase** — descriptive, concise. They double as test case display names via `nameof`.
- **Field names MUST be unique** within each Operation Group — they become xUnit display names.
- **No records, no datasets** — fixtures are pure data. Each project's TestData defines its own records and datasets.

### 9.4 `nameof` Pattern

Use `nameof` for the test case `Name` property — zero magic strings:

```csharp
using F = PineGuard.Testing.Fixtures.FooRulesFixtures;

// Standard pattern: nameof(F.OpGroup.Field), F.OpGroup.Field, expectedOutput
new(nameof(F.IsBar.Valid), F.IsBar.Valid, true)
```

`nameof(F.IsBar.Valid)` returns `"Valid"` — the fixture field name IS the test case display name.

When the same fixture input appears in multiple cases within a single dataset (e.g., default vs custom message paths), disambiguate:

```csharp
new InvalidCase($"{nameof(F.Parse.TooShort)}-CustomMessage", F.Parse.TooShort, ..., "Custom")
```

### 9.5 Layer Consumption

Fixtures define **validation inputs only**. Layer-specific concerns (expected messages, exception types, return values) remain in each project's TestData.

| Layer | How Fixtures Are Used |
| :--- | :--- |
| **Core** | TestData references fixture values for IsCase/ReturnCase/ThrowsCase records |
| **Must** | TestData references fixture values; adds `ExpectedMessage` in edge/invalid records |
| **Guard** | TestData references fixture values; uses ReturnCase (valid) + ThrowsCase (invalid) |
| **Fluent** | TestData destructures fixture tuples into model value + config params |
| **Data** | TestData references fixture values as `object?` for attribute validation |

### 9.6 When NOT to Use Fixtures

- **PineGuard.Testing.UnitTests** — testing the framework itself; no cross-layer validation.
- **One-off utility tests** — utilities not shared across multiple layers.
- **Layer-specific edge cases** — e.g., Guard's custom `exceptionCreator` path uses inline values, not fixtures.

### 9.7 What Cannot Be Fixtures

Fixtures provide **data** inputs only. The following MUST remain inline in TestData:

- **Functional inputs** — `Func<T, bool>` predicates, `Action` delegates, lambda expressions.
  These are runtime closures that cannot be `static readonly` constants.
- **Comparer instances** — `IEqualityComparer<T>`, `StringComparer.*`. Reference directly
  in TestData (e.g., `StringComparer.OrdinalIgnoreCase`).
- **Test doubles** — Custom types like `ReadOnlyCollectionOnly<T>` that implement interfaces
  for testing. Define in TestData shared fields (§4.6).
- **Regex instances** — `new Regex(...)` objects. Define as `private static readonly` in the
  Op Group or shared fields section.
- **Computed/dynamic values** — `DateOnly.FromDateTime(DateTime.Now)`. Fixtures MUST use
  deterministic static constants.

## 10. GOLD-STANDARD Compliance

### 10.1 Definition

The **GOLD-STANDARD** is the target quality level for all PineGuard test classes. It combines structural correctness, dataset completeness, and verified coverage.

### 10.2 Tiers

| Tier | Criteria |
|------|----------|
| **GOLD** | All 3 datasets exist and are populated (or legitimately empty with justification). 100% line+branch coverage confirmed. |
| **SILVER** | Structure correct, all datasets populated or justified, coverage not yet verified. |
| **BRONZE** | Structure correct, some datasets still scaffolded (`=> [];` without justification). |
| **SCAFFOLD** | Empty test data, no coverage verification. |

### 10.3 Empty Dataset Policy

**Do not include empty datasets.** If a method has no edge cases, omit `EdgeCases` entirely. If it never throws, omit `InvalidCases`. Empty arrays (`=> [];`) fail static analysis tools and add noise.

Common reasons to omit a dataset:

| Omitted Dataset | When To Omit |
|----------------|-------------|
| `InvalidCases` | Method returns null/false instead of throwing (null-return, Try*, pure boolean) |
| `InvalidCases` | Immutable record constructor with no validation logic |
| `EdgeCases` | Boundaries already covered in `ValidCases` or `InvalidCases` |
| `EdgeCases` | Pure type conversion or comparison with no boundary conditions |
| `ValidCases` | Only edge/boundary behaviour is meaningful (rare) |

### 10.4 EdgeCase Requirements

EdgeCases should reference constants and statics from Core classes where applicable:
- `Rules/` classes for validation boundaries (min/max lengths, format patterns)
- `Utils/` classes for utility boundaries

### 10.5 Tracking

Compliance is tracked in `docs/ai/specs/testing/gold-standard.md`. Update the index when:
- New test classes are added
- Empty arrays are populated or justified
- Coverage is verified for a project
