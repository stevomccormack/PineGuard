---
name: layer-signatures
description: Per-layer method signature conventions for Must, Guard, FluentValidation and DataAnnotations clauses — parameter order, nullability, delegation, and the mistakes that break the 100% coverage gate
metadata:
  type: project
---

The signature shape of each validation layer, distilled from repeated builds against
`docs/ai/specs/<layer>/project.md`.

**Why:** these are the details a spec states once in prose but that get silently wrong in
implementation — parameter order that displaces `[CallerArgumentExpression]`, a defensive type guard
that becomes an uncoverable branch, a `result: value` that returns the raw input instead of the
parsed one. Each one costs a review cycle or a coverage failure.

**How to apply:** read the layer's project-spec first (it is authoritative); use this as the
checklist for the details the spec assumes you already internalised.

## Layer dependency chain

```
Core Utils -> Core Rules -> MustClauses -> GuardClauses
                                       -> FluentValidation
                                       -> DataAnnotations
```

## MustClause signatures

- Always: `this IMustClause _`, value param, `[CallerArgumentExpression(nameof(value))] string? paramName = null` last
- Reference types: nullable (`string?`). Value types: non-nullable (`bool`, `DateOnly`, `int`)
- Return `MustResult<T>` via `FromBool(ok, code, messageTemplate, paramName, value, result)` or `.Fail(...)` / `.Ok(...)`
- Message templates use `{paramName}` placeholder — NEVER hardcode parameter names
- Config param checks attribute failure to `nameof(configParam)`, NOT `value`
- Facade pattern: simple domains = public static class; standard domains = internal impl + public facade (flatten API)

### Parsed-result contract (`docs/ai/specs/core/project.md` §4.1)

- When `MustResult<T>.Result` needs the parsed/normalized value, call `Utility.TryXxx(value, out var parsed)` — NOT `Rules.IsXxx()`
- The Try method gives both the boolean and the parsed value; pass the parsed value as `result:` to `FromBool()`
- Use `Rules.IsXxx()` only when no parsed output is needed (pure boolean, e.g. collection predicates)
- Bug pattern: `result: value` (raw input) instead of `result: parsed` — always flag

## GuardClause signatures

- Fixed parameter order: `this IGuardClause _`, `value`, required config params, optional forwarded
  config with a default (`StringComparison comparison = StringComparison.Ordinal`, `Inclusion inclusion = ...`),
  then `string? message = null`, `Func<Exception>? exceptionCreator = null`, and
  `[CallerArgumentExpression(nameof(value))] string? paramName = null` **last** — an explicit argument
  must never displace the caller-expression capture
- ALWAYS call the corresponding MustClause (complement logic: `Guard.Against.X` calls `Must.Be.Y` where Y is the positive)
- Throw via `GuardFailure.Throw(result, message, exceptionCreator)` — pass the `IMustResult` itself,
  never a message string, so the exception carries the clause's own `Code` and `ParamName`
- Add the end-of-line complement comment on the Must call: `// Guard.Against.NotX => Must.Be.X (complement)`
- Return the typed result (`result.Result!`) on success — Guard methods return `T`, not `MustResult`
- Method named after the forbidden state (`Guard.Against.NullOrEmpty`)
- File ordering is by the Must clause each guard invokes, mirroring the Must file's order — NOT
  alphabetical and not "all negatives first" (guard-clauses/project.md §4)
- `GuardFailure` picks `ArgumentNullException` vs `ArgumentException` from the **failed result's
  `Value`**, not from the guard's `value` argument — a failed config-parameter check with a null
  config value therefore throws `ArgumentNullException` on the *config* param name

## FluentValidation signatures

- Extension on `IRuleBuilder<TModel, T>`, returns `IRuleBuilderOptions<TModel, T>`
- Single-expression body: `ruleBuilder.MustBe(val => Must.Be.X(val, paramName: null), message, MustCodes.X.Y.Z)` — the trailing `code` arg is standard; see [[must-codes-catalogue]]
- Nullable string handling: `val is not null ? Must.Be.X(val, paramName: null) : MustResult<string>.Ok(null!)`
- Optional `string? message = null` parameter for custom error override
- Type mismatches: use explicit generic args `ruleBuilder.MustBe<T, string?, string>(...)`
- Parameter order mirrors the Must clause minus `paramName`: value config params first, then optional
  forwarded config with a default, then `string? message = null` **last** (project.md §4.3)
- Names may duplicate an existing Fluent name on a different receiver type (`Contains` exists on
  collection and range rule builders) — no collision, the property type disambiguates

## DataAnnotations signatures

- `sealed class`, inherits `ValidationAttributeBase(typeof(T), MustCodes.X.Y.Z)` — the `code` argument
  is required and sits **before** `allowNull:`; see [[must-codes-catalogue]]
- Primary constructor for parameterized attributes; a parameterless attribute still needs the empty
  primary ctor `FooAttribute() : Base(code)` to pass the code up
- Expose constructor params as `public` properties; override `ValidateValue(object? value, ValidationContext validationContext)`
- **No defensive type guard.** Cast straight through: `var strValue = (string)value!;`. The base has
  already returned `Success` for null and thrown `InvalidOperationException` on a type mismatch, so an
  `is not T` check is an uncoverable dead branch that breaks the 100% gate (project.md §4). The two
  exceptions are `typeof(object)` polymorphic families (switch with a throwing `default:`) and
  `allowNull: false` attributes, which really do receive null.
- Delegate: `var result = Must.Be.X(strValue, paramName: null); return FromMustResult(result, validationContext);`
- Naming: string validators suffix `String`; collision avoidance suffixes Type/Domain

### Optional forwarded config (`init` property) and the netstandard2.1 trap

- An optional knob the Must clause takes (`StringComparison comparison = Ordinal`) becomes a **named
  attribute argument**, not a ctor parameter: `public StringComparison Comparison { get; init; } = StringComparison.Ordinal`,
  read as `[EndsWith(".pdf", Comparison = StringComparison.OrdinalIgnoreCase)]`. Required config stays
  a primary-ctor parameter exposed as `{ get; }`.
- `init` on **any** member of a `netstandard2.1`-targeting project needs
  `System.Runtime.CompilerServices.IsExternalInit`, which that TFM does not ship →
  `error CS0518: Predefined type 'IsExternalInit' is not defined`. It compiles fine on net8.0/net10.0,
  so the error only appears on a full multi-TFM build — never conclude from a net10.0-only build.
- Core's polyfill is `internal` and its `InternalsVisibleTo` covers only MustClauses and GuardClauses.
  `src/PineGuard.DataAnnotations/Polyfills/IsExternalInit.cs` is a mirrored copy. FluentValidation has
  no copy yet — it will need one the first time it uses `init`.

## Common mistakes to avoid

- DO NOT pass `result: value` (raw input) when a parsed value is available from `Utility.TryXxx()`
- DO NOT create new messages in GuardClauses (reuse Must messages)
- DO NOT use nullable value types in Rules (causes overload ambiguity)
- DO NOT skip the `CallerArgumentExpression` parameter
- DO NOT put IO in Core Rules/Utils
- DO NOT use `.Must(...)` in FluentValidation — use `.MustBe(...)`
- DO NOT write validation logic in DataAnnotations — strict adaptation only
- DO NOT use `[ExcludeFromCodeCoverage]` unless truly unreachable
