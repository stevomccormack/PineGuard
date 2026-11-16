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
| string→DateOnly | In PR (draft) — C1 done, C2 next | [#66](https://github.com/stevomccormack/PineGuard/pull/66) | No — table not drafted yet (C7) | None yet (C6) | Open |
| Email | Parked — runs only if the string→DateOnly pilot passes | — | — | — | — |
| Bounded numeric | Parked — runs only if the string→DateOnly pilot passes | — | — | — | — |

## Time-box

| Item | Value |
|---|---|
| Start (first code commit) | Not started — no code commit yet |
| End (start + 14 days) | Set when the first code commit lands |
| Days remaining | 14 of 14 (clock not started) |

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
| C2 | The DateOnly rule object: one executable source of truth, with Core-level tests. Inherits Temporal D-1 | Next |
| C3 | Must and Guard adapters, no rule logic in either | Not started |
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
| Core parser | `src/PineGuard.Core/Utils/StringUtility.DateOnly.cs` | `IsValidStyles` (only `None` and `AllowWhiteSpaces` pass) and `TryParse`: rejects null, empty and whitespace, then `DateOnly.TryParse` with `CultureInfo.InvariantCulture`. No exact format. No allocation. No clock |
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
| F-4 | The current-path benchmark baseline uses only public API that exists on main (`StringRules.DateOnly.IsInPast`, `Must.Be.PastDateOnly`), so the same benchmark source can run against a main checkout and against the branch | The pilot rewires the Must clause, so the "current path" will not exist on the branch by C6. Method to be fixed in C6 |

## Ten-line file list (D22)

This list is also in the description of [PR #66](https://github.com/stevomccormack/PineGuard/pull/66), which is what satisfies D22.

Paths marked `+` are new. The new names are provisional until the names table is signed.

1. `+ src/PineGuard.Core/Rules/StringPastDateOnlyRule.cs` — the rule object: predicate, codes, message templates, null policy
2. `src/PineGuard.Core/Rules/StringRules.DateOnly.cs` — current Core predicate; benchmark baseline; `IsInPast` shares the rule object's predicate
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
| AO-4 | Pilot rule choice (F-1) | Keep "in the past". Listed for visibility because it fixes what the pilot proves; it is a design call, not a register item | Nothing — C2 proceeds on F-1 unless the owner redirects | For information |
| AO-5 | Time-box start | Count from the first code commit, as the scheduled task says. If the owner prefers the council's wording, the box ends 2026-11-23 | Nothing | For information |

## Fable review verdict log

| Date | Commit | Chunk | Reviewer | Verdict | Findings |
|---|---|---|---|---|---|
| — | — | — | — | — | No code commit yet. The tracker and C1 commit is documentation only |

## Run log

| Date | Run | What was done | Models | Review verdicts | CI | Days remaining | Awaiting owner | Next |
|---|---|---|---|---|---|---|---|---|
| 2026-11-16 | 1 | Stopped at the environment check: the egress proxy refused `builds.dotnet.microsoft.com`. Nothing written | Fable | — | — | Clock not started | Allow the host | Re-run |
| 2026-11-16 | 2 | Environment and sync checks passed. Created `feature/dateonly-pilot` from main `2a7dd20`. Created this tracker and recorded the authorisation in the plan header and D19. C1: inventory, ten-line file list, draft PR #66 opened, D22 updated. Full local gate run on the source tree, which is identical to main: Release build 0 warnings; 47,007 passing executions (23,491 net8.0; 23,516 net10.0) plus the four timezone executions; 100% line and branch (8,986 of 8,986); format clean; audit gate 0 findings | Fable: orchestration, decisions, tracker. Opus: three read digests (plan documents, repository rules, DateOnly surface) | None — no code commit | Reported in the PR #66 description; a documentation-only change does not trigger the build, test, format or audit jobs | 14 of 14 (clock not started) | AO-3; AO-4 and AO-5 for information | C2 |
