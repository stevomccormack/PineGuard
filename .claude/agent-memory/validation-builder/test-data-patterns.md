---
name: test-data-patterns
description: How one fixture projects into five layers of TestData — case/Expected types, the two-dataset Guard shape, tuple-fixture null traps, config-parameter failures, and the DA attribute-pins-its-config rule
metadata:
  type: project
---

Fixture Architecture v2: one `RuleScenario<T>[]` fixture group feeds every layer's TestData through
`.ToXxxCases()`. Specs: `docs/ai/specs/testing/unit-test.md` §4 (TestData shape),
`docs/ai/rules/fixture-conventions.md` §4 + `docs/ai/specs/testing/fixture.md` (Tests shape), plus
each layer's `docs/ai/specs/<layer>/unit-test.md` addendum.

**Why:** the same scenario means something different at each layer — a null value is a Must failure,
a Guard throw, and a Fluent/DataAnnotations *pass*. Re-deriving that mapping per batch is where
wrong expectations get written, and the projections below are the ones that survived review.

**How to apply:** read the layer addendum first, then use this for the projection details it does not
spell out. Never duplicate fixture data to make a projection easier.

## Shared shape

- TestData files use nested Operation Group classes per method (§4.1); Tests files are flat
  `sealed class` with one `MethodName_BehavesAsExpected` per op
- Element ordering: datasets first, records last (§4.4); Tests methods mirror TestData group order (§4.5)
- Tuple property MUST be `Value` (not `Input`), elements camelCase matching exact method param names (§4.3)
- Input values come from `PineGuard.Testing.Fixtures/`, `nameof` for Name, alias `F` (§9)
- Outer TestData class ordering (§4.6): shared fields → Op Groups → helper methods (at bottom)
- **`Expected`** is the property on every case record (never `ExpectedReturn`); `IsValid` is the
  uniform boolean on every composite Expected type
- Case records: `RuleCase<T>`, `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase`
- Expected types: `RuleExpected`, `MustExpected(IsValid, Message?, ParamName?, Code?)`,
  `GuardExpected(IsValid, ExceptionType?, ParamName?, MessageContains?, Code?)`,
  `FluentExpected(IsValid, Message?, Code?)`, `DataAnnotationExpected`
- Assert through `AssertResult(tc, result)` on the layer's `BaseXxxUnitTest`, never a hand-rolled
  `Assert.Equal` chain
- Zero comments inside datasets, single-line entries, edge-case constants from the Rule classes
- Recipe: `docs/ai/skills/scaffold-unit-test/SKILL.md`

## Guard: the two-dataset shape and its escape hatch

- Guard op groups keep exactly two datasets (`ValidCases`/`InvalidCases`) — unlike Must, which adds
  `NullCases` and other named datasets
- When cases with different expectations must share a dataset, re-join them with a collection
  expression: `[.. a.ToGuardCases(...), .. b.Only(nameof(...)).ToGuardCases(...)]`. The expression
  accepts `..someTheoryData` spreads directly (`TheoryData<GuardCase<T>>` spreads element-wise, not
  as `object[]`) — no `.ToArray()` or manual `Add` loop needed
- Every guard test method must also call `AssertCustomMessage(tc, () => Guard.Against.X(..., message: CustomMessage))`
  right after `AssertResult`, or the `message ?? result.Message` branch is never exercised

### Tuple-fixture null trap

- The `ToGuardCases(string paramName)` overload picks `ArgumentNullException` vs `ArgumentException`
  from `RuleScenario<T>.IsNull`, which is `Inputs is null` — **always false for tuple-shaped fixtures**
  (`(string? value, string substring, ...)`), so a null *inner* value silently expects the wrong type
- For tuple fixtures use the `expectedFactory` overload and inspect the tuple field:
  `.ToGuardCases(s => new GuardExpected(false, s.Inputs.value is null ? typeof(ArgumentNullException) : typeof(ArgumentException), "value", Code: ...))`

### A Must clause whose *configuration* parameter can fail

- Some Must clauses reject the config param before ever looking at `value` — `Must.Be.FileSignature`
  fails on an unregistered `extension`, attributed to `nameof(extension)` and carrying a *different*
  code (`file.signature.unknown` vs `file.signature.mismatch`). Must gives that its own third dataset
  (`UnknownExtensionCases`); Guard folds it into `InvalidCases` to keep the two-dataset shape.
- Two spreads, two expectation factories, one collection expression — one attributing to `"value"`,
  one to the config param name, each with its own `Code:`. The config-param scenarios are built by
  `Project<TIn, TTuple>` off the *config* fixture group's `InvalidScenarios`, pairing every bad config
  value with one known-good `value`.
- A null **config** value still yields `ArgumentNullException`, but on the config param's name, not
  `"value"` — branch on `s.Inputs.<configField> is null`, not `s.IsNull`.

## Fluent: complement (`Not*`) groups without duplicating fixture data

- Both the positive and the complement project the **same** `AllScenarios` array; the complement just
  flips the switch arms:
  `nameof(F.X.NullValue) => new FluentExpected(true), _ when s.IsValid => new FluentExpected(false, "<complement message>", Code: ...), _ => new FluentExpected(true)`
- The null arm stays `true` in BOTH directions — FluentValidation skips null (project.md §5)
- Older Fluent test files declare a private local `Scenarios` array for the `Not*` half; that
  duplicates data. Prefer the flipped-switch projection.
- When a family ships inverted code pairs (positive carries `not-x`, complement carries `x`), assert
  `Code:` on **every** group's invalid arm, not just one spot check — the inversion is exactly the
  wiring a single spot check leaves unguarded. `AssertResult` only reads `Code` when
  `Expected.Code is not null`, so never set it on a valid expectation (it indexes `Errors[0]`).

## DataAnnotations: the attribute fixes its config, the fixture varies it

- `DataAnnotationCase` carries only `(Name, object? Value, Expected)` — nowhere to put a per-case
  needle/threshold, and the test method builds one attribute for the whole dataset. So a DA op group
  **pins** its configuration in `public static readonly` fields the test reads
  (`new ContainsAttribute(TestData.Contains.Substring)`), unlike Fluent/Must, which pull config out of
  the scenario tuple per case.
- When the fixture varies that config, split into a default group plus a companion group per variant
  (`Contains` + `ContainsIgnoringCase`), each selecting only the scenarios whose tuple actually used
  that configuration, with every literal still sourced from a fixture field. Do **not** reuse a
  scenario's value against a different scenario's needle — the fixture's verdict does not survive the
  substitution.
- The companion group is also what covers the `init` accessor: an attribute never constructed with
  `Comparison = ...` leaves that accessor uncovered. The default group covers the ctor initializer.
- Assert the code with the 3-arg overload `AssertResult(tc, result, attr.Code)`; set `Code` on invalid
  arms only.
- DA null expectation is `true` (base skips null).
- Legacy DA files (`StringAttributesTestData`/`Tests`) still use `ValidCase`/`EdgeCases`/`InvalidCases`,
  stacked `[MemberData]` and a private `Verify` helper — all prohibited by the addendum's table. Append
  new op groups in the current shape and move the Tests class onto `BaseDataAnnotationUnitTest(output)`;
  leave the legacy methods untouched.
