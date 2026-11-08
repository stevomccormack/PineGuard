# T1-U4 D-9 scoped commit proposal

**Status: OWNER-APPROVED — commit completed.** Local commit `3902168e4f5e3eae04a739a9d9d01733d928c3a5`
(`fix(guard): correct TimeSpan boundary polarity`) matches this proposal exactly. Receipt:
[`temporal-u4-commit-receipt.md`](temporal-u4-commit-receipt.md).

Claude Fable 5.1 returned **U4 READY** in the persisted review session. The public final result is [`T1-U4-fable-closure-final.json`](../../../artifacts/reviews/T1-U4-fable-closure-final.json); the binding seven-file sweep contract and red-count adjudication are [`temporal-u4-sweep-ruling-fable.md`](temporal-u4-sweep-ruling-fable.md), [`temporal-u4-sweep-ruling-fable.json`](../../../artifacts/reviews/temporal-u4-sweep-ruling-fable.json), and [`temporal-u4-red-check-adjudication.json`](../../../artifacts/reviews/temporal-u4-red-check-adjudication.json).

## Proposed commit

**Subject:** `fix(guard): correct TimeSpan boundary polarity`

**Body:**

```text
Complement delegated strict comparisons so equality follows the requested inclusion semantics. Add boundary cases across TimeSpan, DateOnly, and string DateTimeOffset guard data.

Co-Authored-By: Claude Fable 5.1 <noreply@anthropic.com>
```

The proposed commit contains exactly these seven current GuardClauses source/test files:

1. `src/PineGuard.GuardClauses/GuardTimeSpanClauses.cs`
2. `src/PineGuard.GuardClauses/GuardStringTimeSpanClauses.cs`
3. `tests/PineGuard.GuardClauses.UnitTests/GuardTimeSpanClausesTestData.cs`
4. `tests/PineGuard.GuardClauses.UnitTests/GuardStringTimeSpanClausesTestData.cs`
5. `tests/PineGuard.GuardClauses.UnitTests/GuardDateOnlyClausesTestData.cs`
6. `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateOnlyClausesTestData.cs`
7. `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateTimeOffsetClausesTestData.cs`

The current diff is **7 files changed, 65 insertions(+), 29 deletions(-)**; `git diff --check` passed. The scope-specific `git status` showed only the seven paths above modified and no untracked files within either GuardClauses project. The index was empty. The repository helper's non-mutating preview was run with:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/git/Run-Commits.ps1 -Scope GuardClauses -IncludeTests -Message 'fix(guard): correct TimeSpan boundary polarity' -WhatIf
```

It exited 0 and previewed the two owned project directories. It does not enumerate each changed file in its preview, so the exact seven-file list above comes from the separate scoped status/name check. Recheck the index, the seven-file list, diff stat, and this dry run immediately before an approved commit; stop if anything else has entered either directory or the index.

## Verification evidence

- Failing-first focused tests on net10.0: 8 failed, 10 passed, 0 skipped (18 total); Fable adjudicated all eight equal-boundary twins as the expected red set.
- After the four source complements, the same focused tests passed 18/18 on net10.0.
- Full `GuardClauses` isolated coverage passed for both TFMs: 2,978/2,978 tests each, 3,566/3,566 lines, 1,196/1,196 branches, 57 classes. Archived reports and hashes are in [`+ t1-u4-guardclauses-coverage-20260927/manifest.json`](../../../artifacts/reviews/receipts/t1-u4-guardclauses-coverage-20260927/manifest.json).
- Research reports Format All passed, CompilerDiagnostics All passed with zero warnings, and Tests All passed.
- The specialized `audit nullability` passed with Rule07=0 findings. The canonical shared `audit --gate` passed with Rule11, Rule12, and Rule50 all at zero findings.
- Fable directly checked the live seven files against the binding scope and regression contract and returned U4 READY.

## Authorization boundary

The applicable commit skill is [`.agents/skills/commit/SKILL.md`](../../../.agents/skills/commit/SKILL.md) (absolute path: `D:\Steve McCormack\GitHub\@stevomccormack\PineGuard\.agents\skills\commit\SKILL.md`). Its Step 3 says: “Commits are never auto-approved: propose the conventional-commits message and the staged file list, then wait for the user to confirm before running `git commit`.” The T-00 §8 local-commit workflow does not remove that confirmation requirement. The owner confirmed this exact message and seven-file set, and the commit was made as `3902168e4f5e3eae04a739a9d9d01733d928c3a5`. That approval and this commit are scoped to the local commit only — they do not authorize any merge or push to `main`, which still requires its own explicit owner confirmation (or a recorded program-specific preauthorization) under T-00 §8. See [`temporal-u4-commit-receipt.md`](temporal-u4-commit-receipt.md) for the post-commit evidence.
