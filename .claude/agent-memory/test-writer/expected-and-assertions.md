---
name: expected-and-assertions
description: Uniform `Expected` property naming, per-layer Expected types, composite Expected records, and the exact assertion line to use per layer.
metadata:
  type: feedback
---

Reference: `docs/ai/specs/testing/fixture.md`
Conventions: `docs/ai/rules/fixture-conventions.md`

### Expected Property Naming
- All test case records use `Expected` (NOT `ExpectedReturn`)
- `testCase.Expected` is the uniform access pattern across all layers

### Layer-Specific Expected Types

Every Expected type implements `IExpectedResult { bool IsValid }`, and every layer asserts
through the single `AssertResult` helper on its `BaseXxxUnitTest` — never a hand-rolled
`Assert.Equal` chain.

| Layer | Base test class | Expected type | Assertion |
| :--- | :--- | :--- | :--- |
| Core / Rules | `BaseRuleUnitTest` | `RuleExpected(IsValid)` | `AssertResult(tc, result)` |
| Must | `BaseMustUnitTest` | `MustExpected(IsValid, Message?, ParamName?)` | `AssertResult(tc, result)` |
| Guard | `BaseGuardUnitTest` | `GuardExpected(IsValid, ExceptionType?, ParamName?, MessageContains?)` | `AssertResult(tc, () => ...)` |
| Fluent | `BaseFluentUnitTest` | `FluentExpected(IsValid, Message?)` | `AssertResult(tc, result)` |
| DA | `BaseDataAnnotationUnitTest` | `DataAnnotationExpected(IsValid, Message?, MemberName?)` | `AssertResult(tc, result)` |

`GuardExpected` extends `ThrowExpected`; the other four extend `ReturnExpected(IsValid, Message)`.
Guard cases carry both outcomes in one dataset — a valid case asserts the passthrough return, an
invalid case asserts the thrown exception — so both go through the same `AssertResult(tc, () => ...)`
overload.

### Composite Expected Records

```csharp
// MustExpected — for MustClauses layer
public sealed record MustExpected(bool IsValid, string? Message = null, string? ParamName = null);

// FluentExpected — for FluentValidation layer
public sealed record FluentExpected(bool IsValid, string? Message = null);
```

- `IsValid` is the uniform boolean on all composite types
- Message/ParamName/MemberName are asserted by `AssertResult` only when non-null, so leave them
  unset on a case whose message is not the thing under test

### Legacy (do not write — recognise only when reading old files)
- Raw `bool` Expected on Core and DA, and `string?` passthrough Expected on Guard
- `ThrowsCase` + `ExpectedException` + `var ex = Assert.Throws(...); ThrowsCaseAssert.Expected(ex, testCase)`
- Per-layer `Assert.Equal(testCase.Expected..., result...)` lines in place of `AssertResult`

All three are superseded by the v2 hierarchy above. Encountering them is drift to be fixed, not a
pattern to copy.
