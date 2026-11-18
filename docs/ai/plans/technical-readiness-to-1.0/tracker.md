# Technical readiness to 1.0 — pilot tracker

This page is the single source of truth for the state of the pilots that the
[council verdict](../technical-readiness-to-1.0.council-transcript.md) of 2026-11-09 placed ahead of
every wave ([decision register](references/decision-register.md) D29). It replaces the closure
ledgers and the planning-era review protocol for the pilot. The [plan](plan.md) says what the
programme is; this page says where the pilot stands.

## Read before every run (owner instruction, 2026-11-16)

Every run reads this section first and does what it says before any code or test is written. It
exists because runs 3 and 4 wrote tests from a summary of the testing docs, not from the docs, and
departed from them.

1. **Read the standard itself, in full, not a digest.** The orchestrator and every subagent that
   writes or reviews code or tests reads these files directly:
   - `docs/ai/rules/global.md` (including "Precedent and Consistency"), `docs/ai/rules/testing.md`,
     `docs/ai/rules/fixture-conventions.md`
   - `docs/ai/specs/testing/unit-test.md`, `docs/ai/specs/testing/fixture.md`,
     `docs/ai/specs/testing/gold-standard.md`
   - the layer addenda for every layer the chunk touches: `docs/ai/specs/<layer>/unit-test.md` and
     the layer's coding spec
2. **Name the precedent.** Every implementation packet names the sibling source file, fixture,
   test-data file and test file to mirror, and says "match it exactly".
3. **No deviation without the owner.** If a doc or precedent cannot be followed, stop that item
   before writing it and record it under [Awaiting owner](#awaiting-owner) with the doc section, the
   reason and the smallest alternative. Where a doc and an existing file disagree, follow the doc and
   list the file for the owner; do not change it in the pilot.
4. **Review against the docs.** Every reviewer is told to compare the diff with the docs and the named
   precedent section by section, and to return any unrecorded departure as a blocker.
5. **Checklist for every test change:** fixture scenario arrays per `fixture.md` §2; `Cases` shape
   per the layer addendum; `Xxx_BehavesAsExpected` naming and group order per `unit-test.md` §4.4–§4.5;
   structural comments only per `fixture.md` §11.1; time-relative values per the Core addendum §6;
   `gold-standard.md` index updated per `unit-test.md` §10.5.
6. **Gold standard is the minimum; nothing is left behind** (`global.md` → "Gold Standard Is the
   Minimum"). A chunk is not done while anything it touched is stale, incorrect or half finished: no
   removed assertion without a replacement, no index or doc left untrue, no "later". Anything that
   cannot be finished is stopped and put to the owner as open work, and the run report says so
   first.
7. **Standing packet header.** Subagents see only the packet they are given. Paste this block, word
   for word, at the top of every implementation and review packet:

   > OWNER STANDARD. Gold standard is the minimum. Before writing or judging anything, read in full:
   > `docs/ai/rules/global.md`, `docs/ai/rules/testing.md`, `docs/ai/rules/fixture-conventions.md`,
   > `docs/ai/specs/testing/unit-test.md`, `docs/ai/specs/testing/fixture.md`,
   > `docs/ai/specs/testing/gold-standard.md` and the layer specs for every layer you touch. Match
   > the docs and the nearest sibling file exactly. Never deviate on your own judgment: if a doc or
   > precedent cannot be followed, stop that item and report it. Leave nothing stale, incorrect or
   > half finished: update every doc, index and count your change makes untrue; never remove an
   > assertion without an equal or stronger replacement. Report what is not done before what is.
8. **One run at a time.** Claim the unit in the [Active runs](#active-runs) table before any other
   work, as that section and `docs/ai/rules/coordination.md` ("Sessions in Separate Containers")
   describe. If another session holds a live claim, report and stop.

## Active runs

Owner instruction, 2026-11-18. Sessions run in separate containers and cannot see each other's local
status board, so the claim lives here, on the unit's working branch. One row per unit. The full rule
is in [`docs/ai/rules/coordination.md`](../../rules/coordination.md) ("Sessions in Separate
Containers").

1. Before any other work, fetch and read this table at the remote tip of the working branch.
2. A row with a heartbeat under 3 hours old that is not yours is a live claim: report and stop.
3. Otherwise write your session, the chunk and the UTC time into the row, commit only that change
   and push it first. A rejected push means another run claimed first: abandon the commit and stop.
4. Refresh the heartbeat with every push, and at least every 60 minutes.
5. Before every later push, fetch and confirm the row is still yours and the remote tip is your own
   last push. If not, stop pushing and report what is unpushed.
6. Clear the row in your final commit, including after a failure. A heartbeat over 3 hours old is
   stale: take the row over and name the replaced session in the Run log.

| Unit | Working branch | Session | Chunk | Started (UTC) | Heartbeat (UTC) |
|---|---|---|---|---|---|
| string→DateOnly pilot | `feature/dateonly-pilot` | — | — | — | — |

## Authorisation

The owner authorised exactly one unit on 2026-11-09: the string→DateOnly pilot, as a single PR, per
D29. The authorisation was given through the scheduled-task instruction that runs this pilot and is
recorded here, in the [plan header](plan.md) and in D19, as D19 requires. It covers the pilot only.
Email, bounded numeric, W22, Temporal Phase 2 and every wave stay parked.

## Pilots

| Pilot | Status | PR | Names signed | Benchmark numbers | Verdict |
|---|---|---|---|---|---|
| string→DateOnly | In PR (draft) — C1 and C2 done, C3 blocked on AO-11. C2's tests were reworked on 2026-11-16/17 to match the testing docs; departures R1–R15 await the owner | [#66](https://github.com/stevomccormack/PineGuard/pull/66) | No — table not drafted yet (C7) | None yet (C6) | Open |
| Email | Parked — runs only if the string→DateOnly pilot passes | — | — | — | — |
| Bounded numeric | Parked — runs only if the string→DateOnly pilot passes | — | — | — | — |

## Time-box

| Item | Value |
|---|---|
| Start (first code commit) | 2026-11-16 (`f676e18`) |
| End (start + 14 days) | 2026-11-30 |
| Days remaining | 12 of 14 on 2026-11-18 |

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
| C3 | Must and Guard adapters, no rule logic in either | Blocked — awaiting owner on AO-11 |
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
| F-9 | Guard keeps mapping through the Must result (`Must.Be.PastDateOnly` then `GuardFailure.Throw(result, …)`); it does not call the rule object itself | It is the precedent for every Guard clause, `global.md` ("Guard calls Must. Never duplicate logic.") and the `must-codes` rule (e) require it, and a direct call would repeat the result assembly F-7 warns about. The rule object still supplies everything Guard reports, through the Must result |

## Standing rule: precedent and consistency

Owner instruction, 2026-11-16, given in the run 4 session and written into
[`docs/ai/rules/global.md`](../../rules/global.md) ("Precedent and Consistency"): match the nearest
existing precedent and its spec exactly; never deviate on your own judgment; a deviation is very
exceptional and is put to the owner, with the reason, before it is written; reviewers treat an
unrecorded deviation as a blocker. Every implementation and review packet in this pilot carries it.

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
| AO-8 | Owner's question (2026-11-16) on the missing valid / invalid / edge split, and the instruction never to deviate from the docs or precedent | Done in `373a0c2`, `c44fa4d`, `1e1417c`, `e668263` and `441c88b`: the fixture has `ValidScenarios`, `InvalidScenarios` and `AllScenarios` (two arrays, per `fixture.md` §2 and the sibling `DateOnlyIsInPast`); each group has the standard `Cases` and `Xxx_BehavesAsExpected`; the detail rows are `AdHocCases`; no edge arrays and no `EdgeCases`. `e668263` restores the zero-clock-read assertions and `441c88b` restores the count on all 39 scenarios. What could not be matched is listed below under [Recorded departures](#recorded-departures-in-the-pilot-tests) | Nothing | Done; departures await the owner |
| AO-9 | **CI is red on this PR because of `main`, not the pilot.** `main` commit `dd67c15` ("docs(etc): add feature specification skill plan to .etc/shared", 2026-11-17) added a feature-specification skill plan document under the `.etc` shared folder, with references that do not resolve, so the merge-blocking `doc-links` audit fails. The PR's CI runs on the merge with `main`, so its `audit` job fails from `77b5f98` onward; `main`'s own CI run 44015036304 fails the same way. The pilot branch alone passes the gate audit with 0 findings, and CI was green on `441c88b` before `main` moved | **Resolved 2026-11-18.** The owner instructed in session "Fix everything - lets go!!!". The plan document is left as written; it is allowlisted in `apps/cli/config/exceptions.json` with the reason recorded there (`main` `fc045b3`). `main` is merged into this branch and the audit gate passes with 0 findings | A green PR | Resolved 2026-11-18 (owner instruction in session; `fc045b3` on `main`) |
| AO-4 | Pilot rule choice (F-1) | Keep "in the past". Listed for visibility because it fixes what the pilot proves; it is a design call, not a register item | Nothing — C2 proceeds on F-1 unless the owner redirects | For information |
| AO-10 | **Scheduled runs stop at approval prompts.** On `main`, `.claude/settings.json` lists `git push`, `git merge` and `gh api` under `ask`, and every run needs all three. The fix is written but unmerged, on `claude/serene-galileo-1wtxvh` (last pushed 2026-11-17 08:41 UTC) | **Resolved 2026-11-18.** The owner instructed in session "Push to main then!!!" and "Fix everything - lets go!!!". `main` `02e8afd` moves `git push origin`, `git merge`, `git checkout`, `git cherry-pick`, `git rm` and `gh api` to `allow`, allows the MCP servers, `Skill`, `ToolSearch` and `PushNotification`, and denies pushing to `main` and deleting remote branches. `main` `8d31365` merges the hook rework from `claude/serene-galileo-1wtxvh` (hook tests: 114 passing). Still open and the owner's to do: install the .NET SDK and runtimes in the cloud environment's setup script. Known limit: the deny rule does not catch `git push origin <branch>:main`; a GitHub branch ruleset is the reliable guard | Unattended runs. Nothing in the pilot's content | Resolved 2026-11-18 (owner instruction in session; `02e8afd`, `8d31365` on `main`). Environment setup script: owner |
| AO-11 | **C3 cannot be written without departing from the Must spec.** The exit criterion wants the Must clause to be one mapping call: `FromBool(IsPassed(outcome), Code(outcome), MessageTemplate(outcome), …)`. `docs/ai/specs/must-clauses/project.md` requires the opposite inside every clause: "Parameter validation order" (guard the secondary parameters, declare `const string messageTemplate`, validate, `FromBool`), "One clause, one code" (every `Fail`/`FromBool` call passes exactly one `MustCodes` constant, never zero) and "Parse once, validate typed, return typed". The `must-codes` audit rule enforces the second: a call with no `MustCodes` constant is finding (a), and its baseline has no entry for this file. Keeping the constant in the clause is not a way out: it is the "code literal" the exit criterion forbids | Accept the departure for `Must.Be.PastDateOnly` only, on the same footing as AO-6: D29 is the later, authorised decision. Add one `must-codes` entry to `apps/cli/config/exceptions.json` naming this clause, with the reason. Leave the spec unedited until the pass/kill call; if the pilot passes, the spec and the audit rule are amended as part of rollout; if it is killed, the exception is removed with the code. Guard needs no departure: `Guard.Against.FutureOrPresentDateOnly` already maps through the Must result, as every Guard clause and the Guard spec require (F-9) | **C3**, and through it C4 to C7 | Awaiting owner |
| AO-12 | **`main` builds with 3 package warnings.** `System.Security.Cryptography.Pkcs` 11.0.0 (`main` `5c1c09b`, #75) and its dependencies `Microsoft.Bcl.Cryptography` 11.0.0 and `System.Formats.Asn1` 11.0.0 each warn that they do not support `net8.0`. CI does not fail on them: its warning check counts compiler (CS) warnings only. They reach this branch through the merge with `main` and are not caused by the pilot | **Resolved 2026-11-18.** `main` `8b24472` (#81) returned `System.Security.Cryptography.Pkcs` and `Microsoft.Bcl.TimeProvider` to 10.0.15. `main` is merged into this branch (`dab07ae`) and the Release build reports 0 warnings, package warnings included | Nothing | Resolved 2026-11-18 (`8b24472` on `main`) |
| AO-13 | **`main` now needs the .NET 11 SDK.** `main` `8b24472` (#81) added `net11.0` as a target framework. Step 0 of the scheduled-task instruction installs the .NET 10 SDK and the .NET 8 runtimes only, and its gate wording says "both TFMs"; the test projects now build for three (`net8.0`, `net10.0`, `net11.0`). Run 6 installed SDK 11.0.100 with the same script (`dotnet-install.sh --channel 11.0`) and ran the gate on all three | Add `--channel 11.0` to step 0 of the scheduled-task instruction and to the cloud environment setup script (the open half of AO-10), and change "both TFMs" to "every target framework". Until then each run installs it itself, as run 6 did | Nothing while `builds.dotnet.microsoft.com` stays reachable | Awaiting owner |
| AO-5 | Time-box start | Count from the first code commit, as the scheduled task says. If the owner prefers the council's wording, the box ends 2026-11-23 | Nothing | For information |

## Recorded departures in the pilot tests

Each row is a place where the rule-object tests could not match the testing docs or the nearest
sibling exactly. All await the owner; none blocks C3. R1 to R14 were found by review after the tests were written, which is the failure the read-before-every-run section now exists to prevent.

| ID | Departure | Why | Fable's recommendation |
|---|---|---|---|
| R1 | `Evaluate_NullProviderUsesUtcDate` and its `EvaluateNullProvider` group: the name is not `Xxx_BehavesAsExpected`, it has explanatory comments, it reads `UtcNow` in the test method, and it discards with `out var _` | Core addendum §6 says time-relative values use a `get` accessor in the test data; the tests it names now pin the clock instead. Following §6 loses the guard against midnight passing mid-test; following the files cannot detect a host-zone leak. `out var _` is forced by the compiler because the parameter is itself named `_` | Keep as is, the same recorded exception as `DateTimeUtilityTests.TryGetTodayDate_NullProviderUsesUtcDate`, which it copies |
| R2 | The lookups' `InvalidCases` expect `ArgumentOutOfRangeException` | The Core addendum's only `InvalidCases` exception is `ThrowIfNull`. The lookups throw on an undefined outcome | Keep; decide with AO-6 |
| R3 | `_ThrowsAsExpected` method names | The spec's canonical example (`unit-test.md` §8.3) uses this name; 18 sibling Core Rules test methods (16 `_Throws_WhenXxx` and 2 bare `_Throws`) use `_Throws` or `_Throws_WhenXxx` | Keep the spec form. The sibling files are the ones out of line; not touched in the pilot |
| R4 | Core tests assert codes, message templates and parameter names | Core addendum §3 says no message or parameter-name testing in Core. Follows from the rule-object design (D29). Same item as AO-6 | Decide at the pass/kill call |
| R5 | `AdHocCases` use a group-local `Case` record and mirror the scenarios one to one | `RuleCase` carries only a `bool`; the rule returns a six-valued outcome and a parsed date. `unit-test.md` §4.2 allows a group-local record, the Core addendum says no custom records, and every existing `AdHocCases` uses `RuleCase` | Keep; settle the documented shape for outcome-returning rules at the pass/kill call |
| R6 | The outcome enum is carried as `(int)` in the test-data records and cast back in the tests | The enum is internal and test-data records are public. No sibling does this | Keep while the type is internal (F-2); it disappears if the names table makes the outcome public |
| R7 | The fixture constructs its own ±14-hour zones and failing clock providers | Other fixtures only reference shared providers; `DateTimeRulesTestData` keeps the same providers as test-data fields. Moving them out would drop seven scenarios from `AllScenarios` | Promote the ±14-hour zones to `TestTimeZones` when C5 needs them shared; until then keep |
| R8 | The fixture has an `EvaluateSystemClock` group | It is not a method name, and no other fixture has a system-clock group; the sibling keeps `XxxSystemClock` in the test data only | Keep; the group exists so the scenario arrays stay typed |
| R9 | The Act line of the two standard validity tests compares the outcome with `Passed` | `AssertResult` takes a `bool`; every sibling Act is a bare call | Keep; same root as R5 |
| R10 | The clock-read test is the group `EvaluateClockReads` with a group-local `ClockReadCase` record and the test `Evaluate_ReadsTheClockAsExpected` | It mirrors the Must-layer test for this same rule (`MustStringDateOnlyClausesTestData.PastDateOnlyClockReads`), the only existing shape that asserts zero reads as well as one. `unit-test.md` §4.4–§4.5 want a record named `Case` and a method named `Xxx_BehavesAsExpected`; the Must-layer names are a departure the Temporal plan already tracks. The dataset has one row for each of the 39 pinned-clock scenarios, as the count had before `1e1417c`: 22 expect one read (the value parsed) and 17 expect none (invalid styles, null under either null policy, unparseable). A mutation that reads the clock first, before the styles check, fails every zero-read row; each later position fails at least one | Keep, and rule on the name together with the Must-layer members it copies, so both layers change or stay as one |
| R11 | The six `AheadOfUtc*` / `BehindUtc*` fixture fields keep literal dates | Every `*AcrossTimeZones` sibling writes these as literals; only the `FixedTimeProvider.Default` fields are derived | Keep; it matches the siblings |
| R12 | The private field is `PinnedToday`, not `Today`, and it carries a three-line explanatory comment | The fixture already has a public scenario field `Today`; `DateOnlyRulesTestData` and `StringRulesDateOnlyTestData` use `PinnedToday` for the same expression. The comment is the sibling's text word for word (`DateOnlyRulesTestData`; the same style is in `StringRulesFixtures.DateOnly`), against `fixture.md` §11.1, which allows structural comments only | Keep both: the field matches its siblings. Whether the sibling comments should go is the owner's call and applies at once to every file that carries one (five: the pilot fixture, `DateOnlyRulesTestData`, `StringRulesDateOnlyTestData`, `FluentStringDateOnlyExtensionsTestData` and `StringRulesFixtures.DateOnly`) |
| R13 | `EvaluateClockReads` sits after `EvaluateNullProvider`, with its test in the same position | `unit-test.md` §4.5 requires the tests to follow the test-data group order; `DateTimeUtilityTestData` places its clock-read group the same way | Keep |
| R14 | `gold-standard.md`: the "Coverage Verification" section, the `last_verified` footer and the version were not re-dated; the index lists six of the fifteen test projects | Re-dating needs the per-scope coverage script run. All six listed rows were stale before the pilot and were recounted with the file's own method in `c44fa4d`; the Core operation-group count was corrected again, 701 to 702, in `e668263`, after `1e1417c` added a group without updating it | Owner to decide whether to re-date now or at the pass/kill call, and whether the other nine projects belong in the index |
| R15 | The full audit (`pineguard audit test-structure`, a rule the CI gate does not run) reports four findings on `StringPastDateOnlyRuleTests.cs` that are not in `apps/cli/config/baseline.json`: no `EvaluateNullProvider_BehavesAsExpected`, no `EvaluateClockReads_BehavesAsExpected`, and `Evaluate_AdHoc_BehavesAsExpected` and `EvaluateSystemClock_AdHoc_BehavesAsExpected` with no matching group | They are the machine's view of R1, R10 and R5. The repository carries 142 other findings of this rule outside the baseline. The `_AdHoc_` and clock-read shapes these tests copy (`CollectionRulesTests`, `IdentifierRulesTests`, the Must `…ClockReads` groups, `DateTimeUtilityTests.TryGetTodayDateNullProvider` and `OneReadBehavior`) are findings of the same rule, but theirs are already accepted in the baseline. The pilot's four were deliberately not added: accepting findings is the owner's decision | Decide with R1, R5 and R10. "Audit 0 findings" in this tracker always means the merge-blocking gate |

Three places where existing files, not the pilot, differ from the docs (reported, not changed):
`DateTimeUtilityTests.TryGetTodayDate_NullProviderUsesUtcDate` (name and comments), the 18
`_Throws` / `_Throws_WhenXxx` methods in the Core Rules tests, and `DateTimeRulesTests`, where
`HasMinimumAge_ReadsTheClockExactlyOnce` is out of order against its test data.

## Fable review verdict log

| Date | Commit | Chunk | Reviewer | Verdict | Findings |
|---|---|---|---|---|---|
| 2026-11-16 | `f676e18` | C2 | Fable (fresh subagent) | Findings, no blockers | (1) should-fix: no success projection, so adapters would write `outcome == Passed` — fixed in `0eea9b2` with `IsPassed`; the repeated result assembly is recorded as F-7. (2) should-fix: the file list claimed `IsInPast` shares the predicate — corrected, raised as AO-7. (3) nit: parameter-name literals — accepted, F-8. (4) nit: the styles doc claim had no test — fixed in `0eea9b2`; the sentence was also corrected (white space inside a date component is rejected under every style). (5) nit: third private copy of the ±14h test zones — deferred to C5, which needs them shared. (6) nit: noun-named lookups and the `Null` member — carried to the C7 names table |
| 2026-11-16 | `f676e18` + `0eea9b2` | C2 | Fable (second fresh subagent) | **ship** | None. Notes carried forward, not findings: C4 needs `InternalsVisibleTo` from Core to FluentValidation and DataAnnotations, and the DataAnnotations `IsExternalInit` polyfill will then collide on netstandard2.1 and must go in the same change; the fixed primary code is reachable only as `Code(Passed)`; the no-clock UTC rule is pinned by the `TryGetTodayDate` tests, not at rule level |
| 2026-11-16 | `f676e18` | C2 | Fable (third fresh subagent, run 4, started before run 3's fix was pushed) | Findings, no blockers | (1) should-fix: the file list omitted the outcome and fixture files — already corrected by run 3. (2) should-fix: the spec departure on message ownership was unrecorded — already raised by run 3 as AO-6. (3) nit: the null-provider half of D-1 was not pinned at rule level; a host-zone leak in `Evaluate` survived every test — fixed in `ebc8e42`. (4) nit: `Unparsable` against the repository's spelling — renamed to `Unparseable` in `ebc8e42`. (5) nit: third copy of the ±14h zones — deferred to C5, as before. (6) nit: mixed name-string and cast conventions for expected outcomes — fixed in `ebc8e42`. (7) nit: raw inputs repeated in the `Value` rows — fixed in `ebc8e42` |
| 2026-11-16 | `ebc8e42` + merge `c44e180` (diff from `5a9dd87`) | C2 | Fable (fourth fresh subagent) | **ship** | None. Optional nits, not applied: the timezone job's two-name filter fails on "no tests" only if both names vanish (split into two steps if wanted); the test comment's "at every instant" excludes a run that straddles UTC midnight |
| 2026-11-16 | `373a0c2` | C2 tests | Fable (fresh subagent, docs-conformance brief) | Findings: 1 blocker | (1) blocker: an `Outcome` type alias no sibling has — removed in `c44fa4d`. (2) should-fix: only the Core row of `gold-standard.md` recounted — all six recounted in `c44fa4d`. (3) fixture-built clock providers — recorded, R7. (4) `EvaluateSystemClock` fixture group — recorded, R8. (5) two members in one Act — fixed in `c44fa4d`; the comparison that replaced it is R9. (6) cast and extract in one expression — fixed in `c44fa4d`. `IsPassed` moved to `RuleCase` in `c44fa4d` |
| 2026-11-16 | `c44fa4d` | C2 tests | Fable (fresh subagent) | Findings | (1) far-past system-clock value equalled the default date — fixed in `1e1417c`, value settled in `e668263`. (2) clock-read count folded into `AdHocCases` — moved to its own group in `1e1417c`. (3) clock-relative fixture inputs written as literals — derived in `1e1417c`; the zone fields stay literal, R11. (4) comparison in the Act line — recorded, R9 |
| 2026-11-17 | `1e1417c` | C2 tests | Fable (fresh subagent) | Findings: 1 blocker | (1) blocker: `out var _` outside R1 — removed with the reshaped test in `e668263`. (2) `Value.Cases` expectations not independent literals — fixed in `e668263`. (3) far-past value not the nearest sibling's — fixed in `e668263`. Orchestrator finding, not the reviewer's: `1e1417c` had removed the zero-clock-read assertions — restored in `e668263` and `441c88b` |
| 2026-11-17 | `e668263` | C2 tests | Fable (fresh subagent) | Findings: test structure clean; 3 blockers, all tracker text | (1) fixture comment not covered by a row — R12 extended. (2) sibling throws-method count 16, actually 18 — corrected. (3) R10's mutation sentence overstated — corrected. (4) clock-read count on 7 rows where `c44fa4d` had 39 — all 39 restored in `441c88b`. (5) four non-gate audit findings unrecorded — R15 added. (6), (7) tracker nits — corrected |
| 2026-11-17 | `441c88b` | C2 tests | Fable (fresh subagent) | Findings: all test code and `gold-standard.md` verified clean, nothing unasserted against `c44fa4d`; 2 blockers and 2 smaller items, all tracker text | (1) R15 wrongly said the sibling shapes are outside the audit baseline; they are in it — corrected. (2) AO-8's commit list stopped at `e668263` — corrected. (3) R12 said three files carry the comment; five do — corrected. (4) verdict log, run log and days remaining stopped at run 4 — added in the commit that carries this row |

## Run log

| Date | Run | What was done | Models | Review verdicts | CI | Days remaining | Awaiting owner | Next |
|---|---|---|---|---|---|---|---|---|
| 2026-11-16 | 1 | Stopped at the environment check: the egress proxy refused `builds.dotnet.microsoft.com`. Nothing written | Fable | — | — | Clock not started | Allow the host | Re-run |
| 2026-11-16 | 2 | Environment and sync checks passed. Created `feature/dateonly-pilot` from main `2a7dd20`. Created this tracker and recorded the authorisation in the plan header and D19. C1: inventory, ten-line file list, draft PR #66 opened, D22 updated. Full local gate run on the source tree, which is identical to main: Release build 0 warnings; 47,007 passing executions (23,491 net8.0; 23,516 net10.0) plus the four timezone executions; 100% line and branch (8,986 of 8,986); format clean; audit gate 0 findings | Fable: orchestration, decisions, tracker. Opus: three read digests (plan documents, repository rules, DateOnly surface) | None — no code commit | Reported in the PR #66 description; a documentation-only change does not trigger the build, test, format or audit jobs | 14 of 14 (clock not started) | AO-3; AO-4 and AO-5 for information | C2 |
| 2026-11-16 | 3 | Environment and sync checks passed (.NET 10.0.404 SDK, 8.0.34 runtimes installed; main unchanged at `2a7dd20`). C2: the rule object `StringPastDateOnlyRule` with `StringPastDateOnlyOutcome`, Core tests and fixture (`f676e18`); review follow-up adding `IsPassed` and the white-space cases (`0eea9b2`). Design decisions F-5 to F-8 recorded; AO-6 and AO-7 raised. No kill-criterion signal: the rule object supplies everything the four adapters need, and its only allocation is the styles box on the invalid-styles path, which the current path also takes. Full local gate run by Fable before each commit: Release build 0 warnings; 47,181 passing executions (23,578 net8.0; 23,603 net10.0) plus the four timezone executions; 100% line (22,982 of 22,982) and branch (9,030 of 9,030); format clean; no `[ExcludeFromCodeCoverage]`; audit gate 0 findings | Fable: orchestration, design decisions, gate, tracker. Opus: one read digest, the implementation and the fix (effort xhigh). Fable (two fresh subagents): the code reviews | `f676e18`: findings, no blockers. `f676e18` + `0eea9b2`: ship | Green on `f676e18` (run 43836164687) and `0eea9b2` (run 43860147198) | 14 of 14 (box runs 2026-11-16 to 2026-11-30) | AO-3 (blocks the code assertions in C4 and C5); AO-6; AO-7; AO-4 and AO-5 for information | C3 |
| 2026-11-16 | 4 | Ran concurrently with run 3 and found C2 already committed. Environment and sync checks passed. Reviewed `f676e18` independently, fixed the open findings (`ebc8e42`: rule-level null-provider UTC test added to the `core-null-provider-timezone` job, `Unparseable` spelling, one expected-outcome convention, fixture-sourced `Value` rows), then merged run 3's `0eea9b2` and `5a9dd87`, which had reached the remote first (`c44e180`; no force-push). No behaviour change and no kill-criterion signal. Full local gate run by Fable before `ebc8e42` and again on the merged tree: Release build 0 warnings; 47,209 passing executions (23,592 net8.0; 23,617 net10.0) plus eight timezone executions; 100% line (22,982 of 22,982) and branch (9,030 of 9,030); format clean; no `[ExcludeFromCodeCoverage]`; audit gate 0 findings. No new chunk started | Fable: orchestration, merge, gate, tracker. Opus (effort xhigh): the fix. Fable (two fresh subagents): the code reviews | `f676e18`: findings, no blockers. `ebc8e42` + `c44e180`: ship | See the PR #66 description for the run on `c44e180` | 14 of 14 (box runs 2026-11-16 to 2026-11-30) | AO-3 (blocks the code assertions in C4 and C5); AO-6; AO-7; AO-8; two scheduled runs overlapped on this branch today — if that was not intended, space the schedule; AO-4 and AO-5 for information | C3 |
| 2026-11-16 to 2026-11-17 | 4, continued in session with the owner | The owner asked why the rule-object tests had no valid / invalid / edge split and then instructed: never deviate from the docs or precedent; gold standard is the minimum; leave nothing stale or half finished. Instructions written: `docs/ai/rules/global.md` ("Precedent and Consistency", "Gold Standard Is the Minimum — Nothing Left Behind"), `docs/ai/rules/testing.md` ("Before Every Test Change"), and this tracker's "Read before every run" section with the standing packet header. C2's tests reworked to match the testing docs and sibling tests over five commits (`373a0c2`, `c44fa4d`, `1e1417c`, `e668263`, `441c88b`): fixture scenario arrays, standard `Cases` and `Xxx_BehavesAsExpected` tests, `AdHocCases` for the detail rows, `IsPassed` on `RuleCase`, clock-read count on all 39 scenarios in the Must-layer shape, derived pinned dates, literal expectations; `gold-standard.md` recounted for all six projects. Test-only: no production change since the merge `c44e180`. Departures R1–R15 recorded. Not done: the "Coverage Verification" section of `gold-standard.md` is not re-dated (R14) — it needs the repository's PowerShell coverage scripts, which are not installed in this environment and could not be installed. One Opus fix was lost to a container restart and redone. No new chunk started. Full local gate run by Fable before every code commit; on `441c88b`: Release build 0 warnings; 47,345 passing executions (23,660 net8.0; 23,685 net10.0) plus eight timezone executions, none failed; 100% line (23,035 of 23,035) and branch (9,030 of 9,030); format clean; no `[ExcludeFromCodeCoverage]`; merge-blocking audit 0 findings | Fable: orchestration, decisions, gate, tracker, rules text. Opus (effort xhigh): every test change (six packets). Fable (five fresh subagents): the reviews | Five reviews, each `findings`; see the verdict log. The last found no finding in the test code; its findings were tracker text, corrected in `77b5f98`; a follow-up check by the same reviewer found two more tracker statements (a wrong base commit and a wrong date), corrected in the commit after it. No review returned `ship` | Green on every commit up to `441c88b` (run 44013623837). Red from `77b5f98` on the `audit` job only, caused by `main` commit `dd67c15`; see AO-9 | 13 of 14 (box ends 2026-11-30) | AO-9 (CI red from `main`); AO-3 (blocks the code assertions in C4 and C5); AO-6; AO-7; R1–R15; the R14 coverage re-date; AO-4 and AO-5 for information | C3 |
| 2026-11-18 | 5 | No pilot chunk. The run started on 2026-11-16 22:56 UTC, found the branch tip one minute old and stood down under rule 8; nothing was pushed then. Continued with the owner on 2026-11-18. Found that three sessions had committed to this branch and that the status board in `coordination.md` is local to one container, so cloud runs never saw each other. At the owner's instruction: added the [Active runs](#active-runs) claim table and replaced rule 8 with it; added "Sessions in Separate Containers" to `docs/ai/rules/coordination.md` and a pointer in `docs/ai/rules/global.md`; raised AO-10. The owner replaced the scheduled-task instruction with a revised version that carries the claim step. Not done: `main` is not merged into this branch (it is ahead by the dependency updates and `dd67c15`); the permission fix (AO-10) is not applied; C3 is not started. Gate for this docs-only commit, run by Fable: the audit gate (`doc-links`, `surface-parity`, `test-files`), 0 findings; no build or test run because no code changed | Fable: all of it. No subagents | None — no code commit | Red since `77b5f98` because of `main` (AO-9), unchanged by this run | 12 of 14 (box runs 2026-11-16 to 2026-11-30) | AO-9 (fix `main`); AO-10 (merge the permission fix); AO-3; AO-6; AO-7; AO-8; AO-4 and AO-5 for information | C3 |
| 2026-11-18 | 5, continued | At the owner's instruction in session ("Push to main then!!!", "Fix everything - lets go!!!"), three commits were pushed to `main`: `02e8afd` (permission rules), `fc045b3` (doc-links allowlist for the feature-specification skill plan), `8d31365` (hook rework merged from `claude/serene-galileo-1wtxvh`; hook tests 114 passing). AO-9 and AO-10 resolved. This branch: claimed in Active runs (`1653f81`), `main` merged in. C3 prepared and stopped before any code: the Must spec and the `must-codes` audit forbid the mapping call (AO-11). F-9 recorded. Not done: C3; the .NET install in the cloud environment setup script (owner). Gate run by Fable on the merged tree: Release build 0 errors and 0 compiler (CS) warnings, with 3 package warnings that come from `main` (AO-12); 47,345 passing executions (23,660 net8.0; 23,685 net10.0); 100% line (22,942 of 22,942) and branch (9,012 of 9,012); format clean; no `[ExcludeFromCodeCoverage]`; audit gate 0 findings | Fable: all of it. No subagents | None — no pilot code commit | See the PR #66 description for the run on this commit | 12 of 14 (box runs 2026-11-16 to 2026-11-30) | **AO-11 (blocks C3 and everything after it)**; AO-12; AO-3; AO-6; AO-7; AO-8 departures; AO-4 and AO-5 for information | C3, once AO-11 is answered |
| 2026-11-18 | 6 (scheduled, unattended) | **Not done: no pilot chunk.** The owner has not answered AO-11 (no comment or review by `stevomccormack` on PR #66, and no owner text on this page, since run 5), and AO-11 blocks C3 and through it C4 to C7, so this was a verification-only run. Done: claimed the branch (`40ba888`); merged `main` `8b24472` (#81, `net11.0` as a fourth target framework) into this branch with no conflict (`dab07ae`); installed the .NET 11 SDK, which the merged tree needs (AO-13 raised); AO-12 found resolved by the same `main` commit. No code, test or design change. Gate run by Fable on the merged tree: Release build 0 warnings and 0 errors; 71,038 passing executions (23,660 net8.0; 23,685 net10.0; 23,693 net11.0) plus six timezone executions under `Pacific/Kiritimati`, none failed or skipped; 100% line (23,079 of 23,079) and branch (8,998 of 8,998); format clean; no `[ExcludeFromCodeCoverage]`; audit gate 0 findings | Fable: all of it. No subagents | None — no code commit | Green on `40ba888` (run 44241560615). The merge `dab07ae` (run 44242678358) was still running when this row was written. `main` green on `8b24472` (run 44239592156). See the PR #66 description for the result on `dab07ae` and on this commit | 12 of 14 (box runs 2026-11-16 to 2026-11-30) | **AO-11 (blocks C3 and everything after it)**; AO-13; AO-3; AO-6; AO-7; AO-8 departures (R1–R15, including the R14 re-date); AO-4 and AO-5 for information | C3, once AO-11 is answered |
