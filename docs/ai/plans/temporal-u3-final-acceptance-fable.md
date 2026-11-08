<!-- metadata_header
type: receipt
id: temporal-u3-final-acceptance-fable
version: 1.0
status: final-acceptance
last_updated: 2026-09-27
-->

# T1-U3 — Fable final acceptance receipt (public verdict recovery)

**Reviewer:** Claude Fable 5.1, persisted session `c5e314e0-c32a-4f3d-9bb3-0f684a289797` (the same
resumed session that produced the source review and the missed-branch ruling cited below).
**Date:** 2026-09-27.
**Nature of this document:** this is a public verdict recovery / documentation record of Fable's
already-issued final ACCEPT for T1-U3, not a new review. It transcribes the verdict and the
already-retained gate evidence; it does not quote, reconstruct, or synthesize any reviewer reasoning
beyond what is already public in the cited receipts below. Fable did not execute any format,
diagnostics, test, coverage, or audit command itself; those commands were run separately and their
evidence is retained under `artifacts/`.

## Sequence in this persisted session

1. **Source review (revise)** — [`temporal-u3-source-review-fable.md`](temporal-u3-source-review-fable.md):
   confirmed the Core/Must/Guard/Fluent/DataAnnotations contract, then required two binding source
   corrections (extend ownership to `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.TimeOnly.cs`
   with a start/end rename and new wrap/equal-endpoint string cases; remove the unused
   `WrapStart`/`WrapEnd` fields from `FluentTimeOnlyExtensionsTestData.cs`) before the unit gate.
2. **Missed-branch ruling** — [`temporal-u3-missed-branch-fable.md`](temporal-u3-missed-branch-fable.md):
   after the corrections landed, the Core net8.0 isolated coverage run showed one uncovered branch
   (`value > start` in the exclusive midnight-wrap arm of `TimeOnlyRules.IsBetween`); Fable specified
   exactly one added fixture scenario, `WrapLateNightInsideExclusive`, to close it.
3. **Final ACCEPT** — this document records that verdict. After both corrections and the added
   scenario landed and the gate evidence below was produced, Fable's verdict in this persisted
   session is **ACCEPT**.

## Exact ownership: 32 paths, 20 TimeOnlyRange exclusions, no unexpected/missing paths

The accepted ownership is exactly the `allowedPaths` array (32 entries) in
[`artifacts/reviews/t1-u3-final-ownership-audit.json`](../../../artifacts/reviews/t1-u3-final-ownership-audit.json),
which supersedes the historical 31-file manifest at
[`artifacts/reviews/t1-u3-ownership-manifest.txt`](../../../artifacts/reviews/t1-u3-ownership-manifest.txt)
by adding `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.TimeOnly.cs` per the source-review
correction above. The same audit JSON records:

- `excludedPaths`: 20 entries, all under the TimeOnlyRange family (`TimeOnlyRange.cs`,
  `TimeOnlyRangeRules.cs`, `StringUtility.TimeOnlyRange.cs`, and their Must/Guard/Fluent/
  DataAnnotations clauses, tests, TestData and fixtures) — U3 never touches inverted-bound
  `TimeOnlyRange`/`TimeOnlyRangeRules`/`MustTimeOnlyRangeClauses` behavior (A-09 unchanged).
- `unexpectedChangedPaths`: `[]` — no path outside the 32-file allow-list changed.
- `missingAllowedPaths`: `[]` — every one of the 32 allowed paths actually changed.
- `allowedButUnchangedPaths`: `[]`.

## Accepted U3 contract and boundaries

Per the source review's confirmed contract, unchanged by the final ACCEPT: Core
`TimeOnlyRules.IsBetween` wraps when `start > end`, preserves inclusive/exclusive endpoint behavior,
and treats equal endpoints as a singleton/empty interval. Must checks retain existing null/parse
precedence before negation; no TimeOnly reversed-bound misuse guard or code remains. Non-TimeOnly
range families and `TimeOnlyRange` inverted-bound behavior are unchanged. Guard, Fluent, and
DataAnnotations preserve Must result/code ownership, and the U3 tuple renames (`min`/`max` →
`start`/`end`) are consistent across all 32 owned paths.

## Gate evidence (already retained; not run by Fable)

- **Shared gates**: `All`-scope format passed, `All`-scope compiler diagnostics returned 0 warnings,
  and `All`-scope tests passed — 19,069 tests on net8.0 and 19,094 tests on net10.0, across 15
  assemblies each framework, 0 failures and 0 skips. Logs:
  [`format-all.log`](../../../artifacts/reviews/t1-shared-gates-after-branch-case/logs/format-all.log),
  [`diagnostics-all.log`](../../../artifacts/reviews/t1-shared-gates-after-branch-case/logs/diagnostics-all.log),
  [`tests-all-net8.log`](../../../artifacts/reviews/t1-shared-gates-after-branch-case/logs/tests-all-net8.log),
  [`tests-all-net10.log`](../../../artifacts/reviews/t1-shared-gates-after-branch-case/logs/tests-all-net10.log);
  TRX files under `artifacts/reviews/t1-shared-gates-after-branch-case/test-results/net8.0/` and
  `.../net10.0/`.
- **Isolated coverage**: 12 gates (`Core`, `MustClauses`, `GuardClauses`, `FluentValidation`,
  `DataAnnotations`, `Testing` × `net8.0`/`net10.0`) all exit 0 at 100% filtered line and branch
  coverage. Matrix receipt:
  [`artifacts/reviews/t1-u3-coverage-branchfix/matrix-summary.json`](../../../artifacts/reviews/t1-u3-coverage-branchfix/matrix-summary.json),
  SHA256 `9B00834A01B4BF30CEAF2AF570AA9534E9812C8420A6246C377E126ADC8541D9` (as supplied for this
  citation; not independently recomputed by this document). The Core `net8.0` Cobertura report SHA256
  is `6EBD9CCCC5DCBF73533A4E6040EAFE921C4255BA3A846604F796D4B77D807BEC`, matching the retained matrix
  entry; this run's Core net8.0 branch count is 3197/3197, incorporating the `WrapLateNightInsideExclusive`
  fixture from the missed-branch ruling (the ruling was raised against a prior 3196/3197 result).
- **Audits**: `audit --gate`, `audit must-codes`, and `audit nullability` passed with zero findings on
  the frozen source tree. **Evidence gap**: no U3-specific audit log or summary artifact is currently
  retained under `artifacts/` to cite for this line — a search found only the unrelated
  `t1-u2-u2b-post-fable-fixes-audit-must-codes.log` / `...-audit-nullability.log` pair, which belong to
  T1-U2/U2b, not T1-U3. This line records the stated result without a citable retained U3 artifact.

## Commit status

U3 is accepted on the evidence above, but the local commit is pending separate explicit user
approval; no staging or commit has occurred. T7-U2's stale-documentation cleanups
(`docs/ai/specs/data-annotations/unit-test.md` Pattern D, `docs/ai/plans/core-common-api-decisions.md`
A-3/A-8) identified during the U3 source review are tracked separately under T7-U2 and are not part
of this acceptance.

## Sources

- [`temporal-u3-source-review-fable.md`](temporal-u3-source-review-fable.md) — source review, same
  persisted session.
- [`temporal-u3-missed-branch-fable.md`](temporal-u3-missed-branch-fable.md) — missed-branch ruling,
  same persisted session.
- [`artifacts/reviews/t1-u3-final-ownership-audit.json`](../../../artifacts/reviews/t1-u3-final-ownership-audit.json) —
  32-path ownership audit.
- [`artifacts/reviews/t1-u3-coverage-branchfix/matrix-summary.json`](../../../artifacts/reviews/t1-u3-coverage-branchfix/matrix-summary.json) —
  coverage matrix.
- `artifacts/reviews/t1-shared-gates-after-branch-case/` — shared format/diagnostics/test logs and TRX results.
