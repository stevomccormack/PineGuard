using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Languages;

public static class DefaultIsoLanguageProviderTestData
{
    public static class TryGet
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("Alpha2 en", "en", true, "en"),
            new("Alpha2 fr", "fr", true, "fr"),
            new("Alpha3 eng", "eng", true, "en"),
            new("Alpha3 fra", "fra", true, "fr"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false, null),
            new("Empty", string.Empty, false, null),
            new("Space", " ", false, null),
            new("Tab", "\t", false, null),
            new("Newline", "\r\n", false, null),
            new("Too short", "e", false, null),
            new("Non-alpha", "e1", false, null),
            new("Unknown alpha2", "zz", false, null),
            new("Unknown alpha3", "zzz", false, null),
            new("Too long", "engl", false, null),
            new("Whitespace padded", " en ", false, null),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }

    public static class ContainsAlpha2Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("en", "en", true),
            new("fr", "fr", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Unknown", "zz", false),
            new("Whitespace padded", " en ", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class ContainsAlpha3Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("eng", "eng", true),
            new("fra", "fra", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Unknown", "zzz", false),
            new("Non-alpha", "e1g", false),
            new("Whitespace padded", " eng ", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class TryGetByAlpha2Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("en", "en", true),
            new("fr", "fr", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Unknown", "zz", false),
            new("Non-alpha", "e1", false),
            new("Whitespace padded", " en ", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedReturn ? Value : null);
    }

    public static class TryGetByAlpha3Code
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("eng", "eng", true),
            new("fra", "fra", true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Unknown", "zzz", false),
            new("Non-alpha", "e1g", false),
            new("Whitespace padded", " eng ", false),
        ];

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedReturn ? Value : null);
    }
}
