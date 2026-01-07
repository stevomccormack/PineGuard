using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoLanguageUtilityTestData
{
    public static class TryParseAlpha2
    {
        private static Case V(string name, string? languageCode, bool expectedSuccess, string expectedAlpha2) => new(name, languageCode, expectedSuccess, expectedAlpha2);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("en", "en", true, "en") },
            { V("EN", "EN", true, "EN") },
            { V("trimmed", " fr ", true, "fr") },
            { V("trimmed tabs", "\tde\r\n", true, "de") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("length 1", "e", false, string.Empty) },
            { V("length 3", "eng", false, string.Empty) },
            { V("digit", "e1", false, string.Empty) },
            { V("separator", "e-", false, string.Empty) },
            { V("digit prefix", "1e", false, string.Empty) },
            { V("trimmed invalid", " e1 ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? LanguageCode, bool ExpectedSuccess, string ExpectedAlpha2)
            : TryCase<string, string>(Name, LanguageCode, ExpectedSuccess, ExpectedAlpha2);

        #endregion Cases
    }

    public static class TryParseAlpha3
    {
        private static Case V(string name, string? languageCode, bool expectedSuccess, string expectedAlpha3) => new(name, languageCode, expectedSuccess, expectedAlpha3);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("eng", "eng", true, "eng") },
            { V("ENG", "ENG", true, "ENG") },
            { V("trimmed", " fra ", true, "fra") },
            { V("trimmed tabs", "\tdeu\r\n", true, "deu") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("length 2", "en", false, string.Empty) },
            { V("length 4", "engl", false, string.Empty) },
            { V("digit", "en1", false, string.Empty) },
            { V("separator", "en-", false, string.Empty) },
            { V("digit prefix", "1en", false, string.Empty) },
            { V("trimmed invalid", " en1 ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? LanguageCode, bool ExpectedSuccess, string ExpectedAlpha3)
            : TryCase<string, string>(Name, LanguageCode, ExpectedSuccess, ExpectedAlpha3);

        #endregion Cases
    }
}
