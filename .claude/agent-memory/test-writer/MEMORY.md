# Test Writer Memory

> **Role:** `docs/ai/roles/verifier.md` (Verifier)
> Directives: Trust Nothing, Coverage Matters, Isolation, Test the Contract, CI-Ready Evidence.
> Constraints: No modifying `src/` just for assertions. No flaky tests.

## Learned Patterns

### Test Class Structure (per `docs/ai/specs/testing/unit-test.md` §5)
- `sealed class` inheriting `BaseUnitTest` via primary constructor
- Namespace mirrors source: `PineGuard.X.UnitTests` for `PineGuard.X`
- Naming: `[SubjectClassName]Tests` (e.g., `MustBoolClausesTests`)
- Outer class must NOT contain test methods — use nested `public static class` per Operation Group (§5.1)
- Test methods must be `public static void`
- Method naming (strict §5.1): `Valid_BehavesAsExpected`, `ValidAndEdge_BehavesAsExpected`, `Invalid_ThrowsAsExpected`
- Place `XxxTests.cs` and `XxxTestData.cs` side-by-side in mirrored folder

### Test Data Pattern (Nested Operation Groups, per §4)
- Outer class: `public static class [SubjectClassName]TestData`
- Nested class per operation: `[SubjectClassName]TestData.[MethodName]`
- Element ordering within each group (§4.4): datasets first (`ValidCases` → `EdgeCases` → `InvalidCases`), then records (`ValidCase`/`Case` → `InvalidCase`)
- Record type per scenario: inherits from `RuleCase<T>`, `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase` (v2 case records)
- Access expected value via `testCase.Expected` (NOT `testCase.ExpectedReturn`)
- Dataset properties return `TheoryData<T>` — all three must exist even if empty (`=> []`)
- Outer TestData class ordering (§4.6): shared fields → Op Groups → helper methods (at bottom)
- Structural correspondence (§4.5): Tests file nested classes mirror TestData in same order
- Tuple property MUST be named `Value` (PascalCase) — matches `ValueCase<TValue>.Value`. NEVER use `Input`.
- Tuple element names MUST be camelCase and MUST be the **exact parameter names** from the method under test (§4.3)
- DO NOT hardcode data arrays in test methods
- DO NOT use named arguments in test case records — use named tuples for multi-input
- Use Test Fixtures (`PineGuard.Testing.Fixtures/`) for shared input constants (§10)
- Alias: `using F = PineGuard.Testing.Fixtures.[Class]Fixtures;`
- Use `nameof(F.OpGroup.Field)` for test case Name — zero magic strings
- Fixtures = raw values ONLY (no records, no datasets) — each project defines its own

### MemberData Pattern
```csharp
[Theory]
[MemberData(nameof(MustBoolClausesTestData.True.ValidCases), MemberType = typeof(MustBoolClausesTestData.True))]
[MemberData(nameof(MustBoolClausesTestData.True.EdgeCases), MemberType = typeof(MustBoolClausesTestData.True))]
public static void ValidAndEdge_BehavesAsExpected(MustBoolClausesTestData.True.ValidCase testCase)
```

### Full Canonical Examples
- See §9 of `docs/ai/specs/testing/unit-test.md` for complete file-level TestData + Tests pair
- Shows predicate tests, tuple input tests, value+throws tests with all structural rules demonstrated

### Expected Property Naming

- All test case records use `Expected` (NOT `ExpectedReturn`)
- `testCase.Expected` is the uniform access pattern across all layers

### Layer-Specific Expected Types

| Layer | Expected type | Assertion |
| :--- | :--- | :--- |
| Core | `bool` | `Assert.Equal(testCase.Expected, result)` |
| Must | `MustExpected` | `Assert.Equal(testCase.Expected.IsValid, result.Success)` |
| Guard (valid) | `string?` (passthrough) | `Assert.Equal(testCase.Expected, result)` |
| Guard (throws) | `ExpectedException` (via ThrowsCase) | `ThrowsCaseAssert.Expected(ex, testCase)` |
| Fluent | `FluentExpected` | `Assert.Equal(testCase.Expected.IsValid, result.IsValid)` |
| DA | `bool` | `Assert.Equal(testCase.Expected, result == ValidationResult.Success)` |

### Composite Expected Records

```csharp
// MustExpected — for MustClauses layer
public sealed record MustExpected(bool IsValid, string? Message = null, string? ParamName = null);

// FluentExpected — for FluentValidation layer
public sealed record FluentExpected(bool IsValid, string? Message = null);
```

- `IsValid` is the uniform boolean on all composite types
- Asserting message/paramName: only when `is not null` (conditional assertion pattern)

### Assertion Patterns
- Core: `Assert.Equal(testCase.Expected, result)`
- Must (IsValid): `Assert.Equal(testCase.Expected.IsValid, result.Success)`
- Must (Message): `if (testCase.Expected.Message is not null) Assert.Equal(testCase.Expected.Message, result.Message)`
- Must (ParamName): `if (testCase.Expected.ParamName is not null) Assert.Equal(testCase.Expected.ParamName, result.ParamName)`
- Guard valid: `Assert.Equal(testCase.Expected, result)` (passthrough — Expected = the input value)
- Guard throws: `var ex = Assert.Throws(testCase.ExpectedException.Type, () => ...); ThrowsCaseAssert.Expected(ex, testCase)`
- Fluent (IsValid): `Assert.Equal(testCase.Expected.IsValid, result.IsValid)`
- Fluent (Message): `if (testCase.Expected.Message is not null) Assert.Equal(testCase.Expected.Message, result.Errors[0].ErrorMessage)`
- DA: `Assert.Equal(testCase.Expected, result == ValidationResult.Success)`

### Coverage Requirements
- 100% line AND branch coverage for every class
- Test null inputs, empty strings, whitespace, min/max values, edge cases
- Test both `true` and `false` paths for every condition
- Config parameter null tests (attribute failure to config param name, not value)

### Common Mistakes to Avoid
- DO NOT skip null input tests (every nullable param needs null test case)
- DO NOT forget branch coverage (every if/else needs both paths tested)
- DO NOT use ad-hoc patterns — follow the spec EXACTLY
- DO NOT put TestData inline in test methods
- DO NOT forget to test CallerArgumentExpression propagation

## Fixture Architecture v2

Reference: `docs/ai/specs/testing/fixture.md`
Conventions: `docs/ai/rules/fixture-conventions.md`

### New Type Hierarchy

- `IExpectedResult { bool IsValid }` — universal interface
- `RuleExpected(bool IsValid)` — Rules/Core layer (replaces raw `bool`)
- `MustExpected` → now extends `ReturnExpected(IsValid, Message)`
- `FluentExpected` → now extends `ReturnExpected(IsValid, Message)`, adds `PropertyName`
- `DataAnnotationExpected(IsValid, Message?, MemberName?)` → extends `ReturnExpected`
- `GuardExpected(IsValid, ExceptionType?, ParamName?, MessageContains?)` → extends `ThrowExpected`
- `RuleScenario<TInputs>(Name, Inputs, IsValid)` — fixture scenario record with `.IsNull` helper

### New Case Records

- `RuleCase<T>` replaces `IsCase<T>` / `HasCase<T>` (which are `[Obsolete]`)
- `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase`

### Extension Methods (Fixture → TheoryData)

- `.ToRuleCases()`, `.ToMustCases(Func?)`, `.ToGuardCases(string?)`, `.ToFluentCases(Func?)`, `.ToDataAnnotationCases(Func?)`
- Filter: `.WhereValid()`, `.WhereInvalid()`, `.Except(names)`, `.Only(names)`

### Base Test Classes

- `BaseRuleUnitTest`, `BaseMustUnitTest`, `BaseGuardUnitTest`, `BaseFluentUnitTest`, `BaseDataAnnotationUnitTest`
- Each provides `AssertResult(tc, result)` — uniform assertion pattern

### v2 Test Pattern (replaces nested Op Groups in Tests files)

- Flat test classes: `sealed class`, no nested `public static class` in Tests
- Method naming: `MethodName_BehavesAsExpected(XxxCase<T> tc)`
- TestData still uses nested Op Groups with `.ToXxxCases()` projections
- Zero comments in code
- Single-line entries (max 400 chars)
- Edge case constants MUST reference Rule class constants
- Partial fixture files mirror Rule partial structure

### CallerArgumentExpression + v2 Tests: Local Variable Pattern

When a Must method uses `[CallerArgumentExpression(nameof(value))]`, calling `Must.Be.Foo(tc.Value.value, ...)` captures `"tc.Value.value"` as the paramName, producing messages like `"tc.Value.value must be ..."` — NOT `"value must be ..."`.

Fix: extract a local variable with the exact parameter name before calling the method:
```csharp
{ var value = tc.Value.value; var result = Must.Be.Foo(value, tc.Value.other); AssertResult(tc, result); }
```
This makes CallerArgumentExpression capture just `"value"`, giving `"value must be ..."` in the message. See `MustTimeOnlyClausesTests.cs` for the canonical pattern — EVERY multi-param tuple test uses this pattern.

### Reflection-Based Generic Methods (OfType, etc.)

When invoking generic Must methods via reflection with explicit `null` paramName:
- `MustResult.FormatMessage` leaves `{paramName}` unreplaced in the message
- Set `Message = null` in `MustExpected` to skip message assertion for these cases
- Still assert `IsValid` via the `(bool)result.Success` cast on `dynamic`

## Guard Non-Nullable Fixture Mapping Pattern

When fixture scenarios use `DateTimeOffset?` (nullable) but guard methods take `DateTimeOffset` (non-nullable):
- DO NOT use `.ToGuardCases()` directly on nullable fixture arrays
- Create `new RuleScenario<DateTimeOffset>[]` inline, unpacking `.Value` from fixture constants
- Use the factory overload: `.ToGuardCases(_ => new GuardExpected(...))`
- Example: `new RuleScenario<DateTimeOffset>[] { new(nameof(F.IsPast.PastDate), F.IsPast.PastDate!.Value, true) }.ToGuardCases(_ => new GuardExpected(true))`
- For tuples: unpack each non-null component using `F.Fixture.Field.component!.Value`

### Guard Inversion Rule (CRITICAL)
`Guard.Against.X` calls `Must.Be.Y` (complement). Logic:
- Guard PASSES (ValidCase): when `Must.Be.Y` SUCCEEDS on the input
- Guard THROWS (InvalidCase): when `Must.Be.Y` FAILS on the input
- e.g. `Guard.Against.FutureOrPresent` calls `Must.Be.Past` → PASSES for past values (Must.Be.Past succeeds), THROWS for future values (Must.Be.Past fails)

### Guard Precision Mismatch Warning
Guard methods for Before/After/Same typically call Must methods with fixed inclusion (Exclusive for strict, Inclusive for On*) and precision=null. Do NOT use fixture scenarios that test precision unless the guard explicitly passes precision. Using `SameInstantInclusive` for `Guard.Against.OnOrAfter` (which calls `Must.Be.Before` with Exclusive) correctly expects the guard to THROW (same instant is not strictly before).

### GuardDateTimeClausesTestData Pre-Existing Issues (fixed Mar 2026)
- Missing `using PineGuard.Testing.UnitTests.Rules;` → `.Except()` and `.Project()` not found
- 4-arg `GuardCase` constructor in collection initializers → fix to tuple syntax: `new("name", (value, days), expected)`

### Guard Positive Variant Pattern (Char/TypeOnly/etc.)
- Positive guard (e.g. `Control`) = complement of corresponding Negative guard (`NotControl`)
- `NotControl.ValidCases` = `AllValid.ToGuardCases()` → `Control.ValidCases` = `AllInvalid.Except(Null).ToGuardCases(_ => new GuardExpected(true))`
- `NotControl.InvalidCases` = `AllInvalid.Except(Null).ToGuardCases("value")` → `Control.InvalidCases` = `AllValid.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"))`
- Rule: positive variants swap Valid/Invalid datasets and flip expected GuardExpected

### Guard String DateOnly / TimeOnly: Non-Fixture Inline Pattern
- String-typed guard methods with no matching StringRulesFixtures group → use inline string literals in `Cases` dataset
- Pattern from GuardStringTimeOnlyClausesTestData: single `Cases` property (not `ValidCases`/`InvalidCases`)
- For null string → `typeof(ArgumentNullException)`, for non-null invalid → `typeof(ArgumentException)`
- Non-nullable string method params (e.g. `ChronologicalDateOnly(string start, ...)`) → use `start!` null-forgiving in test call, null case in data throws ANE
- Return value not asserted for string-based guard tests (unlike typed guards where `Assert.Equal(value, result)` is used)

### Missing `using PineGuard.Testing.UnitTests.Rules;` for `.Except()` Extension Method
- `.Except(string name)` is a custom extension method from `PineGuard.Testing.UnitTests.Rules` namespace
- Required in any TestData file that calls `.Except(nameof(...))` on fixture scenario arrays
- Common mistake: adding `.Except()` without this import → CS1929 error
- Files that use `.Except()` MUST have `using PineGuard.Testing.UnitTests.Rules;`
- Check existing `MustHttpClausesTestData.cs` as the canonical example with this import

## Topic Files
- (none yet — will grow organically)
