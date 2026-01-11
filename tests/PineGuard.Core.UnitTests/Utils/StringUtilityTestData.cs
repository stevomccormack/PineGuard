using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityTestData
{
    public static class TryGetTrimmed
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("a", "a", true, "a"),
            new("trim", " a ", true, "a"),
            new("internal spaces", "  a  b  ", true, "a  b"),
            new("0", "0", true, "0"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", "", false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("nbsp", "\u00A0", false, string.Empty),
            new("leading spaces", "  x", true, "x"),
            new("trailing spaces", "x  ", true, "x"),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryParseDigitsOnly
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("123", "123", true, "123"),
            new("trim", " 001 ", true, "001"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("0", "0", true, "0"),
            new("null", null, false, string.Empty),
            new("empty", "", false, string.Empty),
            new("space", " ", false, string.Empty),
            new("tab", "\t", false, string.Empty),
            new("embedded space", "12 34", false, string.Empty),
            new("separator", "12-34", false, string.Empty),
            new("non-digit", "1a2", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryParseDigits
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("spaces", "12 34", null, true, "1234"),
            new("dashes", "12-34", null, true, "1234"),
            new("trim", "  12- 34  ", null, true, "1234"),
            new("custom sep", "12_34", ['_'], true, "1234"),
        ];

        public static TheoryData<Case> EdgeCases =>
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
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, char[]? AllowedSeparators, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TitleCase
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("hello world", "hello world", true, "Hello World"),
            new("trim", "  hELLo wORLD  ", true, "Hello World"),
            new("single", "m", true, "M"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("o'connor", "o'connor", true, "O'connor"),
            new("null", null, false, string.Empty),
            new("empty", "", false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TimeOnlyTryParse
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("12:34:56", "12:34:56", true, new TimeOnly(12, 34, 56)),
            new("trim", " 09:00:00 ", true, new TimeOnly(9, 0, 0)),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", string.Empty, false, null),
            new("space", " ", false, null),
            new("whitespace", "\t\r\n", false, null),
            new("invalid", "not-a-time", false, null),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, TimeOnly? ExpectedOutValue)
            : TryCase<string?, TimeOnly?>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TimeSpanTryParse
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("01:02:03", "01:02:03", true, new TimeSpan(1, 2, 3)),
            new("trim", " 00:00:00 ", true, new TimeSpan(0, 0, 0)),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, null),
            new("empty", string.Empty, false, null),
            new("space", " ", false, null),
            new("whitespace", "\t\r\n", false, null),
            new("invalid", "not-a-timespan", false, null),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, TimeSpan? ExpectedOutValue)
            : TryCase<string?, TimeSpan?>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }
}
