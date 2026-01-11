using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils;

public static class CultureInfoUtilityTestData
{
    private const string InvalidCultureName = "no-such-culture";

    public static class TryGetCultureName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("language only en", "en", null, true, "en"),
            new("language+region en-US", "en", "US", true, "en-US"),
            new("language only fr", "fr", null, true, "fr"),
            new("language only de", "de", null, true, "de"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null null", null, null, false, string.Empty),
            new("empty null", string.Empty, null, false, string.Empty),
            new("space null", " ", null, false, string.Empty),
            new("whitespace null", "\t\r\n", null, false, string.Empty),
            new("invalid language", InvalidCultureName, null, false, string.Empty),
            new("invalid language with region", InvalidCultureName, "US", false, string.Empty),
            new("invalid language with invalid region", InvalidCultureName, "ZZ", false, string.Empty),
            new("trim language", " en ", null, true, "en"),
            new("trim language and region", " en ", " US ", true, "en-US"),
            new("trim region", "en", " US ", true, "en-US"),
            new("whitespace region ignored", "en", "   ", true, "en"),
            new("empty region ignored", "en", "", true, "en"),
            new("whitespace region ignored 2", "en", "\t\r\n", true, "en"),
            new("empty language with region", "", "US", false, string.Empty),
            new("space language with region", " ", "US", false, string.Empty),
            new("invalid language empty region", InvalidCultureName, "", false, string.Empty),
            new("invalid language space region", InvalidCultureName, " ", false, string.Empty),
            new("invalid language whitespace region", InvalidCultureName, "\t\r\n", false, string.Empty),
            new("invalid language padded region", InvalidCultureName, " US ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(
            string Name,
            string? IsoLanguageAlpha2Code,
            string? RegionCode,
            bool ExpectedReturn,
            string ExpectedOutValue)
            : TryCase<(string? IsoLanguageAlpha2Code, string? RegionCode), string>(
                Name,
                (IsoLanguageAlpha2Code, RegionCode),
                ExpectedReturn,
                ExpectedOutValue);

        #endregion
    }

    public static class TryGetCultureNameWithoutRegionOverload
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en", "en", true, "en"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid padded", " no-such-culture ", false, string.Empty),
            new("null", null, false, string.Empty),
            new("space", " ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, IsoLanguageAlpha2Code, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class TryGetCultureNameWithDefaultRegion
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en", "en", true, "en-US"),
            new("pt", "pt", true, "pt-BR"),
            new("es", "es", true, "es-ES"),
            new("fr", "fr", true, "fr-FR"),
            new("de", "de", true, "de-DE"),
            new("zh", "zh", true, "zh-CN"),
            // No configured default region; should fall back to language-only.
            new("it fallback", "it", true, "it"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("invalid", InvalidCultureName, false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool ExpectedReturn, string ExpectedOutValue)
            : TryCase<string?, string>(Name, IsoLanguageAlpha2Code, ExpectedReturn, ExpectedOutValue);

        #endregion
    }

    public static class GetCultures
    {
        public static TheoryData<ValidCase> ValidCases => [];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null),
            new("empty", string.Empty),
            new("space", " "),
            new("two spaces", "  "),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code)
            : ReturnCase<string?, int>(Name, IsoLanguageAlpha2Code, 0);

        #endregion
    }
}
