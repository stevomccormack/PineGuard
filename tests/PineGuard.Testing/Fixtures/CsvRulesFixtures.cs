using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class CsvRulesFixtures
{
    public static class IsCsvLine
    {
        public static readonly string? Simple = "a,b";
        public static readonly string? ThreeFields = "a,b,c";
        public static readonly string? SingleField = "hello";
        public static readonly string? EmptyFields = ",,";
        public static readonly string? QuotedField = "\"hello\",world";
        public static readonly string? EscapedQuote = "\"he\"\"llo\",world";
        public static readonly string? QuotedComma = "\"a,b\",c";
        public static readonly string? WhitespaceField = " a , b ";
        public static readonly string? EmptyQuoted = "\"\",a";
        public static readonly string? TabSeparated = "a\tb";
        public static readonly string? UnclosedQuote = "\"a";
        public static readonly string? MidFieldQuote = "a\"b,c";
        public static readonly string? ContainsCr = "a\rb";
        public static readonly string? ContainsLf = "a\nb";
        public static readonly string? NullValue = null;
        public static readonly string? WhitespaceOnly = "   ";
        public static readonly string? EmptyString = "";

        public static RuleScenario<string?>[] ValidScenarios =>
        [
            new(nameof(Simple),          Simple,          true),
            new(nameof(ThreeFields),     ThreeFields,     true),
            new(nameof(SingleField),     SingleField,     true),
            new(nameof(EmptyFields),     EmptyFields,     true),
            new(nameof(QuotedField),     QuotedField,     true),
            new(nameof(EscapedQuote),    EscapedQuote,    true),
            new(nameof(QuotedComma),     QuotedComma,     true),
            new(nameof(WhitespaceField), WhitespaceField, true),
            new(nameof(EmptyQuoted),     EmptyQuoted,     true),
            new(nameof(TabSeparated),    TabSeparated,    true)
        ];

        public static RuleScenario<string?>[] InvalidScenarios =>
        [
            new(nameof(UnclosedQuote),  UnclosedQuote,  false),
            new(nameof(MidFieldQuote),  MidFieldQuote,  false),
            new(nameof(ContainsCr),     ContainsCr,     false),
            new(nameof(ContainsLf),     ContainsLf,     false),
            new(nameof(NullValue),      NullValue,      false),
            new(nameof(WhitespaceOnly), WhitespaceOnly, false),
            new(nameof(EmptyString),    EmptyString,    false)
        ];

        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsCsvHeaderLine
    {
        public static readonly (string? line, IReadOnlyList<string>? expectedHeader) Matches = ("a,b", ["a", "b"]);
        public static readonly (string? line, IReadOnlyList<string>? expectedHeader) Mismatch = ("a,b", ["a", "c"]);
        public static readonly (string? line, IReadOnlyList<string>? expectedHeader) NullExpected = ("a,b", null);
    }
}
