---
spec:
  id: pineguard.ai.fluent-validation.project-spec
  title: "PineGuard.FluentValidation Project Spec"
  version: 1
  template:
    - ../../meta/template-project.md
  parent:
    - ../spec.md
  dependencies:
    - ../dependencies.md
applies_to:
  - "src/PineGuard.FluentValidation/**"
---

# PineGuard.FluentValidation Project Spec

## Purpose

Define **project-specific** rules for the `PineGuard.FluentValidation` integration.

This integration must keep PineGuard’s layering intact. PineGuard layers in one direction — each layer calls only the one before it:

- **Core** (`Rules`/`Utils`) owns validation logic and parsing.
- **MustClauses** call Core and own the canonical, user-facing messages (`MustResult<T>`).
- **GuardClauses** call MustClauses and throw using `MustResult.Message`.
- **FluentValidation** adapts MustClauses into `IRuleBuilder` extensions.
- **DataAnnotations** adapts MustClauses into `ValidationAttribute`s.

Guard, Fluent and DataAnnotations are sibling adapters over Must — none calls another, and none reimplements Core logic.

## Scope

This spec applies to `src/PineGuard.FluentValidation/**`.

## Feature Implementation Checklist

See `docs/ai/specs/spec.md` §3 ("Feature Implementation Checklist (Master)").

## Related specs

- Unit tests addendum: `docs/ai/specs/fluent-validation/unit-test.md`
- Coverage addendum: `docs/ai/specs/fluent-validation/coverage.md`
- Naming collisions: `docs/ai/specs/language/naming-collisions.md`

## References

- Root rules: `docs/ai/specs/spec.md`
- Validated value vs configuration/dependency parameters: `docs/ai/specs/spec.md` (“Validated value vs configuration/dependency parameters”).
- Dependency graph: `docs/ai/specs/dependencies.md`

---

## 1) Non-negotiables (integration contract)

- Do not implement validation logic in this project.
- **Method Ordering Rule**: Positive methods must appear BEFORE Negative methods in the file, matching `docs/ai/specs/must-clauses/project.md` ("Method ordering").
- Do not call `PineGuard.Rules.*` or `PineGuard.Utils.*` from FluentValidation integration code.
- Always validate by calling `PineGuard.MustClauses.Must.Be.*` and using the `MustResult`.
- Do not embed datasets/tables/regex lists here; those belong in Core (`Rules`/`Utils`).

## 2) Message + parameter name handling (required)

FluentValidation error messages must reference the **property/display name**, not the caller argument expression.

Required pattern:

- Call Must clauses with `paramName: null` so the returned message remains a template containing `{paramName}`.
- Replace `{paramName}` with FluentValidation’s display/property name.

Rationale:

- Keeps one canonical message template.
- Avoids forking messages across Must/Guard/FluentValidation/DataAnnotations.

## 3) Required adapter: `FluentExtension.MustBe(...)`

All FluentValidation extensions must adapt Must clauses using the shared adapter:

- Location: `src/PineGuard.FluentValidation/Common/FluentExtension.cs`
- Namespace: `PineGuard.FluentValidation.Common`

Rules:

- Prefer calling `ruleBuilder.MustBe(...)` over calling FluentValidation’s `.Must(...)` directly.
- The adapter must take a `MustResult`-returning delegate (no exceptions for normal invalid values) and translate it into a FluentValidation rule.
- When the Must result is a failure, compute the final message by taking either the caller-provided `message` (if not null) or `result.Message`, then replace `{paramName}` with FluentValidation’s display/property name.

## 4) Extension methods (folder/shape/naming)

### 4.1 Location + namespace

- Place extension methods at the project root: `src/PineGuard.FluentValidation/Fluent{Domain}Extensions.cs`
- `src/PineGuard.FluentValidation/Common/**` holds the shared `FluentExtension.MustBe(...)` adapter only.
- Namespace must be: `PineGuard.FluentValidation`

### 4.2 Naming conventions

- Use domain-based extension class names:
  - `FluentStringExtensions`
  - `FluentNumberExtensions`
  - `FluentBitWiseExtensions`
- Extension method names should match the corresponding Must clause name whenever practical.
- When a matching name would collide with or strongly mimic a FluentValidation built-in, prefer a clearer PineGuard-specific adapter name instead.
- For URI/web adapters, prefer explicit names such as `WebUrl()` over ambiguous short names like `Url()` when collision or IntelliSense ambiguity is a concern.
- A shorter alternate name may remain only as a thin facade when the explicit collision-safe method is documented as the preferred surface.
- For null/presence adapters, prefer `Required()` / `NotRequired()` over `NotNull()` / `Null()` to avoid collisions with FluentValidation built-in validators while keeping PineGuard intent explicit.
- Follow `docs/ai/specs/language/naming-collisions.md` for collision handling across adapter layers.

Shared vocabulary map (required):

- Naming alternatives and opposite-term relationships are defined in `docs/ai/specs/language/vocabulary.md`.
- Audit scripts consume `docs/ai/specs/language/vocabulary.json` to normalize names to **concepts** (so Guard may prefer `Invalid*` / `Not*` while Fluent prefers the concept name).

### 4.3 Chaining + return types (required)

- Return `IRuleBuilderOptions<T, TProp>` (or `IRuleBuilderOptions<T, TProp?>`) so rules chain naturally.
- Keep the optional `message` parameter last, defaulting to `null`.

Example shape:

```csharp
public static IRuleBuilderOptions<T, string?> DigitsOnly<T>(
    this IRuleBuilder<T, string?> ruleBuilder,
    string? message = null) =>
    ruleBuilder.MustBe<T, string?, string>(v => Must.Be.DigitsOnly(v, paramName: null), message);
```

Guidance:

- Prefer calling `ruleBuilder.MustBe(...)` **without explicit generic type arguments** when the compiler can infer them.
- Only use `MustBe<T, TProp, TResult>(...)` when `TResult` differs from the FluentValidation property type (including nullability), and inference is ambiguous or would select the wrong overload.

For example, if the property type matches the Must clause result type, this is preferred:

```csharp
public static IRuleBuilderOptions<T, DateTimeOffset?> PastOrPresent<T>(
    this IRuleBuilder<T, DateTimeOffset?> ruleBuilder,
    string? message = null) =>
    ruleBuilder.MustBe(value => Must.Be.PastOrPresent(value, paramName: null), message);
```

## 5) Adapter conventions

### Nullability

FluentValidation follows the standard "skip on null" behavior:

- Validation rules should **not** fail on `null` values unless the caller explicitly requires presence using PineGuard `Required()` or FluentValidation native validators such as `.NotNull()` / `.NotEmpty()`.
- Therefore, PineGuard FluentValidation adapters must treat `null` as **success** (skip validation) by returning `MustResult<T>.Ok(...)`.

This is intentionally different from Must/Guard behavior:

- MustClauses follow Rule07 (see `docs/ai/specs/tools/audit-cli/spec.md` and `tools/audit-cli/rules/Test-Rule07-Nullability.ps1`) — the hybrid nullability strategy: inputs may be declared nullable (especially reference types), but **null is invalid by default** unless the method name explicitly encodes null as acceptable (e.g., `NullOrXxx`).
- The adapter layer is responsible for the FluentValidation UX: presence/required checks are expressed via FluentValidation chaining, not by failing on null in PineGuard rules.

Rules:

- **Value type properties** (e.g., `DateOnly?`) must be supported.
  - If `HasValue` is false: return `Ok` (skip).
  - If `HasValue` is true: call the corresponding MustClause with `val.Value`.

- **Reference type properties** (e.g., `string?`) must be supported.
  - If `value is null`: return `Ok` (skip) unless the caller chains `Required()` (or another explicit presence validator such as `.NotNull()`).
  - If `value` is non-null: call the corresponding MustClause.

Standard pattern:

```csharp
public static IRuleBuilderOptions<T, DateOnly?> Past<T>(
    this IRuleBuilder<T, DateOnly?> ruleBuilder,
    string? message = null) =>
    ruleBuilder.MustBe(val => val.HasValue
        ? Must.Be.Past(val.Value, paramName: null)
        : MustResult<DateOnly>.Ok(default),
        message);
```
