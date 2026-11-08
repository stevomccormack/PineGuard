<!-- metadata_header
type: receipt
id: temporal-u4a-string-overload-ruling-fable
version: 1.0
status: binding-ruling
last_updated: 2026-09-27
-->

# T1-U4a — Fable string-overload ruling receipt

**Reviewer:** Claude Fable 5.1, resumed persisted session `8fc20ebe-d70e-47fe-8e1b-01d79e3bb527`.
**Date:** 2026-09-27.
**Result:** binding, narrow correction to the T1-U4a rename table; no implementation or unit-design
change.

## Source facts

`MustStringTimeSpanClauses` declares `GreaterThan(this IMustClause _, string? value, TimeSpan
threshold, ...)` and `LessThan(...)` as overloads of the typed `GreaterThan(TimeSpan value, ...)` /
`LessThan(...)` clauses — not as separately suffixed `GreaterThanTimeSpan`/`LessThanTimeSpan`
members. The string `DurationBetween` overloads are likewise unsuffixed. The plan's prior table row
claiming `Must.Be.GreaterThanTimeSpan(string …)` / `LessThanTimeSpan(string …)` "if present" was
wrong: no such suffixed members exist, and the string overloads are always present.

## Binding contract

Replace the bad table row with exactly:

| From | To |
|---|---|
| `Must.Be.GreaterThan(string …)` / `LessThan(string …)` (`MustStringTimeSpanClauses`, overloads of the typed names) | `Must.Be.LongerThan(string …)` / `Must.Be.ShorterThan(string …)` |

The string Must overloads rename to the same unsuffixed `LongerThan`/`ShorterThan` names as the typed
overloads — this is a rename of an existing overload set, not a new API or a new dependency.

## Exact within-scope call-site renames

Within the already-confirmed 15 U4a files (see
[the selector ruling](temporal-u4a-selector-ruling-fable.md)):

- `tests/PineGuard.MustClauses.UnitTests/MustStringTimeSpanClausesTests.cs`: rename test method names
  and calls that reference `GreaterThan`/`LessThan` to `LongerThan`/`ShorterThan`, mirroring the typed
  `MustTimeSpanClausesTests.cs` renames in the same unit.
- `tests/PineGuard.MustClauses.UnitTests/MustStringTimeSpanClausesTestData.cs`: rename its
  `GreaterThan`/`LessThan` TestData group names to `LongerThan`/`ShorterThan`.
- `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.TimeSpan.cs`: rename its `GreaterThan`/
  `LessThan` fixture group names to mirror Core `StringRules.TimeSpan.IsLongerThan`/
  `IsShorterThan`.

No path is added by this ruling; all renamed members and files were already within U4a's confirmed
15-file list.

## Exclusions

- **T1-U4**: the seven completed Guard boundary-polarity files are untouched.
- **T1-U4b**: owns the Guard/Fluent/DataAnnotations TimeSpan rename, including the string Guard
  callers and their TestData — those pick up the corrected string Must names later in U4b, not here.
- **T1-U3**: owns TimeOnly midnight-crossing Between/NotBetween; untouched.
- **`NumberRules` / `RuleComparison`**: never touched by this or any T1-U4a rename.
- **Every other unrelated family**: excluded; this ruling adds no file and no new dependency, and
  changes no behavior — only names.
