using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public static class IsoDateTimeTestData
{
    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("2020-01-02T03:04:05", "2020-01-02T03:04:05", true,
                new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified)),
            new("1999-12-31T23:59:59", "1999-12-31T23:59:59", true,
                new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified)),
            new("2000-02-29T00:00:00", "2000-02-29T00:00:00", true,
                new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Unspecified)),
            new("Fraction .1", "2020-01-02T03:04:05.1", true,
                new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_000_000)),
            new("Fraction .1234567", "2020-01-02T03:04:05.1234567", true,
                new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_234_567))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, default),
            new("Empty", string.Empty, false, default),
            new("Space", " ", false, default),
            new("Tab", "\t", false, default),
            new("Date only", "2020-01-02", false, default),
            new("Space separator", "2020-01-02 03:04:05", false, default),
            new("Missing seconds", "2020-01-02T03:04", false, default),
            new("Too many fractional digits", "2020-01-02T03:04:05.12345678", false, default),
            new("Non-date", "abcd-ef-ghTij:kl:mn", false, default),
            new("UTC designator", "2020-01-02T03:04:05Z", false, default),
            new("Offset", "2020-01-02T03:04:05+01:00", false, default)
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, DateTime ExpectedOutValue)
            : TryCase<string?, DateTime>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("2020-01-02T03:04:05", "2020-01-02T03:04:05",
                new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified)),
            new("1999-12-31T23:59:59", "1999-12-31T23:59:59",
                new DateTime(1999, 12, 31, 23, 59, 59, DateTimeKind.Unspecified)),
            new("2000-02-29T00:00:00", "2000-02-29T00:00:00",
                new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Unspecified)),
            new("Fraction .1", "2020-01-02T03:04:05.1",
                new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_000_000)),
            new("Fraction .1234567", "2020-01-02T03:04:05.1234567",
                new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified).AddTicks(1_234_567))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null", null, new ExpectedException(typeof(ArgumentNullException), "s")),
            new("Empty", string.Empty, new ExpectedException(typeof(FormatException))),
            new("Space", " ", new ExpectedException(typeof(FormatException))),
            new("Tab", "\t", new ExpectedException(typeof(FormatException))),
            new("Date only", "2020-01-02", new ExpectedException(typeof(FormatException))),
            new("Space separator", "2020-01-02 03:04:05", new ExpectedException(typeof(FormatException))),
            new("Missing seconds", "2020-01-02T03:04", new ExpectedException(typeof(FormatException))),
            new("Too many fractional digits", "2020-01-02T03:04:05.12345678", new ExpectedException(typeof(FormatException))),
            new("Non-date", "abcd-ef-ghTij:kl:mn", new ExpectedException(typeof(FormatException))),
            new("UTC designator", "2020-01-02T03:04:05Z", new ExpectedException(typeof(FormatException))),
            new("Offset", "2020-01-02T03:04:05+01:00", new ExpectedException(typeof(FormatException)))
        ];

        public sealed record ValidCase(string Name, string Value, DateTime ExpectedReturn)
            : ReturnCase<string, DateTime>(Name, Value, ExpectedReturn);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);
    }

    public static class ToIsoString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("2020-01-02", new DateTime(2020, 01, 02, 03, 04, 05, DateTimeKind.Unspecified),
                "2020-01-02T03:04:05.0000000"),
            new("2000-02-29", new DateTime(2000, 02, 29, 00, 00, 00, DateTimeKind.Unspecified),
                "2000-02-29T00:00:00.0000000")
        ];

        public sealed record ValidCase(string Name, DateTime Value, string ExpectedReturn)
            : ReturnCase<DateTime, string>(Name, Value, ExpectedReturn);
    }
}
