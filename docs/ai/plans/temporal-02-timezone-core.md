<!-- metadata_header
type: plan
id: temporal-02-timezone-core
version: 1.0
status: planned
last_updated: 2026-09-22
-->

# Plan T-02 — Phase 2: Time-zone Core (utility, rules, offsets, zone-aware instants, five layers)

> Charter: [T-00](temporal-00-program.md). Decisions cited: **D-2, D-3, D-5**. Naming: T-00 §4.2.
> Codes: T-00 §5. Depends on Phase 1 merged (uses `TestTimeZones`, `FixedTimeProvider.LocalTimeZone`).
> Repo: PineGuard. Worktree `.claude/worktrees/temporal-02-timezone`, branch `feature/temporal-02-timezone`.

## 1. Business plan

This is the deterministic supplied-zone layer: legal offsets, local gaps/overlaps, offset matching
at an instant and same calendar day in a zone. Core does not resolve installed IDs or modify the
Brain no-IO rule. Existing adapters resolve IDs once at composition; Standards keeps versioned
catalog membership and IANA↔Windows mapping. These are different questions. [A-05]

Review authority/exception policy: T-00 §0 and [Astra register](temporal-astra-decisions.md).
Implementation: Codex team; all reviews/decisions/councils: Claude Fable 5.1.

## 2. Units

| Unit | Title | Model / agent | Depends on | Fan-out | Coverage scopes |
|---|---|---|---|---|---|
| U0 | Audit model corrections | Codex team / `general-purpose` | Phase 1 merged | first; ∥ U1/U2 | none (CLI tests + no-baseline audit contract) |
| U1 | Codes | Codex team / `general-purpose` | — | ∥ U2 | — (`audit must-codes` after U3) |
| U2 | Core utility + rules + string offset | Codex team / `validation-builder` | Phase 1 | ∥ U1 | `Core` |
| U3 | Must clauses | Codex team / `validation-builder` | U0, U1, U2 | — | `MustClauses` |
| U4 | Guard clauses | Codex team / `validation-builder` | U3 | — | `GuardClauses` |
| U5 | Fluent extensions | Codex team / `validation-builder` | U4 | ∥ U6 | `FluentValidation` |
| U6 | DataAnnotations attributes | Codex team / `validation-builder` | U4 | ∥ U5 | `DataAnnotations` |
| U7 | Phase review + merge | Claude Fable 5.1 per unit and phase, orchestrator | all | last | — |

## 3. Unit specifications

U0 below is a mandatory audit prerequisite for every temporal Must unit; U1/U2 may run
alongside it where file ownership is disjoint. No source implementation is authorized by
this plan review: these are later implementation contracts.

### U0 — Audit model corrections (first in Phase 2)

**Model/role**: Codex team / `general-purpose`; charter dispatch envelope applies.
**Depends on**: Phase 1 merged. **Fan-out**: parallel U1/U2 only; completes before U3.
**Source coverage scope**: none. Exact binding contract is
[Fable audit-gate ruling](temporal-audit-gate-ruling.md), including schema, rule descriptions,
fixtures, finding keys and baseline deletions. No implementer chooses a mechanism.

**Files and ownership**:

| File | Required change |
|---|---|
| `apps/cli/src/audit/types.ts` | Add VocabularyConfig.layerAbsences with the exact schema below |
| `apps/cli/src/audit/vocabulary.ts` (new) | Move resolveVocabulary/normalizeConcept from layer-parity; DEFAULT_VOCABULARY includes empty layerAbsences |
| `apps/cli/src/audit/rules/layer-parity.ts` | Honor declared concept/layer absences; reject unknown concept, invalid layer and declared-absent-but-implemented cases; update description |
| `apps/cli/src/audit/rules/must-usage.ts` | Share vocabulary normalization; skip only declared concept/layer absence, preserving all other findings and keys; update description |
| `apps/cli/src/audit/rules/ordering.ts` | String-tail Rules discovery uses StringRules.<Rest>.cs and nested <Rest>; empty Rest uses StringRules.cs/StringRules; preserve non-string mapping/domainName; correct expected missing path |
| `docs/ai/specs/language/vocabulary.json`, `vocabulary.md` | Version 2 schema and SameDay declaration; §2 step 5 documents two-way enforcement |
| `docs/ai/specs/tools/audit-cli/spec.md` §3.1 | Exact matching updated rule descriptions from the ruling |
| `apps/cli/config/baseline.json` | Delete only prescribed debt keys; no additions |
| Audit tests and fixtures under `apps/cli/test/` | VIBE vectors below and paired test registrations using existing rule-test conventions |

```ts
readonly layerAbsences: Readonly<Record<string, {
    readonly absentFrom: readonly ("Guard" | "Fluent" | "DataAnnotations")[];
    readonly reason: string;
}>>;
```

Keys use normalized concepts, so NotSameDay is covered by SameDay. Must is never a legal
absence. Unknown concept → `layer-parity:config:unknown-concept:<Key>`; invalid layer →
`layer-parity:config:invalid-layer:<Key>`; implementation of declared absence →
`layer-parity:<Concept>:<Layer>:declared-absent-but-implemented`. A declaration skips only
that pair's missing-layer/must-usage finding. It never hides other layers or concepts.

**VIBE vectors** (paths relative to `apps/cli`):

- `test/fixtures/layer-parity/absences/`: Solo absent DA with Must/Guard/Fluent present;
  Twice absent Guard but implemented; Ghost absent Fluent but unknown; Bad absent Must.
  Exactly the three prescribed Twice/Ghost/Bad findings, no Solo finding. Existing invalid
  fixture still proves an undeclared DA gap flags.
- `test/fixtures/must-usage/absences/`: ResolvedX called from Guard/DA, Fluent declared
  absent → zero ResolvedX findings; Other still reports `must-usage:fluent:Other`.
- `test/fixtures/ordering/string-family/`: complete Delta string sibling family including
  nested Rules Delta → zero; Echo missing Rules → one finding naming
  `src/PineGuard.Core/Rules/StringRules.Echo.cs`; existing invalid Charlie remains four.

Delete only `layer-parity:SameDay:DataAnnotations`,
`must-usage:annotations:SameDay`, `must-usage:annotations:NotSameDay`, and every
`ordering:MustString*Clauses:rules:missing-layer` baseline key. If corrected discovery
exposes a real existing string order mismatch, reorder declarations without behavioral
change as prescribed in the ruling; never baseline it or add suppression. The future
implementation inventories those exact affected files before its corrective dispatch.

**Acceptance**: `pnpm -C apps/cli test` green; attach
`pnpm -C apps/cli exec tsx src/index.ts audit --no-baseline layer-parity ordering must-usage`
output. No new baseline key, ignoreMethods entry, aliases workaround, exceptions entry or
`--update-baseline`. U0 lands before any temporal Must implementation.

The exact U0 fast/full gates are `pnpm -C apps/cli typecheck`, `pnpm -C apps/cli lint`,
`pnpm -C apps/cli test`, `pnpm -C apps/cli exec tsx src/index.ts audit --gate`
(Brain docs changed), `pnpm -C apps/cli exec tsx src/index.ts audit`, and the no-baseline three-rule artifact
above; baseline diff is deletions only. Copy these rule descriptions verbatim into their
rule files and `docs/ai/specs/tools/audit-cli/spec.md` §3.1:

```text
layer-parity:
Every normalised validation concept implemented by Must, Guard, Fluent, or DataAnnotations is implemented by all of them, except in a layer that vocabulary.json layerAbsences declares absent for that one concept, which must then stay absent (concept-set parity via vocabulary.json; no build, no dotnet publish).

must-usage:
Every public Must clause (src/PineGuard.MustClauses) is invoked from Guard, Fluent, and DataAnnotations, except in a layer that vocabulary.json layerAbsences declares absent for that clause's concept. Supersedes legacy Rule03 (Guard), Rule04 (Fluent), and Rule05 (DataAnnotations) — see this module's header comment.
```

### U1 — Codes

**Files**: add `src/PineGuard.Core/Codes/MustCodes.TimeZone.cs` exactly as T-00 §5 (with the
per-constant `/// <summary><c>…</c></summary>` lines and the file-top `// Serves:` comment listing
`MustTimeZoneClauses.cs, MustStringUtcOffsetClauses.cs`). Clone the file shape from
`MustCodes.Time.cs`. Do not edit MustCodes.Date.cs in this unit: its Kind node and
NotUtc/NotUnspecified constants already exist for the Date clause family. Never duplicate
them or consume them from TimeZone clauses; declare the TimeZone-domain codes in T-00 §5.
Budget 40k / 15 min.

Declare only `MustCodes.TimeZone.Kind.NotUnspecified` (`timezone.kind.not-unspecified`), message
`"{paramName} must have DateTimeKind.Unspecified."`, attributed to value.
For both local-time clauses, Zone.Null precedes this Kind check; Kind.Utc and Kind.Local
fail before the rule. No positive TimeZone.Kind.Unspecified code is declared.
Reference projection uses foundation-owned `Date.Reference.OutOfRange` (T-01 / A-07)
only in its owning Date clauses, never as a TimeZone-domain failure; input projection
uses its rule value failure. Unknown host ID is adapter configuration, not a new Core host clause.
The timezone ID constant, if required by DA, must list its actual adapter consumer so the audit
does not retain an unreferenced Core clause constant. [A-10, A-12]

**Controlling Fable correction**: `TimeZone.Id.Unknown` is produced by the deterministic
`Must.Be.ResolvedTimeZone` outcome clause in MustTimeZoneClauses. Core and Must perform
no installed-ID lookup. DA resolves once at its composition boundary, then passes the
`TimeZoneInfo?` outcome and configured `timeZoneId` to this clause. Reuse the existing
constant; add no code or audit exemption. Exact unknown-ID message:
`"Time zone ID '{paramName}' is not a known time zone."`, using MustMessage's supported
parameter-name substitution, attributed
to `nameof(timeZoneId)` and carrying the ID as failing value. Must owns it; DA reuses
the result verbatim. Add direct Must outcome tests and DA-to-Must audit pairing.

### U2 — Core utility + rules + string offset

**Files**:

| Action | File |
|---|---|
| Add | `src/PineGuard.Core/Utils/TimeZoneUtility.cs` |
| Add | `src/PineGuard.Core/Rules/TimeZoneRules.cs` |
| Add | `src/PineGuard.Core/Utils/StringUtility.UtcOffset.cs` (partial `StringUtility`, nested `public static class UtcOffset`) |
| Add | `src/PineGuard.Core/Rules/StringRules.UtcOffset.cs` (partial `StringRules`, nested `public static class UtcOffset`) |
| Add | `tests/PineGuard.Testing/Fixtures/TimeZoneRulesFixtures.cs`, `StringRulesFixtures.UtcOffset.cs` |
| Add | `tests/PineGuard.Core.UnitTests/Utils/TimeZoneUtilityTests.cs` + `TestData`, `Rules/TimeZoneRulesTests.cs` + `TestData`, `Utils/StringUtilityUtcOffsetTests.cs` + `TestData` (name per the existing `StringUtility.*` test file convention — copy the DateTimeOffset one), `Rules/StringRulesUtcOffsetTests.cs` + `TestData` |

**Clone from**: `DateTimeUtility.cs` (utility shape), `DateTimeOffsetRules.cs` (rule shape),
`StringUtility.DateTimeOffset.cs` / `StringRules.DateTimeOffset.cs` (nested string families).

**Exact output** — `TimeZoneUtility` (`namespace PineGuard.Utils`):

```csharp
public static class TimeZoneUtility
{
    /// <summary>Gets the offset <paramref name="timeZone"/> applies at <paramref name="instant"/>.</summary>
    public static bool TryGetUtcOffset(TimeZoneInfo? timeZone, DateTimeOffset instant, out TimeSpan offset)
    {
        offset = TimeSpan.Zero;
        if (timeZone is null) return false;
        offset = timeZone.GetUtcOffset(instant);
        return true;
    }
}
```

`TimeZoneRules` (`namespace PineGuard.Rules`):

```csharp
public static class TimeZoneRules
{
    /// <summary>The smallest legal UTC offset, −14:00.</summary>
    public static readonly TimeSpan MinUtcOffset = TimeSpan.FromHours(-14);
    /// <summary>The largest legal UTC offset, +14:00.</summary>
    public static readonly TimeSpan MaxUtcOffset = TimeSpan.FromHours(14);

    public static bool IsUtcOffset(TimeSpan? value) =>
        value is not null && value.Value.Ticks % TimeSpan.TicksPerMinute == 0 && value.Value >= MinUtcOffset && value.Value <= MaxUtcOffset;

    public static bool IsInvalidTime(DateTime? value, TimeZoneInfo? timeZone) =>
        value is not null && timeZone is not null && value.Value.Kind == DateTimeKind.Unspecified && timeZone.IsInvalidTime(value.Value);

    public static bool IsAmbiguousTime(DateTime? value, TimeZoneInfo? timeZone) =>
        value is not null && timeZone is not null && value.Value.Kind == DateTimeKind.Unspecified && timeZone.IsAmbiguousTime(value.Value);

    public static bool IsDaylightSavingTime(DateTimeOffset? value, TimeZoneInfo? timeZone) =>
        value is not null && timeZone is not null && timeZone.IsDaylightSavingTime(value.Value);

    public static bool HasOffsetOf(DateTimeOffset? value, TimeZoneInfo? timeZone) =>
        value is not null && timeZone is not null && timeZone.GetUtcOffset(value.Value) == value.Value.Offset;

    public static bool IsSameDay(DateTimeOffset? value, DateTimeOffset? other, TimeZoneInfo? timeZone)
    {
        if (timeZone is null) return false;
        if (value is null && other is null) return true;
        if (value is null || other is null) return false;
        var valueOffset = timeZone.GetUtcOffset(value.Value);
        var otherOffset = timeZone.GetUtcOffset(other.Value);
        return DateTimeOffsetUtility.TryGetDateAtOffset(value.Value, valueOffset, out var valueDate)
            && DateTimeOffsetUtility.TryGetDateAtOffset(other.Value, otherOffset, out var otherDate)
            && valueDate == otherDate;
    }
}
```

Gap/overlap inputs are wall-clock `DateTimeKind.Unspecified` only. Utc/Local fail value validation;
do not convert or bind to the host zone. Positive and complement clauses first validate the same
domain: value, zone and required Kind. `ValidLocalTime` and `UnambiguousLocalTime` cannot be naive
negation that accepts unsupported inputs. Zone configuration is checked before nullable equality.
Before zone date projection, detect unrepresentable UTC+offset ticks; do not accept implicit BCL boundary handling
as a valid comparison. Input projection failure is a value failure; reference/provider projection
failure is dependency failure. Expected errors use existing Must boundary behavior. [A-05, A-10]

`StringUtility.UtcOffset.TryParse(string? value, out TimeSpan? offset)`:
trim via `TryGetTrimmed`; `"Z"` or `"z"` → `TimeSpan.Zero`; otherwise regex
`^(?<sign>[+-])(?<h>\d{2})(?::?(?<m>\d{2}))?$` (built through `StringUtility.TryCreateRegex`,
cached in a `static readonly Regex`); `h ≤ 14`, `m ≤ 59`; result `sign × (h:m)`; finally require
`TimeZoneRules.IsUtcOffset(result)`; any failure → `false`, `offset = null`.
`StringRules.UtcOffset.IsUtcOffset(string? value)` → `StringUtility.UtcOffset.TryParse(value, out _)`.

**Fixtures** (`TimeZoneRulesFixtures`, one nested class per rule; all zones from `TestTimeZones`):

| Group | Valid | Invalid |
|---|---|---|
| `IsUtcOffset` | `Zero`, `PlusTen = 10h`, `MinusEight = -8h`, `PlusFiveThirty = 5h30m`, `PlusFourteen = 14h`, `MinusFourteen = -14h`, `PlusFiveFortyFive = 5h45m` | `NullValue`, `PlusFourteenOne = 14h1m`, `MinusFourteenOne`, `HalfMinute = 30s`, `WithSeconds = 1h0m30s` |
| `IsInvalidTime` | `Gap = (TestTimeZones.InvalidLocalTime, DaylightSaving)` | `Ordinary`, `Overlap = (AmbiguousLocalTime, DaylightSaving)` (ambiguous is not invalid), `FixedZone = (InvalidLocalTime, PlusTen)`, `NullValue`, `NullZone` |
| `IsAmbiguousTime` | `Overlap` | `Ordinary`, `Gap`, `FixedZone`, `NullValue`, `NullZone` |
| Half-hour DST gap/overlap | `IsInvalidTime(HalfHourInvalidLocalTime, HalfHourDaylightSaving)`; `IsAmbiguousTime(HalfHourAmbiguousLocalTime, HalfHourDaylightSaving)` | Ordinary time is neither; overlap is not invalid; gap is not ambiguous |
| `IsDaylightSavingTime` | `Summer = (2026-07-01T12:00+02:00, DaylightSaving)` | `Winter = (2026-01-15T12:00+01:00, DaylightSaving)`, `FixedZone`, `NullValue`, `NullZone` |
| `HasOffsetOf` | `PlusTenMatches = (2026-06-15T22:00+10:00, PlusTen)`, `SummerMatches = (2026-07-01T12:00+02:00, DaylightSaving)`, `WinterMatches = (2026-01-15T12:00+01:00, DaylightSaving)` | `UtcStampedOnPlusTen = (2026-06-15T22:00+00:00, PlusTen)`, `WrongSeason = (2026-07-01T12:00+01:00, DaylightSaving)`, `NullValue`, `NullZone` |
| `IsSameDay` | `SameLocalDayAcrossUtcMidnight = (2026-06-15T23:00+00:00, 2026-06-16T01:00+00:00, PlusTen)` (both 06-16 at +10), `BothNull` | `DifferentLocalDaySameUtcDay = (2026-06-15T13:00+00:00, 2026-06-15T15:00+00:00, PlusTen)` (06-15 23:00 vs 06-16 01:00), `OneNull`, `NullZone` |
| `StringRulesFixtures.UtcOffset.IsUtcOffset` | `"Z"`, `"z"`, `"+10:00"`, `"-08:00"`, `"+0530"`, `"+05"`, `"-14:00"`, `"+14:00"`, `" +10:00 "` | `null`, `""`, `"+14:01"`, `"-15:00"`, `"10:00"`, `"+1:00"`, `"+10:60"`, `"UTC"`, `"+10:00:00"` |

`TimeZoneUtilityTests`: `TryGetUtcOffset`
rows: `(PlusTen, any)` → 10h, `(DaylightSaving, summer)` → 2h, `(DaylightSaving, winter)` → 1h,
`(null, any)` → false.

**Acceptance**: gate with `Core`, both TFMs. Budget 150k / 45 min.

### U3 — Must clauses

**Outcome validator signature [controlling Fable correction]**:

U3 also owns the ResolvedTimeZone entry in `docs/ai/specs/language/vocabulary.json`:
`absentFrom: ["Fluent"]`, exact reason from T-00 §5. It is written only when the Must
concept lands. Guard is present via NotResolvedTimeZone; DA calls the Must outcome clause.
No aliases/ignoreMethods/exceptions/new baseline are authorized.
`MustResult<TimeZoneInfo> ResolvedTimeZone(this IMustClause _, TimeZoneInfo? resolvedZone,
string? timeZoneId, string? paramName = null)` in `MustTimeZoneClauses.cs`, with the
standard XML docs and CallerArgumentExpression convention for the supplied ID. Secondary
null `timeZoneId` fails first using `MustCodes.TimeZone.Id.Null` (`timezone.id.null`), message
`"{paramName} must not be null."`, failing value null and `ParamName = nameof(timeZoneId)`.
Non-null ID plus null outcome fails with TimeZone.Id.Unknown and ID attribution; a supplied
non-null outcome succeeds and returns that zone. No lookup, environmental query or cache.
Tests pin both failure paths, precedence and success. Use exact unknown-ID template
`"Time zone ID '{paramName}' is not a known time zone."`; MustMessage substitutes the
dependency name while the supplied ID remains the failing value. [Fable correction #5;
prior `{0}` template retracted]

**Files**: add `src/PineGuard.MustClauses/MustTimeZoneClauses.cs`,
`src/PineGuard.MustClauses/MustStringUtcOffsetClauses.cs`; tests
`tests/PineGuard.MustClauses.UnitTests/MustTimeZoneClausesTests.cs` + `TestData`,
`MustStringUtcOffsetClausesTests.cs` + `TestData`.

Zone-aware `SameDay` / `NotSameDay` belong to `MustTimeZoneClauses.cs` and its paired
Tests/TestData, because every failure must use the owning TimeZone domain. The plain
two-argument DTO overloads remain in MustDateTimeOffsetClauses with Date.Equality codes.
[Controlling Fable audit correction; no audit exemption]

**Clone from**: `MustDateTimeOffsetClauses.cs` (`Past`, `Within`) for the method shape;
zone-taking SameDay overloads live in MustTimeZoneClauses and emit TimeZone.Equality codes.
Clone the parse-and-return string clause shape from `MustStringGuidClauses.cs`.

**Exact members** (all `this IMustClause _` first, `paramName` last with standard
CallerArgumentExpression; ResolvedTimeZone names `timeZoneId`, other rows name `value`):

Exact zone-aware equality signatures in MustTimeZoneClauses:
`MustResult<DateTimeOffset> SameDay(this IMustClause _, DateTimeOffset value,
DateTimeOffset other, TimeZoneInfo timeZone, [CallerArgumentExpression(nameof(value))]
string? paramName = null)` and the same shape for `NotSameDay`. Both first return
`Fail(TimeZone.Zone.Null, "{paramName} must not be null.", nameof(timeZone), value)`
for a null zone. Semantic failures carry value/paramName. No DA equality attribute is
added (cross-property). Plain two-argument DTO equality remains untouched in its Date family.

| Member | Rule call | Result | Message | Code |
|---|---|---|---|---|
| `MustResult<TimeZoneInfo> ResolvedTimeZone(TimeZoneInfo? resolvedZone, string? timeZoneId, paramName)` | No lookup; secondary null ID first, then inspect supplied outcome | non-null `resolvedZone` | `Time zone ID '{paramName}' is not a known time zone.` | `TimeZone.Id.Unknown`; null ID uses `TimeZone.Id.Null` and existing null template |
| `MustResult<TimeSpan> UtcOffset(TimeSpan value, paramName)` | `TimeZoneRules.IsUtcOffset(value)` | `value` | `{paramName} must be a valid UTC offset.` | `TimeZone.Offset.Invalid` |
| `MustResult<TimeSpan> UtcOffset(string? value, paramName)` (in `MustStringUtcOffsetClauses`) | `StringUtility.UtcOffset.TryParse(value, out var offset)` | `offset ?? default` | same | same |
| `MustResult<DateTime> ValidLocalTime(DateTime value, TimeZoneInfo timeZone, paramName)` | valid domain first, then `!IsInvalidTime` | `value` | `{paramName} must be a valid local time in the time zone.` | `TimeZone.LocalTime.Invalid` |
| `MustResult<DateTime> UnambiguousLocalTime(DateTime value, TimeZoneInfo timeZone, paramName)` | valid domain first, then `!IsAmbiguousTime` | `value` | `{paramName} must be an unambiguous local time in the time zone.` | `TimeZone.LocalTime.Ambiguous` |
| `MustResult<DateTimeOffset> InTimeZone(DateTimeOffset value, TimeZoneInfo timeZone, paramName)` | `TimeZoneRules.HasOffsetOf(value, timeZone)` | `value` | `{paramName} must be expressed in the time zone.` | `TimeZone.Offset.Mismatch` |
| `MustResult<DateTimeOffset> DaylightSavingTime(DateTimeOffset value, TimeZoneInfo timeZone, paramName)` | `TimeZoneRules.IsDaylightSavingTime(value, timeZone)` after Zone.Null guard | `value` | `{paramName} must be in daylight saving time in the time zone.` | `TimeZone.DaylightSaving.NotInEffect` |
| `MustResult<DateTimeOffset> NotDaylightSavingTime(DateTimeOffset value, TimeZoneInfo timeZone, paramName)` | common domain checks then `!TimeZoneRules.IsDaylightSavingTime(value, timeZone)` | `value` | `{paramName} must not be in daylight saving time in the time zone.` | `TimeZone.DaylightSaving.InEffect` |
| `MustResult<DateTimeOffset> SameDay(DateTimeOffset value, DateTimeOffset other, TimeZoneInfo timeZone, paramName)` | `TimeZoneRules.IsSameDay(value, other, timeZone)` | `value` | `{paramName} must be the same day in the time zone.` | `MustCodes.TimeZone.Equality.NotSameDay` (`timezone.equality.not-same-day`) |
| `MustResult<DateTimeOffset> NotSameDay(DateTimeOffset value, DateTimeOffset other, TimeZoneInfo timeZone, paramName)` | `!TimeZoneRules.IsSameDay(...)` after common domain checks | `value` | `{paramName} must not be the same day in the time zone.` | `MustCodes.TimeZone.Equality.SameDay` (`timezone.equality.same-day`) |

For semantic zone-taking members only, `timeZone` is required and non-nullable;
ResolvedTimeZone accepts the nullable outcome/ID pair described above. A semantic
`null` zone argument never throws (D-13): the clause
returns `MustResult<T>.Fail(MustCodes.TimeZone.Zone.Null, "{paramName} must not be null.", nameof(timeZone), value)`
before calling the rule, exactly as `MustPredicateClauses` reports a null `predicate` with
`Predicate.Callback.Null` (clone that guard clause; adjust the code and parameter name). The full
D-13 contract this Must behaviour anchors: Guard clauses (U4) inherit this failure and throw as
usual, no special-casing; Fluent extensions (U5) call `ThrowHelper.ThrowIfNull(timeZone)` at
composition time (existing adapter pattern, `ValidationResultExtension.cs:47`), so a null `timeZone`
never reaches the Must clause from Fluent; DataAnnotations attribute constructors (U6) call
`ThrowHelper.ThrowIfNull(timeZoneId)` at construction time, then resolve the zone at validation time
at the adapter boundary (no Core resolver) and fail unknown ids with code `timezone.id.unknown` and message
from `Must.Be.ResolvedTimeZone` (`"Time zone ID '{paramName}' is not a known time zone."`), with ID dependency
attribution; no DA-local diagnostic message. None of this phase's attributes take a
`TimeProvider`, so D-13's `ResolveTimeProvider(validationContext)` clause does not apply to U6.

**Tests**: one TestData group per member with `ValidCases`/`InvalidCases`; at least one invalid case
asserts the exact message and `Code`; every group uses `TimeZoneRulesFixtures` via `using F = …`.
Add one invalid row per zone-taking member with `timeZone = null!` expecting `IsValid == false`,
message `"timeZone must not be null."`, code `timezone.zone.null`.

For ValidLocalTime and UnambiguousLocalTime, check Zone.Null first, then fail Kind.Utc or
Kind.Local with `MustCodes.TimeZone.Kind.NotUnspecified`, exact A-12 template
`"{paramName} must have DateTimeKind.Unspecified."`, failing value value and ParamName
paramName, before calling the rule. Do not add a positive Kind.Unspecified constant.
ResolvedTimeZone tests cover null ID, unknown ID, success and null-ID-before-null-outcome.
The zone-aware equality tests use TimeZoneRulesFixtures.IsSameDay and a null-zone row.

**Acceptance**: per-unit gate with `MustClauses`; `audit must-codes` and `nullability` green.
Ordering/layer-parity/must-usage/rules-usage run at the completed phase cohort after the
final adapter, before Fable phase review.
Budget 150k / 45 min.

For all complements, validate the common domain before negating. Null/invalid zone configuration
precedes nullable equality handling; Kind failures are value failures. `NotSameDay` must not pass
invalid configuration through `!IsSameDay`. Contain only expected validation/configuration/range
failures according to existing Must boundaries; no blanket custom-provider catch. [A-10, A-12]

### U4 — Guard clauses

**Files**: add `src/PineGuard.GuardClauses/GuardTimeZoneClauses.cs`,
`GuardStringUtcOffsetClauses.cs`; tests `GuardTimeZoneClausesTests.cs` + `TestData`,
`GuardStringUtcOffsetClausesTests.cs` + `TestData`.

First method in GuardTimeZoneClauses (matching Must order) is the deterministic outcome
guard below. It performs no lookup and returns the supplied zone instance. Fluent
ResolvedTimeZone is intentionally absent: Fluent accepts an already supplied TimeZoneInfo
and checks it at composition; this absence is declared in vocabulary layerAbsences.
[Binding Fable B2/B3 audit ruling]

```csharp
public static TimeZoneInfo NotResolvedTimeZone(this IGuardClause _,
    TimeZoneInfo? resolvedZone,
    string? timeZoneId,
    string? message = null,
    Func<Exception>? exceptionCreator = null,
    [CallerArgumentExpression(nameof(timeZoneId))] string? paramName = null)
{
    var result = Must.Be.ResolvedTimeZone(resolvedZone, timeZoneId, paramName);
    if (result.Failed)
        GuardFailure.Throw(result, message, exceptionCreator);
    return result.Result!;
}
```

GuardTimeZoneClausesTests/TestData cover null ID (`timeZoneId must not be null.`),
unknown ID (`Time zone ID 'timeZoneId' is not a known time zone.`), null-ID precedence,
and success returning the identical non-null instance.
GuardTimeZoneClauses also includes `NotDaylightSavingTime` calling Must.DaylightSavingTime
and `DaylightSavingTime` calling Must.NotDaylightSavingTime, returning DateTimeOffset and
using normal GuardFailure. Order them after InTimeZone and before SameDay, positive Must
before its Not* complement. Both semantic Must methods use Zone.Null guard first,
`"{paramName} must not be null."`, nameof(timeZone), value; standard CallerArgumentExpression
nameof(value) and paramName-last signature. Tests cover Summer/Winter/FixedZone/null zone
and exact normal/configuration codes/messages.

Zone-aware SameDay / NotSameDay land in `GuardTimeZoneClauses.cs` and its paired
Tests/TestData, with swapped polarity calling the corresponding TimeZone Must clauses.

**Clone from**: `GuardDateTimeOffsetClauses.cs` (`Past`), `GuardStringGuidClauses.cs` (`NotGuid`).

| Guard | Calls | Returns |
|---|---|---|
| `NotUtcOffset(TimeSpan value, …)` | `Must.Be.UtcOffset(TimeSpan)` | `TimeSpan` |
| `NotUtcOffset(string? value, …)` | `Must.Be.UtcOffset(string)` | `TimeSpan` |
| `InvalidLocalTime(DateTime value, TimeZoneInfo timeZone, …)` | `Must.Be.ValidLocalTime` | `DateTime` |
| `AmbiguousLocalTime(DateTime value, TimeZoneInfo timeZone, …)` | `Must.Be.UnambiguousLocalTime` | `DateTime` |
| `NotInTimeZone(DateTimeOffset value, TimeZoneInfo timeZone, …)` | `Must.Be.InTimeZone` | `DateTimeOffset` |
| `NotSameDay(DateTimeOffset value, DateTimeOffset other, TimeZoneInfo timeZone, …)` | `Must.Be.SameDay(…, timeZone)` | `DateTimeOffset` |
| `SameDay(DateTimeOffset value, DateTimeOffset other, TimeZoneInfo timeZone, …)` | `Must.Be.NotSameDay(…, timeZone)` | `DateTimeOffset` |

Guard TestData follows fixture-conventions rule 6 (extract `tc.Value` to a local; explicit
`GuardExpected` factories). A `null` `timeZone` needs no special-casing here (D-13): the guard
calls the Must clause, which returns the `timezone.zone.null` failure, and `GuardFailure.Throw`
throws from that failure's message as usual. Add one invalid row per zone-taking guard with
`timeZone = null!` asserting it throws with message `"timeZone must not be null."`.
**Acceptance**: per-unit gate with `GuardClauses`; nullability and applicable must-codes
checks green. Layer-parity runs at the completed phase cohort after the final adapter.
Budget 120k / 45 min.

### U5 — Fluent extensions (∥ U6)

**Files**: add `src/PineGuard.FluentValidation/FluentTimeZoneExtensions.cs` (`TimeSpan`/`TimeSpan?`
`UtcOffset`, `DateTime`/`DateTime?` `ValidLocalTime(TimeZoneInfo)`,
`UnambiguousLocalTime(TimeZoneInfo)`, `DateTimeOffset`/`DateTimeOffset?` `InTimeZone(TimeZoneInfo)`,
`SameDay(DateTimeOffset other, TimeZoneInfo)`, `NotSameDay(...)` with a literal `other`, the shape
`FluentDateTimeExtensions.SameDay` has today — no model-expression overloads), `FluentStringUtcOffsetExtensions.cs`
(`UtcOffset()` on `string?`);
tests `FluentTimeZoneExtensionsTests.cs` + `TestData`, `FluentStringUtcOffsetExtensionsTests.cs` + `TestData`.

The zone-aware SameDay / NotSameDay overloads listed above land in
`FluentTimeZoneExtensions.cs` and its paired Tests/TestData. Preserve literal-other
signatures, nullable overloads and composition-time `ThrowHelper.ThrowIfNull(timeZone)`.

**Clone from**: `FluentDateTimeOffsetExtensions.cs` (`Past` pair), `FluentDateTimeExtensions.cs`
(`SameDay`, literal `other`), `FluentStringGuidExtensions.cs`.
Each extension passes the Must code as the third `MustBe` argument. Nullable overloads pass on null.
Every extension taking a `TimeZoneInfo timeZone` calls `ThrowHelper.ThrowIfNull(timeZone)` at
composition time (D-13, existing adapter pattern, `ValidationResultExtension.cs:47`) before building
the rule, so a null zone throws immediately rather than reaching the Must clause. Add one test per
such extension asserting `ThrowIfNull` throws on a null `timeZone`.

**Acceptance**: gate with `FluentValidation`. Budget 150k / 45 min.
Include DaylightSavingTime(TimeZoneInfo timeZone, string? message = null) and
NotDaylightSavingTime on DateTimeOffset/DateTimeOffset?, with composition-time
ThrowHelper.ThrowIfNull(timeZone), nullable pass-through and corresponding TimeZone codes.
Tests cover semantic Summer/Winter/fixed-zone vectors and null-zone composition failure.

### U6 — DataAnnotations attributes (∥ U5)

**Files**: add `src/PineGuard.DataAnnotations/TimeZoneAttributes.cs`,
`StringUtcOffsetAttributes.cs`; tests `TimeZoneAttributesTests.cs` + `TestData`,
`StringUtcOffsetAttributesTests.cs` + `TestData`.

**Clone from**: `DateTimeOffsetAttributes.cs` (`PastDateTimeOffsetAttribute`), `StringGuidAttributes.cs`.

| Attribute | Ctor | Target type | Validate |
|---|---|---|---|
| `UtcOffsetTimeSpanStringAttribute` | `()` | `string` | `Must.Be.UtcOffset((string)value, paramName: null)` |
| `UtcOffsetTimeSpanAttribute` | `()` | `TimeSpan` | `Must.Be.UtcOffset((TimeSpan)value, paramName: null)` |
| `ValidLocalTimeDateTimeAttribute` | `(string timeZoneId)` | `DateTime` | resolve zone (below) then `Must.Be.ValidLocalTime(v, zone, paramName: null)` |
| `UnambiguousLocalTimeDateTimeAttribute` | `(string timeZoneId)` | `DateTime` | resolve then `Must.Be.UnambiguousLocalTime` |
| `InTimeZoneDateTimeOffsetAttribute` | `(string timeZoneId)` | `DateTimeOffset` | resolve then `Must.Be.InTimeZone` |

The constructor calls `ThrowHelper.ThrowIfNull(timeZoneId)` (D-13, existing adapter pattern) before
storing it, so a null `timeZoneId` throws at construction time rather than at validation time. Zone
Resolution is implemented once in the existing annotation composition boundary, using target-
available BCL APIs and documented expected catches. The adapter calls
`TimeZoneInfo.TryFindSystemTimeZoneById` under `NET8_0_OR_GREATER`; on netstandard2.1
use `FindSystemTimeZoneById`, catching only `TimeZoneNotFoundException` and
`InvalidTimeZoneException` into a null outcome. The adapter then calls
`Must.Be.ResolvedTimeZone(resolvedZone, timeZoneId)` before its semantic Must clause.
Unknown ID returns that Must configuration failure (`timezone.id.unknown`) and exact
Must-owned message; neither Core nor Must performs lookup. Null validated properties
follow the existing Required convention and succeed before resolution. Verify resolver API
availability on each actual target, not a blanket NET8
assumption. The BCL may access host registry/files/ICU. `TimeZoneId` is a public
get-only property. Base-class code argument is the clause's failure code. None of these attributes
take a `TimeProvider`, so D-13's `ResolveTimeProvider(validationContext)` clause does not apply to
this unit (it governs the temporal attributes added in later phases).

**Tests**: zone attributes use `"UTC"` (valid path), `TestTimeZones.UnknownId` (unknown-id path),
and `TestTimeZones.PlatformSampleId` with `TestTimeZones.PlatformInvalidLocalTime` and
`TestTimeZones.PlatformAmbiguousLocalTime` for the failing paths (T-00 §6.3 exception). Add one test
per zone-id attribute constructor asserting `ThrowIfNull` throws on a null `timeZoneId`.
ValidLocalTime/UnambiguousLocalTime attribute tests also assert
`timezone.kind.not-unspecified` for Kind.Utc and Kind.Local, preserving the Must template.
Add DaylightSavingTimeDateTimeOffsetAttribute(timeZoneId), base code NotInEffect, and
NotDaylightSavingTimeDateTimeOffsetAttribute(timeZoneId), base code InEffect. Constructor
null guard, one ID resolution, ResolvedTimeZone then semantic Must mirror InTimeZone.
TimeZoneAttributesTests use TestTimeZones.PlatformSampleId with the named daylight/standard
instants; UTC rows prove positive false/negative true. No unconditioned regional ID literals.
Assert exact Must codes/messages.

**Acceptance**: gate with `DataAnnotations`; `audit must-codes` verifies each DA failure
is produced by its invoked Must path, including ResolvedTimeZone. Assert exact unknown-ID
code/message and member attribution. Budget 150k / 45 min.

## 4. Verification (phase)

T-00 §9 in full; Qodana; Sonar; Claude Fable 5.1 phase review; merge per T-00 §8.

## 5. Definition of Done

- `Must.Be.ValidLocalTime(TestTimeZones.InvalidLocalTime, TestTimeZones.DaylightSaving).Failed` with code `timezone.local-time.invalid`.
- `Must.Be.InTimeZone(new DateTimeOffset(2026,6,15,22,0,0,TimeSpan.Zero), TestTimeZones.PlusTen).Failed` with code `timezone.offset.mismatch`.
- No Core/Must/Guard/Fluent host-ID validation family or broad spec §5.3 environment exception was introduced. [A-05]
- Every gap/overlap layer rejects Local/Utc; nullable complement paths reject invalid configuration before negation. [A-10]
- `git grep TryConvertIanaIdToWindowsId src` returns nothing (D-5).
