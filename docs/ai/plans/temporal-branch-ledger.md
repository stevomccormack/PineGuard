<!-- metadata_header
type: plan
id: temporal-branch-ledger
version: 1.0
status: active
last_updated: 2026-09-27
-->

# Temporal branch and worktree ledger

Companion to [the orchestration tracker](temporal-orchestration.md). This is a dated inventory and preservation record, not a merge approval or cleanup authorization. No current cleanup is authorized. This ledger and the source worktree's temporal plans are presently uncommitted and must eventually pass the existing documentation commit flow. Actual Claude Fable 5.1 owns decisions, reviews and councils; entries below transcribe existing rulings and execution receipts only.

Raw inventory: [inventory.json](../../../artifacts/reviews/temporal-branch-inventory-20260927/inventory.json), SHA-256 `2C49251045172F79C9AE339A69682AA586FDDC937AD43D66E6350DA387157AA2`.

## Current checkouts

Paths are absolute. Dirty counts are inventory observations before this ledger edit, not a substitute for a fresh file-by-file accounting at retirement. Logical custodians describe responsibility, not exclusive process ownership.

| Branch | Worktree | HEAD | Purpose / custodian | Committed versus uncommitted | Destination and retirement blockers |
|---|---|---|---|---|---|
| `main` | `D:/Steve McCormack/GitHub/@stevomccormack/PineGuard` | `9ba08c743233c8e072a474f7d7773fb3f934e12e` | Owner integration destination | Clean; one local commit ahead of `origin/main` (`84dff7e`) | Primary checkout; preserve. No push or temporal phase merge claimed. |
| `codex/doc-debt-main-integration` | `C:/Users/stevo/.codex/worktrees/doc-debt-main-integration/PineGuard` | `d1b1793b8f5ba2c20d27c8e7058b514f79989a2a` | DOC-DEBT-01/02 integration / documentation team | Clean; local integration committed, four commits ahead of main | Existing prerequisite destination is main, then corrected main into foundation with post-merge gates. Not yet on main or foundation. Preserve receipts and ignored review evidence. |
| `codex/temporal-foundation` | `C:/Users/stevo/.codex/worktrees/temporal-foundation/PineGuard` | `3b5e90fe28aa6f958cd2ca58ea049952380fe43c` | Reviewed Phase 1 consolidation / implementation team | Clean after Package A commits. U0, coverage fix, preserved U4, U1, U2/U2b, U2c, U3 and U4a/U4b committed | Shared final-head gates running before U5. Eventual phase integration remains gated; dated raw inventory ancestry counts predate the three latest commits. Preserve until final gates, reviewed integration and evidence retention. |
| `feature/temporal-u0-standalone` | `C:/Users/stevo/.codex/worktrees/temporal-u0-commit/PineGuard` | `869ce373df2e21d6a33cd1584acba45628daa0a4` | Exact U0 commit / U0 team | Clean; U0 committed | U0 is in foundation ancestry. This alone does not authorize retirement: preserve ignored evidence and verify no active owner/process or unique work. |
| `codex/temporal-readiness-docs` | `C:/Users/stevo/.codex/worktrees/temporal-u0-verification/PineGuard` | `3422d61c9fcaa84bf4e3bfbac7258b3127d95cb0` | Historical U0 verification, readiness/docs/coverage candidate / U0 and documentation teams | Dirty: 18 porcelain entries; includes memory, FixedTimeProvider source/tests and coverage tooling; full exact status must be recaptured | No blanket merge into foundation. Map each candidate to reviewed committed replacement or separately retain it. Outstanding dirty work, untracked files and ignored evidence prohibit retirement. |
| `feature/temporal-timezone` | `D:/Steve McCormack/GitHub/@stevomccormack/PineGuard/.claude/worktrees/temporal-timezone` | `3902168e4f5e3eae04a739a9d9d01733d928c3a5` | Original accepted source, plans, tooling and review provenance / temporal and documentation teams | Dirty: 223 porcelain entries at inventory; accepted unit sources coexist with extensive tracked modifications and untracked plans/code/tests/tooling. Ledger adds another untracked document | Preserve. Never blindly merge this whole source branch over newer foundation. Frozen reviewed snapshots govern selective transfer; all remaining unique source, docs, tooling and evidence need individual accounting. |

No remote temporal refs were observed; `origin/main` was the only remote branch in the local inventory. This is a cached local observation as of 2026-09-27; no fetch was performed, and no publication is claimed.

Related Standards checkouts also require preservation: `feature/temporal-timezone` and `chore/generate-holidays` both point to `a5dfd8ac9ce13881d192406763666de09734fb2a`; Standards main is locally ahead of its origin/main. No Standards Phase 3 implementation or retirement readiness is inferred from those branch names. Reconfirm absolute paths, full main/origin HEADs, complete status and ownership before any future action.

## Accepted source and committed integration are separate

| Unit / scope | Recorded state | Binding evidence / next boundary |
|---|---|---|
| U0 | Committed `869ce373df2e21d6a33cd1584acba45628daa0a4`, present in foundation | Existing U0 closure and origin audit; coverage runner fix separately committed `8ed26177551debb77a5264305bc3f8546a9690fd`. |
| U4 D-9 | Original `3902168e4f5e3eae04a739a9d9d01733d928c3a5` preserved as second parent of foundation merge `40285badd8565dd7df6bd4b15f361017d850736c` | Foundation `artifacts/reviews/foundation-u4-merge-20260927/post-commit-verification.json`: exact seven files and zero source-hash mismatches. |
| U1 | Committed `71d9b290e5b4b209fc830ce2776fd96634a91bae` | Fresh real Fable review, five manifest hashes and foundation `artifacts/reviews/foundation-u1-20260927/commit-receipt.json`; original closure is provenance. |
| U2/U2b | Atomic cohort committed at `552afdda1d691ed0485f33e9da4dce061477eb13` | R2 accepted source and fresh unit gates; foundation `artifacts/reviews/foundation-u2-20260927/` receipts. Linux Release Core CI remains a phase gate. |
| U2c | Committed `5215b655f74eef606ba8c70e3f590460e2f49420` | Separate three-file boundary; preserve historical hashes, fresh transfer receipt and final-head verification requirement. |
| U3 | Committed `129e92f890c024d3581be2587908e18c7c5b1e3c` | Frozen R2 snapshot governs transfer; historical 32-file ownership supersedes 31-file list. Fresh source acceptance does not mean new gate execution. |
| U4a/U4b | Atomic commit `3b5e90fe28aa6f958cd2ca58ea049952380fe43c` | Existing atomic cohort boundary remains; preserve separately committed U4 D-9. |
| U5 | Not started | Wait for committed prerequisites and final-head gates under the existing Fable sequence; no U5 checkout created. |

Transfer provenance: [Sol handoff](../../../artifacts/reviews/foundation-transfer-20260927/sol-plan-update-handoff.md), `u1-public-review.jsonl`, `u2-public-review.jsonl`, `u3-public-review.jsonl`, `r2-public-review.jsonl`, `r2-acceptance.json`, and frozen `source-manifest-r2.json` (SHA-256 `ABDECE729B6DC21E928BE8DE78F6D68E819AB183CF9D3AE81E62E13C0A6CAD2B`). R1 is immutable. The fresh Fable ruling records that 59 U1/U2/U3 paths lack historical source SHA-256 attestation; never claim they historically match. The 37 U2c/U4a-U4b historical hashes were verified.

The [existing real Fable sequence ruling](../../../artifacts/reviews/temporal-foundation-sequence-fable.md), session `8f17e5b8-2df7-424d-b74b-93581d214fe9`, governs the remaining order: **U2c separate → U3 → U4a/U4b atomic → final-head gates → U5**. Fresh transfer source reviews are session `98651aaa-c0bd-4db2-85c5-855fa7427c3c`; the U2 commit receipt is foundation `artifacts/reviews/foundation-u2-20260927/commit-receipt.json`. The documentation prerequisite must reach main, corrected main must be integrated into the temporal foundation, and post-merge audit gates must pass before phase merge. The current foundation audit remains exit 1 with 34 inherited Rule11 documentation findings; this ledger does not declare the global audit green. Mandatory later phase sequence remains **2 → 4 → 5 → 6**, with Phase 3 following Phase 2 independently only where ownership permits.

Historical pre-approval Package A snapshot (superseded by the completion update below): foundation HEAD was `552afdda1d691ed0485f33e9da4dce061477eb13`; **69 unstaged files** comprise U2c (3), U3 (32), U4a/U4b (34). All 69 accepted R2 hashes match, and each cumulative candidate passed canonical Compiler All with zero warnings/errors. No new commits or full final-head gates have occurred. The [bundled exact commit proposal](C:/Users/stevo/.codex/worktrees/temporal-foundation/PineGuard/artifacts/reviews/foundation-consolidation-20260927/approval-bundle.md), SHA-256 `9D579148A9F448B9F124954CEB00D218C5DF78FB835B1175EAEABBB5C5CEE912`, received all three individually scoped approvals before committing. Historical source acceptance remains provenance. This ledger and tracker remain uncommitted.
## Future retirement proof

For every candidate, obtain a fresh complete tracked, staged, unstaged, untracked and ignored inventory, plus current HEAD/ref and active agent/process ownership. Prove either ancestry in the verified destination or a reviewed mapped replacement for every unique change; a similar branch name or a merged flag is insufficient. Accepted source that was selectively transferred needs path/hash and receipt mapping, including any changes left behind. Preserve the source plan/ledger documents through the existing commit flow.

Account for all dirty and untracked work without assuming another session's files are disposable. Retain review transcripts, manifests, receipts, diffs, coverage and audit evidence at a verified durable destination with integrity checks. Managed worktree archive preserves tracked and non-ignored untracked files, but excludes ignored evidence: archive required ignored review evidence separately before retirement. Reconfirm there is no active task or process using the checkout. Use managed archive for eligible managed worktrees when later authorized; primary/shared/pinned/in-use checkouts stay preserved. Do not add automated deletion tooling or perform blanket branch/worktree deletion.


## Commit completion update — 2026-09-28

Package A commits are complete (owner confirmation followed by normal hooks): U2c `5215b655f74eef606ba8c70e3f590460e2f49420`, U3 `129e92f890c024d3581be2587908e18c7c5b1e3c`, U4a/U4b `3b5e90fe28aa6f958cd2ca58ea049952380fe43c`. The 69 approved file hashes and exact commit scopes were verified after hooks. Foundation is clean at the last commit; shared final-head format and compiler checks passed, but All tests failed 14 cases (7 on each TFM) in legacy Guard/DA tests using UTC-derived dates against the local system clock; the campaign stopped before coverage. A deterministic test correction is under bounded Fable review. Evidence is under `artifacts/reviews/foundation-final-head-20260927/` in that worktree. Unit commit receipts are under `artifacts/reviews/foundation-consolidation-20260927/<unit>/commit-receipt.json`. This supersedes earlier pending-approval snapshots; no final-gate pass, U5 implementation, main merge or push is claimed. Status transcribed by the root agent on 2026-09-28 after the Sol agent reached its usage limit.
