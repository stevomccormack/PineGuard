---
spec:
  id: pineguard.ai.guard-clauses.unit-test
  title: "PineGuard.GuardClauses Unit Tests (Addendum)"
  version: 4
  template:
    - ../../meta/template-unit-test.md
  parent:
    - ../../spec.md
    - ../../testing/unit-test.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.GuardClauses/**"
  - "tests/PineGuard.GuardClauses.UnitTests/**"
---

# PineGuard.GuardClauses Unit Tests (Addendum)

This file is a **GuardClauses-specific addendum** to the global unit test spec:

- Global unit test rules: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`

---

## Purpose

Document GuardClauses-specific unit testing guidance only; treat the global unit test spec as the baseline.

## Scope

Prefer testing GuardClauses via the public surface: `Guard.Against.*`

---

## Guard-Specific Patterns

### Base Class

Guard tests inherit **`BaseGuardUnitTest`** (not `BaseUnitTest`):

```csharp
public sealed class GuardBoolClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
```

`BaseGuardUnitTest` provides:

```csharp
protected const string CustomMessage = "Custom guard message.";
protected static TReturn AssertResult<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act)
protected static void AssertCustomMessage<TValue, TReturn>(GuardCase<TValue> testCase, Func<TReturn> act)
```

- If `tc.Expected.IsValid` is `true`: invokes the action and returns the result.
- If `tc.Expected.IsValid` is `false`: asserts the action throws the expected exception type, ParamName, and MessageContains.

### Custom Message Assertion (Required)

Every `Guard.Against.*` clause exposes an optional `message` parameter that overrides the default
`MustResult.Message`. That override is part of the public contract, so **every** guard test method
MUST assert it — immediately after `AssertResult`, repeat the same call with `message: CustomMessage`:

```csharp
var result = AssertResult(tc, () => Guard.Against.False(value));
AssertCustomMessage(tc, () => Guard.Against.False(value, message: CustomMessage));
if (tc.Expected.IsValid) Assert.Equal(value, result);
```

`AssertCustomMessage` no-ops for pass-through (valid) cases and asserts that the thrown exception
message contains `CustomMessage` for throwing cases. Pass `message:` as a **named** argument so the
call site stays valid regardless of the clause's other optional parameters.

Without this assertion the `message ?? result.Message` branch inside each clause is never exercised,
which blocks the 100% branch-coverage target.

### Case Type

Guard tests use **`GuardCase<TValue>`** — no custom record definitions:

```csharp
public sealed record GuardCase<TValue>(string Name, TValue Value, GuardExpected Expected)
    : ReturnCase<TValue, GuardExpected>(Name, Value, Expected);
```

### Expected Type: `GuardExpected`

```csharp
public sealed record GuardExpected(bool IsValid, Type? ExceptionType = null, string? ParamName = null, string? MessageContains = null)
    : ThrowExpected(IsValid, ExceptionType, ParamName, MessageContains);
```

- `new GuardExpected(true)` — valid pass-through case, no exception thrown
- `new GuardExpected(false, typeof(ArgumentException), "value")` — throws `ArgumentException` with ParamName "value"
- `new GuardExpected(false, typeof(ArgumentNullException), "value")` — throws `ArgumentNullException` with ParamName "value"

### Semantic Inversion

Guard methods are **negated** — they guard **against** the condition. A Guard "valid" case is typically a Core Rules "invalid" scenario:

| Scenario | Core `IsTrue` result | Guard `True_BehavesAsExpected` |
|:---|:---|:---|
| `value = false` | `false` (invalid) | `ValidCase` — passes through, no throw |
| `value = true` | `true` (valid) | `InvalidCase` — throws `ArgumentException` |

### TestData Pattern

Two datasets per Op Group — **`ValidCases`** and **`InvalidCases`**:

```csharp
public static TheoryData<GuardCase<bool>> ValidCases  => F.TrueRule.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
public static TheoryData<GuardCase<bool>> InvalidCases => F.TrueRule.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
```

**Note the inversion**: `ValidCases` uses fixture's `InvalidScenarios` (inputs that fail the rule = inputs that should pass the guard); `InvalidCases` uses fixture's `ValidScenarios` (inputs that pass the rule = inputs that should trigger the guard throw).

### Test Structure

- **Flat** — test methods live directly in the outer class, no nested `public static class` per Op Group.
- **Instance methods** — `public void` (not `public static void`).
- **Two `[MemberData]` attributes** per test method — one for `ValidCases`, one for `InvalidCases`.
- **`// Arrange`** section for local variable assignment.
- **`// Act + Assert`** combined — `AssertResult` handles both act and assertion.
- Optional: after `AssertResult`, assert the return value for valid cases.
- Comments inline: `// Guard.Against.MethodName` above each test method.

---

## Canonical Example (BoolRules Guard)

**TestData** (`tests/PineGuard.GuardClauses.UnitTests/GuardBoolClausesTestData.cs`):

```csharp
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.BoolRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardBoolClausesTestData
{
    public static class False
    {
        public static TheoryData<GuardCase<bool>> ValidCases  => F.FalseRule.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<bool>> InvalidCases => F.FalseRule.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
    
    public static class True
    {
        public static TheoryData<GuardCase<bool>> ValidCases  => F.TrueRule.InvalidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<bool>> InvalidCases => F.TrueRule.ValidScenarios.ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }
}
```

**Tests** (`tests/PineGuard.GuardClauses.UnitTests/GuardBoolClausesTests.cs`):

```csharp
using PineGuard.Testing.UnitTests;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardBoolClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardBoolClausesTestData.False.ValidCases), MemberType = typeof(GuardBoolClausesTestData.False))]
    [MemberData(nameof(GuardBoolClausesTestData.False.InvalidCases), MemberType = typeof(GuardBoolClausesTestData.False))]
    public void False_BehavesAsExpected(GuardCase<bool> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.False(value));
        AssertCustomMessage(tc, () => Guard.Against.False(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
    
    [Theory]
    [MemberData(nameof(GuardBoolClausesTestData.True.ValidCases), MemberType = typeof(GuardBoolClausesTestData.True))]
    [MemberData(nameof(GuardBoolClausesTestData.True.InvalidCases), MemberType = typeof(GuardBoolClausesTestData.True))]
    public void True_BehavesAsExpected(GuardCase<bool> tc)
    {
        // Arrange
        var value = tc.Value;

        // Act + Assert
        var result = AssertResult(tc, () => Guard.Against.True(value));
        AssertCustomMessage(tc, () => Guard.Against.True(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
```

---

## Default Test Project

- `tests/PineGuard.GuardClauses.UnitTests/PineGuard.GuardClauses.UnitTests.csproj`

## References

- Global unit test spec: `docs/ai/specs/testing/unit-test.md`
- Fixture architecture: `docs/ai/specs/testing/fixture.md`
- Coverage workflow: `docs/ai/specs/testing/coverage.md`
