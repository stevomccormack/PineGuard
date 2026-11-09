using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class DateTimeUtilityTestData
{
    // Fixed offset zones exactly at the ±14h boundary TryGetDateAtOffset enforces, paired with an instant
    // at the corresponding DateTimeOffset extreme, so the projection overflows/underflows on tick range,
    // not on the offset check.
    private static readonly TimeZoneInfo PlusFourteen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.PlusFourteen", TimeSpan.FromHours(14), "PineGuard Test +14", "PineGuard Test +14");
    private static readonly TimeZoneInfo MinusFourteen = TimeZoneInfo.CreateCustomTimeZone("PineGuard.Test.MinusFourteen", TimeSpan.FromHours(-14), "PineGuard Test -14", "PineGuard Test -14");

    public static class GetUtcNow
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Supplied clock", FixedTimeProvider.Default, FixedTimeProvider.Default.GetUtcNow())
        ];

        // A null Expected asks the test to assert against the system clock instead of a pinned instant.
        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null falls back to the system clock", null, null)
        ];

        public sealed record ValidCase(string Name, TimeProvider? Value, DateTimeOffset? Expected)
            : ReturnCase<TimeProvider?, DateTimeOffset?>(Name, Value, Expected);
    }

    public static class GetLocalNow
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Supplied clock", FixedTimeProvider.DefaultAheadOfUtc, FixedTimeProvider.DefaultAheadOfUtc.GetLocalNow())
        ];

        // A null Expected asks the test to assert against the system clock instead of a pinned instant.
        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null falls back to the system clock", null, null)
        ];

        public sealed record ValidCase(string Name, TimeProvider? Value, DateTimeOffset? Expected)
            : ReturnCase<TimeProvider?, DateTimeOffset?>(Name, Value, Expected);
    }

    public static class TryGetTodayDate
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("AheadOfUtc", FixedTimeProvider.DefaultAheadOfUtc, true, new DateTime(2026, 06, 16, 0, 0, 0, DateTimeKind.Unspecified)),
            new("BehindUtc", FixedTimeProvider.DefaultBehindUtc, true, new DateTime(2026, 06, 14, 0, 0, 0, DateTimeKind.Unspecified)),
            new("Default", FixedTimeProvider.Default, true, new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("ExtremeMinValueUnderflows", new FixedTimeProvider(DateTimeOffset.MinValue, MinusFourteen), false, default),
            new("ExtremeMaxValueOverflows", new FixedTimeProvider(DateTimeOffset.MaxValue, PlusFourteen), false, default)
        ];

        public sealed record ValidCase(string Name, TimeProvider Value, bool Expected, DateTime ExpectedOutValue)
            : TryCase<TimeProvider, DateTime>(Name, Value, Expected, ExpectedOutValue);
    }

    // Only one scenario is meaningful here: a null provider reads the system clock and yields the UTC calendar
    // date, never the host's LocalTimeZone (D-1). Asserted against the real machine clock in two CI processes pinned
    // to UTC+14 and UTC-12 (Pacific/Kiritimati, Etc/GMT+12), one of which always differs from the UTC date (§7).
    public static class TryGetTodayDateNullProvider
    {
        public static TheoryData<string> Cases => ["SystemClock"];
    }

    // Only one scenario is meaningful: proving exactly one GetUtcNow() read per call. Asserted directly
    // against CountingTimeProvider.UtcNowCallCount in the test method, not through AssertResult.
    public static class OneReadBehavior
    {
        public static TheoryData<string> Cases => ["SingleRead"];
    }

#if NET8_0_OR_GREATER
    public static class TryGetAgeDateOnly
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ExactBirthday", (new DateOnly(2008, 06, 15), new DateOnly(2026, 06, 15)), true, 18),
            new("BirthdayNotYetOccurredThisYear", (new DateOnly(2008, 06, 16), new DateOnly(2026, 06, 15)), true, 17),
            new("BirthdayAlreadyOccurredThisYear", (new DateOnly(2008, 06, 14), new DateOnly(2026, 06, 15)), true, 18),
            new("BirthMonthLaterInYear", (new DateOnly(2008, 12, 25), new DateOnly(2026, 06, 15)), true, 17),
            new("BirthMonthEarlierInYear", (new DateOnly(2008, 03, 10), new DateOnly(2026, 06, 15)), true, 18),
            new("LeapDayBirthdayNotYetOccurredInNonLeapYear", (new DateOnly(2008, 02, 29), new DateOnly(2026, 02, 28)), true, 17),
            new("LeapDayBirthdayMaturesOnFirstOfMarchInNonLeapYear", (new DateOnly(2008, 02, 29), new DateOnly(2026, 03, 01)), true, 18)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("NullBirthDate", (null, new DateOnly(2026, 06, 15)), false, 0),
            new("BirthDateInTheFuture", (new DateOnly(2026, 06, 16), new DateOnly(2026, 06, 15)), false, 0),
            new("BirthDateEqualsAsOf", (new DateOnly(2026, 06, 15), new DateOnly(2026, 06, 15)), true, 0)
        ];

        public sealed record ValidCase(string Name, (DateOnly? birthDate, DateOnly asOf) Value, bool Expected, int ExpectedOutValue)
            : TryCase<(DateOnly? birthDate, DateOnly asOf), int>(Name, Value, Expected, ExpectedOutValue);
    }
#endif

    public static class TryGetAgeDateTime
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ExactBirthday", (new DateTime(2008, 06, 15, 8, 0, 0, DateTimeKind.Utc), new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)), true, 18),
            new("BirthdayNotYetOccurredThisYear", (new DateTime(2008, 06, 16, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)), true, 17),
            new("BirthMonthLaterInYear", (new DateTime(2008, 12, 25, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)), true, 17),
            new("BirthMonthEarlierInYear", (new DateTime(2008, 03, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)), true, 18),
            new("LeapDayBirthdayNotYetOccurredInNonLeapYear", (new DateTime(2008, 02, 29, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 02, 28, 0, 0, 0, DateTimeKind.Unspecified)), true, 17),
            new("LeapDayBirthdayMaturesOnFirstOfMarchInNonLeapYear", (new DateTime(2008, 02, 29, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 03, 01, 0, 0, 0, DateTimeKind.Unspecified)), true, 18)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("NullBirthDate", (null, new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)), false, 0),
            new("BirthDateInTheFuture", (new DateTime(2026, 06, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 06, 15, 0, 0, 0, DateTimeKind.Unspecified)), false, 0)
        ];

        public sealed record ValidCase(string Name, (DateTime? birthDate, DateTime asOf) Value, bool Expected, int ExpectedOutValue)
            : TryCase<(DateTime? birthDate, DateTime asOf), int>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class ToUtc
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Utc input", new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)),
            new("Local input", new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()),
            new("Unspecified input", new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, null)
        ];

        public sealed record ValidCase(string Name, DateTime? Value, DateTime? Expected)
            : ReturnCase<DateTime?, DateTime?>(Name, Value, Expected);
    }

    public static class Diff
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Same UTC", (new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)), TimeSpan.Zero),
            new("Local vs UTC equivalent", (new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Local).ToUniversalTime()), TimeSpan.Zero),
            new("Unspecified vs UTC equivalent", (new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)), TimeSpan.Zero)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Start null", (null, DateTime.UtcNow), null),
            new("End null", (DateTime.UtcNow, null), null),
            new("Both null", (null, null), null)
        ];

        public sealed record ValidCase(string Name, (DateTime? Start, DateTime? End) Value, TimeSpan? Expected)
            : ReturnCase<(DateTime? Start, DateTime? End), TimeSpan?>(Name, Value, Expected);
    }

    public static class TryTruncateToPrecisionUtc
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Year", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Year), true, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
            new("Month", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Month), true, new DateTime(2020, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc)),
            new("Day", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Day), true, new DateTime(2020, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc)),
            new("Hour", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Hour), true, new DateTime(2020, 2, 3, 4, 0, 0, 0, DateTimeKind.Utc)),
            new("Minute", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Minute), true, new DateTime(2020, 2, 3, 4, 5, 0, 0, DateTimeKind.Utc)),
            new("Second", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Second), true, new DateTime(2020, 2, 3, 4, 5, 6, 0, DateTimeKind.Utc)),
            new("Millisecond", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Millisecond), true, new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc)),
            new("Microsecond", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc).AddTicks(18), DateTimePrecision.Microsecond), true, new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc).AddTicks(10)),
            new("Tick", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc).AddTicks(18), DateTimePrecision.Tick), true, new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc).AddTicks(18))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", (null, DateTimePrecision.Second), false, null),
            new("Unknown precision", (new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc), (DateTimePrecision)999), false, null)
        ];

        public sealed record ValidCase(string Name, (DateTime? Value, DateTimePrecision Precision) Value, bool Expected, DateTime? ExpectedOutValue)
            : TryCase<(DateTime? Value, DateTimePrecision Precision), DateTime?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryTruncateToPrecisionUtcOffset
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Year", (new DateTimeOffset(2020, 2, 3, 4, 5, 6, 7, TimeSpan.Zero), DateTimePrecision.Year), true, new DateTimeOffset(2020, 1, 1, 0, 0, 0, 0, TimeSpan.Zero)),
            new("Microsecond", (new DateTimeOffset(2020, 2, 3, 4, 5, 6, 7, TimeSpan.Zero).AddTicks(18), DateTimePrecision.Microsecond), true, new DateTimeOffset(2020, 2, 3, 4, 5, 6, 7, TimeSpan.Zero).AddTicks(10))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", (null, DateTimePrecision.Second), false, null),
            new("Unknown precision", (new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero), (DateTimePrecision)999), false, null)
        ];

        public sealed record ValidCase(string Name, (DateTimeOffset? Value, DateTimePrecision Precision) Value, bool Expected, DateTimeOffset? ExpectedOutValue)
            : TryCase<(DateTimeOffset? Value, DateTimePrecision Precision), DateTimeOffset?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryTruncateToPrecision
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Year", (new DateOnly(2020, 2, 3), DatePrecision.Year), true, new DateOnly(2020, 1, 1)),
            new("Month", (new DateOnly(2020, 2, 3), DatePrecision.Month), true, new DateOnly(2020, 2, 1)),
            new("Day", (new DateOnly(2020, 2, 3), DatePrecision.Day), true, new DateOnly(2020, 2, 3))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", (null, DatePrecision.Day), false, null),
            new("Unknown precision", (new DateOnly(2020, 2, 3), (DatePrecision)999), false, null)
        ];

        public sealed record ValidCase(string Name, (DateOnly? Value, DatePrecision Precision) Value, bool Expected, DateOnly? ExpectedOutValue)
            : TryCase<(DateOnly? Value, DatePrecision Precision), DateOnly?>(Name, Value, Expected, ExpectedOutValue);
    }
}
