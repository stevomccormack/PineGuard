using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CultureInfoUtilityTestData
{
    private const string InvalidCultureName = "no-such-culture";

    public static class TryGetCultureName
    {
        private static ValidCase V(string name, string? isoLanguageAlpha2Code, string? regionCode, bool expected, string expectedCultureName)
            => new(name, isoLanguageAlpha2Code, regionCode, expected, expectedCultureName);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("language only en", "en", null, true, "en") },
            { V("language+region en-US", "en", "US", true, "en-US") },
            { V("language only fr", "fr", null, true, "fr") },
            { V("language only de", "de", null, true, "de") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null null", null, null, false, string.Empty) },
            { V("empty null", string.Empty, null, false, string.Empty) },
            { V("space null", " ", null, false, string.Empty) },
            { V("whitespace null", "\t\r\n", null, false, string.Empty) },
            { V("invalid language", InvalidCultureName, null, false, string.Empty) },
            { V("invalid language with region", InvalidCultureName, "US", false, string.Empty) },
            { V("invalid language with invalid region", InvalidCultureName, "ZZ", false, string.Empty) },
            { V("trim language", " en ", null, true, "en") },
            { V("trim language and region", " en ", " US ", true, "en-US") },
            { V("trim region", "en", " US ", true, "en-US") },
            { V("whitespace region ignored", "en", "   ", true, "en") },
            { V("empty region ignored", "en", "", true, "en") },
            { V("whitespace region ignored 2", "en", "\t\r\n", true, "en") },
            { V("empty language with region", "", "US", false, string.Empty) },
            { V("space language with region", " ", "US", false, string.Empty) },
            { V("invalid language empty region", InvalidCultureName, "", false, string.Empty) },
            { V("invalid language space region", InvalidCultureName, " ", false, string.Empty) },
            { V("invalid language whitespace region", InvalidCultureName, "\t\r\n", false, string.Empty) },
            { V("invalid language padded region", InvalidCultureName, " US ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, string? RegionCode, bool Expected, string ExpectedCultureName)
            : Case(Name);

        #endregion
    }

    public static class TryGetCultureNameWithoutRegionOverload
    {
        private static ValidCase V(string name, string? isoLanguageAlpha2Code, bool expected, string expectedCultureName)
            => new(name, isoLanguageAlpha2Code, expected, expectedCultureName);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("en", "en", true, "en") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("invalid padded", " no-such-culture ", false, string.Empty) },
            { V("null", null, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool Expected, string ExpectedCultureName)
            : Case(Name);

        #endregion
    }

    public static class TryGetCultureNameWithDefaultRegion
    {
        private static ValidCase V(string name, string? isoLanguageAlpha2Code, bool expected, string expectedCultureName)
            => new(name, isoLanguageAlpha2Code, expected, expectedCultureName);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("en", "en", true, "en-US") },
            { V("pt", "pt", true, "pt-BR") },
            { V("es", "es", true, "es-ES") },
            { V("fr", "fr", true, "fr-FR") },
            { V("de", "de", true, "de-DE") },
            { V("zh", "zh", true, "zh-CN") },
            // No configured default region; should fall back to language-only.
            { V("it fallback", "it", true, "it") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("invalid", InvalidCultureName, false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool Expected, string ExpectedCultureName)
            : Case(Name);

        #endregion
    }

    public static class GetCultures
    {
        private static ValidCase V(string name, string? isoLanguageAlpha2Code) => new(name, isoLanguageAlpha2Code);

        public static TheoryData<ValidCase> ValidCases => [];

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null) },
            { V("empty", string.Empty) },
            { V("space", " ") },
            { V("two spaces", "  ") },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public abstract record Case(string Name) : BaseCase(Name);

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code)
            : Case(Name);

        #endregion
    }
}
