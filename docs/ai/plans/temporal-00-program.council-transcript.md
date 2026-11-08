<!-- metadata_header
type: council-transcript
id: temporal-00-program-council
version: 1.0
status: final
last_updated: 2026-09-26
-->

# Council Transcript — T-00 Temporal & Time-Zone Program (2026-09-26)

This council run was plan-gated per `docs/ai/specs/council.md` §1 trigger 1 (public API change
across ≥ 3 projects) and trigger 2 (change spanning two repos). A Fable session orchestrated eleven
sub-agents: five advisor personas, one Haiku fact-check/repository-read agent, and (implicitly,
via the peer-review pass) five reviewer agents reading the five anonymized advisor responses. The
framed question given to advisors deviated deliberately from the skill's 150–300-word response cap:
it appended an uncounted 14-line compact-verdict list (`D-n: SIGN | AMEND | REJECT`) per advisor, on
top of the prose analysis, so that every decision row would get an explicit, machine-parseable
call from every advisor without inflating the prose budget. This is recorded here as a deliberate
adaptation of the standard council format, not an oversight.

## 1. Framed question

Verbatim from `framed-question.md`:

> # Framed question (council session 2026-09-26, plan T-00 temporal-00-program.md)
>
> ## Core decision
> PineGuard is a .NET validation library (netstandard2.1 / net8.0 / net10.0) with five layers that all thread `TimeProvider`: Core `*Rules` predicates -> `Must.Be.*` (never throws, returns `MustResult<T>` with a code) -> `Guard.Against.*` (throws, calls the positive Must clause) -> FluentValidation extensions -> DataAnnotations attributes. A sibling repo, PineGuard.Standards, ships registry data (IANA zone1970.tab snapshot, CLDR windowsZones) and pins PineGuard at 0.1.0-alpha.7. The library has not had a 1.0 release; the standing rule is "no back-compat shims before first release; delete replaced members outright".
>
> A seven-phase "Temporal & Time-Zone" program is planned. Its charter (T-00) has a decisions register of 14 rows the owner must sign before any code is written. Each row already carries a recommendation and rejected alternatives. The council's job: for EACH of the 14 rows, state whether the recommendation should be SIGNED AS-IS, AMENDED (say exactly how), or REJECTED (say what instead), and then say which single row is the most dangerous to get wrong.
>
> ## Verified facts the rows rest on (from source, 2026-09-22; F-6 re-confirmed 2026-09-26)
> - F-1: `TimeProvider.LocalTimeZone` is unused anywhere in `src/`. "Today" for DateOnly past/future/age and DateTime age = `DateOnly.FromDateTime(GetUtcNow(tp).UtcDateTime)`, i.e. always the UTC calendar date.
> - F-3: The published test double `FixedTimeProvider(DateTimeOffset utcNow)` in PineGuard.Testing overrides only `GetUtcNow`; `LocalTimeZone` silently falls through to the dev machine's zone.
> - F-4: `TimeOnlyRules.IsBetween` is linear; a 22:00-06:00 window is unrepresentable.
> - F-5: `DateTimeRules.IsSameDay(DateTime, DateTime)` normalises to UTC dates; `IsWeekday`/`IsFirstDayOfMonth` use the value's own wall-clock date. Two calendars in one class.
> - F-6: `GuardTimeSpanClauses.LessThan(v, t, inclusion=Exclusive)` calls `Must.Be.GreaterThan(v, t, inclusion)` forwarding the SAME inclusion, so `Guard.Against.LessThan(v, t)` throws when `v == t`. `GuardNumberClauses.LessThan` calls `Must.Be.GreaterThanOrEqual` (boundary passes).
> - F-9: .NET ships no tz database. Windows ids come from the registry; IANA ids from OS tzdata via ICU; IANA<->Windows conversion is ICU/CLDR-dependent and fails under NLS/invariant globalization. `TimeZoneInfo.TryFindSystemTimeZoneById` does not exist on netstandard2.1.
> - F-11: PineGuard main has unreleased API changes since alpha.7; the Standards bump is a repo-wide adaptation, not a pin bump.
> - F-12: CI is ubuntu-latest; dev is Windows.
> - Spec `docs/ai/specs/spec.md` section 5.3 "Determinism": "No IO-dependent validation (filesystem/network/environment) inside Core Rules/Utils." Precedent: `CultureInfoUtility` in Core already calls `CultureInfo.GetCultures(...)` (ICU/NLS host data).
> - Precedent: `MustPredicateClauses` reports a null `predicate` argument by returning a failed result with code `Predicate.Callback.Null`, not by throwing.
> - Naming canon: Guard names describe the bad state and call the positive Must clause; negation prefix is `Not*`; codes are `<domain>.<aspect>.<condition>`; DA attribute = `<Clause><Type>Attribute`; message = `"{paramName} must <condition>."`
>
> ## The 14 decision rows (recommendation -> rejected alternatives)
> D-1 "Today" for calendar-date rules. REC: use the provider's `LocalTimeZone`: `GetToday(tp) = DateOnly.FromDateTime((tp ?? TimeProvider.System).GetLocalNow().DateTime)`. `TimeProvider.System.LocalTimeZone` is `TimeZoneInfo.Local`, so UTC containers behave as today; tests pin UTC via FixedTimeProvider. A DateTime value's calendar date: `Unspecified` -> `value.Date`; `Utc`/`Local` -> converted into `LocalTimeZone` then `.Date`. REJ: (a) keep UTC (wrong for users east of Greenwich in the morning); (b) explicit `TimeZoneInfo` parameter on every rule (doubles signatures; TimeProvider is the designed seam).
> D-2 Which calendar DateTimeRules predicates use (F-5). REC: keep as is and document: IsWeekday/IsWeekend/IsFirstDayOfMonth/IsLastDayOfMonth use the value's own wall-clock date; IsSameDay(DateTime, DateTime) compares UTC dates. Add explicit-zone overload `IsSameDay(DateTimeOffset, DateTimeOffset, TimeZoneInfo)` in Phase 2. REJ: change IsSameDay to wall-clock (silently changes results for mixed-Kind inputs).
> D-3 OS zone lookup inside Core vs the "no IO in Core" invariant. REC: allowed in Core, documented as "environment-dependent, not IO", precedent CultureInfoUtility; every such member carries a verbatim `<remarks>` paragraph; spec 5.3 amended in the same commit that introduces the first host lookup (T-02 U2). REJ: satellite `PineGuard.TimeZones` package (splits the one primitive every zone-aware rule needs away from those rules).
> D-4 Public PineGuard.Testing clock double. REC: keep `FixedTimeProvider` (published, consumed by Standards); extend with `LocalTimeZone`; document Microsoft's `FakeTimeProvider` as the recommended consumer choice for advancing clocks; PineGuard's double stays frozen-instant only. REJ: depend on `Microsoft.Extensions.TimeProvider.Testing` (adds a dependency to a published package, breaks Standards).
> D-5 IANA<->Windows conversion in Core. REC: No. Stays in Standards (CLDR snapshot, deterministic, territory-aware); callers without Standards use `TimeZoneInfo.TryConvertIanaIdToWindowsId` directly. REJ: wrap the BCL call in `TimeZoneUtility` (two sources of truth that disagree by ICU version).
> D-6 Fate of `IsWithinDaysFromNow` (absolute distance, either direction, DateTime only). REC: keep unchanged; Phase 4 adds directional `IsWithinLast`/`IsWithinNext`. REJ: rename to `IsWithinDaysOfNow` (churn, no gain).
> D-7 Standards clause name once Core ships `Must.Be.SystemTimeZone(string)`. REC: rename Standards' `Must.Be.SystemTimeZone(value, territory, cldrProvider, ianaProvider)` -> `Must.Be.KnownTimeZone(...)`; Guard `NotSystemTimeZone` -> `NotKnownTimeZone`; Fluent `.SystemTimeZone()` -> `.KnownTimeZone()`. Reason: with both packages referenced `Must.Be.SystemTimeZone(x)` is ambiguous (CS0121, both candidates need optional-parameter substitution). REJ: keep the overload (CS0121); `MappedTimeZone` (mechanism); `RegisteredTimeZone` (suggests caller registered it).
> D-8 tzdata `backward` links in Standards. REC: ingest `backward`; generate `DefaultIanaTimeZoneLinkData.cs`; add `IIanaTimeZoneProvider.TryResolveLink(string? value, out string? canonicalId)`; `IsIanaTimeZoneId` stays canonical-only; add `IsIanaTimeZoneIdOrLink` + `Must.Be.IanaTimeZoneIdOrLink`. REJ: `bool acceptLinks` parameter (booleans read badly); enum `IanaIdMatch` (one enum for one method).
> D-9 GuardTimeSpanClauses boundary polarity (F-6). REC: fix during the T-01 rename: threshold guards call the complement Must clause with the COMPLEMENTED inclusion (Inclusive <-> Exclusive) so the boundary passes exactly as in GuardNumberClauses. REJ: leave it (ships a boundary defect under a new name).
> D-10 Cloud routines vs the standing "no PRs, direct `merge --no-ff` to main" rule. REC: routines push `claude/temporal-<unit>` branches only and update the tracker on that branch; merging to main stays a local owner action; routines never open PRs. REJ: let routines open PRs (contradicts the 2026-08-30 standing rule).
> D-11 Council run. REC: this plan crosses the council gate (public API, >=3 projects, two repos); run `/plan-with-council` on T-00, stamp `council_verdict_ref`, move status to active. REJ: skip (violates spec).
> D-13 A `null` `TimeZoneInfo` argument on a Must clause whose `timeZone` parameter is required. REC: Must never throws: return `MustResult<T>.Fail(MustCodes.TimeZone.Zone.Null, "{paramName} must not be null.", nameof(timeZone), value)` mirroring the null-predicate precedent; new code `timezone.zone.null`; Guards inherit the failure and throw as usual. REJ: `ThrowHelper.ThrowIfNull(timeZone)` (a Must clause that throws breaks the never-throws contract).
> D-14 Calendar-date basis for DateTimeOffset age rules (T-04). REC: the value's OWN wall-clock date (`value.Date`), consistent with every other DateTimeOffsetRules calendar predicate; "today" still comes from the clock's LocalTimeZone (D-1). Asymmetric with DateTime (whose Utc/Local kinds are converted into the clock zone) and deliberately so: a DateTimeOffset carries the zone the user entered it in. REJ: convert into clock.LocalTimeZone (discards the offset the caller supplied).
> D-15 Which new Must clauses get a `Not*` pair. REC: state predicates get a pair (LeapYear, LeapDay, Midnight, OnDayOfWeek, OnInterval, WholeSeconds, WholeMinutes, MultipleOf, BusinessDay, SameMonth/SameYear/SameIsoWeek/SameDay(zone), WithinLast, WithinNext, OlderThan, Expired/Unexpired, First/LastDayOfQuarter). Constraints get none (Effective, WithinValidity, Min/MaximumAge, AgeBetween, Timeout, sign family, UtcOffset, SystemTimeZoneId/SystemTimeZone, ValidLocalTime, UnambiguousLocalTime, InTimeZone, Iso8601*/Rfc*, UnixSeconds/UnixMilliseconds). REJ: pair everything (`NotEffective`, `NotTimeout` read as nonsense).
> (D-12 does not exist; the register skips that number.)
>
> ## What is at stake
> These names, codes and semantics become public API across >=3 projects and two repos and cannot be shimmed later (no back-compat before 1.0, but every rename after alpha.8 still costs a Standards adaptation pass). D-1 changes the observable result of existing age/past/future rules for every non-UTC host. D-3 amends a normative spec. D-9 changes throw behaviour on a boundary. The implementers are Sonnet sub-agents who will not exercise judgement: whatever is signed here is what ships.
>
> ## Required output shape (all advisors)
> 1. Your persona analysis, 150-300 words, no preamble, no hedging.
> 2. Then a compact verdict list, one line per row, exactly: `D-n: SIGN | AMEND: <exact change> | REJECT: <what instead>` (this list is not counted toward the word cap).
> 3. Final line: `MOST DANGEROUS: D-n - <one clause why>`.

## 2. Advisor responses

De-anonymized per the map in §3 below: A = The Outsider, B = The Contrarian, C = The Executor,
D = The First Principles Thinker, E = The Expansionist.

### The Outsider (Response A)

I know nothing about PineGuard, its Brain, or its previous councils. I read only what is in front of me, as a .NET developer meeting this API for the first time. Two things stand out.

First, the text is dense with insider shorthand that the target audience will not recognise: "T-02 U2", "council gate", "cloud routines", "the 2026-08-30 standing rule", "spec 5.3", "tzdata `backward` links", "CS0121 with optional-parameter substitution", "MustResult<T> with a code", "MustCodes.TimeZone.Zone.Null". D-10 and D-11 cannot be evaluated at all without prior context; they are process rows, not API rows, and nothing in the question contradicts them, so I sign them by default and flag that they should not be in a register that Sonnet implementers turn into code.

Second, and this is the real finding: several names and behaviours are only guessable if you already know the library. `IsSameDay(DateTime, DateTime)` compares UTC dates while its siblings use wall-clock dates; the name gives no hint. `Must.Be.SystemTimeZone` versus `Must.Be.KnownTimeZone`: known to whom? A newcomer cannot tell which one consults the host OS and which consults a bundled snapshot. `IsIanaTimeZoneId` rejects `US/Eastern`, the id most people actually type, and the escape hatch is called `OrLink`, a tz-maintainer term no .NET developer uses; .NET developers say alias. `IsWithinDaysFromNow` reads as future-only to everyone outside this repo. D-13's failed result for a null `timeZone` argument looks exactly like a failure of the user's data ("timeZone must not be null") when it is a programmer error; the code `timezone.zone.null` does not say "you passed a null argument". D-15 lists `Expired/Unexpired` while the stated canon says the negation prefix is `Not*`; the register contradicts its own facts on the same page. D-4: a double named `FixedTimeProvider` whose zone varies by machine is a trap; the amendment must name the default. D-1 makes `Must.Be.Past(dateOnly)` depend on the server's zone with nothing in the signature saying so; that matches `DateTime.Today` behaviour, which a .NET developer will accept, but only if the XML summary says it in the first sentence.

D-1: AMEND: sign the LocalTimeZone basis, but every affected rule's XML summary first sentence must state "'today' is the calendar date in the TimeProvider's LocalTimeZone (host zone by default)" and name the Kind rule (Unspecified -> value.Date; Utc/Local -> converted); nothing in the signature reveals it.
D-2: REJECT: no release exists to protect, so rename `IsSameDay(DateTime, DateTime)` to `IsSameUtcDay` now; "document it" leaves a name that says nothing about UTC beside siblings that use wall-clock.
D-3: AMEND: the verbatim `<remarks>` must name concrete failure modes ("depends on the host time-zone database; Windows and Linux may disagree; fails under InvariantGlobalization/NLS"), not the internal phrase "environment-dependent, not IO".
D-4: AMEND: the extended `FixedTimeProvider` must default `LocalTimeZone` to `TimeZoneInfo.Utc` when not supplied; a "fixed" double that varies by dev machine is the F-3 trap restated.
D-5: SIGN (add a one-line doc note that `TryConvertIanaIdToWindowsId` is .NET 6+ only, since netstandard2.1 is a target).
D-6: AMEND: XML summary first sentence must say "in the past or the future"; once WithinLast/WithinNext ship in Phase 4, delete it unless a caller need is shown, since pre-1.0 deletion is free and the name reads future-only.
D-7: AMEND: `KnownTimeZone` is indistinguishable from `SystemTimeZone` to a newcomer; the name must carry the data source (e.g. `Must.Be.CatalogedTimeZone` or `Must.Be.IanaOrWindowsTimeZone`) and the summary must contrast "bundled IANA/CLDR snapshot" with "host OS".
D-8: AMEND: use `OrAlias` not `OrLink` (`IsIanaTimeZoneIdOrAlias`, `Must.Be.IanaTimeZoneIdOrAlias`, `TryResolveAlias`); the canonical-only rule's doc must say aliases such as `US/Eastern` are rejected.
D-9: SIGN.
D-10: SIGN (process row; requires prior context; keep it out of anything an implementer reads as API).
D-11: SIGN (this is the row being executed).
D-13: AMEND: keep never-throws, but the code must say it is an argument fault, not a data failure: `MustCodes.TimeZone.Argument.Null` / `timezone.argument.null`, and the message must read "The timeZone argument must not be null." so a caller does not think their value failed validation.
D-14: AMEND: sign the semantics, but the DateTime and DateTimeOffset age rule docs must each state their calendar basis and cross-reference the other in one sentence, since `new DateTimeOffset(dt)` and `dt` will yield different ages near midnight.
D-15: AMEND: `Unexpired` violates the stated `Not*` canon; use `NotExpired`. Otherwise sign the pairing rule.

MOST DANGEROUS: D-1 - it silently changes every existing age/past/future result on non-UTC hosts and nothing in any method signature tells the caller which zone "today" lives in.

### The Contrarian (Response B)

The strongest-sounding row is D-1, and it is where this program breaks. "UTC containers behave as today" is an admission, not a defence: it means the change is invisible in the one environment you test in (ubuntu CI, UTC) and live everywhere else. Combine it with F-3 and the trap closes: the day D-1 lands, every existing test that uses `FixedTimeProvider(utcNow)` starts reading `LocalTimeZone` from the developer's Windows machine. Age and past/future tests pass on CI at 23:30 UTC and fail on a Sydney laptop, or the reverse. D-4 says "extend with LocalTimeZone" but never says what the default is; if it stays machine-local, D-1 ships nondeterminism into a library whose spec is titled "Determinism". Sonnet implementers will not spot this. And "today in the server's zone" is not "today for the user east of Greenwich" either; it is today for wherever ops happened to deploy.

D-8 is the second landmine and nobody is looking at it: since tzdata 2022b, `Europe/Oslo`, `Europe/Amsterdam`, `Europe/Stockholm`, `Europe/Copenhagen` are `backward` links. A canonical-only `IsIanaTimeZoneId` rejects the ids every Scandinavian browser emits. The default must accept links; strictness is the opt-in.

D-14 mixes two calendars in one subtraction. Age = today-in-clock-zone minus birthdate-in-caller-offset. At the 18th-birthday boundary that is a legal outcome flipping on server location. Two-point rules need one calendar.

D-3 launders a registry read as "not IO" and then ships tests that will pass on Windows and fail under ubuntu/invariant globalization (F-9, F-12). D-2 keeps two calendars in one class because changing it would "silently change results", which is not a valid reason before 1.0; you are pre-1.0 precisely so you can fix this.

D-1: AMEND: land D-4's `LocalTimeZone` override with default `TimeZoneInfo.Utc` in the SAME unit; add explicit `TimeZoneInfo` overloads only on today-relative rules (past/future/age), not every rule
D-2: REJECT: unify `IsSameDay(DateTime, DateTime)` on the D-1 calendar-date rule now (pre-1.0); one class, one calendar; zone overload still added
D-3: AMEND: spec 5.3 names "host registry lookup" as a category with a test rule (ids valid on both OSes or OS-conditioned) and mandates a `<remarks>` sentence on the invariant-globalization failure mode
D-4: AMEND: add `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo localTimeZone)`; single-arg ctor defaults `LocalTimeZone` to UTC, never machine zone
D-5: SIGN
D-6: AMEND: delete `IsWithinDaysFromNow` in Phase 4 when `WithinLast`/`WithinNext` ship (no back-compat rule applies; either-direction is trivially composable)
D-7: AMEND: `KnownTimeZone` implies a superset of `SystemTimeZone`, which is false (OS and CLDR snapshot diverge both ways); the name must state the source (e.g. `CatalogedTimeZone`) or `<remarks>` must say so explicitly
D-8: REJECT: default `IsIanaTimeZoneId` accepts canonical ids AND `backward` links; add `IsCanonicalIanaTimeZoneId` as the strict opt-in; drop the `OrLink` suffix family
D-9: AMEND: separate commit BEFORE the rename with a `v == t` boundary test on every threshold guard across all Guard*Clauses (F-6 audited only TimeSpan and Number)
D-10: AMEND: routines never touch the tracker; owner updates it on main at merge (per-branch tracker edits conflict when units run in parallel); state rebase-on-main policy
D-11: SIGN (and record what deleted row D-12 was and why)
D-13: AMEND: SIGN for Must; Fluent/DA adapters throw `ArgumentNullException` at rule-configuration time so a developer bug never surfaces as an end-user validation message
D-14: AMEND: two-point rules (age, WithinLast/Next) compute "today" in the VALUE's offset (`GetUtcNow().ToOffset(value.Offset).Date`); single-value predicates keep own wall-clock
D-15: AMEND: OlderThan/WithinLast/WithinNext are comparisons, not states; no `Not*` pair; `Expired`/`Unexpired` is the pair, drop `NotExpired`

MOST DANGEROUS: D-1 - it silently changes existing rules for every non-UTC host while F-3 turns every existing FixedTimeProvider test machine-dependent, and UTC CI will never see it.

### The Executor (Response C)

Monday morning, the first commit is D-9. It is a one-line polarity fix with an existing test harness, a failing test you can write in two minutes (`Guard.Against.LessThan(v, v)` must not throw), and zero design risk. Land it alone, before the T-01 rename, so the bisect is clean. Never bundle a bug fix inside a rename diff.

The second commit is D-4, and it must land before D-1, not alongside it. D-1 reads `LocalTimeZone` off the provider; F-3 says the published double falls through to the machine zone; F-12 says CI is UTC and dev is Windows. Ship D-1 first and every "today" test is green on ubuntu and flaky on the owner's desk after 5pm, or green everywhere until a contributor in Sydney clones the repo. The fix is mechanical: `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)` with the default pinned to `TimeZoneInfo.Utc`. Then D-1 collapses to one `GetToday(tp)` helper plus a documented Kind rule.

Rows that cost nothing: D-2, D-5, D-6, D-10, D-11, D-14 are "do nothing or write a sentence" decisions. Sign and move on.

Rows with no clear first step: D-3 says "verbatim `<remarks>` paragraph" but the paragraph does not exist yet; each Sonnet agent will write its own. Put the text in T-00 now. D-8 is the largest new-code row (ingest, generator, interface member, rule, clause) and sits on no Core critical path; take it off alpha.8. D-15 contradicts itself: `Unexpired` breaks the `Not*` canon stated in the same document, and Sonnet will ship the contradiction verbatim.

Smallest shippable cut: D-9, D-4, D-1, D-13 in Core. Everything else is a later phase or the Standards adaptation pass F-11 says you owe anyway; fold D-7 into that pass.

D-1: AMEND: lands only after D-4; state the DateTime rule as one formula: `Unspecified` -> `value.Date`, otherwise `value.ToUniversalTime()` -> `TimeZoneInfo.ConvertTimeFromUtc(_, tp.LocalTimeZone).Date`
D-2: SIGN
D-3: AMEND: write the verbatim `<remarks>` paragraph into T-00 now so every unit pastes identical text
D-4: AMEND: `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)` defaulting to `TimeZoneInfo.Utc`, never the machine zone
D-5: SIGN
D-6: SIGN
D-7: AMEND: perform the rename inside the single F-11 Standards adaptation pass, not as a separate pass
D-8: AMEND: sign the shape, but move it off the alpha.8 critical path into its own final unit
D-9: AMEND: land as a standalone commit with its own failing test BEFORE the T-01 rename, not inside it
D-10: SIGN
D-11: SIGN
D-13: SIGN
D-14: SIGN
D-15: AMEND: `Unexpired` -> `NotExpired` per the `Not*` canon; sign the rest of the list as written

MOST DANGEROUS: D-1 - it changes every non-UTC host's answer and, without D-4 landing first, the regression is invisible on UTC CI and only surfaces on a developer's desk after the local day rolls.

### The First Principles Thinker (Response D)

The register asks "which zone should the clock use?" The real question is "whose calendar date is a rule judging?" — and the answer is never the machine's. A birthday, an expiry, a business day belong to the value or to the caller's business context; the host's zone is a proxy that is exactly wrong where it matters. D-1's own justification proves this: "UTC containers behave as today" means production behaviour does not change at all, while every Windows dev box and every test that forgets to pin the zone (F-3 says the shipped double cannot pin it) now diverges from ubuntu CI (F-12). The row buys nothing in prod and buys nondeterminism in dev.

Strip it to one principle and the register simplifies: **a value is judged on its own calendar date; only "today" comes from the clock.** D-14 already states this for DateTimeOffset. D-2 already states it for IsWeekday. D-1 contradicts it by converting Utc/Local DateTimes into the clock zone (and Kind.Local means `TimeZoneInfo.Local`, not `tp.LocalTimeZone`, so that conversion is not even well-defined). D-2's IsSameDay is the last hold-out, defended by a back-compat argument the project forbids before 1.0. Apply the one principle everywhere and the "deliberate asymmetry" in D-14 vanishes because there is nothing to be asymmetric with.

Two other framings hide assumptions. D-8 treats canonical ids as the default and links as the exception; tzdata says the opposite — a link IS a valid zone id, canonical-only is the strict special case, so the plain name should accept links. D-13 treats a null `TimeZoneInfo` as data; it is configuration, and Fluent/DA adapters receive it at composition time, where a bug should fail fast rather than fail every entity with a validation message forever.

D-1: AMEND: `GetToday(tp)` uses `tp.LocalTimeZone` as recommended, but a DateTime value's calendar date is always `value.Date` regardless of Kind (own wall-clock, one rule with D-2/D-14); drop the Utc/Local conversion.
D-2: AMEND: change `IsSameDay(DateTime, DateTime)` to compare `.Date` of each (one calendar per class); the "silent change" objection is a pre-1.0 back-compat argument the standing rule forbids; keep the explicit-zone DTO overload.
D-3: AMEND: spec 5.3 wording must say "host-registry-dependent (culture, time zone); results may differ by OS/ICU/globalization mode", not "environment-dependent, not IO"; each such member's `<remarks>` names the Standards snapshot clause as the deterministic alternative.
D-4: AMEND: `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)` MUST override `LocalTimeZone`, null → `TimeZoneInfo.Utc`; fall-through to the host zone is forbidden.
D-5: SIGN
D-6: SIGN
D-7: AMEND: rename as recommended, but record the rationale as semantic (host registry vs CLDR/IANA snapshot), not CS0121, so the `<remarks>` Sonnet writes explain meaning, not a compiler error.
D-8: REJECT: `IsIanaTimeZoneId` accepts canonical ids AND links (that is what tzdata calls a valid id); add `IsCanonicalIanaTimeZoneId` / `Must.Be.CanonicalIanaTimeZoneId` for the strict case; keep `TryResolveLink`.
D-9: SIGN
D-10: SIGN
D-11: SIGN
D-13: AMEND: keep the Must failure result as recommended, but Fluent extensions and DA attributes `ThrowIfNull(timeZone)` at composition/construction time so a null zone fails at startup, not per-entity at runtime.
D-14: SIGN (with D-1 amended, this is no longer an asymmetry but the single library-wide rule).
D-15: SIGN

MOST DANGEROUS: D-1 - its stated benefit is zero in production while it makes dev and CI disagree through the unpinned zone in F-3, and it bakes in a Kind-conversion rule that contradicts D-2 and D-14.

### The Expansionist (Response E)

The framing treats this as a correctness clean-up. It is bigger: D-1, D-3 and D-4 together make PineGuard the first .NET validation library with a fully injectable, zone-aware clock across every layer. No competitor threads `TimeProvider` through Fluent and DataAnnotations. Sign those three and "deterministic time validation, test-pinnable to the instant and the zone" becomes a category claim, not a bullet.

The hidden funnel is D-3. `Must.Be.SystemTimeZone(string)` in Core turns PineGuard into a config validator for `appsettings` zone strings with zero extra packages. Every consumer who hits an ICU/NLS mismatch (F-9) then discovers Standards' CLDR snapshot is the deterministic answer. D-5 is right to keep conversion in Standards; that is the upsell, not a gap. Market it as such.

D-4 is undervalued. F-3 is a latent flaky-test generator on every non-UTC machine, and F-12 says dev is Windows, CI is Ubuntu. Extending `FixedTimeProvider` with `LocalTimeZone` is not enough: default it to UTC and add a `(DateTimeOffset, TimeZoneInfo)` constructor. A double that is deterministic by construction is a product feature; one that inherits the host zone is a footgun with a new property bolted on.

D-8's `TryResolveLink` is canonicalisation, which is what real systems want (store canonical, accept legacy input). Ship it prominently, not as a footnote.

D-7 seeds a vocabulary: `Known*` as the Standards-wide prefix for "exists in the shipped registry" (KnownTimeZone today, KnownCurrency, KnownCountry later). Sign it with that intent stated.

D-15 has a canon violation: "Expired/Unexpired" must be `Expired/NotExpired`. Sonnet implementers will ship whatever is written.

If this works better than expected, PineGuard.Standards becomes the tz story for .NET shops that cannot take NodaTime.

D-1: SIGN
D-2: SIGN
D-3: SIGN
D-4: AMEND: `FixedTimeProvider.LocalTimeZone` defaults to `TimeZoneInfo.Utc` (not host zone); add constructor `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo localTimeZone)`
D-5: SIGN
D-6: SIGN
D-7: SIGN
D-8: SIGN
D-9: SIGN
D-10: SIGN
D-11: SIGN
D-13: SIGN
D-14: SIGN
D-15: AMEND: replace `Expired/Unexpired` with `Expired/NotExpired` per the `Not*` canon

MOST DANGEROUS: D-1 - it is the seam every zone-aware rule and the whole "deterministic clock" category claim rests on; wrong here and the upside of all six later phases collapses.

## 3. Anonymization map

| Letter | Advisor |
|---|---|
| A | The Outsider |
| B | The Contrarian |
| C | The Executor |
| D | The First Principles Thinker |
| E | The Expansionist |

## 4. Peer reviews

### Reviewer 1

**1. Strongest: B.** It is the only response that connects the verified facts into failure chains rather than commenting row by row: F-3 + F-12 make D-1 invisible on UTC CI and machine-dependent on Windows dev boxes; F-9 makes `KnownTimeZone` a false superset; D-14's age subtraction mixes clock-zone "today" with caller-offset birthdate and flips at the 18th-birthday boundary. Its D-8 tzdata claim (Europe/Oslo, Amsterdam, Stockholm, Copenhagen became `backward` links in 2022b and vanished from zone1970.tab) I can independently confirm. D is a close second: its observation that `DateTimeKind.Local` binds to `TimeZoneInfo.Local`, not `tp.LocalTimeZone`, so D-1's Kind conversion is ill-defined under a pinned provider, is correct BCL behaviour and unique to D.

**2. Biggest blind spot: E.** It signs D-1, D-3, D-8 and D-13 as-is, frames a determinism breach (D-3) as an upsell funnel, and never engages the F-3/F-12 ordering hazard beyond a D-4 default. "First .NET validation library with an injectable zone-aware clock" is unverifiable marketing, not analysis.

**3. All five missed the DataAnnotations layer.** Attribute arguments must be compile-time constants, so no DA attribute can take a `TimeZoneInfo`. D-13, B/D's "throw at construction", and B's `TimeZoneInfo` overloads all stop at Fluent. DA must take a zone-id string and resolve it via host lookup (D-3) at validation time, which needs its own code (`timezone.id.unknown`) and how DA obtains `TimeProvider` (`ValidationContext.GetService`) is unstated. Second gap: D-1's `TimeZoneInfo.Local` read is itself a D-3 host-registry lookup and owes the same `<remarks>`; "UTC containers" is an assumption defeated by a `TZ` env var.

Row calls: D-1: C — D-4 UTC-default lands first (F-3, F-12); fold in D's Kind.Local caveat (confirmed) · D-2: D — no-back-compat rule voids the "silent change" objection; one calendar per class · D-3: B — F-9/F-12 demand a both-OS test rule, not a paragraph · D-4: C — explicit `(utcNow, TimeZoneInfo? = null)` defaulting UTC; F-3 trap closed · D-5: A — `TryConvertIanaIdToWindowsId` is .NET 6+; confirmed · D-6: B — pre-1.0 deletion is free; directional pair supersedes it · D-7: B — F-9: host and CLDR snapshot diverge both ways; "Known" implies superset · D-8: B — 2022b links confirmed; zone1970.tab-based snapshot rejects Europe/Oslo · D-9: B — F-6 audited two classes only; boundary test across all Guard*Clauses, separate commit · D-10: B — parallel-unit tracker edits conflict; reasoning sound · D-11: B — sign; recording D-12's deletion costs nothing · D-13: D — Fluent throws at configuration (matches FluentValidation's own null-predicate behaviour, confirmed); DA part moot · D-14: B — two calendars in one subtraction; "today" must use the value's offset · D-15: C — `Unexpired` violates the `Not*` canon; B's contrary call is wrong

### Reviewer 2

**1. Strongest: B.** It is the only response that turns the verified facts into a causal chain (F-1 + F-3 + F-12 -> D-1 is invisible on UTC CI and machine-dependent on dev), adds a real external fact on D-8 (tzdata 2022b demoted Europe/Oslo, Amsterdam, Stockholm, Copenhagen to `backward` links; I confirm this from my own knowledge, and browsers/ICU still emit those ids), extends D-9 beyond the two classes F-6 actually audited, and spots the two-calendar subtraction in D-14. Its one flaw: D-15 endorses `Unexpired`, contradicting the stated `Not*` canon.

**2. Biggest blind spot: E.** It signs D-1, D-2, D-3, D-8 and D-13 as-is and reframes the register as a marketing story. It never engages F-5, F-9 or the D-1 Kind-conversion rule, and treats D-8's canonical-only default as a feature when it rejects the ids Scandinavian browsers actually send.

**3. All five missed:** zone1970.tab contains no `Etc/*` rows, so a snapshot-based canonical `IsIanaTimeZoneId` rejects `Etc/UTC` and `UTC` unless the `etcetera` file is ingested alongside `backward`. DataAnnotations attributes cannot take a `TimeZoneInfo` argument, so D-13 is moot for the DA layer, and every zone-aware attribute is forced into a string id plus a host lookup (D-3) at validation time. D-3 semantics diverge by TFM, not just by OS: on .NET 6+ `FindSystemTimeZoneById` silently accepts both IANA and Windows ids under ICU; netstandard2.1 runtimes do not.

Row calls: D-1: D — Kind.Local is machine-zone-relative (confirmed); converting Utc-kind midnight dates shifts days · D-2: D · D-3: B · D-4: D — explicit "fall-through forbidden" · D-5: A · D-6: B · D-7: B · D-8: B · D-9: B · D-10: B · D-11: B · D-13: D · D-14: B · D-15: A — `NotExpired` per canon

Independent fact checks: B's tzdata 2022b claim — confirmed (Oslo/Stockholm/Copenhagen -> Europe/Berlin, Amsterdam -> Europe/Brussels, all in `backward`). A's .NET 6+ claim for `TryConvertIanaIdToWindowsId` — confirmed. D's Kind.Local claim — confirmed (`ToUniversalTime()` always uses the machine zone). Question's claim that `TimeProvider.System.LocalTimeZone` is `TimeZoneInfo.Local` — confirmed.

### Reviewer 3

**1. Strongest: B.** Only response that turns the verified facts into reproducible failures: F-3+F-12 → green on UTC CI, red on a Windows desk after 5pm; F-6 audited only two Guard classes, so D-9 needs a sweep; tzdata 2022b demoted Europe/Oslo/Amsterdam/Stockholm/Copenhagen (and Europe/Kiev→Kyiv) to `backward`. I independently confirm both the tzdata claim and the F-6 scope gap. B alone fixes D-14's two-calendar subtraction. Flaws: D-15 keeps `Unexpired` against canon; D-1's explicit-zone overloads partly revive rejected alt (b).

**2. Biggest blind spot: E.** Describes the F-3 footgun in its D-4 paragraph, then signs D-1 and D-8 as-is; treats a determinism defect as a marketing category. Runner-up A: every fix is an XML sentence; docs do not make `FixedTimeProvider` deterministic.

**3. All five missed:** (a) DataAnnotations attributes accept only compile-time constants — no `TimeZoneInfo` parameter is possible, so every zone-aware DA attribute must take a string id and perform the D-3 host lookup per validation. (b) On netstandard2.1 (F-9) `IsSystemTimeZoneId` can only be `FindSystemTimeZoneById` + catch — exception-driven control flow in a Core predicate — and on .NET 6+ that API accepts both Windows and IANA ids, so "system time zone" is a different set per TFM/OS. (c) D-7's CS0121 premise holds only if Core's `SystemTimeZone(string)` also has optional params (e.g. `paramName`); otherwise C# prefers the candidate needing no default substitution — verify before renaming on that basis.

Row calls: D-1: C — sequence after D-4; formula handles Kind.Local via ToUniversalTime · D-2: D · D-3: B · D-4: D · D-5: A · D-6: B · D-7: B · D-8: B · D-9: B · D-10: B · D-11: B · D-13: D · D-14: B — fixed offset ignores DST, acceptable · D-15: A

Fact checks: B's tzdata claim confirmed. A's .NET 6+ claim confirmed. D's Kind.Local claim confirmed (though C's `ToUniversalTime()` formula handles it, so "not well-defined" is overstated). D's "a link IS a valid tzdata id" — confirmed in practice, though zone1970.tab itself omits them. E's competitor claim — cannot confirm.

### Reviewer 4

**1. Strongest: B.** It is the only response that derives concrete failures from the verified facts rather than restating them: F-3 + F-12 → D-1 is invisible on UTC CI and machine-dependent on dev boxes; F-6 covered only two Guard classes → audit all threshold guards; per-branch tracker edits → parallel-unit merge conflicts (D-10); and the tzdata 2022b fact that sinks canonical-only D-8. Its D-14 fix (today computed in the value's offset) is the one proposal that gives two-point rules a single calendar. Weakness: its D-15 defence of `Unexpired` contradicts the stated `Not*` canon.

**2. Biggest blind spot: E.** It signs D-1, D-3, D-8 and D-13 as-is without testing any row against F-1/F-3/F-9, then names D-1 "most dangerous" — incoherent. Its only substantive amendment (D-4 UTC default) is shared by all five.

**3. All five missed:** (a) a UTC default on `FixedTimeProvider` makes D-1 *untested by default*; the register must mandate non-UTC test zones built with `TimeZoneInfo.CreateCustomTimeZone` (OS-independent under F-12) plus canonical midnight-boundary vectors. (b) netstandard2.1 has no `TryFindSystemTimeZoneById` (F-9), so D-3's host lookup must catch `TimeZoneNotFoundException`/`InvalidTimeZoneException` in a hot Core predicate, and on .NET Framework hosts IANA ids never resolve — the TFM split belongs in the verbatim `<remarks>`.

Row calls: D-1: C · D-2: D · D-3: B · D-4: D · D-5: A · D-6: B · D-7: C — F-11 says a Standards adaptation pass is owed anyway; fold rename in · D-8: B — confirmed (zone1970.tab lists Europe/Berlin for DE,DK,NO,SE) · D-9: B · D-10: B · D-11: B · D-13: D · D-14: B — caveat: fixed offset, not zone · D-15: C

Fact checks: D's Kind.Local claim is correct. A's claim that D-1 matches `DateTime.Today` behaviour (machine zone) is correct for `TimeProvider.System`.

### Reviewer 5

**1. Strongest: B.** B is the only response that brings a checkable external fact that changes a verdict: tzdata 2022b turned Europe/Oslo, Amsterdam, Stockholm, Copenhagen (also Luxembourg, Monaco) into `backward` links. I can confirm this, and I can confirm ICU/CLDR keeps them canonical, so browsers emit them. B also catches the D-14 two-calendar subtraction, F-6's audit scope, and the parallel-tracker conflict in D-10. Its one error: keeping `Unexpired` against the stated `Not*` canon.

**2. Biggest blind spot: E.** Twelve SIGNs. Signs D-1 without connecting F-3/F-12 to sequencing; signs D-8 blind to the link problem; reframes D-3's F-9 failure mode as an "upsell" rather than a red-CI risk.

**3. What all five missed:** F-4 (TimeOnly window cannot cross midnight) has no decision row. zone1970.tab excludes `Etc/UTC`, `UTC`, `Etc/GMT±n`; even B/D's "accept links" fix still rejects `UTC`. D-3 on netstandard2.1: no `TryFindSystemTimeZoneById`, so a never-throws Must clause must catch `TimeZoneNotFoundException` internally. Unanimous D-1 as "most dangerous" is groupthink; D-3 amends a normative spec permanently.

Row calls: D-1: C · D-2: D · D-3: B · D-4: C · D-5: A · D-6: B · D-7: D — keeps rename, fixes rationale to semantic · D-8: B · D-9: B · D-10: B — reasoning, not verified fact · D-11: B · D-13: D · D-14: B · D-15: A

Fact checks: D's Kind.Local claim correct. `TimeProvider.GetLocalNow()` and `TimeProvider.System.LocalTimeZone == TimeZoneInfo.Local` correct. E's "first .NET validation library" claim cannot be confirmed.

## 5. Repository fact-check and orchestrator notes

Verbatim from `peer-reviews-and-facts.md`:

> # Repository fact-check (Haiku read agent, 2026-09-26, both repos) and orchestrator notes
>
> 1. **Negation canon**: `docs/ai/specs/guard-clauses/project.md:205` — "Standardize on `Not*` plus explicit semantic opposites". The only existing `Un*` Must member is `Unspecified` (MustDateTimeClauses.cs:989), which names `DateTimeKind.Unspecified`, not a negation. Existing pairs: `NotFirstDayOfMonth` (MustDateOnlyClauses.cs:688), `NotLastDayOfMonth` (:740), `NotWithinDaysFromNow` (MustDateTimeClauses.cs:524).
> 2. **Adapter null handling today**: FluentValidation adapter uses `ThrowHelper.ThrowIfNull` at ValidationResultExtension.cs:47 and :69; DataAnnotations adapter uses `ThrowHelper.ThrowIfNull` at MustValidationResultExtension.cs:56 and :74. So "adapters throw on null arguments" is already the repo's pattern.
> 3. **DateTimeRules**: `IsSameDay(DateTime?, DateTime?)` (lines 302–310) converts both to UTC then compares `.Date`; `IsWeekday` (250–251) compares `DayOfWeek` on the value as given.
> 4. **spec.md §5.3** has several bullets (lines 267–275): "No IO-dependent validation (filesystem/network/environment) inside Core Rules/Utils", "Prefer BCL Try*", plus clarifications.
> 5. **Standards IANA data** (`DefaultIanaTimeZoneData.cs`, from zone1970.tab, no tzdata version declared): `Europe/Oslo`, `Europe/Amsterdam`, `Europe/Stockholm`, `Europe/Copenhagen`, `US/Eastern` are ALL ABSENT. `America/New_York` present. The charter (F-10) separately records `Etc/UTC` and `Etc/GMT` as present.
> 6. **Standards `Must.Be.SystemTimeZone`**: MustSystemTimeZoneClauses.cs:47–53, forwards to a handler. No public `TryGetSystemTimeZone*`; only internal `CldrTimeZoneUtility.TryGetSystemTimeZone`.
>
> Orchestrator notes (from the charter text, not from advisors):
> - The charter §4.2 ALREADY specifies zone-aware DataAnnotations attributes as taking a `string timeZoneId` (`ValidLocalTimeDateTimeAttribute(string timeZoneId)` etc.) and §6.3 already mandates `TimeZoneInfo.CreateCustomTimeZone` test zones plus a `FixedTimeProvider.DefaultAheadOfUtc` fixture. Reviewers' "all five missed DA" and "untested by default" gaps are therefore partially pre-empted by the charter; the chairman should say what, if anything, still needs to be added to the register.
> - Every PineGuard Must clause ends with `[CallerArgumentExpression] string? paramName = null`, so both `SystemTimeZone` candidates would need optional-parameter substitution and the CS0121 premise in D-7 holds. Reviewer 3's (c) caveat is resolved.
> - The charter §6.4 ALREADY contains the verbatim `<remarks>` paragraph (Executor's D-3 "paragraph does not exist yet" is wrong; it exists but may need the amendments B/D/A ask for).
> - The charter §6.2 ALREADY prescribes `try { FindSystemTimeZoneById } catch (TimeZoneNotFoundException) catch (InvalidTimeZoneException)` for netstandard2.1.
> - F-4 (TimeOnly midnight-crossing) is handled in §4.1 as a Phase 1 fix without a D-row; the chairman may say whether it needs one.
> - D-12 was removed during the pre-council Fable review passes; its content is not recorded in the plan set. Recording a tombstone is cheap.

## 6. Chairman verdict

Verbatim from `chairman-verdict.md`, including the orchestrator corrections section:

> ## Council Verdict: Temporal & Time-Zone Program decisions register (T-00 §3)
>
> De-anonymized: A = Outsider, B = Contrarian, C = Executor, D = First Principles, E = Expansionist. All five reviewers ranked B strongest and E the blind spot; I concur.
>
> ### Where the Council Agrees
>
> - **D-4 is under-specified and is the lever for D-1.** All five advisors independently demanded that `FixedTimeProvider` override `LocalTimeZone` with a UTC default; C, B and D added that it must land *before* D-1. F-3 + F-12 make this the one fully verified failure chain in the packet: a pinned instant with an unpinned zone is green on ubuntu and machine-dependent on every Windows desk.
> - **D-15 contradicts its own canon.** A, C, E, and the fact-check (`guard-clauses/project.md:205`; `Unspecified` is a Kind name, not a negation) agree: `Unexpired` becomes `NotExpired`.
> - **D-1 is the most dangerous row** — five of five. Reviewer 5 calls this groupthink; I accept the warning but not the conclusion: D-1 alone changes observable results of shipped rules on every non-UTC host, and D-3's spec amendment is reversible by a docs commit.
> - **D-5, D-11 sign.** Nobody found a defect; only doc sentences were added.
>
> ### Where the Council Clashes
>
> **D-1: convert `Utc`/`Local` DateTimes into the clock zone (register, C, E) or judge every value on its own `.Date` (D)?** D's argument is the stronger one and every reviewer confirmed its factual core: `DateTimeKind.Local` binds to `TimeZoneInfo.Local`, so C's `ToUniversalTime()` formula re-introduces the machine zone under a pinned provider — the F-3 trap in new clothes. Converting a `Utc`-kind midnight birthdate into a zone behind UTC also shifts it a day (Reviewer 2). **I side with D**, sequenced after D-4 per C. B's explicit `TimeZoneInfo` overloads are rejected: they partially revive rejected alternative (b).
>
> **D-2: keep `IsSameDay(DateTime, DateTime)` on UTC (register, C, E) or unify on own-date (D, B, A)?** The register's only defence is "silently changes results", which is a back-compat argument the project forbids pre-1.0. Five reviewers took D. **I overrule the majority of advisors who signed it.**
>
> **D-8: canonical-only default (register, C, E) or links-by-default (B, D)?** B brought the one externally checkable fact that flips a verdict: tzdata 2022b demoted `Europe/Oslo`, `Amsterdam`, `Stockholm`, `Copenhagen` to `backward` links, and the fact-check confirms all four are absent from `DefaultIanaTimeZoneData.cs`. A canonical-only `IsIanaTimeZoneId` rejects what Scandinavian browsers emit. **REJECT the register.**
>
> **D-14: today from the clock zone (register, C, D, E) or from the value's offset (B)?** B is right that a two-point subtraction needs one calendar; an 18th-birthday outcome must not flip on server location. **I take B**, with the reviewer caveat (fixed offset, not DST-aware) documented.
>
> **D-7: `KnownTimeZone` (register, D, E) or a source-carrying name (A, B)?** B's point that "Known" falsely implies a superset is fair, but `KnownColor`/`KnownFolder` are exact .NET precedents for "in a fixed catalogue", and E's `Known*` prefix seeds Standards-wide vocabulary. **Keep the name; fix the rationale and the docs** (D's amendment, B's fallback clause).
>
> **D-6: keep or delete `IsWithinDaysFromNow`?** All reviewers took B. Pre-1.0 deletion is free and A confirms the name reads future-only to newcomers. **Delete in Phase 4.**
>
> ### Blind Spots the Council Caught
>
> | Gap | Surfaced by | Status |
> |---|---|---|
> | DA attributes cannot take `TimeZoneInfo` | Reviewers 1–3 | **Charter §4.2 covers** (`string timeZoneId`); register D-13 still needs the adapter sentence and how DA resolves `TimeProvider` |
> | UTC-default double leaves D-1 untested by default | Reviewer 4 | **§6.3 covers** (`CreateCustomTimeZone`, `DefaultAheadOfUtc`); add a behind-UTC fixture to D-4 |
> | netstandard2.1 has no `TryFindSystemTimeZoneById` | Reviewers 3–5 | **§6.2 covers**; TFM split still belongs in the D-3 `<remarks>` |
> | Verbatim `<remarks>` "does not exist" (C) | Orchestrator | **§6.4 exists**; amend wording per D-3 below |
> | `Kind.Local` binds to machine zone | D, confirmed ×5 | **Register change** (D-1) |
> | Scandinavian ids are `backward` links, absent from Standards data | B, fact-check 5 | **Register change** (D-8) |
> | Adapters already `ThrowIfNull` | Fact-check 2 | **Register change** (D-13 codifies existing pattern) |
> | `TimeZoneInfo.Local` read is itself a D-3 host lookup; `TZ` env var defeats "UTC containers" | Reviewer 1 | **Register change** (D-1 sentence) |
> | CS0121 premise in D-7 | Reviewer 3 | **Resolved** by orchestrator; holds |
> | F-4 has no D-row | Reviewer 5 | **§4.1 covers**; uncontested bug fix, no row needed |
> | D-12 content unrecorded | B | Tombstone under D-11 |
>
> ### The Recommendation
>
> Amend ten rows, reject two, sign two. Land D-4 before D-1. Treat the Standards adaptation pass (F-11) as the single vehicle for D-7 and D-8.
>
> | Id | Verdict | Final recommendation text to put in the register | Rejected alternatives to add |
> |---|---|---|---|
> | D-1 | AMEND | "Today" = `GetToday(tp) = DateOnly.FromDateTime((tp ?? TimeProvider.System).GetLocalNow().DateTime)`. A DateTime value's calendar date is always `value.Date`, regardless of `Kind`; no conversion into `LocalTimeZone` (one rule with D-2 and D-14; `Kind.Local` binds to `TimeZoneInfo.Local`, not `tp.LocalTimeZone`, so conversion is machine-dependent under a pinned provider). Lands only after D-4. Every affected rule's `<summary>` first sentence states: "'Today' is the calendar date in the TimeProvider's LocalTimeZone (host zone by default); the value is judged on its own calendar date." `GetToday` carries the D-3 `<remarks>` because `TimeZoneInfo.Local` is a host lookup (a `TZ` env var defeats the UTC-container assumption). | (c) Convert `Utc`/`Local` kinds into `LocalTimeZone` — `Kind.Local` conversion needs the machine zone; `Utc`-kind midnight dates shift a day. (d) Explicit `TimeZoneInfo` overloads on today-relative rules — revives (b). |
> | D-2 | REJECT | Change `DateTimeRules.IsSameDay(DateTime?, DateTime?)` to compare `a.Value.Date == b.Value.Date` (Kind ignored), so every DateTimeRules calendar predicate uses the value's own wall-clock date. Document "Kind is ignored" in the `<summary>`. Add `IsSameDay(DateTimeOffset, DateTimeOffset, TimeZoneInfo timeZone)` in Phase 2 as written. | Keep UTC comparison and document — the only defence is back-compat, forbidden pre-1.0. |
> | D-3 | AMEND | Allowed in Core. Spec 5.3 gains a bullet: "Host-registry lookups (culture, time zone) are permitted but not IO; results may differ by OS, ICU version, globalization mode (Invariant/NLS) and TFM (.NET 6+ resolves both IANA and Windows ids under ICU; netstandard2.1/.NET Framework hosts do not resolve IANA ids). Tests for such members use ids valid on both Windows and Linux or are OS-conditioned." The §6.4 verbatim `<remarks>` is amended to name those same failure modes and to name the Standards snapshot clause as the deterministic alternative. Spec and remarks change in the T-02 U2 commit. | Satellite package (as written). "Environment-dependent, not IO" as the remarks wording — internal phrase, names no failure mode. |
> | D-4 | AMEND | Keep `FixedTimeProvider`. New ctor `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)`; `LocalTimeZone` is overridden and returns `localTimeZone ?? TimeZoneInfo.Utc`; fall-through to the host zone is forbidden and tested. Add `DefaultBehindUtc` beside §6.3's `DefaultAheadOfUtc`; every today-relative rule has at least one midnight-boundary vector in each. Lands as its own commit before D-1. `FakeTimeProvider` recommendation as written. | Extend with a settable `LocalTimeZone` that defaults to the host zone — F-3 restated. |
> | D-5 | SIGN | As written. Add: "`TimeZoneInfo.TryConvertIanaIdToWindowsId` is .NET 6+; netstandard2.1 callers need Standards." | — |
> | D-6 | AMEND | Keep unchanged until Phase 4. Phase 4 ships `WithinLast`/`WithinNext` and deletes `IsWithinDaysFromNow`/`NotWithinDaysFromNow` at every layer in the same unit (verify no Standards reference during the F-11 pass). Until then its `<summary>` first sentence reads "in the past or the future". | Rename to `IsWithinDaysOfNow` (as written). Keep permanently — reads future-only; either direction is composable. |
> | D-7 | AMEND | Rename as written (`KnownTimeZone`, `NotKnownTimeZone`, `.KnownTimeZone()`), performed inside the F-11 Standards adaptation pass. Record the rationale as semantic, not CS0121: `SystemTimeZone` consults the host registry; `KnownTimeZone` consults the bundled CLDR/IANA snapshot; neither set is a superset of the other (F-9). `<summary>` first sentence states that contrast. `Known*` is the Standards-wide prefix for "present in the shipped registry". | `CatalogedTimeZone` — no .NET precedent; `Known*` has (`KnownColor`, `KnownFolder`). `IanaOrWindowsTimeZone` — names id syntax, not source. |
> | D-8 | REJECT | Ingest `backward` (and `etcetera` if `Etc/GMT±n` are absent); generate `DefaultIanaTimeZoneLinkData.cs`. `IsIanaTimeZoneId` / `Must.Be.IanaTimeZoneId` accept canonical ids AND links (a link is a valid tzdata id). Add strict opt-in `IsCanonicalIanaTimeZoneId` / `Must.Be.CanonicalIanaTimeZoneId` / `Guard.Against.NotCanonicalIanaTimeZoneId`, code in the existing IANA code family with condition `canonical`, message `"{paramName} must be a canonical IANA time zone id."`. Keep `IIanaTimeZoneProvider.TryResolveLink(string? value, out string? canonicalId)`; document it as the canonicalisation path. Own unit in the F-11 pass. | Canonical-only default with `*OrLink` clauses — rejects `Europe/Oslo` et al. (2022b). `OrAlias` suffix family — same defect, different word. |
> | D-9 | AMEND | Fix as written, but as a standalone commit BEFORE the T-01 rename, with a failing test first (`Guard.Against.LessThan(v, v)` must not throw). Sweep every threshold guard in every `Guard*Clauses` class with a `v == t` boundary test; F-6 audited TimeSpan and Number only. | Fix inside the rename diff — dirty bisect. |
> | D-10 | AMEND | Routines push `claude/temporal-<unit>` branches only, rebased on main before push, and never edit the tracker; the owner updates the tracker on main at merge. Routines never open PRs. | Per-branch tracker edits — conflict when units run in parallel. |
> | D-11 | SIGN | As written. Add tombstone: "D-12 removed during pre-council Fable review; content not retained." | — |
> | D-13 | AMEND | Must clause as written (`MustCodes.TimeZone.Zone.Null`, `"{paramName} must not be null."`, `nameof(timeZone)`). Fluent extensions and DA attribute constructors call `ThrowHelper.ThrowIfNull` on `timeZone`/`timeZoneId` at composition time (existing adapter pattern, ValidationResultExtension.cs:47). DA attributes take `string timeZoneId` (§4.2); an unknown id at validation time fails with `timezone.id.unknown`, `"{paramName} must be a known time zone id."`. DA resolves the clock via `validationContext.GetService(typeof(TimeProvider)) ?? TimeProvider.System`. | `ThrowIfNull` in Must (as written). Argument-fault code `timezone.argument.null` — unnecessary once adapters throw. |
> | D-14 | AMEND | Single-value DateTimeOffset predicates use `value.Date`. Any DateTimeOffset rule comparing the value's date with today's (age, `WithinLast`/`WithinNext`, `Expired`) computes today as `GetUtcNow(tp).ToOffset(value.Offset).Date`, so both ends share one calendar. `<remarks>`: "fixed offset, not DST-aware; for zone-aware comparison use the `TimeZoneInfo` overload." DateTime age rules use D-1's `GetToday`; each doc cross-references the other. | Today from `LocalTimeZone` — two calendars in one subtraction; flips at the birthday boundary by server location. Convert into clock zone (as written). |
> | D-15 | AMEND | As written, with `Expired/Unexpired` replaced by `Expired/NotExpired`. `WithinLast`/`WithinNext` keep pairs (precedent `NotWithinDaysFromNow`). | `Unexpired` — violates `Not*` canon (project.md:205). Drop pairs on comparisons — contradicts existing `NotWithinDaysFromNow`. |
>
> ### The One Thing to Do First
>
> Land D-4 as a standalone commit: `FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)` overriding `LocalTimeZone` to `localTimeZone ?? TimeZoneInfo.Utc`, with a test asserting the default and a `DefaultBehindUtc` fixture — before any T-01 or D-1 work touches `src/`.
>
> ---
>
> ## Orchestrator (Fable) corrections applied before the verdict was written into the plan
>
> 1. **D-14 scope.** The chairman listed `WithinLast`/`WithinNext`/`Expired` as date-comparing rules. For `DateTimeOffset` those are instant comparisons (`value <= now`, `now − span <= value`) with no calendar basis, so the value-offset "today" applies only to the **age rules** (`MinimumAge`, `MaximumAge`, `AgeBetween`), which compare calendar dates in years. The register row is scoped accordingly.
> 2. **D-13 DA clock resolution.** DataAnnotations attributes already resolve the clock through the existing helper `ResolveTimeProvider(validationContext)` (DateOnlyAttributes.cs:40 and siblings). The row cites that helper, not a new `GetService` expression. The unknown-id message reuses the §4.2 wording `{paramName} must be a valid system time zone id.` so the same code carries one message.
> 3. **D-8 code constant.** The exact Standards code constant for the canonical-only clause is a T-03 U2 discovery item (the Standards IANA code family was not read in this session); the row says so instead of inventing a constant.

## 7. Disposition

| Id | Council verdict | Applied to §3 | Owner signature |
|---|---|---|---|
| D-1 | AMEND | Yes | pending |
| D-2 | REJECT | Yes | pending |
| D-3 | AMEND | Yes | pending |
| D-4 | AMEND | Yes | pending |
| D-5 | SIGN | Yes | pending |
| D-6 | AMEND | Yes | pending |
| D-7 | AMEND | Yes | pending |
| D-8 | REJECT | Yes | pending |
| D-9 | AMEND | Yes | pending |
| D-10 | AMEND | Yes | pending |
| D-11 | SIGN | Yes | pending |
| D-13 | AMEND | Yes | pending |
| D-14 | AMEND | Yes | pending |
| D-15 | AMEND | Yes | pending |
</content>
