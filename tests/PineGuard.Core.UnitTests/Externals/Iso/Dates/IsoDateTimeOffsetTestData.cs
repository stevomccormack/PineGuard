using PineGuard.Externals.Iso.Dates;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;
using System.Globalization;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public static class IsoDateTimeOffsetTestData
{
    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Zulu", "2020-01-02T03:04:05Z", true, new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero)),
            new("Fraction .1234567 Z", "2020-01-02T03:04:05.1234567Z", true,
                new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero).AddTicks(1_234_567)),
            new("Offset +01:00", "2020-01-02T03:04:05+01:00", true,
                new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(1))),
            new("Fraction .5 -08:00", "2020-01-02T03:04:05.5-08:00", true,
                new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(-8)).AddTicks(5_000_000)),
            new("Missing timezone", "2020-01-02T03:04:05", true, ParseExpected("2020-01-02T03:04:05"))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, default),
            new("Empty", string.Empty, false, default),
            new("Space", " ", false, default),
            new("Tab", "\t", false, default),
            new("Date only", "2020-01-02", false, default),
            new("Missing seconds", "2020-01-02T03:04", false, default),
            new("Too many fractional digits", "2020-01-02T03:04:05.12345678Z", false, default),
            new("Invalid offset format", "2020-01-02T03:04:05+01:0", false, default),
            new("Invalid offset values", "2020-01-02T03:04:05+99:99", false, default),
            new("Non-date", "abcd", false, default)
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, DateTimeOffset ExpectedOutValue)
            : TryCase<string?, DateTimeOffset>(Name, Value, ExpectedReturn, ExpectedOutValue);

        private static DateTimeOffset ParseExpected(string value)
            => DateTimeOffset.ParseExact(
                value,
                IsoDateTimeOffset.AllFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind);
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Zulu", "2020-01-02T03:04:05Z", new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero)),
            new("Fraction .1234567 Z", "2020-01-02T03:04:05.1234567Z",
                new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero).AddTicks(1_234_567)),
            new("Offset +01:00", "2020-01-02T03:04:05+01:00",
                new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(1))),
            new("Fraction .5 -08:00", "2020-01-02T03:04:05.5-08:00",
                new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.FromHours(-8)).AddTicks(5_000_000)),
            new("Missing timezone", "2020-01-02T03:04:05", ParseExpected("2020-01-02T03:04:05"))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null", null, new ExpectedException(typeof(ArgumentNullException), "input")),
            new("Empty", string.Empty, new ExpectedException(typeof(FormatException))),
            new("Space", " ", new ExpectedException(typeof(FormatException))),
            new("Tab", "\t", new ExpectedException(typeof(FormatException))),
            new("Date only", "2020-01-02", new ExpectedException(typeof(FormatException))),
            new("Missing seconds", "2020-01-02T03:04", new ExpectedException(typeof(FormatException))),
            new("Too many fractional digits", "2020-01-02T03:04:05.12345678Z", new ExpectedException(typeof(FormatException))),
            new("Invalid offset format", "2020-01-02T03:04:05+01:0", new ExpectedException(typeof(FormatException))),
            new("Invalid offset values", "2020-01-02T03:04:05+99:99", new ExpectedException(typeof(FormatException))),
            new("Non-date", "abcd", new ExpectedException(typeof(FormatException)))
        ];

        public sealed record ValidCase(string Name, string Value, DateTimeOffset ExpectedReturn)
            : ReturnCase<string, DateTimeOffset>(Name, Value, ExpectedReturn);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);

        private static DateTimeOffset ParseExpected(string value)
            => DateTimeOffset.ParseExact(
                value,
                IsoDateTimeOffset.AllFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.RoundtripKind);
    }

    public static class ToIsoString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Zulu", new DateTimeOffset(2020, 01, 02, 03, 04, 05, TimeSpan.Zero),
                "2020-01-02T03:04:05.0000000+00:00"),
            new("Leap day", new DateTimeOffset(2000, 02, 29, 00, 00, 00, TimeSpan.Zero),
                "2000-02-29T00:00:00.0000000+00:00")
        ];

        public sealed record ValidCase(string Name, DateTimeOffset Value, string ExpectedReturn)
            : ReturnCase<DateTimeOffset, string>(Name, Value, ExpectedReturn);
    }
}
