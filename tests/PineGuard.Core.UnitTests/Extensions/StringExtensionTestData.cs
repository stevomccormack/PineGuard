using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Extensions;

public static class StringExtensionTestData
{
    public static class TitleCase
    {
        private static ValidCase V(string name, string value, bool expected, string expectedTitle) => new(Name: name, Value: value, Expected: expected, ExpectedTitle: expectedTitle);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("simple", "hello", expected: true, expectedTitle: "Hello") },
            { V("two words", "hello world", expected: true, expectedTitle: "Hello World") },
            { V("all caps", "HELLO WORLD", expected: true, expectedTitle: "Hello World") },
            { V("trim", "  hello world  ", expected: true, expectedTitle: "Hello World") },
            { V("mixed", "mIXeD caSe", expected: true, expectedTitle: "Mixed Case") },
            { V("single char", "a", expected: true, expectedTitle: "A") },
            { V("xUnit", "xUnit", expected: true, expectedTitle: "Xunit") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("already title", "already Title Case", expected: true, expectedTitle: "Already Title Case") },
            { V("multiple spaces", "two   spaces", expected: true, expectedTitle: "Two   Spaces") },
            { V("leading digits", "123 abc", expected: true, expectedTitle: "123 Abc") },
            { V("empty", "", expected: false, expectedTitle: string.Empty) },
            { V("space", " ", expected: false, expectedTitle: string.Empty) },
            { V("tab", "\t", expected: false, expectedTitle: string.Empty) },
            { V("newline", "\r\n", expected: false, expectedTitle: string.Empty) },
            { V("mixed whitespace", "   \t  ", expected: false, expectedTitle: string.Empty) },
            { V("nbsp", "\u00A0", expected: false, expectedTitle: string.Empty) },
            { V("figure space", "\u2007\u2007", expected: false, expectedTitle: string.Empty) },
        };

        #region Cases

        public record Case(string Name, string Value);

        public sealed record ValidCase(string Name, string Value, bool Expected, string ExpectedTitle)
            : Case(Name, Value);

        public record InvalidCase(string Name, string Value, ExpectedException ExpectedException)
            : Case(Name, Value);

        #endregion
    }
}
