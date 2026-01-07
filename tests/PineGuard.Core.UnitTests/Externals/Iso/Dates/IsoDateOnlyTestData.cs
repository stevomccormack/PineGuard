using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public static class IsoDateOnlyTestData
{
    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("2020-01-02", value: "2020-01-02", expectedResult: new DateOnly(2020, 01, 02)),
            V("1999-12-31", value: "1999-12-31", expectedResult: new DateOnly(1999, 12, 31)),
            V("2000-02-29", value: "2000-02-29", expectedResult: new DateOnly(2000, 02, 29)),
            V("2024-02-29", value: "2024-02-29", expectedResult: new DateOnly(2024, 02, 29)),
            V("Min", value: "0001-01-01", expectedResult: DateOnly.MinValue),
            V("Max", value: "9999-12-31", expectedResult: DateOnly.MaxValue),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            E("Null", value: null),
            E("Empty", value: string.Empty),
            E("Space", value: " "),
            E("Tab", value: "\t"),
            E("Newline", value: "\r\n"),
            E("Slash separators", value: "2020/01/02"),
            E("Non-padded", value: "2020-1-2"),
            E("Short year", value: "20-01-02"),
            E("Month 13", value: "2020-13-01"),
            E("Month 0", value: "2020-00-01"),
            E("Day 0", value: "2020-01-00"),
            E("Day 32", value: "2020-01-32"),
            E("Non-leap Feb 29", value: "2021-02-29"),
            E("Non-date", value: "abcd-ef-gh"),
            E("DateTime", value: "2020-01-02T00:00:00"),
            E("Date with Z", value: "2020-01-02Z"),
        };

        private static ValidCase V(string name, string value, DateOnly expectedResult) => new(name, value, Expected: true, expectedResult);

        private static ValidCase E(string name, string? value) => new(name, value, Expected: false, ExpectedResult: default);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, DateOnly ExpectedResult) : Case(Name);

        #endregion
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("2020-01-02", value: "2020-01-02", expectedResult: new DateOnly(2020, 01, 02)),
            V("1999-12-31", value: "1999-12-31", expectedResult: new DateOnly(1999, 12, 31)),
            V("2000-02-29", value: "2000-02-29", expectedResult: new DateOnly(2000, 02, 29)),
            V("2024-02-29", value: "2024-02-29", expectedResult: new DateOnly(2024, 02, 29)),
            V("Min", value: "0001-01-01", expectedResult: DateOnly.MinValue),
            V("Max", value: "9999-12-31", expectedResult: DateOnly.MaxValue),
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("Null", value: null, expectedExceptionType: typeof(ArgumentNullException), expectedParamName: "s"),
            I("Empty", value: string.Empty),
            I("Space", value: " "),
            I("Tab", value: "\t"),
            I("Newline", value: "\r\n"),
            I("Slash separators", value: "2020/01/02"),
            I("Non-padded", value: "2020-1-2"),
            I("Short year", value: "20-01-02"),
            I("Month 13", value: "2020-13-01"),
            I("Month 0", value: "2020-00-01"),
            I("Day 0", value: "2020-01-00"),
            I("Day 32", value: "2020-01-32"),
            I("Non-leap Feb 29", value: "2021-02-29"),
            I("Non-date", value: "abcd-ef-gh"),
            I("DateTime", value: "2020-01-02T00:00:00"),
            I("Date with Z", value: "2020-01-02Z"),
        };

        private static ValidCase V(string name, string value, DateOnly expectedResult) => new(name, value, expectedResult);

        private static InvalidCase I(
            string name,
            string? value,
            Type? expectedExceptionType = null,
            string? expectedParamName = null)
            => new(
                name,
                value,
                ExpectedException: new ExpectedException(
                    expectedExceptionType ?? typeof(FormatException),
                    ParamName: expectedParamName,
                    MessageContains: expectedExceptionType is null ? "date" : null));

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Value, DateOnly ExpectedResult) : Case(Name);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException) : Case(Name);

        #endregion
    }

    public static class ToIsoString
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("Min", value: DateOnly.MinValue, expected: "0001-01-01"),
            V("Max", value: DateOnly.MaxValue, expected: "9999-12-31"),
            V("2020-01-02", value: new DateOnly(2020, 01, 02), expected: "2020-01-02"),
            V("2024-02-29", value: new DateOnly(2024, 02, 29), expected: "2024-02-29"),
        };

        private static ValidCase V(string name, DateOnly value, string expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, DateOnly Value, string Expected) : Case(Name);

        #endregion
    }
}
