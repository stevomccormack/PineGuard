# T-01 U4 Guard sweep ruling — Claude Fable 5.1

**Status:** Binding implementation ruling for the missing U4 regression-sweep inventory.
**Reviewer:** Claude Fable 5.1, resumed read-only review session `8fc20ebe-d70e-47fe-8e1b-01d79e3bb527`.
**Date:** 2026-09-27. The first invocation reached its turn cap before a verdict; the same persisted session was resumed and returned this successful final ruling. No files were changed by the reviewer and no .NET commands were run.
**Public result:** `artifacts/reviews/temporal-u4-sweep-ruling-fable.json` in the temporal worktree.
**Scope:** T-01 §3 U4 and T-00 D-9 only. This resolves the missing explicit TestData inventory; it does not revise other temporal decisions.

## Binding decision

Use a precise regression subset rather than adding duplicate boundary rows to every range fixture. Every inclusion-taking Guard must have an on-bound case that would detect an accidental complement of its forwarded `Inclusion`. Add the opposite-inclusion twin only where that fixture already publishes the twin. Fable verified that 11 of the 16 in-scope files already prove the boundary; adding duplicate rows there would add churn without evidence and exceed the T-00 §7.2 12-test-file cap.

Keep U4 standalone before the U4a TimeSpan rename. Fix only the `inclusion` argument for the four complementary single-threshold Must calls in the two TimeSpan Guard source files. Do not change fixtures, Test classes, Number guards, or U3-owned files.

## Exact U4 ownership

| Action | File |
|---|---|
| Fix | `src/PineGuard.GuardClauses/GuardTimeSpanClauses.cs` — `LessThan` and `GreaterThan` |
| Fix | `src/PineGuard.GuardClauses/GuardStringTimeSpanClauses.cs` — `LessThan` and `GreaterThan` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardTimeSpanClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardStringTimeSpanClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardDateOnlyClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateOnlyClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateTimeOffsetClausesTestData.cs` |

The two TimeSpan data files complement the fixture inclusion for `LessThan`/`GreaterThan`; the other three files receive the boundary rows below. U4 excludes `GuardTimeOnlyClausesTestData.cs` and `GuardStringTimeOnlyClausesTestData.cs` (U3 ownership), and does not edit any other test-data file. `GuardTimeOnlyRangeClausesTestData.cs` remains in U4's inspected range inventory and already has boundary evidence.

## Required boundary rows

For each of the TimeSpan and string-TimeSpan `LessThan`/`GreaterThan` groups, complement the fixture inclusion (`Inclusive ↔ Exclusive`) and retain the existing `BehavesAsExpected` test method. For `value == threshold`, `Exclusive` passes and `Inclusive` throws `ArgumentException` with parameter `value`, for both guard directions. Use the existing fixture-derived equal rows as the failing-first evidence; do not introduce separately named test methods, which violate the unit-test spec's one-method-per-operation-group rule.

Add these 17 rows to existing groups (fixture/member names and outcomes are binding):

| TestData file / group | Fixture row | Inclusion | Guard outcome |
|---|---|---|---|
| `GuardDateOnlyClausesTestData.cs` / `Between` | `IsBetween.AtMinExclusive` | Exclusive | pass |
| same / `Between` | `IsBetween.AtMinInclusive` | Inclusive | throw `ArgumentException`, `value` |
| same / `NotBetween` | `IsBetween.AtMinInclusive` | Inclusive | pass |
| same / `NotBetween` | `IsBetween.AtMinExclusive` | Exclusive | throw `ArgumentException`, `value` |
| same / `NotChronological` | `IsChronological.SameDayInclusive` | Inclusive | pass |
| same / `NotChronological` | `IsChronological.SameDayExclusive` | Exclusive | throw `ArgumentException`, `start` |
| same / `Chronological` | `IsChronological.SameDayExclusive` | Exclusive | pass |
| same / `Chronological` | `IsChronological.SameDayInclusive` | Inclusive | throw `ArgumentException`, `start` |
| same / `Overlapping` | `IsOverlapping.TouchingExclusive` | Exclusive | pass |
| same / `Overlapping` | `IsOverlapping.TouchingInclusive` | Inclusive | throw `ArgumentException`, `start1` |
| same / `NotOverlapping` | `IsOverlapping.TouchingInclusive` | Inclusive | pass |
| same / `NotOverlapping` | `IsOverlapping.TouchingExclusive` | Exclusive | throw `ArgumentException`, `start1` |
| `GuardStringDateOnlyClausesTestData.cs` / `ChronologicalDateOnly` | `DateOnlyIsChronological.EqualExclusive` (`2020-01-15` equals `2020-01-15`) | default Exclusive | pass |
| same / `OverlappingDateOnly` | `DateOnlyIsOverlapping.TouchingInclusive` (`01-01..01-15`, `01-15..01-31`) | default Exclusive | pass |
| same / `NotOverlappingDateOnly` | same touching values | default Exclusive | throw `ArgumentException`, `start1` |
| `GuardStringDateTimeOffsetClausesTestData.cs` / `Between` | `DateTimeOffsetIsBetween.OffsetLessAssumedUtc` (`min == max == value`) | Inclusive | throw `ArgumentException`, `value` |
| `GuardStringTimeSpanClausesTestData.cs` / `DurationBetween` | `TimeSpanIsDurationBetween.AtMinInclusive` (`min == max == value`) | Inclusive | throw `ArgumentException`, `value` |

The string DateOnly groups keep their existing default Exclusive because their fixture tuples do not carry an inclusion value; do not widen tuple shapes. `NotBetween` in string DateTimeOffset and `NotDurationBetween` in string TimeSpan already have the needed boundary evidence.

## Failing-first and focused green evidence

After all source writers were frozen, the four existing TimeSpan `LessThan`/`GreaterThan` behavior
tests were run on net10.0 through the canonical test runner. Before the four source fixes, the build
succeeded and the result was **8 failed, 10 passed, 0 skipped (18 total)**. The eight failures were
both fixture twins (`EqualInclusive` and `EqualExclusive`) in each of the four methods. Fable's
same-session follow-up confirmed this is the correct red set: `EqualInclusive` projects to an
`Exclusive` Guard call that should pass, while `EqualExclusive` projects to an `Inclusive` Guard call
that should throw. The original checklist's “four” was corrected to eight; no TestData expectation
changed. See `artifacts/reviews/temporal-u4-red-check-adjudication.json`.

After the four complement expressions were applied, the same four methods passed **18/18 on
net10.0** (0 failed, 0 skipped). These are targeted failing-first and green proofs only. The complete
T-00 §9 GuardClauses gates on net8.0/net10.0, nullability audit, shared audit gate, and final Fable
unit acceptance remain pending.

## Inspected, no-edit families

Fable confirmed that existing fixtures already prove the on-bound behavior for:

- `GuardDateTimeClausesTestData.cs`, `GuardDateTimeOffsetClausesTestData.cs`, `GuardNumberClausesTestData.cs`, `GuardStringNumberTypesClausesTestData.cs`, and `GuardStringNumbersClausesTestData.cs` (two-ended range / count groups).
- `GuardDateOnlyRangeClausesTestData.cs`, `GuardDateTimeRangeClausesTestData.cs`, `GuardDateTimeOffsetRangeClausesTestData.cs`, and `GuardTimeOnlyRangeClausesTestData.cs` (chronology, overlap, contains groups).
- `GuardCollectionClausesTestData.cs` and `GuardStringGraphemesClausesTestData.cs` (count ranges).

Do not touch these files. Fixed `Before`/`After`/`OnOrBefore`/`OnOrAfter` and Number comparisons do not take `Inclusion`. U3 owns `GuardTimeOnlyClausesTestData.cs` and `GuardStringTimeOnlyClausesTestData.cs` in full.

## Verification contract

1. Demonstrate the eight TimeSpan equality-derived cases fail against the old forwarding: for each of four methods, the EqualInclusive twin executes as public `Exclusive` and should pass, while EqualExclusive executes as public `Inclusive` and should throw. The initial red run was 18 total, 10 passed, 8 failed. After the four complements, rerun the same methods and verify 18/18 pass. This count correction was adjudicated by Claude Fable 5.1 in the same persisted review session; the public follow-up is `artifacts/reviews/temporal-u4-red-check-adjudication.json`.
2. Add the 17 rows above, then run the GuardClauses gate on net8.0 and net10.0, nullability audit, and `audit --gate`.
3. Confirm the diff is limited to the seven files in the ownership table. No fixture, `Tests.cs`, TimeOnly, or Number file is edited.
4. Record the cited existing boundary evidence from the source result when closing D-9; leave the unrelated `DateOnlyClausesTestData.cs:11` nondeterministic `DateTime.UtcNow` fixture issue and the empty String Number edge fixture for separate work.

## Evidence and source references

Fable verified 18 inclusion-taking Guard files. The four TimeSpan single-threshold methods (`GuardTimeSpanClauses.cs:112,146` and `GuardStringTimeSpanClauses.cs:112,146`) forward to a complementary comparison and are the only polarity defect. The range-style guards forward unchanged to name-negated Must clauses, so unchanged forwarding is correct; Core semantics and strict `min > max` misuse checks confirm equal endpoints are permitted (`RuleComparison.IsBetween`, `RangeRules.IsChronological`, `DateTimeRange.Overlaps`, `DateTimeRangeRules.Contains`). See the public result artifact for the detailed existing-fixture citations and method-specific outcome table.
