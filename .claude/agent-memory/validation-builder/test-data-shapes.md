---
name: test-data-shapes
description: Fixture-v2 test wiring across the five layers — Expected/Case types, dataset shapes per layer, and the tuple-fixture, DA-config and Fluent-complement traps
metadata:
  type: project
---

Shapes come from `docs/ai/specs/testing/unit-test.md` §4, `docs/ai/specs/testing/fixture.md` and
`docs/ai/rules/fixture-conventions.md` §4; the per-layer addendum
(`docs/ai/specs/<layer>/unit-test.md`) overrides for that layer. Recipe:
`docs/ai/skills/scaffold-unit-test/SKILL.md`.

## Baseline shape

- Fixtures hold `RuleScenario<T>[]` arrays, not raw tuples. TestData projects them with `.ToXxxCases()`.
- TestData files use a nested Operation Group class per method (§4.1); Tests files are flat `sealed class`
  with one `MethodName_BehavesAsExpected` per op, mirroring the TestData group order (§4.5).
- Element ordering: datasets first, records last (§4.4). Outer TestData ordering: shared fields → op
  groups → helper methods at the bottom (§4.6).
- The tuple property is `Value` (never `Input`); elements are camelCase and match the method's parameter
  names exactly (§4.3).
- Input values come from `tests/PineGuard.Testing/Fixtures/`, aliased `F`, with `nameof` for `Name` (§9).
  Zero comments, single-line entries, edge-case constants sourced from the Rule classes.
- Case records: `RuleCase<T>`, `MustCase<T>`, `GuardCase<T>`, `FluentCase<T>`, `DataAnnotationCase`.
  Expected types: `RuleExpected`, `MustExpected`, `GuardExpected`, `FluentExpected`,
  `DataAnnotationExpected` — all carry `IsValid` as the uniform boolean.
- Assert through `AssertResult(tc, result)` on the layer's `BaseXxxUnitTest`, never a hand-rolled
  `Assert.Equal` chain.

## Guard: `ToGuardCases(paramName)` and tuple fixtures

The `ToGuardCases(string paramName)` overload picks `ArgumentNullException` vs `ArgumentException` from
`RuleScenario<T>.IsNull`, which is `Inputs is null` — **always false for a tuple-shaped fixture**
(`(string? value, string substring, …)`), so a null *inner* value silently expects the wrong exception
type. Use the `expectedFactory` overload and inspect the tuple member directly:
`.ToGuardCases(s => s.Inputs.value is null ? new GuardExpected(false, typeof(ArgumentNullException), "value") : new GuardExpected(false, typeof(ArgumentException), "value"))`.

Guard groups keep exactly two datasets (`ValidCases`/`InvalidCases`). When scenarios have to come from
two fixture sources they are re-joined with a collection expression
(`[.. a.ToGuardCases(…), .. b.Only(nameof(…)).ToGuardCases(…)]`), not a third dataset — unlike the Must
layer, which does add `NullCases`.

## Fluent: complement (`Not*`) groups without duplicating fixture data

Both the positive and the complement project the **same** `AllScenarios` array; the complement just
flips the switch arms:
`nameof(F.X.NullValue) => new FluentExpected(true), _ when s.IsValid => new FluentExpected(false, "<complement message>", Code: …), _ => new FluentExpected(true)`.
The null arm stays `true` in BOTH directions. Older files declare a private local `Scenarios` array for
the `Not*` half; that duplicates data — prefer the flipped-switch projection.

When a family ships inverted code pairs (positive carries `not-x`, complement carries `x`), assert
`Code:` on **every** group's invalid arm, not just one spot check — the inversion is exactly what a
single spot check leaves unguarded. `AssertResult` reads `Code` only when `Expected.Code is not null`,
so never set it on a valid expectation (it indexes `Errors[0]`).

## DataAnnotations: the attribute fixes its config, the fixture varies it

`DataAnnotationCase` carries only `(Name, object? Value, Expected)` — there is nowhere to put a per-case
needle or threshold, and the test method builds one attribute for the whole dataset. So a DA op group
**pins** its configuration in `public static readonly` fields that the test reads
(`new ContainsAttribute(TestData.Contains.Substring)`), unlike Fluent/Must, which pull config from the
scenario tuple per case.

When the fixture varies that config, split into a default group plus a companion group per variant
(`Contains` + `ContainsIgnoringCase`), each selecting only the scenarios whose tuple actually used that
configuration, with every literal still sourced from a fixture field. Never reuse a scenario's value
against a different scenario's needle — the fixture's verdict does not survive the substitution. The
companion group is also what covers the `init` accessor; the default group covers the ctor initializer.

Assert the code with the three-arg `AssertResult(tc, result, attr.Code)`, on invalid arms only.

The cheapest way to keep a pinned config honest is to read it back out of the fixture tuple the group
selects: `public static string Extension => F.HasSignature.PngHeader.extension;` plus
`.Only(nameof(F.HasSignature.PngHeader), …)`. No literal is duplicated, and the group can only ever
run scenarios that declared that exact config. When a fixture varies config across many scenarios
(one per file format, say), keep one primary group carrying the full pass/fail matrix, add a companion
group per *form* of the argument (dot-optional, casing, padding), and let a sibling no-config op group
(`KnownFileSignature`) absorb the breadth cases rather than minting a near-identical group per value.
A config value the fixture never pairs with a value (an unregistered extension) has no scenario to
select, so that group falls back to Pattern C inline `DataAnnotationCase` entries.

Legacy DA files (`StringAttributesTestData`/`Tests`) still use `ValidCase`/`EdgeCases`/`InvalidCases`,
stacked `[MemberData]` and a private `Verify` helper — all prohibited by the addendum. Append new op
groups in the current shape and move the Tests class onto `BaseDataAnnotationUnitTest(output)`; leave
the legacy methods untouched, the same treatment the Must layer gave `MustStringClausesTests`.

## Null is different at every layer

One fixture's null scenario is a failure at Must, a throw at Guard, and a pass at both Fluent and
DataAnnotations (FluentValidation skips null per its project.md §5; the DA base returns `Success` for
null). See [[fluent-adapter-nuances]].
