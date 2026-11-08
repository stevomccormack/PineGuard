<!-- metadata_header
type: receipt
id: temporal-u3-source-review-fable
version: 1.0
status: source-review-revise
last_updated: 2026-09-27
-->

# T1-U3 — Fable source review receipt

**Reviewer:** Claude Fable 5.1, persistent session `c5e314e0-c32a-4f3d-9bb3-0f684a289797`  
**Result:** source behavior is correct across layers, but two corrections are required before the unit gate. This is not unit acceptance.

## Confirmed contract

Core `TimeOnlyRules.IsBetween` wraps when `start > end`, preserves inclusive/exclusive endpoint behavior, and treats equal endpoints as a singleton/empty interval. Must checks retain existing null/parse precedence before negation; no TimeOnly reversed-bound misuse guard or code remains. Non-TimeOnly range families and `TimeOnlyRange` inverted-bound behavior remain unchanged. Guard, Fluent, and DataAnnotations preserve Must result/code ownership and the U3 tuple renames are consistent across the reviewed 31 paths.

## Binding source corrections

1. Add `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.TimeOnly.cs` to T1-U3 ownership. Rename its `IsBetween` tuple fields from `min`/`max` to `start`/`end`, and update the existing string Must/Guard fixture projections in `MustStringTimeOnlyClausesTestData.cs` and `GuardStringTimeOnlyClausesTestData.cs` to use the renamed fields. Add string Core `IsBetween` wrap-inside, wrap-outside, and equal-endpoint scenarios, mirroring the typed fixture. This closes the real test-data mismatch; tuple element names compile despite mismatching the method parameter names, so compilation alone cannot detect it.
2. In `tests/PineGuard.FluentValidation.UnitTests/FluentTimeOnlyExtensionsTestData.cs`, remove the unused `WrapStart` and `WrapEnd` fields. They are expected to trigger CS0414 under `TreatWarningsAsErrors`; the test methods already carry their own bounds.

Fable also recommended two small fixture-quality corrections within the existing U3-owned test-data files: use the shared fixture members for `WrapInside`/`WrapOutside` rather than inline values in Fluent TestData, and use `F.IsKnownTimes.T2000` in the two Must TestData rows instead of `new TimeOnly(20, 0)`.

## Follow-up documentation drift

- `docs/ai/specs/data-annotations/unit-test.md` Pattern D still uses `BetweenTimeOnlyAttribute(min, max)` and MinParam/MaxParam; Fable classified this as a non-gate documentation follow-up for U6 or T-07.
- `docs/ai/plans/core-common-api-decisions.md` still uses `IsBetween(value, min, max, inclusion)`; it is a non-gate living-reference follow-up.
- `src/PineGuard.Core/Codes/MustCodes.Time.cs` retains a minimum/maximum summary comment; leave it to the later T1-U4a single-writer unit.
- Single-line test method bodies in the six Must/Guard test files are inherited family debt, not a U3 regression.

## Binding documentation follow-up assignment

A narrow follow-up in the same persistent Fable session assigned both stale documentation items to
**T-07 U2, “Brain reconciliation (roadmap, competitive analysis, decisions, specs)”**, tracked under
`T7-U1…U6` in `temporal-orchestration.md`. It remains out of T1-U3 acceptance and T1-U6; it changes
no behavior. T7-U2 already owns the two documentation families, runs after Phases 1–6 merge, in
parallel with T7-U1, and before T7-U3/T7-U5. If completed earlier, only a standalone docs commit
after Phase 1 merge is allowed; do not fold it into T-01.

The exact follow-up is: update Pattern D in
`docs/ai/specs/data-annotations/unit-test.md` from min/max and MinParam/MaxParam to start/end and
StartParam/EndParam; update both A-3 and A-8 references in
`docs/ai/plans/core-common-api-decisions.md` to distinguish TimeOnly start/end with wrap from the
other types' min/max contract. Keep A-3/A-8 open and unchanged. T7-U2 must run `audit --gate`
(including doc-links), grep for the stale phrases, and receive its own Claude Fable 5.1 review; no
build, test, or coverage run is required for this doc-only work.

## Ownership manifest supersession

[`artifacts/reviews/t1-u3-ownership-manifest.txt`](../../../artifacts/reviews/t1-u3-ownership-manifest.txt) recorded the historical 31-file evidence and is
preserved unchanged. It is superseded by the authoritative exact 32-path audit at
[`artifacts/reviews/t1-u3-final-ownership-audit.json`](../../../artifacts/reviews/t1-u3-final-ownership-audit.json), which adds
`tests/PineGuard.Testing/Fixtures/StringRulesFixtures.TimeOnly.cs` per Fable's approved fixture
addition above. Neither artifact is modified by this note.

## Required validation before U3 closure

Fable ran no format, diagnostics, test, audit, or coverage command for U3. After the binding corrections, run the full T-00 §9 gate for both target frameworks across the five U3 scopes, plus `audit --gate`, `audit must-codes`, and `audit nullability`. A later Claude Fable 5.1 unit closure is still required.
