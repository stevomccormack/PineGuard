using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Extensions;

public static class StringExtensionTestData
{
    public static class TitleCase
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("simple", "hello", true, "Hello"),
            new("two words", "hello world", true, "Hello World"),
            new("all caps", "HELLO WORLD", true, "Hello World"),
            new("trim", "  hello world  ", true, "Hello World"),
            new("mixed", "mIXeD caSe", true, "Mixed Case"),
            new("single char", "a", true, "A"),
            new("xUnit", "xUnit", true, "Xunit"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("already title", "already Title Case", true, "Already Title Case"),
            new("multiple spaces", "two   spaces", true, "Two   Spaces"),
            new("leading digits", "123 abc", true, "123 Abc"),
            new("empty", "", false, string.Empty),
            new("space", " ", false, string.Empty),
            new("tab", "\t", false, string.Empty),
            new("newline", "\r\n", false, string.Empty),
            new("mixed whitespace", "   \t  ", false, string.Empty),
            new("nbsp", "\u00A0", false, string.Empty),
            new("figure space", "\u2007\u2007", false, string.Empty),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, string Value, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string, string>(Name, Value, ExpectedReturn, ExpectedOutValue);

        #endregion
    }
}
