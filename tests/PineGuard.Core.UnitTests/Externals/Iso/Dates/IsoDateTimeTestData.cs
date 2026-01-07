using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public static class IsoDateTimeTestData
{
    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("2020-01-02T03:04:05", value: "2020-01-02T03:04:05", expectedResult: new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified)),
            V("1999-12-31T23:59:59", value: "1999-12-31T23:59:59", expectedResult: new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified)),
            V("2000-02-29T00:00:00", value: "2000-02-29T00:00:00", expectedResult: new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Unspecified)),
            V("Fraction .1", value: "2020-01-02T03:04:05.1", expectedResult: new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_000_000)),
            V("Fraction .1234567", value: "2020-01-02T03:04:05.1234567", expectedResult: new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_234_567)),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            E("Null", value: null),
            E("Empty", value: string.Empty),
            E("Space", value: " "),
            E("Tab", value: "\t"),
            E("Date only", value: "2020-01-02"),
            E("Space separator", value: "2020-01-02 03:04:05"),
            E("Missing seconds", value: "2020-01-02T03:04"),
            E("Too many fractional digits", value: "2020-01-02T03:04:05.12345678"),
            E("Non-date", value: "abcd-ef-ghTij:kl:mn"),
            E("UTC designator", value: "2020-01-02T03:04:05Z"),
            E("Offset", value: "2020-01-02T03:04:05+01:00"),
        };

        private static ValidCase V(string name, string value, DateTime expectedResult) => new(name, value, Expected: true, expectedResult);

        private static ValidCase E(string name, string? value) => new(name, value, Expected: false, ExpectedResult: default);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, DateTime ExpectedResult) : Case(Name);

        #endregion
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("2020-01-02T03:04:05", value: "2020-01-02T03:04:05", expectedResult: new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified)),
            V("1999-12-31T23:59:59", value: "1999-12-31T23:59:59", expectedResult: new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified)),
            V("2000-02-29T00:00:00", value: "2000-02-29T00:00:00", expectedResult: new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Unspecified)),
            V("Fraction .1", value: "2020-01-02T03:04:05.1", expectedResult: new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_000_000)),
            V("Fraction .1234567", value: "2020-01-02T03:04:05.1234567", expectedResult: new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_234_567)),
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("Null", value: null, expectedExceptionType: typeof(ArgumentNullException), expectedParamName: "s"),
            I("Empty", value: string.Empty),
            I("Space", value: " "),
            I("Tab", value: "\t"),
            I("Date only", value: "2020-01-02"),
            I("Space separator", value: "2020-01-02 03:04:05"),
            I("Missing seconds", value: "2020-01-02T03:04"),
            I("Too many fractional digits", value: "2020-01-02T03:04:05.12345678"),
            I("Non-date", value: "abcd-ef-ghTij:kl:mn"),
            I("UTC designator", value: "2020-01-02T03:04:05Z"),
            I("Offset", value: "2020-01-02T03:04:05+01:00"),
        };

        private static ValidCase V(string name, string value, DateTime expectedResult) => new(name, value, expectedResult);

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

        public sealed record ValidCase(string Name, string Value, DateTime ExpectedResult) : Case(Name);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException) : Case(Name);

        #endregion
    }

    public static class ToIsoString
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("2020-01-02", value: new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified), expected: "2020-01-02T03:04:05.0000000"),
            V("2000-02-29", value: new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Unspecified), expected: "2000-02-29T00:00:00.0000000"),
        };

        private static ValidCase V(string name, DateTime value, string expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, DateTime Value, string Expected) : Case(Name);

        #endregion
    }
}
