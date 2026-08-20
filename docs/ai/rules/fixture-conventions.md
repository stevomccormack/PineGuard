> Inherits from: `docs/ai/rules/global.md`

# Fixture Architecture v2 — Code Conventions

These conventions apply to ALL code generated during the fixture migration.

## 1. Structural Comments Only

No ad-hoc or explanatory comments. Structural comments that provide navigation value ARE allowed and encouraged:

- `// Arrange`, `// Act`, `// Assert` — test phase markers
- `// Guard.Against.NotInteger` — method-under-test identification
- `// CsvRules.IsCsvLine` — section headers in TestData files

Do NOT add: `// valid input`, `// this checks the length`, `// returns true when valid`, or any other inline explanation.

## 2. Single-Line Formatting (Max 400 Characters)

- Fixture field declarations: one field per line
- RuleScenario entries: `new(nameof(Field), Field, true),` — single line
- TestData switch cases: `nameof(F.X.Y) => new MustExpected(false, "msg"),` — single line
- MemberData attributes: full attribute on one line
- TheoryData entries: `new("name", value, expected),` — single line

## 3. Edge Case Constants from Source Classes

When a Rule, Standards, or Utils class defines `const` or `static readonly` boundary values, fixture edge cases MUST reference them:

```csharp
// Rule constants
public static readonly double AtMin = GeoLocationRules.MinLatitude;
public static readonly double BelowMin = GeoLocationRules.MinLatitude - 0.0001;

// Utils constants
public static readonly int AtMaxEmail = EmailUtility.MaxEmailLength;
```

NEVER hardcode boundary values that exist as constants in Rules or Utils.

**Known constant locations** (not exhaustive — always read the Rule source to find references):

| Class | Constants |
|---|---|
| `GeoLocationRules` | `MinLatitude`, `MaxLatitude`, `MinLongitude`, `MaxLongitude` |
| `PhoneRules` | `DefaultMinDigits`, `DefaultMaxDigits`, `DefaultAllowedNonDigitCharacters` |
| `CharRules` | `AsciiMinValue`, `AsciiMaxValue`, `PrintableAsciiMinValue`, `PrintableAsciiMaxValue` |
| `SqlDateTimeRules` | `MinValue`, `MaxValue` |
| `BufferRules` | `Base64CharsPerQuantum`, `Base64BytesPerQuantum` |
| `HttpSecurityHeaderRules` | `DefaultStrictTransportSecurityMinMaxAgeSeconds`, 7 default value constants |
| `CsvRules` | `DefaultCsvSeparator` |
| `StringRules` | `SignedIntegerPattern`, `DefaultAllowedDigitSeparators` |
| `OwaspRegex` | ~23 pattern constants across nested classes |
| `EmailUtility` | `MaxEmailLength`, `MaxLocalPartLength`, `MaxDomainLength` |

## 4. Flat Test Classes

No nested `public static class` Operation Groups in test files. Use `sealed class` with method-per-rule:

```csharp
public sealed class CsvRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(CsvRulesTestData.IsCsvLine.Cases), MemberType = typeof(CsvRulesTestData.IsCsvLine))]
    public void IsCsvLine_BehavesAsExpected(RuleCase<string?> tc) { ... }
}
```

Method naming: `MethodName_BehavesAsExpected` — the method name provides the grouping.

> This matches `docs/ai/specs/testing/unit-test.md` §4.5/§5.1: the Tests file mirrors the TestData
> Operation Groups **as test methods, not as nested classes**. TestData files keep their Op Groups
> (see §9 below); only the Tests file is flat. `docs/ai/specs/testing/fixture.md` carries the canonical example.

## 5. Partial Fixture Classes

Fixture files mirror the source Rules file naming exactly: `XxxRules.Yyy.cs` gets `XxxRulesFixtures.Yyy.cs`. If `StringRules` is split across `StringRules.cs`, `StringRules.Casing.cs`, `StringRules.Numbers.cs`, the fixture is split across matching partials:

```
StringRulesFixtures.cs
StringRulesFixtures.Casing.cs
StringRulesFixtures.Numbers.cs
```

A monolithic fixture file standing alone beside a partial Rules class is drift — fold it into the matching partial. Because all partials share one class scope, inner group names must stay unique across the set: the groups in `StringRulesFixtures.Numbers.cs` carry the partial's qualifier (`NumbersIsPositive`, `NumbersIsInRange`, …).

## 6. Naming Precision

| Term | Meaning | Suffix |
|---|---|---|
| Scenario | `RuleScenario<T>` array | `Scenarios` (e.g., `ValidScenarios`) |
| Case | `TheoryData<XxxCase>` property | `Cases` (e.g., `ValidCases`, `Cases`) |
| Expected | Layer-specific expected record | `Expected` (e.g., `MustExpected`) |

## 7. camelCase Tuple Elements

Tuple element names MUST be camelCase and MUST be the exact parameter names from the method under test:

```csharp
// Method: IsExactLength(string? value, int length)
// Fixture: (string? value, int length) Matching = ("abc", 3);
// RuleScenario<(string? value, int length)>
```

## 8. Fixture Alias Convention

```csharp
using F = PineGuard.Testing.Fixtures.[RulesClass]Fixtures;
```

All test case Names use `nameof(F.OpGroup.Field)` — zero magic strings.

## 9. TestData Op Groups Still Required

TestData files still use nested `public static class` per method. Only the Tests file is flat.

## 10. Ad-Hoc Cases

Layer-specific cases not derivable from RuleScenarios (e.g., custom message overrides, type-mismatch tests) use `AdHocCases` property with inline values:

```csharp
public static TheoryData<DataAnnotationCase> AdHocCases =>
[
    new("int-value", 42, new DataAnnotationExpected(true)),
];
```

## 11. Guard Test Methods — CallerArgumentExpression

Guard methods use `[CallerArgumentExpression(nameof(value))]`. Passing `tc.Value` directly captures the expression `"tc.Value"` as the paramName, which breaks paramName assertions. Always extract to a local variable first:

```csharp
// Non-tuple (string, string?, int, etc.):
public void NotCamelCase_BehavesAsExpected(GuardCase<string> tc)
{
    var value = tc.Value;
    var result = AssertResult(tc, () => Guard.Against.NotCamelCase(value));
    if (tc.Expected.IsValid) Assert.Equal(value, result);
}

// Tuple ((string? value, StringCasing style), etc.):
public void NotCaseStyle_BehavesAsExpected(GuardCase<(string? value, StringCasing style)> tc)
{
    var value = tc.Value.value;
    var style = tc.Value.style;
    var result = AssertResult(tc, () => Guard.Against.NotCaseStyle(value, style));
    if (tc.Expected.IsValid) Assert.Equal(value, result);
}
```

## 12. Guard TestData — Inverted Guard Classes

Some guard methods are "inverted" (Guard.Against.CamelCase throws when value IS camelCase). Their TestData must use explicit factory to produce correct GuardExpected:

```csharp
// WRONG — ToGuardCases() on InvalidScenarios (IsValid=false) → GuardExpected(false) with null ExceptionType → crash:
public static TheoryData<GuardCase<string>> ValidCases => F.IsCamelCase.InvalidScenarios.ToGuardCases();

// CORRECT — explicit factory:
public static TheoryData<GuardCase<string>> ValidCases => F.IsCamelCase.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
public static TheoryData<GuardCase<string>> InvalidCases => F.IsCamelCase.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
```

For nullable variants with NullValue (UpperInvariant, LowerInvariant):
```csharp
public static TheoryData<GuardCase<string?>> ValidCases => F.IsUpperInvariant.InvalidScenarios.Except(nameof(F.IsUpperInvariant.NullValue)).ToGuardCases(_ => new GuardExpected(true));
public static TheoryData<GuardCase<string?>> InvalidCases => F.IsUpperInvariant.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
public static TheoryData<GuardCase<string?>> NullCases => F.IsUpperInvariant.InvalidScenarios.Only(nameof(F.IsUpperInvariant.NullValue)).ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentNullException), "value"));
```

## 13. Guard TestData — Tuple NullValue Fields

`RuleScenario<T>.IsNull` checks if the entire `T` is null — not fields within it. For tuple inputs like `(string? value, StringCasing style)`, `IsNull` is always false even when `value` is null. Use explicit factory switch in these cases:

```csharp
// NotCaseStyle.InvalidCases — tuple input, NullValue field must be ANE not AE:
public static TheoryData<GuardCase<(string? value, StringCasing style)>> InvalidCases =>
    F.IsCaseStyle.InvalidScenarios
        .Except(nameof(F.IsCaseStyle.UnknownStyle))
        .ToGuardCases(s => s.Name switch
        {
            nameof(F.IsCaseStyle.NullValue) => new GuardExpected(false, typeof(ArgumentNullException), "value"),
            _ => new GuardExpected(false, typeof(ArgumentException), "value")
        });
```

Non-inverted guard InvalidCases with simple `string?` inputs can still use `.ToGuardCases("value")` auto-logic (IsNull works correctly for non-tuple types).
