---
name: guard-testdata-patterns
description: Guard-layer TestData quirks — string DateOnly/TimeOnly inline pattern and the .Except() extension method import gotcha.
metadata:
  type: feedback
---

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

### Guard Non-Nullable Fixture Mapping Pattern
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
