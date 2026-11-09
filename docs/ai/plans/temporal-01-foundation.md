<!-- metadata_header
type: plan
id: temporal-01-foundation
version: 1.1
status: active
last_updated: 2026-11-08
-->

# Plan T-01 — Phase 1: Foundation (today derivation, test clock, TimeOnly wrap, TimeSpan rename, precision)

> Charter: [T-00](temporal-00-program.md). Decisions cited: **D-1, D-2, D-4, D-9; A-01–A-12**.
> Astra adjudicated the design under owner authorization; actual revised Fable council remains
> a mandatory execution gate. No repeated owner signatures. Repo: PineGuard.
> Worktree `.claude/worktrees/temporal-01-foundation`, branch `feature/temporal-01-foundation`.
> **Status**: active (2026-11-08). Phase 1 units through U5 are committed on `codex/temporal-foundation` (U5 as `6b3cab3`); U6 (review + merge) is in progress. See the [orchestration tracker](temporal-orchestration.md) §1.

> Current authority and exception handling: T-00 §0 and
> [Astra register](temporal-astra-decisions.md). Implementation: Codex team;
> all reviews, decisions and councils: Claude Fable 5.1. [A-01]

## 1. Business plan

Every later phase reads "today" or pins a zone in tests. This phase fixes the one correctness defect
(F-1), gives the test double a zone (F-3), makes night-shift time windows expressible (F-4), removes
the last "this < that" name from the temporal surface and its guard-polarity defect (F-6), and
completes the precision enums (F-8). Public changes include the TimeSpan rename, shared
Testing clock fixtures, foundation TryGetAge helpers, and TimeOnly start/end parameter naming.

## 2. Units

| Unit | Title | Model / agent | Depends on | Fan-out | Coverage scopes |
|---|---|---|---|---|---|
| U0 | `FixedTimeProvider` ctor + zone-aware defaults (D-4) | Codex team / `test-writer` | — | first | `Testing` |
| U1 | Test clock zone + custom test zones | Codex team / `test-writer` | U0 | after U0 | `Testing` |
| U2 | Today derivation (D-1) + same-day calendar fix (D-2) | Codex team / `validation-builder` | U0 | after U0 | `Core` |
| U2b | Must consumers of guarded calendar references | Codex team / `validation-builder` | U2 | after U2 | `MustClauses` |
| U2c | String DateOnly consumers of guarded calendar references | Codex team / `validation-builder` | U2, U2b (accepted) | after U2b (accepted); before U6 and before any T-04 string-DateOnly work | `MustClauses` |
| U3 | TimeOnly midnight-crossing Between/NotBetween | Codex team / `validation-builder` | U1 | ∥ U4, ∥ U4a | `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, `DataAnnotations` (behaviour cases only) |
| U4 | Guard boundary polarity fix (D-9) | Codex team / `validation-builder` | U0 | before U4a; no zone dependency | `GuardClauses` |
| U4a | TimeSpan rename: Core + codes + Must | Codex team / `validation-builder` | U1, U4 | ∥ U3, after U4 | `Core`, `MustClauses` |
| U4b | TimeSpan rename: Guard (renames only, polarity already fixed by U4) + Fluent + DA | Codex team / `validation-builder` | U4a | after U4a | `GuardClauses`, `FluentValidation`, `DataAnnotations` |
| U5 | Precision enum symmetry | Codex team / `validation-builder` | U2, U3 | after both (shared utility and TimeOnly test data) | `Core` |
| U6 | Phase review + merge | Claude Fable 5.1 per unit and phase, orchestrator merge | all | last | — |

U0 lands first because U1's fixtures, U2's fixtures and every later phase's tests construct
`FixedTimeProvider.DefaultAheadOfUtc`/`DefaultBehindUtc` (council D-4, 2026-09-26). U4 lands before
U4a because U4 fixes the guard boundary polarity defect (F-6/D-9) while the Guard clauses still call
the pre-rename Must clause names (`Must.Be.GreaterThan`/`Must.Be.LessThan`); U4a then renames those
Must/Core names, and U4b (unchanged from before) carries the already-correct complement-inclusion
logic through the Guard/Fluent/DA rename. Fixing polarity inside the rename diff would make the
defect and the rename indistinguishable in `git blame` and in review (D-9 explicitly forbids this).

Every build unit is followed by a Claude Fable 5.1 review dispatch named `t01-u<n>-review` whose prompt is
the orchestration §2.3 template. U6 is the Claude Fable 5.1 phase review (orchestration §2.4) and the merge.

## 3. Unit specifications

### U0 — `FixedTimeProvider` ctor + zone-aware defaults (D-4)

**Files** (all under `tests/`):

| Action | File |
|---|---|
| Edit | `tests/PineGuard.Testing/Common/FixedTimeProvider.cs` |
| Add | `+ tests/PineGuard.Testing/Common/CountingTimeProvider.cs` |
| Add | `+ tests/PineGuard.Testing/Common/AdvancingTimeProvider.cs` |
| Add | `+ tests/PineGuard.Testing.UnitTests/Common/CountingTimeProviderTests.cs`, `CountingTimeProviderTestData.cs` |
| Add | `+ tests/PineGuard.Testing.UnitTests/Common/AdvancingTimeProviderTests.cs`, `AdvancingTimeProviderTestData.cs` |
| Add | `+ tests/PineGuard.Testing/Common/TestTimeZones.cs` (this unit adds only `PlusTen` and `MinusEight`; U1 extends the same file with the DST/platform members) |
| Edit | `tests/PineGuard.Testing.UnitTests/Common/FixedTimeProviderTests.cs`, `FixedTimeProviderTestData.cs` |

**Clone from**: the existing `FixedTimeProviderTests.cs` / `FixedTimeProviderTestData.cs` pair.

**Exact output** — `FixedTimeProvider.cs`:

```csharp
/// <param name="utcNow">The instant this provider reports, normalized to UTC regardless of the supplied offset.</param>
/// <param name="localTimeZone">The zone <see cref="TimeProvider.LocalTimeZone"/> reports. If <see langword="null"/>, <see cref="TimeZoneInfo.Utc"/>.</param>
public sealed class FixedTimeProvider(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)
    : TimeProvider
{
    public static readonly FixedTimeProvider Default = new(new DateTimeOffset(2026, 06, 15, 12, 0, 0, TimeSpan.Zero));

    /// <summary>2026-06-15T20:00:00Z in a UTC+10 zone: the local calendar date is 2026-06-16, one day ahead of the UTC date.</summary>
    public static readonly FixedTimeProvider DefaultAheadOfUtc = new(new DateTimeOffset(2026, 06, 15, 20, 0, 0, TimeSpan.Zero), TestTimeZones.PlusTen);

    /// <summary>2026-06-15T04:00:00Z in a UTC−8 zone: the local calendar date is 2026-06-14, one day behind the UTC date.</summary>
    public static readonly FixedTimeProvider DefaultBehindUtc = new(new DateTimeOffset(2026, 06, 15, 4, 0, 0, TimeSpan.Zero), TestTimeZones.MinusEight);

    private readonly DateTimeOffset fixedUtcNow = utcNow.ToUniversalTime();

    public override DateTimeOffset GetUtcNow() => fixedUtcNow;

    public override TimeZoneInfo LocalTimeZone => localTimeZone ?? TimeZoneInfo.Utc;
}
```

`TestTimeZones.cs` (namespace `PineGuard.Testing.Common`, `public static class TestTimeZones`) — this
unit's slice only; U1 appends the rest to the same file:

`FixedTimeProvider` freezes wall-clock reads only. Its inherited GetTimestamp/CreateTimer behavior
is real elapsed-time/timer behavior; document this rather than presenting it as a virtual scheduler.
Advancement/timer tests may use Microsoft.Extensions.TimeProvider.Testing FakeTimeProvider under
test-dependency precedent. No production dependency or second clock abstraction is introduced.
Add nonzero-input-offset normalization and counting/advancing-provider read-contract fixtures. [A-02, A-03]

**Named read-contract fixture [Fable corrective council]**: U0 also produces public
`PineGuard.Testing.Common.CountingTimeProvider` and `AdvancingTimeProvider` in separate
declared files, used by Core, Must and integration tests. Both are sealed TimeProvider types.
Counting ctor: `(DateTimeOffset utcNow, TimeZoneInfo? localTimeZone = null)`; advancing
ctor: `(IReadOnlyList<DateTimeOffset> sequence, TimeZoneInfo? localTimeZone = null)`.
Both expose read-only `int UtcNowCallCount` and fixed `LocalTimeZone` (null → UTC).
GetUtcNow increments the count and returns the fixed UTC-normalized value or next
UTC-normalized sequence value; AdvancingTimeProvider throws InvalidOperationException
when exhausted. Every predicate test asserts exactly one read; a one-item advancing sequence
fails loudly on resampling. Tests cover constructors, normalization, counts and exhaustion.
No virtual timestamp/timer behavior is promised. Rejected:
unnamed private copies (cannot share across projects), replacement of FixedTimeProvider
(changes its frozen-clock contract), and a production clock abstraction (A-02).

```csharp
/// <summary>A fixed UTC+10 zone with no daylight saving.</summary>
public static readonly TimeZoneInfo PlusTen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.PlusTen", TimeSpan.FromHours(10), "PineGuard Test +10", "PineGuard Test +10");

/// <summary>A fixed UTC−8 zone with no daylight saving.</summary>
public static readonly TimeZoneInfo MinusEight = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.MinusEight", TimeSpan.FromHours(-8), "PineGuard Test -8", "PineGuard Test -8");
```

§6.3 of T-00 does not itself spell out concrete offsets; it defers to "T-01 U1" (now U0) for the
zones behind `DefaultAheadOfUtc`/`DefaultBehindUtc`. `+10:00`/`−08:00` are used here (not the
`+10:00`/`−05:00` fallback pair) because every downstream unit in this plan (U2's fixtures, and the
worked examples in T-00 §4.1/§4.4) already computes its expected values against `PlusTen`/`MinusEight`
by name; introducing a third offset pair here would silently invalidate that arithmetic.

**Test to add** (`[Theory]` + `TheoryData`; TestData next to test): a single test asserting the UTC
default and that the host zone is never consulted —

| Test | Cases |
|---|---|
| `FixedTimeProviderTests.LocalTimeZone_BehavesAsExpected` | `Default` → `TimeZoneInfo.Utc`; `DefaultAheadOfUtc` → `TestTimeZones.PlusTen`; `DefaultBehindUtc` → `TestTimeZones.MinusEight`; `new FixedTimeProvider(x, null)` → `Utc` (proves the fall-through is always `Utc`, never `TimeZoneInfo.Local`) |

**Acceptance**: T-00 §9 gate with `<Scope>` = `Testing`. Budget 60k / 20 min.

### U1 — Test clock zone + custom test zones

**Files** (all under `tests/`):

| Action | File |
|---|---|
| Edit | `tests/PineGuard.Testing/Common/TestTimeZones.cs` (extends the `PlusTen`/`MinusEight` file from U0) |
| Edit | `tests/PineGuard.Testing.UnitTests/Common/FixedTimeProviderTests.cs`, `FixedTimeProviderTestData.cs` (adds `GetLocalNow_BehavesAsExpected` only; `LocalTimeZone_BehavesAsExpected` already landed in U0) |
| Add | `+ tests/PineGuard.Testing.UnitTests/Common/TestTimeZonesTests.cs`, `TestTimeZonesTestData.cs` |

**Clone from**: the existing `FixedTimeProviderTests.cs` / `FixedTimeProviderTestData.cs` pair.

**Exact output** — append to `TestTimeZones.cs` (namespace `PineGuard.Testing.Common`,
`public static class TestTimeZones`; `PlusTen`/`MinusEight` already exist from U0):

```csharp
/// <summary>
/// A UTC+1 zone that observes daylight saving on European rules: clocks go forward at 02:00 on the
/// last Sunday of March and back at 03:00 on the last Sunday of October. In 2026 that is 29 March
/// (02:00–02:59 do not exist) and 25 October (02:00–02:59 occur twice).
/// </summary>
public static readonly TimeZoneInfo DaylightSaving = TimeZoneInfo.CreateCustomTimeZone(
    "PineGuard.Test.DaylightSaving", TimeSpan.FromHours(1), "PineGuard Test DST", "PineGuard Test Standard", "PineGuard Test Daylight",
    [
        TimeZoneInfo.AdjustmentRule.CreateAdjustmentRule(
            DateTime.MinValue.Date, DateTime.MaxValue.Date, TimeSpan.FromHours(1),
            TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 2, 0, 0), 3, 5, DayOfWeek.Sunday),
            TimeZoneInfo.TransitionTime.CreateFloatingDateRule(new DateTime(1, 1, 1, 3, 0, 0), 10, 5, DayOfWeek.Sunday))
    ]);

/// <summary>An invalid local time in <see cref="DaylightSaving"/>: 2026-03-29 02:30 falls in the spring-forward gap.</summary>
public static readonly DateTime InvalidLocalTime = new(2026, 3, 29, 2, 30, 0, DateTimeKind.Unspecified);

/// <summary>An ambiguous local time in <see cref="DaylightSaving"/>: 2026-10-25 02:30 occurs twice.</summary>
public static readonly DateTime AmbiguousLocalTime = new(2026, 10, 25, 2, 30, 0, DateTimeKind.Unspecified);

/// <summary>A local time in <see cref="DaylightSaving"/> that is neither invalid nor ambiguous.</summary>
public static readonly DateTime OrdinaryLocalTime = new(2026, 6, 15, 12, 0, 0, DateTimeKind.Unspecified);

/// <summary>An identifier that resolves on this host: a Windows id on Windows, an IANA id elsewhere (US Pacific). Used only by the tests that must resolve a real zone.</summary>
public static readonly string PlatformSampleId = OperatingSystem.IsWindows() ? "Pacific Standard Time" : "America/Los_Angeles";

/// <summary>Invalid in <see cref="PlatformSampleId"/>: 2026-03-08 02:30 falls in the US spring-forward gap (second Sunday of March).</summary>
public static readonly DateTime PlatformInvalidLocalTime = new(2026, 3, 8, 2, 30, 0, DateTimeKind.Unspecified);

/// <summary>Ambiguous in <see cref="PlatformSampleId"/>: 2026-11-01 01:30 occurs twice (first Sunday of November).</summary>
public static readonly DateTime PlatformAmbiguousLocalTime = new(2026, 11, 1, 1, 30, 0, DateTimeKind.Unspecified);

/// <summary>A daylight-saving instant for the supported platform sample zone.</summary>
public static readonly DateTimeOffset PlatformDaylightSavingInstant = new(2026, 7, 1, 12, 0, 0, TimeSpan.FromHours(-7));

/// <summary>A standard-time instant for the supported platform sample zone.</summary>
public static readonly DateTimeOffset PlatformStandardTimeInstant = new(2026, 1, 15, 12, 0, 0, TimeSpan.FromHours(-8));

/// <summary>An identifier no host resolves.</summary>
public const string UnknownId = "PineGuard/Nowhere";
```

**Tests to add** (all `[Theory]` + `TheoryData`; TestData next to tests):

| Test | Cases |
|---|---|
| `FixedTimeProviderTests.GetLocalNow_BehavesAsExpected` | `DefaultAheadOfUtc.GetLocalNow()` == `2026-06-16T06:00+10:00`; `DefaultBehindUtc.GetLocalNow()` == `2026-06-14T20:00-08:00`; `Default.GetLocalNow()` == `2026-06-15T12:00+00:00` |
| `TestTimeZonesTests.Zones_HaveExpectedShape` | `PlusTen.BaseUtcOffset == 10h`, `SupportsDaylightSavingTime == false`; `MinusEight` likewise; `DaylightSaving.SupportsDaylightSavingTime == true` |
| `TestTimeZonesTests.DaylightSaving_ClassifiesLocalTimes` | `IsInvalidTime(InvalidLocalTime) == true`, `IsAmbiguousTime(AmbiguousLocalTime) == true`, `IsInvalidTime(OrdinaryLocalTime) == false`, `IsAmbiguousTime(OrdinaryLocalTime) == false` |
| `TestTimeZonesTests.PlatformSampleId_Resolves` | `TimeZoneInfo.FindSystemTimeZoneById(PlatformSampleId)` does not throw; `UnknownId` throws `TimeZoneNotFoundException` |
| `TestTimeZonesTests.PlatformSample_ClassifiesLocalTimes` | the resolved zone reports `IsInvalidTime(PlatformInvalidLocalTime) == true` and `IsAmbiguousTime(PlatformAmbiguousLocalTime) == true` |

Add a custom zone with a half-hour transition and prove gap/overlap boundaries using explicit
adjustment rules. Exact Fable contract: `TestTimeZones.HalfHourDaylightSaving`, base +10:30,
delta +00:30, forward 02:00 first Sunday of October, back 02:00 first Sunday of April.
Add `HalfHourInvalidLocalTime = 2026-10-04 02:15 Unspecified`,
`HalfHourAmbiguousLocalTime = 2026-04-05 01:45 Unspecified`,
`HalfHourOrdinaryLocalTime = 2026-06-15 12:00 Unspecified`. Add rows to
`TestTimeZonesTests.DaylightSaving_ClassifiesLocalTimes` and the T-02 gap/overlap fixtures.
Keep host-dependent platform resolution/classification cases identified as
integration tests with runtime/OS/globalization prerequisites, separate from deterministic custom
zone correctness tests. Do not assume every host has the sample zone installed. [A-05, A-10]

**Acceptance**: T-00 §9 gate with `<Scope>` = `Testing`. Budget 100k / 30 min.

**Source-review follow-up (2026-09-27):** Claude Fable 5.1's bounded source review found two required
corrections before the unit gate: fix the `HalfHourDaylightSaving` fall-back XML range to
01:30–01:59 and remove the silent host-zone skip from `PlatformSample_ClassifiesLocalTimes`, making
both platform-test summaries state the fail-fast host prerequisite. The source review is not unit
acceptance; gates remain pending. Exact evidence and instructions are recorded in
[`temporal-u1-fable-source-review.md`](temporal-u1-fable-source-review.md), with the public result
at `artifacts/reviews/T1-U1-fable-source-review.json`.

### U2 — Today derivation (D-1) + same-day calendar fix (D-2)

**Named host integration check (U2 ownership)**: U2 adds
`DateTimeUtilityTests.TryGetTodayDate_NullProviderUsesUtcDate` (named
`…UsesSystemLocalTimeZone` until the D-1 ratification of 2026-11-09) and paired
data in `DateTimeUtilityTestData.cs`. Run it in a dedicated Linux CI process with
`TZ=Pacific/Auckland` (non-UTC) once, alongside the normal UTC jobs. Bracket the system
instant reads to tolerate midnight rollover and assert the derived **UTC** calendar date, proving
the null-provider path is independent of the process zone; do not change the running process's
global zone or infer this result from FixedTimeProvider.
List `.github/workflows/ci.yml` as U2's CI file ownership. [A-03; Fable corrective council]

**Atomic acceptance cohort**: U2 and U2b share one pre-merge gate. U2 is not independently commit-ready or mergeable: Date.Reference.OutOfRange gains its first Must consumers in U2b before the complete gate and audit must-codes run. All Brain checks run for this combined cohort; Phase 1 cannot merge without U2b and a passing audit.

**Files**:

| Action | File |
|---|---|
| Edit | `src/PineGuard.Core/Utils/DateTimeUtility.cs` |
| Edit | `.github/workflows/ci.yml` (dedicated non-UTC system-provider integration check) |
| Edit | `+ src/PineGuard.Core/Utils/DateTimeOffsetUtility.cs` (`TryGetDateAtOffset`; paired boundary tests below) |
| Edit | `src/PineGuard.Core/Codes/MustCodes.Date.cs` (`Date.Reference.OutOfRange`; first consumers in U2b before the Phase 1 merge) |
| Edit | `+ tests/PineGuard.Core.UnitTests/Utils/DateTimeOffsetUtilityTests.cs`, `DateTimeOffsetUtilityTestData.cs` |
| Edit | `src/PineGuard.Core/Rules/DateOnlyRules.cs` (`IsInPast`, `IsInFuture`, `HasMinimumAge`) |
| Edit | `src/PineGuard.Core/Rules/DateTimeRules.cs` (`HasMinimumAge` and `IsSameDay` only) |
| Edit | tests: `tests/PineGuard.Core.UnitTests/Utils/DateTimeUtilityTests.cs` + `TestData`, `tests/PineGuard.Core.UnitTests/Rules/DateOnlyRulesTestData.cs`, `DateTimeRulesTestData.cs`, `tests/PineGuard.Testing/Fixtures/DateOnlyRulesFixtures.cs`, `DateTimeRulesFixtures.cs` |

D-2's `IsSameDay` change stays inside this unit rather than a separate one: it edits the same
`DateTimeRules.cs`, `DateTimeRulesTestData.cs` and `DateTimeRulesFixtures.cs` files D-1 already
touches, and the combined file list (5 source, 8 test/fixture files) is still under the §7.2 caps
(≤ 8 source, ≤ 12 test).

**Clone from**: `DateTimeUtility.GetUtcNow` (docs style), existing `HasMinimumAge` and `IsSameDay`
fixtures.

**Shared safe projection helper [A-07, A-10]**: this foundation unit also owns
`src/PineGuard.Core/Utils/DateTimeOffsetUtility.cs` and its paired utility boundary test data/tests.
Add internal `TryGetDateAtOffset(DateTimeOffset instant, TimeSpan offset, out DateTime date)`;
verify offset is whole-minute within ±14h and UTC ticks plus offset remain within DateTime bounds
before projecting. Success returns Kind.Unspecified midnight wall date; failure returns false/out
default. It reads no clock. Test Min/Max with both offset directions, exact endpoints, invalid
offsets, ordinary dates and Kind. Use existing internal-testing precedent, never expose a public
method merely for tests. Exact precedent: `src/PineGuard.Core/PineGuard.Core.csproj` grants
`InternalsVisibleTo` to Core.UnitTests; `tests/PineGuard.Core.UnitTests/Utils/NetworkUtilityTests.cs`
directly tests the internal helpers in `src/PineGuard.Core/Utils/NetworkUtility.cs`.
The same Core.csproj already grants friend access to `PineGuard.MustClauses` (line 19),
so U2b can invoke guarded internal evaluation; do not add a duplicate friend attribute.
T-02 zone-day comparison and T-04 age consume this helper; neither adds
its own duplicate implementation or depends on a later phase.

Also add internal `DateTimeUtility.TryGetTodayDate(TimeProvider? timeProvider, out DateTime today)`:
sample GetUtcNow once (the system clock when the provider is null); for an injected provider read
its LocalTimeZone and obtain that zone's offset at the sampled instant once, for a null provider use
offset zero (the UTC calendar date; D-1 as ratified 2026-11-09), then call TryGetDateAtOffset. New today/calendar-age/DateOnly
relative predicates use the guarded reference; they must not accept unverified GetLocalNow boundary behavior at
range extremes. Existing unrelated public GetLocalNow behavior stays unchanged. Must evaluates
this single sampled reference through existing internal-rule precedent without resampling.
Register `MustCodes.Date.Reference.OutOfRange` here using computed Date.Reference prefix,
message `"{paramName} requires a reference date within the supported range."`, attributed to
nameof(timeProvider). T-04 consumes this code rather than registering it again. Counting/advancing
provider fixtures assert one read at every user-facing predicate boundary. [A-07, A-10, A-12]

**Foundation age helper [A-07; Fable corrective council]**: U2 owns both
`DateTimeUtility.TryGetAge(DateOnly? birthDate, DateOnly asOf, out int years)`
(`NET8_0_OR_GREATER`) and `TryGetAge(DateTime? birthDate, DateTime asOf, out int years)`
(all supported compile targets, using `.Date`), with tests in the existing
`DateTimeUtilityTests.cs` / `DateTimeUtilityTestData.cs` pair. Return false/out zero for
null or future birth dates. Compute the calendar-year difference and subtract one when
the birthday's month/day has not occurred in the reference year; compare month/day directly
instead of applying `AddYears` to an arbitrary threshold. Preserve the March-1 observation
for 29-February: 2008-02-29 is 17 on 2026-02-28 and 18 on 2026-03-01. Document this as
PineGuard's calendar convention. T-04 consumes these helpers; it does not introduce them.

**Exact output** — add to `DateTimeUtility`:

```csharp
/// <summary>Reads the current instant expressed in the clock's <see cref="TimeProvider.LocalTimeZone"/>.</summary>
public static DateTimeOffset GetLocalNow(TimeProvider? timeProvider) =>
    (timeProvider ?? TimeProvider.System).GetLocalNow();

// Do not add public GetTodayDate/GetToday wrappers. New predicates use the internal guarded
// TryGetTodayDate defined above, converting its date to DateOnly on supported targets. [A-07]
```

Per the council's 2026-09-26 amendment to D-1, there is no `ToCalendarDate(DateTime?, TimeZoneInfo)`
helper any more: a `DateTime` value's own calendar date is always `value.Date`, regardless of
`Kind`, and is never converted into the clock's `LocalTimeZone`. Only "today" (the right-hand side
of the comparison) reads the clock; the value being judged never does. Do not add `ToCalendarDate`
to `DateTimeUtility` — if a prior draft or a stale worktree file still has it, remove it as part of
this unit.

Rule reference paths after the change (retain existing value/threshold guards and use the guarded
reference). Must passes that captured date to the internal evaluation path rather than rereading:

```csharp
// DateOnlyRules
if (!DateTimeUtility.TryGetTodayDate(timeProvider, out var referenceDate)) return false;
var today = DateOnly.FromDateTime(referenceDate);
IsInPast:      value is not null && IsBefore(value, today, inclusion);
IsInFuture:    value is not null && IsAfter(value, today, inclusion);
HasMinimumAge: use TryGetAge with the same captured today and compare the completed age to years

// DateTimeRules.HasMinimumAge — no Kind switch; the birth date's own .Date is compared directly
if (!DateTimeUtility.TryGetTodayDate(timeProvider, out var today)) return false;
if (!DateTimeUtility.TryGetAge(value!.Value.Date, today, out var age)) return false;
return age >= years;

// DateTimeRules.IsSameDay (D-2) — Kind ignored on both sides, no UTC normalisation
if (value is null && other is null) return true;
if (value is null || other is null) return false;
return value.Value.Date == other.Value.Date;
```

Update the `<remarks>` on the four D-1 members (`DateOnlyRules.IsInPast`, `IsInFuture`,
`HasMinimumAge`, and `DateTimeRules.HasMinimumAge`): replace "UTC" wording with "the clock's local
calendar date, derived from one sampled instant with checked projection". Do not touch `IsWithinDaysFromNow` (instant
arithmetic, D-6). State explicitly that null reads the system clock and uses the UTC calendar date
(PineGuard never guesses the host's zone); inject a provider to evaluate today in a chosen zone.
(Until the owner's D-1 ratification of 2026-11-09 this sentence read "today follows the host zone";
the UTC default was adopted on the 2026-11-08 council's recommendation and the remarks on
`DateOnlyRules`, `DateTimeRules` and the 15 DataAnnotations attributes were reworded in the same
commit.) [A-03; Fable corrective council; council 2026-11-08]

Update `DateTimeRules.IsSameDay`'s `<summary>`/`<remarks>` (D-2): first sentence states the two
values are compared "by their own calendar date; `Kind` is ignored" (drop the old "in UTC" wording
entirely — this rule no longer normalises anything). `Must.Be.SameDay`/`NotSameDay`, the Guard
clauses that call them, the Fluent extensions and the message/code stay exactly as they are; only
the Core predicate's semantics and doc change.

**Fixtures to add** (single-line entries; names are the fixture field names):

| Group | Field | Value | Clock | Expected |
|---|---|---|---|---|
| `DateOnlyRulesFixtures.IsInPast` | `LocalYesterdayAheadOfUtc` | `2026-06-15` | `DefaultAheadOfUtc` | true (local today is 06-16) |
| | `LocalTodayAheadOfUtc` | `2026-06-16` | `DefaultAheadOfUtc` | false |
| | `LocalTodayBehindUtc` | `2026-06-14` | `DefaultBehindUtc` | false |
| | `UtcDateIsTomorrowBehindUtc` | `2026-06-15` | `DefaultBehindUtc` | false (it is still 06-14 locally) |
| `DateOnlyRulesFixtures.IsInFuture` | `LocalYesterdayAheadOfUtc` | `2026-06-15` | `DefaultAheadOfUtc` | false |
| | `LocalTodayAheadOfUtc` | `2026-06-16` | `DefaultAheadOfUtc` | false (today is not in the future under `Exclusive`) |
| | `LocalTodayBehindUtc` | `2026-06-14` | `DefaultBehindUtc` | false |
| | `UtcDateIsTomorrowBehindUtc` | `2026-06-15` | `DefaultBehindUtc` | true (locally it is still 06-14) |
| `DateOnlyRulesFixtures.HasMinimumAge` | `EighteenTodayAheadOfUtc` | `2008-06-16`, 18 | `DefaultAheadOfUtc` | true |
| | `EighteenTomorrowAheadOfUtc` | `2008-06-17`, 18 | `DefaultAheadOfUtc` | false |
| `DateTimeRulesFixtures.HasMinimumAge` | `UnspecifiedAtDefault` | `2008-06-15 00:00 Unspecified`, 18 | `Default` | true (reproduces the pre-D-1 result: 18th birthday exactly today) |
| | `UtcKindAtDefault` | `2008-06-15 00:00 Utc`, 18 | `Default` | true (same as `Unspecified`: `Kind` never mattered while the clock zone was UTC) |
| | `LocalKindAtDefault` | `2008-06-15 00:00 Local`, 18 | `Default` | true (pre-D-1 this branch called `TimeZoneInfo.ConvertTime` against the *host* zone and was non-deterministic across machines; post-D-1 it is `value.Date`, same as the other two Kinds, deterministically) |
| | `UnspecifiedBoundaryAheadOfUtc` | `2008-06-16 00:00 Unspecified`, 18 | `DefaultAheadOfUtc` | true (local today is 06-16; exactly 18) |
| | `UtcKindNoLongerConvertedAheadOfUtc` | `2008-06-16 20:00 Utc`, 18 | `DefaultAheadOfUtc` | true — **was `false` pre-D-1**: the old rule converted this UTC instant into `+10` (→ 2008-06-17, one day short); the new rule reads `value.Date` literally (2008-06-16), which meets the boundary |
| | `OneDayShortBehindUtc` | `2008-06-15 02:00 Utc`, 18 | `DefaultBehindUtc` | false — **was `true` pre-D-1**: the old rule converted this UTC instant into `−8` (→ 2008-06-14, meeting the boundary); the new rule reads `value.Date` literally (2008-06-15), which is one day short of local today (2026-06-14 minus 18 years = 2008-06-14) |
| Internal guarded reference test data (`TryGetTodayDate`, using existing internal-testing precedent) | `AheadOfUtc` → `2026-06-16`; `BehindUtc` → `2026-06-14`; `Default` → `2026-06-15`; extreme projection → false/out default. Null-provider fallback is pinned by TryGetTodayDate_NullProviderUsesUtcDate in a dedicated non-UTC CI process, asserting the bracketed UTC calendar date (D-1 as ratified 2026-11-09). | | | |
| `DateTimeRulesFixtures.IsSameDay` | `MixedKindUnspecifiedAndUtcSameDate` | `2026-06-15 23:30 Unspecified`, `2026-06-15 05:00 Utc` | — | true (unchanged pre/post D-2: neither Kind was ever converted by the old rule either, since `DateTimeUtility.ToUtc` only shifts `Local`) |
| | `MixedKindLocalAndUnspecifiedNowDeterministic` | `2026-06-15 23:30 Local`, `2026-06-15 23:30 Unspecified` | — | true, and now deterministic on every host — **pre-D-2 this pair's result depended on the actual host's `TimeZoneInfo.Local`** (the `Local`-Kind side was silently converted via `DateTime.ToUniversalTime()`), so it could not be pinned as a fixture at all; this is the case D-2 exists to fix |

`DateTimeRulesFixtures.IsSameDay`'s existing null-handling cases (`both null` → true, `one null` →
false) are unchanged.

**Acceptance**: gate with `<Scope>` = `Core`; all existing DateOnly/DateTime tests still pass
unchanged (the pinned default clock is UTC) **except** the two named `HasMinimumAge` reversals above,
which the unit's report must call out explicitly as intentional (D-1). Budget 150k / 45 min.

### U2b — Must consumers of guarded calendar references

**Depends on**: U2. This unit completes before the Phase 1 merge and the `must-codes` audit;
`Date.Reference.OutOfRange` is not an unconsumed codes-only exception. [Fable corrective council]

**Files**: edit `src/PineGuard.MustClauses/MustDateOnlyClauses.cs` and
`src/PineGuard.MustClauses/MustDateTimeClauses.cs`; edit each matching test/TestData pair
under `tests/PineGuard.MustClauses.UnitTests/` using the existing class placement.

Wire the calendar-reference clauses to the same captured reference used by the Core evaluation: for
DateOnly, `Must.Be.Past`, `PastOrPresent`, `Future`, `FutureOrPresent` and `MinimumAge`; for DateTime,
`MinimumAge` only. DateTime `Past`, `PastOrPresent`, `Future` and `FutureOrPresent` are instant
comparisons against `GetUtcNow` (D-6, A-06) and are unchanged by this unit. Validate configuration
first, sample the provider once,
using `DateTimeUtility.TryGetTodayDate(timeProvider, out referenceDate)`. After success,
DateOnly Past/Future uses `DateOnlyRules.IsBefore` / `IsAfter` with
`DateOnly.FromDateTime(referenceDate)` and the requested inclusion. MinimumAge calls the
foundation `DateTimeUtility.TryGetAge` with the captured reference (DateOnly conversion
on supported targets, DateTime `.Date` otherwise) and compares the completed age to years.
Do not call a clock-reading rule again. Projection
failure emits `MustCodes.Date.Reference.OutOfRange`, attributed to `nameof(timeProvider)`,
with `"{paramName} requires a reference date within the supported range."`; it must never be
reported as a value that is not past, future or old enough. See `docs/ai/specs/spec.md`
§5.4.2 and `docs/ai/specs/must-clauses/project.md` secondary-parameter validation contract.

Tests cover both projection extremes, ordinary semantics, dependency attribution, exact
code/message, and one-read behavior with `CountingTimeProvider`. Run the MustClauses gate
and `audit must-codes` before merging Phase 1. Budget 100k / 30 min.

### U2c — String DateOnly consumers of guarded calendar references

**Binding ruling (2026-09-27):** this unit is created by Claude Fable 5.1's follow-up ruling on the
T1-U2/U2b string-DateOnly gap, recorded verbatim in
[`../../../artifacts/reviews/T1-U2-U2b-string-followup-ruling.json`](../../../artifacts/reviews/T1-U2-U2b-string-followup-ruling.json).
**Not part of the T1-U2/U2b atomic acceptance cohort; no U2/U2b file changes.** Depends on **T1-U2
and T1-U2b accepted** (uses `TryGetTodayDate`, `TryGetAge` and `Date.Reference.OutOfRange`). Must
land before T1-U6 and before any T-04 string-DateOnly work. Coverage scope `MustClauses`. Model
Codex team / `validation-builder`, budget 80k tokens / 30 min.

**Files**:

| Action | File |
|---|---|
| Edit | `src/PineGuard.MustClauses/MustStringDateOnlyClauses.cs` |
| Edit | `tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTests.cs` |
| Edit | `tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTestData.cs` |

No fixture, adapter, Core, codes or `.csproj` change: the existing `InternalsVisibleTo` grant from
`PineGuard.Core` to `PineGuard.MustClauses` already covers the internal helpers this unit calls.

**Inventory (verified from source, binding ruling §1)**: nothing in the string family currently
inherits U2/U2b's guarded-reference fix. No method in `MustStringDateOnlyClauses.cs` calls a
`Must.Be.*` clause; all five clock-consuming methods parse and then call the clock-reading Core
predicates directly:

| Method | Calls | Line |
|---|---|---|
| `PastDateOnly` | `DateOnlyRules.IsInPast(parsed, timeProvider:)` | 56 |
| `PastOrPresentDateOnly` | `DateOnlyRules.IsInPast(parsed, Inclusion.Inclusive, tp)` | 93 |
| `FutureDateOnly` | `DateOnlyRules.IsInFuture(parsed, timeProvider:)` | 130 |
| `FutureOrPresentDateOnly` | `DateOnlyRules.IsInFuture(parsed, Inclusion.Inclusive, tp)` | 167 |
| `MinimumAge(string, int)` | `DateOnlyRules.HasMinimumAge(parsed, years, tp)` | 1088 |

Every other method in the file compares against caller-supplied dates and reads no clock; those are
unaffected. `GuardStringDateOnlyClauses`, `FluentStringDateOnlyExtensions` and
`StringDateOnlyAttributes` call these five Must clauses and inherit the fix automatically — **no
edits** to any adapter. `MustStringDateTimeOffsetClauses` delegates to `DateTimeOffsetRules` instant
predicates and is unaffected. The Core `StringRules.DateOnly` predicates keep returning `false` on
projection failure, per the existing Core contract, and are unchanged.

**Behavior contract**: successful parsed values use the guarded helper directly; do not call the
typed `Must.Be.Past` family sideways (that would fork message ownership and break the
parse-then-typed-Rules layering). Exact precedence in each of the five methods, unchanged where
already present:

1. `MinimumAge` only: `years < 0` fails first with the existing `Date.Age.BelowMinimum` template,
   attributed to `nameof(years)`.
2. `value is null` fails with the existing null template and existing per-clause code.
3. Parse failure fails with `Date.Format.Invalid` and the clause template, as today.
4. New: reference projection —

   ```csharp
   if (!DateTimeUtility.TryGetTodayDate(timeProvider, out var referenceDate))
       return MustResult<DateOnly>.Fail(MustCodes.Date.Reference.OutOfRange, "{paramName} requires a reference date within the supported range.", nameof(timeProvider), timeProvider);
   ```

5. Semantic evaluation against the captured date, with existing template and code:
   `IsBefore(parsedValue, today, Exclusive)` for `PastDateOnly`, `Inclusive` for
   `PastOrPresentDateOnly`; `IsAfter` likewise for the two Future clauses; `TryGetAge(parsedValue,
   today, out var age) && age >= years` for `MinimumAge`, where `today =
   DateOnly.FromDateTime(referenceDate)`.

**One-read semantics**: exactly one `GetUtcNow` read on the successful-parse path, zero reads on the
null, unparseable and negative-years paths. Sampling after parse is chosen because the
parse-once-then-validate-typed contract places all typed evaluation after parse, it reads no clock
for input that cannot be judged, and it leaves every existing test row's outcome unchanged.

Add to each of the five `<remarks>` the same guarded-reference sentence the typed clauses carry
(U2's "the clock's local calendar date, derived from one sampled instant with checked projection").

**Codes and messages (no new constants)**: this unit emits only existing constants —
`MustCodes.Date.Age.BelowMinimum`, the existing per-clause null/parse-failure codes, the existing
`Date.Format.Invalid`, the existing semantic codes for Past/PastOrPresent/Future/FutureOrPresent/
MinimumAge, and the foundation's `MustCodes.Date.Reference.OutOfRange` (message
`"{paramName} requires a reference date within the supported range."`, attributed to
`nameof(timeProvider)`) that U2 already created. No `MustCodes.*` file changes.

**Required tests (binding ruling §4)**: all `[Theory]`, `MustCase`, `AssertResult`, AAA markers,
groups before records.

- Pin the four relative `_BehavesAsExpected` methods to `FixedTimeProvider.Default` and add a
  "present" row `"2026-06-15"` to each existing `Cases` group: Past false, PastOrPresent true,
  Future false, FutureOrPresent true. The existing 2000/2999, null and unparseable rows keep their
  expectations.
- New groups `PastDateOnlyAcrossTimeZones`, `PastOrPresentDateOnlyAcrossTimeZones`,
  `FutureDateOnlyAcrossTimeZones`, `FutureOrPresentDateOnlyAcrossTimeZones` with tuple `(string?
  value, TimeProvider timeProvider)`: `"2026-06-15"` at `DefaultAheadOfUtc` (local today 06-16: Past
  true, PastOrPresent true, Future false, FutureOrPresent false) and at `DefaultBehindUtc` (local
  today 06-14: Past false, PastOrPresent false, Future true, FutureOrPresent true). This proves the
  string path reads the local reference, not the UTC date.
- New groups `PastDateOnlyReferenceProjection`, `PastOrPresentDateOnlyReferenceProjection`,
  `FutureDateOnlyReferenceProjection`, `FutureOrPresentDateOnlyReferenceProjection` (tuple as above)
  and `MinimumAgeReferenceProjection` (`(string? value, int years, TimeProvider timeProvider)`), each
  with `MinValueUnderflow` and `MaxValueOverflow` rows using the same ±14h providers as the typed
  file, parseable value `"2000-01-01"`, expecting `MustExpected(false, "timeProvider requires a
  reference date within the supported range.", "timeProvider", Code:
  MustCodes.Date.Reference.OutOfRange)`. Each group also carries precedence rows with the failing
  provider: `NullValueBeforeProjection` (null, existing null message and code, `"value"`),
  `UnparseableBeforeProjection` (`"not-a-date"`, clause template, `Date.Format.Invalid`), and for
  `MinimumAge` additionally `NegativeYearsBeforeProjection` (`years` −1, `"years requires a
  non-negative number of years."`, `"years"`).
- New groups `PastDateOnlyClockReads`, `PastOrPresentDateOnlyClockReads`,
  `FutureDateOnlyClockReads`, `FutureOrPresentDateOnlyClockReads`, `MinimumAgeClockReads`, each with
  record `ClockReadCase(string Name, string? Value, int Expected) : ReturnCase<string?, int>` and
  rows `("2000-01-01", 1)`, `(null, 0)`, `("not-a-date", 0)`; methods
  `<Clause>_ReadsTheClockAsExpected` construct a `CountingTimeProvider` from
  `FixedTimeProvider.Default.GetUtcNow()` and assert `UtcNowCallCount`. `MinimumAge` passes `years`
  18.
- Keep `MinimumAge_BehavesAsExpected` and `MinimumAge_LeapDayBirthDate_MaturesOnTheFirstOfMarch`
  unchanged; they already pin a provider.

**Gates**: shared `All` format, diagnostics and tests; isolated `MustClauses` coverage 100%/100% on
net8.0 and net10.0 (the new branch in each of the five methods must be hit by the projection rows);
`audit --gate`; `audit must-codes` (expect 0 — no constants added, the file is already in the Date
`Serves` list); `audit nullability`.

**Status**: Claude Fable 5.1's final ruling is **ACCEPT** and the unit is **closed** (implementation
review session `30b7e0ea-6804-467a-b728-0813cafe2a02`); no corrections required. All verification
gates have passed: 44 focused cases passed on both TFMs; shared `All` format/diagnostics/tests exit
0, with diagnostics reporting zero warnings/errors and 38,259/38,259 tests passing (zero
failed/skipped); `MustClauses` coverage is 100%/100% on net8.0 and net10.0 (3,870 lines, 1,382
branches each); `audit --gate` (Rule11/12/50), `audit must-codes` (MustCodes Rule13), and
`audit nullability` (Rule07) each exit 0 with zero findings. Public review record:
[`artifacts/reviews/t1-u2c-fable-source-review-public.md`](../../../artifacts/reviews/t1-u2c-fable-source-review-public.md).
Final closure evidence:
[`artifacts/reviews/t1-u4b-cohort-gates/u2c-final-closure.json`](../../../artifacts/reviews/t1-u4b-cohort-gates/u2c-final-closure.json)
(SHA256 `836F34CD57FE22BA8FBEDD82CF65C14FF97248B4C6DCEF64158EACE45C2E035D`) and cohort evidence
[`artifacts/reviews/t1-u4b-cohort-gates/cohort-evidence.json`](../../../artifacts/reviews/t1-u4b-cohort-gates/cohort-evidence.json).
Exact 3-file source scope: `src/PineGuard.MustClauses/MustStringDateOnlyClauses.cs`,
`tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTests.cs`,
`tests/PineGuard.MustClauses.UnitTests/MustStringDateOnlyClausesTestData.cs`. This unit is `Done`
in the tracker ([temporal-orchestration.md](temporal-orchestration.md) §1). Local commit remains
pending user authorization; no staging or commit has occurred.

### U3 — TimeOnly midnight-crossing Between/NotBetween [A-09]

**Files**: `src/PineGuard.Core/Rules/TimeOnlyRules.cs` (`IsBetween` only), fixtures
`tests/PineGuard.Testing/Fixtures/TimeOnlyRulesFixtures.cs`, TestData for Core, Must
(`MustTimeOnlyClausesTestData.cs`), Guard, Fluent, DA `TimeOnly` test data files.
Also edit `MustTimeOnlyClauses.cs`, `GuardTimeOnlyClauses.cs`, `FluentTimeOnlyExtensions.cs`
and `TimeOnlyAttributes.cs`, and their string-TimeOnly counterparts where they delegate to
this range family. Rename TimeOnly Between/NotBetween arguments and tuple elements from
min/max to **start/end** across all affected layers. Non-TimeOnly ranges remain unchanged.
Explicitly exclude `TimeOnlyRange`, `TimeOnlyRangeRules`, and `MustTimeOnlyRangeClauses`:
these range types/clauses continue rejecting start greater than end.
Delete the TimeOnly Must reversed-bound guard and its invalid-range message/code emission;
retain actual existing secondary checks first; add no new Inclusion enum check. Preserve the shared
range code constants used by non-TimeOnly clauses. [Controlling Fable correction; A-09 unchanged]

**Exact output**:

```csharp
/// <remarks>
/// When start is later than end, the interval crosses midnight. Each endpoint follows
/// the requested inclusion. Equal endpoints mean one instant under Inclusive and none
/// under Exclusive; they never mean a full-day interval.
/// </remarks>
public static bool IsBetween(TimeOnly? value, TimeOnly start, TimeOnly end, Inclusion inclusion = Inclusion.Inclusive)
{
    if (value is null) return false;
    if (start <= end) return RuleComparison.IsBetween(value.Value, start, end, inclusion);
    return inclusion == Inclusion.Inclusive
        ? value.Value >= start || value.Value <= end
        : value.Value > start || value.Value < end;
}
```

Equal endpoints preserve the existing inclusion contract: Inclusive accepts exactly that one
time; Exclusive accepts none. Equal bounds never mean a full-day window. Test equal endpoints
with each inclusion. NotBetween negates only after common domain checks. [A-09; controlling Fable correction]

**Fixtures**: 23:00 and 01:00 inside 22:00–02:00; 12:00 outside; both endpoints under
Inclusive/Exclusive; equal endpoints under both inclusions; positive and negated clauses at
all layers. Must proves reversed bounds are valid and no longer emit misuse failures.
Remove TimeOnly reversed-bound misuse rows. String-TimeOnly delegates receive the same cases.
Fluent TestData uses the shared fixture members for wrap-inside/wrap-outside instead of inline
values; two Must TestData rows use `F.IsKnownTimes.T2000` instead of a literal `TimeOnly`.

**Acceptance**: gate with `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`,
`DataAnnotations`. Budget 100k / 30 min.

**Source-review follow-up (2026-09-27):** Claude Fable 5.1's bounded source review confirmed the
Core/Must/Guard/Fluent/DataAnnotations contract is correct and found required corrections before
the unit gate. Source ownership is exactly 32 files: the 31 already reviewed plus
`tests/PineGuard.Testing/Fixtures/StringRulesFixtures.TimeOnly.cs`, added to this unit. Rename that
fixture's `IsBetween` tuple fields from min/max to start/end; update the
`MustStringTimeOnlyClausesTestData.cs` and `GuardStringTimeOnlyClausesTestData.cs` projections to
match; add string Core `IsBetween` wrap-inside, wrap-outside and equal-endpoint cases mirroring the
typed fixture. [`artifacts/reviews/t1-u3-ownership-manifest.txt`](../../../artifacts/reviews/t1-u3-ownership-manifest.txt) (historical 31-file evidence) is
superseded by the authoritative exact 32-path audit at
[`artifacts/reviews/t1-u3-final-ownership-audit.json`](../../../artifacts/reviews/t1-u3-final-ownership-audit.json), recorded after Fable's approved fixture
addition; neither artifact is modified by this transcription. Remove the unused `WrapStart`/`WrapEnd` fields from
`tests/PineGuard.FluentValidation.UnitTests/FluentTimeOnlyExtensionsTestData.cs`. The two stale
documentation phrases the review also found (the DataAnnotations unit-test spec Pattern D, and
`core-common-api-decisions.md` A-3/A-8) are T-07 U2 follow-up work, not this unit (T-00 §4.1;
[T-07 §3 U2](temporal-07-closeout.md)). The source review is not unit acceptance: run the full
T-00 §9 gate for both TFMs across the five U3 scopes, `audit --gate`, `audit must-codes` and
`audit nullability` before claiming closure. Exact evidence is recorded in
[`temporal-u3-source-review-fable.md`](temporal-u3-source-review-fable.md).

### U4 — Guard boundary polarity fix (D-9)

U4 excludes the TimeOnly range family, `GuardTimeOnlyClausesTestData.cs` and
`GuardStringTimeOnlyClausesTestData.cs` from its regression sweep: U3 owns those
equal-endpoint cases. This preserves disjoint file ownership for U3/U4.

This unit lands **before** U4a so the fix is reviewable on its own, against the pre-rename names
(`Guard.Against.LessThan`/`GreaterThan`, `Must.Be.GreaterThan`/`LessThan`). It is a standalone
commit, not folded into the rename diff (T-00 §3.1, D-9: "Land it as a standalone commit BEFORE the
T-01 rename, with a failing test first").

**Files**:

| Action | File |
|---|---|
| Edit | `src/PineGuard.GuardClauses/GuardTimeSpanClauses.cs` (`LessThan`, `GreaterThan`) |
| Edit | `src/PineGuard.GuardClauses/GuardStringTimeSpanClauses.cs` (`LessThan`, `GreaterThan`) |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardTimeSpanClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardStringTimeSpanClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardDateOnlyClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateOnlyClausesTestData.cs` |
| Edit | `tests/PineGuard.GuardClauses.UnitTests/GuardStringDateTimeOffsetClausesTestData.cs` |

**Binding Fable ruling (2026-09-27):** Claude Fable 5.1 narrowed the prior draft's wildcard
"every range-style TestData" sweep to the exact seven-file ownership table above. The ruling verified
18 inclusion-taking Guard files; only the four TimeSpan `LessThan`/`GreaterThan` methods forward
inclusion to a complementary comparison, so only those source calls need polarity correction. The
range-style guards forward unchanged to their name-negated Must clauses, where that polarity is
correct. Eleven of the 16 range-style TestData files already prove the on-bound behavior; adding
duplicate rows there would be churn and exceed the T-00 §7.2 12-test-file cap. Add 17 evidence-backed
rows only to the three other range-data files listed above. The two TimeSpan files also complement
their fixture-derived inclusion values to expose the four failing-first equality cases.

**No-edit sweep evidence:** existing boundary cases already cover
`GuardDateTimeClausesTestData.cs`, `GuardDateTimeOffsetClausesTestData.cs`,
`GuardNumberClausesTestData.cs`, `GuardStringNumberTypesClausesTestData.cs`,
`GuardStringNumbersClausesTestData.cs`, `GuardDateOnlyRangeClausesTestData.cs`,
`GuardDateTimeRangeClausesTestData.cs`, `GuardDateTimeOffsetRangeClausesTestData.cs`,
`GuardTimeOnlyRangeClausesTestData.cs`, `GuardCollectionClausesTestData.cs`, and
`GuardStringGraphemesClausesTestData.cs`. The entire files
`GuardTimeOnlyClausesTestData.cs` and `GuardStringTimeOnlyClausesTestData.cs` remain U3-owned.
Fixed comparisons with no Inclusion parameter and all other TestData files are outside U4.

The exact fixture members, group names, Inclusion values, expected pass/throw outcomes, source
evidence, and verification checklist are binding in
[`temporal-u4-sweep-ruling-fable.md`](temporal-u4-sweep-ruling-fable.md). That receipt supersedes the
previous draft's proposed blanket range-fixture edit list; D-9's standalone pre-U4a ordering and
pre-rename API scope are unchanged.

**Failing test first:** in the existing `LessThan_BehavesAsExpected` and
`GreaterThan_BehavesAsExpected` Test methods, complement the fixture inclusion projections in the
two TimeSpan TestData files first. Run those existing methods and confirm all eight equal-value
twins fail against the current Guard bodies: four `Exclusive` calls expected to pass and four
`Inclusive` calls expected to throw. The first run produced 8 failed / 10 passed; Claude Fable 5.1
adjudicated that as the correct red set and corrected its initial undercount (see
[`temporal-u4-sweep-ruling-fable.md`](temporal-u4-sweep-ruling-fable.md) and
`artifacts/reviews/temporal-u4-red-check-adjudication.json`). Do not add separately named equality
test methods; the unit-test spec requires the existing one-method-per-operation-group pattern. Apply
the four source call fixes below only after the failing-first result is recorded.

**Exact output** — both guard bodies in both files become:

```csharp
// Guard.Against.LessThan: throw when value is shorter than threshold (strictly, by default)
var result = Must.Be.GreaterThan(value, threshold, inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive, paramName);

// Guard.Against.GreaterThan: throw when value is longer than threshold
var result = Must.Be.LessThan(value, threshold, inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive, paramName);
```

(Only the `inclusion` argument changes — from forwarding `inclusion` unmodified to forwarding its
complement. The clause names stay `LessThan`/`GreaterThan`; U4a/U4b rename them afterward and must
carry this complement forward unchanged.)

Guard test data derives from the Must fixture group with the inclusion **complemented** in the tuple
(`Inclusion.Inclusive ↔ Exclusive`). The exact four equality-derived results and remaining 17
regression rows are listed in the binding Fable ruling receipt linked above.

**Acceptance**: record the red-then-green output from the existing `LessThan_BehavesAsExpected` and
`GreaterThan_BehavesAsExpected` methods, then add the 17 Fable-specified rows and run the
`GuardClauses` gate on net8.0 and net10.0, nullability audit, and `audit --gate`. Confirm the diff is
limited to the seven files in the ownership table; no fixture, `Tests.cs`, TimeOnly, or Number files
are touched. Record the existing-boundary citations listed in the Fable ruling when closing D-9.
**Targeted red/green evidence recorded (2026-09-27):** the four existing methods failed 8/18 and
then passed 18/18 on net10.0 after the four source complements. This proves the boundary defect and
its fix, but does not replace the full net8.0/net10.0 GuardClauses gates, nullability audit, or shared
audit gate; U4 remains in progress until those checks and Fable unit acceptance pass.

### U4a — TimeSpan rename: Core + codes + Must

**File list is generated, not chosen**: run inside the worktree

```powershell
git grep -l -E "IsGreaterThan|IsLessThan|GreaterThan|LessThan|NotGreater|NotLess" -- 'src/PineGuard.Core/Rules/TimeSpanRules.cs' 'src/PineGuard.Core/Rules/StringRules.TimeSpan.cs' 'src/PineGuard.Core/Codes/MustCodes.Time.cs' 'src/PineGuard.MustClauses/*TimeSpan*' 'tests/PineGuard.Core.UnitTests/**/*TimeSpan*' 'tests/PineGuard.MustClauses.UnitTests/*TimeSpan*' 'tests/PineGuard.Testing/Fixtures/*TimeSpan*'
```

Corrected pathspec (2026-09-27): the Must test pathspec above is `tests/PineGuard.MustClauses.UnitTests/*TimeSpan*`,
not `**/*TimeSpan*` — root-level Must test/data files have no slash after the project directory, so the
original literal `**/` pathspec returned no matches. See [the Fable ruling](temporal-u4a-selector-ruling-fable.md)
for the dated current-output snapshot and scope-preserving rationale; the generated command above
remains the ownership authority, not the snapshot.

and touch exactly those files.

**Renames** (whole-word, case-sensitive, TimeSpan files only — never touch `NumberRules` or
`RuleComparison`):

| From | To |
|---|---|
| `TimeSpanRules.IsGreaterThan` | `TimeSpanRules.IsLongerThan` |
| `TimeSpanRules.IsLessThan` | `TimeSpanRules.IsShorterThan` |
| `StringRules.TimeSpan.IsGreaterThan` / `IsLessThan` | `IsLongerThan` / `IsShorterThan` |
| `Must.Be.GreaterThan(TimeSpan …)` / `LessThan(TimeSpan …)` | `Must.Be.LongerThan` / `Must.Be.ShorterThan` |
| `Must.Be.GreaterThan(string …)` / `LessThan(string …)` (`MustStringTimeSpanClauses`, overloads of the typed names) | `Must.Be.LongerThan(string …)` / `Must.Be.ShorterThan(string …)` |
| `MustCodes.Time.Duration.NotGreater` (`time.duration.not-greater`) | `NotLonger` (`time.duration.not-longer`) |
| `MustCodes.Time.Duration.NotLess` (`time.duration.not-less`) | `NotShorter` (`time.duration.not-shorter`) |
| message `"{paramName} must be greater than the threshold."` | `"{paramName} must be longer than the threshold."` |
| message `"{paramName} must be less than the threshold."` | `"{paramName} must be shorter than the threshold."` |
| XML doc phrases "greater than" / "less than" in these files | "longer than" / "shorter than" |
| test method, TestData group and fixture group names containing `GreaterThan` / `LessThan` | `LongerThan` / `ShorterThan` |

**Corrected string-overload row (2026-09-27):** the string `MustStringTimeSpanClauses.GreaterThan`/
`LessThan` overloads (and the unsuffixed string `DurationBetween` overloads) coexist with the typed
overloads under the same unsuffixed names — there is no separate `GreaterThanTimeSpan`/
`LessThanTimeSpan` string clause. They rename to the same unsuffixed `LongerThan`/`ShorterThan` as
the typed overloads, and their tests/TestData/fixture groups within the already-confirmed 15 U4a
files rename to match. See
[the Fable ruling](temporal-u4a-string-overload-ruling-fable.md) for the binding contract and source
facts.

**Acceptance**: pass the scoped Core/Must gates below, require zero `audit nullability` findings, and
record the U4a `audit must-codes` residue check with exactly the four cohort-approved findings;
the zero-findings MustCodes requirement is at the cohort head. Budget 120k / 45 min.

**U4a/U4b verification cohort (2026-09-27):** U4a and U4b are one atomic acceptance cohort; U4a is
not independently commit-ready or mergeable because adapter callers still use the old Must names.
U4a runs only the scoped Core/Must format, Roslyn diagnostics, tests, and isolated coverage on
net8.0 and net10.0, plus `audit nullability` — do not run the All-scope
format, diagnostics, or test commands until U4b has renamed the adapters. `audit must-codes` is
executed at the U4a scoped gate as a recorded residue inventory: it passes only if every Rule13
finding is one of the four old `GreaterThan`/`LessThan` clause and `NotGreater`/`NotLess` code
references in `src/PineGuard.DataAnnotations/StringTimeSpanAttributes.cs` and
`src/PineGuard.DataAnnotations/TimeSpanAttributes.cs`, any fifth or out-of-file finding blocks U4a
acceptance; the zero-finding requirement is deferred to the U4a/U4b cohort head, where the command
is rerun after U4b and must report zero with no baseline key, alias, exception or suppression added.
See the [MustCodes deferral ruling](../../../artifacts/reviews/t1-u4a-scoped-gates/fable-mustcodes-deferral.md).
At the U4a handoff, run
the two exact residue sweeps defined in [the Fable cohort ruling](temporal-u4a-u4b-cohort-ruling-fable.md)
(Sweep A, the ownership sweep; Sweep B, the stray sweep) over the 18-file U4b inventory (six adapter
source files, twelve adapter test files); the four `apps/cli/config/baseline.json` keys named in
[the baseline-key ruling](temporal-u4a-baseline-key-ruling-fable.md) are the only permitted matches
outside that inventory at handoff. U4b must make both sweeps return zero lines — after the paired,
net-count-unchanged baseline key move the baseline-key ruling requires — then run the full All format, All compiler diagnostics,
and All tests, isolated coverage for `Core`, `MustClauses`, `GuardClauses`, `FluentValidation`, and
`DataAnnotations` on both TFMs, and `audit --gate`, `audit must-codes`, and `audit nullability`;
`audit layer-parity` stays at the already specified completed-Phase-1-cohort timing. U4a and U4b
each require their own Fable review and receipt; the cohort then receives one acceptance and one
commit once the U4b head is green, with that hash recorded on both tracker rows. See
[the Fable ruling](temporal-u4a-u4b-cohort-ruling-fable.md).

### U4b — TimeSpan rename: Guard (renames only, D-9 already fixed by U4) + Fluent + DA

**File list**: same grep as U4a over `src/PineGuard.GuardClauses/*TimeSpan*`,
`src/PineGuard.FluentValidation/*TimeSpan*`, `src/PineGuard.DataAnnotations/*TimeSpan*` and their
test projects' `*TimeSpan*` files.

**Renames**: `Guard.Against.GreaterThan` → `Guard.Against.LongerThan`; `LessThan` → `ShorterThan`;
Fluent `.GreaterThan(TimeSpan)` → `.LongerThan`, `.LessThan` → `.ShorterThan`; DA
`GreaterThanTimeSpanAttribute` → `LongerThanTimeSpanAttribute`, `LessThanTimeSpanAttribute` →
`ShorterThanTimeSpanAttribute`, and the string variants `GreaterThanTimeSpanStringAttribute` →
`LongerThanTimeSpanStringAttribute`, `LessThanTimeSpanStringAttribute` → `ShorterThanTimeSpanStringAttribute`.

The boundary-polarity fix (D-9) already landed in U4, against the pre-rename names. This unit is a
pure rename: the two guard bodies keep U4's complemented-`inclusion` shape and simply pick up the new
Must clause names —

```csharp
// Guard.Against.ShorterThan: throw when value is shorter than threshold (strictly, by default)
var result = Must.Be.LongerThan(value, threshold, inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive, paramName);

// Guard.Against.LongerThan: throw when value is longer than threshold
var result = Must.Be.ShorterThan(value, threshold, inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : Inclusion.Inclusive, paramName);
```

Do not re-derive the complement logic here — if the diff against U4's output touches anything other
than the identifier names in this snippet, that is a regression, not a rename.

XML docs for both: "Throws if `value` is shorter than `threshold`" / "longer than". Guard test data
derives from `F.IsLongerThan` / `F.IsShorterThan` with the inclusion **complemented** in the tuple
(`Inclusion.Inclusive ↔ Exclusive`) — this is a rename of U4's already-complemented test data, not a
new derivation; the `value == threshold` boundary rows U4 added simply gain the new names.

**Acceptance**: gate with `GuardClauses`, `FluentValidation`, `DataAnnotations`; `audit layer-parity`
runs at the completed phase cohort after the final adapter, not this intermediate unit.
Per-unit checks remain green. Budget 120k / 45 min.

### U5 — Precision enum symmetry

**Files**: `src/PineGuard.Core/Common/Precision.cs`, `src/PineGuard.Core/Utils/DateTimeUtility.cs`
(`TryTruncateToPrecisionUtc` ×2), `src/PineGuard.Core/Utils/TimeOnlyUtility.cs`
(`TryTruncateToPrecision`), fixtures `DateTimeUtilityTests`/`TimeOnlyUtilityTests` data, and the
`IsBefore`/`IsAfter`/`IsSame` TestData groups for DateTime, DateTimeOffset, TimeOnly (one
`Microsecond` case each).

**Exact output**: `DateTimePrecision` gains `Microsecond = 7`, `Tick = 8`; `TimePrecision` becomes
`Hour = 0, Minute = 1, Second = 2, Millisecond = 3, Microsecond = 4, Tick = 5`. Truncation:
`Microsecond` → `ticks - ticks % 10`; `Tick` → unchanged value. On `netstandard2.1` there is no
`DateTime.Microsecond`; the tick arithmetic above is TFM-independent, so no guard is needed.

**Acceptance**: gate with `Core`. Budget 80k / 30 min.

## 4. Verification (phase)

Run T-00 §9 in full (all five scopes plus `Testing`, both TFMs), then Qodana and Sonar. Then the
Claude Fable 5.1 phase review (orchestration §2.4) with this phase's file list. Merge per T-00 §8.

## 5. Definition of Done

- All eleven units (U0, U1, U2, U2b, U2c, U3, U4, U4a, U4b, U5, U6) Done in the tracker with commit hashes.
- `grep -n "public static bool Is\(Greater\|Less\)Than" src/PineGuard.Core/Rules/TimeSpanRules.cs` returns nothing (the public surface carries only `IsLongerThan`/`IsShorterThan`; the private bodies still call `RuleComparison.IsGreaterThan`/`IsLessThan`, which U4a forbids touching — the original unqualified grep could never pass and was corrected on 2026-11-08 during the Phase 1 review).
- `grep -rn "ToCalendarDate" src/PineGuard.Core/Utils/DateTimeUtility.cs` returns nothing (D-1: removed, not renamed).
- `DateOnlyRules.IsInPast(new DateOnly(2026,6,15), timeProvider: FixedTimeProvider.DefaultAheadOfUtc)` is `true` (it was `false`).
- `TimeOnlyRules.IsBetween(new TimeOnly(23,0), new TimeOnly(22,0), new TimeOnly(6,0))` is `true`; every TimeOnly integration layer follows this wrap contract.
- `Guard.Against.ShorterThan(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5))` does not throw (its predecessor `Guard.Against.LessThan` did, before U4b; the clause was renamed by U4a).
- `DateTimeUtility.TryGetTodayDate(null, out var today)` yields the UTC calendar date on a host in any zone (D-1 as ratified 2026-11-09; asserted by the `core-null-provider-timezone` CI job under `TZ=Pacific/Auckland`).
- Standards is **not** affected yet (it pins alpha.7); the rename reaches it in Phase 3.

## 6. Risks specific to this phase

| Risk | Mitigation |
|---|---|
| D-1 changes `HasMinimumAge` results for any `DateTime` birth date whose local calendar date, read literally, differs from what the old UTC-conversion logic computed — not just `Utc`-kind values, since `Kind` is now ignored entirely | Only when the caller injects a provider whose `LocalTimeZone` is not UTC; with no provider "today" is the UTC calendar date (D-1 as ratified 2026-11-09), so the pre- and post-D-1 reference dates coincide and only the Kind handling of the birth date changes. Documented in the `<remarks>`. U2's fixtures pin both directions (`DefaultAheadOfUtc`/`DefaultBehindUtc`) and name the two rows whose expectation flips, with the pre-change result recorded alongside each. |
| D-2 makes `DateTimeRules.IsSameDay` deterministic for `DateTimeKind.Local` operands, where it was previously silently host-dependent (via `DateTime.ToUniversalTime()` against the real `TimeZoneInfo.Local`) | This is a bug fix, not a behaviour change callers can rely on: any test asserting the old Local-Kind result was already unreproducible off the author's machine. U2's fixtures record this explicitly rather than picking an arbitrary "before" value. |
| D-9's guard-polarity fix only inspects the two TimeSpan guard files without confirming no other `Guard*Clauses` class has the same single-threshold-plus-`Inclusion` shape | U4's audit note classifies each current file; Claude Fable 5.1 re-runs the inventory and checks actual boundaries, not only the two diffs. [A-01, A-11] |
| Renaming `GreaterThan` touches a Number-family file by mistake | The grep restricts to `*TimeSpan*` paths; Claude Fable 5.1 checks the diff against the owned file list. [A-01] |
