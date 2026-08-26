---
name: utils-layer-test-pattern
description: How to test Utils-layer (and any non-Rule/Must/Guard/Fluent/DA "Other") static classes — plain BaseUnitTest, ReturnCase/ThrowsCase (not RuleCase), and the _BehavesAsExpected/_ThrowsAsExpected split for methods that both return and throw.
metadata:
  type: feedback
---

Core `Utils/` classes (and anything else outside the five layered stacks) are tested against plain
`BaseUnitTest`, not `BaseRuleUnitTest` — `RuleCase<T>`/`RuleExpected` are Core **Rules**-only
(`docs/ai/specs/core/unit-test.md` §2). Use the primary-constructor + wrapped-colon form from
`unit-test.md` §2.1's "(Other)" row:

```csharp
public sealed class FooUtilityTests(ITestOutputHelper output)
    : BaseUnitTest(output)
```

### Case shapes for Utils methods
- Value-returning method (no `out` param): `ReturnCase<TValue, TExpected>` — `Expected` is just the
  return type (e.g. `string`), not a composite `RuleExpected`/`MustExpected` record.
- `Try*` pattern (`bool` + `out`): `TryCase<TValue, TOut>`.
- Throwing method (e.g. `ThrowHelper.ThrowIfNull` preconditions): `ThrowsCase<TValue>` +
  `ExpectedException` + `ThrowsCaseAssert.Expected(ex, testCase)` — same primitives as Core's
  documented `ThrowIfNull` exception pattern.

### Split test methods when a method both returns and can throw
Follow the sanctioned exception documented in `unit-test.md` §8.3 for `FooRules.Parse` — generalize
it to any Utils method with both `ValidCases` and `InvalidCases`:
- `Xxx_BehavesAsExpected(ValidCase testCase)` → `Assert.Equal(testCase.Expected, result)`.
- `Xxx_ThrowsAsExpected(InvalidCase testCase)` → `Assert.Throws(testCase.ExpectedException.Type, () => ...)`
  then `ThrowsCaseAssert.Expected(exception, testCase)`.
Use concrete case types (`TheoryData<InvalidCase>`), not `TheoryData<IThrowsCase>` — the interface +
cast form is only needed when one dataset mixes multiple concrete case types.

### Best current precedents (read these, not the older Utils/*Tests.cs files)
- `tests/PineGuard.Core.UnitTests/Codes/MustCodesTests.cs` — cleanest fully-flat "(Other)" example.
- `tests/PineGuard.Core.UnitTests/Common/EnumerationTestData.cs` (`IntConstructor`, `FromName` groups)
  and `Common/ThrowHelperTests.cs` — `ReturnCase`/`ThrowsCase` + `ThrowsCaseAssert` end-to-end.
- Most existing `tests/PineGuard.Core.UnitTests/Utils/*Tests.cs` files (e.g. `CollectionUtilityTests`,
  `StringUtilityTests`, `FilePathUtilityTests`) still use the pre-v2 `_ReturnsExpected` naming and
  `IsCase<T>`/no-`ITestOutputHelper`-ctor style — that's drift from before the flat-v2 migration, not
  a pattern to copy for new files.

### Functional-input fixtures (`Func<T,TResult>`, `LambdaExpression`, etc.)
Per `unit-test.md` §9.7 these can never be fixtures. Declare them as `private static readonly` fields
— but scope them to the specific nested Operation Group class that uses them, not the outer TestData
class's "shared fields" section (§4.6), unless the value/type is genuinely reused by more than one
group (e.g. a small sample record type used to build several `Expression<Func<T,TProp>>` cases for
one `FromExpression`-style group — that sample type *does* belong in the outer shared-fields section
since multiple cases within (and potentially across) groups reference it).

Useful trick for asserting a "must NOT invoke this callback" short-circuit contract: pass a
`Func<...>` that throws `InvalidOperationException` when called, then assert the expected
short-circuited result. If the SUT wrongly invokes the callback, the test fails loudly via the
unexpected exception instead of silently passing.

### CallerArgumentExpression is a non-issue here
`fixture.md` §12.1's "extract `tc.Value` to a local first" warning only applies when the method under
test itself re-exposes `[CallerArgumentExpression]` on its own public parameter (the Guard-layer
pattern). When a Utils method just internally calls `ThrowHelper.ThrowIfNull(param)` without
re-declaring `[CallerArgumentExpression]` on its own signature, the captured `paramName` is fixed at
that internal call site at compile time (e.g. always `"property"` for `PropertyPathUtility.Combine`'s
`ThrowHelper.ThrowIfNull(property)`) regardless of how the test invokes the outer method — no special
handling needed, `ExpectedException`'s `ParamName` can just be the literal source parameter name.
