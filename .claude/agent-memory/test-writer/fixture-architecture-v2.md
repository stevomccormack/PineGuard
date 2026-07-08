---
name: fixture-architecture-v2
description: v2 Fixture/Case type hierarchy, extension methods, base test classes, the flat v2 Tests pattern, CallerArgumentExpression local-variable fix, and reflection-based generic Must method handling.
metadata:
  type: feedback
---

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
