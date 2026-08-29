---
spec:
  id: pineguard.ai.specs.testing.unit-tests
  title: "PineGuard Unit Tests (Global Spec)"
  version: 11
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "tests/**"
  - "src/**" # when adding test-focused hooks/visibility helpers
---

# PineGuard Unit Tests (Global Spec)

This is the **shared, cross-cutting unit test specification** for PineGuard.

All unit test specs (per domain/project) should treat this file as the baseline and only describe what’s different or additional for their scope.

Coverage workflow is documented in `docs/ai/specs/testing/coverage.md`.

Related specs in this folder:

- `docs/ai/specs/testing/fixture.md` — the fixture / scenario architecture. It is **authoritative** for fixtures, `Expected` types, layer `Case` records and dataset construction; where it differs from §4, §5 or §8 below, fixture.md wins.
- `docs/ai/specs/testing/gold-standard.md` — the compliance index that tracks each test project against this spec.

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

  **"(Other)" means a package outside the five layers above — a new adapter package with no
  layer base of its own (e.g. `PineGuard.Extensions.Options`, `PineGuard.AspNetCore`).** Such a
  project inherits `BaseUnitTest` directly, defines a project-local `XxxExpected` (extending
  `ReturnExpected`/`ThrowExpected`) and `XxxCase` (extending `ReturnCase<,>`) pair for the result
  type it asserts, and keeps that pair in the test project rather than promoting it into
  `PineGuard.Testing` — promotion only happens once a second project needs the same family (§3
  rule 1 of `docs/ai/specs/testing/project.md`). Precedent: `tests/PineGuard.DataAnnotations.UnitTests/ThrowsCase.cs`.
  This is the only case where inheriting `BaseUnitTest` directly is correct; every one of the
  five layers above always uses its own base class.

  All classes use **primary constructor** syntax, with the inheritance clause wrapped per §1:
  ```csharp
  public sealed class BoolRulesTests(ITestOutputHelper output)
      : BaseRuleUnitTest(output)
  ```

- **`UseCulture(...)`**: Use only if explicitly testing culture-specific behavior.
- **Additional helpers**: `UseEnvironmentVariable(key, value)`, `CreateDeterministicRandom(seed)`, `CreateCancelledToken()`.
- **Layer-specific addenda**: Each layer has its own spec addendum that overrides patterns here —
  `docs/ai/specs/core/unit-test.md`, `docs/ai/specs/must-clauses/unit-test.md`, `docs/ai/specs/guard-clauses/unit-test.md`,
  `docs/ai/specs/fluent-validation/unit-test.md`, `docs/ai/specs/data-annotations/unit-test.md` (scope-id → directory map:
  `spec.md` §11.2).

### 2.2 PineGuard.Testing — Shared Test Infrastructure Library

`PineGuard.Testing` (`tests/PineGuard.Testing/`) is the **shared test helper library** referenced by all `*.UnitTests` projects. It is exercised directly by `tests/PineGuard.Testing.UnitTests/` and indirectly by every other `*.UnitTests` run.

**Namespaces**: `PineGuard.Testing.Common`, `PineGuard.Testing.UnitTests`, `PineGuard.Testing.UnitTests.{Rules, MustClauses, GuardClauses, FluentValidation, DataAnnotations}`, `PineGuard.Testing.Fixtures`.

**Roots & case bases**

| Type | Purpose |
| :--- | :--- |
| `BaseUnitTest` | Abstract base for all test classes. Enforces `InvariantCulture`. |
| `BaseCase` | Root abstract record; provides `Name` and `ToString()`. |
| `ValueCase<TValue>` | Case with a single `Value` input. |
| `ReturnCase<TValue, TExpected>` | Case for value-returning methods; exposes `Expected`. |
| `ReturnOutCase<TValue, TExpected, TOut>` | Case for methods with both a return value and an `out` parameter. |
| `TryCase<TValue, TOut>` | Case for Try-pattern methods (`bool` return + `out TOut`). |
| `ThrowsCase<TValue>` | Case for exception-throwing scenarios; supports `TValue = Action` for procedural inputs. |
| `ThrowsCaseAssert` | Asserts `ThrowsCase` expectations against a caught exception. |
| `ExpectedException` | Positional record: `new(typeof(ArgumentException), "paramName", "messageContains")`. |
| `IThrowsCase` | Interface used for `TheoryData<IThrowsCase>` datasets. |
| `IReturnsCase<TExpected>` | Interface for return-value cases. |
| `IReturnsOutCase<TExpected, TOut>` | Interface for out-value cases. |

**Expected hierarchy** (see `fixture.md` §1 for the full tree)

| Type | Shape |
| :--- | :--- |
| `IExpectedResult` | `bool IsValid` — the uniform success flag on every `Expected` type. |
| `ReturnExpected` | `(bool IsValid, string? Message = null)` — abstract base for result-returning layers. |
| `ThrowExpected` | `(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null)` — abstract base for throwing layers. |
| `RuleExpected` | `(bool IsValid)` — Core rules. |
| `MustExpected` | `(bool IsValid, string? Message = null, string? ParamName = null, string? Code = null)` — Must layer. |
| `MustValidationExpected` | `(bool IsValid, string? Message = null, int? FailureCount = null, string? PropertyPath = null, string? Code = null)` — `MustValidator<T>` object validation. |
| `GuardExpected` | `(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null, string? Code = null)` — Guard layer. |
| `FluentExpected` | `(bool IsValid, string? Message = null, string? PropertyName = null, string? Code = null)` — Fluent layer. |
| `DataAnnotationExpected` | `(bool IsValid, string? Message = null, string? MemberName = null, string? Code = null)` — Data Annotations layer. |

`Code` is a trailing optional parameter on every layer's `Expected` type (added alongside the `MustCodes`
catalogue — see `docs/ai/specs/must-clauses/project.md` "Error codes"). Set it only on the representative
spot-check cases; the base test class's `AssertResult`/`AssertThrow` asserts it only when the expectation
carries one.

**Layer case records, scenarios & extensions**

| Type | Purpose |
| :--- | :--- |
| `RuleCase<TValue>` / `MustCase<TValue>` / `GuardCase<TValue>` / `FluentCase<TValue>` / `DataAnnotationCase` | Sealed per-layer case records pairing a `Value` with the layer's `Expected`. |
| `MustValidationCase<TValue>` | `(string Name, TValue Value, MustValidationExpected Expected)` — pairs an object with the whole-object validation result it should produce. |
| `RuleScenario<TInputs>` | `(string Name, TInputs Inputs, bool IsValid)` — the layer-neutral scenario a fixture publishes. |
| `RuleScenarioExtension` | Filter combinators (`WhereValid`, `WhereInvalid`, `Except`, `Only`), `Project`, and `.ToRuleCases()`. |
| `MustScenarioExtension` / `GuardScenarioExtension` / `FluentScenarioExtension` / `DataAnnotationScenarioExtension` | `.ToMustCases()`, `.ToGuardCases()`, `.ToFluentCases()`, `.ToDataAnnotationCases()` — scenario arrays to `TheoryData`. |
| `MustValidationScenarioExtension` | `.ToMustValidationCases<T>()` — mirrors `GuardScenarioExtension`, for `IMustValidator<T>`-driven scenarios. |
| `BaseMustValidationUnitTest` | Abstract base with `AssertResult<TValue>(MustValidationCase<TValue>, MustValidationResult)` — asserts `IsValid`, then `FailureCount`, then the first failure's `PropertyPath`/`Code`/`Message` when the expectation carries them. |
| `*Fixtures` (in `Fixtures/`) | Shared input constants and their scenario arrays for cross-layer validations (§9). |

**Superseded**

| Type | Status |
| :--- | :--- |
| `IsCase<TValue>` | Superseded — use `RuleCase<TValue>`. Annotated `[Description("Use RuleCase<T> for rules.")]`; soft-deprecated only, so it still compiles without warning and remains in use at many derivation sites. |
| `HasCase<TValue>` | Superseded — use `RuleCase<TValue>` (same soft-deprecation). |

### 2.3 Folder Structure

Mirror the source layout exactly so navigation is obvious:

- Source: `src/<Library>/<Subfolders>/<File>.cs`
- Tests: `tests/<Library>.UnitTests/<Subfolders>/<File>Tests.cs`
- Shared helpers: `tests/PineGuard.Testing/` _(helper library; its own tests live in `tests/PineGuard.Testing.UnitTests/`)_

## 3. File Structure (Strict)

For each unit under test (e.g., `MyClass`), maintain exactly two files:

1. `MyClassTestData.cs` (Data definitions — records, datasets, references to fixtures)
2. `MyClassTests.cs` (Test execution)

Shared input constants live in `PineGuard.Testing/Fixtures/` (see §9). TestData files reference these fixtures; they never duplicate the raw values.

## 4. Canonical TestData Pattern

### 4.1 Structure

Define `public static class XxxTestData` containing nested **Operation Groups** for each method/feature under test.

**The dataset model is layer-specific** — each layer addendum is normative for its own layer:

| Layer | Datasets per Operation Group | Addendum |
|-------|------------------------------|----------|
| Core | single `Cases` rollup | `docs/ai/specs/core/unit-test.md` |
| MustClauses | `ValidCases` + `InvalidCases` (no `EdgeCases`) | `docs/ai/specs/must-clauses/unit-test.md` |
| GuardClauses | `ValidCases` + `InvalidCases` | `docs/ai/specs/guard-clauses/unit-test.md` |
| FluentValidation | single `Cases` rollup | `docs/ai/specs/fluent-validation/unit-test.md` |
| DataAnnotations | single `Cases` rollup | `docs/ai/specs/data-annotations/unit-test.md` |

The `Cases` rollup is built from a fixture's scenario arrays (§9, `fixture.md` §2) via
`AllScenarios` + `.ToXxxCases()`: `Cases` (`TheoryData<RuleCase<T>>`, `TheoryData<MustCase<T>>`, …)
carries the whole scenario set for that member. Where a split is used, the dataset names are
`ValidCases`, `EdgeCases`, `InvalidCases` — never any other names.

Notes:

- **Only include datasets that have test cases.** Omit a dataset entirely when no valid cases exist for that category (e.g., omit `InvalidCases` for pure boolean validators that never throw; omit `EdgeCases` when boundaries are already covered in `ValidCases`).
- Do **not** leave empty scaffolding (`=> [];`). Empty arrays fail static analysis (SonarQube, inspections) and add noise.
- For **non-throwing predicate** methods (e.g., `Is*`, `Has*` rules that return `false` for bad input), omit `InvalidCases` and put “bad-but-not-throwing” inputs (e.g., `null`, whitespace, wrong length, unknown codes) in `EdgeCases`.

### 4.2 Case Records

Scenario-backed groups need no records at all — they use the sealed layer case records (`RuleCase<T>`, `MustCase<T>`, …) directly. Everything else defines its case records inside its own Operation Group.

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
    public static TheoryData<RuleCase<(int value, int min, int max)>> ValidCases =>
    [
        new("inside", (value: 5, min: 0, max: 10), new RuleExpected(true)),
    ];

    public static TheoryData<RuleCase<(int value, int min, int max)>> EdgeCases =>
    [
        new("on min", (value: 0, min: 0, max: 10), new RuleExpected(true)),
        new("on max", (value: 10, min: 0, max: 10), new RuleExpected(true)),
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

The Tests file MUST mirror the TestData file's Operation Group structure — as **test methods**, not as nested classes (§5.1):

- For every `public static class Xxx` in `FooTestData`, there MUST be a corresponding `Xxx_BehavesAsExpected` test method in `FooTests`.
- Test methods MUST appear in the **same order** as their Operation Groups in the TestData file.
- Each test method consumes ONLY the datasets from its corresponding TestData Operation Group.
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

- Test class `XxxTests` must be `sealed` and inherit the **layer-specific base from §2.1** via primary constructor.
- The test class is **flat**: test methods are declared directly on it. Do **NOT** nest `public static class` Operation Groups inside the Tests file — Operation Groups exist in the TestData file only, and each one is consumed by exactly one test method (§4.5).
- Test methods must be **instance** methods declared `public void` — the layer base supplies `AssertResult`.
- Each test method takes a single layer case parameter named `tc` (`RuleCase<T>`, `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase`) and delegates its assertion to `AssertResult(tc, result)`.

#### Method Naming Convention (Strict)

One method per Operation Group, named after the member under test:

```
public void <MemberUnderTest>_BehavesAsExpected(<Layer>Case<T> tc)
```

| Scenario | MemberData Datasets | Method Name |
| :--- | :--- | :--- |
| Fixture rollup | `Cases` | `<MemberUnderTest>_BehavesAsExpected` |
| Split datasets | any of `ValidCases`, `EdgeCases`, `InvalidCases` (stacked `[MemberData]` attributes) | `<MemberUnderTest>_BehavesAsExpected` |

Where two Operation Groups exercise overloads of the same member, suffix the group and method name with the distinguishing argument shape (e.g. `IsCsvRowLineSchema_BehavesAsExpected`, `IsCsvRowLineHeaderTypes_BehavesAsExpected`).

Do **NOT** use `ShouldReturnExpected`, `ShouldThrowExpected`, `ReturnsExpected`, or any other naming pattern.

Throwing behaviour is **not** a separate method: the Guard layer expresses it through `GuardExpected` and `AssertResult(tc, () => act())`. Only tests that are outside the layer-base architecture (e.g. `ThrowHelper` in Core) still use raw `ThrowsCase` / `ThrowsCaseAssert` datasets.

### 5.2 Example Implementation

```csharp
public sealed class FooRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FooRulesTestData.IsBar.Cases), MemberType = typeof(FooRulesTestData.IsBar))]
    public void IsBar_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = FooRules.IsBar(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FooRulesTestData.IsBaz.Cases), MemberType = typeof(FooRulesTestData.IsBaz))]
    public void IsBaz_BehavesAsExpected(RuleCase<(string? value, int threshold)> tc)
    {
        // Arrange
        var (value, threshold) = tc.Value;

        // Act
        var result = FooRules.IsBaz(value, threshold);

        // Assert
        AssertResult(tc, result);
    }
}
```

## 6. Helper Types (Reference)

- **`RuleCase<T>` / `MustCase<T>` / `GuardCase<T>` / `FluentCase<T>` / `DataAnnotationCase`**: the per-layer case records — the default choice (§2.2).
- **`IsCase<T>` / `HasCase<T>`**: superseded predecessors for boolean-returning rules; use `RuleCase<T>` instead.
- **`ReturnCase<TValue, TExpected>`**: Value-returning methods; exposes `Expected`.
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

Fixtures provide **input constants and the `RuleScenario<T>[]` arrays derived from them** — no case records, no `TheoryData`. Each project's TestData turns those scenarios into its own layer's cases via `.ToXxxCases()`.

```csharp
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class FooRulesFixtures
{
    public static class IsBar
    {
        public static readonly string? Valid      = "bar";
        public static readonly string? Null       = null;
        public static readonly string? Empty      = "";
        public static readonly string? Whitespace = "   ";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Valid), Valid, true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(Null), Null, false),
            new(nameof(Empty), Empty, false),
            new(nameof(Whitespace), Whitespace, false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsBaz
    {
        public static readonly (string? value, int threshold) AboveThreshold = ("abc", 2);
        public static readonly (string? value, int threshold) AtThreshold    = ("ab", 2);
        public static readonly (string? value, int threshold) BelowThreshold = ("a", 2);
        public static readonly (string? value, int threshold) NullValue      = (null, 0);

        public static RuleScenario<(string? value, int threshold)>[] ValidScenarios =>
        [
            new(nameof(AboveThreshold), AboveThreshold, true)
        ];

        public static RuleScenario<(string? value, int threshold)>[] ValidEdgeScenarios =>
        [
            new(nameof(AtThreshold), AtThreshold, true)
        ];

        public static RuleScenario<(string? value, int threshold)>[] InvalidScenarios =>
        [
            new(nameof(NullValue), NullValue, false)
        ];

        public static RuleScenario<(string? value, int threshold)>[] InvalidEdgeScenarios =>
        [
            new(nameof(BelowThreshold), BelowThreshold, false)
        ];

        public static RuleScenario<(string? value, int threshold)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(string? value, int threshold)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(string? value, int threshold)>[] AllScenarios => [.. AllValid, .. AllInvalid];
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

`IsBar` is a **format** rule — two scenario arrays plus `AllScenarios`. `IsBaz` has a numeric boundary parameter, so it is a **boundary** rule — four arrays plus the rollups. `Parse` throws, so it publishes constants only and its TestData builds `ThrowsCase` datasets by hand. The selection rule is stated in `fixture.md` §2.

### 8.2 Complete TestData File

Scenario-backed groups project the fixture arrays through `.ToRuleCases()`. Groups that fall outside the scenario architecture (throwing methods, non-boolean return values) still define their **own records and datasets** and reference fixture constants via `nameof` + alias `F`.

```csharp
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.FooRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class FooRulesTestData
{
    public static class IsBar
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsBar.AllScenarios.ToRuleCases();
    }

    public static class IsBaz
    {
        public static TheoryData<RuleCase<(string? value, int threshold)>> Cases => F.IsBaz.AllScenarios.ToRuleCases();
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

Tests reference TestData only — never fixtures directly. The class is flat: one test method per Operation Group, in the same order (§4.5, §5.1).

```csharp
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using PineGuard.Testing.UnitTests.Rules;
using Xunit;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class FooRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FooRulesTestData.IsBar.Cases), MemberType = typeof(FooRulesTestData.IsBar))]
    public void IsBar_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = FooRules.IsBar(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FooRulesTestData.IsBaz.Cases), MemberType = typeof(FooRulesTestData.IsBaz))]
    public void IsBaz_BehavesAsExpected(RuleCase<(string? value, int threshold)> tc)
    {
        // Arrange
        var (value, threshold) = tc.Value;

        // Act
        var result = FooRules.IsBaz(value, threshold);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(FooRulesTestData.Parse.ValidCases), MemberType = typeof(FooRulesTestData.Parse))]
    [MemberData(nameof(FooRulesTestData.Parse.EdgeCases), MemberType = typeof(FooRulesTestData.Parse))]
    public void Parse_BehavesAsExpected(FooRulesTestData.Parse.ValidCase tc)
    {
        // Act
        var result = FooRules.Parse(tc.Value!);

        // Assert
        Assert.Equal(tc.Expected, result);
    }

    [Theory]
    [MemberData(nameof(FooRulesTestData.Parse.InvalidCases), MemberType = typeof(FooRulesTestData.Parse))]
    public void Parse_ThrowsAsExpected(IThrowsCase tc)
    {
        // Arrange
        var t = (FooRulesTestData.Parse.InvalidCase)tc;

        // Act & Assert
        var ex = Assert.Throws(tc.ExpectedException.Type, () => FooRules.Parse(t.Value!));
        ThrowsCaseAssert.Expected(ex, tc);
    }
}
```

`Parse` is the exception to the uniform shape: it throws and has no layer base to delegate to, so it keeps hand-built `ThrowsCase` datasets and splits into a `_BehavesAsExpected` / `_ThrowsAsExpected` pair. Scenario-backed groups never need that split.

### 8.4 Structural Rules Demonstrated

| Rule | Where Demonstrated |
| :--- | :--- |
| Fixtures hold constants + scenario arrays, never cases (§9) | `FooRulesFixtures` — `readonly` values plus `RuleScenario<T>[]` |
| Format vs boundary scenario shape (`fixture.md` §2) | `IsBar` (2 arrays) vs `IsBaz` (4 arrays + rollups) |
| Fixture alias convention (§4.3) | `using F = PineGuard.Testing.Fixtures.FooRulesFixtures;` |
| `nameof` for test case Name (§4.3) | `nameof(Valid)` in the fixture becomes the case display name |
| Zero magic strings in TestData (§9) | Every `new(...)` uses `nameof` + fixture reference |
| Scenario projection into layer cases (§4.1) | `F.IsBar.AllScenarios.ToRuleCases()` |
| Datasets before records (§4.4) | `Parse` in TestData |
| Dataset order: Valid → Edge → Invalid (§4.4) | `Parse` |
| No empty datasets (§4.1, §10.3) | `IsBar` and `IsBaz` declare only `Cases`; `Parse.EdgeCases` is populated or omitted |
| Two-line record format (§4.2) | All record definitions |
| Tuple property named `Value`, not `Input` (§4.3) | `RuleCase<(string? value, int threshold)>` |
| Tuple elements camelCase, exact param names (§4.3) | `value`, `threshold` match the method signature |
| Tuple deconstruction in test (§4.3) | `IsBaz_BehavesAsExpected` |
| One test method per TestData Operation Group (§4.5) | `IsBar`, `IsBaz`, `Parse` |
| Same group order in both files (§4.5) | `IsBar` → `IsBaz` → `Parse` |
| Method naming convention (§5.1) | `IsBar_BehavesAsExpected`, `IsBaz_BehavesAsExpected` |
| Flat test class, instance `public void` methods (§5.1) | All test methods |
| Test class sealed + primary constructor + layer base (§2.1) | `FooRulesTests(ITestOutputHelper output)` : `BaseRuleUnitTest(output)` |
| Inheritance clause wrapped onto its own line (§1) | `FooRulesTests` declaration |
| Assertion delegated to the layer base (§5.1) | `AssertResult(tc, result)` |
| `MemberData` with `nameof` + `MemberType` (§5.2) | All `[MemberData]` attributes |
| Throws pattern: cast → extract → assert → verify (§5.2) | `Parse_ThrowsAsExpected` |
| AAA section markers (§1) | `// Arrange`, `// Act`, `// Assert`, `// Act & Assert` |

## 9. Test Fixtures (Shared Input Constants)

### 9.1 Purpose

Test Fixtures provide **shared, reusable input constants** for validations tested across multiple layers (Core → Must → Guard → Fluent → Data), together with the `RuleScenario<T>[]` arrays (`ValidScenarios`, `ValidEdgeScenarios`, `InvalidScenarios`, `InvalidEdgeScenarios` and the rollups) derived from those constants. Fixtures hold **no case records and no `TheoryData`** — each project's TestData turns the scenarios into its own layer's cases via `.ToXxxCases()` (`fixture.md` §4).

### 9.2 Location & Naming

| Aspect | Convention |
| :--- | :--- |
| **Folder** | `tests/PineGuard.Testing/Fixtures/` |
| **Namespace** | `PineGuard.Testing.Fixtures` |
| **Class name** | `[CoreRulesClassName]Fixtures` — mirrors the Core Rules class where the validation originates |
| **Inner class** | Matches the Core Rules method name (e.g., `IsExactLength`, `IsBetween`, `Parse`) |
| **Alias** | `using F = PineGuard.Testing.Fixtures.[Class]Fixtures;` — standard alias in TestData files |
| **Partial split** | When the source Rules class is split across `XxxRules.Yyy.cs` files, the fixture class mirrors it one-for-one as `XxxRulesFixtures.Yyy.cs`, each declaring `public static partial class XxxRulesFixtures` |

**Partial split**: `src/PineGuard.Core/Rules/StringRules.Bool.cs` → `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.Bool.cs`, and so on for every partial. Monolithic per-method fixture files are not used. Because all partials contribute to one class, inner class names must be unique across the whole set — prefix them with the partial's sub-scope where a bare method name would collide (e.g. `BoolIsTrue`, `BoolIsFalse` in `StringRulesFixtures.Bool.cs`). The alias in TestData stays `F` and points at the whole partial class, not a single file.

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
| **Core** | TestData projects scenario arrays into `RuleCase<T>` via `.ToRuleCases()`; non-scenario groups reference fixture values for `ReturnCase`/`ThrowsCase` records |
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
| **GOLD** | Only datasets with real cases are present — no `=> [];` anywhere. 100% line+branch coverage confirmed. |
| **SILVER** | Structure correct, no empty datasets, coverage not yet verified. |
| **BRONZE** | Structure correct, some empty `=> [];` datasets remain. |
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

## 11. Enforcement

Two of this spec's rules are machine-checked, not merely conventions:

- §1 — `[Theory]`-only parameterization (no `[Fact]` in `*Tests.cs`)
- §3 — the `*Tests.cs` ↔ `*TestData.cs` pairing (no orphans on either side)

Both are enforced by **audit-cli Rule50** (`Unit Test File Normalization`), which runs in CI and **gates pull requests**. A violation fails the build, so fix it rather than working around it. Legitimate pre-existing exceptions are allowlisted in `tools/audit-cli/test-audit-exceptions.json`.

- Tool spec: `docs/ai/specs/tools/audit-cli/spec.md`
- Agent: `docs/ai/agents/audit-cli.md` (exposed as `/audit-cli`)
- Reproduce locally: `./tools/audit-cli/Run-All.ps1 -RuleId Rule50`

The remaining unit-test rules in this spec (§4, §5, §9) are reviewed by agents and humans, not gated.
