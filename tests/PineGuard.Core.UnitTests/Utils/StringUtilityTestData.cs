using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityTestData
{
    private static readonly string LongDigits = new('7', 300);

    public static class TryGetTrimmed
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("a", "a", true, "a"),
            new("trim", " a ", true, "a"),
            new("internal spaces", "  a  b  ", true, "a  b"),
            new("0", "0", true, "0")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", "", false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("nbsp", "\u00A0", false, string.Empty),
            new("leading spaces", "  x", true, "x"),
            new("trailing spaces", "x  ", true, "x")
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseDigitsOnly
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("123", "123", true, "123"),
            new("trim", " 001 ", true, "001")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("0", "0", true, "0"),
            new("null", null, false, string.Empty),
            new("empty", "", false, string.Empty),
            new("space", " ", false, string.Empty),
            new("tab", "\t", false, string.Empty),
            new("embedded space", "12 34", false, string.Empty),
            new("separator", "12-34", false, string.Empty),
            new("non-digit", "1a2", false, string.Empty),
            new("less than zero char", "/", false, string.Empty)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryParseDigits
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("spaces", "12 34", null, true, "1234"),
            new("dashes", "12-34", null, true, "1234"),
            new("trim", "  12- 34  ", null, true, "1234"),
            new("custom sep", "12_34", ['_'], true, "1234"),
            new("long value exceeds stack allocation", LongDigits, null, true, LongDigits)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("single", "1", [], true, "1"),
            new("single trimmed", " 9 ", [], true, "9"),
            new("null", null, null, false, string.Empty),
            new("empty", "", null, false, string.Empty),
            new("space", " ", null, false, string.Empty),
            new("only separators", "--", null, false, string.Empty),
            new("separator not allowed", "12_34", null, false, string.Empty),
            new("non-digit", "1a2", null, false, string.Empty),
            new("dash disallowed", "-", [], false, string.Empty),
            new("space disallowed", " ", [], false, string.Empty),
            new("explicit disallowed", "1a", ['b'], false, string.Empty)
        ];

        public sealed record ValidCase(string Name, string? Value, char[]? AllowedSeparators, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TitleCase
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("hello world", "hello world", true, "Hello World"),
            new("trim", "  hELLo wORLD  ", true, "Hello World"),
            new("single", "m", true, "M")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("o'connor", "o'connor", true, "O'connor"),
            new("null", null, false, string.Empty),
            new("empty", "", false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class Bool
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("true", "true", true, true),
            new("false", "false", true, false),
            new("trim", " true ", true, true),
            new("case-insensitive", "True", true, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", "", false, null),
            new("whitespace", "   ", false, null),
            new("not a bool", "yes", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, bool? ExpectedOutValue)
            : TryCase<string?, bool?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TimeOnlyTryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("12:34:56", "12:34:56", true, new TimeOnly(12, 34, 56)),
            new("trim", " 09:00:00 ", true, new TimeOnly(9, 0, 0))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", string.Empty, false, null),
            new("space", " ", false, null),
            new("whitespace", "\t\r\n", false, null),
            new("invalid", "not-a-time", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, TimeOnly? ExpectedOutValue)
            : TryCase<string?, TimeOnly?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TimeSpanTryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("01:02:03", "01:02:03", true, new TimeSpan(1, 2, 3)),
            new("trim", " 00:00:00 ", true, new TimeSpan(0, 0, 0))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", string.Empty, false, null),
            new("space", " ", false, null),
            new("whitespace", "\t\r\n", false, null),
            new("invalid", "not-a-timespan", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, TimeSpan? ExpectedOutValue)
            : TryCase<string?, TimeSpan?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class DateTimeOffsetTryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("offset-less assumes utc deterministically", "2024-01-15T10:30:00", true, new DateTimeOffset(2024, 01, 15, 10, 30, 00, TimeSpan.Zero)),
            new("z suffix stays utc", "2024-01-15T10:30:00Z", true, new DateTimeOffset(2024, 01, 15, 10, 30, 00, TimeSpan.Zero)),
            new("explicit offset preserved", "2024-01-15T10:30:00+05:00", true, new DateTimeOffset(2024, 01, 15, 10, 30, 00, TimeSpan.FromHours(5)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", string.Empty, false, null),
            new("whitespace", "\t\r\n", false, null),
            new("invalid", "not-a-datetimeoffset", false, null)
        ];

        public sealed record ValidCase(string Name, string? Value, bool Expected, DateTimeOffset? ExpectedOutValue)
            : TryCase<string?, DateTimeOffset?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class DateOnlyRangeTryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", ("2020-01-01", "2020-01-02"), true, new DateOnlyRange(new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 02))),
            new("trim", (" 2020-01-01 ", " 2020-01-01 "), true, new DateOnlyRange(new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 01)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("start null", (null, "2020-01-01"), false, null),
            new("start whitespace", (" ", "2020-01-01"), false, null),
            new("end null", ("2020-01-01", null), false, null),
            new("end whitespace", ("2020-01-01", "\t\r\n"), false, null),
            new("start invalid", ("not-a-date", "2020-01-01"), false, null),
            new("end invalid", ("2020-01-01", "not-a-date"), false, null),
            new("start after end", ("2020-01-02", "2020-01-01"), false, null)
        ];

        public sealed record ValidCase(string Name, (string? Start, string? End) Value, bool Expected, DateOnlyRange? ExpectedOutValue)
            : TryCase<(string? Start, string? End), DateOnlyRange?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TimeOnlyRangeTryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", ("09:00:00", "10:00:00"), true, new TimeOnlyRange(new TimeOnly(9, 0, 0), new TimeOnly(10, 0, 0))),
            new("trim", (" 09:00:00 ", " 09:00:00 "), true, new TimeOnlyRange(new TimeOnly(9, 0, 0), new TimeOnly(9, 0, 0)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("start null", (null, "09:00:00"), false, null),
            new("start whitespace", (" ", "09:00:00"), false, null),
            new("end null", ("09:00:00", null), false, null),
            new("end whitespace", ("09:00:00", "\t\r\n"), false, null),
            new("start invalid", ("not-a-time", "09:00:00"), false, null),
            new("end invalid", ("09:00:00", "not-a-time"), false, null),
            new("start after end", ("10:00:00", "09:00:00"), false, null)
        ];

        public sealed record ValidCase(string Name, (string? Start, string? End) Value, bool Expected, TimeOnlyRange? ExpectedOutValue)
            : TryCase<(string? Start, string? End), TimeOnlyRange?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class DateTimeRangeTryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok utc", ("2020-01-01T00:00:00.0000000Z", "2020-01-01T00:00:01.0000000Z"), true, new DateTimeRange(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 01, DateTimeKind.Utc))),
            new("trim utc", (" 2020-01-01T00:00:00.0000000Z ", " 2020-01-01T00:00:00.0000000Z "), true, new DateTimeRange(new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2020, 01, 01, 00, 00, 00, DateTimeKind.Utc))),
            new("z and explicit zero offset both normalize to utc", ("2024-01-01T00:00:00Z", "2024-12-31T23:59:59+00:00"), true, new DateTimeRange(new DateTime(2024, 01, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2024, 12, 31, 23, 59, 59, DateTimeKind.Utc))),
            new("explicit offset normalizes deterministically to utc", ("2024-06-01T05:00:00+05:00", "2024-06-01T10:00:00Z"), true, new DateTimeRange(new DateTime(2024, 06, 01, 00, 00, 00, DateTimeKind.Utc), new DateTime(2024, 06, 01, 10, 00, 00, DateTimeKind.Utc)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("start null", (null, "2020-01-01T00:00:00.0000000Z"), false, null),
            new("start whitespace", (" ", "2020-01-01T00:00:00.0000000Z"), false, null),
            new("end null", ("2020-01-01T00:00:00.0000000Z", null), false, null),
            new("end whitespace", ("2020-01-01T00:00:00.0000000Z", "\t\r\n"), false, null),
            new("start invalid", ("not-a-datetime", "2020-01-01T00:00:00.0000000Z"), false, null),
            new("end invalid", ("2020-01-01T00:00:00.0000000Z", "not-a-datetime"), false, null),
            new("start after end", ("2020-01-01T00:00:01.0000000Z", "2020-01-01T00:00:00.0000000Z"), false, null)
        ];

        public sealed record ValidCase(string Name, (string? Start, string? End) Value, bool Expected, DateTimeRange? ExpectedOutValue)
            : TryCase<(string? Start, string? End), DateTimeRange?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class DateTimeOffsetRangeTryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("ok", ("2020-01-01T00:00:00.0000000+00:00", "2020-01-01T00:00:01.0000000+00:00"), true, new DateTimeOffsetRange(new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 01, TimeSpan.Zero))),
            new("trim", (" 2020-01-01T00:00:00.0000000+00:00 ", " 2020-01-01T00:00:00.0000000+00:00 "), true, new DateTimeOffsetRange(new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero), new DateTimeOffset(2020, 01, 01, 00, 00, 00, TimeSpan.Zero))),
            new("offset-less assumes utc deterministically", ("2024-01-15T10:30:00", "2024-01-15T11:30:00"), true, new DateTimeOffsetRange(new DateTimeOffset(2024, 01, 15, 10, 30, 00, TimeSpan.Zero), new DateTimeOffset(2024, 01, 15, 11, 30, 00, TimeSpan.Zero)))
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("start null", (null, "2020-01-01T00:00:00.0000000+00:00"), false, null),
            new("start whitespace", (" ", "2020-01-01T00:00:00.0000000+00:00"), false, null),
            new("end null", ("2020-01-01T00:00:00.0000000+00:00", null), false, null),
            new("end whitespace", ("2020-01-01T00:00:00.0000000+00:00", "\t\r\n"), false, null),
            new("start invalid", ("not-a-datetimeoffset", "2020-01-01T00:00:00.0000000+00:00"), false, null),
            new("end invalid", ("2020-01-01T00:00:00.0000000+00:00", "not-a-datetimeoffset"), false, null),
            new("start after end", ("2020-01-01T00:00:01.0000000+00:00", "2020-01-01T00:00:00.0000000+00:00"), false, null)
        ];

        public sealed record ValidCase(string Name, (string? Start, string? End) Value, bool Expected, DateTimeOffsetRange? ExpectedOutValue)
            : TryCase<(string? Start, string? End), DateTimeOffsetRange?>(Name, Value, Expected, ExpectedOutValue);
    }

    public static class TryCreateRegex
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("literal", "abc", true, true),
            new("anchored", "^abc$", true, true),
            new("character class", "[a-z]+", true, true),
            new("quantified", @"^\d{3}-\d{4}$", true, true),
            new("named group", @"(?<year>\d{4})", true, true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("space is a significant pattern", " ", true, true),
            new("null", null, false, false),
            new("empty", "", false, false),
            new("unclosed character class", "[unclosed", false, false),
            new("unclosed group", "(unclosed", false, false),
            new("unbalanced close paren", "a)b", false, false),
            new("dangling quantifier", "*", false, false),
            new("reversed quantifier range", "a{3,1}", false, false),
            new("unknown unicode category", @"\p{NotACategory}", false, false)
        ];

        public sealed record ValidCase : ReturnCase<string?, (bool ok, bool hasRegex)>
        {
            public ValidCase(string name, string? value, bool expectedOk, bool expectedHasRegex)
                : base(name, value, (expectedOk, expectedHasRegex)) { }
        }
    }
}
