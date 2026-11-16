# Technical readiness to 1.0 — pilot tracker

This page is the single source of truth for the state of the pilots that the
[council verdict](../technical-readiness-to-1.0.council-transcript.md) of 2026-11-09 placed ahead of
every wave ([decision register](references/decision-register.md) D29). It replaces the closure
ledgers and the planning-era review protocol for the pilot. The [plan](plan.md) says what the
programme is; this page says where the pilot stands.

## Authorisation

The owner authorised exactly one unit on 2026-11-09: the string→DateOnly pilot, as a single PR, per
D29. The authorisation was given through the scheduled-task instruction that runs this pilot and is
recorded here, in the [plan header](plan.md) and in D19, as D19 requires. It covers the pilot only.
Email, bounded numeric, W22, Temporal Phase 2 and every wave stay parked.

## Pilots

| Pilot | Status | PR | Names signed | Benchmark numbers | Verdict |
|---|---|---|---|---|---|
| string→DateOnly | In PR (draft) — C1 and C2 done, C3 next | [#66](https://github.com/stevomccormack/PineGuard/pull/66) | No — table not drafted yet (C7) | None yet (C6) | Open |
| Email | Parked — runs only if the string→DateOnly pilot passes | — | — | — | — |
| Bounded numeric | Parked — runs only if the string→DateOnly pilot passes | — | — | — | — |

## Time-box

| Item | Value |
|---|---|
| Start (first code commit) | 2026-11-16 (`f676e18`) |
| End (start + 14 days) | 2026-11-30 |
| Days remaining | 14 of 14 on 2026-11-16 |

The council's own wording is stricter: "two weeks from authorization to merge of PR one". Read that
way the box started on 2026-11-09 and ends on 2026-11-23. The scheduled-task instruction defines the
start as the first code commit, and this page follows it. The difference is listed under
[Awaiting owner](#awaiting-owner) as AO-5.

## Exit and kill criteria

D29, verbatim from the [decision register](references/decision-register.md):

> Exit: no adapter contains rule logic; consistency test green; benchmarks committed; CI 100% line
> and branch with no `[ExcludeFromCodeCoverage]`; names signed. Kill: any adapter needs logic the
> rule object cannot supply, or the new path allocates where the current one does not → W03 returns
> to redesign. Two-week time-box.

The council verdict that D29 records, verbatim from the
[transcript](../technical-readiness-to-1.0.council-transcript.md):

> **Exit criterion for PR one (all five, or the pilot has not passed):**
> - No adapter contains rule logic: no predicate branch, no message text literal, no code literal; each adapter is a mapping call into the rule object. This is the falsification test for D's objection and is checkable by reading the diff.
> - A cross-layer consistency test, driven by a single `TheoryData`, asserting that Must, Guard, Fluent and DataAnnotations agree with the rule object on validity, code and rendered message for every input.
> - A BenchmarkDotNet project with two benchmarks: the new rule-object path and the current `StringRules.DateOnly`/Must path for the same inputs, with numbers committed to the tracker.
> - CI green on the existing gates: 100% line+branch across the merged report, audit ratchet, dotnet format. No `[ExcludeFromCodeCoverage]`.
> - Owner has signed off every public name in the PR's names table.
>
> **Kill criterion:** if any adapter must carry logic the rule object cannot supply, or the benchmark shows the new path allocating where the current path does not, W03 returns for redesign before PRs two and three, and the owner decides whether the rule object is worth a second attempt. That is a two-week answer to a question the plan has been circling since September.
>
> **Time-box:** two weeks from authorization to merge of PR one. If it is not merged, the work stops and the reason is written into the tracker as the finding.

## Chunks

| Chunk | Scope | Status |
|---|---|---|
| C1 | Inventory the current DateOnly surface, write the ten-line file list and the draft PR description, open the draft PR, update D22 | Done 2026-11-16 |
| C2 | The DateOnly rule object: one executable source of truth, with Core-level tests. Inherits Temporal D-1 | Done 2026-11-16 |
| C3 | Must and Guard adapters, no rule logic in either | Next |
| C4 | FluentValidation and DataAnnotations adapters, no rule logic. DataAnnotations resolves `TimeProvider` through `ValidationContext.GetService` | Not started — the code assertions depend on AO-3 |
| C5 | Cross-layer consistency test: one `TheoryData`, four adapters against the new rule object on validity, failure code and rendered message; expected cases written independently per W01 | Not started — the code assertions depend on AO-3 |
| C6 | BenchmarkDotNet project (TP-BENCH), new path against the current `StringRules.DateOnly` / Must path; results and an allocation comparison committed | Not started |
| C7 | Names table (awaits owner signature), full-PR Fable review of record, exit-criteria evaluation with a pass/kill recommendation (awaits owner call) | Not started |

## C1 inventory (main at `2a7dd20`)

Names and the benchmark baseline come from this surface (D30), not from the September snapshots.

**There is no plain "this string is a valid DateOnly" rule on main, in any layer.** The
string→DateOnly surface is a family of parse-then-compare rules built on one parser. All of it is
compiled for .NET 8 and later only.

| Layer | File | What is there |
|---|---|---|
| Core parser | `src/PineGuard.Core/Utils/StringUtility.DateOnly.cs` | `IsValidStyles` (only `None`, `AllowWhiteSpaces` and its constituent flags pass) and `TryParse`: rejects null, empty and whitespace, then `DateOnly.TryParse` with `CultureInfo.InvariantCulture`. No exact format. No allocation. No clock |
| Core rules | `src/PineGuard.Core/Rules/StringRules.DateOnly.cs` | Four `bool` predicates: `IsInPast`, `IsInFuture`, `IsInRange`, `HasMinimumAge`. Parse, then `DateOnlyRules`. No allocation |
| Codes | `src/PineGuard.Core/Codes/MustCodes.Date.cs` | `const string` codes, including `Date.Format.Invalid` and `Date.Reference.OutOfRange` |
| Must | `src/PineGuard.MustClauses/MustStringDateOnlyClauses.cs` | 24 clauses returning `MustResult<DateOnly>`. They do not call `StringRules.DateOnly`; each repeats the same sequence (styles check, null check, parse, clock, compare) with inline message literals. A `MustResult` is allocated on every call |
| Guard | `src/PineGuard.GuardClauses/GuardStringDateOnlyClauses.cs` | 20 complement clauses, each calling its Must clause and throwing through `GuardFailure.Throw`; the code travels in `Exception.Data` |
| FluentValidation | `src/PineGuard.FluentValidation/FluentStringDateOnlyExtensions.cs` | 24 extensions calling Must. Null is skipped. The error code is a fixed constant per extension, not the Must result's code |
| DataAnnotations | `src/PineGuard.DataAnnotations/StringDateOnlyAttributes.cs` | 21 attributes calling Must. Null is skipped. `ValidationResult` carries no code. `ResolveTimeProvider` already uses `ValidationContext.GetService` |
| Clock | `src/PineGuard.Core/Utils/DateTimeUtility.cs` | `TryGetTodayDate` implements Temporal D-1. Only the Past, Future and MinimumAge rules read it |
| Fixtures | `tests/PineGuard.Testing/Fixtures/StringRulesFixtures.DateOnly.cs`, `tests/PineGuard.Testing/Common/FixedTimeProvider.cs` | Shared scenarios and the fixed clock |

Three findings from the inventory shape the pilot:

1. **The layers disagree today on the failure code for an unparsable string.** Must and Guard report
   `date.format.invalid`; FluentValidation reports the extension's fixed code (for example
   `date.relative.not-past`); DataAnnotations reports no per-result code at all. The consistency
   test will make this visible. See AO-3.
2. **Null handling differs by layer and is currently a branch inside two adapters.** Must fails on
   null; FluentValidation and DataAnnotations turn null into success in adapter code. Under the exit
   criterion that branch is rule logic, so the rule object has to supply the null policy. This is
   the most likely place for the kill criterion to bite and is the first thing C3 and C4 must prove.
3. **The rule sequence is duplicated 24 times in Must.** That duplication is what the rule object
   replaces.

## Design decisions made by Fable

These are implementation-design calls inside the pilot's authorised scope. They are recorded so the
owner can redirect them; none is an owner gate unless listed under Awaiting owner.

| ID | Decision | Reason |
|---|---|---|
| F-1 | The pilot's rule is **string→DateOnly "in the past"**: the rule behind `StringRules.DateOnly.IsInPast`, `Must.Be.PastDateOnly`, `Guard.Against.FutureOrPresentDateOnly`, the FluentValidation `PastDateOnly` extension and `PastDateOnlyStringAttribute`. One rule, not the family of 24 | Main has no plain "valid DateOnly" rule, so a pilot of that would add public API and have no current path to benchmark. "In the past" already exists in all four adapters, exercises parse, format code, the D-1 clock and `TimeProvider` resolution, and is the hard case the council asked for. Converting all 24 is rollout, not a pilot |
| F-2 | The rule object and its supporting types are `internal` to Core | `PineGuard.Core` already grants `InternalsVisibleTo` to the Must and Guard assemblies. Keeping the type internal means the pilot adds no public rule-object name ahead of W22. Whether FluentValidation and DataAnnotations reach it directly or through the Must result is settled in C4 |
| F-3 | Culture stays as it is on main: `CultureInfo.InvariantCulture`, styles `None` or `AllowWhiteSpaces` | D16 leaves culture BLOCKED. The pilot makes no culture decision; it carries the current behaviour unchanged |
| F-5 | Rule-object shape: a static class `StringPastDateOnlyRule` with `Evaluate(value, styles, timeProvider, allowNull, out parsed)` returning the enum `StringPastDateOnlyOutcome`, plus allocation-free lookups by outcome (`IsPassed`, `Code`, `MessageTemplate`, `ParamName`, `Value`). No result struct, no interface, no instance | W03 forbids a runtime rule-object hierarchy and per-call allocation graphs, and D08 (the failure contract) is BLOCKED, so the rule object returns an outcome and lets each adapter project it into its own result type. The lookups are total over `Passed` so an adapter can pass all of them to one result-factory call without a branch |
| F-6 | Null policy is a `bool allowNull` argument to `Evaluate`. Styles are checked before null under either policy | It is the repository's existing word (`ValidationAttributeBase`). It reproduces today's behaviour exactly: Must and Guard fail on null; FluentValidation and DataAnnotations pass null only after the styles check. The rule object owns the branch, so no adapter needs one |
| F-7 | The lookups do not assemble a `MustResult`; each adapter that needs one writes the same mapping expression | Assembling the result inside `PineGuard.Rules` would make Rules depend on the Must result type, against the Utils → Rules → Must layer order. Whether C3 and C4 share one assembly point is settled there |
| F-8 | The failing-parameter names `"styles"` and `"timeProvider"` are literals in the rule object, not `nameof` at the adapter | They have to live with the rule for the adapters to carry no literal. The C5 consistency test pins them |
| F-4 | The current-path benchmark baseline uses only public API that exists on main (`StringRules.DateOnly.IsInPast`, `Must.Be.PastDateOnly`), so the same benchmark source can run against a main checkout and against the branch | The pilot rewires the Must clause, so the "current path" will not exist on the branch by C6. Method to be fixed in C6 |

## Ten-line file list (D22)

This list is also in the description of [PR #66](https://github.com/stevomccormack/PineGuard/pull/66), which is what satisfies D22.

Paths marked `+` are new. The new names are provisional until the names table is signed.

1. `+ src/PineGuard.Core/Rules/StringPastDateOnlyRule.cs` (with `StringPastDateOnlyOutcome.cs` beside it, one type per file) — the rule object: predicate, codes, message templates, null policy
2. `src/PineGuard.Core/Rules/StringRules.DateOnly.cs` — current Core predicate; benchmark baseline. `IsInPast` shares the rule object's primitives, not its sequence (see AO-7)
3. `src/PineGuard.Core/Utils/StringUtility.DateOnly.cs` — the parser the rule object calls; expected unchanged
4. `src/PineGuard.Core/Codes/MustCodes.Date.cs` — existing codes the rule object references; no new code expected
5. `src/PineGuard.MustClauses/MustStringDateOnlyClauses.cs` — `PastDateOnly` becomes a mapping call
6. `src/PineGuard.GuardClauses/GuardStringDateOnlyClauses.cs` — `FutureOrPresentDateOnly` maps through the rule object
7. `src/PineGuard.FluentValidation/FluentStringDateOnlyExtensions.cs` — `PastDateOnly` becomes a mapping call; fixed-code divergence removed
8. `src/PineGuard.DataAnnotations/StringDateOnlyAttributes.cs` — `PastDateOnlyStringAttribute` becomes a mapping call
9. `+ tests/PineGuard.Testing.UnitTests.Rules/` — TP-RULES project holding the cross-layer consistency test under `Conformance/`
10. `+ benchmarks/PineGuard.Benchmarks/` — TP-BENCH BenchmarkDotNet project, registered in `PineGuard.slnx`, outside ordinary `dotnet test`

Supporting changes that follow from these: unit tests and test data beside each changed member in
the five existing unit-test projects, shared scenarios in
`tests/PineGuard.Testing/Fixtures/StringRulesFixtures.DateOnly.cs`, `PineGuard.slnx`, the CI
workflow and `apps/cli/config/baseline.json` only where the two new projects require it, and this
tracker.

## Awaiting owner

A signature counts only if it is the owner's own text on this page or a PR comment by
`stevomccormack`.

| ID | Item | Fable's recommendation | Blocks | Status |
|---|---|---|---|---|
| AO-1 | Sign the names table | Not drafted yet; drafted in C7 with rationale | Exit criterion 5 | Not yet due |
| AO-2 | Pass/kill call | Made from the C7 exit-criteria evaluation | Pilot verdict | Not yet due |
| AO-3 | Failure code on the FluentValidation and DataAnnotations projections (touches D08 and D09, both not settled) | FluentValidation reports the rule object's code for the outcome that occurred, so an unparsable string gives `date.format.invalid` in every layer that can carry a code. DataAnnotations keeps its current contract: `ValidationResult` carries no code, the attribute's `Code` property is the rule's primary code, and the consistency test records that gap as an intentional projection difference with this reason (W05 forbids forcing DataAnnotations into a universal-code contract). No new public API | The code assertions in C4 and C5. C2 and C3 do not depend on it | Awaiting owner |
| AO-6 | Standing specs say Core rules own no user-facing messages and Core tests assert validity only (`docs/ai/specs/core/project.md` §1, the Core unit-test spec §3, `docs/ai/rules/global.md` "Must owns canonical messages"). The rule object holds the codes and message templates, and its Core tests assert them | Accept the departure for the pilot: D29 is the later, authorised decision and it puts code and message in the rule object. The specs are left unedited. If the pilot passes, amend them as part of the rollout decision; if it is killed, nothing needs undoing | Nothing in the pilot. It is a spec change the owner must make or decline at the pass/kill call | Awaiting owner |
| AO-7 | Two statements of "parse, derive today, compare" remain in Core: the rule object (exclusive only) and the public `StringRules.DateOnly.IsInPast(value, inclusion, …)`, which also collapses an out-of-range reference date into `false`. They share the parser, `TryGetTodayDate` and the comparison, but not the sequence | Leave `IsInPast` alone in the pilot and record the duplication as a finding for the pass/kill evaluation. Folding it in needs an `inclusion` parameter on the rule object and a decision on the public predicate's future; both belong to rollout, and adding the parameter now would be speculative | Nothing — C3 proceeds | Awaiting owner (at C7) |
| AO-8 | Owner's question on PR #66 (2026-11-16): why the rule-object tests have no valid / invalid / boundary-edge split | The boundary cases exist (today, yesterday and tomorrow in three zones; year 0001 and 9999; the two failing clock projections; empty, white space, white space inside a component) but are not labelled. Two reasons: the Core test spec prescribes a single `Cases` rollup per operation group, and the fixture holds flat fields instead of the `ValidScenarios` / `ValidEdgeScenarios` / `InvalidScenarios` / `InvalidEdgeScenarios` arrays of `fixture.md` §2, because `RuleScenario<T>` carries a `bool` and the rule returns a six-valued outcome. Recommendation: group the fixture into the four scenario arrays with a rollup, keeping the expected outcome in the test data (W01), so the V / I / BE classification is visible and C5 can reuse it. It is a test-only change of about one file pair | Nothing. Best done before C5, which builds on the fixture | Awaiting owner |
| AO-4 | Pilot rule choice (F-1) | Keep "in the past". Listed for visibility because it fixes what the pilot proves; it is a design call, not a register item | Nothing — C2 proceeds on F-1 unless the owner redirects | For information |
| AO-5 | Time-box start | Count from the first code commit, as the scheduled task says. If the owner prefers the council's wording, the box ends 2026-11-23 | Nothing | For information |

## Fable review verdict log

| Date | Commit | Chunk | Reviewer | Verdict | Findings |
|---|---|---|---|---|---|
| 2026-11-16 | `f676e18` | C2 | Fable (fresh subagent) | Findings, no blockers | (1) should-fix: no success projection, so adapters would write `outcome == Passed` — fixed in `0eea9b2` with `IsPassed`; the repeated result assembly is recorded as F-7. (2) should-fix: the file list claimed `IsInPast` shares the predicate — corrected, raised as AO-7. (3) nit: parameter-name literals — accepted, F-8. (4) nit: the styles doc claim had no test — fixed in `0eea9b2`; the sentence was also corrected (white space inside a date component is rejected under every style). (5) nit: third private copy of the ±14h test zones — deferred to C5, which needs them shared. (6) nit: noun-named lookups and the `Null` member — carried to the C7 names table |
| 2026-11-16 | `f676e18` + `0eea9b2` | C2 | Fable (second fresh subagent) | **ship** | None. Notes carried forward, not findings: C4 needs `InternalsVisibleTo` from Core to FluentValidation and DataAnnotations, and the DataAnnotations `IsExternalInit` polyfill will then collide on netstandard2.1 and must go in the same change; the fixed primary code is reachable only as `Code(Passed)`; the no-clock UTC rule is pinned by the `TryGetTodayDate` tests, not at rule level |
| 2026-11-16 | `f676e18` | C2 | Fable (third fresh subagent, run 4, started before run 3's fix was pushed) | Findings, no blockers | (1) should-fix: the file list omitted the outcome and fixture files — already corrected by run 3. (2) should-fix: the spec departure on message ownership was unrecorded — already raised by run 3 as AO-6. (3) nit: the null-provider half of D-1 was not pinned at rule level; a host-zone leak in `Evaluate` survived every test — fixed in `ebc8e42`. (4) nit: `Unparsable` against the repository's spelling — renamed to `Unparseable` in `ebc8e42`. (5) nit: third copy of the ±14h zones — deferred to C5, as before. (6) nit: mixed name-string and cast conventions for expected outcomes — fixed in `ebc8e42`. (7) nit: raw inputs repeated in the `Value` rows — fixed in `ebc8e42` |
| 2026-11-16 | `ebc8e42` + merge `c44e180` (diff from `5a9dd87`) | C2 | Fable (fourth fresh subagent) | **ship** | None. Optional nits, not applied: the timezone job's two-name filter fails on "no tests" only if both names vanish (split into two steps if wanted); the test comment's "at every instant" excludes a run that straddles UTC midnight |

## Run log

| Date | Run | What was done | Models | Review verdicts | CI | Days remaining | Awaiting owner | Next |
|---|---|---|---|---|---|---|---|---|
| 2026-11-16 | 1 | Stopped at the environment check: the egress proxy refused `builds.dotnet.microsoft.com`. Nothing written | Fable | — | — | Clock not started | Allow the host | Re-run |
| 2026-11-16 | 2 | Environment and sync checks passed. Created `feature/dateonly-pilot` from main `2a7dd20`. Created this tracker and recorded the authorisation in the plan header and D19. C1: inventory, ten-line file list, draft PR #66 opened, D22 updated. Full local gate run on the source tree, which is identical to main: Release build 0 warnings; 47,007 passing executions (23,491 net8.0; 23,516 net10.0) plus the four timezone executions; 100% line and branch (8,986 of 8,986); format clean; audit gate 0 findings | Fable: orchestration, decisions, tracker. Opus: three read digests (plan documents, repository rules, DateOnly surface) | None — no code commit | Reported in the PR #66 description; a documentation-only change does not trigger the build, test, format or audit jobs | 14 of 14 (clock not started) | AO-3; AO-4 and AO-5 for information | C2 |
| 2026-11-16 | 3 | Environment and sync checks passed (.NET 10.0.404 SDK, 8.0.34 runtimes installed; main unchanged at `2a7dd20`). C2: the rule object `StringPastDateOnlyRule` with `StringPastDateOnlyOutcome`, Core tests and fixture (`f676e18`); review follow-up adding `IsPassed` and the white-space cases (`0eea9b2`). Design decisions F-5 to F-8 recorded; AO-6 and AO-7 raised. No kill-criterion signal: the rule object supplies everything the four adapters need, and its only allocation is the styles box on the invalid-styles path, which the current path also takes. Full local gate run by Fable before each commit: Release build 0 warnings; 47,181 passing executions (23,578 net8.0; 23,603 net10.0) plus the four timezone executions; 100% line (22,982 of 22,982) and branch (9,030 of 9,030); format clean; no `[ExcludeFromCodeCoverage]`; audit gate 0 findings | Fable: orchestration, design decisions, gate, tracker. Opus: one read digest, the implementation and the fix (effort xhigh). Fable (two fresh subagents): the code reviews | `f676e18`: findings, no blockers. `f676e18` + `0eea9b2`: ship | Green on `f676e18` (run 43836164687) and `0eea9b2` (run 43860147198) | 14 of 14 (box runs 2026-11-16 to 2026-11-30) | AO-3 (blocks the code assertions in C4 and C5); AO-6; AO-7; AO-4 and AO-5 for information | C3 |
| 2026-11-16 | 4 | Ran concurrently with run 3 and found C2 already committed. Environment and sync checks passed. Reviewed `f676e18` independently, fixed the open findings (`ebc8e42`: rule-level null-provider UTC test added to the `core-null-provider-timezone` job, `Unparseable` spelling, one expected-outcome convention, fixture-sourced `Value` rows), then merged run 3's `0eea9b2` and `5a9dd87`, which had reached the remote first (`c44e180`; no force-push). No behaviour change and no kill-criterion signal. Full local gate run by Fable before `ebc8e42` and again on the merged tree: Release build 0 warnings; 47,209 passing executions (23,592 net8.0; 23,617 net10.0) plus eight timezone executions; 100% line (22,982 of 22,982) and branch (9,030 of 9,030); format clean; no `[ExcludeFromCodeCoverage]`; audit gate 0 findings. No new chunk started | Fable: orchestration, merge, gate, tracker. Opus (effort xhigh): the fix. Fable (two fresh subagents): the code reviews | `f676e18`: findings, no blockers. `ebc8e42` + `c44e180`: ship | See the PR #66 description for the run on `c44e180` | 14 of 14 (box runs 2026-11-16 to 2026-11-30) | AO-3 (blocks the code assertions in C4 and C5); AO-6; AO-7; AO-8; two scheduled runs overlapped on this branch today — if that was not intended, space the schedule; AO-4 and AO-5 for information | C3 |
