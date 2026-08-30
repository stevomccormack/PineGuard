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
- Always: `this IGuardClause _`, value param, `CallerArgumentExpression`, `string? message = null`, `Func<Exception>? exceptionCreator = null`
- ALWAYS call corresponding MustClause (complement logic: Guard.Against.X calls Must.Be.Y where Y is the positive)
- Throw via `GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator)`
- Return the typed result (`result.Result!`) on success — Guard methods return `T` not `MustResult`
- Method named after forbidden state (`Guard.Against.NullOrEmpty`)

### FluentValidation Signatures
- Extension on `IRuleBuilder<TModel, T>`, returns `IRuleBuilderOptions<TModel, T>`
- Single-expression body using `ruleBuilder.MustBe(val => Must.Be.X(val, paramName: null), message, MustCodes.X.Y.Z)` — the trailing `code` arg is now standard; see [MustCodes catalogue](must-codes-catalogue.md)
- Nullable string handling: `val is not null ? Must.Be.X(val, paramName: null) : MustResult<string>.Ok(null!)`
- Optional `string? message = null` parameter for custom error override
- Type mismatches: use explicit generic args `ruleBuilder.MustBe<T, string?, string>(...)`

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

## Topic Files
- [MustCodes catalogue](must-codes-catalogue.md) — wiring codes into Must + Fluent + DataAnnotations: arg positions, one-clause-one-code and its bitwise exception, fixed-at-build ErrorCode
- [Batch D vocabulary aliases deferred](project_batch-d-vocabulary-aliases-deferred.md) — ScaleAbove/PrecisionAbove shipped without their vocabulary.json rows; docs/ freeze, not a defect
