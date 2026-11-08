<!-- metadata_header
type: plan
id: temporal-u2c-fable-ruling
version: 1.0
status: pending
last_updated: 2026-09-27
-->

# T1-U2c — Fable Ruling Receipt (String DateOnly consumers of guarded calendar references)

> Binding ruling: [`artifacts/reviews/T1-U2-U2b-string-followup-ruling.json`](../../artifacts/reviews/T1-U2-U2b-string-followup-ruling.json)
> (Claude Fable 5.1, generated 2026-09-27T02:16:17Z). Full unit specification:
> [temporal-01-foundation.md §3 "U2c"](temporal-01-foundation.md). Tracker row:
> [temporal-orchestration.md §1](temporal-orchestration.md).

## What this ruling decided

Claude Fable 5.1's follow-up review of the T1-U2/U2b string-DateOnly gap found that
`MustStringDateOnlyClauses.cs`'s five clock-consuming methods call the Core DateOnly predicates
directly and do not inherit T1-U2/U2b's guarded-reference fix. The ruling creates a new, separate
unit — **T1-U2c** — to close that gap. T1-U2c is explicitly **not** part of the T1-U2/U2b atomic
acceptance cohort and requires no changes to U2/U2b's files.

## Scope

Exactly three files; no fixture, adapter, Core, codes or `.csproj` change:

- `src/PineGuard.MustClauses/MustStringDateOnlyClauses.cs`
- `tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTests.cs`
- `tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTestData.cs`

`GuardStringDateOnlyClauses`, `FluentStringDateOnlyExtensions` and `StringDateOnlyAttributes`
inherit the fix automatically through their existing calls into these five Must clauses and are not
edited. `MustStringDateTimeOffsetClauses` and the Core `StringRules.DateOnly` predicates are
unaffected.

## Dependency and sequencing

Depends on **T1-U2 and T1-U2b accepted**. Must complete before **T1-U6** (Phase 1 review + merge)
and before any T-04 string-DateOnly work.

## Gates

- Shared `All` format, diagnostics and tests.
- Isolated `MustClauses` coverage 100%/100% on net8.0 and net10.0.
- `audit --gate`.
- `audit must-codes` (no new constants).
- `audit nullability`.

## Status

**Pending.** Implementation and acceptance are pending; nothing in this receipt or the linked
plan/tracker sections marks any code done.
