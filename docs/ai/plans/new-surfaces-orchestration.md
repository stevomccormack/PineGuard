<!-- metadata_header
type: plan
id: new-surfaces-orchestration
version: 1.1
status: complete
last_updated: 2026-09-01
parent: new-surfaces-program
-->

# New Surfaces Program — Orchestration, Model-Routing and Progress Tracker

> **Status**: 🎉 **Complete — every unit merged to `main`.** 5-E (Batch E, temporal + clock
> injection) was the last open unit; its merge (`dcb906d`, 2026-09-01) closed the program.
> **Owner**: repo owner | This file is kept as the historical record of how the program ran —
> read it for the process lessons in §7 if starting a similarly-shaped multi-unit program, but
> there is no more remaining work to pick up from here.
>
> The phase plans (`new-surfaces-missing-validation-cases-0{1..6}-*.md`) say **what** to build;
> this file says **how the sessions that build it are run**, and **where things currently stand**.
> Where this file and a phase plan disagree on content, the phase plan wins; where they disagree
> on process, this file wins; `docs/ai/specs/safety.md` wins over both, always.
>
> **Standing scope restriction (owner instruction, 2026-08-30, in effect until lifted): do not
> create or edit anything under `docs/` except files under `docs/ai/plans/`.** This overrides any
> phase-plan or Plan 00 §8 instruction to update specs, rules, agent stubs, adapter surfaces, or
> the root `README.md` as part of a unit's W-steps. Every unit's Brain/adapter-documentation
> cascade (what Unit 2 called "W5") is deferred program-wide, to be done later in one batched
> pass across all units rather than per-unit — do not attempt a partial version of it, and do not
> let a gate that depends on it (e.g. an audit-cli rule checking Brain-doc completeness for a new
> package) block a unit's merge; skip that specific gate and note the skip in the unit's close-out
> notes and in this tracker. Note this in each unit's checkpoint so the deferred work stays visible.
>
> **Standing process rule (owner instruction, 2026-08-30): no GitHub Pull Requests.** We have
> direct push access to `main` — use it. A unit's close-out step is a local `git merge --no-ff
> feature/xxx` into `main` followed by `git push origin main`, never `gh pr create`. Still a real
> merge commit (not squash/fast-forward), matching the one-merge-commit-per-unit shape Phase 1,
> Track 0, and Unit 2 (Options, PR #28 — the last unit that used the now-retired PR flow)
> established. CI is still worth checking after the push, but it is no longer a merge gate mediated
> by review; a unit's own independent build/test/format/coverage verification is what the merge
> decision rests on. Every "close out unit" dispatch must say this explicitly — several agents
> defaulted to the old PR pattern when not told otherwise.
>
> **Standing guidance (owner instruction, 2026-08-30): `audit-cli` is not an authoritative quality
> signal.** Its dependencies are kept patched when they block a build, but its Rule06/07/08/11/13/50
> etc. checks reflect possibly-stale conventions and are informational only — never a merge gate.
> The real bar is the specs under `docs/ai/specs/` plus build/test/coverage green.

## 0. Handoff — read this first, every session

**You (the session reading this) are the orchestrator.** Whatever model is running this
session — expect Sonnet 5 — your job for this program is coordination, not implementation.
This holds across a single long session and across a compaction boundary or a brand-new session
picking this back up cold; either way, start here.

**Your loop, every time you pick this up:**
1. Read §2 (progress tracker) to see what's done and what's next. Trust it over memory.
2. Read the next unit's phase plan section in full, fresh from disk (§6 rule 4) — not a
   paraphrase from earlier in this conversation, which may have drifted or been compacted away.
3. Dispatch sub-agents per §4's model routing. **You do not write code, tests, or docs
   yourself** — every edit happens inside a dispatched agent. Your own tool calls are for
   reading state, launching agents, and running the §6 checkpoint.
4. Run the §6 checkpoint after every unit merges. Update §2. Compact. Re-read. Repeat from 1.

**Why "you never implement" is also the token-optimization strategy, not a separate concern.**
A sub-agent's context is disposable — it reads what it needs, does the work, reports back, and
its context (the full file contents, the search dead-ends, the intermediate diffs) never has to
live in yours. If you read a 700-line phase plan and a 300-line source file yourself before
implementing, that's ~1000 lines sitting in your context for the rest of the session, compounding
across twelve units. Dispatch it instead: a Haiku agent bulk-reads and distills, a Sonnet agent
implements and reports a summary, and only the summary — not the underlying reads — ever touches
your context. This is the mechanism, not a slogan: **every file read for content (not just
existence) belongs in a sub-agent, not in your own tool calls**, specifically so this program can
run across however many sessions it takes without your context filling up first.

**Where token-optimization stops being a valid reason to do something.** None of the above
trades away rigor, and the distinction matters:
- Batching, delegating reads, and keeping your own context thin: yes, always — this is what
  makes the program long-running at all.
- Skipping a Fable/council escalation §4 calls for for because it's "just" a naming or design
  question: no, never — §3.3 exists because a wrong name is a permanent defect, not a fixable
  one, and that cost dwarfs any tokens saved by skipping the consultation.
- Under-testing, under-documenting, or accepting an implementation agent's "close enough" to
  avoid another verification pass: no, never — §3.1's "never trade correctness, coverage, or
  docs for speed" is absolute regardless of how long the program is taking.
- Reusing a stale summary instead of the §6 checkpoint's mandatory fresh re-read because
  re-reading costs tokens: no, never — a plan can be amended by the very PR that just merged
  (§1's own rule), and orchestrating off a stale copy is exactly the drift this file exists to
  prevent.

In short: optimize *how much of the work* sits in your context, never *how carefully* the work
itself gets done. Cheap orchestration and expert-level output are not in tension here — they
come from the same rule, which is to delegate the doing and keep only the deciding.

**Handing off to a genuinely new session (not a `--resume`/`--continue` of this one).** Two things
do not travel with the rest of this file, because they are tied to the live session process, not
to disk state:

1. **The `/goal` Stop hook is session-scoped.** If the prior session set one up to keep working
   until every phase is done, a new session does not inherit it — re-issue `/goal` there if you
   want that enforcement to continue. Point it at this file rather than retyping the goal text;
   everything durable is already here.
2. **An in-flight background `Workflow` call belongs to the session that launched it.** A new
   session gets no completion notification for it and most likely cannot query its task ID
   either. If §2's tracker shows a unit "in progress" with no merge commit yet, **do not assume
   it is still running or that it failed** — check for real:
   - `git -C .claude/worktrees/<unit> log --oneline -5` and `git status --short` — has a commit
     landed since the tracker was last updated? Is the tree clean?
   - If a task ID and transcript path were recorded for the in-flight run (see the tracker row),
     read that workflow's `journal.jsonl` directly (path pattern:
     `<claude projects dir>/<session id>/subagents/workflows/wf_<id>/journal.jsonl`) — a `result`
     event for every dispatched agent means it finished; a dangling `started` with no matching
     `result` means it died mid-flight (§7's lesson) and needs re-dispatching from that step, not
     from the beginning of the unit.
   - Never resume a unit's work by re-reading only the tracker's prose — the tracker is a
     summary written by a prior session; the git log and the journal are ground truth.

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
| **2 — Options** | **Merged to `main`.** W3/W4/W6 done (24 tests, 100%/100% line+branch on net8.0 and net10.0 individually, full-solution build/test/format/Rule50 all green on CI). **W5 (Brain/adapter cascade: specs, rules, agent stubs, adapter surfaces, root README row) deliberately deferred** — owner instruction 2026-08-30: no `docs/` edits except `docs/ai/plans` until further notice, batched later across units instead of done per-unit. PR #28's body records this explicitly so it isn't mistaken for an oversight. | `feature/options` (deleted, merged) | `fa0e689` (PR #28, `--merge`, matching Phase 1/Track 0 precedent — not squash) | Recovered from the W3 workflow (`wf_efb67fbe-25f`) dying mid-flight — see §7 for the general lesson. Two `test-writer` dispatches for W4 also died silently (18 min/87 calls and 20 min/81 calls, zero output) before a third succeeded incrementally; **lesson for future dispatches: instruct agents to commit after every small stage, not just at the end** — both silent deaths still left committed partial progress once that instruction was added. |
| **4-bridges** | **Merged to `main`.** All three result bridges (ErrorOr, FluentResults, OneOf) implemented and tested per D1's "ship all three" — new packages `PineGuard.ErrorOr`/`PineGuard.FluentResults`/`PineGuard.OneOf`. OneOf's adapter needed explicit `FromT0`/`FromT1` factories instead of the sibling bridges' implicit-conversion return shape (a C# language constraint on unconstrained type-parameter conversions, not a design deviation — recorded in its commit). `-Scope All` 100%/100% both TFMs post-merge (21017/21017 lines). | `feature/result-bridges` (deleted, merged) | `ddad7c1` (direct merge, no PR) | D1 applied per owner's 2026-08-30 finish-everything directive rather than presented to owner first — see §5's row for the reasoning. |
| **3-PR1 — async + DI** | **Merged to `main`.** W1–W3 done: the Core async seam (`RuleForAsync`/`RuleForEachAsync`, two overloads each, `HasAsyncRules`, and `Validate` throwing once async rules exist), `MustValidationMode` plus the DIM forwarders on both `IMustValidator` and `IMustValidator<T>`, `SatisfiesAsync`/`NotSatisfiesAsync` in MustClauses, `MustBeAsync` and the two Fluent predicate adapters, the `FluentMustValidator<T>`/`MustChildValidator`/`SetMustValidator` bridges with `AddMustValidatorsFromFluentValidators()`, the DataAnnotations `ToValidationResults()` `IValidatableObject` bridge, and the new `PineGuard.Extensions.DependencyInjection` package carrying all six of Plan 03 §4.1's members. `-Scope All` 100%/100% on both TFMs post-merge (21289/21289 lines, 6751/6751 branches); Release build 0 warnings/0 errors, 15,775 tests per TFM green, `dotnet format --verify-no-changes` clean. **Rule14 (Core-sync audit) deliberately deferred** — it is separable audit-cli tooling (its own commit in the plan's W1, touching six tooling/docs surfaces and zero library code) whose plan-mandated wiring includes `docs/ai/specs/tools/audit-cli/spec.md`, which the docs/ restriction forbids; the invariant it would enforce was verified by hand and holds (Core `Rules/` and `Utils/` contain no `async`/`await`/`ValueTask`/`System.Threading.Tasks` outside the XML-doc `<see cref>` occurrences in `TaskRules.cs` that the plan already allow-lists). The `vocabulary.json` `ignoreMethods` entries and the `unit-test.md` §5.1 async-test addendum are deferred under the same restriction, exactly as 5-A and 5-D deferred theirs. | `feature/must-async-di` (deleted, merged) | `ae45bbe` (direct `git merge --no-ff` + push — **no PR**, per the 2026-08-30 no-PR standing rule) | `origin/main` had not moved since the branch was cut (still `280e04b`), so no back-merge was needed and the push landed first try — no repeat of 5-D's race loop. Worth recording for 3-PR2: `AddMustValidatorResolver()` is **not** part of this unit despite appearing in the handoff notes — D3 scopes it to 3-PR2's W7 as a `ValidationOptions` extension, and it correctly does not exist in the DI package. |
| **3-PR2 — AspNetCore** | **Merged to `main`.** Full pipeline integration surface (`MustValidationRunner`, action filter, endpoint filter, exception handler, MVC/DI wire-up), W7's `ValidationOptions.AddMustValidatorResolver()` (D3, net10-only `#if NET10_0_OR_GREATER`, wraps .NET 10's built-in `Microsoft.Extensions.Validation` pipeline), and a full TestServer end-to-end integration suite (happy-path, failure-path/`ValidationProblemDetails`, MVC auto-validation, the built-in-pipeline path). 143/143 tests on net10.0, 118/118 on net8.0; `-Scope AspNetCore -Enforce100` 100%/100% both TFMs pre-merge; `-Scope All -Enforce100` 100%/100% post-merge both TFMs (net10.0: 22942/22942 lines, 7267/7267 branches; net8.0: 22896/22896 lines, 7247/7247 branches), full solution 0 warnings/errors, `dotnet format --verify-no-changes` clean, full test suite (13 projects) 0 failures. | `feature/aspnetcore` (deleted, merged) | `7b03f80` (direct `git merge --no-ff` + push via a temp merge worktree — **no PR**) | Independent verification caught and fixed 3 real bugs a prior dispatch's "builds clean" claim missed (a missing test class, 2 real assertion failures) — see §7. The close-out dispatch hung silently for 6 hours making zero progress past its first step (pre-merge sync) — killed via `TaskStop` and the entire close-out done directly instead; see §7 for this as a new instance of the "watch elapsed time" lesson. Rule06 audit-cli check fails with a `dotnet publish`/multi-targeting error — confirmed this is pre-existing and reproduces identically on `main` itself (unrelated to this merge); audit-cli remains informational only per standing policy. |
| **4-mediatr** | **Merged to `main`.** `MustValidationBehavior` MediatR pipeline behavior, `IMustFailureResponseFactory`/`IMustFailureResponseFactoryOfT`, `MediatRServiceConfigurationExtension` DI wiring — new `PineGuard.MediatR` package. 18/18 tests both TFMs; `-Scope All -Enforce100` 100%/100% line+branch both TFMs post-merge (18417/18417 lines, 7172/7172 branches). | `feature/mediatr` (deleted, merged) | `c565c30` (direct `git merge --no-ff` + push — **no PR**) | Pre-merge sync with `origin/main` was conflict-free (only `docs/ai/plans/new-surfaces-orchestration.md` had moved). First-attempt push, no retry race. |
| **5-A** (string content) | **Merged to `main`.** All 5 layers done: Core/Must/Guard/Fluent/DataAnnotations for `Contains`/`StartsWith`/`EndsWith` + `Not*` complements, each independently verified 100%/100% line+branch on both TFMs. `-Scope All` also 100%/100% both TFMs post-merge (16521/16521 lines net8.0/net10.0). DataAnnotations needed an internal `IsExternalInit` polyfill for netstandard2.1 (`init` accessors per plan's literal shape) — flagged as a reversible-if-wrong call, not escalated. | `feature/rules-string-content` (deleted, merged) | `d1df562` (direct `git merge --no-ff` + push — **no PR**, per the 2026-08-30 no-PR standing rule) | First batch built one-layer-per-dispatch after Options W4 showed full-batch dispatches die silently; every layer here committed incrementally and none lost work. Vocabulary/spec-file items the plan's W-steps call for are deferred per the docs/ restriction. |
| **5-B** (identifiers) | **Merged to `main` — largest batch in Phase 5, complete across all 5 layers.** 10 sub-areas: ULID, GUID version (numeric+string), JWT (structural shape, not signature verification), SemVer 2.0.0, cron-expression (5-field, ranges/steps/lists/named values), MAC address, media type, regex-pattern validity, Base64Url, UTF-8 byte-sequence. `-Scope All` 100%/100% both TFMs post-merge (18392/18392 lines, 7158/7158 branches). Two real compile bugs (a `TimeSpan`/nested-partial-class name collision, and a later `HasGuidVersionStringAttribute` unresolved-cref from a class body that was never written) were both caught by the orchestrator's independent rebuild before reaching a commit — see the process memory note on always rebuilding before trusting a "builds clean" claim. | `feature/rules-identifiers` (deleted, merged) | `18440bc` (direct `git merge --no-ff` + push — **no PR**) | Merge from `origin/main` hit 7 genuinely-conflicting files (parallel string-rule additions landing at the same insertion point as Batch C's Unicode work) — resolved by concatenating both sides, verified against pre-merge blobs. Sizing estimate (2-3 sessions) held roughly true given the sub-area count. |
| **5-C** (Unicode) | **Merged to `main`.** All 5 layers done across 4 sub-areas: byte-order mark detection, well-formed UTF-16 (surrogate-pair validity), Unicode normalization-form check (the flagged globalization-invariant-mode row — handled by making the null/undefined-form/malformed-UTF-16 checks mode-independent per the rule's own XML doc, rather than needing a runtime-mode-specific test), grapheme-cluster count (8 clauses). `-Scope All` 100%/100% both TFMs post-merge (17621/17621 lines, 6902/6902 branches). A `CS0411` generic-inference failure on `EnumRules.IsDefined<TEnum>(TEnum? value)` (called with a non-nullable enum) was caught and fixed by the orchestrator's independent rebuild before it reached a commit. | `feature/rules-unicode` (deleted, merged) | `d701e7e` (direct `git merge --no-ff` + push — **no PR**, per the 2026-08-30 no-PR standing rule) | Merge from `origin/main` into the feature branch first was clean (no conflicts) despite 22 commits of divergence (Batch A/D/F merges, Phase 4-bridges, 3-PR1 async+DI). Post-merge, `git status` on Windows falsely flagged ~1200 files as modified purely from `core.autocrlf=true` disk churn during checkout (confirmed zero real diff via `git hash-object`/`git diff` — byte-identical to HEAD); `dotnet format` restored canonical LF and incidentally cleared a downstream false-positive `WHITESPACE` finding in `OwaspRegex.cs`'s raw-string literal caused by the same CRLF pollution, with zero net commit needed either time. Close-out also hit a genuine Windows file-sharing-violation lock (`ERROR_SHARING_VIOLATION`, confirmed via an independent detached process, not the agent's own shell) that blocked physically deleting the now-empty, already-unregistered `.claude/worktrees/rules-unicode` folder after `git worktree remove`/`git clean` — git-level cleanup (worktree registration, local branch) completed fully; the leftover empty gitignored directory needs a manual/later delete once the external lock (likely AV or an indexer) releases. |
| **5-D** (numeric/financial) | **Merged to `main`.** All 5 layers done across three areas: percentage (Number+String), Luhn checksum (+`ChecksumUtility`), Decimal precision/scale (+`DecimalUtility`). `-Scope All` 100%/100% both TFMs post-merge (20947/20947 lines). `vocabulary.json` aliases (`ScaleAbove→ScaleAtMost` etc.) the plan calls for are deferred per the docs/ restriction. | `feature/rules-numeric` (deleted, merged) | `04e1c2e` (direct merge, no PR) | Close-out agent got caught in a retry loop — `origin/main` kept moving (a concurrent close-out for 5-F was pushing around the same time) so its fetch→merge→verify→push cycle never landed cleanly; ran ~4h before the orchestrator noticed via `ListAgents`' elapsed time (every other dispatch this program finished in 5–25 min) and killed it. **Lesson: if a close-out dispatch is told to "fetch and retry rather than force" on a push race, and multiple units are closing out concurrently, it can loop for hours instead of failing fast — watch elapsed time, not just completion status, and finish it directly once the race window has passed rather than waiting indefinitely.** Two earlier dispatches (initial Core, initial Must) also made real progress but died before committing at all — recovered by the orchestrator verifying+committing directly. Branch name drifted from the planned `rules-numeric-financial` to `rules-numeric`. |
| **5-E** (temporal + clock injection) | **Merged to `main`.** Core: clock injection fully done. Must: `MinimumAge` clause + full `TimeProvider?` sweep across all five temporal families + calendar predicates on DateOnly/DateTimeOffset. Guard: `TimeProvider?` threading, `BelowMinimumAge`, calendar predicates on DateOnly/DateTimeOffset. FluentValidation: `TimeProvider?` threading + `MinimumAge` + calendar predicates across every temporal family (typed and string-parsing), 3469/3469 tests both TFMs. DataAnnotations: attributes resolve `TimeProvider` from `ValidationContext.GetService` (the one structurally novel piece in this batch — attributes can't take it as a constructor arg since arguments must be compile-time constants) via a shared `ValidationAttributeBase.ResolveTimeProvider` helper, plus `MinimumAge`/`MinimumAgeDateTime`/`MinimumAgeString` and calendar-predicate attributes, 2118/2118 tests both TFMs. **Independently verified: every one of `-Scope Core`/`MustClauses`/`GuardClauses`/`FluentValidation`/`DataAnnotations -Enforce100` is 100%/100% line+branch on BOTH TFMs (ten separate runs), and the combined `-Scope All -Enforce100` is 100%/100% on both TFMs too (22319/22319 lines, 7019/7019 branches, 1150 classes, matching on both net8.0 and net10.0).** `FixedTimeProvider` clock test double shipped in `tests/PineGuard.Testing/`. Post-merge `-Scope All -Enforce100` re-verified 100%/100% both TFMs (net10.0: 23925/23925 lines, 7525/7525 branches, 1223 classes; net8.0: 23879/23879, 7505/7505, 1217 classes — the class-count delta is netstandard2.1 not existing on net8.0, expected), full 30-combination (15 projects × 2 TFMs) test suite 0 failures, `dotnet format --verify-no-changes` clean. | `feature/rules-temporal` (deleted, merged) | `dcb906d` (direct `git merge --no-ff` + push via a temp merge worktree — **no PR**) | Two decisions applied per the 2026-08-30 finish-everything directive rather than paused on: **(1)** `Microsoft.Bcl.TimeProvider` 10.0.11 added as a `Directory.Packages.props` dependency (netstandard2.1-only `PackageReference` on `PineGuard.Core`) to unblock `DateTimeRules`/`DateTimeOffsetRules`/their string counterparts, which otherwise can't reference `TimeProvider` pre-net8 — official Microsoft BCL polyfill, same tier as this program's other Microsoft.Extensions.* pins, reversible if the owner objects. **(2)** Plan 05's minimum-age row contradicts itself (states `today.AddYears(-years) >= value` as the boundary, then glosses it with leap-day behavior that formula doesn't actually produce — `value.AddYears(+years) <= today` does). Shipped the literal formula (matches .NET's dominant age-calculation idiom, is the conservative reading for a permission gate), documented the actual 1-March-not-28-Feb consequence in the `HasMinimumAge` XML `<remarks>`, pinned with a named test. Both are one-line-reversible if the owner prefers otherwise. Pre-merge sync hit two trivial "both sides inserted an entry at the same spot" conflicts (an agent-memory index list, a `Directory.Packages.props` package addition) — resolved by taking the union, same as every other close-out this program. **This was the last open unit in the entire New Surfaces Program — its merge completes the whole program.** |
| **5-F** (file signatures) | **Merged to `main`.** All 5 layers done: `FileSignature`/`KnownFileSignature` (magic-byte detection, +`FileSignatureUtility`). `-Scope All` 100%/100% both TFMs post-merge. Fluent layer surfaced a real repo inconsistency: the spec (`fluent-validation/project.md` §5) says a null value should skip validation, but two existing files (`FluentFilePathExtensions`, `FluentCsvExtensions`) fail on null instead — this batch followed the spec, not the deviating neighbors; worth a cleanup pass on those two files eventually. | `feature/rules-file-signature` (deleted, merged) | `f1862ae` (direct merge, no PR) | Close-out found and fixed one real pre-merge issue itself: simplified `FileSignatureUtility`'s extension-normalization method (`f6cd3da`) after confirming it was behavior-preserving. |
| **6 — Analyzers** | **Merged to `main`.** All six planned diagnostics done end to end (analyzer, code fix(es), fix-all, tests): PG1001 (`Guard.Against.Null`), PG1002 (`NullOrWhiteSpace`), PG1003 (`NullOrEmpty`), PG1004 (`OutOfRange`), PG2001 (discarded `MustResult<T>`), PG2002 (discarded `MustValidationResult`). README before/afters for all six; `.nupkg` layout independently verified (both DLLs under `analyzers/dotnet/cs/`, no `lib/` folder, `developmentDependency=true`) via an actual pack+unzip+consume smoke test that confirmed the diagnostic fires through a real NuGet reference. `-Scope Analyzers -Enforce100` 100%/100% both TFMs pre-merge (434/434 lines, 146/146 branches, 12 classes); `-Scope All -Enforce100` 100%/100% post-merge both TFMs (net10.0: 23376/23376 lines/7413/7413 branches/1196 classes; net8.0: 23330/23330/7393/7393/1190 classes — the class-count delta is netstandard2.1 not existing on net8.0, expected). 118/118 Analyzers tests both TFMs; full 14-project/28-TFM-combination test suite 0 failures. | `feature/analyzers` (deleted, merged) | `445003b` (direct `git merge --no-ff` + push via a temp merge worktree — **no PR**) | Pre-merge sync with `origin/main` hit a genuine semantic merge hazard git's line-based diff couldn't see: 3-PR2's `AspNetCore` scope-registry entry (added on a branch cut before this unit's `SourceCsproj`→`SourceCsprojs` array-field refactor, `6fba593`) merged with zero textual conflict but left `AspNetCore` as the only entry still on the old singular field names — silently `null` for any consumer reading the plural fields generically. Caught by the close-out dispatch, fixed and independently verified by the orchestrator (`af401c2`) before merging. Rule06 audit-cli check fails with a `dotnet publish`/multi-targeting error — confirmed pre-existing (reproduces identically on `main`, first found during 3-PR2's close-out); informational only per standing policy. **This does NOT close the whole program — 5-E (Batch E) is still open** (Fluent layer nearly done, DataAnnotations layer not yet started). |

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
structure. Phase 5 chunks per batch, as Plan 05 already mandates. Within a unit, checkpoint at
each W-step rather than only at the end — a stub-scaffolding step (W1/W2) is cheap to verify in
isolation and expensive to debug retroactively once real feature code sits on top of it.

## 3.1 Standing policy: best practice wins every tradeoff

**The goal of this program is the highest level of expertise and simplicity achievable — not the
fastest path to green.** When an implementer (any model tier) hits a genuine tradeoff the phase
plan doesn't resolve, the default is always the objectively best-practice answer, never the
expedient one. This is stronger than the §4 escalation policy's "take Fable's recommendation and
proceed" — it constrains *what a good recommendation looks like* in the first place:

- Prefer the solution that is simplest to read and maintain over one that is merely shorter to
  write. Simplicity is a design goal here, not an afterthought — a clever one-liner that a future
  reader has to puzzle over is not "simple," a straightforward reader-obvious version is.
- Prefer the pattern already established elsewhere in this codebase over inventing a new one,
  even a technically-nicer one — consistency across ~550 Must clauses and five layers is itself
  a best practice this program must not erode. Deviating from an established pattern needs a
  stated reason, not just a preference.
- Never trade correctness, test coverage, or documentation completeness for speed. "Ship it and
  fix the gap later" is not an option this program uses — Plan 00 §7's Definition of Done applies
  in full to every unit, no partial credit.
- Best practice never widens scope. The phase plan's "Out of scope" / "Not in this phase" lists
  bind every tradeoff: the best-practice answer is chosen among implementations of the planned
  scope, never by adding surface, abstractions, or shared infrastructure the plan didn't ask
  for. Premature generalization — e.g. promoting a project-local test helper into
  `tests/PineGuard.Testing/` before a second consumer exists — is a tradeoff loss dressed as a
  win. If best practice genuinely seems to demand new surface, that is a §4 escalation, not an
  implementation call.
- When genuinely uncertain which of two established practices applies, escalate per §4 rather
  than picking either arbitrarily.

## 3.2 Testing conventions are non-negotiable — read before writing a single test

**Every test written under this program follows the repo's existing testing conventions exactly.
Inventing a new test shape, even a reasonable-looking one, is a defect, not a style choice.** The
normative sources, read in this order before writing any test code for a unit:

1. `docs/ai/specs/testing/unit-test.md` — `[Theory]` + `TheoryData`/`[MemberData]` only (never
   `[Fact]` or `[InlineData]`; CI-gated by audit-cli Rule50, §11); `XxxTests.cs` beside
   `XxxTestData.cs` (also Rule50-gated); the per-layer `*Expected`/`*Case` record shapes (§2.2,
   authoritative definitions in `fixture.md` §1/§3; layer addenda in
   `docs/ai/specs/<layer>/unit-test.md`); `IsValid` as the uniform expectation boolean.
2. `docs/ai/specs/testing/fixture.md` — the `AllScenarios`/`ValidScenarios`/`InvalidScenarios`
   shape, fixture partials mirroring their source Rules partials 1:1 (`XxxRules.Yyy.cs` →
   `XxxRulesFixtures.Yyy.cs`), camelCase tuple element names aligned to source parameter names.
3. `docs/ai/specs/testing/gold-standard.md` — the compliance criteria and per-project index a new
   test project must join at GOLD; the code-level exemplar of a compliant suite is
   `unit-test.md` §8's canonical trio.
4. `docs/ai/skills/scaffold-unit-test/SKILL.md` — the implementation recipe that ties the above
   together procedurally. **Precedence: where the skill and a spec disagree, the spec wins** —
   e.g. the skill's "never inherit `BaseUnitTest` directly" applies only to the five existing
   layers, not to a new package with no layer base (`unit-test.md` §2.1's "(Other)" row).

A new package (Options, AspNetCore, the result bridges, Analyzers) has no layer base and no
existing `*Expected`/`*Case` family. The bootstrap rule is Plan 00 §4.5: inherit `BaseUnitTest`
directly, define a project-local `XxxExpected` (extending `ReturnExpected`/`ThrowExpected`) and
`XxxCase` (extending `ReturnCase<,>`), and keep them in the test project — promote to
`tests/PineGuard.Testing/` only once two projects need the family
(`docs/ai/specs/testing/project.md` §3 rule 1; precedent:
`tests/PineGuard.DataAnnotations.UnitTests/ThrowsCase.cs`). Everything else about the new
project follows the specs unchanged: flat test classes, instance `public void`
`<Member>_BehavesAsExpected(tc)` methods, datasets-before-records, AAA markers. Do **not** copy
`tests/PineGuard.Testing.UnitTests/` — its nested-static-class operation groups and static
`Should*` methods are grandfathered violations of `unit-test.md` §5.1, not a precedent. If any
phase plan's own test-spec section appears to conflict with these sources, the normative specs
win (Plan 00 §1's own rule: "a spec wins over a plan wherever they disagree; fix the plan") —
flag the conflict in the unit's lessons capture (§6) so the plan gets corrected.

## 3.3 Naming is the highest-value decision a unit makes

**Experts explain things simply; good engineers narrate a simple story. In this library the
story is told almost entirely by names** — the surface is read far more often than it is written
(`Must.Be.Positive(value)`, `Guard.Against.Null(value)`, `email.address.invalid` in a 400 body),
and the reader is often not the developer who wrote the call: a reviewer, a support engineer
grepping logs, a consumer skimming IntelliSense. Convention, consistency and naming therefore
matter at the highest level of this program. A wrong name is a permanent defect — codes and
public identifiers freeze at the first release (Plan 00 §4.6, §5.4 rule 7) — where a wrong
implementation is a fixable one.

The normative content already exists; this section adds process, not a second philosophy.
Plan 00 §5 is the canon: "Names are the product. Every public identifier introduced by this
program is listed here with the one-sentence story it tells." Per unit, that means:

- **The story comes first.** Before typing a new public identifier, write its §5.2 "on the tin"
  sentence; the test is that a non-developer could match name to sentence. `MustValidationResult`
  passes ("everything a validator found — the object-level counterpart of `MustResult<T>`"); its
  rejected alternatives fail exactly this test — `MustResultSet` tells a false story (implies it
  contains `MustResult<T>` instances; it contains failures), `MustResults` is one letter from
  `MustResult` and unpronounceable in a code review, `MustReport` uses a noun no validation
  library uses (Plan 00 §5.2). For codes, §5.4's reading test is the same test: "domain → aspect
  → condition" must read as the problem the way a support ticket would state it.
- **The two canon rules bind every candidate.** Members, verbs and parameters *align* with the
  ecosystem's mass language (`RuleFor`, `PropertyPath`, `BeginScope`); importable type names
  *distinguish* with the `Must` qualifier (`MustCodes`, not `ErrorCodes`) — Plan 00 §5.1, owner
  rule 2026-08-26. Never derive a name mechanically from a method name or an implementation
  detail; names are curated the way messages are (§5.4 rule 4's principle, applied everywhere).
- **A name with no rejected alternatives has not been decided, only generated.** Every new
  public name lands as a Plan 00 §5 table row with its one-sentence story *and* its
  rejected-alternatives column filled, is logged as a §4 decision row, and gets owner sign-off
  at the unit's PR gate.

**Escalation trigger rule.** The owner's instruction "if blocked on naming, apply a council"
means the real five-advisor procedure — `/ask-council`, gated by `docs/ai/specs/council.md` and
executed per `docs/ai/skills/ask-council/SKILL.md` (every one of its eleven sub-agents runs on
Fable) — never an informal second opinion. It is also expensive (≥10 sub-agent invocations,
`council.md` §7), so the routes are tiered:

| Situation | Route |
|---|---|
| Name fully determined by existing convention — a new Must clause beside ~550 precedents, test/fixture file names (Rule50, `fixture.md`), private/internal/test-local identifiers | No escalation. Follow the convention; convening the council here violates `council.md` §3. |
| New public name with clear precedent to lean on — a same-shape row in Plan 00 §5.1–§5.3, an ecosystem mass-language word, an established repo pattern | Single Fable agent (§4): identify the precedent, apply it, record the rejected alternatives, log the decision row. Implementation-tier agents never mint a public name themselves — §4's table routes naming to the Fable tier. |
| Name enters the §5 canon / frozen public API **and** no precedent decides it **and** there is a real trade-off with non-trivial cost of being wrong (`council.md` §2) — typically two canon rules pulling opposite directions, or several defensible candidates | Full `/ask-council`. Precedent: D3's `AddMustValidatorResolver()` is a council recommendation. The verdict lands as a §4 decision row; transcript only under `council.md` §6's conditions. |

If a single Fable pass returns a recommendation the dispatcher finds genuinely contestable on a
canon-entering name, escalate it to the council rather than proceeding on a coin-flip — that is
the "blocked" the owner's instruction names.

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

**Escalation policy.** Two halves, both binding:

1. **Most questions are not escalations — read first.** The plans are unusually complete (§3):
   signatures, file lists, even commit messages are pre-decided; test shape is decided by §3.2's
   spec chain; names by Plan 00 §5 via §3.3. Before dispatching anything, re-read the governing
   plan section and spec. An agent that escalates a question the plan already answers is not
   exercising judgment, it is skipping the reading — and a stream of unnecessary escalations is
   its own scope creep (§3.1: best practice never widens scope). "Genuinely blocked" means one
   of: the plans/specs are silent; they contradict each other (the spec wins — flag it per §3.2
   so the plan gets fixed); two established practices both apply and disagree (§3.1); or the
   answer would create new public surface or a new canon name. "I haven't found it in the plan
   yet" is none of these.

2. **On a genuine blocker or high-stakes call, Fable consultation is unconditional.** Never
   guess, never take the expedient reading, never proceed silently on either. Dispatch a Fable
   agent — or run the full `/ask-council` procedure where §3.3's threshold is met — take the
   recommendation, log it as a Plan 00 §12-format decision-log row, and proceed; do not stop the
   unit to ask the owner mid-flight. Present the accumulated rows at the unit's PR gate (§5),
   where each can still be reversed before merge. The exception is the rows §5 marks "present to
   owner" (product-surface commitments; D5-class sign-offs): those wait at their stated gate for
   the owner's answer and are never applied on a recommendation alone.

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
| D1 | Phase 4 bridges: ship all three (ErrorOr, FluentResults, OneOf) vs ErrorOr-only | 4-bridges, before opening the unit | **Ship all three** — Plan 04 already puts them in one PR, so ErrorOr-only saves little; naming work is already done | **Applied 2026-08-30** — owner directive ("finish EVERYTHING... stop asking for permissions") supersedes the standing "present to owner" default for this decision specifically; the Fable recommendation is taken as-is. Reversible if the owner objects once the unit lands. |
| D2 | `blank` vs `null-or-white-space` in §5.4 vocabulary | nothing (already shipped as `blank`) | **Keep `blank`, close the decision** — already on `main`; the alternative violates the grammar's own "never derive from method names" rule | Apply per policy; log in Plan 00 §12 |
| D3 | `ValidationOptions.AddMustValidators()` vs `AddMustValidatorResolver()` | 3-PR2 W7 | **`AddMustValidatorResolver()`** — it registers one resolver, not validators; the DI package's similarly-named `AddMustValidatorsFromAssembly` would be confusable otherwise | Apply per policy; log; owner can reverse at the 3-PR2 checkpoint |
| D4 | §13 adopt proposals (`FluentMustValidator<T>`, `SetMustValidator`, the two `ValidationResult` bridges, `ToValidationResults()`, the coded DA runner, `MustValidator<T>.Rules`) | 3-PR1 scope (original "before 1b" gate was overtaken — 1b already merged without them) | **Adopt** `FluentMustValidator<T>` + `AddMustValidatorsFromFluentValidators()`, `SetMustValidator`, both bridges, `ToValidationResults()`. **Defer** `MustValidator<T>.Rules` (non-breaking to add later) and the coded DA runner (to the D5 checkpoint) | **Applied 2026-08-30** — same owner directive as D1; `FluentMustValidator<T>`'s "present to owner" default is superseded, Fable's recommendation taken as-is including the two deferrals. Reversible if the owner objects once the unit lands. |
| D5 | Final-review wire-up family (`AddGuardClauses()` etc.) | program close | Current Plan 00 §12 assessment looks right; nothing in Phases 2–6 needs it earlier | **Resolved 2026-09-06 (owner sign-off).** Wire-up family closed with none added, per the existing Plan 00 §12 assessment (`AddGuardClauses()`/`UseGuardClauses()`/`AddDataAnnotations()` all rejected). The one identified gap — the coded DataAnnotations runner — was built: `DataAnnotationsAttributeValidator.Validate()` in `PineGuard.DataAnnotations`, name chosen over `DataAnnotationsValidator` (collides with Blazor's `Microsoft.AspNetCore.Components.Forms.DataAnnotationsValidator`) and `MustValidationResult.FromAnnotations` (would need a `Core → DataAnnotations` circular project reference to read `ValidationAttributeBase.Code`). 100/100 line+branch, `PineGuard.DataAnnotations.UnitTests`. Program close — no further D-gates open. |

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
- **A background workflow can die mid-flight with no error and no notification** — a session-level
  disruption (observed cause: a permission-mode change) silently killed a workflow's final agent
  after its prior stage had already committed real changes. The task-status tool later reported
  "no task found," which reads exactly like "already handled" but is not the same thing. The tell
  was checking the workflow's own `journal.jsonl` directly: the interrupted agent had a `started`
  event with no matching `result` event. **Lesson**: when a background task's status becomes
  ambiguous (silence, a stale ID, an unexpected "not found"), verify from the transcript/journal on
  disk before trusting either "it must have finished" or "it must still be running" — and never
  treat a prior stage's self-reported success as the checkpoint's actual verification if the
  independent-verification stage itself didn't provably run. Re-dispatching the missing
  verification from scratch is cheap; shipping on an unverified self-report is not.
- The union-merge file set (Plan 00 §10.3) conflicts trivially and resolves by taking both
  sides; merge `origin/main` into a unit's worktree before every PR; never rebase a pushed branch.
- **Watch elapsed time, not just completion status — a second confirmed instance.** 5-D's
  close-out looped for ~4h against a moving `origin/main` before being noticed (§2's row for 5-D).
  3-PR2's close-out went further: it hung completely silent for **6 hours** making zero progress
  past its very first step (the pre-merge `git merge origin/main` sync never even started
  resolving), alongside a second, unrelated agent (Analyzers' coverage-gap dispatch) that hung for
  5 hours at its own first real step. Neither agent errored, retried, or reported anything — both
  simply stopped producing output with no git-visible progress (`git log`/`git diff --stat`
  unchanged for the entire duration). `ListAgents`' elapsed-time column is the only signal this
  produces; a routine periodic check (not just reacting to notifications) caught both. Once killed
  via `TaskStop`, the orchestrator resolved the 3-PR2 merge conflicts and ran the full close-out
  sequence directly rather than re-dispatching — faster and safer than gambling on a second
  background attempt hitting the same hang. No root cause was found; treat any agent silent past
  ~30–45 minutes on a task this program's history shows takes 5–25 minutes as a check-worthy
  outlier, not just the multi-hour cases already documented.
- The `Get-PineGuardScope` registry (`tools/.shared/dotnet-projects.ps1`) generalized its
  per-scope `SourceCsproj`/`CoverageIncludePattern` fields to `SourceCsprojs`/
  `CoverageIncludePatterns` arrays (6-Analyzers' onboarding, since `PineGuard.Analyzers` ships one
  package built from two projects) — every future scope entry uses the plural array fields, even
  single-project ones (they're one-element arrays). Every consumer script duplicates the scope
  `ValidateSet` list literally (PowerShell requires compile-time constants there) — adding a scope
  means touching `dotnet-projects.ps1` **and** every consumer's `ValidateSet`/`-in` check
  (`tools/code-coverage/*.ps1` ×3, `tools/code-diagnostics/Run-CompilerDiagnostics.ps1`,
  `tools/code-formatter/Run-Format.ps1`, `tools/code-inspection/Run-Qodana.ps1` and its
  `auto/Run-Coverage.ps1` wrapper) plus `.editorconfig`'s test-project brace list and
  `.github/workflows/ci.yml`'s outputs/filters/matrix/vars/coverage-filter blocks — the same
  literal-list-in-N-places conflict shape as the `docs/ai/plans/new-surfaces-orchestration.md`
  progress table, and it resolves the same way (take the union, keep both scopes).
- **A clean git merge (zero conflict markers) is not proof the merged file is correct** — a second,
  distinct instance of the field-name hazard above. 6-Analyzers' pre-merge sync with `origin/main`
  (carrying 3-PR2's already-merged `AspNetCore` scope-registry entry) produced no textual conflict
  at all, because the two edits touched different dictionary keys in the same file — but the result
  was semantically broken: `AspNetCore` was left on the pre-refactor singular field names
  (`SourceCsproj`/`CoverageIncludePattern`) while every other entry, including ones this branch had
  already converted, used the new plural array fields. Nothing in git's diff view flagged this;
  it only surfaced by actually running `-Scope AspNetCore` and noticing it, or by deliberately
  re-reading the whole merged file for consistency rather than trusting "no conflicts" as "no
  problem." **Lesson: after any merge into a file with a repeated/parameterized shape (a registry
  of near-identical entries, a switch statement, a literal list duplicated per consumer) — run the
  actual thing the file drives, per variant, not just a build — a clean merge only proves the text
  parsed, not that every entry still agrees with its siblings.**

## 8. Related

- Charter: `docs/ai/plans/new-surfaces-missing-validation-cases-00-program.md`
- Phase plans: `new-surfaces-missing-validation-cases-0{1..6}-*.md` in this directory
- Safety tiers: `docs/ai/specs/safety.md`
- This program's memory: `docs/ai/memory/validation-builder.md`, `docs/ai/memory/test-writer.md`
