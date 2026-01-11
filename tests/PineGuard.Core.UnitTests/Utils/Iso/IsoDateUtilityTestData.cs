using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoDateUtilityTestData
{
    public static class TryParseDateOnly
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("date", "2020-01-02", true, new DateOnly(2020, 1, 2)),
            new("trimmed", " 2020-01-02 ", true, new DateOnly(2020, 1, 2)),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, default),
            new("empty", string.Empty, false, default),
            new("space", " ", false, default),
            new("whitespace", "\t\r\n", false, default),
            new("bad format", "2020-1-2", false, default),
            new("month 13", "2020-13-01", false, default),
            new("not a date", "not-a-date", false, default),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, DateOnly ExpectedDateOnly)
            : TryCase<string, DateOnly>(Name, Value, ExpectedSuccess, ExpectedDateOnly);

        #endregion Cases
    }

    public static class TryParseDateTime
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("datetime", "2020-01-02T03:04:05", true, new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified)),
            new("trimmed", " 2020-01-02T03:04:05 ", true, new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified)),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, default),
            new("empty", string.Empty, false, default),
            new("space", " ", false, default),
            new("whitespace", "\t\r\n", false, default),
            new("date only", "2020-01-02", false, default),
            new("missing seconds", "2020-01-02T03:04", false, default),
            new("not a datetime", "not-a-datetime", false, default),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, DateTime ExpectedDateTime)
            : TryCase<string, DateTime>(Name, Value, ExpectedSuccess, ExpectedDateTime);

        #endregion Cases
    }

    public static class TryParseDateTimeOffset
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("offset", "2020-01-02T03:04:05+00:00", true, new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero)),
            new("trimmed", " 2020-01-02T03:04:05Z ", true, new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero)),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, default),
            new("empty", string.Empty, false, default),
            new("space", " ", false, default),
            new("whitespace", "\t\r\n", false, default),
            new("missing seconds", "2020-01-02T03:04", false, default),
            new("offset +25", "2020-01-02T03:04:05+25:00", false, default),
            new("bad offset", "2020-01-02T03:04:05+00", false, default),
            new("not a datetimeoffset", "not-a-datetimeoffset", false, default),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedSuccess, DateTimeOffset ExpectedDateTimeOffset)
            : TryCase<string, DateTimeOffset>(Name, Value, ExpectedSuccess, ExpectedDateTimeOffset);

        #endregion Cases
    }
}
