---
spec:
  id: pineguard.ai.must-clauses.project-spec
  title: "PineGuard.MustClauses Project Spec"
  version: 2
  template:
    - ../template-project.md
  parent:
    - ../project.md
  dependencies:
    - ../../dependencies.md
applies_to:
  - "src/PineGuard.MustClauses/**"
  - "src/PineGuard.Core/MustClauses/**"
---

# PineGuard.MustClauses Project Spec

This document is the **source-of-truth instruction set** for generating and maintaining **production MustClauses code** in this repository.

**Inheritance**: Inherits from docs/ai/specs/project.md.

---

## What MustClauses are

**MustClauses** provide fluent “validation/parse” helpers that return `MustResult<T>`.

Typical usage:

```csharp
var result = Must.Be.NotNullOrEmpty(value);

if (result)
{
    var normalized = result.Result;
}
else
{
    Console.WriteLine(result.Message);
}
```

Semantics (required):

- `MustResult<T>.Value` is the **original input** value.
- `MustResult<T>.Result` is the **typed output** (parsed / normalized) when successful.
- Must clause methods should prefer **returning failures** (not throwing). Consumers can call `ThrowIfFailed()` / `OrThrow()` when needed.

---

## Source layout (must follow)

### Core types (do not reimplement)

The foundational types live in src/PineGuard.Core/MustClauses/\*\*:

- `IMustClause`
- `MustClause`
- `Must` (exposes `Must.Be`)
- `MustResult<T>`

### Must clause implementations (this spec covers these)

Implementations live in src/PineGuard.MustClauses/\*\* and are mostly extension methods on `IMustClause`.

Folder conventions (strict):

- Everything can live at src/PineGuard.MustClauses/ unless/until a domain folder is defined.

---

## Non-negotiables

- Do not change behavior of existing public APIs unless explicitly requested.
- MustClauses must never throw for “invalid value” (return `MustResult.Fail(...)` instead).
  - Exception: genuinely exceptional conditions (e.g., platform APIs throwing unexpectedly) may bubble, but prefer avoiding them by using `TryXxx` APIs.
- Every failure message must be a stable, user-readable sentence.
- All messages must use the `{paramName}` token.

Validated value vs configuration/dependency parameters (required):

- If a failure is due to a **configuration/dependency parameter** (regex/predicate/provider/etc), attribute the failure to that parameter using `nameof(failingParam)` (do not use the caller-argument-expression name for `value`).

---

## Naming & file conventions (strict)

### Namespace

All MustClauses live in:

```csharp
namespace PineGuard.MustClauses;
```

### File names and classes

- One file per domain: MustXxxClauses.cs
- One public static class per file: `public static class MustXxxClauses`

Required invariants:

- File name must match the public class name exactly.
- MustClauses class names must never use `*Extension` / `*Extensions` naming.

Required placement (strict):

- Place files under the appropriate domain folder if one is defined, otherwise at the project root.

---

## Method naming

- Use **PascalCase** verbs/adjectives: `NotNull`, `Positive`, `Email`.
- Prefer names that match existing PineGuard naming and read well in fluent form: `Must.Be.<Method>(...)`.

### Opposites / negations (Not*/Non*)

PineGuard prefers **positive, intention-revealing naming** in MustClauses.

Rules:

- Prefer semantic names over `Not*` when the complement is ambiguous (boundary values, tri-states, etc).
- Add `Not*` only when it is a strict boolean complement and it adds real value.
- Do not introduce `Non*`/`Invalid*` naming in MustClauses; use `Not*` and curated semantic opposites instead.
- Negations must not treat invalid input as success (i.e., they must preserve the same “input validity” preconditions as the positive method).

---

## Signature conventions (strict)

### Clause method receiver

All Must clause methods must be extension methods on `IMustClause`:

```csharp
public static MustResult<T> Something(
    this IMustClause _,
    /* value, other args */,
    [CallerArgumentExpression(nameof(value))] string? paramName = null)
```

Rules:

- Receiver parameter name must be `_`.
- The validated input parameter must be named `value`.
- The last parameter must be `paramName` populated using `[CallerArgumentExpression(nameof(value))]`.

### Nullability

MustClauses use a **hybrid nullability strategy** (Rule07):

- **Primary validated value (reference types)**: use nullable inputs.
  - Example: `string? value`, `object? value`.
- **Primary validated value (struct/value types)**: use non-nullable inputs.
  - Example: `DateOnly value`, `TimeOnly value`, `Guid value`.

Semantics (required):

- **Null is invalid by default**.
  - If an input is nullable (`string?` / `T?`), then `null` must fail unless the method name/intent explicitly treats null as valid.
- **Null-accepting methods must encode intent in the name**.
  - Use the `NullOrX` pattern when null is a valid success case.

Result typing rules (required):

- For `NullOrX` methods on string inputs, return `MustResult<T?>` and treat `value is null` as success with `Result = null`.
- For non-NullOrX methods, null must fail with the method’s canonical message.

Secondary (configuration) parameters:

- Secondary parameters may be nullable **when the underlying Core rule/API is nullable** (e.g., `int? precision = null`).
- **Configuration/Reference parameters** (e.g., `string reference`, `string minimum`) must be **non-nullable** if the underlying Core/Rule logic requires a value. Do not use `string?` simply to avoid compiler warnings; enforce the contract by requiring a non-null value.
- Do not add overloads that differ only by nullability.

Note (integrations):

- Adapter layers (FluentValidation/DataAnnotations) may accept nullable property types and “skip on null”, but MustClauses are the canonical semantic validators.

---

## Parameter validation order (required)

To avoid misleading semantic failures (e.g., “must be within range” when the caller passed an invalid range), MustClauses must validate **programmer misuse** first.

Required structure inside each MustClause method:

1. Guard secondary parameters (ranges, enum-like parameters, `window`, `precision`, masks, etc.) that represent programmer misuse.
2. Declare `const string messageTemplate = "{paramName} ...";`.
3. Validate the primary value.
4. Evaluate the semantic rule and return via `FromBool(...)`.

Failure attribution for secondary parameters (required):

- When a failure is caused by a secondary parameter, the `MustResult` must attribute the failure to that parameter:
  - `paramName` passed to `Fail(...)` must be `nameof(failingParam)`.
  - `value` passed to `Fail(...)` must be the failing parameter’s value.

Example:

```csharp
public static MustResult<int> Between(
    this IMustClause _,
    int value,
    int min,
    int max,
    Inclusion inclusion = Inclusion.Inclusive,
    [CallerArgumentExpression(nameof(value))] string? paramName = null)
{
    if (min > max)
        return MustResult<int>.Fail("{paramName} requires a valid range.", nameof(min), min);

    const string messageTemplate = "{paramName} must be within the expected range.";

    var ok = NumberRules.IsBetween(value, min, max, inclusion);
    return MustResult<int>.FromBool(ok, messageTemplate, paramName, value, value);
}
```

---

## MustString\*.cs (string-in, typed-out) generation rules

MustString clauses parse/normalize string inputs into typed results.

### Naming & location (strict)

- File name: MustString{ParsedType}Clauses.cs
- Class name: `public static class MustString{ParsedType}Clauses`
- Location: src/PineGuard.MustClauses/ (unless a domain folder is explicitly required)

### Required semantic contract

- `MustResult<T>.Value` must always contain the original `string? value`.
- `MustResult<T>.Result` must contain the parsed/normalized value on success.

### Parse once, validate typed, return typed (required)

MustString clauses must follow this exact flow:

1. Parse exactly once using a `PineGuard.Utils.StringUtility.*` method.
2. If parsing fails, return failure with the method’s canonical message.
3. If parsing succeeds, validate using typed `PineGuard.Rules.*Rules` methods.
4. Return `MustResult<TParsed>.FromBool(ok, messageTemplate, paramName, value, parsedValue)`.

Canonical structure:

```csharp
const string messageTemplate = "{paramName} must ... .";

if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed))
    return MustResult<DateTimeOffset>.FromBool(false, messageTemplate, paramName, value);

var parsedValue = parsed.GetValueOrDefault();
var ok = DateTimeOffsetRules.IsInPast(parsedValue);
return MustResult<DateTimeOffset>.FromBool(ok, messageTemplate, paramName, value, parsedValue);
```

Notes:

- Many `StringUtility.*.TryParse` methods use nullable out parameters (e.g., `DateTimeOffset?`). After a successful parse, convert to non-null via `parsed.GetValueOrDefault()`.

### Null-handling for MustString methods

See the `### Nullability` section.

If a MustString method explicitly allows null as valid, it must use the `NullOrX` naming pattern and return a nullable typed result (`MustResult<T?>`) with `Result = null` on `value is null`.

### Numeric generic rule calls (required; prevents CS0411)

Some numeric rules are generic and accept nullable `T?` (e.g. `NumberRules.IsPositive<T>(T? value)`).

When calling these from MustString numeric clauses, always provide an explicit type argument:

```csharp
var ok = NumberRules.IsPositive<decimal>(parsed);
```

---

## Output expectations

When asked to add a new MustClause feature, produce:

- One or more new/updated MustXxxClauses.cs files under src/PineGuard.MustClauses/.
- Methods that compile under net8.0 with nullable enabled.
- No unit tests (unless explicitly requested in a separate task).
