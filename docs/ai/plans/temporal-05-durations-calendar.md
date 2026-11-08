<!-- metadata_header
type: plan
id: temporal-05-durations-calendar
version: 1.0
status: planned
last_updated: 2026-09-22
-->

# Plan T-05 — Phase 5: Durations, calendar predicates, ISO week, intervals, business days

> Charter: [T-00](temporal-00-program.md). Decisions cited: **D-1, D-2**. Naming: T-00 §4.5.
> Codes: T-00 §5 (`Time.Duration` additions, `Time.Interval`, `Date.Interval`, `Date.Calendar` additions, `Date.Equality` additions).
> Depends on Phase 4 merged (including the Phase 1 TimeSpan rename). Phase 6 waits for this merge.
> Repo: PineGuard. Worktree `.claude/worktrees/temporal-05-calendar`, branch `feature/temporal-05-calendar`.

> Review provenance and exception policy: T-00 §0 and
> [Astra decision register](temporal-astra-decisions.md). Implementation: Codex team. All reviews, decisions and councils: Claude Fable 5.1. [A-01]

## 1. Business plan

`TimeSpan` has three rules; every Options validator needs "non-negative or infinite timeout"; every
scheduler needs "on a 15-minute boundary"; every finance app needs "business day" and "quarter end";
every reporting app needs "same ISO week". These are all pure, deterministic, and cheap.

## 2. Units

| Unit | Title | Model / agent | Depends on | Fan-out | Scopes |
|---|---|---|---|---|---|
| U1 | Codes | Codex team | Phase 4 merged | ∥ U2a/U2b | — |
| U2a | Core: TimeSpan family | Codex team / `validation-builder` | Phase 4 merged | ∥ U2b | `Core` |
| U2b | Core: calendar + ISO week + interval + business day | Codex team / `validation-builder` | Phase 4 merged | ∥ U2a | `Core` |
| U3a | Must: TimeSpan | Codex team | U1, U2a | ∥ U3b | `MustClauses` |
| U3b-1 … U3b-4 | Must: calendar — one dispatch per type file (`MustDateOnlyClauses.cs`, `MustDateTimeClauses.cs`, `MustDateTimeOffsetClauses.cs`, `MustTimeOnlyClauses.cs`) | Codex team | U1, U2b | ∥ U3a and each other | `MustClauses` |
| U4a; U4b-1 … U4b-4 | Guard: `GuardTimeSpanClauses.cs` (U4a); calendar per type file (U4b-n) | Codex team | U3a / U3b-n | ∥ U5 | `GuardClauses` |
| U5a; U5b-1 … U5b-4 | Fluent: `FluentTimeSpanExtensions.cs` (U5a); calendar per type file (U5b-n) | Codex team | U3a / U3b-n | ∥ U4 | `FluentValidation` |
| U5c; U5d-1 … U5d-4 | DataAnnotations: `TimeSpanAttributes.cs` (U5c); calendar per type file (U5d-n) | Codex team | U3a / U3b-n | ∥ U4 | `DataAnnotations` |
| U6 | Phase review + merge | Claude Fable 5.1, orchestrator | all | last | — |

File ownership is disjoint by construction: U2a/U3a touch only `*TimeSpan*` files; U2b/U3b touch
`DateOnlyRules.cs`, `DateTimeRules.cs`, `DateTimeOffsetRules.cs`, `TimeOnlyRules.cs` and their
Must files. `MustCodes.Time.cs` and `MustCodes.Date.cs` are edited by U1 only.

## 3. Unit specifications

### U1 — Codes

`MustCodes.Time.cs`: under `Duration` add `NotPositive`, `NotNegative`, `NotZero`, `Zero`, `Negative`,
`Positive`, `NotWholeSeconds`, `WholeSeconds`, `NotWholeMinutes`, `WholeMinutes`, `NotMultiple`,
`Multiple`, `NotTimeout`; add nested `Interval`
(`Prefix = Time.Prefix + ".interval"`, `OffInterval`, `OnInterval`). `MustCodes.Date.cs`: under `Calendar` add
`NotLeapYear`, `LeapYear`, `NotLeapDay`, `LeapDay`, `NotDayOfWeek`, `DayOfWeek`,
`NotFirstDayOfQuarter`, `FirstDayOfQuarter`, `NotLastDayOfQuarter`, `LastDayOfQuarter`,
`NotMidnight`, `Midnight`, `NotBusinessDay`, `BusinessDay`; under
`Equality` add `NotSameMonth`, `SameMonth`, `NotSameYear`, `SameYear`, `NotSameIsoWeek`, `SameIsoWeek`.
Add Date-domain `Interval` for DateTime/DateTimeOffset only:
`Prefix = Date.Prefix + ".interval"`, `OffInterval = Prefix + ".off-interval"`,
`OnInterval = Prefix + ".on-interval"`. TimeOnly retains Time.Interval.
`OnInterval` / `NotOnInterval` in MustDateTimeClauses and MustDateTimeOffsetClauses use
Date.Interval; MustTimeOnlyClauses uses Time.Interval. The `interval <= 0` guard runs
before negation, reports the owning domain's OffInterval with
`"{paramName} requires a positive interval."`, `nameof(interval)` and failing value interval.
Normal positive/negative failure templates are `"{paramName} must fall on the expected interval."`
and `"{paramName} must not fall on the expected interval."`. Matching DA base codes follow
Date.Interval for DateTime/DateTimeOffset and Time.Interval for TimeOnly.
[Focused Fable audit ruling; no audit exemption]
Budget 40k / 15 min.

### U2a — Core: TimeSpan family

**Files**: `src/PineGuard.Core/Rules/TimeSpanRules.cs`, fixtures `TimeSpanRulesFixtures.cs`, TestData/tests.
No `StringRules.TimeSpan` counterparts in this program (they would need their own §4.5 rows across
four layers; backlog).

```csharp
public static bool IsPositive(TimeSpan? value)       => value is not null && value.Value > TimeSpan.Zero;
public static bool IsNegative(TimeSpan? value)       => value is not null && value.Value < TimeSpan.Zero;
public static bool IsZero(TimeSpan? value)           => value is not null && value.Value == TimeSpan.Zero;
public static bool IsZeroOrPositive(TimeSpan? value) => value is not null && value.Value >= TimeSpan.Zero;
public static bool IsZeroOrNegative(TimeSpan? value) => value is not null && value.Value <= TimeSpan.Zero;
public static bool IsWholeSeconds(TimeSpan? value)   => value is not null && value.Value.Ticks % TimeSpan.TicksPerSecond == 0;
public static bool IsWholeMinutes(TimeSpan? value)   => value is not null && value.Value.Ticks % TimeSpan.TicksPerMinute == 0;
public static bool IsMultipleOf(TimeSpan? value, TimeSpan unit) => value is not null && unit > TimeSpan.Zero && value.Value.Ticks % unit.Ticks == 0;
public static bool IsTimeout(TimeSpan? value)        => value is not null && (value.Value == Timeout.InfiniteTimeSpan || value.Value >= TimeSpan.Zero);
```

Fixtures: boundaries `TimeSpan.Zero`, `TimeSpan.MinValue`, `TimeSpan.MaxValue`, `Timeout.InfiniteTimeSpan`
(−1 ms), `TimeSpan.FromMilliseconds(-2)` (not a timeout), `TimeSpan.FromTicks(1)`, `1s`, `1.5s`, `1m`,
`90s` (whole seconds, not whole minutes), `15m` with unit `5m` (multiple), `16m` with unit `5m` (not),
unit `Zero` (false), unit negative (false), `null`.

Timeout accepts zero for immediate/nonblocking operation, positive values and exactly −1 ms
infinity. This is the semantic timeout domain; API-specific Timer/Task limits are separate and
must not be baked into this generic predicate. Add zero-valid and every other negative-invalid
fixtures. Alignment/multiple helpers retain strictly positive units and exact tick modulo. [A-09]

**Acceptance**: gate `Core`. Budget 120k / 45 min.

### U2b — Core: calendar + ISO week + interval + business day

**TFM correction [Fable corrective council]**: `System.Globalization.ISOWeek` is absent
from netstandard2.1. Guard `IsSameIsoWeek` and its complete Must/Guard/Fluent/DA surface
with the established `#if NET8_0_OR_GREATER` pattern. Expose it on net8/net10 and omit
it from the netstandard2.1 compile surface; apply corresponding guards to tests and
code consumers. The earlier "one code path on every TFM" wording does not apply to
ISOWeek. See `docs/ai/specs/must-clauses/project.md` TFM/BCL-gap contract. No fallback
algorithm is authorized by this correction.

**Files**: `DateOnlyRules.cs`, `DateTimeRules.cs`, `DateTimeOffsetRules.cs`, `TimeOnlyRules.cs`, their
fixtures and TestData/tests (eight files; tests paired).

**Members on each of `DateOnly` (net8+), `DateTime`, `DateTimeOffset`** (`d` = the value's calendar date:
`DateOnly` itself; `DateTime`/`DateTimeOffset` → the wall-clock `.Date`, matching the existing
`IsWeekday`, D-2):

```csharp
IsLeapYear(T? value)                      => value is not null && DateTime.IsLeapYear(d.Year);
IsLeapDay(T? value)                       => d.Month == 2 && d.Day == 29;
IsSameMonth(T? value, T? other)           // null semantics as IsSameDay; compares (Year, Month)
IsSameYear(T? value, T? other)            // compares Year
#if NET8_0_OR_GREATER
IsSameIsoWeek(T? value, T? other)         // compares (ISOWeek.GetYear(dt), ISOWeek.GetWeekOfYear(dt)); DateOnly uses ToDateTime(TimeOnly.MinValue)
#endif
IsDayOfWeek(T? value, DayOfWeek day)      => d.DayOfWeek == day;
IsFirstDayOfQuarter(T? value)             => d.Day == 1 && (d.Month - 1) % 3 == 0;
IsLastDayOfQuarter(T? value)              => d.Month % 3 == 0 && d.Day == DateTime.DaysInMonth(d.Year, d.Month);
IsBusinessDay(DateOnly? value, IReadOnlyCollection<DateOnly>? holidays = null, IReadOnlyCollection<DayOfWeek>? weekend = null)
IsBusinessDay(DateTime? value, IReadOnlyCollection<DateTime>? holidays = null, IReadOnlyCollection<DayOfWeek>? weekend = null)          // holidays compared by .Date
IsBusinessDay(DateTimeOffset? value, IReadOnlyCollection<DateTime>? holidays = null, IReadOnlyCollection<DayOfWeek>? weekend = null)    // d compared to holiday.Date
```

`IsBusinessDay`: `weekend ?? DefaultWeekend`; expose `DefaultWeekend` once on `DateTimeRules`
through truly read-only private-backed storage, never an array accessible by downcast. The other
types reference that storage. False when the day of week is in
`weekend` or the date is in `holidays`; `null` value → false. **On `DateTime` and `DateTimeOffset` only**:
`IsMidnight(T? value) => value is not null && value.Value.TimeOfDay == TimeSpan.Zero` and
`IsOnInterval(T? value, TimeSpan interval) => value is not null && interval > TimeSpan.Zero && value.Value.TimeOfDay.Ticks % interval.Ticks == 0`.
**On `TimeOnly`**: `IsOnInterval(TimeOnly? value, TimeSpan interval)` (same arithmetic on `value.Value.Ticks`).

Validate every `DayOfWeek` input, including entries in weekend collections: invalid enum → Rules
false and Must configuration failure attributed to that parameter. Null weekend means Saturday /
Sunday; an empty weekend means no weekend days; duplicates are harmless. Composition adapters
snapshot supplied collections once. Direct Core callers own their collections and must keep them
stable during the call. Holidays and business days use the value's own wall date, with no host
conversion. Do not introduce mutable global regional policy. [A-07]

**Fixtures** (per type; dates chosen so the same literal serves all three types):
leap `2024-02-29` / non-leap `2023-03-01` / century `1900-02-28` (not leap) / `2000-02-29` (leap);
same month `2026-06-01` & `2026-06-30` (true), `2026-06-30` & `2026-07-01` (false); same year;
ISO week (verified at planning time): `2026-12-31` (Thursday, 2026-W53) & `2027-01-03` (Sunday, 2026-W53)
→ true; `2026-12-31` & `2027-01-04` (Monday, 2027-W01) → false; `2021-01-01` (Friday, 2020-W53) &
`2020-12-28` (Monday, 2020-W53) → true; day of week
`2026-06-15` is Monday; quarter: `2026-04-01` first, `2026-06-30` last, `2026-05-01` neither,
`2026-03-31` last, `2026-12-31` last; business: `2026-06-15` (Mon) true, `2026-06-13` (Sat) false,
`2026-06-15` with holiday `[2026-06-15]` false, `2026-06-13` with weekend `[Friday, Saturday]` false
and `2026-06-14` (Sun) with that weekend true, empty holidays true, null value false; midnight
`2026-06-15T00:00` true, `2026-06-15T00:00:00.0000001` false; interval `10:15` with 15 min true,
`10:20` with 15 min false, `10:20` with 10 min true, interval zero false, interval negative false.
The fixture stores these literals; Claude Fable 5.1 re-checks them with `ISOWeek.GetWeekOfYear`.

**Acceptance**: gate `Core`, both TFMs. Budget 150k / 45 min.

### U3a / U3b-n — Must clauses

U3b-1 is the sole writer of the SameMonth, SameYear and SameIsoWeek entries in
`docs/ai/specs/language/vocabulary.json`. Each uses `absentFrom: ["DataAnnotations"]`
and exact reason `"Cross-property calendar comparison (T-00 §4.5)."`, entered only
when its Must concept lands. Positive and Not* methods share the normalized concept.
No DA attribute, ignoreMethods/aliases workaround, exception or new baseline key is added.
All other adapter layers are present; completed cohort audits enforce them.

Clone `MustNumberClauses` wording for the TimeSpan sign family; `MustDateTimeClauses.SameDay` /
`Weekday` / `FirstDayOfMonth` for the calendar family. Members, messages and codes exactly as
T-00 §4.5, including every `Not*` pair D-15 lists. In `MustTimeSpanClauses` the `Timeout` clause
shadows `System.Threading.Timeout`: write `System.Threading.Timeout.InfiniteTimeSpan` in code and crefs. `BusinessDay(value, holidays = null, weekend = null, paramName)` keeps the optional
collections before `paramName`. `OnDayOfWeek(value, DayOfWeek day, paramName)`. `OnInterval(value, TimeSpan interval, paramName)`.
`Timeout(TimeSpan value, paramName)`. Every test group asserts message and code once.
Budget 150k / 45 min each.

### U4a / U4b-n — Guard clauses

Polarity per T-00 §4.5 (`GuardNumberClauses` mirror for signs; `Not*` → positive Must and positive
→ `Not*` Must for every D-15 pair; e.g. `Guard.Against.LeapYear` → `Must.Be.NotLeapYear`).
Budget 150k / 45 min each.

### U5a / U5b-n — Fluent; U5c / U5d-n — DataAnnotations

Fluent: every Must clause (and its `Not*` pair) as an extension with nullable overload;
`SameMonth`/`SameYear`/`SameIsoWeek` take a literal `other` (no model-expression overloads, T-00 §12).
DA attributes per T-00 §4.5 naming, including the `Not*` attributes D-15 lists
(`<Clause><Type>Attribute`): `PositiveTimeSpanAttribute`, `NegativeTimeSpanAttribute`,
`ZeroTimeSpanAttribute`, `NotZeroTimeSpanAttribute`, `ZeroOrPositiveTimeSpanAttribute`,
`ZeroOrNegativeTimeSpanAttribute`, `WholeSecondsTimeSpanAttribute`, `WholeMinutesTimeSpanAttribute`,
`MultipleOfTimeSpanAttribute(int minutes)`, `TimeoutTimeSpanAttribute`, `LeapYear<T>Attribute`,
`LeapDay<T>Attribute`, `OnDayOfWeek<T>Attribute(DayOfWeek day)`, `FirstDayOfQuarter<T>Attribute`,
`LastDayOfQuarter<T>Attribute`, `MidnightDateTimeAttribute`, `MidnightDateTimeOffsetAttribute`,
`OnIntervalTimeOnlyAttribute(int minutes)`, `OnIntervalDateTimeAttribute(int minutes)`,
`OnIntervalDateTimeOffsetAttribute(int minutes)`, `BusinessDay<T>Attribute` (default weekend, no
holidays; XML docs say so). No DA for `Same*` (cross-property). Budget 150k / 45 min each.
The reviewed DA omissions are declared by U3b-1 in layerAbsences, not baseline debt.
Interval DA base codes are Date.Interval for DateTime/DateTimeOffset and Time.Interval
for TimeOnly; the positive-interval configuration guard precedes complement evaluation.

## 4. Verification and Definition of Done

T-00 §9; Qodana; Sonar; Claude Fable 5.1 phase review. Start only after Phase 4 merges;
Phase 6 starts only after this phase merges. Shared code ownership prevents concurrent phases.
- `Must.Be.Timeout(Timeout.InfiniteTimeSpan).Success && Must.Be.Timeout(TimeSpan.Zero).Success`.
- `DateOnlyRules.IsSameIsoWeek(new(2026,12,31), new(2027,1,3))` is true on net8.0 and net10.0.
- `DateOnlyRules.IsBusinessDay(new(2026,6,14), weekend: [DayOfWeek.Friday, DayOfWeek.Saturday])` is true.
