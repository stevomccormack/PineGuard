<!-- metadata_header
type: plan
id: temporal-04-validity-relative
version: 1.0
status: planned
last_updated: 2026-11-09
-->

# Plan T-04 — Phase 4: Validity windows, relative time, age bounds, DateTimeOffset parity

> Charter: [T-00](temporal-00-program.md). Decisions cited: **D-1, D-6, D-14, D-15**. Naming: T-00 §4.4.
> Codes: T-00 §5 (`Date.Validity`, `Date.Relative` additions, `Date.Age` additions).
> Depends on Phase 2 merged. Phase 3 can run independently where ownership permits;
> Phases 4 → 5 → 6 are serial dependencies, including across worktrees.
> Repo: PineGuard. Worktree `.claude/worktrees/temporal-04-validity`, branch `feature/temporal-04-validity`.

> Review provenance and exception policy: T-00 §0 and
> [Astra register A-02 / A-03](temporal-astra-decisions.md). Implementers are Codex team; Claude Fable 5.1 owns every review, decision and council.

## 1. Business plan

The most common temporal checks in production code are "has this expired", "is this in effect
yet", "did this happen in the last N minutes", "is this older than N", and "is this person at most
N years old". Today each is written as `Must.Be.Future` / `Must.Be.Past` with a comment. This phase
gives them names that say what they check, with the clock injected.

## 2. Units

| Unit | Title | Model / agent | Depends on | Fan-out | Coverage scopes |
|---|---|---|---|---|---|
| U1 | Codes (preserve existing Proximity constants under D-6) | Codex team / `general-purpose` | Phase 2 merged | ∥ U2 | — |
| U2 | Core rules consuming foundation `TryGetAge` (+ delete `DateTimeRules.IsWithinDaysFromNow`, D-6) | Codex team / `validation-builder` | Phase 2 merged | ∥ U1 | `Core` |
| U3a / U3b / U3c | Must clauses — one dispatch per type file: `t04-u3a` (`MustDateTimeOffsetClauses.cs`), `t04-u3b` (`MustDateTimeClauses.cs`, + delete `WithinDaysFromNow`/`NotWithinDaysFromNow`, D-6), `t04-u3c` (`MustDateOnlyClauses.cs`), each with its Tests + TestData | Codex team / `validation-builder` | U1, U2 | U3a ∥ U3b ∥ U3c | `MustClauses` |
| U4a / U4b / U4c | Guard clauses — per type file as U3 (`GuardDateTimeOffsetClauses.cs`, `GuardDateTimeClauses.cs` + delete `WithinDaysFromNow`/`NotWithinDaysFromNow` (D-6), `GuardDateOnlyClauses.cs`) | Codex team / `validation-builder` | matching U3 | ∥ each other and U5 | `GuardClauses` |
| U5a-1 / U5a-2 / U5a-3 | Fluent — per type file (`FluentDateTimeOffsetExtensions.cs`, `FluentDateTimeExtensions.cs` + delete `.WithinDaysFromNow`/`.NotWithinDaysFromNow` (D-6), `FluentDateOnlyExtensions.cs`) | Codex team / `validation-builder` | matching U3 | ∥ | `FluentValidation` |
| U5b-1 / U5b-2 / U5b-3 | DataAnnotations — per type file (`DateTimeOffsetAttributes.cs`, `DateTimeAttributes.cs`, `DateOnlyAttributes.cs`) | Codex team / `validation-builder` | matching U3 | ∥ | `DataAnnotations` |
| U6 | Phase review + merge | Claude Fable 5.1, orchestrator | all | last | — |

## 3. Unit specifications

**Consumer documentation [A-06; Fable corrective council]**: strict instant DateTime APIs
accept only `DateTimeKind.Utc`. XML remarks explain that ASP.NET model binding and
System.Text.Json can supply Local or Unspecified values; use DateTimeOffset for consumer
inputs that must preserve an offset/instant. The existing host-converting
`DateTimeUtility.ToUtc` remains for legacy comparisons required by
`docs/ai/specs/core/project.md`; it does not relax this phase's strict UTC contract.

### U1 — Codes

Edit `src/PineGuard.Core/Codes/MustCodes.Date.cs` only: add nested `Validity` (`Prefix = Date.Prefix + ".validity"`;
`Expired`, `NotExpired`, `NotEffective`, `Outside`), add to `Relative` (`NotWithinLast`, `WithinLast`,
`NotWithinNext`, `WithinNext`, `NotOlderThan`, `OlderThan`), add to `Age` (`AboveMaximum`, `OutOfRange`).
Code strings are the kebab-case of the constant name under the node prefix
(`date.validity.not-effective`, `date.relative.not-within-last`, `date.age.above-maximum`).
The existing `// Serves:` comment remains unchanged; this unit adds no Must consumer files.

Append only this unit's new nodes after Phase 2 merges. Reuse the already-existing
`MustCodes.Date.Kind.NotUtc` (`date.kind.not-utc`); add no duplicate Kind node or constant.
Reuse the foundation's
`Date.Reference.OutOfRange` (`date.reference.out-of-range`). Kind failure is attributed to value
with message `"{paramName} must have DateTimeKind.Utc."`; reference projection failure to
`nameof(timeProvider)` with `"{paramName} requires a reference date within the supported range."`.
Use only expected dependency/configuration/range failures at the existing Must boundary; do not
catch arbitrary unexpected exceptions from custom provider code. [A-06, A-07, A-12]

**Preserve MustCodes (D-6)**: remove only the named WithinDaysFromNow /
NotWithinDaysFromNow APIs and their Core IsWithinDaysFromNow predicate. Keep
`MustCodes.Date.Proximity.NotWithin` / `Within`: existing Within/NotWithin clauses in
DateOnly, DateTime, DateTimeOffset, string and Fluent families still consume them.
U1 does not edit or delete this Proximity node or its constants. The calendar-month
constants also remain. [Final Fable acceptance B1 source correction]
Budget 40k / 15 min.

### U2 — Core rules consuming foundation `TryGetAge`

**Files**: `src/PineGuard.Core/Rules/DateTimeOffsetRules.cs`, `DateTimeRules.cs`, `DateOnlyRules.cs`,
fixtures `DateTimeOffsetRulesFixtures.cs`, `DateTimeRulesFixtures.cs`, `DateOnlyRulesFixtures.cs`;
TestData/tests for the three rule classes. DateTimeUtility and its TryGetAge tests are existing
T-01 U2 outputs consumed here, not edits or new ownership in this unit.

**Clone from**: `DateTimeOffsetRules.IsInPast` / `IsWithin`, `DateOnlyRules.HasMinimumAge`.

**Exact members** — `DateTimeOffsetRules` (all with `TimeProvider? timeProvider = null` last;
`now = DateTimeUtility.GetUtcNow(timeProvider)`):

```csharp
public static bool IsExpired(DateTimeOffset? value, TimeProvider? timeProvider = null) =>
    value is not null && value.Value <= DateTimeUtility.GetUtcNow(timeProvider);

public static bool IsEffective(DateTimeOffset? notBefore, TimeProvider? timeProvider = null) =>
    notBefore is not null && notBefore.Value <= DateTimeUtility.GetUtcNow(timeProvider);

public static bool IsWithinValidity(DateTimeOffset? notBefore, DateTimeOffset? notAfter, TimeProvider? timeProvider = null)
{
    if (notBefore is null || notAfter is null) return false;
    var now = DateTimeUtility.GetUtcNow(timeProvider);
    return notBefore.Value <= now && now < notAfter.Value;
}

public static bool IsWithinLast(DateTimeOffset? value, TimeSpan span, TimeProvider? timeProvider = null)
{
    if (value is null || span < TimeSpan.Zero) return false;
    var now = DateTimeUtility.GetUtcNow(timeProvider);
    return value.Value <= now && now - value.Value <= span;
}

public static bool IsWithinNext(DateTimeOffset? value, TimeSpan span, TimeProvider? timeProvider = null)
{
    if (value is null || span < TimeSpan.Zero) return false;
    var now = DateTimeUtility.GetUtcNow(timeProvider);
    return value.Value >= now && value.Value - now <= span;
}

public static bool IsOlderThan(DateTimeOffset? value, TimeSpan age, TimeProvider? timeProvider = null)
{
    if (value is null || age < TimeSpan.Zero) return false;
    var now = DateTimeUtility.GetUtcNow(timeProvider);
    return value.Value < now && now - value.Value > age;
}

public static bool HasMinimumAge(DateTimeOffset? value, int years, TimeProvider? timeProvider = null)
public static bool HasMaximumAge(DateTimeOffset? value, int years, TimeProvider? timeProvider = null)
public static bool IsAgeInRange(DateTimeOffset? value, int minYears, int maxYears, TimeProvider? timeProvider = null)
public static bool IsSameDay(DateTimeOffset? value, DateTimeOffset? other)   // UTC dates; null semantics as DateTimeRules.IsSameDay
```

`IsExpired`, `IsEffective`, `IsWithinValidity`, `IsWithinLast`, `IsWithinNext`, and `IsOlderThan` above
are **instant comparisons** (`value` vs. `now`, both resolved through `TimeProvider`) and have no
calendar basis at all — no `<remarks>` about "today" applies to them, on any of the three types.

**D-14 (council, 2026-09-26)** — calendar basis for the age rules only, and only on `DateTimeOffset`:
`HasMinimumAge`, `HasMaximumAge`, and `IsAgeInRange` sample GetUtcNow once and derive "today"
with checked foundation `TryGetDateAtOffset(sample, value.Offset, out referenceDate)`, so both the birth date and
"today" are read back through the *same* offset — the caller's own — and share one calendar. This is
deliberately **not** the provider-local today projection (D-1): that would mix the value's
offset with the clock's zone and could flip the computed age at a birthday boundary depending on
server location. `<remarks>` on each of these three members: "Fixed offset, not DST-aware; for
zone-aware calendar age needs a separately reviewed API; no such overload is promised here."
Use shared `TryGetAge` once and compare completed integer years: minimum `age >= years`, maximum
`age <= years`, between `minYears <= age && age <= maxYears`. Negative thresholds, inverted bounds
and future birth dates fail; any nonnegative int threshold is safe. Never compute `(years + 1)`
or threshold-based AddYears. Before projecting now to the value's offset, verify UTC ticks plus
offset remain representable. Projection failure returns false from the Try helper; Must reports
the reference/provider configuration failure attributed to that dependency under Brain §5.4.2,
never clamps or mislabels the birth date. Register that diagnostic using existing code precedent
in U1. [A-07]

`DateTimeRules`'s age rules (`HasMinimumAge`, `HasMaximumAge`, `IsAgeInRange`) and `DateOnlyRules`'s
age rules use internal guarded `TryGetTodayDate` (D-1/A-07), then DateOnly conversion where needed,
and judge the value on its own calendar date — `value.Date` for
`DateTime` (no `Kind` conversion, per D-1's simplification; `ToCalendarDate(value, clock.LocalTimeZone)`
is gone), and the `DateOnly` value directly (it has no time component to reduce). Each of the three
age-rule docs (`DateTimeOffsetRules`, `DateTimeRules`, `DateOnlyRules`) cross-references its siblings'
calendar basis in its `<remarks>` — the `DateTimeOffset` doc points at D-14, the `DateTime`/`DateOnly`
docs point at D-1 — so a reader lands on the right rule for the type in front of them.
`DateTimeRules`'s six new instant members (`IsExpired`…`IsOlderThan`) require `DateTimeKind.Utc`;
Local and Unspecified inputs fail validation. Do not silently convert through the machine zone.
Civil-date D-1/D-2 predicates still ignore Kind. Existing legacy APIs keep their established
semantics until explicitly migrated. UTC DateTime windows use ordered elapsed differences as
above, never now ± span. [A-06]
`DateOnlyRules` (net8+): `IsExpired(DateOnly? value, tp)` ⇔ `today > value`; `IsEffective` ⇔
`notBefore <= today`; `IsWithinValidity` ⇔ `notBefore <= today <= notAfter` (inclusive end for
whole-day validity; state this in the remarks); `IsWithinLast(DateOnly? value, int days, tp)` ⇔
`days >= 0 && value <= today && today.DayNumber - value.DayNumber <= days`; `IsWithinNext` ⇔
`days >= 0 && value >= today && value.DayNumber - today.DayNumber <= days`;
`IsOlderThan(DateOnly? value, int days, tp)` ⇔ `days >= 0 && value < today && today.DayNumber - value.DayNumber > days`;
`HasMaximumAge`, `IsAgeInRange` use completed years. These are calendar-date comparisons;
never AddDays with unchecked user bounds. Validity rejects start > end. Equal instant bounds
produce an empty [start,end) window; equal DateOnly bounds succeed only on that date. Test actual
MinValue/MaxValue inclusivity, zero spans, TimeSpan.MaxValue / int.MaxValue and every DateTime Kind
at each layer. [A-06, A-07]

`DateTimeUtility.TryGetAge` and its tests are created in T-01 U2. This unit consumes
those members; it does not recreate them or postpone their availability. [Fable corrective council]

The `TryGetAge` 29-February remark preserves the existing March-1 birthday observation: a
2008-02-29 birth date is 17 on 2026-02-28 and 18 on 2026-03-01. Document it as PineGuard's calendar
convention, not a legal/regional age guarantee. Such configurable legal policy is outside scope.
Internal `DateTimeOffsetUtility.TryGetDateAtOffset(DateTimeOffset instant, TimeSpan offset,
out DateTime date)` lands in T-01 U2 foundation; this unit consumes it. It returns Unspecified midnight, reads no clock and fails/out default for invalid
whole-minute ±14h offsets or unrepresentable UTC ticks plus offset. DTO age samples once, calls
that helper then TryGetAge; DateOnly wraps its date only on supported targets. [A-07]

**Clock/configuration contract [A-03]**: capture now exactly once per predicate. Tests include a
counting provider and a provider that advances on each read, proving one read and stable boundary
behavior. Separate composed validations may read different instants; no cross-rule atomic snapshot
is promised. Calendar operations use the injected provider's LocalTimeZone only where D-1 applies,
and the UTC calendar date when no provider is injected (D-1 as ratified 2026-11-09);
DateTimeOffset age retains D-14's fixed-offset basis. Do not introduce a global clock or zone.

**Fixtures** (every group has the clock as the last tuple element; use `FixedTimeProvider.Default`
unless the row says otherwise; `Now` = `2026-06-15T12:00Z`). `DefaultAheadOfUtc`/`DefaultBehindUtc`
land in T-01 (D-4) and are calibrated against T-01 U0: +10:00 / −08:00 — `DefaultAheadOfUtc` = UTC
instant `2026-06-15T20:00:00Z` with `LocalTimeZone` `TestTimeZones.PlusTen` (+10:00) (local now
`2026-06-16T06:00:00+10:00`, date already rolled to 06-16 while UTC is still 06-15 — matching the
"local today is 2026-06-16" fact the `HasMaximumAge` row below relies on) and `DefaultBehindUtc` =
UTC instant `2026-06-15T04:00:00Z` with `LocalTimeZone` `TestTimeZones.MinusEight` (−08:00) (local
now `2026-06-14T20:00:00−08:00`, date still 06-14 while UTC has already reached 06-15). Every row
below was re-checked against these literals; the qualitative calendar-date facts each row relies on
already held under the real offsets, so no expected value changes, only this calibration note:

| Group | Rows (name → inputs → expected) |
|---|---|
| `IsExpired` | `Past` (Now−1h) → true; `Exactly` (Now) → true; `Future` (Now+1h) → false; `Null` → false |
| `IsEffective` | `Past` → true; `Exactly` → true; `Future` → false; `Null` → false |
| `IsWithinValidity` | `Inside` (Now−1h, Now+1h) → true; `AtStart` (Now, Now+1h) → true; `AtEnd` (Now−1h, Now) → false; `Before` (Now+1h, Now+2h) → false; `After` (Now−2h, Now−1h) → false; `Inverted` (Now+1h, Now−1h) → false; `NullStart`, `NullEnd` → false |
| `IsWithinLast` | `Inside` (Now−30m, 1h) → true; `AtLowerBound` (Now−1h, 1h) → true; `AtNow` (Now, 1h) → true; `TooOld` (Now−2h, 1h) → false; `Future` (Now+1m, 1h) → false; `NegativeSpan` (Now, −1h) → false; `ZeroSpanAtNow` (Now, 0) → true; `Null` → false |
| `IsWithinNext` | mirror of `IsWithinLast` |
| `IsOlderThan` | `Older` (Now−2h, 1h) → true; `Exactly` (Now−1h, 1h) → false; `Newer` (Now−30m, 1h) → false; `Future` → false; `NegativeAge` → false; `Null` → false |
| `HasMaximumAge` (DateTime/DateOnly D-1; DTO D-14 with explicit birth offset matching the ahead/behind reference date) | `Under` (dob 2010-01-01, 17; age 16) → true; `ExactlyMax` (dob 2008-06-15, 18; age 18) → true; `AtNineteen` (dob 2007-06-15, 18; age 19) → false; `OneDayUnderNineteen` (dob 2007-06-16, 18; age 18) → true; `AheadOfUtcBoundary` (dob 2008-06-16, 17, clock `DefaultAheadOfUtc`; local today is 2026-06-16 so the age is 18) → false; `AheadOfUtcUnder` (dob 2008-06-17, 17, clock `DefaultAheadOfUtc`; age 17) → true; `BehindUtcBoundary` (dob 2008-06-15, 17, clock `DefaultBehindUtc`; local today is 2026-06-14, one day short of the birthday, so the age is 17) → true; `Negative` (years −1) → false; `Null` → false |
| `IsAgeInRange` (DateTime/DateOnly D-1; DTO D-14 fixed-offset basis) | `Inside` (dob 2000-01-01, 18, 65) → true; `AtMin`, `AtMax` → true; `BelowMin`, `AboveMax`, `InvertedBounds` (65, 18) → false; `Null` → false |
| `HasMinimumAge` (DTO only — new in this phase; D-14 offset basis `checked TryGetDateAtOffset(sample, value.Offset, out referenceDate)`, never `tp.LocalTimeZone`) | `MidnightAhead` (dob `2008-06-16T00:00:00+10:00`, 18): clock `Default` → false (today in the dob's own +10 offset is still 2026-06-15 — the birthday hasn't landed); clock `DefaultAheadOfUtc` → true (the later UTC instant rolls the +10 "today" to 2026-06-16 — same dob and offset, only the clock's instant moved, never a zone conversion applied to the clock itself); `MidnightBehind` (dob `2008-06-15T14:00:00-08:00`, 18): clock `Default` → true (today in the dob's own −08 offset is 2026-06-15, the birthday just landed); clock `DefaultBehindUtc` → false (the earlier UTC instant leaves the −08 "today" at 2026-06-14, one day short) |
| `IsSameDay` (DTO) | `SameUtcDate` (2026-06-15T01:00+00:00, 2026-06-15T23:00+00:00) → true; `SameInstantDifferentOffsets` (2026-06-15T23:00+00:00, 2026-06-16T09:00+10:00) → true (same UTC date); `DifferentUtcDate` → false; `BothNull` → true; `OneNull` → false |

Every expected value above was derived by hand at planning time (ages: 2008-06-15 → 18, 2008-06-16 → 17,
2007-06-15 → 19, 2010-01-01 → 16 as of 2026-06-15; 2008-02-29 → 17 as of 2026-02-28 and 18 as of
2026-03-01). The `DefaultAheadOfUtc`/`DefaultBehindUtc` rows in `HasMaximumAge` and the DTO
`HasMinimumAge` row additionally depend on this phase's calibration of those two clocks against T-01
U0 (stated above the table) — re-verified against the +10:00/−08:00 literals and unchanged. The
Claude Fable 5.1 review re-derives three rows (at least one of them a boundary row); expected values are
literals in the fixture, never computed in test code.

**Astra fixture correction [A-07]**: `IsAgeInRange.AtMin` and `.AtMax` are true (inclusive completed
years); only BelowMin/AboveMax/InvertedBounds are false. The maximum/between rows use D-1 for
DateTime/DateOnly, and D-14 for DateTimeOffset. For DTO ahead/behind boundary rows explicitly use
birth-date offsets +10:00/−08:00 respectively so the stated reference dates remain correct; add
the same birth date with a different offset and unchanged clock to prove the provider zone does
not determine DTO age. Test future DOB false, min/max integer thresholds and safe reference date
projection at DTO MinValue/MaxValue. No row may silently use provider local today for DTO age.

**Deletion (D-6)**: this unit ships `DateTimeRules.IsWithinLast`/`IsWithinNext`, so it also deletes
`DateTimeRules.IsWithinDaysFromNow` (`src/PineGuard.Core/Rules/DateTimeRules.cs:183`) and its test
coverage: `tests/PineGuard.Core.UnitTests/Rules/DateTimeRulesTestData.cs`,
`tests/PineGuard.Core.UnitTests/Rules/DateTimeRulesTests.cs` (remove only the
`IsWithinDaysFromNow` rows/cases; leave the rest of each file intact). No fixtures partial exists for
this rule (it was never migrated to the `XxxRulesFixtures` convention), so there is nothing to trim
there. `DateOnly`/`DateTimeOffset` never had this member — nothing to delete on those types. No
back-compat shim is added; per the standing no-back-compat-before-first-release rule the member is
removed outright, not `[Obsolete]`-shimmed.

**Acceptance**: gate with `Core`, both TFMs. Budget 150k / 45 min.

### U3a / U3b / U3c — Must clauses (one dispatch per type file)

**Removed-member baseline burn-down (D-6)**: the owning deletion units U2/U3b/U4b/U5a-2
remove only their deleted members' existing keys from `apps/cli/config/baseline.json`:
`layer-parity:WithinDaysFromNow:DataAnnotations`,
`must-usage:annotations:WithinDaysFromNow`,
`must-usage:annotations:NotWithinDaysFromNow`,
`test-structure:missing:tests/PineGuard.Core.UnitTests/Rules/DateTimeRulesTests.cs:IsWithinDaysFromNowSystemClock`,
`test-structure:missing:tests/PineGuard.MustClauses.UnitTests/MustDateTimeClausesTests.cs:NotWithinDaysFromNow`,
`test-structure:missing:tests/PineGuard.MustClauses.UnitTests/MustDateTimeClausesTests.cs:WithinDaysFromNow`.
Serialize edits to this shared file; no additions or `--update-baseline`. The completed
phase audit verifies the burn-down. [Binding Fable audit-gate ruling §5]

U3a is the sole writer of the `WithinValidity` entry in
`docs/ai/specs/language/vocabulary.json`: `absentFrom: ["DataAnnotations"]`, reason
`Cross-property [notBefore, notAfter) window (T-00 §4.4).` Enter it when the Must
concept lands; normalized positive/negative forms share the declaration. Other layers
remain required. Do not add an attribute, ignore, exception or baseline key for this
intentional absence. [Binding Fable audit-gate ruling §2]

**Files**: `MustDateTimeOffsetClauses.cs` (U3a), `MustDateTimeClauses.cs` (U3b), `MustDateOnlyClauses.cs`
(U3c), each with its TestData/tests. Members per T-00 §4.4 for each type (`DateOnly` variants take `int days`):

`NotExpired`, `Expired`, `Effective`, `WithinValidity(notBefore, notAfter, tp)`, `WithinLast`,
`NotWithinLast`, `WithinNext`, `NotWithinNext`, `OlderThan`, `NotOlderThan`, `MaximumAge`,
`AgeInRange`; plus on `DateTimeOffset` only: `MinimumAge`, `SameDay(value, other)`, `NotSameDay(value, other)`
(messages/codes identical to the `DateTime` versions).

`WithinValidity` returns `MustResult<T>` whose value is `notBefore` (clone the `Overlapping`
clause's handling of multi-argument values). `timeProvider` sits immediately before `paramName`.
Messages and codes exactly as T-00 §4.4.

**Deletion (D-6, U3b only)**: `MustDateTimeClauses.cs` ships `WithinLast`/`NotWithinLast`/`WithinNext`/
`NotWithinNext` for `DateTime`, so U3b also deletes `Must.Be.WithinDaysFromNow` and
`Must.Be.NotWithinDaysFromNow` (`src/PineGuard.MustClauses/MustDateTimeClauses.cs:494-534`) and their
tests in `tests/PineGuard.MustClauses.UnitTests/MustDateTimeClausesTests.cs` and
`MustDateTimeClausesTestData.cs` (remove only the `WithinDaysFromNow`/`NotWithinDaysFromNow`
cases). U3a/U3c are unaffected — the clause never existed on `DateTimeOffset` or `DateOnly`. Guard
deletion is U4b's job below; Fluent deletion is U5a-2's job. DataAnnotations never had a
`WithinDaysFromNow` attribute (confirmed by grep across `src/PineGuard.DataAnnotations` and its
tests) — there is nothing to delete at that layer. No back-compat shim.

**Acceptance** (each dispatch): applicable per-unit gate with `MustClauses`; `must-codes`
and `nullability` green. Ordering and the other cross-layer audits run at the completed
Phase 4 cohort after its final adapter, before the Fable phase review (T-00 §9).
Budget 150k / 45 min per dispatch.

### U4a / U4b / U4c — Guard clauses (one dispatch per type file, ∥ U5)

`GuardDateTimeOffsetClauses.cs` (U4a), `GuardDateTimeClauses.cs` (U4b), `GuardDateOnlyClauses.cs` (U4c) + tests.
Guard → Must mapping per T-00 §4.4 (D-15: the guard is named `NotExpired`, not `Unexpired`):
`Guard.Against.Expired` → calls `Must.Be.NotExpired`; `Guard.Against.NotExpired` → calls `Must.Be.Expired`;
`NotEffective` → `Effective`; `OutsideValidity` → `WithinValidity`; `NotWithinLast` → `WithinLast`;
`WithinLast` → `NotWithinLast`; `NotWithinNext` → `WithinNext`; `WithinNext` → `NotWithinNext`;
`NotOlderThan` → `OlderThan`; `OlderThan` → `NotOlderThan`; `AboveMaximumAge` → `MaximumAge`;
`AgeOutOfRange` → `AgeInRange`; on `DateTimeOffset` also `BelowMinimumAge` → `MinimumAge`,
`NotSameDay` → `SameDay`, `SameDay` → `NotSameDay`.

**Deletion (D-6, U4b only)**: `GuardDateTimeClauses.cs` deletes `Guard.Against.WithinDaysFromNow` and
`Guard.Against.NotWithinDaysFromNow` (`src/PineGuard.GuardClauses/GuardDateTimeClauses.cs:516-576`)
and their tests in `tests/PineGuard.GuardClauses.UnitTests/GuardDateTimeClausesTests.cs` and
`GuardDateTimeClausesTestData.cs` (remove only those cases). U4a/U4c are unaffected. No back-compat
shim.

Budget 150k / 45 min per dispatch.

### U5a-1 / U5a-2 / U5a-3 — Fluent (one dispatch per type file, ∥ U5b)

`FluentDateTimeOffsetExtensions.cs`, `FluentDateTimeExtensions.cs`, `FluentDateOnlyExtensions.cs` +
tests: `.NotExpired(tp)`, `.Expired(tp)`, `.Effective(tp)`, `.WithinLast(span, tp)`, `.NotWithinLast`,
`.WithinNext`, `.NotWithinNext`, `.OlderThan(age, tp)`, `.NotOlderThan`, `.MaximumAge(years, tp)`,
`.AgeInRange(min, max, tp)`; `DateTimeOffset` also `.MinimumAge`, `.SameDay(other)`, `.NotSameDay(other)` with a literal `other`
(as `FluentDateTimeExtensions.SameDay`); `.WithinValidity(T notAfter, TimeProvider? tp = null, string? message = null)`
on the `notBefore` property with a literal `notAfter`. No model-expression overloads (T-00 §12).
Nullable overloads for every member.

**Deletion (D-6, U5a-2 only)**: `FluentDateTimeExtensions.cs` deletes `.WithinDaysFromNow(days, tp)`
and `.NotWithinDaysFromNow(days, tp)` (`src/PineGuard.FluentValidation/FluentDateTimeExtensions.cs:358-379`)
and their tests in `tests/PineGuard.FluentValidation.UnitTests/FluentDateTimeExtensionsTests.cs` and
`FluentDateTimeExtensionsTestData.cs` (remove only those cases). U5a-1/U5a-3 are unaffected —
`DateTimeOffset` and `DateOnly` never had this extension. No back-compat shim.

Budget 150k / 45 min per dispatch.

### U5b-1 / U5b-2 / U5b-3 — DataAnnotations (one dispatch per type file, ∥ U5a)

`DateTimeOffsetAttributes.cs`, `DateTimeAttributes.cs`, `DateOnlyAttributes.cs` + tests:
`NotExpired<T>Attribute`, `Expired<T>Attribute`, `Effective<T>Attribute`, `WithinLast<T>Attribute(int days)`,
`WithinNext<T>Attribute(int days)`, `OlderThan<T>Attribute(int days)`, `MaximumAge<T>Attribute(int years)`,
`AgeInRange<T>Attribute(int minYears, int maxYears)`; `MinimumAgeDateTimeOffsetAttribute(int years)`.
`int days` converts with `TimeSpan.FromDays(days)` for `DateTime`/`DateTimeOffset`. No
`WithinValidity` attribute (cross-property). The clock comes from `ResolveTimeProvider(validationContext)`.
Tests use `ValidationContextFactory.WithTimeProvider(FixedTimeProvider.Default)`.
Budget 150k / 45 min per dispatch.

## 4. Verification and Definition of Done

T-00 §9 in full; Qodana; Sonar; Claude Fable 5.1 phase review; merge.
- `Must.Be.NotExpired(now.AddMinutes(-1), FixedTimeProvider.Default).Failed` with code `date.validity.expired`.
- `Must.Be.WithinLast(now.AddMinutes(-30), TimeSpan.FromHours(1), FixedTimeProvider.Default).Success`.
- `DateTimeOffsetRules.HasMinimumAge` exists with the same signature shape as the `DateTime` one (`layer-parity` green).
- `TryGetAge(new DateOnly(2008,2,29), new DateOnly(2026,2,28), out var y)` gives `y == 17`.
