<!-- metadata_header
type: plan
id: new-surfaces-orchestration
version: 1.0
status: active
last_updated: 2026-08-29
parent: new-surfaces-program
-->

# New Surfaces Program — Orchestration, Model-Routing and Progress Tracker

> **Status**: Active | **Owner**: repo owner | **Applies to**: every remaining unit of the
> New Surfaces Program (Plan 00 §10.1). Read this file **fresh from disk** at the start of
> every session that touches this program, before opening any unit — do not rely on
> conversation memory across a compaction boundary.
>
> The phase plans (`new-surfaces-missing-validation-cases-0{1..6}-*.md`) say **what** to build;
> this file says **how the sessions that build it are run**, and **where things currently stand**.
> Where this file and a phase plan disagree on content, the phase plan wins; where they disagree
> on process, this file wins; `docs/ai/specs/safety.md` wins over both, always.

## 1. Why this file exists

The program is roughly 18–28 focused work-session's worth of content across ~12 PR-sized units
(sizing and reasoning in §3). No single session holds that much context, and a fresh session
(after compaction, or a new day) has no memory of what a prior session did. This file is the
**persistent state** a session reads first to know exactly where the program stands, without
re-deriving it from git log and conversation scrollback every time.

**Rule: update §2 (progress tracker) as the last step of every unit's checkpoint (§6), in the
same PR/commit that closes the unit out.** A tracker that lags reality is worse than no tracker.

## 2. Progress tracker (source of truth — update after every unit)

| Unit | Status | Branch / worktree | Merge commit | Notes |
|---|---|---|---|---|
| Phase 1 (1a–1d) | **Done** | `feature/structural-validation` (removed) | `357ab00` | All four sub-units shipped together, not as separate PRs — see Plan 01's archive banner |
| Track 0 | **Done** | `feature/tooling-scope-registry` (removed) | `7ffaf40` | `Get-PineGuardScope` registry; unblocks 2, 3-PR1, 4-bridges, 6 |
| CI coverage gate → 100% | **Done** | — | `18d7890` | Verified via live CI re-run (job `99088572473`), not assumed |
| **2 — Options** | Not started | `feature/options` (worktree exists, based pre-Track-0) | — | **First action**: merge `origin/main` into the worktree before any other work — it predates the Track 0 merge |
| **4-bridges** | Not started | none yet | — | Blocked on owner decision D1 (scope) before opening |
| **3-PR1 — async + DI** | Not started | none yet | — | Blocked on owner decision D4 (FV adapter scanner in/out of scope) before opening |
| **3-PR2 — AspNetCore** | Not started | none yet | — | Hard-depends on 3-PR1 merged; 1d dependency already satisfied; decision D3 gates its W7 only |
| **4-mediatr** | Not started | none yet | — | Hard-depends on 3-PR1 merged (soft dependency per Plan 00 §10.1 note — do not reorder without a plan edit) |
| **5-A** (string content) | Not started | none yet | — | No blockers |
| **5-B** (identifiers) | Not started | none yet | — | No blockers; biggest batch (cron parser, JWT shape, SemVer) — budget 2–3 sessions |
| **5-C** (Unicode) | Not started | none yet | — | Has an at-risk row (globalization-invariant mode) — verify on CI early |
| **5-D** (numeric/financial) | Not started | none yet | — | No blockers |
| **5-E** (temporal + clock injection) | Not started | none yet | — | Riskiest batch: scripted `TimeProvider?` signature insertion across every temporal member and call site; budget 2–3 sessions |
| **5-F** (file signatures) | Not started | none yet | — | No blockers |
| **6 — Analyzers** | Not started | none yet | — | Deliberately run last (Plan 00 §3.2); no blockers |

**Recommended order** (adapts Plan 00 §10.4 for the current state): `2 → 4-bridges → 3-PR1 → 3-PR2 → 4-mediatr`, with `5-A…F` and `6` filling any gap — they block nothing and wait for nothing. Keep at most 2–3 branches open at once (Plan 00 §10.3); all `dotnet` build/test/coverage steps are serialized by the coordination hook regardless of how many sub-agents are authoring in parallel.

## 3. Sizing (why "finish everything in one session" isn't the plan)

The phase plans are unusually complete — signatures, file lists, even commit messages are
pre-decided — so the work is largely mechanical *given* the plans, but the volume is real:

| Unit | Real content | Est. sessions |
|---|---|---|
| 2 — Options | ~150 LOC production, 2 test classes; pays the full §8 scope-onboarding tax (its explicit purpose per Plan 02 §1.3) | 1–2 |
| 3-PR1 | Async seam (4 `RuleForAsync*` overloads, `HasAsyncRules`, `MustValidationMode`), Rule14 audit script, DI package | 2–3 |
| 3-PR2 | ~12 source files, largest test surface (TestServer end-to-end), a known net10-only API-verification risk | 3–4 |
| 4-bridges | 3 micro-packages, ~5 files total — onboarding tax (3 scopes) outweighs the code | 1–2 |
| 4-mediatr | 4 files | 1 |
| 5-A…F | ~45 Core rules with 5-layer parity; A/D/F small, B/C/E larger (see tracker notes) | 8–12 total |
| 6 — Analyzers | 2 projects, 6 diagnostics + fixes, Roslyn-testing infra | 2–3 |

**Chunk exactly along Plan 00 §10.1's units** (one worktree = one PR = one checkpoint). Do not
invent a finer decomposition — each unit's own W-step playbook is already the intra-unit
structure. Phase 5 chunks per batch, as Plan 05 already mandates.

## 4. Roles and model routing

- **The orchestrating session coordinates only.** It never edits source, tests, tooling or docs
  itself — every edit happens inside a dispatched sub-agent. It opens worktrees, dispatches
  agents, runs the §6 checkpoint, and holds the conversation with the owner.
- **Every sub-agent prompt names the exact plan file and section numbers governing its task**;
  the agent reads them from disk itself. A Haiku-produced distilled brief may supplement this,
  but never replaces it — this is the anti-drift discipline applied at the sub-agent level.

| Tier | Model | Used for | Examples |
|---|---|---|---|
| Judgment (high) | **Fable** | Council-grade calls a plan leaves open: naming, design trade-offs, drop/keep decision rules, error-code curation review | Plan 03 §4.2 resolver drop/keep; Plan 05 per-batch code curation; any gap between a plan and the shipped surface |
| Judgment (light) | **Opus** | Verification/review passes needing reasoning but not design authority | Plan 00 §8 checklist walk-throughs, cascade completeness review, PR-body review |
| Implementation | **Sonnet 5** | All coding, test writing, tooling edits, mechanical doc generation | The plans' own stated audience |
| Bulk IO | **Haiku** | Reading many files and returning a distillation; inventory sweeps | W0 spec-list reads; grep-and-summarize sweeps |

**Escalation policy**: when implementation surfaces an open judgment call, dispatch a Fable
agent, take its recommendation, log it as a Plan 00 §12-format decision-log row, and proceed —
do not stop the unit to ask the owner mid-flight. Present the accumulated rows to the owner at
the unit's PR gate (§5), where they can still be reversed before merge.

**This policy never overrides `docs/ai/specs/safety.md`.** Commit-and-merge-to-`main` for this
program is pre-authorized by the owner (2026-08-29 — see `feedback_new-surfaces-authorization`
in project memory) so a unit's PR-merge checkpoint does not need to pause for confirmation. That
authorization is scoped precisely: it does **not** cover force-push, `git reset --hard`, branch
protection/ruleset changes, or anything touching NuGet publish/release tooling — those still get
per-instance confirmation regardless. `Directory.Packages.props`/`Directory.Build.props` edits
(new external dependencies) should still be surfaced to the owner, batched into one confirmation
per unit rather than skipped.

## 5. Open-decision gates

Plan 00 §12's "Still open" list, with recommendations from an independent Fable review
(2026-08-29) — presented per §4's escalation policy, not silently applied where the decision is
a product-surface commitment rather than a naming/implementation detail:

| # | Decision | Gates | Recommendation | Handling |
|---|---|---|---|---|
| D1 | Phase 4 bridges: ship all three (ErrorOr, FluentResults, OneOf) vs ErrorOr-only | 4-bridges, before opening the unit | **Ship all three** — Plan 04 already puts them in one PR, so ErrorOr-only saves little; naming work is already done | **Present to owner** — this is a product-surface commitment (packages to support forever), not a pure implementation call |
| D2 | `blank` vs `null-or-white-space` in §5.4 vocabulary | nothing (already shipped as `blank`) | **Keep `blank`, close the decision** — already on `main`; the alternative violates the grammar's own "never derive from method names" rule | Apply per policy; log in Plan 00 §12 |
| D3 | `ValidationOptions.AddMustValidators()` vs `AddMustValidatorResolver()` | 3-PR2 W7 | **`AddMustValidatorResolver()`** — it registers one resolver, not validators; the DI package's similarly-named `AddMustValidatorsFromAssembly` would be confusable otherwise | Apply per policy; log; owner can reverse at the 3-PR2 PR gate |
| D4 | §13 adopt proposals (`FluentMustValidator<T>`, `SetMustValidator`, the two `ValidationResult` bridges, `ToValidationResults()`, the coded DA runner, `MustValidator<T>.Rules`) | 3-PR1 scope (original "before 1b" gate was overtaken — 1b already merged without them) | **Adopt** `FluentMustValidator<T>` + `AddMustValidatorsFromFluentValidators()`, `SetMustValidator`, both bridges, `ToValidationResults()`. **Defer** `MustValidator<T>.Rules` (non-breaking to add later) and the coded DA runner (to the D5 checkpoint) | **Present `FluentMustValidator<T>` to owner before 3-PR1** (real product weight); apply the small additive items per policy |
| D5 | Final-review wire-up family (`AddGuardClauses()` etc.) | program close | Current Plan 00 §12 assessment looks right; nothing in Phases 2–6 needs it earlier | **Owner sign-off required — not a council call**, by design |

## 6. Per-unit checkpoint (mandatory, after every merged PR)

1. **Update §2's progress tracker** in this file — status, branch, merge commit. This is not
   optional busywork; it is the mechanism that makes the program resumable.
2. **Lessons capture** — anything a future session needs that the plans don't already say:
   checklist corrections fold into Plan 00 §8 *inside* the unit's own PR (Plan 02 §6 requires
   this); process discoveries go to `docs/ai/memory/`; engineering invariants go to the relevant
   `docs/ai/specs/` file. "We finished W3" is status (→ §2), not a lesson — do not write status
   into memory.
3. **Compact** the orchestrator's context. Carry forward only: §2's tracker state, any
   unresolved decision rows from §5, and the next unit's id.
4. **Re-read from disk**: this file (fresh — it may have changed) and the next unit's phase plan
   in full. Never proceed on remembered plan content; plans are amended by the PRs themselves.
5. **Verify repo state** (`git log`, `git worktree list`, clean status) before opening the next
   unit's worktree.

## 7. Known process lessons already on the books

- Plan 01's §4.1 generator was never built; hand-curation in-branch worked instead (Plan 01
  archive note). Phase 5 batches follow that precedent deliberately: codes are curated in the
  batch table and reviewed at the batch PR — no separate map artifact.
- Coverage is gated per TFM: two `-Framework` runs per scope, then `-Scope All` twice (Plan 00
  §7). Never trust a single multi-TFM run as proof of 100%.
- GitHub Actions repo variables (`vars.*`) are resolved once when a workflow run is **triggered**,
  not fresh at each step — a run in flight when a variable changes still uses the old value.
  Verify a variable change with a genuinely new run (`gh run rerun` after the change), not the
  run that happened to be executing when you made it.
- The union-merge file set (Plan 00 §10.3) conflicts trivially and resolves by taking both
  sides; merge `origin/main` into a unit's worktree before every PR; never rebase a pushed branch.

## 8. Related

- Charter: `docs/ai/plans/new-surfaces-missing-validation-cases-00-program.md`
- Phase plans: `new-surfaces-missing-validation-cases-0{1..6}-*.md` in this directory
- Safety tiers: `docs/ai/specs/safety.md`
- This program's memory: `docs/ai/memory/validation-builder.md`, `docs/ai/memory/test-writer.md`
