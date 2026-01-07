using PineGuard.Iso.Dates;
using PineGuard.Testing;
using System.Globalization;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public static class IsoDateTimeOffsetTestData
{
    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("Zulu", value: "2020-01-02T03:04:05Z", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero)),
            V("Fraction .1234567 Z", value: "2020-01-02T03:04:05.1234567Z", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero).AddTicks(1_234_567)),
            V("Offset +01:00", value: "2020-01-02T03:04:05+01:00", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(1))),
            V("Fraction .5 -08:00", value: "2020-01-02T03:04:05.5-08:00", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(-8)).AddTicks(5_000_000)),
            V("Missing timezone", value: "2020-01-02T03:04:05", expectedResult: ParseExpected("2020-01-02T03:04:05")),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            E("Null", value: null),
            E("Empty", value: string.Empty),
            E("Space", value: " "),
            E("Tab", value: "\t"),
            E("Date only", value: "2020-01-02"),
            E("Missing seconds", value: "2020-01-02T03:04"),
            E("Too many fractional digits", value: "2020-01-02T03:04:05.12345678Z"),
            E("Invalid offset format", value: "2020-01-02T03:04:05+01:0"),
            E("Invalid offset values", value: "2020-01-02T03:04:05+99:99"),
            E("Non-date", value: "abcd"),
        };

        private static ValidCase V(string name, string value, DateTimeOffset expectedResult) => new(name, value, Expected: true, expectedResult);

        private static ValidCase E(string name, string? value) => new(name, value, Expected: false, ExpectedResult: default);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, DateTimeOffset ExpectedResult) : Case(Name);

        #endregion

        private static DateTimeOffset ParseExpected(string value)
            => DateTimeOffset.ParseExact(
                value,
                IsoDateTimeOffset.AllFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind);
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("Zulu", value: "2020-01-02T03:04:05Z", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero)),
            V("Fraction .1234567 Z", value: "2020-01-02T03:04:05.1234567Z", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero).AddTicks(1_234_567)),
            V("Offset +01:00", value: "2020-01-02T03:04:05+01:00", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(1))),
            V("Fraction .5 -08:00", value: "2020-01-02T03:04:05.5-08:00", expectedResult: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(-8)).AddTicks(5_000_000)),
            V("Missing timezone", value: "2020-01-02T03:04:05", expectedResult: ParseExpected("2020-01-02T03:04:05")),
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("Null", value: null, expectedExceptionType: typeof(ArgumentNullException), expectedParamName: "input"),
            I("Empty", value: string.Empty),
            I("Space", value: " "),
            I("Tab", value: "\t"),
            I("Date only", value: "2020-01-02"),
            I("Missing seconds", value: "2020-01-02T03:04"),
            I("Too many fractional digits", value: "2020-01-02T03:04:05.12345678Z"),
            I("Invalid offset format", value: "2020-01-02T03:04:05+01:0"),
            I("Invalid offset values", value: "2020-01-02T03:04:05+99:99"),
            I("Non-date", value: "abcd"),
        };

        private static ValidCase V(string name, string value, DateTimeOffset expectedResult) => new(name, value, expectedResult);

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
                    ParamName: expectedParamName));

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Value, DateTimeOffset ExpectedResult) : Case(Name);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException) : Case(Name);

        #endregion

        private static DateTimeOffset ParseExpected(string value)
            => DateTimeOffset.ParseExact(
                value,
                IsoDateTimeOffset.AllFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind);
    }

    public static class ToIsoString
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("Zulu", value: new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero), expected: "2020-01-02T03:04:05.0000000+00:00"),
            V("Leap day", value: new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero), expected: "2000-02-29T00:00:00.0000000+00:00"),
        };

        private static ValidCase V(string name, DateTimeOffset value, string expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, DateTimeOffset Value, string Expected) : Case(Name);

        #endregion
    }
}
