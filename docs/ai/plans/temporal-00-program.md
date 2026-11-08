<!-- metadata_header
type: plan
id: temporal-00-program
version: 1.1
status: active
last_updated: 2026-09-27
final_acceptance_ref: docs/ai/plans/temporal-final-fable-acceptance-2.md
council_verdict_ref: docs/ai/plans/temporal-00-program.fable-council-transcript.md
historical_council_verdict_ref: docs/ai/plans/temporal-00-program.council-transcript.md
council_verdict_date: 2026-09-26
council_trigger: api
-->

# Plan T-00 — Temporal & Time-Zone Program: Charter, Decisions, Naming Canon, Sub-Agent Contract

> **Status**: Final design adjudicated by Astra under the owner's authorization for this revision.
> Revised real Claude Fable 5.1 council and corrective acceptance are complete: P0 READY FOR
> IMPLEMENTATION, recorded in `temporal-final-fable-acceptance-2.md`. Historical evidence remains separate.
> No further owner row-signing is required. [A-01]
> **Author**: Claude Fable 5.1 planning pass (2026-09-22), from the temporal gap assessment of the same day.
> **Implementers**: Codex team agents; Claude Fable 5.1 handles all
> implementation decisions, reviews and councils. Dispatch follows
> [temporal-orchestration.md](temporal-orchestration.md). **Nothing in the phase plans asks an
> implementer to decide anything.** Where a choice exists, this charter has already made it, or §3
> records the final Astra direction and mandatory Brain execution gates.
> **Repos**: `stevomccormack/PineGuard` (this repo) and `stevomccormack/PineGuard.Standards` (sibling,
> `D:\Steve McCormack\GitHub\@stevomccormack\PineGuard.Standards`). Both repos do all work in
> `.claude/worktrees/<slug>` on `feature/<slug>`.

## 0. How to read this plan set

### Review authority and evidence (2026-09-26)

The initial plan review used **Astra for adjudication, Luna for research and source verification,
and GPT-6 Sol for transcription**. [The Astra decision register](temporal-astra-decisions.md)
records each material change as `A-xx`, including the previous direction, final recommendation,
reason, evidence and implementation risks. Compact references beside changed sections identify
these decisions without repeating the governance contract. The earlier Claude Fable 5.1 council transcript
is historical evidence and has not been rewritten as a new approval.

Current execution routing (owner override, 2026-09-27) supersedes earlier Sonnet/Opus-only instructions: the Codex team handles implementation, documentation, tests and commits; GPT-6 Sol writes plans; GPT-6 Luna handles read/query/web/CLI evidence. Actual Claude Fable 5.1 performs reviews. Historical model attribution remains provenance. **Claude Fable 5.1 owns
every future decision, review and council**, including the required fresh council on this revised
plan before activation. An unavailable model or failed council run is a recorded gate, never an
implied approval. Astra's decisions are final for this review; an implementer may reopen one only
for a reproducible implementation bug or demonstrable correctness failure. A confidence score
alone cannot justify reversal. First identify the applicable Brain clauses and repository
precedent, reproduce the failure and assess root-cause alternatives; Claude Fable 5.1 then adjudicates and
runs the architectural council where required. Any exceptional Brain departure must be the
demonstrated last resort after compliant alternatives fail. Record the approved reversal and
evidence in the decision register and affected plan/spec sections, and disclose it in the PR and
final report. Until approved, stop only the affected unit and continue independent work. [A-01]

| File | Purpose |
|---|---|
| **T-00 (this file)** | Charter: premise, scope, decisions register, naming canon, error-code additions, conventions, sub-agent contract, worktree/merge protocol, verification gate, Definition of Done, routines design, risks. Every phase plan assumes this and does not restate it. |
| [T-01 Foundation](temporal-01-foundation.md) | Fix "today" derivation, `FixedTimeProvider.LocalTimeZone`, TimeOnly midnight-crossing ranges, TimeSpan rename + guard polarity, precision-enum symmetry. |
| [T-02 Time-zone Core](temporal-02-timezone-core.md) | Deterministic supplied-zone `TimeZoneRules`, UTC-offset rules and zone-aware instant rules across all five layers; host-ID resolution stays at composition boundaries. [A-05] |
| [T-03 Release + Standards sync](temporal-03-release-standards-sync.md) | Publish `v0.1.0-alpha.8`; bump Standards; adapt Standards to the alpha.8 API; separate host resolution from packaged membership; download-only reference refresh before explicitly approved generation. [A-04, A-05] |
| [T-04 Validity + relative time](temporal-04-validity-relative.md) | Expired / effective / validity windows, within-last / within-next / older-than, maximum age, DateTimeOffset parity. |
| [T-05 Durations + calendar](temporal-05-durations-calendar.md) | TimeSpan family (positive, whole units, multiple-of, timeout), calendar predicates, ISO week, business days, intervals. |
| [T-06 Formats + epoch](temporal-06-formats-epoch.md) | `StringRules.DateTime` family, ISO 8601 / RFC 3339 / RFC 1123 / ISO 8601 duration, Unix seconds and milliseconds. |
| [T-07 Close-out](temporal-07-closeout.md) | Docs, README rows, roadmap reconciliation, agent memory, Standards Brain sync, worktree cleanup. |
| [Orchestration + progress tracker](temporal-orchestration.md) | The session handoff loop, the progress tracker (source of truth), verbatim dispatch prompts, verbatim routine prompts and setup script. |

### 0.1 Implementer quick start

1. Read [temporal-orchestration.md](temporal-orchestration.md) §0 and §1. Trust the tracker over memory.
2. Open the phase plan for the next incomplete unit and read the unit **in full, from disk**.
3. Every unit lists: worktree, model, agent name, inputs (files to clone the pattern from), exact
   outputs (files + signatures + messages + codes), acceptance commands, budget, timeout, check-in.
4. Follow the final Astra decisions in §3 and the A-register; complete mandatory Claude Fable 5.1 council
   activation gates before gated implementation. Do not ask the owner to sign the same decisions again.
5. Do not start a unit whose `Depends on` units are not marked **Done** in the tracker.

## 1. Business plan

### 1.1 Thesis

PineGuard already threads `TimeProvider` through all five layers and has a deeper temporal Must
surface than the roadmap admits. What it lacks is (a) any awareness of time zones, (b) the
"says what it does" vocabulary for the most common temporal checks in production code (expired,
effective, within the last hour, older than five minutes, business day, timeout), and (c) one
correctness defect: "today" is always the UTC calendar date. This program closes all three while
keeping Core deterministic. Supplied `TimeZoneInfo` objects drive Core predicates; host-ID
resolution stays at existing composition/adapter boundaries. Standards supplies versioned
catalog membership and mapping, which answer different questions from host resolvability. [A-05]

### 1.2 Verified findings this program is built on (2026-09-22, from source, not docs)

| # | Finding | Where |
|---|---|---|
| F-1 | `TimeProvider.LocalTimeZone` is unused in `src/`. "Today" for `DateOnly` past/future/age and `DateTime` age is the UTC date. | [DateOnlyRules.cs:279](../../../src/PineGuard.Core/Rules/DateOnlyRules.cs#L279), [DateTimeRules.cs:367](../../../src/PineGuard.Core/Rules/DateTimeRules.cs#L367) |
| F-2 | Zero uses of `TimeZoneInfo`, `ISOWeek` or `Calendar` in `src/`. | grep |
| F-3 | `FixedTimeProvider` pins `GetUtcNow` only; `LocalTimeZone` silently returns the dev machine's zone. | [FixedTimeProvider.cs](../../../tests/PineGuard.Testing/Common/FixedTimeProvider.cs) |
| F-4 | `TimeOnlyRules.IsBetween` is a linear comparison; a 22:00–06:00 window is unrepresentable. `TimeOnlyRange` rejects `Start > End`. | [TimeOnlyRules.cs:21](../../../src/PineGuard.Core/Rules/TimeOnlyRules.cs#L21) |
| F-5 | `DateTimeRules.IsSameDay` normalises to the UTC date; `IsWeekday` / `IsFirstDayOfMonth` use the wall-clock date. Two calendars in one class. | [DateTimeRules.cs:244](../../../src/PineGuard.Core/Rules/DateTimeRules.cs#L244), [:302](../../../src/PineGuard.Core/Rules/DateTimeRules.cs#L302) |
| F-6 | `TimeSpanRules` has three rules; `GuardTimeSpanClauses.LessThan` / `GreaterThan` call the complement Must clause but forward the same `inclusion`, so `Guard.Against.LessThan(v, t)` throws when `v == t` while every other guard family passes on the boundary. | [GuardTimeSpanClauses.cs:112](../../../src/PineGuard.GuardClauses/GuardTimeSpanClauses.cs#L112) |
| F-7 | No `StringRules.DateTime` family exists (only DateOnly, DateTimeOffset, TimeOnly, TimeSpan). | `src/PineGuard.Core/Rules/` |
| F-8 | `DateTimePrecision` stops at `Millisecond`; `TimePrecision` has `Tick`; neither has `Microsecond`. | `src/PineGuard.Core/Common/Precision.cs` |
| F-9 | .NET ships no time-zone database. Windows ids come from the registry; IANA ids from OS tzdata via ICU; IANA↔Windows conversion is ICU/CLDR-dependent and fails under NLS mode, invariant mode, or older Windows without app-local ICU. | Microsoft Learn: `dotnet/core/extensions/globalization-icu` |
| F-10 | Standards ships `Iana.TimeZones` (zone1970.tab snapshot, 312 zones; the provider hand-adds `Etc/UTC` and `Etc/GMT`; no other `Etc/*` rows, e.g. `Etc/GMT+1`, and no `backward` links — verified 2026-09-26, hence D-8 ingests `etcetera` as well as `backward`) and `Cldr.TimeZones` (CLDR 48 windowsZones, 500 mappings), plus `Must.Be.SystemTimeZone` which already calls `TimeZoneInfo.TryFindSystemTimeZoneById`. | Standards `src/PineGuard.Standards.Iana.TimeZones/` |
| F-11 | Standards pins `PineGuard.*` at `0.1.0-alpha.7` from nuget.org. PineGuard `main` has unreleased API changes since alpha.7 (error codes on `MustResult.FromBool`, `GuardFailure.Throw(result, message, exceptionCreator)`). The Standards bump is a repo-wide adaptation. | Standards `Directory.Packages.props`; PineGuard `git tag` |
| F-12 | CI runs on `ubuntu-latest`; local development is Windows. Zone-id tests must resolve on both. | `.github/workflows/ci.yml` |
| F-13 | Roadmap Part 7 is mostly already implemented (contains/starts/ends, JWT, SemVer, cron, Base64Url, UTF-8, graphemes, BOM, normalisation, Luhn, decimal scale, magic bytes, UUID version, hostname, regex). Only slug, ULID, MAC, full MIME remain. | `src/PineGuard.Core/Rules/`, `Utils/` |

### 1.3 Success metrics

- Every unit ships at 100 % line **and** branch coverage on `net8.0` and `net10.0`, zero Roslyn
  warnings, zero Qodana problems, `audit --gate` green, `dotnet format` clean.
- Every new public name appears in §4 with its rejected alternatives and is covered by the actual
  required Claude Fable 5.1 review/council before gated implementation; no repeated owner signing.
- `Must.Be.NotExpired(token.ExpiresAt)`, `Must.Be.ValidLocalTime(local, zone)`,
  `Must.Be.Timeout(options.Timeout)`, `Must.Be.BusinessDay(date)` compile and read on the tin.
- Standards builds against `PineGuard.* 0.1.0-alpha.8`, host resolution remains at its existing
  composition boundary, packaged membership stays versioned and deterministic, and `US/Eastern`
  is accepted as an IANA link. [A-05]
- Both repos end with a single worktree and a clean `main`.

### 1.4 Program risks (mitigations in §11)

Signature ossification before sign-off; zone lookups that behave differently on CI (Linux) and dev
(Windows); Standards adaptation debt (F-11) dwarfing the facade change; routines that cannot merge
to `main`; sub-agents that die silently.

## 2. Scope by phase

### Temporal type and policy coverage [A-02, A-03]

| Type / concern | Decision and owning phase |
|---|---|
| `DateTime`, `DateTimeOffset`, `DateOnly`, `TimeOnly`, `TimeSpan` and their nullable forms | First-class existing validation families; extend all applicable layers consistently. T-01/T-04/T-05/T-06 define civil dates, instants, ranges, durations and formats. |
| `TimeZoneInfo` | Supplied configuration for deterministic zone-aware predicates (T-02); custom zones are supported without requiring an installed ID. |
| `TimeProvider.System`, custom `TimeProvider`, `FixedTimeProvider`, `FakeTimeProvider` | All clock-consuming APIs accept `TimeProvider`; these are implementations, not separate validation families. T-01 preserves FixedTimeProvider for reproducible wall-clock fixtures. Advancing/timer tests may use the Microsoft test package under repository test-dependency precedent; no production dependency. |
| Timestamp counters, `ITimer`, `Stopwatch` | Scheduling and monotonic elapsed measurement are outside validation-family scope. `TimeSpan` represents elapsed values; wall-clock predicates must never compare timestamp counter units. |
| `Calendar`, `DayOfWeek`, `DateTimeKind` | Existing enum validation handles enum legality; Gregorian/ISO calendar semantics are explicit in T-05. Arbitrary calendars require a separate reviewed proposal. |
| UTC offsets and numeric epoch values | Offset legality and zone-at-instant matching in T-02; Unix seconds/milliseconds and checked range/precision in T-06. |
| NodaTime and `SqlDateTime` / other wrappers | Considered and deferred: no production dependency or new family without a separate demonstrated consumer use case and Claude Fable 5.1 review/council. |

Use existing explicit `TimeProvider` / `TimeZoneInfo` parameters. Add narrowly scoped immutable
policy options only when a real distinct policy warrants them; do not introduce a global mutable
clock, zone or culture, or one umbrella `TemporalOptions`. Calendar today uses the provider's
`LocalTimeZone`; civil-date comparisons and instant comparisons remain explicitly separate.
Capture now once per predicate/operation. Composed rules evaluate independently unless the caller
explicitly supplies a shared snapshot; this program does not promise cross-rule atomic now.

| Phase | Repo | Worktree slug | Units | Fan-out | Depends on |
|---|---|---|---|---|---|
| 0 Gate | both | — | Astra-adjudicated design; actual revised Claude Fable 5.1 council activation evidence | — | — |
| 1 Foundation | PineGuard | `temporal-01-foundation` | 10 | U4 after U0; U3 after U1; U4a after U4; U2b after U2; U5 after U2/U3 | Phase 0 |
| 2 Time-zone Core | PineGuard | `temporal-02-timezone` | 8 | U0 ∥ U1 ∥ U2 first; U3 after U0/U1/U2; U4 after U3; U5 ∥ U6 after U4 | Phase 1 merged |
| 3 Release + Standards | both | PineGuard: none (release from `main`); Standards: `temporal-timezone` | 7 | U3 fans out per domain | Phase 2 merged |
| 4 Validity + relative | PineGuard | `temporal-04-validity` | 7 | U1 ∥ U2 first; U3a/b/c after U1/U2; matching U4/U5 adapters after U3; U6 last | Phase 2 merged (not Phase 3) |
| 5 Durations + calendar | PineGuard | `temporal-05-calendar` | 7 | U1 ∥ U2a ∥ U2b first; U3a after U1/U2a, U3b-n after U1/U2b; matching adapters after U3; U6 last | Phase 4 merged |
| 6 Formats + epoch | PineGuard | `temporal-06-formats` | 6 | U1/U2 after Phase 5 merge; U4 ∥ U5 after U3 | Phase 5 merged |
| 7 Close-out | both | `temporal-07-closeout` (both repos) | 6 | U1 ∥ U2 | Phases 1–6 merged |

Phases 2, 4, 5 and 6 run in dependency order **2 → 4 → 5 → 6** because their
`MustCodes.*.cs` ownership overlaps. A later phase starts only after its predecessor merges.
Phase 3 may proceed after Phase 2 independently of Phase 4 where its declared file ownership
permits. Never rebase a pushed branch; merge `origin/main` into the worktree instead.
[Fable corrective council; A-01–A-12 remain final]

## 3. Decisions register (Astra final revision)

The owner authorized Astra to make final decisions for this review. D-rows retain prior council
context where unchanged; A-xx overrides are authoritative for this revision. No further owner
signature is needed. Earlier council approval does not prove the revised council ran: the required
Claude Fable 5.1 rerun completed with corrective findings; its actual transcript/receipt
records the run and controlling corrections. Final Fable acceptance gates activation.
Exceptional reopening follows section 0, not model preference. [A-01]
| Id | Decision | Recommendation | Rejected alternatives | Status |
|---|---|---|---|---|
| D-1 | Calendar today for DateOnly and DateTime age. | Provider LocalTimeZone defines today, derived from one GetUtcNow sample via foundation TryGetTodayDate and checked TryGetDateAtOffset. Do not route predicates through unchecked GetLocalNow. DateTime input is own Date with Kind ignored; no host conversion. Reference projection failure is date.reference.out-of-range attributed timeProvider. Fixed-clock foundation lands first. [A-03, A-07] | UTC calendar today, implicit host conversion, out-of-range reference dates. | Astra final under owner authorization |
| D-2 | Which calendar `DateTimeRules` calendar predicates use (F-5). | Change `DateTimeRules.IsSameDay(DateTime?, DateTime?)` to compare `a.Value.Date == b.Value.Date` (Kind ignored), so every DateTimeRules calendar predicate uses the value's own wall-clock date. Document "Kind is ignored" in the `<summary>`. Add `IsSameDay(DateTimeOffset, DateTimeOffset, TimeZoneInfo timeZone)` in Phase 2 as written. | Keep UTC comparison and document — the only defence is back-compat, forbidden pre-1.0. | Recommended (council 2026-09-26: REJECT — replaced) |
| D-3 | OS zone lookup inside Core versus the "no IO in Core" invariant. | **Astra override A-05:** preserve spec §5.3. Core accepts supplied `TimeZoneInfo`; remove the Core host-ID lookup family and proposed broad spec exception. Existing composition/DA boundaries resolve host IDs once; consumers can use BCL resolution directly. Host resolvability, versioned snapshot membership and mapping remain distinct. Runtime/OS/globalization behavior is verified at composition boundaries; netstandard2.1 is a compile surface, not a runtime guarantee. | A Core host lookup with a generic culture/environment exception violates the Brain. A satellite package is unnecessary. Snapshot membership cannot answer host resolvability. A separate host predicate proposal is deferred pending concrete demand and Claude Fable 5.1 review/council; no exception is authorized by this plan. | Astra final recommendation; revised real Claude Fable 5.1 council and corrective acceptance completed; P0 READY FOR IMPLEMENTATION (`temporal-final-fable-acceptance-2.md`) |
| D-4 | Public `PineGuard.Testing` clock double. | Keep `FixedTimeProvider`. New ctor `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)`; `LocalTimeZone` is overridden and returns `localTimeZone ?? TimeZoneInfo.Utc`; fall-through to the host zone is forbidden and tested. Add `DefaultBehindUtc` beside §6.3's `DefaultAheadOfUtc`; every today-relative rule has at least one midnight-boundary vector in each. Lands as its own commit before D-1. Document Microsoft's `FakeTimeProvider` as the recommended consumer choice for advancing clocks; PineGuard's double stays frozen-instant only. | Replace with `FakeTimeProvider` dependency — adds `Microsoft.Extensions.TimeProvider.Testing` to a published package and breaks Standards. Extend with a settable `LocalTimeZone` that defaults to the host zone — F-3 restated. | Recommended (council 2026-09-26: AMEND) |
| D-5 | IANA↔Windows conversion in Core. | **No.** Conversion stays in Standards (versioned CLDR snapshot, deterministic, territory-aware). Core wraps nothing here. `TimeZoneInfo.TryConvertIanaIdToWindowsId` is available on PineGuard's .NET 8+ targets (currently `net8.0` and `net10.0`); it is absent from the `netstandard2.1` compile-time API surface. Code targeting `netstandard2.1` uses Standards' versioned CLDR mapping path. Host BCL conversion remains fallible and subject to ICU/NLS/invariant-globalization caveats in §6.4. [A-02, A-05 framework clarification] | Wrap the BCL call in `TimeZoneUtility` — two sources of truth that disagree by ICU version. | Astra-approved factual clarification; conversion direction unchanged |
| D-6 | Fate of `IsWithinDaysFromNow` (absolute distance, either direction, `DateTime` only). | Keep unchanged until Phase 4. Phase 4 ships `WithinLast`/`WithinNext` and deletes `IsWithinDaysFromNow`/`NotWithinDaysFromNow` at every layer in the same unit (verify no Standards reference during the F-11 pass). Until then its `<summary>` first sentence reads "in the past or the future". | Rename to `IsWithinDaysOfNow` — churn with no semantic gain. Keep permanently — reads future-only; either direction is composable. | Recommended (council 2026-09-26: AMEND) |
| D-7 | Standards host resolver versus catalog membership. | Astra override A-12: preserve existing Standards SystemTimeZone names, behavior and diagnostics; no competing Core host family ships. IANA/catalog membership stays versioned and separate. Do not rename a host-backed API KnownTimeZone or claim it consults a snapshot. | Fake snapshot semantics and unnecessary rename/collision rationale. | Astra final under owner authorization |
| D-8 | tzdata `backward` links in Standards. | Ingest `backward` (and `etcetera` if `Etc/GMT±n` are absent); generate `DefaultIanaTimeZoneLinkData.cs`. `IsIanaTimeZoneId` / `Must.Be.IanaTimeZoneId` accept canonical ids AND links (a link is a valid tzdata id). Add strict opt-in `IsCanonicalIanaTimeZoneId` / `Must.Be.CanonicalIanaTimeZoneId` / `Guard.Against.NotCanonicalIanaTimeZoneId`, message `"{paramName} must be a canonical IANA time zone id."`; code constant is a T-03 U2 discovery item (in the existing Standards IANA code family, condition `not-canonical`) (the Standards IANA code family was not read in this session). Keep `IIanaTimeZoneProvider.TryResolveLink(string? value, out string? canonicalId)`; document it as the canonicalisation path. Own unit in the F-11 pass. | Canonical-only default with `*OrLink` clauses (`IanaTimeZoneRules.IsIanaTimeZoneId(value, provider)` staying canonical-only, `IsIanaTimeZoneIdOrLink`, `Must.Be.IanaTimeZoneIdOrLink`) — rejects `Europe/Oslo` et al. (2022b `backward` links). A `bool acceptLinks` parameter — boolean parameters read badly at call sites. An enum `IanaIdMatch` — one enum for one method. `OrAlias` suffix family — same defect, different word. | Recommended (council 2026-09-26: REJECT — replaced) |
| D-9 | `GuardTimeSpanClauses` boundary polarity (F-6). | Fix as originally specified: the two threshold guards call the complement Must clause with the **complemented** `inclusion` (`Inclusive ↔ Exclusive`), so the boundary passes exactly as in `GuardNumberClauses` (`Guard.Against.LessThan` → `Must.Be.GreaterThanOrEqual`); the exact mapping is in T-01 U4b. Land it as a standalone commit BEFORE the T-01 rename, with a failing test first (`Guard.Against.LessThan(v, v)` must not throw). Sweep every threshold guard in every `Guard*Clauses` class with a `v == t` boundary test; F-6 audited TimeSpan and Number only. | Leave it — a `Guard.Against.ShorterThan(v, t)` that throws on `v == t` is a boundary defect shipped under a new name. Fix inside the rename diff — dirty bisect. | Recommended (council 2026-09-26: AMEND) |
| D-10 | Routines and the "no PRs, direct `merge --no-ff` to `main`" standing rule. | Routines push `claude/temporal-<unit>` branches only, rebased on main before push, and never edit the tracker; the owner updates the tracker on main at merge. Routines never open PRs. | Let routines open PRs — contradicts the 2026-08-30 standing rule. Per-branch tracker edits — conflict when units run in parallel. | Recommended (council 2026-09-26: AMEND) |
| D-11 | Council evidence and activation. | Earlier council remains historical evidence. Current revised design is Astra-adjudicated under owner authorization; no owner row-signing is required. The actual revised Claude Fable 5.1 council completed; its real transcript records corrective findings. Final Fable acceptance of these corrections is mandatory before activation. | Implied or fabricated rerun approval. | Design finalized; execution gate pending |
| D-13 | Required zone configuration. | Must returns Zone.Null failure attributed nameof(timeZone) before value/null equality or negation; Guards delegate. Fluent checks zone at composition. DA checks the ID constructor and resolves it once at the existing adapter boundary; unknown ID is passed as a resolution outcome to Must.Be.ResolvedTimeZone; Must owns Id.Unknown and its message, with no Core/Must lookup. | Must throwing on null zone, naive complement success, nonexistent Core resolver. | Astra final A-05/A-10 |
| D-14 | DTO calendar age basis. | Age uses value.Date and one sampled clock instant projected to the value offset through foundation TryGetDateAtOffset, then shared TryGetAge completed years. Guard representability before projection; failure is provider reference diagnostic, never clamp or unchecked ToOffset. This fixed-offset convention is not named-zone/DST or legal/regional age. New instant predicates use actual instants and separate rules. No zone-aware age overload is promised. [A-03, A-07] | Mix caller offset with provider zone, threshold AddYears overflow, machine-date conversion. | Astra final under owner authorization |
| D-15 | Which new Must clauses get a `Not*` pair. | **State predicates** get a pair, matching `FirstDayOfMonth`/`NotFirstDayOfMonth`: `LeapYear`, `LeapDay`, `Midnight`, `OnDayOfWeek`, `OnInterval`, `WholeSeconds`, `WholeMinutes`, `MultipleOf`, `BusinessDay`, `SameMonth`/`SameYear`/`SameIsoWeek`/`SameDay(zone)`, `WithinLast`, `WithinNext`, `OlderThan`, `Expired`/`NotExpired`, `FirstDayOfQuarter`, `LastDayOfQuarter`. `WithinLast`/`WithinNext` keep their pairs per the existing precedent `NotWithinDaysFromNow`. **Constraints** get none, matching `MinimumAge`: `Effective`, `WithinValidity`, `MinimumAge`/`MaximumAge`/`AgeBetween`, `Timeout`, the sign family (explicit complements exist), `UtcOffset`, `ValidLocalTime`, `UnambiguousLocalTime`, `InTimeZone`, `Iso8601*`/`Rfc*`, `UnixSeconds`/`UnixMilliseconds`. §4.5/§5 list the pairs and their codes. | Pair everything — `NotEffective`, `NotTimeout` read as nonsense at call sites. `Unexpired` — violates the `Not*` canon (`docs/ai/specs/guard-clauses/project.md:205`). Drop pairs on comparisons (`WithinLast`/`WithinNext`/`OlderThan`) — contradicts the existing precedent `NotWithinDaysFromNow`. | Recommended (council 2026-09-26: AMEND) |

### 3.1 Historical council and current revision

D-15's state-predicate list also includes DaylightSavingTime / NotDaylightSavingTime.
The Fable scope-fidelity follow-up preserves the planned Core capability by adding its
meaningful Must/Guard/Fluent/DA consumers and tests; the earlier rule-removal proposal is
withdrawn. No unused rule or audit exemption remains.

The earlier council transcript remains historical evidence. This revision preserves its applicable
civil-date, fixed-clock, guard-polarity, alias-default and vocabulary outcomes while Astra A-05
overrides Core host lookup/spec exception and A-12 overrides the Standards rename/delegation.
Current phase instructions and the A-register are authoritative; the revised actual Fable council
completed with corrective findings; final Fable acceptance gates activation. No owner re-signing is needed. [A-01]
## 4. Naming canon

Conventions the table obeys (all are existing repo precedent, verified 2026-09-22):

- Core rules are `Is*` / `Has*` predicates on `<Type>Rules`; string variants live on nested
  `StringRules.<Type>`; parsing lives on `StringUtility.<Type>.TryParse`.
- The only Must entry point is `Must.Be` (650 uses; `Must.Have` does not exist).
- Guard names describe the **bad state** and call the **positive** Must clause
  (`Guard.Against.Past` → `Must.Be.FutureOrPresent`; `Guard.Against.NotEmail` → `Must.Be.Email`;
  `Guard.Against.BelowMinimumAge` → `Must.Be.MinimumAge`).
- Fluent extension = Must clause name; DataAnnotations attribute = `<Clause><Type>Attribute`
  (`PastDateTimeOffsetAttribute`).
- Message template = `"{paramName} must <condition>."` (Must owns it; Guard/Fluent/DA reuse it).
- Negation prefix is `Not*`, never `Non*` / `Invalid*` (future-language.md §7).
- Codes follow `<domain>.<aspect>.<condition>`; the code named on a clause is the **failure** it
  reports (`Past` reports `date.relative.not-past`).

### 4.1 Phase 1 — renames and fixes

| Public Testing type | Namespace / file / producer | Contract | Rejected alternatives |
|---|---|---|---|
| `CountingTimeProvider` | `PineGuard.Testing.Common`; `tests/PineGuard.Testing/Common/CountingTimeProvider.cs`; T-01 U0 | Sealed TimeProvider; ctor `(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)`; read-only UtcNowCallCount, fixed UTC-normalized instant, zone default UTC | Unnamed private copies; replacement of FixedTimeProvider; production clock abstraction. Fable final detail #4. |
| `AdvancingTimeProvider` | `PineGuard.Testing.Common`; `tests/PineGuard.Testing/Common/AdvancingTimeProvider.cs`; T-01 U0 | Sealed TimeProvider; ctor `(IReadOnlyList<DateTimeOffset> sequence, TimeZoneInfo? localTimeZone = null)`; read-only UtcNowCallCount, UTC-normalized sequence, throws InvalidOperationException on exhaustion, zone default UTC | Hidden repeated reads; timer virtualization; source-package use. Fable final detail #4. |

| Core rule (after) | Was | Must | Guard (calls) | Message | Code | Rejected |
|---|---|---|---|---|---|---|
| `TimeSpanRules.IsLongerThan(value, threshold, inclusion)` | `IsGreaterThan` | `Must.Be.LongerThan` | `Guard.Against.ShorterThan` → `Must.Be.LongerThan(value, threshold, inclusion: complement)` | `{paramName} must be longer than the threshold.` | `time.duration.not-longer` (was `not-greater`) | `IsAtLeast` (inclusive-only reading) |
| `TimeSpanRules.IsShorterThan(value, threshold, inclusion)` | `IsLessThan` | `Must.Be.ShorterThan` | `Guard.Against.LongerThan` → `Must.Be.ShorterThan(value, threshold, inclusion: complement)` | `{paramName} must be shorter than the threshold.` | `time.duration.not-shorter` (was `not-less`) | `IsAtMost` |
| Internal `DateTimeUtility.TryGetTodayDate(tp, out DateTime date)` | UTC-date reference | U2b captures checked reference once | Delegates | Reference-range configuration message | `Date.Reference.OutOfRange` | Public GetToday wrappers; unchecked projection [A-03, A-05, A-07] |
| `TimeOnlyRules.IsBetween(value, start, end, inclusion)` (wraps when `start > end`) | linear; min/max arguments | Remove reversed-bound misuse guard in TimeOnly Between/NotBetween only | Delegate wrap semantics; start/end arguments | Existing semantic messages; update argument wording | Existing semantic codes; shared range codes remain for other families | Separate window family — rejected; A-09 remains unchanged. Controlling Fable correction. |
| `DateTimePrecision.Microsecond`, `DateTimePrecision.Tick`, `TimePrecision.Microsecond` | absent | — | — | — | — | — |
| `DateTimeRules.IsSameDay(DateTime?, DateTime?)` (`.Date` comparison, Kind ignored) | UTC-date comparison | unchanged | unchanged | unchanged | unchanged | Keep UTC comparison — back-compat argument, forbidden pre-1.0 (council D-2, 2026-09-26). |

Inclusion complement: `Inclusive` ↔ `Exclusive`. `Guard.Against.ShorterThan(v, t, Inclusion.Exclusive)`
("throw if strictly shorter") calls `Must.Be.LongerThan(v, t, Inclusion.Inclusive)`.

### 4.2 Phase 2 — time-zone Core

Outcome-only configuration clause: `Must.Be.ResolvedTimeZone(TimeZoneInfo? resolvedZone,
string? timeZoneId, paramName)` in MustTimeZoneClauses; no lookup. DA resolves once with
the target-available `TryFindSystemTimeZoneById`, then invokes this clause before its
semantic clause. Non-null ID plus null outcome emits existing TimeZone.Id.Unknown,
`ParamName = nameof(timeZoneId)`, failing value supplied ID, exact template
`"Time zone ID '{paramName}' is not a known time zone."`. Null ID emits new
`MustCodes.TimeZone.Id.Null` (`timezone.id.null`) with the existing null template first,
attributed to `nameof(timeZoneId)`. No host lookup or audit exemption is introduced.
[Controlling Fable audit correction; supersedes only correction #5's cross-domain null-code
pick. A-05/A-10 semantics and A-12 attribution/templates remain unchanged.]

| Core | Must | Guard (calls) | Fluent | DA attribute | Message | Code | Rejected |
|---|---|---|---|---|---|---|---|
| `TimeZoneUtility.TryGetUtcOffset(TimeZoneInfo? tz, DateTimeOffset instant, out TimeSpan offset)` | — | — | — | — | — | — | — |
| `TimeZoneRules.IsUtcOffset(TimeSpan?)` (whole minutes, −14:00 ≤ v ≤ +14:00) | `Must.Be.UtcOffset(TimeSpan)` | `Guard.Against.NotUtcOffset` → `UtcOffset` | `.UtcOffset()` on `TimeSpan` / `TimeSpan?` | `UtcOffsetTimeSpanAttribute` | `{paramName} must be a valid UTC offset.` | `timezone.offset.invalid` | `IsOffset` (ambiguous) |
| `StringRules.UtcOffset.IsUtcOffset(string?)`; `StringUtility.UtcOffset.TryParse(string?, out TimeSpan?)` accepts `Z`, `z`, `±HH:mm`, `±HHmm`, `±HH` | `Must.Be.UtcOffset(string?)` → `MustResult<TimeSpan>` | `Guard.Against.NotUtcOffset(string?)` → `TimeSpan` | `.UtcOffset()` on `string?` | `UtcOffsetTimeSpanStringAttribute` (pattern `<Clause><Type>StringAttribute`, as `PastDateOnlyStringAttribute`) | same | same | — |
| `TimeZoneRules.IsInvalidTime(DateTime?, TimeZoneInfo?)` | `Must.Be.ValidLocalTime(DateTime, TimeZoneInfo)` | `Guard.Against.InvalidLocalTime` → `ValidLocalTime` | `.ValidLocalTime(TimeZoneInfo)` on `DateTime` / `DateTime?` | `ValidLocalTimeDateTimeAttribute(string timeZoneId)` | `{paramName} must be a valid local time in the time zone.` | `timezone.local-time.invalid` | `NotInDstGap` (jargon), `Exists` (vague) |
| `TimeZoneRules.IsAmbiguousTime(DateTime?, TimeZoneInfo?)` | `Must.Be.UnambiguousLocalTime(DateTime, TimeZoneInfo)` | `Guard.Against.AmbiguousLocalTime` → `UnambiguousLocalTime` | `.UnambiguousLocalTime(TimeZoneInfo)` | `UnambiguousLocalTimeDateTimeAttribute(string timeZoneId)` | `{paramName} must be an unambiguous local time in the time zone.` | `timezone.local-time.ambiguous` | `Unique` (vague) |
| `TimeZoneRules.HasOffsetOf(DateTimeOffset?, TimeZoneInfo?)` | `Must.Be.InTimeZone(DateTimeOffset, TimeZoneInfo)` | `Guard.Against.NotInTimeZone` → `InTimeZone` | `.InTimeZone(TimeZoneInfo)` on `DateTimeOffset` / `DateTimeOffset?` | `InTimeZoneDateTimeOffsetAttribute(string timeZoneId)` | `{paramName} must be expressed in the time zone.` | `timezone.offset.mismatch` | `OffsetConsistentWith` (mechanism), `HasZoneOffset` |
| `TimeZoneRules.IsDaylightSavingTime(DateTimeOffset?, TimeZoneInfo?)` | `DaylightSavingTime` / `NotDaylightSavingTime` | `NotDaylightSavingTime` → `DaylightSavingTime`; `DaylightSavingTime` → `NotDaylightSavingTime` | `.DaylightSavingTime(TimeZoneInfo)`, `.NotDaylightSavingTime(TimeZoneInfo)` on DTO/DTO? | `DaylightSavingTimeDateTimeOffsetAttribute(timeZoneId)` / `NotDaylightSavingTimeDateTimeOffsetAttribute(timeZoneId)` | `{paramName} must be in daylight saving time in the time zone.` / `{paramName} must not be in daylight saving time in the time zone.` | `timezone.daylight-saving.not-in-effect` / `timezone.daylight-saving.in-effect` | Rule-only surface — rejected by binding Fable scope-fidelity follow-up |
| `TimeZoneRules.IsSameDay(DateTimeOffset?, DateTimeOffset?, TimeZoneInfo?)` | `MustTimeZoneClauses.SameDay(DateTimeOffset, DateTimeOffset, TimeZoneInfo)` / `NotSameDay` via `Must.Be` | `GuardTimeZoneClauses.NotSameDay` → `SameDay`; `SameDay` → `NotSameDay` | `FluentTimeZoneExtensions.SameDay(DateTimeOffset other, TimeZoneInfo)` / `NotSameDay` with literal `other` and nullable overloads | — (cross-property) | `{paramName} must be the same day in the time zone.` / `{paramName} must not be the same day in the time zone.` | `timezone.equality.not-same-day` / `timezone.equality.same-day` | `SameLocalDay` (the zone parameter already says it) |

### 4.3 Standards integration [A-04, A-05, A-12]

Preserve current Standards SystemTimeZone clauses/utility names, host-resolution behavior and
owning diagnostics. Core introduces no competing host-ID family and Standards delegates to no
new Core resolver. Bundled IANA membership accepts Zone IDs and valid Links from the pinned
dataset; CanonicalIanaTimeZoneId is strict opt-in, with owning code iana.timezone.not-canonical.
Host resolvability, packaged membership and territory-aware mapping remain distinct. Download-only
reference refresh follows T-03 and the local manifest; generated classes are a separate explicitly
authorized implementation step. No KnownTimeZone rename is prescribed.
### 4.4 Phase 4 — validity and relative time

`T` below is each of `DateTimeOffset`, `DateTime`; `DateOnly` variants take `int days` instead of
`TimeSpan` and compare calendar dates against `captured referenceDate from internal TryGetTodayDate` (D-1). `DateTimeOffset` age rules
(`MinimumAge`/`MaximumAge`/`AgeBetween`) compute "today" as `checked TryGetDateAtOffset(sample, value.Offset, out referenceDate)`
(council D-14, 2026-09-26), not via `captured referenceDate from internal TryGetTodayDate`.

| Core (`<T>Rules`) | Must | Guard (calls) | Fluent / DA | Message | Code | Rejected |
|---|---|---|---|---|---|---|
| `IsExpired(T? value, TimeProvider? tp)` ⇔ `value ≤ now` (`DateOnly`: `today > value`) | `Must.Be.NotExpired(value, tp)` / `Must.Be.Expired(value, tp)` | `Guard.Against.Expired` → `NotExpired`; `Guard.Against.NotExpired` → `Expired` | `.NotExpired(tp)`, `.Expired(tp)`; `NotExpired<T>Attribute`, `Expired<T>Attribute` | `{paramName} must not be expired.` / `{paramName} must be expired.` | `date.validity.expired` / `date.validity.not-expired` | `Alive`, `Current` (vague); `Unexpired` as the positive clause name — violates the `Not*` canon (council D-15, 2026-09-26) |
| `IsEffective(T? notBefore, TimeProvider? tp)` ⇔ `notBefore ≤ now` (`DateOnly`: `notBefore ≤ today`) | `Must.Be.Effective(value, tp)` | `Guard.Against.NotEffective` → `Effective` | `.Effective(tp)`; `Effective<T>Attribute` | `{paramName} must be effective.` | `date.validity.not-effective` | `Active`, `Started`, `Commenced` (domain-specific), `NotYetValid` (negative); no `NotEffective` Must clause (D-15) |
| `IsWithinValidity(T? notBefore, T? notAfter, TimeProvider? tp)` ⇔ `notBefore ≤ now < notAfter`; either null → false | `Must.Be.WithinValidity(notBefore, notAfter, tp)` → `MustResult<T>` carrying `notBefore` | `Guard.Against.OutsideValidity` → `WithinValidity` | `.WithinValidity(T notAfter, tp)` on the `notBefore` property with a literal `notAfter` (no model-expression overloads in this program, §12); **no DA attribute** (cross-property) | `{paramName} must be within its validity period.` | `date.validity.outside` | `ValidAt`, `CurrentlyValid` |
| `IsWithinLast(T? value, TimeSpan span, TimeProvider? tp)` ⇔ `span ≥ 0 && now − span ≤ value ≤ now` | `Must.Be.WithinLast(value, span, tp)` / `NotWithinLast` | `Guard.Against.NotWithinLast` → `WithinLast`; `Guard.Against.WithinLast` → `NotWithinLast` | `.WithinLast(span, tp)`; `WithinLast<T>Attribute(int days)` | `{paramName} must be within the expected period before now.` | `date.relative.not-within-last` / `date.relative.within-last` | `Recent` (no span), `Since` |
| `IsWithinNext(T? value, TimeSpan span, TimeProvider? tp)` ⇔ `span ≥ 0 && now ≤ value ≤ now + span` | `Must.Be.WithinNext` / `NotWithinNext` | `Guard.Against.NotWithinNext` → `WithinNext`; `Guard.Against.WithinNext` → `NotWithinNext` | `.WithinNext(span, tp)`; `WithinNext<T>Attribute(int days)` | `{paramName} must be within the expected period after now.` | `date.relative.not-within-next` / `date.relative.within-next` | `Upcoming`, `Soon` |
| `IsOlderThan(T? value, TimeSpan age, TimeProvider? tp)` ⇔ `age ≥ 0 && value < now − age` | `Must.Be.OlderThan(value, age, tp)` / `NotOlderThan` | `Guard.Against.NotOlderThan` → `OlderThan`; `Guard.Against.OlderThan` → `NotOlderThan` | `.OlderThan(age, tp)`; `OlderThan<T>Attribute(int days)` | `{paramName} must be older than the expected age.` | `date.relative.not-older-than` / `date.relative.older-than` | `Stale` (judgemental), `NewerThan` (dropped: equals `WithinLast` for past values) |
| `HasMaximumAge(T? value, int years, TimeProvider? tp)` ⇔ age ≤ years (`DateOnly`, `DateTime`, `DateTimeOffset`) | `Must.Be.MaximumAge(value, years, tp)` | `Guard.Against.AboveMaximumAge` → `MaximumAge` | `.MaximumAge(years, tp)`; `MaximumAge<T>Attribute(int years)` | `{paramName} must not exceed the expected maximum age.` | `date.age.above-maximum` | `AgeAtMost` |
| `IsAgeBetween(T? value, int minYears, int maxYears, TimeProvider? tp)` | `Must.Be.AgeBetween(value, min, max, tp)` | `Guard.Against.AgeOutOfRange` → `AgeBetween` | `.AgeBetween(min, max, tp)`; `AgeBetween<T>Attribute(int min, int max)` | `{paramName} must be within the expected age range.` | `date.age.out-of-range` | — |
| `DateTimeOffsetRules.HasMinimumAge` (computes "today" as `checked TryGetDateAtOffset(sample, value.Offset, out referenceDate)`, council D-14, 2026-09-26), `DateTimeOffsetRules.IsSameDay(a, b)` (UTC dates) | `Must.Be.MinimumAge(DateTimeOffset, …)`, `Must.Be.SameDay(DateTimeOffset, DateTimeOffset)` / `NotSameDay` | `BelowMinimumAge`, `NotSameDay` / `SameDay` | parity with the `DateTime` family | existing messages | existing codes | — |
| `DateTimeUtility.TryGetAge(DateOnly? birthDate, DateOnly asOf, out int years)` (net8+), `TryGetAge(DateTime? birthDate, DateTime asOf, out int years)` | — | — | — | — | — | `GetAge` (throws), `CalculateAge` |

### 4.5 Phase 5 — durations and calendar

| Core | Must | Guard (calls) | Fluent / DA | Message | Code | Rejected |
|---|---|---|---|---|---|---|
| `TimeSpanRules.IsPositive/IsNegative/IsZero/IsZeroOrPositive/IsZeroOrNegative(TimeSpan?)` | `Positive/Negative/Zero/NotZero/ZeroOrPositive/ZeroOrNegative(TimeSpan)` | mirror `GuardNumberClauses` polarity exactly (`ZeroOrNegative` → `Positive`, `Negative` → `ZeroOrPositive`, `NotZero` → `Zero`, `Zero` → `NotZero`, `Positive` → `ZeroOrNegative`, `ZeroOrPositive` → `Negative`) | `.Positive()` …; `PositiveTimeSpanAttribute` … | `{paramName} must be positive.` (mirror `MustNumberClauses` wording) | `time.duration.not-positive`, `.not-negative`, `.not-zero`, `.zero`, `.negative`, `.positive` | — |
| `TimeSpanRules.IsWholeSeconds`, `IsWholeMinutes(TimeSpan?)` | `WholeSeconds`/`NotWholeSeconds`, `WholeMinutes`/`NotWholeMinutes` | `NotWholeSeconds` → `WholeSeconds`; `WholeSeconds` → `NotWholeSeconds` (same for minutes) | `.WholeSeconds()`, `.NotWholeSeconds()`; `WholeSecondsTimeSpanAttribute`, `NotWholeSecondsTimeSpanAttribute` (and minutes) | `{paramName} must be a whole number of seconds.` / `must not be` | `time.duration.not-whole-seconds` / `.whole-seconds`, `.not-whole-minutes` / `.whole-minutes` | `NoFractionalSeconds` |
| `TimeSpanRules.IsMultipleOf(TimeSpan? value, TimeSpan unit)` (unit > 0) | `MultipleOf(value, unit)` / `NotMultipleOf` | `NotMultipleOf` → `MultipleOf`; `MultipleOf` → `NotMultipleOf` | `.MultipleOf(unit)`, `.NotMultipleOf(unit)`; `MultipleOfTimeSpanAttribute(int minutes)`, `NotMultipleOfTimeSpanAttribute(int minutes)` | `{paramName} must be a multiple of the expected unit.` / `must not be` | `time.duration.not-multiple` / `.multiple` | `Divisible` |
| `TimeSpanRules.IsTimeout(TimeSpan?)` ⇔ `value == Timeout.InfiniteTimeSpan \|\| value >= TimeSpan.Zero` | `Timeout(value)` | `NotTimeout` → `Timeout` | `.Timeout()`; `TimeoutTimeSpanAttribute` | `{paramName} must be a non-negative or infinite timeout.` | `time.duration.not-timeout` | `PositiveOrInfinite` (mechanism). Note: inside `MustTimeSpanClauses` the clause name shadows `System.Threading.Timeout`; XML `cref`s must be fully qualified `System.Threading.Timeout.InfiniteTimeSpan`. |
| `<T>Rules.IsLeapYear`, `IsLeapDay` (`DateOnly`, `DateTime`, `DateTimeOffset`) | `LeapYear`/`NotLeapYear`, `LeapDay`/`NotLeapDay` | `NotLeapYear` → `LeapYear`; `LeapYear` → `NotLeapYear` (same for leap day) | `.LeapYear()`, `.NotLeapYear()`; `LeapYear<T>Attribute`, `NotLeapYear<T>Attribute` (and leap day) | `{paramName} must be in a leap year.` / `must not be`; `must be a leap day.` / `must not be` | `date.calendar.not-leap-year` / `.leap-year`, `.not-leap-day` / `.leap-day` | — |
| `<T>Rules.IsSameMonth(a, b)`, `IsSameYear(a, b)`, `IsSameIsoWeek(a, b)` | `SameMonth`/`NotSameMonth`, `SameYear`/`NotSameYear`, `SameIsoWeek`/`NotSameIsoWeek` | `NotSameMonth` → `SameMonth` etc. | Fluent with a literal `other` argument (as `SameDay`); no DA | `{paramName} must be in the same month.` / `year` / `ISO week` | `date.equality.not-same-month` / `.same-month`, `.not-same-year` / `.same-year`, `.not-same-iso-week` / `.same-iso-week` | `SameWeek` (ambiguous: ISO vs culture) |
| `<T>Rules.IsDayOfWeek(value, DayOfWeek day)` | `OnDayOfWeek(value, day)` / `NotOnDayOfWeek` | `NotOnDayOfWeek` → `OnDayOfWeek`; `OnDayOfWeek` → `NotOnDayOfWeek` | `.OnDayOfWeek(day)`, `.NotOnDayOfWeek(day)`; `OnDayOfWeek<T>Attribute(DayOfWeek day)`, `NotOnDayOfWeek<T>Attribute(DayOfWeek day)` | `{paramName} must fall on the expected day of the week.` / `must not fall on` | `date.calendar.not-day-of-week` / `.day-of-week` | `DayOfWeek` (shadows the type name at call sites) |
| `<T>Rules.IsFirstDayOfQuarter`, `IsLastDayOfQuarter` | `FirstDayOfQuarter`/`NotFirstDayOfQuarter`, `LastDayOfQuarter`/`NotLastDayOfQuarter` | complements | `.FirstDayOfQuarter()`; attributes | `{paramName} must be the first day of the quarter.` | `date.calendar.not-first-day-of-quarter`, `.first-day-of-quarter`, `.not-last-day-of-quarter`, `.last-day-of-quarter` | `QuarterStart` |
| `DateTimeRules.IsMidnight`, `DateTimeOffsetRules.IsMidnight` (`TimeOfDay == Zero`) | `Midnight` / `NotMidnight` | `NotMidnight` → `Midnight`; `Midnight` → `NotMidnight` | `.Midnight()`, `.NotMidnight()`; `MidnightDateTimeAttribute`, `NotMidnightDateTimeAttribute` (and `DateTimeOffset`) | `{paramName} must be midnight.` / `must not be midnight.` | `date.calendar.not-midnight` / `.midnight` | `HasNoTimeComponent` (long), `DateOnlyValue` |
| `TimeOnlyRules.IsOnInterval(TimeOnly? value, TimeSpan interval)`, `DateTimeRules.IsOnInterval`, `DateTimeOffsetRules.IsOnInterval` (time-of-day ticks % interval == 0; interval > 0) | `OnInterval(value, interval)` / `NotOnInterval` | `NotOnInterval` → `OnInterval`; `OnInterval` → `NotOnInterval` | `.OnInterval(interval)`, `.NotOnInterval(interval)`; `OnInterval<T>Attribute(int minutes)`, `NotOnInterval<T>Attribute(int minutes)` | `{paramName} must fall on the expected interval.` / `{paramName} must not fall on the expected interval.` | TimeOnly: `time.interval.off-interval` / `.on-interval`; DateTime/DateTimeOffset: `date.interval.off-interval` / `.on-interval` | `Aligned`, `OnBoundary` |
| `DateOnlyRules.IsBusinessDay(DateOnly? value, IReadOnlyCollection<DateOnly>? holidays = null, IReadOnlyCollection<DayOfWeek>? weekend = null)`; `DateTimeRules` / `DateTimeOffsetRules` overloads take `IReadOnlyCollection<DateTime>? holidays` compared by `.Date` | `BusinessDay(value, holidays, weekend)` / `NotBusinessDay` | `NotBusinessDay` → `BusinessDay`; `BusinessDay` → `NotBusinessDay` | `.BusinessDay(holidays, weekend)`; `BusinessDay<T>Attribute` (default weekend, no holidays — documented) | `{paramName} must be a business day.` | `date.calendar.not-business-day` / `.business-day` | `WorkingDay` (British/Australian only), `Weekday` (exists, ignores holidays) |

Default `weekend` is `[DayOfWeek.Saturday, DayOfWeek.Sunday]`; `null` holidays means none. Holiday
registries stay in the Standards repo (out of scope here).

### 4.6 Phase 6 — formats and epoch

| Core | Must | Guard (calls) | Fluent / DA | Message | Code | Rejected |
|---|---|---|---|---|---|---|
| `StringUtility.DateTime.TryParse(string?, out DateTime?, DateTimeStyles = AdjustToUniversal \| AssumeUniversal \| AllowWhiteSpaces)` — `RoundtripKind` combined with `AssumeUniversal` throws `ArgumentException` for `DateTime`, so every result is `Kind.Utc`; `StringRules.DateTime.IsInPast / IsInFuture / IsBetween` mirroring `StringRules.DateTimeOffset` member-for-member (no parse-only `IsDateTime`: the DTO family has none and the name would shadow the type) | `MustStringDateTimeClauses` mirroring `MustStringDateTimeOffsetClauses` member-for-member (`PastDateTime`, `FutureDateTime`, `PastOrPresentDateTime`, `FutureOrPresentDateTime`, `BetweenDateTime`, …) | mirror `GuardStringDateTimeOffsetClauses` | mirror Fluent/DA string DateTimeOffset families | existing string-family messages with `date/time` wording | existing codes | — |
| `StringRules.DateTimeOffset.IsIso8601(string?)` | `Must.Be.Iso8601DateTimeOffset(string?)` → `MustResult<DateTimeOffset>` | `NotIso8601DateTimeOffset` | `.Iso8601DateTimeOffset()`; `Iso8601DateTimeOffsetStringAttribute` | `{paramName} must be an ISO 8601 date/time.` | `date.format.not-iso8601` | `IsoDateTime` |
| `StringRules.DateTimeOffset.IsRfc3339(string?)` (supported representable profile: ASCII, T/t and Z/z, seconds/offset mandatory, 0 or 1–7 fraction digits; no whitespace/space, leap seconds or unknown -00:00; T-06/A-08) | `Rfc3339DateTimeOffset` | `NotRfc3339DateTimeOffset` | `.Rfc3339DateTimeOffset()`; `Rfc3339DateTimeOffsetStringAttribute` | `{paramName} must be an RFC 3339 date/time.` | `date.format.not-rfc3339` | — |
| `StringRules.DateTimeOffset.IsRfc1123(string?)` (`"R"` format) | `Rfc1123DateTimeOffset` | `NotRfc1123DateTimeOffset` | `.Rfc1123DateTimeOffset()`; `Rfc1123DateTimeOffsetStringAttribute` | `{paramName} must be an RFC 1123 date/time.` | `date.format.not-rfc1123` | `HttpDate` (alias for later) |
| `StringRules.DateOnly.IsIso8601(string?)` (`yyyy-MM-dd`), `StringRules.TimeOnly.IsIso8601(string?)` (`HH:mm`, `HH:mm:ss`, `HH:mm:ss.FFFFFFF`) | `Iso8601DateOnly`, `Iso8601TimeOnly` | `NotIso8601DateOnly`, `NotIso8601TimeOnly` | `.Iso8601DateOnly()`, `.Iso8601TimeOnly()`; `Iso8601DateOnlyStringAttribute`, `Iso8601TimeOnlyStringAttribute` | `{paramName} must be an ISO 8601 date.` / `time.` | `date.format.not-iso8601`, `time.format.not-iso8601` | — |
| `StringUtility.Iso8601Duration.TryParse(string?, out TimeSpan?)` (`PnW` or `PnDTnHnMn.nS`; `Y`/`M` components → false; `P`, `PT` alone → false); `StringRules.TimeSpan.IsIso8601Duration(string?)` | `Iso8601Duration(string?)` → `MustResult<TimeSpan>` | `NotIso8601Duration` → `TimeSpan` | `.Iso8601Duration()`; `Iso8601DurationTimeSpanStringAttribute` | `{paramName} must be an ISO 8601 duration.` | `time.duration.not-iso8601` | `XmlConvert.ToTimeSpan` (lossy on `P1Y`/`P1M`) |
| `UnixTimeRules.IsUnixSeconds(long?)`, `IsUnixMilliseconds(long?)` (bounds: `DateTimeOffset.MinValue`…`MaxValue` in that unit); `DateTimeUtility.TryFromUnixSeconds(long?, out DateTimeOffset?)`, `TryFromUnixMilliseconds` | `UnixSeconds(long)`, `UnixMilliseconds(long)` → `MustResult<long>` | `NotUnixSeconds`, `NotUnixMilliseconds` | `.UnixSeconds()`, `.UnixMilliseconds()`; `UnixSecondsAttribute`, `UnixMillisecondsAttribute` (recorded exception to the `<Type>` suffix rule: the clause name carries the unit and `Int64` adds nothing) | `{paramName} must be a Unix timestamp in seconds.` / `milliseconds.` | `date.unix.not-seconds`, `date.unix.not-milliseconds` | `Epoch` (ambiguous unit) |

## 5. Error-code additions (`src/PineGuard.Core/Codes/`)

New file `MustCodes.TimeZone.cs` (`// Serves: MustTimeZoneClauses.cs, MustStringUtcOffsetClauses.cs`):

The Date code file's final mapping is
`// Serves: MustDateTimeClauses.cs, MustDateOnlyClauses.cs, MustDateTimeOffsetClauses.cs, MustSqlDateTimeClauses.cs, MustStringDateOnlyClauses.cs, MustStringDateTimeOffsetClauses.cs, MustStringDateTimeClauses.cs, MustUnixTimeClauses.cs`.
Each clause file emits only its owning domain's codes; the mandatory audit remains
unchanged, with no exception or allowlist. These narrow code-ownership corrections are
grounded in `apps/cli/src/audit/rules/must-codes.ts` checks (including lines 322–332 and
392–407), not a Brain deviation. [Focused Fable audit ruling]

```csharp
public static class TimeZone
{
    public const string Prefix = "timezone";
    public static class Id        { public const string Prefix = TimeZone.Prefix + ".id";         public const string Null = Prefix + ".null"; public const string Unknown = Prefix + ".unknown"; }
    public static class Kind      { public const string Prefix = TimeZone.Prefix + ".kind";       public const string NotUnspecified = Prefix + ".not-unspecified"; }
    public static class Equality  { public const string Prefix = TimeZone.Prefix + ".equality";   public const string NotSameDay = Prefix + ".not-same-day"; public const string SameDay = Prefix + ".same-day"; }
    public static class Zone      { public const string Prefix = TimeZone.Prefix + ".zone";       public const string Null = Prefix + ".null"; }
    public static class Offset    { public const string Prefix = TimeZone.Prefix + ".offset";     public const string Invalid = Prefix + ".invalid"; public const string Mismatch = Prefix + ".mismatch"; }
    public static class LocalTime { public const string Prefix = TimeZone.Prefix + ".local-time"; public const string Invalid = Prefix + ".invalid"; public const string Ambiguous = Prefix + ".ambiguous"; }
    public static class DaylightSaving { public const string Prefix = TimeZone.Prefix + ".daylight-saving"; public const string NotInEffect = Prefix + ".not-in-effect"; public const string InEffect = Prefix + ".in-effect"; }
}
```

Additions to existing files (each with the `/// <summary><c>code</c></summary>` doc line the file
already uses on every constant):

| File | Node | New constants |
|---|---|---|
| `MustCodes.Time.cs` | `Duration` | `NotLonger`, `NotShorter` (replace `NotGreater`, `NotLess`); `NotPositive`, `NotNegative`, `NotZero`, `Zero`, `Negative`, `Positive`, `NotWholeSeconds`, `WholeSeconds`, `NotWholeMinutes`, `WholeMinutes`, `NotMultiple`, `Multiple`, `NotTimeout`, `NotIso8601` |
| `MustCodes.Time.cs` | existing `Format` | `NotIso8601` |
| `MustCodes.Time.cs` | new `Interval` | `Prefix = "time.interval"`, `OffInterval`, `OnInterval` |
| `MustCodes.Date.cs` | new `Interval` | `Prefix = Date.Prefix + ".interval"`, `OffInterval = Prefix + ".off-interval"`, `OnInterval = Prefix + ".on-interval"` (DateTime/DateTimeOffset only) |
| `MustCodes.Date.cs` | new `Validity` | `Prefix = "date.validity"`, `Expired`, `NotExpired`, `NotEffective`, `Outside` |
| `MustCodes.Date.cs` | `Relative` | `NotWithinLast`, `WithinLast`, `NotWithinNext`, `WithinNext`, `NotOlderThan`, `OlderThan` |
| `MustCodes.Date.cs` | `Age` | `AboveMaximum`, `OutOfRange` |
| `MustCodes.Date.cs` | `Equality` | `NotSameMonth`, `SameMonth`, `NotSameYear`, `SameYear`, `NotSameIsoWeek`, `SameIsoWeek` |
| `MustCodes.Date.cs` | `Calendar` | `NotLeapYear`, `LeapYear`, `NotLeapDay`, `LeapDay`, `NotDayOfWeek`, `DayOfWeek`, `NotFirstDayOfQuarter`, `FirstDayOfQuarter`, `NotLastDayOfQuarter`, `LastDayOfQuarter`, `NotMidnight`, `Midnight`, `NotBusinessDay`, `BusinessDay` |
| `MustCodes.Date.cs` | existing `Format` (already holds `Invalid`) | `NotIso8601`, `NotRfc3339`, `NotRfc1123` |
| `MustCodes.Date.cs` | new `Unix` | `Prefix = "date.unix"`, `NotSeconds`, `NotMilliseconds` |

The `must-codes` audit rule verifies every code constant is referenced by exactly the clauses that
declare it; run `pnpm -C apps/cli exec tsx src/index.ts audit must-codes` after any code edit.

**Single-writer code ownership [Fable corrective council]**:

**Declared layer absences (vocabulary version 2; binding Fable B2/B3 ruling)**:

| Normalized concept | absentFrom | reason (exact vocabulary text) | Owning writer |
|---|---|---|---|
| SameDay | DataAnnotations | Cross-property comparison against a second instant; no single-property attribute (T-00 §4.2, §12). | T-02 U0 converts existing debt |
| ResolvedTimeZone | Fluent | Outcome validator for an adapter-resolved zone ID. Fluent extensions take a supplied TimeZoneInfo and check it at composition (D-13); there is no Fluent ID surface. | T-02 U3 |
| WithinValidity | DataAnnotations | Cross-property [notBefore, notAfter) window (T-00 §4.4). | T-04 U3a |
| SameMonth | DataAnnotations | Cross-property calendar comparison (T-00 §4.5). | T-05 U3b-1 |
| SameYear | DataAnnotations | Cross-property calendar comparison (T-00 §4.5). | T-05 U3b-1 |
| SameIsoWeek | DataAnnotations | Cross-property calendar comparison (T-00 §4.5). | T-05 U3b-1 |

Entries are written only when the owning Must concept exists; Not* methods normalize to
the same concept. Must cannot be absent. Unknown concepts/invalid layers and implemented
declared absences fail. Every undeclared layer remains strictly enforced. No baseline,
ignoreMethods, aliases or exceptions are used to represent these deliberate absences.

The code ownership table follows; vocabulary.json is also a single-writer file per row above.

| File / node | Creating or appending unit | Emitting clause family | Audit treatment |
|---|---|---|---|
| Date.Reference | T-01 U2 creates; U2b wires first consumers; U2c wires the string-DateOnly consumer | MustDateOnlyClauses / MustDateTimeClauses / MustStringDateOnlyClauses calendar reference failures; T-04 reuses | Consumers required before Phase 1 merge; no skip exemption; no new codes/constants added by U2c |
| Date.Kind | Existing node/constants; T-04 reuses NotUtc | Existing DateTime kind clauses; strict UTC DateTime clauses | No declarations or duplicate constants; A-12 templates apply to new clauses |
| TimeZone.Id | T-02 U1 | MustTimeZoneClauses.ResolvedTimeZone validates supplied resolution outcome without lookup; DA resolves then delegates | Existing audit unchanged; no exemption |
| TimeZone.Zone | T-02 U1 | Zone-taking Must configuration checks | Existing audit |
| TimeZone.Offset | T-02 U1 | MustTimeZoneClauses / MustStringUtcOffsetClauses | Existing audit |
| TimeZone.LocalTime | T-02 U1 | MustTimeZoneClauses | Existing audit |
| TimeZone.Kind | T-02 U1 creates NotUnspecified only | MustTimeZoneClauses.ValidLocalTime / UnambiguousLocalTime | A-12 message/value attribution unchanged; no unused positive constant |
| TimeZone.Equality | T-02 U1 creates NotSameDay/SameDay | Zone-taking SameDay/NotSameDay in MustTimeZoneClauses | All failure paths remain TimeZone-domain; no exemption |
| TimeZone.DaylightSaving | T-02 U1 creates NotInEffect/InEffect | DaylightSavingTime / NotDaylightSavingTime in MustTimeZoneClauses, with all adapters | Full validation family; fixtures retained, rules-usage enforced |
| Time.Duration | T-01 U4a rename; T-05 U1 appends | MustTimeSpanClauses / matching string clauses | Consumers land in owning phase |
| Time.Format | T-06 U1 appends | MustTimeOnly / MustTimeSpan format clauses | Existing audit |
| Time.Interval | T-05 U1 creates | MustTimeOnlyClauses | Existing audit |
| Date.Interval | T-05 U1 creates | MustDateTimeClauses / MustDateTimeOffsetClauses and matching DA | Date-domain OffInterval/OnInterval; configuration guard before negation |
| Date.Validity | T-04 U1 creates | MustDateTimeOffset / DateTime / DateOnly clauses | Existing audit |
| Date.Relative | T-04 U1 appends | MustDateTimeOffset / DateTime / DateOnly clauses | Existing audit |
| Date.Age | T-04 U1 appends | MustDateTimeOffset / DateTime / DateOnly age clauses | Existing audit |
| Date.Equality | Existing unzoned SameDay codes; T-05 U1 appends calendar equality codes | Owning date clause families, including plain two-argument DTO SameDay | Zone-taking equality uses TimeZone.Equality in MustTimeZoneClauses |
| Date.Calendar | T-05 U1 appends | MustDateOnly / DateTime / DateTimeOffset clauses | ISO-week surface guarded on supported TFMs |
| Date.Format | T-06 U1 appends | Must string-date format clauses | Existing audit |
| Date.Unix | T-06 U1 creates | MustUnixTimeClauses | Existing audit |

Later phases start after their predecessor merges (2 → 4 → 5 → 6), including
across worktrees. No code-only unit may leave orphan constants at a phase merge.

## 6. Conventions every unit obeys

### 6.1 Clone, do not invent

Each unit names the exact file whose pattern to clone. Copy its file header, XML-doc style, method
shape, `MustResult<T>.FromBool(ok, code, messageTemplate, paramName, value, result)` call, Guard
`GuardFailure.Throw(result, message, exceptionCreator)` call, Fluent `ruleBuilder.MustBe(...)` shape
with the nullable overload, DA `ValidationAttributeBase(typeof(T), code)` + `ValidateValue` +
`FromMustResult` shape, and the test/TestData/Fixture triple.

### 6.2 Target frameworks

`netstandard2.1;net8.0;net10.0`. DateOnly/TimeOnly members follow existing NET8_0_OR_GREATER
guards. ISOWeek DateOnly overloads are net10-only; use the existing DateTime form on net8.
`System.Globalization.ISOWeek` is absent from netstandard2.1; the complete SameIsoWeek
surface uses `NET8_0_OR_GREATER` guards, including DateTime/DateTimeOffset overloads.
Core has no host resolver. Composition/adapter BCL resolution verifies API availability and
runtime/OS/globalization behavior separately; do not infer IANA support from a compile TFM. [A-05]
### 6.3 Determinism in tests

Never look up a real zone by id in deterministic Core unit tests. Host-resolution tests belong
to composition/adapter integration tests and declare their runtime, OS and globalization mode.
Use `"UTC"` plus `TestTimeZones.PlatformSampleId` where supported, never unconditioned
`"Australia/Sydney"` literals. [A-05]
All DST / offset semantics are tested against custom zones built with
`TimeZoneInfo.CreateCustomTimeZone(...)` in `tests/PineGuard.Testing/Common/TestTimeZones.cs`
(T-01 U1 defines them; every later phase reuses them). **Exception**: DataAnnotations attributes
take a zone *id* and must resolve a real zone, so their tests use `"UTC"`, `TestTimeZones.UnknownId`,
and `TestTimeZones.PlatformSampleId` with the named constants `TestTimeZones.PlatformInvalidLocalTime`
and `TestTimeZones.PlatformAmbiguousLocalTime` (never date literals). D-1 midnight-boundary vectors
pin the clock with `FixedTimeProvider.DefaultAheadOfUtc` (+10:00) and `FixedTimeProvider.DefaultBehindUtc`
(−08:00) (council D-4, 2026-09-26); never a real host zone.

### 6.4 Host resolution at composition boundaries [A-05]

Core Rules/Utils do not look up installed zone IDs and spec §5.3 is unchanged. Adapters accepting
an ID resolve it once at composition/validation setup, following existing precedent and Must
configuration-failure messaging. Supplied custom `TimeZoneInfo` objects need no installed ID.

Document ID resolution honestly: the BCL may read host registry/files/ICU, and the result depends
on runtime, OS, installed data and globalization mode. Verify API availability on each compile
target and behavior on each supported runtime; netstandard2.1 alone says nothing about host
support for IANA IDs. Versioned Standards catalog membership and mapping are deterministic
questions distinct from host resolvability; neither set implies the other. Do not add global
positive/negative caches without measured need, explicit invalidation and ClearCachedData semantics.

### 6.5 Parameter ordering and nullability

`TimeProvider? timeProvider = null` sits immediately before `paramName` (Must), before `message`
(Guard, Fluent). `TimeZoneInfo timeZone` is a required positional parameter immediately after the
value(s) it qualifies. `paramName` is always last. The `nullability` and `ordering` audit rules
enforce this; run them after every Must/Guard file edit.

### 6.6 Tests

Every clock-consuming predicate test asserts `CountingTimeProvider.UtcNowCallCount == 1`.
An `AdvancingTimeProvider` with one supplied instant throws if any path samples twice.
Neither test fixture virtualizes timestamps/timers or is referenced by `src/`.

`[Theory]` + `TheoryData` + `[MemberData]` only. Fixtures in `tests/PineGuard.Testing/Fixtures/`
mirror source file names (`TimeZoneRules.cs` → `TimeZoneRulesFixtures.cs`). TestData next to the
test class. Single-line fixture entries, `nameof(F.Group.Field)` names, no magic strings, edge
constants imported from the source class. Clock-reading tests pass `FixedTimeProvider.Default`
(UTC) and, where D-1 matters, `FixedTimeProvider.DefaultAheadOfUtc` (+10:00) and `FixedTimeProvider.DefaultBehindUtc`
(−08:00). Expected records:
`RuleExpected(bool)`, `MustExpected(IsValid, Message, ParamName, Code)`, `GuardExpected`,
`FluentExpected(IsValid, Message, PropertyName, Code)`, `DataAnnotationExpected`. Each Must test
data group asserts the exact message on at least one invalid case and the code on at least one.

### 6.7 Documentation

CS1591 is an error: every public member gets XML docs in the style of the cloned file, including
`<seealso href="https://pineguard.ai/docs/...">` on the class. README rows are added in Phase 7,
not per unit (repo lesson: docs edits are program-wide).

## 7. Sub-agent contract

### 7.1 Model routing

| Tier | Model | Used for in this program |
|---|---|---|
| Decisions / reviews / councils | **Claude Fable 5.1** | All naming and architectural decisions, per-unit and phase reviews, merge verification, council, adaptation triage and escalations. |
| Implementation | **Codex team** | Code, tests, fixtures, docs, tooling and explicitly authorized later generator changes. |
| Evidence collection | **GPT-6 Luna** | Inventories, compile-error capture and source reads; decision-making and review remain Claude Fable 5.1-owned. |

### 7.2 Dispatch envelope (every Agent call carries all of these)

| Field | Rule |
|---|---|
| `name` | `t<phase>-u<unit>-<role>` e.g. `t02-u3-build`, `t02-u3-review`. Named agents are addressable by `SendMessage`. |
| `subagent_type` | Match the Brain playbook's authoritative role. Validate adapter/model compatibility before dispatch; no alias silently substitutes a different model. |
| `model` | From §7.1; passed explicitly. |
| Task size | One unit, ≤ 8 source files touched, ≤ 12 test files touched. If the unit's file list is longer, the phase plan has already split it. |
| Budget | Retain the unit prompt token allowance and Claude Fable 5.1 review allowance (≤ 80k); do not infer a new model-specific allowance. At 80 %, report partial state and continue via an explicit handoff; budgets never relax quality gates. |
| Timeout | Retain the unit prompt wall-clock limit; Claude Fable 5.1 reviews retain 30 min. Report state at the deadline; resume with evidence instead of skipping required work. |
| Check-in | Orchestrator sends `SendMessage` at 15 and 30 minutes if no report has arrived; at 45 minutes with no reply, treat as dead: verify with `ListAgents` elapsed time and `git -C <worktree> status`, then re-dispatch a fresh agent with "resume from worktree state". |
| Inputs | Worktree path; unit section to read from disk; files to clone; decision rows it depends on. |
| Outputs | Files created/modified (exact list), the verification commands it ran with pass/fail, and a ≤ 25-line report. |
| Forbidden | `AskUserQuestion`; touching files outside the unit's list; running `dotnet` outside the worktree; `git add .`; commits (the orchestrator commits via `/commit-<scope>`). |

### 7.3 Fan-out rules

- Units marked ∥ in a phase may run concurrently **in the same worktree** only when their file lists
  are disjoint (the phase plan guarantees this). The `dotnet-ops` hook serialises builds and tests
  inside a worktree automatically; authors do not coordinate builds by hand.
- Phases 2 → 4 → 5 → 6 are serial dependencies, including across separate worktrees.
- Qodana and Sonar runs are serialised program-wide: one at a time, on the local machine only.
- Never dispatch two agents that edit the same file. `MustCodes.*.cs`, `README.md`, and
  `Directory.Packages.props` are single-writer files: the phase plan assigns each to exactly one unit.

### 7.4 Liveness

Watch elapsed time, not completion status. `ListAgents` every checkpoint. A background workflow
can die silently; prefer named `Agent` dispatches with check-ins over the `Workflow` tool for this
program.

## 8. Worktree and merge protocol

PineGuard (run from the repo root, PowerShell):

Before a merge/push to `main`, obtain explicit owner confirmation or verify that an
existing explicit, program-specific preauthorization covers that operation, under
`docs/ai/specs/safety.md` Tier 1. Authorization persists across turns; do not ask again
for the identical approved operation. This protocol, context-transfer approval and another
program's authorization do not imply publication authorization. All implementation work
stays in existing worktrees; the owner performs the authorized main-branch sequence below.

```powershell
git worktree add ".claude/worktrees/<slug>" -b "feature/<slug>" origin/main
# ... all work inside .claude/worktrees/<slug> ...
git -C ".claude/worktrees/<slug>" merge origin/main            # before close-out; resolve, re-verify
git checkout main; git pull --ff-only
git merge --no-ff "feature/<slug>"                              # one merge commit per unit/phase
git push origin main
git worktree remove ".claude/worktrees/<slug>"; git branch -d "feature/<slug>"
```

Standards: identical, with slug `temporal-timezone` for Phase 3 and `temporal-07-closeout` for
Phase 7. Commits use `/commit-<scope>` (dry-run with `-WhatIf`, then `-AutoMessage`), conventional
subject + flowing-prose body, `Co-Authored-By: Claude Fable 5.1 <noreply@anthropic.com>` trailer.
No pull requests (standing rule, 2026-08-30). The Phase 3 release (T-03 U1) is a Tier 1 action:
the owner confirms it interactively.

Release confirmation covers later merge/push operations only when its explicit scope says
so; otherwise use the confirmation/preauthorization rule above.

## 9. Verification gate (per unit unless the unit says otherwise) and Definition of Done

Run applicable per-unit checks inside the worktree, in this order; each must exit 0.
Cross-layer audits are verified at the completed phase cohort below, when all adapters exist:

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-format/Run-Format.ps1 -Scope All
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-diagnostics/Run-CompilerDiagnostics.ps1 -Scope All
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/testing/Run-Tests.ps1 -Scope All
# per touched scope, per TFM (the repo lesson: never trust one multi-TFM run):
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-coverage/Run-CodeCoverage.ps1 -Mode GenerateAndAnalyze -Scope <Scope> -Framework net8.0  -Top 30 -Isolated -SkipHtml
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-coverage/Run-CodeCoverage.ps1 -Mode GenerateAndAnalyze -Scope <Scope> -Framework net10.0 -Top 30 -Isolated -SkipHtml
pnpm -C apps/cli exec tsx src/index.ts audit --gate
# Units touching Codes/Must/Guard/DA; codes-only and T1-U2/U2b cohort rules below apply:
pnpm -C apps/cli exec tsx src/index.ts audit must-codes
# Must/Guard units:
pnpm -C apps/cli exec tsx src/index.ts audit nullability
```

Per phase, before merge (local machine only; Docker and a SonarQube server are required):

After the final layer unit and before Fable phase review, run the completed phase cohort:
`pnpm -C apps/cli exec tsx src/index.ts audit` (all rules with baseline) must exit 0.
Attach `git diff --stat apps/cli/config/baseline.json`; baseline changes are deletions only, except
for the one-for-one rename move defined immediately below. A
rename unit may move a baseline key whose member it renames: one deleted key per added key, net
count unchanged, each pair cited in the cohort receipt with the old and new declaration lines. Any
other addition remains prohibited. See
[the baseline-key ruling](temporal-u4a-baseline-key-ruling-fable.md).
Layer-parity, ordering, must-usage and rules-usage remain strict and visible. Delete debt keys
owned by removed members in their deleting unit; do not add keys except the exact paired rename
moves defined above, and do not add ignoreMethods/aliases workarounds, exceptions,
`--update-baseline`, or fake layer members. T-02 U0 establishes the
reviewed absence schema and string Rules mapping before temporal Must work.

Default Qodana also requires securely provisioned `QODANA_TOKEN`; the documented
`-Linter auto` mode is its alternate invocation. Missing scanner prerequisites remain
visible gates. InspectCode is a distinct scanner and is not an automatic substitute for
Qodana/Sonar. An owner decision accepting any phase-gate substitution must be recorded
here explicitly; tooling acceptance alone does not change temporal readiness.

```powershell
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/qodana/Run-Qodana.ps1 -Scope All -Clean
pwsh -NoProfile -ExecutionPolicy Bypass -File ./tools/code-scan/sonarqube/Run-SonarScanner.ps1
```

`<Scope>` values are exactly: `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`,
`DataAnnotations`, `Testing`. Coverage runs auto-enforce 100 % line and branch for per-layer scopes.
Codes-only units (T2-U1, T4-U1, T5-U1, T6-U1) add constants no clause references yet, so they skip
`audit must-codes` and run only format + diagnostics; the Must unit of the same phase runs `must-codes`.

T1-U2 and T1-U2b are an atomic acceptance cohort: U2 is not standalone commit-ready or mergeable while its new code lacks consumers. Run the complete gate including audit must-codes only after U2b wires first consumers; no audit exemption or Phase 1 merge is permitted for the unfinished cohort. T1-U2c follows the accepted T1-U2/U2b cohort and must finish before Phase 1 T1-U6, but it is a separate unit and does not join the cohort.

**Definition of Done (unit)**: every command above green; Claude Fable 5.1 review says "no deviations
from T-00 §4/§5/§6"; tracker row updated with the commit hash.
**Definition of Done (phase)**: all units Done; Qodana and Sonar zero findings; Claude Fable 5.1 phase review
green; merged `--no-ff` to `main`, pushed, worktree removed; tracker row Done.

## 10. Routines (cloud, two runs per day)

### 10.1 What the cloud can and cannot do (verified against Claude Code docs, 2026-09-22)

| Capability | Status | Consequence for this program |
|---|---|---|
| Fresh Ubuntu 24.04 VM per run, repo cloned from the default branch | Yes | State lives only in committed files: the tracker and the `claude/…` branch. |
| .NET SDK, PowerShell | **Not preinstalled** | Environment setup script installs `dotnet-sdk-10.0` and `pwsh` (see orchestration §3); first run is a smoke run that proves both. |
| Push branches | `claude/`-prefixed only; protected branches refused | D-10: routines never merge; the owner merges locally. |
| `.claude/settings.json` hooks and `.mcp.json` | Load only in **single-repo** sessions | One routine per repo; never a multi-repo routine. |
| Docker | Available | Qodana can run in the cloud; SonarQube needs a server and stays local/CI. |
| nuget.org, apt, GitHub | In the default trusted allowlist | Restore, tool install, and push work without custom network rules. |
| Interactive questions | None; permissions auto-approved | Prompts must contain no decisions; every choice is pre-made here. |
| Limits (duration, tokens, concurrency) | Undocumented | Each run does **one unit**, then stops. Observe run history before assuming more. |
| Scheduling | Presets or custom cron (≥ 1 h), local time converted to UTC; runs stagger a few minutes | `0 6 * * *` and `0 18 * * *` in `Australia/Sydney`. |

### 10.2 The two daily phases

| Run | Routine | Does |
|---|---|---|
| 06:00 | `pineguard-temporal-build` | Reads the tracker on `main`; picks the next unit whose dependencies are Done and whose real Fable acceptance is recorded; creates `claude/temporal-<unit>` from `main` (or resumes it); dispatches the unit's build agent (Codex team) then the verify commands; commits; rebases on `main`; pushes; does not edit the tracker (D-10); the owner updates it on main at merge. |
| 18:00 | `pineguard-temporal-review` | For every `claude/temporal-*` branch pushed since the last run: verifies from a clean clone, runs Claude Fable 5.1 review (§7.1), writes evidence into `artifacts/reviews/<unit>.md` on the branch and stops. The owner mirrors readiness into the tracker at merge. Never merges. |

The owner's local session (any time) merges **Ready to merge** branches `--no-ff`, runs Qodana +
Sonar per phase, and removes worktrees. A Standards routine pair with the same shape exists for
Phase 3 and Phase 7 only. Verbatim prompts and the setup script are in
[temporal-orchestration.md](temporal-orchestration.md) §3.

### 10.3 Why this split

Build-then-review on different runs gives every unit a second fresh-clone verification before a
human sees it, and keeps each run to a bounded, single-unit workload the undocumented limits are
unlikely to interrupt. Nothing the cloud does is irreversible: a branch that fails review is fixed
by the next build run or deleted by the owner.

## 11. Risks

| Risk | Mitigation |
|---|---|
| Names ossify before sign-off | Phase 0 gate; no code before real Fable acceptance of the revised plan. |
| Zone lookups differ between Windows dev and Linux CI | §6.3: custom zones for semantics; only `"UTC"` and `TestTimeZones.PlatformSampleId` for lookups. |
| Null-provider today follows the host zone [A-03] | Document null → System/LocalTimeZone, inject a provider for deterministic civil-date results, and run the named non-UTC CI integration check in T-01 U2; routine `TZ=UTC` alone cannot prove this behavior. |
| Standards adaptation (F-11) dwarfs the facade | T-03 U2 is a discovery unit that measures it first; U3 fans out per domain; the owner can pause after U2 if the debt is larger than a day. |
| Routines cannot merge; branches pile up | 18:00 review marks readiness; owner merges in one sitting; stale `claude/*` branches older than 7 days are listed by the review run. |
| A sub-agent dies silently | §7.2 check-ins at 15/30/45 min; `ListAgents` elapsed column; re-dispatch with resume. |
| `MustCodes` edits collide across concurrent phases | Single-writer rule (§7.3); phases 2 → 4 → 5 → 6 are serial dependencies; each owns its additions and first consumers. |
| Coverage proven once, not per TFM | §9 runs each touched scope twice with `-Framework`. |
| `DateTime` age semantics change under D-1 | T-01 U1 fixtures assert every `Kind` is judged on `value.Date` against `captured referenceDate from internal TryGetTodayDate`, at `FixedTimeProvider.Default`, `DefaultAheadOfUtc` and `DefaultBehindUtc`, with midnight-boundary vectors. Pre-change results are recorded for `Unspecified` only; `Utc`/`Local` result changes are intended (council D-1, 2026-09-26). |

## 12. Out of scope (recorded so they are not blind spots)

- IANA↔Windows conversion in Core (D-5).
- Holiday registries (Standards repo, its own initiative `generate-holidays`).
- Business-hours-in-zone (`IsTimeOfDayBetween(instant, open, close, zone)`): backlog, after Phase 5.
- Slug, ULID, MAC, full MIME rules (roadmap Part 7 remainder; separate batch).
- Localisation of messages; `IStringLocalizer` seam (roadmap Part 3).
- Fluent model-expression (cross-property) overloads for any new clause. `FluentDateTimeExtensions.SameDay`
  takes a literal `other`; no `Expression<Func<...>>` overload exists in the project today. This program
  ships literal-argument shapes only; a cross-property shape is a backlog item with its own naming row.
