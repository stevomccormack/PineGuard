namespace PineGuard.Core.UnitTests.Externals.Iso.Languages;

public static class DefaultIsoLanguageProviderTestData
{
    public static class TryGet
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("Alpha2 en", value: "en", expected: true, expectedAlpha2: "en"),
            V("Alpha2 fr", value: "fr", expected: true, expectedAlpha2: "fr"),
            V("Alpha3 eng", value: "eng", expected: true, expectedAlpha2: "en"),
            V("Alpha3 fra", value: "fra", expected: true, expectedAlpha2: "fr"),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            V("Null", value: null, expected: false, expectedAlpha2: null),
            V("Empty", value: string.Empty, expected: false, expectedAlpha2: null),
            V("Space", value: " ", expected: false, expectedAlpha2: null),
            V("Tab", value: "\t", expected: false, expectedAlpha2: null),
            V("Newline", value: "\r\n", expected: false, expectedAlpha2: null),
            V("Too short", value: "e", expected: false, expectedAlpha2: null),
            V("Non-alpha", value: "e1", expected: false, expectedAlpha2: null),
            V("Unknown alpha2", value: "zz", expected: false, expectedAlpha2: null),
            V("Unknown alpha3", value: "zzz", expected: false, expectedAlpha2: null),
            V("Too long", value: "engl", expected: false, expectedAlpha2: null),
            V("Whitespace padded", value: " en ", expected: false, expectedAlpha2: null),
        };

        private static Case V(string name, string? value, bool expected, string? expectedAlpha2) => new(name, value, expected, expectedAlpha2);

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected, string? ExpectedAlpha2);

        #endregion
    }

    public static class ContainsAlpha2Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("en", value: "en", expected: true),
            V("fr", value: "fr", expected: true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Tab", value: "\t", expected: false),
            V("Unknown", value: "zz", expected: false),
            V("Whitespace padded", value: " en ", expected: false),
        };

        private static Case V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected);

        #endregion
    }

    public static class ContainsAlpha3Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("eng", value: "eng", expected: true),
            V("fra", value: "fra", expected: true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Tab", value: "\t", expected: false),
            V("Unknown", value: "zzz", expected: false),
            V("Non-alpha", value: "e1g", expected: false),
            V("Whitespace padded", value: " eng ", expected: false),
        };

        private static Case V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected);

        #endregion
    }

    public static class TryGetByAlpha2Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("en", value: "en", expected: true),
            V("fr", value: "fr", expected: true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Tab", value: "\t", expected: false),
            V("Unknown", value: "zz", expected: false),
            V("Non-alpha", value: "e1", expected: false),
            V("Whitespace padded", value: " en ", expected: false),
        };

        private static Case V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected);

        #endregion
    }

    public static class TryGetByAlpha3Code
    {
        public static TheoryData<Case> ValidCases => new()
        {
            V("eng", value: "eng", expected: true),
            V("fra", value: "fra", expected: true),
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Tab", value: "\t", expected: false),
            V("Unknown", value: "zzz", expected: false),
            V("Non-alpha", value: "e1g", expected: false),
            V("Whitespace padded", value: " eng ", expected: false),
        };

        private static Case V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected);

        #endregion
    }
}
