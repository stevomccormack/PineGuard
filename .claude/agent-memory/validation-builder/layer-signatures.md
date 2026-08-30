---
name: layer-signatures
description: Method-signature and body conventions for each validation layer — Must, Guard, Fluent, DataAnnotations — including parameter ordering, the parsed-result contract, and the mistakes that recur
metadata:
  type: project
---

Per-layer shape rules. The specs under `docs/ai/specs/<layer>/project.md` are authoritative; these are
the parts that are easy to get wrong or that only show up when a build fails.

## MustClause signatures

- Always: `this IMustClause _`, value param, `[CallerArgumentExpression(nameof(value))] string? paramName = null` last.
- Reference types nullable (`string?`); value types non-nullable (`bool`, `DateOnly`, `int`).
- Return `MustResult<T>` via `FromBool(ok, code, messageTemplate, paramName, value, result)` or `.Fail(...)` / `.Ok(...)`.
- Message templates use the `{paramName}` placeholder — never hardcode a parameter name.
- A config-param null/validity check attributes failure to `nameof(configParam)`, **not** `value`.
- Facade pattern: simple domains are a public static class; standard domains are an internal impl plus a public facade that flattens the API.

## Enum config parameters are passed through, not guarded

`project.md` §"Parameter validation order" says to guard "enum-like parameters" as programmer misuse,
but **no clause in the repo does** — `Inclusion`, `StringComparison`, `CronFormat` are all forwarded
straight to the Core rule, which answers `false` for an undefined value. Follow the code, not that
line: guarding one would need either a second code (breaking one-clause-one-code, since the domain
catalogue usually ships a single constant) or a `nameof(format)` attribution that reads no differently
to the caller. The guarded cases that *do* exist are numeric/range config (`precision`, `min > max`)
and lookup config (`FileSignatureUtility.IsKnownExtension(extension)`) — i.e. where the catalogue
already carries a distinct `…Unknown`/`…Invalid` code for the configuration error. Fixtures reflect
this: `CronRulesFixtures.IsCronExpression.UnknownFormat` is an ordinary invalid-*value* scenario.

## Verifying one new clause's coverage

`Run-CodeCoverage.ps1 -Scope MustClauses` prints only a `-Top 30` lowest-covered list, so an
alphabetically-late new class (`MustToken*`, `MustVersion*`) is absent from the output even at 100%.
Do not read that as "not covered" — confirm with
`-Mode Analyze -Scope <layer> -IncludeClassNameRegex 'MustToken|MustCron'`, which prints a summary
scoped to just those classes.

## MustClause parsed-result contract

When `MustResult<T>.Result` should carry the parsed/normalized value, call `Utility.TryXxx(value, out var parsed)`
and pass `parsed` as `result:` — not `Rules.IsXxx()`. Use `Rules.IsXxx()` only when no parsed output is
needed (pure boolean, e.g. collection predicates). The recurring bug is `result: value` (raw input)
where a parsed value was available. Reference: `docs/ai/specs/core/project.md` §4.1.

## GuardClause signatures

- Fixed order: `this IGuardClause _`, `value`, required config params, optional forwarded config with a
  default (`StringComparison comparison = StringComparison.Ordinal`, `Inclusion inclusion = …`), then
  `string? message = null`, `Func<Exception>? exceptionCreator = null`, and the
  `[CallerArgumentExpression]` `paramName` **last** — an explicit argument must never displace the capture.
- Always call the corresponding MustClause; `Guard.Against.X` calls `Must.Be.Y` where `Y` is the positive.
- Throw via `GuardFailure.Throw(result, message, exceptionCreator)` — pass the `IMustResult` itself, never
  a message string, so the exception carries the clause's own `Code` and `ParamName`.
- End-of-line comment on the Must call: `// Guard.Against.NotX => Must.Be.X (complement)`.
- Return the typed `result.Result!` on success — Guard methods return `T`, not a `MustResult`.
- Method named after the forbidden state (`Guard.Against.NullOrEmpty`).
- File ordering follows the Must clause each guard invokes, mirroring the Must file — not alphabetical,
  not "all negatives first" (guard-clauses/project.md §4).

## FluentValidation signatures

- Extension on `IRuleBuilder<TModel, T>`, returns `IRuleBuilderOptions<TModel, T>`.
- Single-expression body: `ruleBuilder.MustBe(val => Must.Be.X(val, paramName: null), message, MustCodes.X.Y.Z)`.
- Parameter order mirrors the Must clause minus `paramName`: required config, then optional forwarded
  config with a default, then `string? message = null` **last** (project.md §4.3).
- Type mismatches need explicit generic args: `ruleBuilder.MustBe<T, string?, string>(...)`.
- A name may duplicate an existing Fluent name on a different receiver type (`Contains` exists on both
  collection and range rule builders) — the property type disambiguates, so it is not a collision.
- See [[fluent-adapter-nuances]] for null handling and config-param messages.

## DataAnnotations signatures

- `sealed class` inheriting `ValidationAttributeBase(typeof(T), MustCodes.X.Y.Z)`; the `code` argument is
  required and sits **before** `allowNull:`. Even a parameterless attribute needs the empty primary ctor
  `FooAttribute() : Base(code)` to pass the code up.
- Constructor params become `public` properties; override `ValidateValue(object? value, ValidationContext validationContext)`.
- **No defensive type guard.** Cast straight through (`var strValue = (string)value!;`). The base already
  returned `Success` for null and threw `InvalidOperationException` on a type mismatch, so an `is not T`
  check is an uncoverable dead branch that breaks the 100% gate (project.md §4). Exceptions:
  `typeof(object)` polymorphic families (switch with a throwing `default:`) and `allowNull: false`
  attributes, which really do receive null.
- Delegate: `var result = Must.Be.X(strValue, paramName: null); return FromMustResult(result, validationContext);`
- Naming: string validators take the `String` suffix; collisions are broken with a Type/Domain suffix.

## DataAnnotations: optional forwarded config and the netstandard2.1 `init` trap

- An optional knob the Must clause takes (`StringComparison comparison = Ordinal`) becomes a **named
  attribute argument**, not a ctor parameter: `public StringComparison Comparison { get; init; } =
  StringComparison.Ordinal`, used as `[EndsWith(".pdf", Comparison = StringComparison.OrdinalIgnoreCase)]`.
  Required config stays a primary-ctor parameter exposed as `{ get; }`.
- `init` on **any** member of a `netstandard2.1` project needs `System.Runtime.CompilerServices.IsExternalInit`,
  which that TFM does not ship → `error CS0518`. It compiles fine on net8.0/net10.0, so the error only
  appears on a full multi-TFM build — never conclude from a net10.0-only build.
- Core's polyfill is `internal` and its `InternalsVisibleTo` covers only MustClauses and GuardClauses.
  `src/PineGuard.DataAnnotations/Polyfills/IsExternalInit.cs` is a mirrored copy. FluentValidation has no
  copy yet and will need one the first time it uses `init`.

## Recurring mistakes

- Passing `result: value` (raw input) when a parsed value is available from `Utility.TryXxx()`.
- Creating new messages in GuardClauses instead of reusing the Must message.
- Nullable value types in Rules (causes overload ambiguity).
- Omitting the `CallerArgumentExpression` parameter, or letting an explicit argument displace it.
- IO in Core Rules/Utils.
- `.Must(...)` in FluentValidation instead of `.MustBe(...)`.
- Validation logic in DataAnnotations — strict adaptation only.
- `[ExcludeFromCodeCoverage]` on anything that is actually reachable.
