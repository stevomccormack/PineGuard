using PineGuard.Rules;

namespace PineGuard.Core.UnitTests.Utils;

public static class SqlDateTimeUtilityTestData
{
    public static TheoryData<DateTime?> SqlDateTimeValidCases =>
    [
        SqlDateTimeRules.MinValue,
        SqlDateTimeRules.MaxValue,
        SqlDateTimeRules.MinValue.AddMilliseconds(1),
        SqlDateTimeRules.MaxValue.AddMilliseconds(-1)
    ];

    public static TheoryData<DateTime?> SqlDateTimeInvalidCases =>
    [
        null,
        SqlDateTimeRules.MinValue.AddTicks(-1),
        SqlDateTimeRules.MaxValue.AddTicks(1)
    ];

    public static TheoryData<DateTimeOffset?> SqlDateTimeOffsetValidCases =>
    [
        new DateTimeOffset(SqlDateTimeRules.MinValue, TimeSpan.Zero),
        new DateTimeOffset(SqlDateTimeRules.MaxValue, TimeSpan.Zero)
    ];

    public static TheoryData<DateTimeOffset?> SqlDateTimeOffsetInvalidCases =>
    [
        null,
        new DateTimeOffset(SqlDateTimeRules.MinValue.AddTicks(-1), TimeSpan.Zero),
        new DateTimeOffset(SqlDateTimeRules.MaxValue.AddTicks(1), TimeSpan.Zero),

        // Non-zero offsets still compare as UTC instants; keep it deterministic.
        new DateTimeOffset(SqlDateTimeRules.MinValue, TimeSpan.FromHours(1))
    ];

    public static TheoryData<DateOnly?> SqlDateOnlyValidCases =>
    [
        DateOnly.FromDateTime(SqlDateTimeRules.MinValue),
        DateOnly.FromDateTime(SqlDateTimeRules.MaxValue)
    ];

    public static TheoryData<DateOnly?> SqlDateOnlyInvalidCases =>
    [
        null,
        new DateOnly(1752, 12, 31)
    ];
}
