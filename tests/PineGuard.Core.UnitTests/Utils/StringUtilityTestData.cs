using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class StringUtilityTestData
{
    public static class TryGetTrimmed
    {
        private static ValidCase V(string name, string? value, bool expectedOk, string expectedTrimmed) => new(name, value, expectedOk, expectedTrimmed);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("a", "a", true, "a") },
            { V("trim", " a ", true, "a") },
            { V("internal spaces", "  a  b  ", true, "a  b") },
            { V("0", "0", true, "0") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", "", false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("nbsp", "\u00A0", false, string.Empty) },
            { V("leading spaces", "  x", true, "x") },
            { V("trailing spaces", "x  ", true, "x") },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool ExpectedOk, string ExpectedTrimmed)
            : Case(Name);

        #endregion
    }

    public static class TryParseDigitsOnly
    {
        private static ValidCase V(string name, string? value, bool expectedOk, string expectedDigits) => new(name, value, expectedOk, expectedDigits);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("123", "123", true, "123") },
            { V("trim", " 001 ", true, "001") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("0", "0", true, "0") },
            { V("null", null, false, string.Empty) },
            { V("empty", "", false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("tab", "\t", false, string.Empty) },
            { V("embedded space", "12 34", false, string.Empty) },
            { V("separator", "12-34", false, string.Empty) },
            { V("non-digit", "1a2", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool ExpectedOk, string ExpectedDigits)
            : Case(Name);

        #endregion
    }

    public static class TryParseDigits
    {
        private static ValidCase V(string name, string? value, char[]? allowedSeparators, bool expectedOk, string expectedDigits)
            => new(name, value, allowedSeparators, expectedOk, expectedDigits);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("spaces", "12 34", null, true, "1234") },
            { V("dashes", "12-34", null, true, "1234") },
            { V("trim", "  12- 34  ", null, true, "1234") },
            { V("custom sep", "12_34", new[] { '_' }, true, "1234") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("single", "1", Array.Empty<char>(), true, "1") },
            { V("single trimmed", " 9 ", Array.Empty<char>(), true, "9") },
            { V("null", null, null, false, string.Empty) },
            { V("empty", "", null, false, string.Empty) },
            { V("space", " ", null, false, string.Empty) },
            { V("only separators", "--", null, false, string.Empty) },
            { V("separator not allowed", "12_34", null, false, string.Empty) },
            { V("non-digit", "1a2", null, false, string.Empty) },
            { V("dash disallowed", "-", Array.Empty<char>(), false, string.Empty) },
            { V("space disallowed", " ", Array.Empty<char>(), false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, char[]? AllowedSeparators, bool ExpectedOk, string ExpectedDigits)
            : Case(Name);

        #endregion
    }

    public static class TitleCase
    {
        private static ValidCase V(string name, string? value, bool expectedOk, string expectedTitleCased) => new(name, value, expectedOk, expectedTitleCased);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("hello world", "hello world", true, "Hello World") },
            { V("trim", "  hELLo wORLD  ", true, "Hello World") },
            { V("single", "m", true, "M") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("o'connor", "o'connor", true, "O'connor") },
            { V("null", null, false, string.Empty) },
            { V("empty", "", false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool ExpectedOk, string ExpectedTitleCased)
            : Case(Name);

        #endregion
    }

    public static class TimeOnlyTryParse
    {
        private static ValidCase V(string name, string? value, bool expectedOk, global::System.TimeOnly? expectedTime) => new(name, value, expectedOk, expectedTime);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("12:34:56", "12:34:56", true, new global::System.TimeOnly(12, 34, 56)) },
            { V("trim", " 09:00:00 ", true, new global::System.TimeOnly(9, 0, 0)) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, false, null) },
            { V("empty", string.Empty, false, null) },
            { V("space", " ", false, null) },
            { V("whitespace", "\t\r\n", false, null) },
            { V("invalid", "not-a-time", false, null) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool ExpectedOk, global::System.TimeOnly? ExpectedTime)
            : Case(Name);

        #endregion
    }

    public static class TimeSpanTryParse
    {
        private static ValidCase V(string name, string? value, bool expectedOk, global::System.TimeSpan? expectedTimeSpan) => new(name, value, expectedOk, expectedTimeSpan);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("01:02:03", "01:02:03", true, new global::System.TimeSpan(1, 2, 3)) },
            { V("trim", " 00:00:00 ", true, new global::System.TimeSpan(0, 0, 0)) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, false, null) },
            { V("empty", string.Empty, false, null) },
            { V("space", " ", false, null) },
            { V("whitespace", "\t\r\n", false, null) },
            { V("invalid", "not-a-timespan", false, null) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? Value, bool ExpectedOk, global::System.TimeSpan? ExpectedTimeSpan)
            : Case(Name);

        #endregion
    }
}
