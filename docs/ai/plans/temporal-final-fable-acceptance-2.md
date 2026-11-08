# Fable final temporal plan acceptance

- Reviewer/model: Claude Fable 5.1 (claude-fable-5-1, provider firstParty)
- Claude Code CLI: 2.1.283
- Persistent session: 7342dc25-a35f-42a2-a77e-3ea894c6b4eb
- CLI result fields as reported: subtype success; exit 0; num_turns 26; duration_api_ms 1348461; total_cost_usd 19.94766475
- ModelUsage fields as reported: input 682, cache read 8785139, cache creation 609168, output 111224; web searches 0. Resumed-session accounting scope is not independently verified; these are not asserted as independent per-call time or usage.
- Scope: final focused readiness acceptance of the corrected temporal plan set. Public result only.

# Final plan-readiness acceptance, 2026-09-27 (frozen batch)

All seven finding groups close against the live text and source. Two editorial residues remain and are listed below as non-blocking; they cannot alter execution because T-00 §0.1 steps 2 and 5 and the tracker's `Depends on` rows govern dispatch, not the §2 summary cells.

## Closure matrix

| # | Finding | Status | Current evidence |
|---|---|---|---|
| 1a | Stale council receipt | CLOSED | `temporal-review-council-rerun.md:13-53` is a dated log through the 2026-09-27 NOT READY result and both follow-up rulings; transcript `temporal-00-program.fable-council-transcript.md:352-363` mirrors it; Standards `temporal-timezone-sync.md:18-28` cites the transcript only. |
| 1b | Duplicate `Date.Kind` | CLOSED | Reuse only at `temporal-00-program.md:387`; `temporal-02-timezone-core.md:122-124`; `temporal-04-validity-relative.md:58-59`; `temporal-astra-decisions.md:192`. Source node exists at `MustCodes.Date.cs:172-195`. |
| 1c | `Date.Reference.OutOfRange` first consumer, atomic U2/U2b | CLOSED | `temporal-01-foundation.md:233` and U2b; `temporal-00-program.md:386, 601`; orchestration `:272-273`. |
| 1d | Phase 1 ownership | CLOSED | `temporal-01-foundation.md:450` (U4 excludes both TimeOnly TestData files); U5 after U2/U3 at `temporal-00-program.md:148`. |
| 1e | T-05 codes complete | CLOSED | `temporal-05-durations-calendar.md:48-66`, including `Date.Interval`. |
| 1f | Half-hour Lord Howe fixture | CLOSED | `temporal-01-foundation.md:204-213`; `temporal-02-timezone-core.md:240`. |
| 1g | InternalsVisibleTo stated as existing, not added | CLOSED | `temporal-01-foundation.md:263-267` cites the existing grant; source `PineGuard.Core.csproj:19` (MustClauses) and `:20` (GuardClauses). Editorial note E2 below. |
| 2a | Qodana token or `-Linter auto`; no InspectCode substitution | CLOSED | `temporal-00-program.md:585-589`; orchestration `:212-214, 275-278`; `Run-Qodana.ps1:46-48, 79, 248-249`. |
| 2b | Tier 1 owner confirmation for main merge/push; Standards mirror | CLOSED | `temporal-00-program.md:530-535, 553-554`; Standards `temporal-timezone-sync.md:85`; orchestration §3.6. |
| 2c | U2b exact evaluation path | CLOSED | `temporal-01-foundation.md:379-385` names `TryGetTodayDate`, `DateOnlyRules.IsBefore`/`IsAfter`, `TryGetAge`. |
| 2d | backward/etcetera provenance; routine never re-downloads; no generation | CLOSED | Manifest `:3, 10-14, 24, 32`; `temporal-03-release-standards-sync.md:33-78, 186-198`; orchestration `:70, 298-300, 326`; Standards `:26-28, 107-119`. |
| 2e | TimeOnlyRange excluded; `GuardStringTimeOnlyClausesTestData` named; T-04 consumes `TryGetAge` | CLOSED | `temporal-01-foundation.md:411, 450`; `temporal-04-validity-relative.md:79-80`. |
| 3 | Focused MustCodes ruling applied | CLOSED | `Id.Null`/`Id.Unknown` at `temporal-00-program.md:237-246` and `temporal-02-timezone-core.md:260-270, 299`; `Kind.NotUnspecified` at `:127-130, 331-334`; zoned equality in TimeZone families at `temporal-00-program.md:257` and `temporal-02-timezone-core.md:277-283, 289-295, 307-308`; `Date.Interval` vs `Time.Interval` at `temporal-00-program.md:301, 351-352` and `temporal-05-durations-calendar.md:56-66`; Serves at `temporal-00-program.md:323-324`, `temporal-06-formats-epoch.md:56-59`; A-08 at `temporal-astra-decisions.md:192`. |
| 4 | B1 Proximity codes preserved; six deleted-member keys burned down | CLOSED | `temporal-04-validity-relative.md:32, 56, 67-72` keep `Proximity.NotWithin`/`Within` (live at `MustDateTimeOffsetClauses.cs:503, 533` and siblings); the six keys, deletions only, at `:245-254`. |
| 5a | `layerAbsences` v2 schema, six entries, strict validation, Must never absent, no baseline/ignore/alias/exception | CLOSED | `temporal-00-program.md:366-382`; `temporal-02-timezone-core.md:56-77, 256-259`; `temporal-04-validity-relative.md:256-261`; `temporal-05-durations-calendar.md:165-170, 200`. |
| 5b | U0 gates and VIBE vectors; string-tail Rules discovery; baseline burn-down | CLOSED | `temporal-02-timezone-core.md:44-115` (files, fixtures, deletions, `typecheck`/`lint`/`test`/`--gate`/full audit/no-baseline artifact). |
| 5c | Per-unit strict checks plus post-adapter phase cohort | CLOSED | `temporal-00-program.md:556-583`; T-01 `:579-580`; T-02 `:338-340, 406-407`; T-04 `:285-287`; T-06 `:205-213`; orchestration `:271-275`. |
| 5d | UnixTime and string DateTime / UTC offset siblings | CLOSED | `temporal-06-formats-epoch.md:181-195`; `temporal-02-timezone-core.md:153-158, 272-275, 350-352, 412-418, 440-442`. |
| 6a | `IsDaylightSavingTime` full concept | CLOSED | Rule kept at `temporal-02-timezone-core.md:198-199, 241`; codes `temporal-00-program.md:340, 394`; D-15 addendum `:189-192`; Must `temporal-02-timezone-core.md:305-306`; Guard `:378-384`; Fluent `:433-436`; DA `:477-482`; test instants `temporal-01-foundation.md:191, 194`; canon row `temporal-00-program.md:256`; no backlog line remains in §12 (`:657-666`). |
| 6b | Literal rule descriptions | CLOSED | `temporal-02-timezone-core.md:106-115` matches `temporal-audit-gate-followup.md` §2 character for character; U0 owns the rule files and `+ audit-cli/spec.md` §3.1 copy (`:62`). |
| 6c | T-02 U0 DAG | CLOSED | `temporal-00-program.md:149`; `temporal-02-timezone-core.md:29-36, 40-47` (U0 first, ∥ U1/U2, U3 depends on U0); tracker `temporal-orchestration.md:57`. |
| 7 | Sequencing, single-writer ownership, Repo B boundary, status | CLOSED | 2 → 4 → 5 → 6 at `temporal-00-program.md:156-160, 407-408, 515`; vocabulary single writers `:370-375, 382`; download-only boundary as in 2d; status `planned` (`temporal-00-program.md:5`), README `:54`, tracker P0 `Needs fix` (`:45`), Standards `:5`. Receipt, transcript and Standards mirror agree. |

## Editorial residues (non-blocking, apply in the activation edit)

- **E1** `temporal-00-program.md:151-152`: the Phase 4 fan-out cell says "U0 then U4" though T-04 has no U0 (`temporal-04-validity-relative.md:30-38`), and the Phase 5 cell is an approximation of T-05 §2. Replace with the phase plans' actual fan-out. Non-blocking because T-00 §0.1 step 2 and step 5 route implementers to the phase plan §2 table and the tracker `Depends on` rows.
- **E2** `temporal-01-foundation.md:266`: cite the MustClauses grant as `PineGuard.Core.csproj:19`; line 20 is the GuardClauses grant.

## Verdict

**P0 READY FOR IMPLEMENTATION.**

The frozen plan set is coherent; all prior blockers, corrections and rulings are applied; no owner or model decision, material factual correction, or gate issue remains. This verdict covers plan readiness only. It claims no code implementation, test or scanner pass, merge, release, generated classes, or any InspectCode gate substitution.