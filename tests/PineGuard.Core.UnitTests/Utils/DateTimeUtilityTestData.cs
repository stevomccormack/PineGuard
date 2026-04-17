using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class DateTimeUtilityTestData
{
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
            new("Millisecond", (new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc), DateTimePrecision.Millisecond), true, new DateTime(2020, 2, 3, 4, 5, 6, 7, DateTimeKind.Utc))
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
            new("Year", (new DateTimeOffset(2020, 2, 3, 4, 5, 6, 7, TimeSpan.Zero), DateTimePrecision.Year), true, new DateTimeOffset(2020, 1, 1, 0, 0, 0, 0, TimeSpan.Zero))
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
