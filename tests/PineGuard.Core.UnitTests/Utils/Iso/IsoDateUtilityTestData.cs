using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoDateUtilityTestData
{
    public static class TryParseDateOnly
    {
        private static Case V(string name, string? input, bool expectedSuccess, DateOnly expectedDateOnly) => new(name, input, expectedSuccess, expectedDateOnly);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("date", "2020-01-02", true, new DateOnly(2020, 1, 2)) },
            { V("trimmed", " 2020-01-02 ", true, new DateOnly(2020, 1, 2)) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, default) },
            { V("empty", string.Empty, false, default) },
            { V("space", " ", false, default) },
            { V("whitespace", "\t\r\n", false, default) },
            { V("bad format", "2020-1-2", false, default) },
            { V("month 13", "2020-13-01", false, default) },
            { V("not a date", "not-a-date", false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, DateOnly ExpectedDateOnly)
            : TryCase<string, DateOnly>(Name, Input, ExpectedSuccess, ExpectedDateOnly);

        #endregion Cases
    }

    public static class TryParseDateTime
    {
        private static Case V(string name, string? input, bool expectedSuccess, DateTime expectedDateTime) => new(name, input, expectedSuccess, expectedDateTime);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("datetime", "2020-01-02T03:04:05", true, new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified)) },
            { V("trimmed", " 2020-01-02T03:04:05 ", true, new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Unspecified)) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, default) },
            { V("empty", string.Empty, false, default) },
            { V("space", " ", false, default) },
            { V("whitespace", "\t\r\n", false, default) },
            { V("date only", "2020-01-02", false, default) },
            { V("missing seconds", "2020-01-02T03:04", false, default) },
            { V("not a datetime", "not-a-datetime", false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, DateTime ExpectedDateTime)
            : TryCase<string, DateTime>(Name, Input, ExpectedSuccess, ExpectedDateTime);

        #endregion Cases
    }

    public static class TryParseDateTimeOffset
    {
        private static Case V(string name, string? input, bool expectedSuccess, DateTimeOffset expectedDateTimeOffset) => new(name, input, expectedSuccess, expectedDateTimeOffset);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("offset", "2020-01-02T03:04:05+00:00", true, new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero)) },
            { V("trimmed", " 2020-01-02T03:04:05Z ", true, new DateTimeOffset(2020, 1, 2, 3, 4, 5, TimeSpan.Zero)) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, default) },
            { V("empty", string.Empty, false, default) },
            { V("space", " ", false, default) },
            { V("whitespace", "\t\r\n", false, default) },
            { V("missing seconds", "2020-01-02T03:04", false, default) },
            { V("offset +25", "2020-01-02T03:04:05+25:00", false, default) },
            { V("bad offset", "2020-01-02T03:04:05+00", false, default) },
            { V("not a datetimeoffset", "not-a-datetimeoffset", false, default) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? Input, bool ExpectedSuccess, DateTimeOffset ExpectedDateTimeOffset)
            : TryCase<string, DateTimeOffset>(Name, Input, ExpectedSuccess, ExpectedDateTimeOffset);

        #endregion Cases
    }
}
