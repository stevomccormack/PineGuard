<!-- metadata_header
type: plan
id: temporal-06-formats-epoch
version: 1.0
status: planned
last_updated: 2026-11-09
-->

# Plan T-06 — Phase 6: String formats (ISO 8601, RFC 3339, RFC 1123, ISO 8601 duration), `StringRules.DateTime`, Unix epoch

> Charter: [T-00](temporal-00-program.md). Naming: T-00 §4.6. Codes: T-00 §5 (`Date.Format`,
> `Date.Unix`, `Time.Format`, `Time.Duration.NotIso8601`). Depends on Phase 5 merged;
> shared code ownership requires 2 → 4 → 5 → 6. Repo: PineGuard. Worktree `.claude/worktrees/temporal-06-formats`,
> branch `feature/temporal-06-formats`.

> Review provenance and exception policy: T-00 §0 and
> [Astra decision register](temporal-astra-decisions.md). Implementation: Codex team. All reviews, decisions and councils: Claude Fable 5.1. [A-01]

## 1. Business plan

API boundaries receive dates as strings and epochs as numbers. There is no `StringRules.DateTime`
family at all (F-7), no strict-format rule, and epoch units need explicit methods and range checks.
Magnitude cannot reliably identify the caller's intended unit. Zod ships `iso.date/time/datetime/duration`; the competitive analysis marks duration High
Priority.

## 2. Units

All Phase 1 dependencies below are transitive through the mandatory Phase 5 merge.
No unit in this phase starts before Phase 5 merges, including the U1 codes unit.

| Unit | Title | Model / agent | Depends on | Fan-out | Scopes |
|---|---|---|---|---|---|
| U1 | Codes | Codex team | Phase 5 merged | ∥ U2 | — |
| U2a | Core: `StringUtility.DateTime` + `StringRules.DateTime` | Codex team / `validation-builder` | Phase 5 merged | ∥ U2b, U2c | `Core` |
| U2b | Core: strict date/time formats (`IsIso8601`, `IsRfc3339`, `IsRfc1123` on `StringRules.DateTimeOffset`; `IsIso8601` on `StringRules.DateOnly` / `StringRules.TimeOnly`) | Codex team / `validation-builder` | Phase 5 merged | ∥ U2a, U2c, U2d | `Core` |
| U2c | Core: `UnixTimeRules` + `TryFromUnix*` | Codex team / `validation-builder` | Phase 5 merged | ∥ U2a, U2b | `Core` |
| U2d | Core: ISO 8601 duration parser (`StringUtility.Iso8601Duration.cs`, `StringRules.TimeSpan.IsIso8601Duration`) | Codex team / `validation-builder` | Phase 5 merged | ∥ U2a, U2b, U2c | `Core` |
| U3 | Must: all string/epoch clauses | Codex team ×2 (`t06-u3a-strings`, `t06-u3b-epoch`) | U1, U2 | — | `MustClauses` |
| U4 | Guard | Codex team | U3 | ∥ U5 | `GuardClauses` |
| U5 | Fluent + DA | Codex team ×2 | U3 | ∥ U4 | `FluentValidation`, `DataAnnotations` |
| U6 | Phase review + merge | Claude Fable 5.1, orchestrator | all | last | — |

File ownership: U2a owns `StringUtility.DateTime.cs`, `StringRules.DateTime.cs`; U2b owns
`StringRules.DateTimeOffset.cs`, `StringRules.DateOnly.cs`, `StringRules.TimeOnly.cs` (+ their fixtures);
U2d owns `StringRules.TimeSpan.cs`, `StringUtility.Iso8601Duration.cs` (+ fixtures); U2c owns
`UnixTimeRules.cs` and the `DateTimeUtility.cs` additions. Disjoint.

## 3. Unit specifications

### U1 — Codes

`MustCodes.Date.cs`: the `Format` node exists (`Invalid`); add `NotIso8601`, `NotRfc3339`, `NotRfc1123` to it;
add nested `Unix` (`Prefix = Date.Prefix + ".unix"`; `NotSeconds`, `NotMilliseconds`). `MustCodes.Time.cs`:
`Format.NotIso8601` (the `Format` node exists with `Invalid`); `Duration.NotIso8601`.
Update `MustCodes.Date.cs`'s `// Serves:` mapping to include both new consumers
`MustStringDateTimeClauses.cs` and `MustUnixTimeClauses.cs`, preserving existing entries.
Exact final mapping:
`// Serves: MustDateTimeClauses.cs, MustDateOnlyClauses.cs, MustDateTimeOffsetClauses.cs, MustSqlDateTimeClauses.cs, MustStringDateOnlyClauses.cs, MustStringDateTimeOffsetClauses.cs, MustStringDateTimeClauses.cs, MustUnixTimeClauses.cs`
Budget 40k / 15 min.

### U2a — `StringUtility.DateTime` + `StringRules.DateTime`

Mirror `StringUtility.DateTimeOffset.cs` and `StringRules.DateTimeOffset.cs` member-for-member,
substituting `DateTime` and `DateTimeRules`:

```csharp
// StringUtility.DateTime — RoundtripKind combined with AssumeUniversal throws ArgumentException for DateTime.TryParse;
// AdjustToUniversal | AssumeUniversal makes every successful result Kind.Utc.
public static bool TryParse(string? value, out DateTime result, DateTimeStyles styles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces)
// StringRules.DateTime (no parse-only IsDateTime: the DTO family has none and the name would shadow the type)
IsInPast(string? value, Inclusion inclusion = Inclusion.Exclusive, DateTimeStyles styles = DefaultStyles, TimeProvider? timeProvider = null)
IsInFuture(...)
IsInRange(string? value, DateTime min, DateTime max, Inclusion inclusion = Inclusion.Inclusive, DateTimeStyles styles = DefaultStyles)
```

Fixtures: `StringRulesFixtures.DateTime.cs` mirroring `StringRulesFixtures.DateTimeOffset.cs` rows,
plus `"2026-06-15T12:00:00"` (no offset; `AssumeUniversal` → Kind Utc), `"2026-06-15T12:00:00Z"`,
`"2026-06-15T12:00:00+10:00"` (adjusted to universal: the utility test asserts `result.Kind == DateTimeKind.Utc`
and `result == new DateTime(2026, 6, 15, 2, 0, 0, DateTimeKind.Utc)`), `"not a date"`, null, empty.
Budget 120k / 45 min.

### U2b — Strict date/time formats [A-08]

Keep `StringRules.DateTimeOffset.IsIso8601`, `.IsRfc3339`, `.IsRfc1123`,
`StringRules.DateOnly.IsIso8601` and `StringRules.TimeOnly.IsIso8601`. Each named profile delegates
to one shared strict Try parser returning the parsed value; Must invokes that same parser once.
Never validate with a strict parser and then reparse through general culture/style semantics.

The supported ISO instant profile is extended `yyyy-MM-dd'T'HH:mm[:ss[.fraction]]` with required
`Z` or numeric offset; fraction has 1–7 ASCII digits when present. No offset-free DTO formats,
whitespace, calendar-week/ordinal/basic forms or host-local defaults. Document this as a supported
representable profile, not the whole ISO standard. Date-only is exactly `yyyy-MM-dd`; time-only
is `HH:mm`, `HH:mm:ss` or `HH:mm:ss.fraction` (1–7 digits), invariant and untrimmed.

RFC 3339 profile requires ASCII digits, T/t separator, seconds, Z/z or an explicit numeric offset,
and zero or 1–7 fractional digits. Reject space separator, leading/trailing whitespace, empty
fraction, leap seconds, `-00:00` unknown offset, >7 fractional digits and offsets beyond the DTO
±14-hour domain. State these representability restrictions in XML docs and user docs. RFC 1123
uses invariant exact `R`/GMT semantics, untrimmed. Enforce the same grammar at every layer.

General `TryParse`/`TryParseExact` remains separately configurable with existing invariant default
styles. Invalid style combinations/format arrays are configuration failures at Must boundaries;
strict named profiles do not accept caller parse styles or silently normalize their grammar.

Fixtures — ISO instant valid: `2026-06-15T12:00:00Z`, `2026-06-15T12:00:00+10:00`,
`2026-06-15T12:00:00.123Z`, `2026-06-15T12:00Z`;
invalid: offset-free datetime, `2026-06-15 12:00:00Z` (space: both ISO and RFC profile false),
date-only, culture-specific date, impossible month, whitespace, null and empty.
RFC 3339 valid: `2026-06-15T12:00:00Z`, `2026-06-15t12:00:00z`, `2026-06-15T12:00:00.5+10:00`;
invalid: `2026-06-15T12:00:00` (no offset), `2026-06-15T12:00Z` (no seconds), leap-second text,
`-00:00`, +14:01, Unicode digits, >7 fractional digits, empty fraction, spaces and huge input.
RFC 1123 valid: `Mon, 15 Jun 2026 12:00:00 GMT`; invalid: `Mon, 15 Jun 2026 12:00:00 UTC`, `15 Jun 2026`.

### U2d — ISO 8601 duration parser

```csharp
// StringUtility.Iso8601Duration (new partial file)
public static bool TryParse(string? value, out TimeSpan? duration)
// nonnegative standalone PnW OR ordered PnDTnHnMnS; >=1 component; T requires a time component.
// Y/date-M, mixed weeks, repeated/out-of-order units, signs and exponents are false.
// Seconds alone may have decimal point/comma and 1–7 ASCII fractional digits.
// Parse with TryParse and checked exact ticks; overflow/range/precision loss returns false.
// StringRules.TimeSpan
public static bool IsIso8601Duration(string? value)
```

Duration valid: `P1D`, `PT1H`, `PT1H30M`, `P1DT2H3M4.5S`, `P2W`, `PT0.5S`, `PT1H0M0S`; invalid:
`P`, `PT`, `P1DT`, `P1Y`, `P1M`, `P1W1D`, `1H`, `PT1H1H`, `P-1D`, out-of-order units,
exponents, Unicode digits, >7 fractional digits, overflow, null, empty. Zero durations are valid.
Bound the grammar and numeric input; avoid unbounded regex and decimal.Parse. If regex is retained,
use the Brain's timeout precedent. Test exact TimeSpan limits, one tick, 0.0000001 seconds and
rejected precision loss; never accumulate via double. [A-08]
Budget 120k / 45 min (U2b) and 100k / 30 min (U2d).

### U2c — `UnixTimeRules` + `TryFromUnix*`

New `+ src/PineGuard.Core/Rules/UnixTimeRules.cs`:

```csharp
public static class UnixTimeRules
{
    public static readonly long MinSeconds = DateTimeOffset.MinValue.ToUnixTimeSeconds();          // -62135596800
    public static readonly long MaxSeconds = DateTimeOffset.MaxValue.ToUnixTimeSeconds();          //  253402300799
    public static readonly long MinMilliseconds = DateTimeOffset.MinValue.ToUnixTimeMilliseconds();
    public static readonly long MaxMilliseconds = DateTimeOffset.MaxValue.ToUnixTimeMilliseconds();
    public static bool IsUnixSeconds(long? value)      => value is not null && value.Value >= MinSeconds && value.Value <= MaxSeconds;
    public static bool IsUnixMilliseconds(long? value) => value is not null && value.Value >= MinMilliseconds && value.Value <= MaxMilliseconds;
}
// DateTimeUtility
public static bool TryFromUnixSeconds(long? value, out DateTimeOffset? result)        // IsUnixSeconds ? FromUnixTimeSeconds : false
public static bool TryFromUnixMilliseconds(long? value, out DateTimeOffset? result)
```

Fixtures import the four bounds from the source class (fixture rule 3): `MinSeconds`, `MaxSeconds`,
`MinSeconds - 1`, `MaxSeconds + 1`, `0`, `1750000000` (2025), `1750000000000` (ms; also a legal
seconds value? `1.75e12 > MaxSeconds` → false as seconds, true as ms; state both rows), null.
Also test each unit's Min/Max and ±1 outside, negative epochs and values valid in both units.
Integral input and explicit units are required; never guess units by magnitude or claim all ×1000
mistakes are detectable. Any exposed truncation follows BCL Unix conversion semantics. [A-08]
Budget 100k / 30 min.

### U3a — Must: strings

New `MustStringDateTimeClauses.cs` mirroring `MustStringDateTimeOffsetClauses.cs` member-for-member
(`PastDateTime`, `PastOrPresentDateTime`, `FutureDateTime`, `FutureOrPresentDateTime`,
`InRangeDateTime`, and any others the DateTimeOffset file has). Add to
`MustStringDateTimeOffsetClauses.cs`: `Iso8601DateTimeOffset`, `Rfc3339DateTimeOffset`,
`Rfc1123DateTimeOffset` (→ `MustResult<DateTimeOffset>` from the shared strict parser, invoked once);
to `MustStringDateOnlyClauses.cs`: `Iso8601DateOnly`; to
`MustStringTimeOnlyClauses.cs`: `Iso8601TimeOnly`; to `MustStringTimeSpanClauses.cs`:
`Iso8601Duration` (→ `MustResult<TimeSpan>`). Messages/codes per T-00 §4.6.

### U3b — Must: epoch

New `MustUnixTimeClauses.cs`: `UnixSeconds(long value)`, `UnixMilliseconds(long value)` →
`MustResult<long>`; messages/codes per T-00 §4.6.

### U4 / U5

**Exact sibling ownership**: U4 adds `+ src/PineGuard.GuardClauses/GuardUnixTimeClauses.cs`
with `NotUnixSeconds` / `NotUnixMilliseconds` returning `long`, plus
`GuardUnixTimeClausesTests` and paired TestData. U5 adds
`+ src/PineGuard.FluentValidation/FluentUnixTimeExtensions.cs` with `UnixSeconds()` /
`UnixMilliseconds()` on `long` and `long?`, plus paired tests/TestData; it also adds
`+ src/PineGuard.DataAnnotations/UnixTimeAttributes.cs` with `UnixSecondsAttribute` /
`UnixMillisecondsAttribute`, plus `UnixTimeAttributesTests` and paired TestData.
U2c owns `UnixTimeRules.cs`, `IsUnixSeconds` / `IsUnixMilliseconds`,
`UnixTimeRulesTests`, TestData and `UnixTimeRulesFixtures.cs`; U3b owns
`MustUnixTimeClauses.cs` and its paired Tests/TestData. For the string DateTime family,
U4 owns `GuardStringDateTimeClauses.cs`; U5 owns `FluentStringDateTimeExtensions.cs`
and `StringDateTimeAttributes.cs`, each with paired tests/TestData. U2a owns
`StringRules.DateTime.cs` with nested `DateTime`. These exact siblings satisfy ordering's
family discovery; no missing-layer baseline or fake adapter is permitted.
[Binding Fable audit-gate ruling §4]

Guard `Not*` complements; Fluent extensions on `string?` and `long`/`long?`; DA attributes
`Iso8601DateTimeOffsetStringAttribute`, `Rfc3339DateTimeOffsetStringAttribute`, `Rfc1123DateTimeOffsetStringAttribute`,
`Iso8601DateOnlyStringAttribute`, `Iso8601TimeOnlyStringAttribute`, `Iso8601DurationTimeSpanStringAttribute`,
`UnixSecondsAttribute`, `UnixMillisecondsAttribute` (recorded exception, T-00 §4.6), plus the whole
`StringDateTimeAttributes.cs` file mirroring `StringDateTimeOffsetAttributes.cs` (`PastDateTimeStringAttribute`, …). Budgets 150k / 45 min each.

## 4. Verification and Definition of Done

T-00 §9 applicable per-unit gates and full completed-phase audit cohort after the final
adapter, then Qodana, Sonar and Claude Fable 5.1 phase review. Phase 5 must already be
merged before Phase 6 starts; publication follows T-00 §8 authorization.
- Both RFC 3339 and ISO instant clauses reject `"2026-06-15T12:00:00"` because no offset is supplied.
- `Must.Be.Iso8601Duration("P1Y").Failed` with code `time.duration.not-iso8601`.
- `UnixTimeRules.IsUnixSeconds(1750000000000) == false` and `IsUnixMilliseconds(1750000000000) == true`.
- The completed-phase audit cohort reports the `StringDateTime` and `UnixTime` families
  present in all required layers; ordering, layer-parity, must-usage and rules-usage remain
  strict. Baseline changes contain deletions only.
