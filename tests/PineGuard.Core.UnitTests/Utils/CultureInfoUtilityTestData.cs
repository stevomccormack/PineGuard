using System.Globalization;
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
            new("trim language", " en ", null, true, "en"),
            new("trim language and region", " en ", " US ", true, "en-US"),
            new("trim region", "en", " US ", true, "en-US"),
            new("whitespace region ignored", "en", "   ", true, "en"),
            new("empty region ignored", "en", "", true, "en"),
            new("whitespace region ignored 2", "en", "\t\r\n", true, "en")
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
            new("empty language with region", "", "US", false, string.Empty),
            new("space language with region", " ", "US", false, string.Empty),
            new("invalid language empty region", InvalidCultureName, "", false, string.Empty),
            new("invalid language space region", InvalidCultureName, " ", false, string.Empty),
            new("invalid language whitespace region", InvalidCultureName, "\t\r\n", false, string.Empty),
            new("invalid language padded region", InvalidCultureName, " US ", false, string.Empty)
        ];

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, string? RegionCode, bool Expected, string ExpectedOutValue)
            : TryCase<(string? IsoLanguageAlpha2Code, string? RegionCode), string>(Name, (IsoLanguageAlpha2Code, RegionCode), Expected, ExpectedOutValue);
    }

    public static class TryGetCultureNameWithoutRegionOverload
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en", "en", true, "en")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid padded", " no-such-culture ", false, string.Empty),
            new("null", null, false, string.Empty),
            new("space", " ", false, string.Empty)
        ];

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string>(Name, IsoLanguageAlpha2Code, Expected, ExpectedOutValue);
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
            new("it fallback", "it", true, "it")
        ];

        // No configured default region; should fall back to language-only.

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("invalid", InvalidCultureName, false, string.Empty)
        ];

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool Expected, string ExpectedOutValue)
            : TryCase<string?, string>(Name, IsoLanguageAlpha2Code, Expected, ExpectedOutValue);
    }

    public static class TryGetCultureInfo
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en-US", "en", "US", true, "en-US")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid", InvalidCultureName, null, false, null)
        ];

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, string? RegionCode, bool Expected, string? ExpectedCultureName)
            : TryCase<(string? IsoLanguageAlpha2Code, string? RegionCode), string?>(Name, (IsoLanguageAlpha2Code, RegionCode), Expected, ExpectedCultureName);
    }

    public static class GetRegionCodes
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en", "en", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid", "zz", false),
            new("null", null, false),
            new("empty", "", false),
            new("whitespace", "  ", false)
        ];

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool ExpectedNonEmpty)
            : ReturnCase<string?, bool>(Name, IsoLanguageAlpha2Code, ExpectedNonEmpty);
    }

    public static class GetCultures
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en", "en", true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invalid", "zz", false),
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("two spaces", "  ", false)
        ];

        public sealed record ValidCase(string Name, string? IsoLanguageAlpha2Code, bool ExpectedNonEmpty)
            : ReturnCase<string?, bool>(Name, IsoLanguageAlpha2Code, ExpectedNonEmpty);
    }

    public static class TryGetTwoLetterIsoRegionName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en-US", CultureInfo.GetCultureInfo("en-US"), true, "US")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invariant", CultureInfo.InvariantCulture, false, string.Empty)
        ];

        public sealed record ValidCase(string Name, CultureInfo Culture, bool Expected, string ExpectedRegion)
            : TryCase<CultureInfo, string>(Name, Culture, Expected, ExpectedRegion);
    }

    public static class AddRegionCode
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("en-US", CultureInfo.GetCultureInfo("en-US"), "US")
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("invariant", CultureInfo.InvariantCulture, null)
        ];

        public sealed record ValidCase(string Name, CultureInfo Culture, string? Expected)
            : ReturnCase<CultureInfo, string?>(Name, Culture, Expected);
    }
}
