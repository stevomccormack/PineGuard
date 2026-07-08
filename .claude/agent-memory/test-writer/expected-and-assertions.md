---
name: expected-and-assertions
description: Uniform `Expected` property naming, per-layer Expected types, composite Expected records, and the exact assertion line to use per layer.
metadata:
  type: feedback
---

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
