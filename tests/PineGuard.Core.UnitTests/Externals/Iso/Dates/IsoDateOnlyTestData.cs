using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Dates;

public static class IsoDateOnlyTestData
{
    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("2020-01-02", "2020-01-02", true, new DateOnly(2020, 01, 02)),
            new("1999-12-31", "1999-12-31", true, new DateOnly(1999, 12, 31)),
            new("2000-02-29", "2000-02-29", true, new DateOnly(2000, 02, 29)),
            new("2024-02-29", "2024-02-29", true, new DateOnly(2024, 02, 29)),
            new("Min", "0001-01-01", true, DateOnly.MinValue),
            new("Max", "9999-12-31", true, DateOnly.MaxValue)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, default),
            new("Empty", string.Empty, false, default),
            new("Space", " ", false, default),
            new("Tab", "\t", false, default),
            new("Newline", "\r\n", false, default),
            new("Slash separators", "2020/01/02", false, default),
            new("Non-padded", "2020-1-2", false, default),
            new("Short year", "20-01-02", false, default),
            new("Month 13", "2020-13-01", false, default),
            new("Month 0", "2020-00-01", false, default),
            new("Day 0", "2020-01-00", false, default),
            new("Day 32", "2020-01-32", false, default),
            new("Non-leap Feb 29", "2021-02-29", false, default),
            new("Non-date", "abcd-ef-gh", false, default),
            new("DateTime", "2020-01-02T00:00:00", false, default),
            new("Date with Z", "2020-01-02Z", false, default)
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, DateOnly ExpectedOutValue)
            : TryCase<string?, DateOnly>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }

    public static class Parse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("2020-01-02", "2020-01-02", new DateOnly(2020, 01, 02)),
            new("1999-12-31", "1999-12-31", new DateOnly(1999, 12, 31)),
            new("2000-02-29", "2000-02-29", new DateOnly(2000, 02, 29)),
            new("2024-02-29", "2024-02-29", new DateOnly(2024, 02, 29)),
            new("Min", "0001-01-01", DateOnly.MinValue),
            new("Max", "9999-12-31", DateOnly.MaxValue)
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null", null, new ExpectedException(typeof(ArgumentNullException), "s")),
            new("Empty", string.Empty, new ExpectedException(typeof(FormatException), null, "date")),
            new("Space", " ", new ExpectedException(typeof(FormatException), null, "date")),
            new("Tab", "\t", new ExpectedException(typeof(FormatException), null, "date")),
            new("Newline", "\r\n", new ExpectedException(typeof(FormatException), null, "date")),
            new("Slash separators", "2020/01/02", new ExpectedException(typeof(FormatException), null, "date")),
            new("Non-padded", "2020-1-2", new ExpectedException(typeof(FormatException), null, "date")),
            new("Short year", "20-01-02", new ExpectedException(typeof(FormatException), null, "date")),
            new("Month 13", "2020-13-01", new ExpectedException(typeof(FormatException), null, "date")),
            new("Month 0", "2020-00-01", new ExpectedException(typeof(FormatException), null, "date")),
            new("Day 0", "2020-01-00", new ExpectedException(typeof(FormatException), null, "date")),
            new("Day 32", "2020-01-32", new ExpectedException(typeof(FormatException), null, "date")),
            new("Non-leap Feb 29", "2021-02-29", new ExpectedException(typeof(FormatException), null, "date")),
            new("Non-date", "abcd-ef-gh", new ExpectedException(typeof(FormatException), null, "date")),
            new("DateTime", "2020-01-02T00:00:00", new ExpectedException(typeof(FormatException), null, "date")),
            new("Date with Z", "2020-01-02Z", new ExpectedException(typeof(FormatException), null, "date"))
        ];

        public sealed record ValidCase(string Name, string Value, DateOnly ExpectedReturn)
            : ReturnCase<string, DateOnly>(Name, Value, ExpectedReturn);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);
    }

    public static class ToIsoString
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Min", DateOnly.MinValue, "0001-01-01"),
            new("Max", DateOnly.MaxValue, "9999-12-31"),
            new("2020-01-02", new DateOnly(2020, 01, 02), "2020-01-02"),
            new("2024-02-29", new DateOnly(2024, 02, 29), "2024-02-29")
        ];

        public sealed record ValidCase(string Name, DateOnly Value, string ExpectedReturn)
            : ReturnCase<DateOnly, string>(Name, Value, ExpectedReturn);
    }
}
