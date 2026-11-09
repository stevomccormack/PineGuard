<!-- metadata_header
type: receipt
id: temporal-u4a-baseline-key-ruling-fable
version: 1.0
status: binding-ruling
last_updated: 2026-09-27
-->

# T1-U4a/U4b — Fable baseline-key ruling receipt

> Historical review receipt: declarations and ownership below describe the reviewed Phase 1
> commit. Core/Common integration subsequently applies `InRange` / `OutOfRange` and
> parsed-type suffixes (`LongerThanTimeSpan` / `ShorterThanTimeSpan` for string input),
> while retaining D-16 duration vocabulary. Use the [current decision record](core-common-api-decisions.md)
> for the integrated API; earlier unsuffixed-string directions are superseded.

**Reviewer:** Claude Fable 5.1, resumed persisted session `8fc20ebe-d70e-47fe-8e1b-01d79e3bb527`.
**Date:** 2026-09-27.
**Result:** binding ruling on the U4a/U4b baseline treatment; a narrow rename-move exception to the
deletions-only baseline ratchet, not a new policy. Exact policy and disclosure changes are
Fable-owned; no owner re-signing per T-00 §0 and A-01.

## Decision: a narrow rename-move exception, not deletion-only

At the U4a/U4b cohort head, and only there, `apps/cli/config/baseline.json` is edited so that each
obsolete key is replaced by the key the analyzer now emits for the *same* member:

- Delete `rules-usage:StringRules.TimeSpan.IsGreaterThan` (line 658), add
  `rules-usage:StringRules.TimeSpan.IsLongerThan`.
- Delete `rules-usage:StringRules.TimeSpan.IsLessThan` (line 659), add
  `rules-usage:StringRules.TimeSpan.IsShorterThan`.

The same treatment applies to any U4b-renamed test-name keys, currently
`test-structure:missing:…StringTimeSpanAttributesTests.cs:GreaterThanTimeSpanString` (line 1315) and
`…LessThanTimeSpanString` (line 1316), each replaced by the key for its U4b-renamed test identity
(candidate names `LongerThanTimeSpanString`/`ShorterThanTimeSpanString`, or whatever exact test names
U4b lands).

Net key count must be unchanged: one deleted key per added key.

## Why not deletion-only

The debt itself is untouched: `StringRules.TimeSpan.IsLongerThan`/`IsShorterThan` are uncalled by
Must for the same parse-once reason as the already-baselined `IsDurationBetween` (line 657), and
resolving that is an API change outside a rename unit. Deletion-only would leave two unbaselined
findings, and T-00 §9 requires the phase-cohort `pnpm -C apps/cli exec tsx src/index.ts audit` (all
rules with baseline) to exit 0. Hiding them behind `--gate` (the rule is `gate: false`) would satisfy
the letter of one command while failing the phase gate; that is not an honest option.

## Why this is not new debt

The "deletions only" ratchet exists to stop debt growing. A key whose only change is the analyzer's
identity string for a renamed member has the same file, the same declaration, the same finding
message shape and the same count. It is a move, not an addition.

## Exact policy and disclosure changes (Fable-owned, no owner re-signing per T-00 §0 and A-01)

1. T-00 §9, baseline paragraph, adds one sentence: "A rename unit may move a baseline key whose
   member it renames: one deleted key per added key, net count unchanged, each pair cited in the
   cohort receipt with the old and new declaration lines. Any other addition remains prohibited."
2. This receipt is recorded and linked from T-00 §9 and the T-01 U4a cohort paragraph, in the same
   form as the three existing U4a rulings.
3. The cohort commit body, the U4b receipt and the Phase 1 final report list every moved pair
   explicitly and attach `git diff --stat apps/cli/config/baseline.json` showing equal insertions and
   deletions. This satisfies the T-00 §0 "disclose it in the PR and final report" clause.
4. Acceptance authority is this ruling; the owner is informed through item 3, not asked to re-sign.

## Visibility guard

The cohort receipt must show the `rules-usage` finding count before and after the baseline edit is
identical, and that each added key's message cites `src/PineGuard.Core/Rules/StringRules.TimeSpan.cs`
at the renamed declaration. A key that cannot be paired with a deleted key is rejected.

## Timing

Do not update the baseline before the cohort head. Only at the U4a/U4b cohort head — after U4b's
rename lands — is the paired move made. Before that, at the U4a handoff, the four old keys remain in
place and are the only permitted `apps/`-path matches for Sweep B of
[the cohort ruling](temporal-u4a-u4b-cohort-ruling-fable.md).

## Sources

- [`artifacts/reviews/t1-u4a-implementation-20260927/fable-followup-result.md`](../../../artifacts/reviews/t1-u4a-implementation-20260927/fable-followup-result.md),
  Ruling 1.
- [`artifacts/reviews/t1-u4a-implementation-20260927/fable-residue-count-result.md`](../../../artifacts/reviews/t1-u4a-implementation-20260927/fable-residue-count-result.md),
  §§2–4 (the four-key handoff allowance and cohort-head criterion).
- [`temporal-00-program.md`](temporal-00-program.md), §0 (exceptional reversal/disclosure authority)
  and §9 (baseline verification gate).
- [Astra decision register A-01](temporal-astra-decisions.md).
