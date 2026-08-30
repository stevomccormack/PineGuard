---
name: must-complement-test-wiring
description: Wiring Must-layer tests for a positive/Not* clause pair off one shared fixture group — why the null-value case cannot be inverted, the Except/Only slicing tools, and migrating a legacy BaseUnitTest test class onto BaseMustUnitTest
metadata:
  type: project
---

A `Must.Be.X` / `Must.Be.NotX` pair is backed by **one** `XxxRulesFixtures` group (the fixture
describes the Core rule, which only has a positive form). Projecting it into two Must op groups is
mechanical except for one trap.

**Why:** the fixture's `IsValid` flag is the *Core rule's* answer. Must adds two failure modes Core
does not have — null value and null configuration parameter — and both fail the positive clause **and
its complement**. Blindly inverting `ValidScenarios`/`InvalidScenarios` for the `Not*` clause marks
the null-value scenario valid, which is wrong and hides an uncovered branch.

**How to apply:**
- Slice by scenario name with `RuleScenarioExtension.Except(names)` / `.Only(names)` (in
  `PineGuard.Testing.UnitTests.Rules`) — never re-declare fixture data in the TestData file.
- The four-dataset shape that comes out symmetric for both halves of the pair:

  | Dataset | Positive clause | `Not*` clause |
  |---|---|---|
  | `ValidCases` | `F.X.ValidScenarios.ToMustCases()` | `F.X.InvalidScenarios.Except(nameof(F.X.NullValue)).ToMustCases(_ => new MustExpected(true))` |
  | `InvalidCases` | `F.X.InvalidScenarios.Except(nameof(F.X.NullValue))` + semantic message/code | `F.X.ValidScenarios` + semantic message/code |
  | `NullCases` | `F.X.InvalidScenarios.Only(nameof(F.X.NullValue))` → `"value must not be null."`, ParamName `value` | same slice, but the complement's code |
  | `NullConfigCases` | inline, one case | inline, one case |

  Each dataset then has a single uniform expectation, so no `switch` factory is needed. Extra named
  datasets beside `ValidCases`/`InvalidCases` are an established shape
  (`MustStringNumberClausesTestData` uses `NullCases`/`ZeroFactorCases`/`LettersCases`); the
  must-clauses `unit-test.md` "two datasets" line describes the single-source case, not a cap.
- **Config-parameter-null cases stay inline** and are not added to the fixture. They are programmer
  misuse, not rule scenarios — the Core rule *throws* on them, so they have no `IsValid` bool to
  carry, and Plan 05 pins several fixture groups to the two-array **format shape** (adding
  `InvalidEdgeScenarios` would silently promote them to boundary shape). Core's own W1 TestData does
  the same thing with a `ThrowsCase`.
- Coverage consequence: a `Not*` clause with a `if (value is null)` early return needs its own
  null-value case, otherwise that branch is uncovered. `MustCharClauses`' `Not*` groups omit null
  only because their value type is a non-nullable `char`.

**Legacy test classes.** Some Must test classes still read
`public class MustXxxClausesTests : BaseUnitTest` and assert with hand-rolled `Assert.Equal` chains.
`BaseUnitTest`'s ctor is `(ITestOutputHelper? output = null)` while `BaseMustUnitTest` is
`(ITestOutputHelper output)`, so the migration is a one-line declaration change —
`public class MustXxxClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)` — that leaves
every existing method compiling and unlocks `AssertResult(tc, result)` for new spec-shaped methods.
Do that rather than inventing a second assertion style inside the file. `MustStringClausesTests.cs`
was migrated this way in Phase 5 Batch A W2.

See [[MEMORY]] for the general clause-layer patterns and [[must-codes-catalogue]] for code wiring.
