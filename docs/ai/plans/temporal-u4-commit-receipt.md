# T1-U4 D-9 commit receipt

**This is local commit evidence only. No push or merge to `main` has occurred.**

## Commit

- **SHA:** `3902168e4f5e3eae04a739a9d9d01733d928c3a5`
- **Subject:** `fix(guard): correct TimeSpan boundary polarity`

## Committed paths (exactly seven)

1. `src/PineGuard.GuardClauses/GuardTimeSpanClauses.cs`
2. `src/PineGuard.GuardClauses/GuardStringTimeSpanClauses.cs`
3. `tests/PineGuard.GuardClauses.UnitTests/GuardTimeSpanClausesTestData.cs`
4. `tests/PineGuard.GuardClauses.UnitTests/GuardStringTimeSpanClausesTestData.cs`
5. `tests/PineGuard.GuardClauses.UnitTests/GuardDateOnlyClausesTestData.cs`
6. `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateOnlyClausesTestData.cs`
7. `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateTimeOffsetClausesTestData.cs`

`git show` on this commit confirms exactly these seven paths, matching
[`temporal-u4-commit-proposal.md`](temporal-u4-commit-proposal.md) — 65 insertions(+), 29 deletions(-).

## Authorized-scope confirmation

Per the commit skill's Step 3 (`.agents/skills/commit/SKILL.md`), the owner confirmed this exact
conventional-commits message and this exact seven-file set before `git commit` was run. This
approval covers only this specific local commit. Per T-00 §8, a separate explicit owner
confirmation (or a recorded program-specific preauthorization) is still required before any
merge or push of this work to `main`.

## Formatter pre-commit hook

The configured pre-commit hook ran `dotnet format PineGuard.slnx --include` on the seven staged
C# files and exited 0. The only output was a workspace-loading warning (non-blocking).

## Post-commit state

`git show` confirmed the exact seven paths listed above. The index was empty after the commit.
No push or merge occurred.

## Fable U4 READY closure

Claude Fable 5.1 returned **U4 READY** in the persisted review session:
[`T1-U4-fable-closure-final.json`](../../../artifacts/reviews/T1-U4-fable-closure-final.json).
The binding seven-file sweep contract and red-count adjudication:
[`temporal-u4-sweep-ruling-fable.md`](temporal-u4-sweep-ruling-fable.md),
[`temporal-u4-sweep-ruling-fable.json`](../../../artifacts/reviews/temporal-u4-sweep-ruling-fable.json),
[`temporal-u4-red-check-adjudication.json`](../../../artifacts/reviews/temporal-u4-red-check-adjudication.json).

## Prior verification evidence (from the approved proposal)

- Failing-first focused tests on net10.0: 8 failed, 10 passed, 0 skipped (18 total); Fable
  adjudicated all eight equal-boundary twins as the expected red set.
- After the four source complements, the same focused tests passed 18/18 on net10.0.
- Full `GuardClauses` isolated coverage passed for both TFMs: 2,978/2,978 tests each,
  3,566/3,566 lines, 1,196/1,196 branches, 57 classes. Archived reports and hashes:
  [`+ t1-u4-guardclauses-coverage-20260927/manifest.json`](../../../artifacts/reviews/receipts/t1-u4-guardclauses-coverage-20260927/manifest.json).
- Research reports Format All passed, CompilerDiagnostics All passed with zero warnings, and
  Tests All passed.
- The specialized `audit nullability` passed with Rule07 = 0 findings. The canonical shared
  `audit --gate` passed with Rule11, Rule12, and Rule50 all at zero findings.
- Fable directly checked the live seven files against the binding scope and regression contract
  and returned U4 READY.
