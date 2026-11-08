# T-01 U0 provider and isolated-coverage closure — Claude Fable 5.1

**Status:** Both provider fixes and the isolated-coverage tooling repair accepted; T1-U0 is ready for its authorized commit steps.
**Reviewer:** Claude Fable 5.1, resumed persisted code-review session `04825da4-0e28-4721-b665-2adbc91d7796`.
**Date:** 2026-09-27. Review was read-only and limited to the provider/test and eight coverage-tooling files against the prior binding review. No .NET commands were run by the reviewer.
**Public result:** `artifacts/reviews/T1-U0-fable-closure.json` in the temporal worktree. Prior binding review: `artifacts/reviews/T1-U0-fable-review.md` (SHA-256 `C3B78C75BA0725486DEAC8661946FF807171EDE17CAE90DF5489E87ECD9F419B`).

## Accepted findings and evidence

Provider fixes accepted: the three provider tests use `Assert.Same` for the UTC identity contract; all three provider XML remarks distinguish controlled `GetUtcNow`/`GetLocalNow` reads from inherited real timestamp/timer behavior and point to `FakeTimeProvider`; CountingTimeProvider proves count 0 at construction and 1 after the first read. Public signatures, default behavior, normalization/exhaustion cases, and test structure remain within the prior approval.

Coverage repair accepted: isolated coverlet publish now propagates each selected TFM with `-f`, fixing the reproduced NETSDK1129 failure for multi-target projects. The shared resolver/publish helpers preserve the unbound non-isolated `dotnet test` path; diagnostics retain command, project, framework, exit code and SDK output; cleanup is tested. DotCover reuses the shared framework resolver/publish helper. Fast/slow Pester tests cover resolver vectors, exact argument rendering, failure details, the required isolated run, and temporary-directory cleanup. The documented `-Isolated` workaround was removed; the 100% coverage gate was not weakened.

Fable accepted these reported gates: Format All; CompilerDiagnostics All with zero warnings; canonical tests with Testing.UnitTests 678/678 on each of net8 and net10; audit gate true with Rule12 and Rule50 zero and one unrelated pre-existing Rule11 doc-link finding at `.claude/agent-memory/test-writer/must-validator-layer-test-pattern.md:33`; isolated Testing coverage across all 15 projects on both TFMs at 6389/6389 lines and 124/124 branches (100%); fast Pester 9/9 and Slow Pester 4/4.

## Non-blocking notes

Fable noted that a validity retry clears the shared per-project report directory in a multi-pass run, while only the newest result is consumed as before; per-TFM gating remains the caller's responsibility through `-Framework`. It also recommended adding a clause to the coverage script's `Framework` parameter help so it mentions the isolated publish restriction. Neither note blocks the unchanged gate.

## Audit-status correction (2026-09-27)

The audit statement above preserves what was reported to and accepted by the U0 closure review, but its description of the Rule11 finding as "unrelated pre-existing" and compatible with a green gate was inaccurate. The canonical shared `audit --gate` command was rerun against the frozen combined temporal worktree: it exited 1 with Rule11 `doc-links`=1, Rule12 `surface-parity`=0, and Rule50 `test-files`=0. The Rule11 finding identified the stale reference on line 33 of `.claude/agent-memory/test-writer/must-validator-layer-test-pattern.md` to a nonexistent `+ PineGuard.Core/MustClauses/MustValidatorCast.cs` path.

Claude Sonnet 5 corrected only that reference to the verified repository-relative path `src/PineGuard.Core/MustClauses/MustValidatorCast.cs`; the actual source is `src/PineGuard.Core/MustClauses/MustValidatorCast.cs`. `git diff --check` passed for the correction. The canonical shared audit was rerun elevated with the existing pinned pnpm cache and exited 0: Rule11=0, Rule12=0, Rule50=0, total findings=0. This later clean run is authoritative for the current combined snapshot. It supersedes the earlier audit status without erasing the historical failed result or changing U0's accepted provider/coverage code findings. The memory-document correction is an additional documentation file in the current review/commit inventory; it is outside T1-U4's GuardClauses unit scope.

## DOC-DEBT-01 origin/main audit addendum (2026-09-27)

The prior corrections above describe the combined temporal worktree snapshot only. Claude Fable 5.1,
resuming this same session (`04825da4-0e28-4721-b665-2adbc91d7796`), separately reviewed the
`origin/main` base at `84dff7e195a114b031f7de13783f81513b9e4e49` and found the canonical shared
`audit --gate` exits **1** with **34 findings**, all under Rule11 `doc-links`, **0 attributable to
U0's own files**. This is **base-branch documentation debt, not a U0 overlay regression**, and it is
not accurately described as "unrelated/pre-existing and nonblocking" — it is a real red gate on
`origin/main` itself. The full ruling, source-cited with its JSON path and SHA-256, is mirrored at
[`temporal-u0-origin-audit-fable.md`](temporal-u0-origin-audit-fable.md); the authoritative source is
`artifacts/reviews/t1-u0-standalone-commit-validation/fable-doc-debt-01-ruling.json` (SHA-256
`A8944A803041B640FA0AEDF2C7F0710E9A3D0464A1B87FCE1A431F9893CD820A`).

U0's own unit acceptance is unchanged (`ACCEPTED`). Local commit readiness is `READY after closure
receipt amendment` — this addendum is that amendment. Phase merge readiness is `BLOCKED` until
DOC-DEBT-01 (31-file scope, worktree `.claude/worktrees/readiness-doc-links`, branch
`feature/readiness-doc-links`) is merged to `main`, the corrected `origin/main` is merged into the
temporal worktree, and the post-merge `audit --gate` exits 0. This addendum does not erase the
historical reported result (§"Fable accepted these reported gates" above) or the prior actual
correction note; it adds the separately discovered base-branch finding and its consequences.
