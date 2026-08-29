# Validation Builder Memory

> **Role:** `docs/ai/roles/builder.md` (Builder)
> Directives: Follow the Spec, Pick the Right Layer, Always Valid State, Clean Code, Verify Locally.
> Constraints: No architectural changes without approval. No broken builds. No IO in Core.

## Learned Patterns

### MustClause Signatures
- Always: `this IMustClause _`, value param, `[CallerArgumentExpression(nameof(value))] string? paramName = null` last
- Reference types: nullable (`string?`). Value types: non-nullable (`bool`, `DateOnly`, `int`)
- Return `MustResult<T>` via `FromBool(ok, messageTemplate, paramName, value, result)` or `.Fail(...)` / `.Ok(...)`
- Message templates use `{paramName}` placeholder — NEVER hardcode parameter names
- Config param null checks attribute failure to `nameof(configParam)`, NOT `value`
- Facade Pattern: Simple domains = public static class. Standard domains = internal impl + public facade (flatten API)

### GuardClause Signatures
- Fixed parameter order: `this IGuardClause _`, `value`, required config params, optional forwarded
  config with a default (`StringComparison comparison = StringComparison.Ordinal`,
  `Inclusion inclusion = ...`), then `string? message = null`, `Func<Exception>? exceptionCreator = null`,
  and `[CallerArgumentExpression(nameof(value))] string? paramName = null` **last** — an explicit
  argument must never displace the caller-expression capture
- ALWAYS call corresponding MustClause (complement logic: Guard.Against.X calls Must.Be.Y where Y is the positive)
- Throw via `GuardFailure.Throw(result, message, exceptionCreator)` — pass the `IMustResult` itself,
  never a message string, so the exception carries the clause's own `Code` and `ParamName`
- Add the end-of-line complement comment on the Must call: `// Guard.Against.NotX => Must.Be.X (complement)`
- Return the typed result (`result.Result!`) on success — Guard methods return `T` not `MustResult`
- Method named after forbidden state (`Guard.Against.NullOrEmpty`)
- File ordering is by the Must clause each guard invokes, mirroring the Must file's order — NOT
  alphabetical and not "all negatives first" (guard-clauses/project.md §4)

### FluentValidation Signatures
- Extension on `IRuleBuilder<TModel, T>`, returns `IRuleBuilderOptions<TModel, T>`
- Single-expression body using `ruleBuilder.MustBe(val => Must.Be.X(val, paramName: null), message, MustCodes.X.Y.Z)` — the trailing `code` arg is now standard; see [MustCodes catalogue](must-codes-catalogue.md)
- Nullable string handling: `val is not null ? Must.Be.X(val, paramName: null) : MustResult<string>.Ok(null!)`
- Optional `string? message = null` parameter for custom error override
- Type mismatches: use explicit generic args `ruleBuilder.MustBe<T, string?, string>(...)`
- Parameter order mirrors the Must clause minus `paramName`: value config params first, then
  optional forwarded config with a default (`StringComparison comparison = StringComparison.Ordinal`),
  then `string? message = null` **last** (project.md §4.3)
- Names may duplicate an existing Fluent name on a different receiver type (`Contains` exists on
  collection and range rule builders) — no collision, the property type disambiguates

### Fluent complement (`Not*`) TestData without duplicating fixture data
- Both the positive and the complement project the **same** `AllScenarios` array; the complement
  just flips the switch arms:
  `nameof(F.X.NullValue) => new FluentExpected(true), _ when s.IsValid => new FluentExpected(false, "<complement message>", Code: ...), _ => new FluentExpected(true)`
- The null arm stays `true` in BOTH directions — FluentValidation skips null (project.md §5), unlike
  Must (failure) and Guard (throw). Same fixture, three different null expectations per layer.
- Older Fluent test files declare a private local `Scenarios` array for the `Not*` half; that
  duplicates data. Prefer the flipped-switch projection.
- When a family ships inverted code pairs (positive carries `not-x`, complement carries `x`),
  assert `Code:` on **every** group's invalid arm, not just one spot check — the inversion is
  exactly the wiring a single spot check leaves unguarded. `AssertResult` only reads `Code` when
  `Expected.Code is not null`, so never set it on a valid expectation (it indexes `Errors[0]`).

### DataAnnotations Signatures
- `sealed class`, inherits `ValidationAttributeBase(typeof(T), MustCodes.X.Y.Z)` — the `code` argument is now required and sits **before** `allowNull:`; see [MustCodes catalogue](must-codes-catalogue.md)
- Primary constructor for parameterized attributes: `sealed class ExactLengthAttribute(int length) : ValidationAttributeBase(typeof(string), MustCodes.X.Y.Z)`; a parameterless attribute still needs the empty primary ctor `FooAttribute() : Base(code)` to pass the code up
- Expose constructor params as `public` properties
- Override `ValidateValue(object? value, ValidationContext validationContext)`
- Type check: `if (value is not string strValue) return ValidationResult.Success;` (skip on wrong type)
- Delegate: `var result = Must.Be.X(strValue, paramName: null); return FromMustResult(result, validationContext);`
- Naming: String validators suffix `String`. Collision avoidance suffix Type/Domain

### MustClause Parsed-Result Contract
- When `MustResult<T>.Result` needs the parsed/normalized value, call `Utility.TryXxx(value, out var parsed)` — NOT `Rules.IsXxx()`
- The Try method gives both the boolean and the parsed value; pass the parsed value as `result:` to `FromBool()`
- Use `Rules.IsXxx()` only when no parsed output is needed (pure boolean, e.g., collection predicates)
- Bug pattern: `result: value` (raw input) instead of `result: parsed` — always flag
- Reference: `docs/ai/specs/core/project.md` §4.1

### Common Mistakes to Avoid
- DO NOT pass `result: value` (raw input) when a parsed/normalized value is available from `Utility.TryXxx()`
- DO NOT create new messages in GuardClauses (reuse Must messages)
- DO NOT use nullable value types in Rules (causes overload ambiguity)
- DO NOT skip CallerArgumentExpression parameter
- DO NOT put IO in Core Rules/Utils
- DO NOT use `.Must(...)` in FluentValidation — use `.MustBe(...)`
- DO NOT write validation logic in DataAnnotations — strict adaptation only
- DO NOT use `[ExcludeFromCodeCoverage]` unless truly unreachable

## Layer Dependency Chain
```
Core Utils -> Core Rules -> MustClauses -> GuardClauses
                                       -> FluentValidation
                                       -> DataAnnotations
```

## Test Structure

TestData shape per `docs/ai/specs/testing/unit-test.md` §4; Tests shape per
`docs/ai/rules/fixture-conventions.md` §4 and `docs/ai/specs/testing/fixture.md`.

- TestData files use nested Operation Group classes per method (§4.1)
- Tests files are flat `sealed class` with one `MethodName_BehavesAsExpected` per op
- Element ordering: datasets first, records last (§4.4)
- Tests methods mirror TestData group order (§4.5)
- Tuple property MUST be `Value` (not `Input`), elements camelCase matching exact method param names (§4.3)
- Test Fixtures: input values from `PineGuard.Testing.Fixtures/`, `nameof` for Name, alias `F` (§9 "Test Fixtures")
- Full canonical examples in §8 "Full Canonical Examples"
- Outer TestData class ordering (§4.6): shared fields → Op Groups → helper methods (at bottom)
- Use `docs/ai/skills/scaffold-unit-test/SKILL.md` for test implementation recipe
- **`Expected` property** on all test case records (NOT `ExpectedReturn`)
- Layer-specific Expected types: Core=`RuleExpected`, Must=`MustExpected`, Guard=`GuardExpected`, Fluent=`FluentExpected`, DA=`DataAnnotationExpected` — all carry `IsValid`
- `MustExpected(bool IsValid, string? Message = null, string? ParamName = null)` — composite for Must tests
- `FluentExpected(bool IsValid, string? Message = null)` — composite for Fluent tests
- `IsValid` is the uniform boolean name on all composite Expected types
- Assert through `AssertResult(tc, result)` on the layer's `BaseXxxUnitTest`, never a hand-rolled `Assert.Equal` chain

## Fixture Architecture v2

Reference: `docs/ai/specs/testing/fixture.md`
Conventions: `docs/ai/rules/fixture-conventions.md`

When implementing new validations, tests should follow the v2 architecture:
- Fixtures contain `RuleScenario<T>[]` arrays, not raw tuples
- TestData uses `.ToXxxCases()` extensions to project into layer-specific case records
- Tests use `BaseXxxUnitTest` with `AssertResult(tc, result)`
- Expected types: `RuleExpected`, `MustExpected`, `GuardExpected`, `FluentExpected`, `DataAnnotationExpected`
- Case records: `RuleCase<T>`, `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase`
- Zero comments, single-line entries, flat test classes, edge case constants from Rule classes

### Guard TestData pitfall: `ToGuardCases(paramName)` and tuple fixtures
- The `ToGuardCases(string paramName)` overload picks `ArgumentNullException` vs `ArgumentException`
  from `RuleScenario<T>.IsNull`, which is `Inputs is null` — **always false for tuple-shaped
  fixtures** (`(string? value, string substring, ...)`), so a null *inner* value silently expects
  the wrong exception type
- For tuple fixtures use the `expectedFactory` overload instead:
  `.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"))`
- Guard groups keep exactly two datasets (`ValidCases`/`InvalidCases`); when a null-value scenario
  has to be split off from the fixture side it is re-joined with a collection expression
  (`[.. a.ToGuardCases(...), .. b.Only(nameof(...)).ToGuardCases(...)]`), not a third dataset —
  unlike the Must layer, which does add `NullCases`

## Topic Files
- [MustCodes catalogue](must-codes-catalogue.md) — wiring codes into Must + Fluent + DataAnnotations: arg positions, one-clause-one-code and its bitwise exception, fixed-at-build ErrorCode
