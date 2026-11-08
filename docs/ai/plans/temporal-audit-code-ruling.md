# Fable corrective ruling — MustCodes audit ownership

- Reviewer/model: Claude Fable 5.1 (claude-fable-5-1, provider firstParty)
- Claude Code CLI: 2.1.283
- Persistent session: 7342dc25-a35f-42a2-a77e-3ea894c6b4eb
- Result: success; exit 0; 32 turns; duration 234465 ms; cost USD 3.888455
- Usage: input 162, cache read 380100, cache creation 137773, output 20727; web searches 0
- Scope: read-only focused correction of the final review's MustCodes domain/audit finding. The public ruling below is the only model text retained. Intermediate tool payloads and private reasoning are excluded.

# Corrective ruling: MustCodes audit design details (binding)

**Verdict: ACCEPTED as corrected below.** Every cross-domain usage is fixed at source by giving the TimeZone and Date domains their own constants. The audit rule is untouched, no exemption is added, and all A-01 through A-12 decisions stand except the two sentences named in item 6.

Governing evidence for the whole ruling:

- `apps/cli/src/audit/rules/must-codes.ts:392-407` check (f) fails any clause file that references a constant outside its own `// Serves:` domain. `:322-332` derives that domain from the Serves comment.
- `docs/ai/specs/must-clauses/project.md:269-270` fixes the domain by the clause class. `:285-291` requires one code per Fail/FromBool call, chosen per failure path.
- `src/PineGuard.Core/Codes/MustCodes.cs:8-12` states the same grammar.
- Precedent for a dependency-null failure inside a domain: `src/PineGuard.MustClauses/MustPredicateClauses.cs:49-50` with `src/PineGuard.Core/Codes/MustCodes.Predicate.cs:25-33`.
- Precedent for a configuration guard that reuses the clause's own code with `nameof(param)`: `src/PineGuard.MustClauses/MustTimeOnlyClauses.cs:45-46, 311-312`.

## Item 1. Null `timeZoneId` on `Must.Be.ResolvedTimeZone`

| Field | Decision |
|---|---|
| Constant | `MustCodes.TimeZone.Id.Null` |
| String | `"timezone.id.null"` |
| Message | `"{paramName} must not be null."` |
| ParamName | `nameof(timeZoneId)` |
| Failing value | `timeZoneId` (null) |
| Precedence | Checked first, before inspecting `resolvedZone`. |
| Distinct from | `MustCodes.TimeZone.Id.Unknown` (`"timezone.id.unknown"`), emitted only for non-null ID plus null outcome, template `"Time zone ID '{paramName}' is not a known time zone."`, same `nameof(timeZoneId)` attribution, failing value the supplied ID. |
| Clause file | `src/PineGuard.MustClauses/MustTimeZoneClauses.cs` |
| Tests | `tests/PineGuard.MustClauses.UnitTests/MustTimeZoneClausesTests.cs` + `MustTimeZoneClausesTestData.cs`, group `ResolvedTimeZone`: rows for null ID, unknown ID, success, and null-ID-beats-null-outcome precedence. |

T-00 §4.2 (`temporal-00-program.md:237-239`) already names this constant. T-02 U3 (`temporal-02-timezone-core.md:175-177, 206`) still says `Value.State.Null` and is the text that must change. `MustCodes.Value.cs` is not edited.

## Item 2. Non-Unspecified `DateTime.Kind` on local-time checks

| Field | Decision |
|---|---|
| Constant | `MustCodes.TimeZone.Kind.NotUnspecified` |
| String | `"timezone.kind.not-unspecified"` |
| Message | `"{paramName} must have DateTimeKind.Unspecified."` (A-12 template verbatim) |
| ParamName | `paramName` (the value, per A-10/A-12) |
| Failing value | `value` |
| Emitted by | `ValidLocalTime` and `UnambiguousLocalTime` in `MustTimeZoneClauses.cs`, after the `Zone.Null` guard and before the rule call. Both positive clauses share the guard, so no `Not*` complement is affected. |
| Not declared | No positive `TimeZone.Kind.Unspecified`. Nothing would emit it and check (b) at `must-codes.ts:667-678` would flag it. |
| Tests | `MustTimeZoneClausesTests` groups `ValidLocalTime` and `UnambiguousLocalTime`: one row each with `Kind.Utc` and `Kind.Local` asserting this code and message. `TimeZoneAttributesTests`: one row per Kind-sensitive attribute asserting the same code flows through. |

`MustCodes.Date.Kind.NotUnspecified` stays declared and stays referenced by `MustDateTimeClauses.cs:996, 1074`, so no orphan is created.

## Item 3. Zone-aware `SameDay` / `NotSameDay`

Names stay `SameDay` and `NotSameDay`, matching `MustDateTimeClauses.cs:830, 858`. Family is `MustTimeZoneClauses`, because the rule invoked is `TimeZoneRules.IsSameDay` and the clause class fixes the domain.

| Field | Positive clause | Complement |
|---|---|---|
| Signature | `MustResult<DateTimeOffset> SameDay(this IMustClause _, DateTimeOffset value, DateTimeOffset other, TimeZoneInfo timeZone, [CallerArgumentExpression(nameof(value))] string? paramName = null)` | `NotSameDay(...)` same shape |
| Zone guard | `Fail(MustCodes.TimeZone.Zone.Null, "{paramName} must not be null.", nameof(timeZone), value)` | identical |
| Rule | `TimeZoneRules.IsSameDay(value, other, timeZone)` | `!TimeZoneRules.IsSameDay(...)` |
| Constant | `MustCodes.TimeZone.Equality.NotSameDay` | `MustCodes.TimeZone.Equality.SameDay` |
| String | `"timezone.equality.not-same-day"` | `"timezone.equality.same-day"` |
| Message | `"{paramName} must be the same day in the time zone."` | `"{paramName} must not be the same day in the time zone."` |
| Failing value / ParamName | `value` / `paramName` | same |

Layer placement, replacing the "edit the DateTimeOffset files" paragraphs:

- Guard: `GuardTimeZoneClauses.NotSameDay` calls `Must.Be.SameDay(value, other, timeZone)`; `GuardTimeZoneClauses.SameDay` calls `Must.Be.NotSameDay(...)`. Tests in `GuardTimeZoneClausesTests` + `TestData`.
- Fluent: `FluentTimeZoneExtensions.SameDay(DateTimeOffset other, TimeZoneInfo timeZone)` and `NotSameDay`, nullable overloads, `ThrowHelper.ThrowIfNull(timeZone)` at composition. Tests in `FluentTimeZoneExtensionsTests` + `TestData`.
- DataAnnotations: none (cross-property, unchanged).
- Must tests: `MustTimeZoneClausesTests` groups `SameDay` and `NotSameDay` using `TimeZoneRulesFixtures.IsSameDay`, plus the `timeZone = null!` row per T-02 U3.

Plain `Must.Be.SameDay(DateTimeOffset value, DateTimeOffset other)` with no zone (T-00 §4.4, `temporal-00-program.md:280`) stays in `MustDateTimeOffsetClauses` emitting `Date.Equality.NotSameDay` / `SameDay`. Overload resolution is by arity, so both families coexist in namespace `PineGuard.MustClauses`.

## Item 4. `OnInterval` / `NotOnInterval` across three types

| Type | Clause file | Node | Fail code (OnInterval) | Fail code (NotOnInterval) |
|---|---|---|---|---|
| `TimeOnly` | `MustTimeOnlyClauses.cs` | `MustCodes.Time.Interval` (unchanged) | `time.interval.off-interval` | `time.interval.on-interval` |
| `DateTime` | `MustDateTimeClauses.cs` | `MustCodes.Date.Interval` (new) | `date.interval.off-interval` | `date.interval.on-interval` |
| `DateTimeOffset` | `MustDateTimeOffsetClauses.cs` | `MustCodes.Date.Interval` (new) | `date.interval.off-interval` | `date.interval.on-interval` |

Shape for all six clauses, mirroring the `Within` guard at `MustTimeOnlyClauses.cs:311-317`:

```csharp
public static MustResult<DateTime> OnInterval(this IMustClause _,
    DateTime value,
    TimeSpan interval,
    [CallerArgumentExpression(nameof(value))] string? paramName = null)
{
    if (interval <= TimeSpan.Zero)
        return MustResult<DateTime>.Fail(MustCodes.Date.Interval.OffInterval, "{paramName} requires a positive interval.", nameof(interval), interval);

    const string messageTemplate = "{paramName} must fall on the expected interval.";

    var ok = DateTimeRules.IsOnInterval(value, interval);
    return MustResult<DateTime>.FromBool(ok, MustCodes.Date.Interval.OffInterval, messageTemplate, paramName, value, value);
}
```

`NotOnInterval` uses the same positive-interval guard with the `OnInterval` constant and template `"{paramName} must not fall on the expected interval."`. This satisfies A-09's "common domain before negation". `TimeOnlyRules.IsOnInterval` operates on `value.Value.Ticks` and TimeOnly's wrap semantics for `Between` are untouched.

DataAnnotations base codes: `OnIntervalTimeOnlyAttribute` declares `MustCodes.Time.Interval.OffInterval`; `OnIntervalDateTimeAttribute` and `OnIntervalDateTimeOffsetAttribute` declare `MustCodes.Date.Interval.OffInterval`. Check (d) keys on method name across files (`must-codes.ts:425-430`), so each declared code is found. Tests: `MustDateTimeClausesTests`, `MustDateTimeOffsetClausesTests`, `MustTimeOnlyClausesTests` and their TestData, plus the three DA test files.

## Item 5. Exact `// Serves:` lines and catalogue files

`src/PineGuard.Core/Codes/MustCodes.TimeZone.cs` (T-02 U1 creates; single writer). Final content, in the shape of `MustCodes.Time.cs`:

```csharp
namespace PineGuard.Codes;

// Serves: MustTimeZoneClauses.cs, MustStringUtcOffsetClauses.cs
public static partial class MustCodes
{
    /// <summary>The <c>timezone</c> domain: supplied-zone configuration, UTC offsets, local-time gaps and overlaps, and same-day-in-zone equality.</summary>
    public static class TimeZone
    {
        /// <summary>The code prefix for this node (<c>"timezone"</c>).</summary>
        public const string Prefix = "timezone";

        /// <summary>The time-zone identifier supplied to an adapter, as an already-resolved outcome.</summary>
        public static class Id
        {
            /// <summary>The code prefix for this node (<c>"timezone.id"</c>).</summary>
            public const string Prefix = TimeZone.Prefix + ".id";

            /// <summary><c>timezone.id.null</c></summary>
            public const string Null = Prefix + ".null";

            /// <summary><c>timezone.id.unknown</c></summary>
            public const string Unknown = Prefix + ".unknown";
        }

        /// <summary>The <see cref="TimeZoneInfo"/> configuration supplied to a zone-taking clause.</summary>
        public static class Zone
        {
            /// <summary>The code prefix for this node (<c>"timezone.zone"</c>).</summary>
            public const string Prefix = TimeZone.Prefix + ".zone";

            /// <summary><c>timezone.zone.null</c></summary>
            public const string Null = Prefix + ".null";
        }

        /// <summary>The <see cref="DateTimeKind"/> a local wall-clock value must carry before a zone check.</summary>
        public static class Kind
        {
            /// <summary>The code prefix for this node (<c>"timezone.kind"</c>).</summary>
            public const string Prefix = TimeZone.Prefix + ".kind";

            /// <summary><c>timezone.kind.not-unspecified</c></summary>
            public const string NotUnspecified = Prefix + ".not-unspecified";
        }

        /// <summary>UTC offset legality and offset agreement with a zone at an instant.</summary>
        public static class Offset
        {
            /// <summary>The code prefix for this node (<c>"timezone.offset"</c>).</summary>
            public const string Prefix = TimeZone.Prefix + ".offset";

            /// <summary><c>timezone.offset.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>timezone.offset.mismatch</c></summary>
            public const string Mismatch = Prefix + ".mismatch";
        }

        /// <summary>Existence and uniqueness of a local wall-clock time in a zone.</summary>
        public static class LocalTime
        {
            /// <summary>The code prefix for this node (<c>"timezone.local-time"</c>).</summary>
            public const string Prefix = TimeZone.Prefix + ".local-time";

            /// <summary><c>timezone.local-time.invalid</c></summary>
            public const string Invalid = Prefix + ".invalid";

            /// <summary><c>timezone.local-time.ambiguous</c></summary>
            public const string Ambiguous = Prefix + ".ambiguous";
        }

        /// <summary>Calendar-day equality of two instants evaluated in one zone.</summary>
        public static class Equality
        {
            /// <summary>The code prefix for this node (<c>"timezone.equality"</c>).</summary>
            public const string Prefix = TimeZone.Prefix + ".equality";

            /// <summary><c>timezone.equality.not-same-day</c></summary>
            public const string NotSameDay = Prefix + ".not-same-day";

            /// <summary><c>timezone.equality.same-day</c></summary>
            public const string SameDay = Prefix + ".same-day";
        }
    }
}
```

`MustCodes.Date.cs` line 5 becomes, written by T-06 U1 (the two new names may precede their files; the audit only maps files that exist, `must-codes.ts:359-371`):

```csharp
// Serves: MustDateTimeClauses.cs, MustDateOnlyClauses.cs, MustDateTimeOffsetClauses.cs, MustSqlDateTimeClauses.cs, MustStringDateOnlyClauses.cs, MustStringDateTimeOffsetClauses.cs, MustStringDateTimeClauses.cs, MustUnixTimeClauses.cs
```

T-05 U1 adds to `MustCodes.Date.cs`, beside `Calendar`:

```csharp
        /// <summary>Alignment of the time-of-day component to a fixed interval.</summary>
        public static class Interval
        {
            /// <summary>The code prefix for this node (<c>"date.interval"</c>).</summary>
            public const string Prefix = Date.Prefix + ".interval";

            /// <summary><c>date.interval.off-interval</c></summary>
            public const string OffInterval = Prefix + ".off-interval";

            /// <summary><c>date.interval.on-interval</c></summary>
            public const string OnInterval = Prefix + ".on-interval";
        }
```

Unchanged: `MustCodes.Time.cs` Serves line, `MustCodes.Value.cs`, `MustCodes.Predicate.cs`. No clause file moves domain.

## Item 6. What this supersedes, and its classification

This is a **corrective application of the already-authorized audit contract**, not a Brain deviation. Check (f) has been the rule since the audit rebuild, the Must-clauses spec fixes domain by clause class, and the plans require `audit must-codes` to pass unchanged (`temporal-00-program.md:345-346, 519-520`).

Narrowly superseded, and only these:

1. T-02 U3 null-ID code `Value.State.Null` (`temporal-02-timezone-core.md:175-177, 206`). Replaced by `TimeZone.Id.Null`. Brings T-02 in line with T-00 §4.2 lines 237-241, which already said so.
2. T-02 U1 "Reuse existing `MustCodes.Date.Kind.NotUnspecified`" (`temporal-02-timezone-core.md:48-49`), T-00 §5 single-writer row `Date.Kind` (`temporal-00-program.md:353`), and the one A-08 sentence "T-02 reuses existing Date.Kind.NotUnspecified" (`temporal-astra-decisions.md:192`). T-04's reuse of `Date.Kind.NotUtc` inside `MustDateTimeClauses` stands. A-12's templates and value attribution stand verbatim.
3. T-00 §4.2 SameDay row code column (`temporal-00-program.md:252`), T-00 §5 row `Date.Equality` "SameDay is not placed in timezone clause family" (`:364`), and the T-02 U3/U4/U5 "edit the DateTimeOffset file" paragraphs (`temporal-02-timezone-core.md:190-199, 250-251, 283-285`). The earlier "[Fable corrective council]" placement was itself the audit violation and is withdrawn.
4. T-00 §4.5 OnInterval code column for the DateTime and DateTimeOffset forms (`temporal-00-program.md:296`), T-00 §5 row `Time.Interval` scope (`:360`), and T-05 U1 code list (`temporal-05-durations-calendar.md:48-56`).
5. T-06 U1 Serves omission (`temporal-06-formats-epoch.md:53-56`).

All other D-rows and A-01 through A-12 remain final.

## Plan sections that must change

- `temporal-00-program.md` §4.2 SameDay row; §4.5 OnInterval row; §5 TimeZone code block (add `Id.Null`, `Kind`, `Equality`) and additions table (add `Date.Interval` row); §5 single-writer rows `Date.Kind`, `Date.Equality`, `Time.Interval`, plus a new `Date.Interval` row and `TimeZone.Kind` / `TimeZone.Equality` rows.
- `temporal-02-timezone-core.md` U1 second paragraph; U1 Serves list is unchanged; U3 signature paragraph, member table rows for `ResolvedTimeZone`, `ValidLocalTime`, `UnambiguousLocalTime`, `SameDay`, `NotSameDay`; U3, U4 and U5 "edit DateTimeOffset files" paragraphs deleted; U6 test note for the Kind code.
- `temporal-05-durations-calendar.md` U1 codes; U3b file list note that `MustDateTimeClauses.cs` and `MustDateTimeOffsetClauses.cs` use `Date.Interval`; U5d attribute base codes.
- `temporal-06-formats-epoch.md` U1: add the two Serves entries to `MustCodes.Date.cs`.
- `temporal-astra-decisions.md` A-08: one-line addendum recording that T-02 uses `TimeZone.Kind.NotUnspecified` under the A-12 template, per this ruling.