# Fable final plan acceptance

- Reviewer/model: Claude Fable 5.1 (claude-fable-5-1, provider firstParty)
- Claude Code CLI: 2.1.283
- Persistent session: 7342dc25-a35f-42a2-a77e-3ea894c6b4eb
- Result: success; CLI exit 0; 69 turns; duration 678142 ms; cost USD 12.55129675
- Usage: input 452, cache read 3644867, cache creation 438928, output 57140; web searches 0
- Scope: focused acceptance of enumerated prior findings and corrected temporal plan set. Public result only.

# Final correction acceptance review, 2026-09-27

Scope: the six prior blockers, the listed corrections and polish, and the six items of the focused audit ruling, checked against the live PineGuard and Standards worktrees. Everything I was asked to close is closed. Three new gate-consistency defects surfaced while verifying "all mandatory audits pass without a bypass", and one of them is a source-fact error of the same kind as the earlier Date.Kind blocker. They are non-design corrections, but they block literal execution, so the verdict is not ready.

## Closure matrix: prior blockers

| # | Finding | Status | Evidence |
|---|---|---|---|
| 1 | Stale council receipt | CLOSED | `temporal-review-council-rerun.md:13-44` is a dated log with both 2026-09-27 entries; Standards `temporal-timezone-sync.md:19-22` cites the transcript only. |
| 2 | Duplicate `Date.Kind` constants | CLOSED | Reuse only: `temporal-00-program.md:363`; `temporal-02-timezone-core.md:44-46`; `temporal-04-validity-relative.md:58-59`; `temporal-astra-decisions.md:192`. Source node at `MustCodes.Date.cs:172-195`. |
| 3 | `Date.Reference.OutOfRange` consumer and audit gate | CLOSED | Atomic U2/U2b cohort: `temporal-01-foundation.md:227, 236, 370-394`; `temporal-00-program.md:362, 567`; `temporal-orchestration.md:270-271`. Friend grant exists at `PineGuard.Core.csproj:19` and is cited, not added, at `temporal-01-foundation.md:257-261`. |
| 4 | Phase 1 unit ownership conflict | CLOSED | U4 excludes the TimeOnly range family at `temporal-01-foundation.md:443-445`; U5 after U2 and U3 at `:40`; fan-out cell `temporal-00-program.md:148`; tracker `temporal-orchestration.md:55`. |
| 5 | T-05 missing codes | CLOSED | Full positive and negative lists plus `Date.Interval` at `temporal-05-durations-calendar.md:48-66`. |
| 6 | Half-hour fixture underspecified | CLOSED | Exact zone, delta, transitions and three dates at `temporal-01-foundation.md:204-213`; T-02 fixture row `temporal-02-timezone-core.md:162`. |

## Closure matrix: prior corrections and polish

| Item | Status | Evidence |
|---|---|---|
| Cite existing InternalsVisibleTo, do not add | CLOSED | `temporal-01-foundation.md:257-261`; `PineGuard.Core.csproj:19`. |
| Qodana token or `-Linter auto`; InspectCode not a substitute | CLOSED | `temporal-00-program.md:551-555`; `temporal-orchestration.md:209-214, 271-275`. Verified against `Run-Qodana.ps1:46-48, 79, 248-249` and `qodana.md:24-28`. Gate stays Qodana plus Sonar; no substitution recorded. |
| Tier 1 merge and push authority explicit, no blanket delegation | CLOSED | `temporal-00-program.md:505-510, 525-529` (release confirmation does not extend to merges); `temporal-orchestration.md:316-324`; `temporal-03-release-standards-sync.md:82, 108-109`. |
| U2b names the internal evaluation path | CLOSED | `TryGetTodayDate` then `DateOnlyRules.IsBefore`/`IsAfter` or `TryGetAge`, `temporal-01-foundation.md:379-385`. |
| No false claim that backward/etcetera are undownloaded | CLOSED | Manifest hashes at `temporal-reference-manifest.md:13-14`; `temporal-orchestration.md:69, 322`; `temporal-03-release-standards-sync.md:194-198`. |
| TimeOnly range helper outside the reversal change | CLOSED | `temporal-01-foundation.md:405-406`. Current linear body at `TimeOnlyRules.cs:21-22`; Must guard to delete at `MustTimeOnlyClauses.cs:45-46, 80-81`. |
| T-01 U4 names `GuardStringTimeOnlyClausesTestData` | CLOSED | `temporal-01-foundation.md:443-444`; file exists. |
| T-04 consumes, not duplicates, `TryGetAge` | CLOSED | `temporal-04-validity-relative.md:80-81, 175-184`. |
| Phase 1 U5 fan-out and codes-only cohort explicit | CLOSED | `temporal-00-program.md:148, 564-567`; `temporal-orchestration.md:270-271`. |
| Status not active before acceptance | CLOSED | T-00 header `planned` (`:5`); `docs/ai/README.md:54`; tracker P0 "Needs fix" (`temporal-orchestration.md:45`); Standards plan `:5, 18-20`. |
| Repo B downloads and verifies only, no generation | CLOSED | `temporal-reference-manifest.md:3, 27-32`; `temporal-03-release-standards-sync.md:33-78, 186-192`; Standards `temporal-timezone-sync.md:26-28, 102-117`. |
| Phase order 2 → 4 → 5 → 6 | CLOSED | Stated consistently at `temporal-00-program.md:156-160, 382-383, 490`, `temporal-orchestration.md:80-84`, T-04/T-05/T-06 headers. |

## Closure matrix: focused audit ruling

| Item | Status | Evidence |
|---|---|---|
| `TimeZone.Id.Null` first, `Id.Unknown` distinct | CLOSED | `temporal-00-program.md:237-241, 329`; `temporal-02-timezone-core.md:179-187, 216`. `MustMessage.cs:16-19` confirms only `{paramName}` is substituted, so the unknown-ID template renders as specified. |
| `TimeZone.Kind.NotUnspecified`, A-12 template, no positive code | CLOSED | `temporal-00-program.md:330, 368`; `temporal-02-timezone-core.md:49-52, 246-249, 351-352`; `temporal-astra-decisions.md:192`. |
| Zone-aware SameDay in TimeZone families with `TimeZone.Equality` | CLOSED | `temporal-00-program.md:252, 331, 369, 377`; `temporal-02-timezone-core.md:194-212, 222-223, 267-268, 279-280, 300-302`. Plain DTO SameDay stays Date at `temporal-00-program.md:280` and `temporal-02-timezone-core.md:196, 212`. No DA attribute (`:211`). |
| `Date.Interval` for DateTime/DTO, `Time.Interval` for TimeOnly, guard before negation | CLOSED | `temporal-00-program.md:296, 346, 373`; `temporal-05-durations-calendar.md:56-66`. |
| Serves mappings | CLOSED | `temporal-00-program.md:318-323`; `temporal-06-formats-epoch.md:56-59`; `temporal-02-timezone-core.md:41-43`. |
| Corrective application, superseded sentences only | CLOSED | `temporal-00-program.md:320-323`; `temporal-astra-decisions.md:192`; `temporal-review-council-rerun.md:38-44`; transcript `:352-356`. Historical follow-up #5 text at transcript `:220` stays as history. |

Contract checks that pass by source: `nullability` classifies `TimeZoneInfo? resolvedZone` as a nullable reference primary (`nullability.ts:187-194, 331-339`), so `ResolvedTimeZone` passes. `must-codes` checks (a), (b) and (d) pass for the planned TimeZone and Interval constants and the DA dispatch shape (`must-codes.ts:409-461, 554-579`). The cited Fluent precedent exists at `src/PineGuard.FluentValidation/ValidationResultExtension.cs:47`.

## New blocking corrections (non-design)

**B1. T-04 U1 deletes two constants that are still referenced.** `temporal-04-validity-relative.md:32, 67-74` instructs deleting `MustCodes.Date.Proximity.NotWithin` and `.Within` and asserts a grep confirmed no other references. Source contradicts this. The existing proximity clauses `Within`/`NotWithin` use them at `MustDateOnlyClauses.cs:503, 532`, `MustDateTimeOffsetClauses.cs:503, 533`, `MustDateTimeClauses.cs:563, 593`, `MustStringDateOnlyClauses.cs:288-344`, `MustStringDateTimeOffsetClauses.cs:286-342`, and five Fluent extension files (for example `FluentDateTimeOffsetExtensions.cs:681-728`). Deleting them breaks compilation across Must and Fluent. Requirement: `spec.md` §6.2 (do not invent repo facts); T-00 §0 (implementers decide nothing). Correction: strike the deletion from T-04 U1 and its unit-table title; the D-6 removal leaves both constants live; T-04 U1 makes no Proximity edit.

**B2. `audit layer-parity` cannot exit 0 for the units that add a concept with a deliberately missing layer.** The rule is symmetric and strict and treats a missing DataAnnotations implementation as a finding (`layer-parity.ts:30-39, 76-83, 409-434`). The CLI exits 1 on any un-baselined finding (`docs/ai/specs/tools/audit-cli/spec.md:84, 99, 177-179`). Today's analogous gaps pass only because they are baselined (`baseline.json:257` `SameDay:DataAnnotations`, `:278` `Within:DataAnnotations`). New un-baselined keys the plans create: `ResolvedTimeZone` missing Guard and Fluent (`temporal-02-timezone-core.md:177-178, 337`); `WithinValidity` missing DA (`temporal-00-program.md:274`; `temporal-04-validity-relative.md:315`); `SameMonth`, `SameYear`, `SameIsoWeek` missing DA (`temporal-00-program.md:292`; `temporal-05-durations-calendar.md:192`). T-00 §9 `:544` requires `audit layer-parity` to exit 0 per unit, and `temporal-02-timezone-core.md:287`, `temporal-04-validity-relative.md:324` require it green. No plan names a mechanism; grep of `temporal-*.md` shows no `ignoreMethods`, `exceptions.json` or baseline step. Correction: T-00 §9 and the affected units must state the documented mechanism for these deliberate absences. The rule's own documented exemption path is `vocabulary.json` `ignoreMethods`/`aliases` (`layer-parity.ts:40-41, 64-67`); the baseline is described as debt to burn down (`audit-cli/spec.md:186-188`), which these are not. Choosing between them is an owner call to record, not an implementer choice.

**B3. `audit ordering` cannot exit 0 for new string Must families.** The rule expects a Rules sibling at `src/PineGuard.Core/Rules/<Tail>Rules.cs` (`ordering.ts:656-665, 443-447`), which string families never have; every existing one is baselined (`baseline.json:591-599`, for example `ordering:MustStringDateTimeOffsetClauses:rules:missing-layer`). New families `MustStringUtcOffsetClauses` (T-02) and `MustStringDateTimeClauses` (T-06) will emit new un-baselined `rules:missing-layer` keys while `temporal-02-timezone-core.md:253` and T-00 §9 `:545` require `ordering` green. Same class of fix as B2: name the mechanism. Additionally, `ordering` derives sibling file names from the Must tail (`ordering.ts:487, 541, 647`), so T-06 U4/U5 (`temporal-06-formats-epoch.md:179-185`) must name `GuardUnixTimeClauses.cs`, `FluentUnixTimeExtensions.cs` and `UnixTimeAttributes.cs` explicitly, or the unit will produce `missing-layer` findings and the build prompt forbids the implementer from choosing.

## Non-blocking polish

- `temporal-02-timezone-core.md:199-201`: the "Clone from" sentence is garbled after the edit.
- `temporal-04-validity-relative.md:56`: "Update the file's Serves comment" is a no-op in T-04; say "unchanged".
- `temporal-orchestration.md:295-296`: "never downloads the backward file (the owner commits it)" should say the routine must not re-download the already-verified files.
- Standards `temporal-timezone-sync.md:82-84`: add a cross-reference to T-00 §8's owner-confirmation rule for the Standards merge and push.

## Verdict

**P0 NOT READY.**

Blocking corrections, all non-design:

1. `temporal-04-validity-relative.md` §U1 (`:32, 67-74`): remove the `Date.Proximity.NotWithin`/`.Within` deletion. Requirement: `spec.md` §6.2; source at `MustDateTimeOffsetClauses.cs:503, 533` and the other files listed under B1.
2. `temporal-00-program.md` §9 and `temporal-02-timezone-core.md` U3/U4, `temporal-04-validity-relative.md` U3, `temporal-05-durations-calendar.md` U3b/U5d: state the documented, owner-recorded mechanism by which `audit layer-parity` exits 0 for `ResolvedTimeZone`, `WithinValidity`, `SameMonth`, `SameYear`, `SameIsoWeek`. Requirement: `audit-cli/spec.md:99, 177-188`; `layer-parity.ts:40-41, 409-434`; no bypass or undocumented exemption.
3. `temporal-00-program.md` §9 and `temporal-02-timezone-core.md` U3, `temporal-06-formats-epoch.md` U3a/U4/U5: state the same for `ordering`'s `rules:missing-layer` on `MustStringUtcOffsetClauses` and `MustStringDateTimeClauses`, and name the UnixTime family's Guard, Fluent and DA files. Requirement: `ordering.ts:443-447, 487, 541, 647, 656-665`; `baseline.json:591-599` precedent.

All six prior blockers, every listed correction, and all six items of the focused audit ruling are closed in the live documents. This verdict covers plan readiness only. It claims no implementation, test, scan, merge, release or generation.