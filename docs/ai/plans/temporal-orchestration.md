<!-- metadata_header
type: plan
id: temporal-orchestration
version: 1.1
status: planned
last_updated: 2026-09-27
-->

# Temporal & Time-Zone Program — Orchestration, Progress Tracker, Dispatch Prompts, Routines

> Companion to [T-00](temporal-00-program.md). This file is the **source of truth for progress**.
> Update §1 after every unit. Everything a session needs to resume is here or in the phase plans.
> Branch/worktree preservation and retirement accounting: [temporal branch ledger](temporal-branch-ledger.md). No cleanup is currently authorized.

## 0. Handoff — read this first, every session

You are the orchestrator. Your loop:

1. Read §1. Trust it over memory or conversation history.
2. Follow the Astra-adjudicated design under owner authorization (T-00 §0 and A-register).
   The mandatory revised-plan Claude Fable 5.1 council and corrective acceptance are complete.
   The earlier council remains historical. Real Fable final acceptance is recorded in
   `temporal-final-fable-acceptance-2.md`: P0 READY FOR IMPLEMENTATION. Phase 1 is active;
   implementation, verification and publication gates remain required. Do not ask for row signatures again.
3. Resume the current work package in §0.1; check the required source acceptance and committed prerequisites in §1. Read each owned unit section **in full from disk**.
4. Create or reuse the phase worktree (T-00 §8). Announce scope:
   `bash "$CLAUDE_PROJECT_DIR/.claude/hooks/coordination.sh" status "temporal-<phase>-<unit>" "<title>"`.
5. Dispatch the build agent with the §2.1 prompt, then the review agent with §2.3. Check in per
   T-00 §7.2 (15 / 30 / 45 min). **You never edit source files yourself.**
6. On review pass: commit with `/commit-<scope>` (dry-run `-WhatIf` first), update §1 with the
   hash, clear status (`coordination.sh status-clear`), compact, and go to 1.
7. On phase complete: run the phase gate (T-00 §9 incl. Qodana + Sonar), dispatch the Claude Fable 5.1 phase
   review (§2.4), merge per T-00 §8, update §1; retirement is a separate owner decision subject to the branch ledger preservation proof.

Current execution routing (owner override, 2026-09-27) supersedes earlier Sonnet/Opus-only instructions: GPT-6 Luna handles read/query/web/CLI evidence; GPT-6 Sol writes plans; the Codex team handles implementation, documentation, tests and commits. Historical attribution remains provenance. Actual Claude Fable 5.1 owns the existing decision,
reviews and councils. Codex team implement. [A-01]
Liveness: `ListAgents` at every checkpoint; an agent silent past 45 minutes is re-dispatched with
"resume from worktree state" after `git -C <worktree> status` is inspected.

## 0.1 Recovery execution — larger work packages

Scanner execution preserves the explicit owner directive: skip Qodana when a license/token is needed. This overrides earlier Qodana/token requirements in this tracker; remaining required checks still apply.

This execution grouping follows the existing real Fable [foundation sequence ruling](../../../artifacts/reviews/temporal-foundation-sequence-fable.md), especially D.7–D.8; it changes no technical gate or commit boundary. GPT-6 Astra reviewed recovery efficiency on 2026-09-27 under owner request. Finish the package instead of rediscovering and re-reviewing each accepted source unit.

| Work package | Execution boundary | Evidence and exit condition |
|---|---|---|
| A — remaining accepted Phase 1 consolidation | Transfer **U2c (3 files) → U3 (32 files) → U4a/U4b (34 files)** on current foundation. Preserve three exact commits: U2c separate, U3 separate, U4a/U4b atomic. | Reuse frozen accepted manifests and real Fable source receipts; hash before/after transfer and after normal hooks. Run Compiler All before each intermediate commit per D.7. Byte-identical accepted source needs no duplicate source review absent drift, conflict or hook mutation. Prepare one concrete manifest listing each exact commit scope and subject; owner may approve all in one response while each approval remains individually scoped. |
| B — committed prerequisite head and U5 | At the completed A head, run shared All format, diagnostics and tests plus gate/must-codes/nullability audits **once**, and required six-scope coverage on both TFMs. Then implement U5 from that verified committed head. | D.8 final-head gates remain before U5; they are not deferred until after U5. Reuse unchanged valid evidence with its source/tree hashes, TFM, scope, receipt and reviewer. Rerun when bytes/dependencies change, evidence lacks required attestation, a failure is unresolved, or the contract explicitly requires same-final-head evidence. |
| C — Phase 1 integration | Plan one final Phase 1 gate campaign on the finished U5 tree. Complete U5 unit gates, documentation prerequisite through main and corrected main into foundation, post-merge gates, required Linux Release Core CI, phase scanners and actual Fable phase review. | Matching format/compiler/tests/Core coverage/audits may support both U5 and U6 only when exact source tree, config and tools remain unchanged through the normal commit. U6 still needs all six scopes on both TFMs, Sonar, full phase audit and real Fable phase review. Preserve the explicit owner directive: skip Qodana when it requires a license/token; do not treat it as an unresolved paid gate. Pre-U5 results do not establish final U5-tree verification. Preserve existing owner merge/publication controls. |
| D — later program delivery | Group each phase's dependent unit work under one execution package; preserve unit ownership/contracts and commit boundaries. Maintain **2 → 4 → 5 → 6**; Phase 3 follows Phase 2 separately where ownership permits. | Required per-unit/phase gates stay binding. Release, reference generation and publication retain existing owner authorization boundaries. |

Do not repeat the full suite and coverage for every accepted-source transfer in package A. Run required intermediate compiler checks, then the required shared final-head verification once. Do not restart councils or technical decisions to implement this execution grouping. A changed candidate still requires the existing fresh real Fable review and acceptance procedure. Check scanner, Linux CI and documentation prerequisite readiness read-only in parallel with consolidation. One Fable session may return separately scoped U5 and phase verdicts once both required evidence sets exist; do not seek a new council.

Package A candidate is ready for exact commit approval: **69 unstaged files transferred** on unchanged foundation HEAD `552afdda1d691ed0485f33e9da4dce061477eb13`; all 69 R2 hashes match. Each cumulative U2c/U3/U4a-U4b candidate passed canonical Compiler All with zero warnings/errors. No commits or full final-head gates are claimed. The [single approval bundle](C:/Users/stevo/.codex/worktrees/temporal-foundation/PineGuard/artifacts/reviews/foundation-consolidation-20260927/approval-bundle.md) names the three separate exact commits; SHA-256 `9D579148A9F448B9F124954CEB00D218C5DF78FB835B1175EAEABBB5C5CEE912`. Historical source acceptance remains provenance; candidate transfer/compiler evidence is the current execution state.
Package A commits are complete (owner confirmation followed by normal hooks): U2c `5215b655f74eef606ba8c70e3f590460e2f49420`, U3 `129e92f890c024d3581be2587908e18c7c5b1e3c`, U4a/U4b `3b5e90fe28aa6f958cd2ca58ea049952380fe43c`. The 69 approved file hashes and exact commit scopes were verified after hooks. Foundation is clean at the last commit; shared final-head format and compiler checks passed, but All tests failed 14 cases (7 on each TFM) in legacy Guard/DA tests using UTC-derived dates against the local system clock; the campaign stopped before coverage. A deterministic test correction is under bounded Fable review. Evidence is under `artifacts/reviews/foundation-final-head-20260927/` in that worktree. Unit commit receipts are under `artifacts/reviews/foundation-consolidation-20260927/<unit>/commit-receipt.json`. This supersedes earlier pending-approval snapshots; no final-gate pass, U5 implementation, main merge or push is claimed. Status transcribed by the root agent on 2026-09-28 after the Sol agent reached its usage limit.

2026-09-28 regression prerequisite: the shared run at `3b5e90f` passed format/compiler but failed 14 cases across both TFMs because legacy Guard/DA relative-date tests derived today from UTC while the system provider uses its local date. Real Fable session `4bcf3dfb-7a47-4c76-aafa-9afd037b067b` confirmed a six-file test-only fixed-clock correction; no production semantics or accepted 69-file snapshots change. The correction is a separate local commit prerequisite for U5, with fresh Fable source review, candidate hashes, required gates and exact owner commit approval. Evidence is foundation `artifacts/reviews/foundation-clock-regression-20260928/`. Other live-clock instant-comparison tests are recorded as later test-hygiene debt, not part of this correction.

## 1. Progress tracker (source of truth)

Implementation statuses: `Pending`, `Blocked (D-n)`, `In progress`, `Source accepted`, `Transferred; awaiting commit approval`, `Committed`, `Ready to merge`, `Merged`, `Needs fix`. Source accepted means reviewed bytes only; Transferred means the accepted candidate is present on foundation but not committed; Committed means a verified local commit; Merged requires verified destination ancestry. Historical P0 Done means its review gate completed.

| Unit | Title | Status | Worktree / branch | Commit | Notes |
|---|---|---|---|---|---|
| P0-historical | Earlier `/plan-with-council` on T-00 | Historical evidence | — | — | Earlier transcript retained; does not activate this revision |
| P0 | Revised real Claude Fable 5.1 council and corrective acceptance | Done | existing `feature/temporal-timezone` | — | Real Fable P0 READY FOR IMPLEMENTATION; see `temporal-final-fable-acceptance-2.md` and revised transcript. Implementation and publication gates remain required |
| P0-review | Astra current-revision adjudication under owner authorization | Design finalized | existing `feature/temporal-timezone` | — | A-01 through A-12 remain final; actual Fable corrective rulings preserve them. No repeated owner signing. |
| T1-U0 | FixedTimeProvider ctor + zone-aware defaults (D-4) | Committed | standalone feature/temporal-u0-standalone | 869ce37 | Fable acceptance: temporal-u0-fable-closure.md and temporal-u0-origin-audit-fable.md. Authorized commits now complete: exact 10-file provider/test scope 869ce37; separate exact 8-file coverage runner scope 8ed2617; separate exact 4-file DOC-DEBT-02 scope 63674f4. These supersede the pending combined 18-file proposal. Historical DOC-DEBT-01 + DOC-DEBT-02 candidate evidence was a conflict-free no-commit merge with canonical audit exit 0 (Rules 11/12/50), actual Claude Fable 5.1 final ACCEPT recorded in integration fable-final-integration-acceptance.json (SHA-256 F01A60810176A0BD90C1307A7C4516F73DBCE806F672F065BD67454D1C75B5A8), with both pinned full audit formats exit 0/zero findings in integration-final-audit-receipt.json (SHA-256 3EC31E30FF0A733465ED627FAAC05EDFEF1EA60A31FEA964444FCFB866FFB454). Local DOC-DEBT integration commit is complete at `d1b1793b8f5ba2c20d27c8e7058b514f79989a2a` on `codex/doc-debt-main-integration`; it is not on main or foundation. No main merge is claimed. Phase merge remains BLOCKED until the documentation prerequisite reaches main, corrected main is integrated into the temporal worktree, and the post-merge audit gate passes |
| T1-U1 | Test clock zone + custom test zones | Committed | `codex/temporal-foundation` (see branch ledger for actual path) | `71d9b290e5b4b209fc830ce2776fd96634a91bae` | Fresh real Fable source review and five frozen manifest hashes establish current acceptance; fresh gates and normal-hook commit receipt: foundation `artifacts/reviews/foundation-u1-20260927/commit-receipt.json`. Historical closure remains provenance. Local commit only; no main merge/push. |
| T1-U2 | Today derivation (D-1) + same-day calendar fix (D-2) | Committed | `codex/temporal-foundation` (see branch ledger for actual path) | `552afdda1d691ed0485f33e9da4dce061477eb13` | Atomic U2/U2b R2 cohort accepted and locally committed; fresh Fable review/snapshot manifest and fresh gates supersede the old pending-commit statement. Receipt: foundation `artifacts/reviews/foundation-u2-20260927/commit-receipt.json`. Linux Release Core CI remains a Phase 1 pre-merge gate. No main merge/push. |
| T1-U2b | Must consumers of guarded calendar references | Committed | `codex/temporal-foundation` (see branch ledger for actual path) | `552afdda1d691ed0485f33e9da4dce061477eb13` | Atomic U2/U2b R2 cohort accepted and locally committed; fresh Fable review/snapshot manifest and fresh gates supersede the old pending-commit statement. Receipt: foundation `artifacts/reviews/foundation-u2-20260927/commit-receipt.json`. Linux Release Core CI remains a Phase 1 pre-merge gate. No main merge/push. |
| T1-U2c | String DateOnly consumers of guarded calendar references | Committed | `codex/temporal-foundation` | `5215b655f74eef606ba8c70e3f590460e2f49420` | depends on T1-U2/U2b accepted; before T1-U6; scope MustClauses; exact 3-file source scope (`src/PineGuard.MustClauses/MustStringDateOnlyClauses.cs`, `tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTests.cs`, `tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTestData.cs`); Guard/Fluent/DA string DateOnly inherit via Must, no edits; T-04 string DateOnly work depends on this; Claude Fable 5.1 final ruling ACCEPT — unit closed; 44 focused cases passed on both TFMs; shared format/diagnostics/tests exit 0, diagnostics zero warnings/errors, 38,259/38,259 tests passed (zero failed/skipped); `MustClauses` coverage 100%/100% on net8.0 and net10.0 (3,870 lines, 1,382 branches each); Rule11/12/50, MustCodes Rule13, and nullability Rule07 audits exit 0 with zero findings; closure evidence [`artifacts/reviews/t1-u4b-cohort-gates/u2c-final-closure.json`](../../../artifacts/reviews/t1-u4b-cohort-gates/u2c-final-closure.json) (SHA256 `836F34CD57FE22BA8FBEDD82CF65C14FF97248B4C6DCEF64158EACE45C2E035D`) and cohort evidence [`artifacts/reviews/t1-u4b-cohort-gates/cohort-evidence.json`](../../../artifacts/reviews/t1-u4b-cohort-gates/cohort-evidence.json); committed on foundation with normal hooks and reviewed hashes verified; see also [`artifacts/reviews/t1-u2c-fable-source-review-public.md`](../../../artifacts/reviews/t1-u2c-fable-source-review-public.md) |
| T1-U3 | TimeOnly midnight-crossing Between/NotBetween | Committed | `codex/temporal-foundation` | `129e92f890c024d3581be2587908e18c7c5b1e3c` | depends on T1-U1; start/end naming; remove TimeOnly reversed-bound misuse guards only; A-09 unchanged; Fable ACCEPT and full unit gates complete — 32-file ownership incl. `StringRulesFixtures.TimeOnly.cs`, historical 31-file manifest superseded by [`artifacts/reviews/t1-u3-final-ownership-audit.json`](../../../artifacts/reviews/t1-u3-final-ownership-audit.json); see [`temporal-u3-final-acceptance-fable.md`](temporal-u3-final-acceptance-fable.md) (final acceptance receipt) and [`temporal-u3-source-review-fable.md`](temporal-u3-source-review-fable.md) (source review); stale-doc fixes it found are T7-U2 work, not this unit; committed on foundation with normal hooks and reviewed hashes verified; Phase 1 merge remains blocked independently of this unit — by T1-U5 and T1-U6 and by the program's existing merge prerequisites: T1-U0's DOC-DEBT-01 `origin/main` fix and the owner-run green Linux CI `workflow_dispatch` of `core-null-provider-timezone` noted on T1-U0/T1-U2 |
| T1-U4 | Guard boundary polarity fix (D-9) | Committed | `codex/temporal-foundation` | `3902168e4f5e3eae04a739a9d9d01733d928c3a5` | Original commit preserved as second parent of local foundation merge `40285badd8565dd7df6bd4b15f361017d850736c`; exact seven files and zero hash mismatches in foundation `artifacts/reviews/foundation-u4-merge-20260927/post-commit-verification.json`; no main merge/push. Historical targeted red 8/18 then green 18/18 on net10.0; full net8/net10 GuardClauses gates, audits, and Fable acceptance recorded; standalone before U4a; exact seven-file contract: `temporal-u4-sweep-ruling-fable.md`; receipt: `temporal-u4-commit-receipt.md` |
| T1-U4a | TimeSpan rename: Core + codes + Must | Committed | `codex/temporal-foundation` | `3b5e90fe28aa6f958cd2ca58ea049952380fe43c` | depends on T1-U1, T1-U4; Claude Fable 5.1 ACCEPT as an atomic U4a/U4b cohort — see `artifacts/reviews/t1-u4b-cohort-gates/fable-final-cohort-acceptance.md` and `fable-final-cohort-receipt.json`, final valid evidence `artifacts/reviews/t1-u4b-cohort-gates/cohort-evidence.json`; unit work and gates done — shared All tests 38,259/38,259 passed (0 failed/skipped), Core/MustClauses/GuardClauses/FluentValidation/DataAnnotations 100%/100% coverage both TFMs, `audit --gate`/`must-codes`/`nullability` all exit 0; earlier prompt-only wrapper gate logs and the mistyped `audits/gate.log` exit 2 are invalid/superseded and not counted — the final cohort receipt (including corrected `Testing`-scope recorder metadata) is authoritative; atomic U4a/U4b committed on foundation with normal hooks and reviewed hashes verified; T1-U4's own D-9 commit `3902168e4f5e3eae04a739a9d9d01733d928c3a5` is preserved separately |
| T1-U4b | TimeSpan rename: Guard (renames only, polarity already fixed by T1-U4) + Fluent + DA | Committed | `codex/temporal-foundation` | `3b5e90fe28aa6f958cd2ca58ea049952380fe43c` | depends on T1-U4a; same atomic Claude Fable 5.1 ACCEPT cohort as T1-U4a — see `artifacts/reviews/t1-u4b-cohort-gates/fable-final-cohort-acceptance.md` and `fable-final-cohort-receipt.json`, final valid evidence `artifacts/reviews/t1-u4b-cohort-gates/cohort-evidence.json`; unit work and gates done (same shared/coverage/audit evidence as T1-U4a); earlier prompt-only wrapper gate logs and the mistyped `audits/gate.log` exit 2 are invalid/superseded and not counted — the final cohort receipt (including corrected `Testing`-scope recorder metadata) is authoritative; atomic U4a/U4b committed on foundation with normal hooks and reviewed hashes verified; T1-U4's own D-9 commit `3902168e4f5e3eae04a739a9d9d01733d928c3a5` is preserved separately |
| T1-U5 | Precision enum symmetry | Pending | `codex/temporal-foundation` (after committed prerequisite head) | — | Not started. Existing real Fable foundation sequence supersedes the historical dirty-source overlay route: U2c → U3 → atomic U4a/U4b are now committed on foundation; shared final-head gates are running before U5 starts. U0/U4/U1/U2-U2b remain committed. No separate U5 checkout has been created. |
| T1-U6 | Phase 1 review + merge | Pending | | | depends on all Phase 1 units |
| T2-U0 | Audit model corrections | Pending | `temporal-02-timezone` | | first; ∥ U1/U2; vocabulary v2 layerAbsences + string Rules mapping; lands before U3; CLI tests and no-baseline audit evidence |
| T2-U1 | `MustCodes.TimeZone` | Pending | `temporal-02-timezone` | | |
| T2-U2 | `TimeZoneUtility` + `TimeZoneRules` + UtcOffset strings | Pending | | | |
| T2-U3 | Must time-zone clauses | Pending | | | |
| T2-U4 | Guard | Pending | | | |
| T2-U5 | Fluent | Pending | | | |
| T2-U6 | DataAnnotations | Pending | | | |
| T2-U7 | Phase 2 review + merge | Pending | | | |
| T3-U1 | Cut `v0.1.0-alpha.8` (owner) | Pending | PineGuard `main` | | Tier 1; never a routine |
| T3-U2 | Standards bump + discovery | Pending | Standards temporal-timezone | | S-01 diagnostic inventory, actual published package graph |
| T3-U3 | Standards adaptation per domain | Pending | | | S-01/A-12; no owner signature |
| T3-U4 | Verify existing Standards host composition | Pending | | | Preserve existing SystemTimeZone names; no Core delegation [A-12] |
| T3-U5 | Removed rename task | Removed | | | A-12: current host-backed SystemTimeZone stays named and unchanged |
| T3-U6a | `backward` generator + data + abstractions (D-8) | Pending | | | `backward`/`etcetera` references already downloaded, verified and hashed in the reference manifest; future generation still requires separate explicit authorization |
| T3-U6b | `backward` rules + clauses + tests | Pending | | | |
| T3-U7 | Standards verification + review + merge | Pending | | | |
| T4-U1…U6 | Validity + relative (see T-04) | Pending | `temporal-04-validity` | | rows expand when phase opens; `Unexpired` is `NotExpired` everywhere (Must/Guard/Fluent/DA), never the positive clause name (D-15); the WithinLast/WithinNext unit also deletes `IsWithinDaysFromNow`/`NotWithinDaysFromNow` at every layer in the same unit (D-6) |
| T5-U1…U6 | Durations + calendar (see T-05) | Pending | `temporal-05-calendar` | | |
| T6-U1…U6 | Formats + epoch (see T-06) | Pending | `temporal-06-formats` | | |
| T7-U1…U6 | Close-out (see T-07) | Pending | `temporal-07-closeout` ×2 | | T7-U2 includes the Claude Fable 5.1-assigned T1-U3 documentation follow-up (DataAnnotations Pattern D, `core-common-api-decisions.md` A-3/A-8); see T-07 §3 U2 |

When a phase opens, replace its collapsed row with one row per unit (copy the unit ids from the
phase plan's §2 table).

Mandatory phase sequence is **2 → 4 → 5 → 6**, including across worktrees, because
the code files have single-writer ownership. Phase 3 follows Phase 2 independently where
its own file ownership permits. T3-U6a/U6b remain later implementation design requiring
separate explicit generation authorization; current review/reference refresh only downloads
and verifies reference inputs, never generates classes or chains a generator command.

## 2. Dispatch prompts (verbatim; fill the `<>` fields)

### 2.1 Build unit (Codex team)

```
You are implementing unit <UNIT-ID> of the PineGuard temporal program. Model: <actual Codex team model>. You make
no design decisions: every name, signature, message, code and file is specified. If something is
not specified, stop and report — do not choose.

Worktree (all work here, nothing outside): <WORKTREE-ABS-PATH>
Read from disk, in full, before touching anything:
  1. <WORKTREE>/docs/ai/README.md; applicable specs/rules/role/playbook and nearest AGENTS.md;
     cite exact Brain sections and source precedent in the report. Then temporal-00-program.md
     §0, §4, §5, §6 and temporal-astra-decisions.md.
  2. <WORKTREE>/docs/ai/plans/<PHASE-FILE>.md — section "### <UNIT-ID> — ..."
  3. Every file the unit says to clone from, in full.
Decisions this unit cites are Astra-adjudicated: <D/A-LIST>; actual Claude Fable 5.1 activation evidence: <PATH>.

Do exactly the unit's file list. For each new member: clone the named template's header, XML docs,
shape, and test triple (Tests + TestData + Fixtures). All tests [Theory]+TheoryData. Fixture entries
single-line, nameof() names, expected values as literals.

Verification (run inside the worktree, in order; all must exit 0):
<T-00 §9 commands for this unit's scopes, both TFMs>

Budget: <BUDGET> tokens (from the unit); stop at <STOP-AT> (80 % of budget) and report partial state.
Timeout: <TIMEOUT> minutes wall-clock (from the unit); at the deadline stop and report with
`git -C <WORKTREE> status --short`. Never run AskUserQuestion, never
`git add`/`git commit`, never touch files outside the list.

Final report (≤ 25 lines): files created/modified; each verification command with pass/fail and the
coverage numbers per TFM; anything unspecified you hit (and stopped on).
```

### 2.2 Inventory / discovery unit (GPT-6 Luna)

```
Evidence inventory for unit <UNIT-ID>. Model: GPT-6 Luna. Do not edit source files. Worktree: <PATH>.
Task: <the unit's numbered task list, verbatim from the phase plan>.
Write the report to <WORKTREE>/artifacts/temporal/<UNIT-ID>-report.md (create the folder).
Budget 60k tokens, timeout 15 minutes. Final reply: the report path plus an 8-line summary.
```

### 2.3 Unit review (Claude Fable 5.1)

```
You are reviewing unit <UNIT-ID> against the plan. Model: Claude Fable 5.1. You change nothing.
Worktree: <PATH>. Read from disk: temporal-00-program.md §4–§6 and §9, the unit section in
<PHASE-FILE>.md, then `git -C <PATH> diff --stat` and the full diff of every listed file.
Check, and answer each with PASS/FAIL + one line of evidence:
  1. Files touched == unit file list (no extras, none missing).
  2. Every public member's name, parameter order, nullability and defaults match §4 exactly.
  3. Every message template and code constant match §4/§5 exactly.
  4. Guard polarity: each guard calls the Must clause the plan names.
  5. Tests: Theory+TheoryData only; fixtures single-line; a message and a code asserted per Must group;
     clock-reading tests use FixedTimeProvider (Default / DefaultAheadOfUtc as the plan says);
     no real zone id except "UTC" and TestTimeZones.PlatformSampleId.
  6. XML docs present on every public member; §6.4 remark present where required.
  7. TFM guards match §6.2.
  8. Re-run: format, diagnostics, tests, coverage per TFM for the unit's scopes; paste the summary lines.
Verdict: "no deviations from T-00" or the numbered list of deviations with file:line.
Budget 100k, timeout 30 minutes. Write the report to <WORKTREE>/artifacts/reviews/<UNIT-ID>.md
and reply with the verdict line.
```

### 2.4 Phase review (Claude Fable 5.1)

```
Claude Fable 5.1 phase review for Phase <N> of the PineGuard temporal program. You change nothing.
Read: temporal-00-program.md in full; <PHASE-FILE>.md in full; every artifacts/reviews/T<N>-*.md;
`git -C <WORKTREE> log --oneline origin/main..HEAD`; the complete diff against origin/main.
Judge: (a) does the phase deliver exactly its §4 canon — nothing more, nothing renamed; (b) are the
semantics in the plan actually what the code does (re-derive three fixture rows by hand and say
which); (c) any message that would read wrong to a consumer; (d) any place a future name is boxed
in; (e) merge risk (conflicts with a concurrent phase's files). Verdict: MERGE / FIX (numbered list).
Budget 80k, timeout 30 minutes. Write artifacts/reviews/phase-<N>.md and reply with the verdict.
```

### 2.5 Naming and decision verification (Claude Fable 5.1, Phase 0)

```
Verify the Astra-adjudicated naming for the PineGuard temporal program. Read temporal-00-program.md
§3 and §4. For each proposed public name, confirm the precedent cited exists in src/ (grep), state in
one line whether the name is on-the-tin, and list any collision with an existing member name across
Must/Guard/Fluent/DA (grep for the bare name). Output a table: name | precedent | collision | keep/
change with reproducible evidence and the applicable Brain clauses. Use T-00 §0's exceptional
reopening process; no speculative alternative reopens final architecture.
```

## 3. Routines (cloud)

### 3.1 Environment (one per repo, single-repo, Ubuntu 24.04)

Setup script (runs once, cached ~7 days; must exit 0):

```bash
#!/bin/bash
set -euo pipefail
# .NET SDKs the solution targets (net8.0 + net10.0), plus PowerShell for the repo tooling
sudo apt-get update
if ! sudo apt-get install -y dotnet-sdk-8.0 dotnet-sdk-10.0; then
  echo "SDK apt install failed; attempting the documented Microsoft channel installer." >&2
fi
# fallback when the apt feed lacks a channel: Microsoft's install script
if ! dotnet --list-sdks 2>/dev/null | grep -q '^10\.'; then curl -fsSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0 --install-dir "$HOME/.dotnet"; fi
if ! dotnet --list-sdks 2>/dev/null | grep -q '^8\.'; then curl -fsSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 8.0 --install-dir "$HOME/.dotnet"; fi
export PATH="$PATH:$HOME/.dotnet"
if ! command -v pwsh >/dev/null 2>&1; then dotnet tool install --global PowerShell; fi
export PATH="$PATH:$HOME/.dotnet/tools"
dotnet --list-sdks | grep -q '^8\.'
dotnet --list-sdks | grep -q '^10\.'
pwsh -NoProfile -Command '$PSVersionTable.PSVersion'
corepack enable
# Use the repository's pinned packageManager / lockfile toolchain; never pnpm@latest.
corepack pnpm -C apps/cli install --frozen-lockfile
dotnet restore PineGuard.slnx
```

This setup must fail on missing SDKs, restore errors, unavailable pinned package manager or smoke
failure. Validate it in the actual routine runtime before scheduling; a successful installer does
not prove repository checks passed. Record unresolved runtime/toolchain requirements rather than
masking them with `|| true` or unconditional exit 0. [A-11]

Environment variables: `DOTNET_CLI_TELEMETRY_OPTOUT=1`, `DOTNET_NOLOGO=1`, `TZ=UTC`. Network: default
trusted allowlist (nuget.org, github.com, apt are included). The GitHub proxy
authenticates git, but default Qodana requires `QODANA_TOKEN`; provision it securely or use
the documented `-Linter auto` mode. Docker is available for Qodana; SonarQube is **not** run
in the cloud. InspectCode is not identical to Qodana and does not silently replace its phase
gate; any substitution requires an owner decision recorded in T-00 §9.

**Smoke run first.** Before enabling the schedule, "Run now" with the fire text `SMOKE` and confirm
the run reports: `dotnet --list-sdks` shows 8.x and 10.x; `pwsh` runs; `Run-Tests.ps1 -Scope Testing`
passes; a branch `claude/temporal-smoke` was pushed and then deleted by the owner. If PowerShell
does not install, switch the setup script to Microsoft's apt repository per
`https://learn.microsoft.com/powershell/scripting/install/install-ubuntu` and re-smoke.

### 3.2 Routine A — `pineguard-temporal-build` (06:00 Australia/Sydney, cron `0 6 * * *`)

Repository: `stevomccormack/PineGuard`. Model: <actual Codex team model> (the routine's own session; it dispatches
sub-agents per T-00 §7.1). Prompt:

```
You are the orchestrator for one unit of the PineGuard temporal program, running unattended. You
never edit docs/ai/plans/temporal-orchestration.md (D-10): you only read the tracker, you never
write it. Nobody can answer questions: if anything below cannot be satisfied, write the reason into
artifacts/reviews/<unit-id>.build.md on the branch, push, and stop.

1. Read docs/ai/plans/temporal-orchestration.md §0–§2 and docs/ai/plans/temporal-00-program.md §3, §7–§9.
2. On `origin/main`, in §1, pick the FIRST unit that is Pending, whose Depends-on rows are Done,
   whose Astra decisions and required Claude Fable 5.1 activation gates are satisfied, and that is not owner-only (T3-U1 is owner-only: skip it). The
   tracker on `main` is the only copy you ever read for planning — the tracker is never edited on a
   branch. If none, stop.
3. Branch: if `claude/temporal-<unit-id>` exists on origin, check it out and read
   artifacts/reviews/<unit-id>-evening.md on that branch, if present, for the prior evening review's
   verdict. If its first line is `Status: Ready to merge` → stop (the owner has not merged yet). If
   `Status: Needs fix` → this run is a fix pass against the listed deviations. Otherwise create the
   branch from `origin/main`. Rebase `claude/temporal-<unit-id>` on `origin/main` before every push;
   never rebase a branch that has been pushed by a previous run — merge `origin/main` into it
   instead. You cannot push anywhere else; never merge into main.
4. Dispatch the build agent with the §2.1 prompt (worktree = this checkout), then the §2.3 review
   agent. Enforce their budgets and timeouts with ListAgents elapsed time; re-dispatch once on
   silence past 45 minutes; if still silent, write the reason to
   artifacts/reviews/<unit-id>.build.md, push, and stop.
5. `git add` the unit's files **by name** (always by name, never `git add .`) and commit with the
   conventional message from docs/ai/workflows/commit.md (subject + prose body; trailer
   Co-Authored-By: Claude Fable 5.1 <noreply@anthropic.com>), whether the review found deviations or
   not. Write artifacts/reviews/<unit-id>.build.md with what was built, the verification commands
   run with pass/fail, and the §2.3 review verdict (this is a build report only — it states no
   merge-readiness verdict; that is Routine B's job). Rebase on `origin/main`, push, stop.
6. Do exactly one unit per run. Never run Sonar. Never delete branches. Never edit T-00 §3/§4. Never
   edit docs/ai/plans/temporal-orchestration.md — the owner mirrors readiness into the tracker on
   `main` at merge time (§3.5).
```

### 3.3 Routine B — `pineguard-temporal-review` (18:00 Australia/Sydney, cron `0 18 * * *`)

Same repository and environment. Prompt:

```
You are the evening verifier for the PineGuard temporal program, running unattended. You never
edit docs/ai/plans/temporal-orchestration.md (D-10): readiness is recorded only in the artifact
file below, and the owner mirrors it into the tracker on `main` at merge time.
1. List remote branches matching claude/temporal-* pushed or updated since the last review run
   (record the timestamp you use in step 5's report).
2. For each branch, in this fresh clone: check it out; run the applicable per-unit T-00 §9 gate for the scopes the
   unit's row names (both TFMs; codes-only units follow §9; T1-U2/U2b is one atomic
   acceptance cohort with no audit exemption). After the final adapter of a phase,
   run the full audit cohort before Fable phase review; baseline changes are deletions
   only. Cross-layer audits remain strict and are not claimed green mid-phase. Run Qodana via
   ./tools/code-scan/qodana/Run-Qodana.ps1 -Scope All -Clean with securely provisioned
   `QODANA_TOKEN`, or its documented `-Linter auto` mode (Docker is available).
   InspectCode is a distinct scanner; no silent phase-gate substitution. Do not run Sonar.
3. Dispatch the §2.3 Claude Fable 5.1 review for the branch's unit and read artifacts/reviews/<unit>.md (the
   build routine's report — do not overwrite it).
4. Write artifacts/reviews/<unit>-evening.md on the branch with the gate results and the verdict.
   Its first line is exactly `Status: Ready to merge` (all green + "no deviations") or
   `Status: Needs fix` followed by the numbered list; commit it on the branch; push. This file is
   the only place merge readiness is recorded.
5. On the `claude/temporal-housekeeping` branch (create it from `origin/main` if absent, otherwise
   rebase it on `origin/main` first — you cannot push to `main` itself), append to
   artifacts/reviews/stale-branches.md a one-line dated list of claude/temporal-* branches older
   than 7 days that are not Done, so the owner can prune; commit and push that one file on that
   branch. Never merge, never delete, never open a PR, never edit
   docs/ai/plans/temporal-orchestration.md.
```

### 3.4 Standards routines

Identical pair named `standards-temporal-build` / `standards-temporal-review` on
`stevomccormack/PineGuard.Standards`, enabled only while Phase 3 or Phase 7 has Pending Standards
units, reading `docs/ai/plans/temporal-timezone-sync.md` in that repo (its tracker mirrors the T3/T7
rows). Its setup script restores `PineGuard.Standards.slnx`. It never runs T3-U1 and never downloads
already verified `backward`/`etcetera` references again. Use the verified manifest inputs;
a fresh reference update remains a separate explicitly authorized download-only refresh.

### 3.5 Owner's local loop (any time)

1. `git fetch`; for every `claude/temporal-*` branch, read `artifacts/reviews/<unit>-evening.md` on
   that branch. Its first line is the routine's readiness verdict — routines never write this into
   the tracker themselves (D-10). If `Status: Ready to merge`: mirror that into the tracker (§1) on
   `main` — set the row to Ready to merge with the branch/commit — then `git merge --no-ff
   claude/temporal-<unit>` into `main` in a local session (or into the phase's feature branch when
   the phase has several units; then the phase merge), run Sonar for the phase, push, and set the
   tracker row to Done with the merge commit hash. If `Status: Needs fix`: mirror the numbered
   deviations into the tracker row as Needs fix instead of merging. The tracker is edited only here,
   by the owner, at merge time.
2. Record actual required Claude Fable 5.1 revised-plan council evidence; Astra already adjudicated this revision.
3. Do T3-U1 (the release) interactively.
4. Prune `claude/temporal-*` branches listed in `artifacts/reviews/stale-branches.md` on
   `claude/temporal-housekeeping`.

### 3.6 What the cloud will not do (and who does it)

| Action | Cloud | Owner |
|---|---|---|
| Merge to `main` | never | yes |
| Cut a release / push tags | never | yes (T3-U1) |
| Sonar scan | never | yes (local server) |
| Decide/review/council | Claude Fable 5.1 only | receives documented decisions and exceptional departures |
| Refresh `backward`/`etcetera` references | never automatically | current download-only refresh is recorded in the reference manifest; later refresh requires its explicit scope |
| Delete branches | never | yes |
| Edit the progress tracker (this file, §1) | never (D-10) | yes — mirrors readiness from `artifacts/reviews/<unit>-evening.md` at merge time (§3.5) |

## 4. Lessons (append-only; program retrospective goes here)

- (empty at planning time)

- 2026-09-28 — Fable session 4bcf3dfb-7a47-4c76-aafa-9afd037b067b allowed the root to adopt Builder/Verifier for the exact six-file clock regression only after implementation subagents exhausted their usage limit; independent Fable review, all gates and exact owner commit approval remain mandatory. This is a temporary execution-routing exception, not a technical Brain waiver; report it in the eventual PR and restore delegation when available. U5 remains behind its gated committed prerequisite and is not authorized by this exception.
