using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoLanguageUtilityTestData
{
    public static class TryParseAlpha2
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("en", "en", true, "en"),
            new("EN", "EN", true, "EN"),
            new("trimmed", " fr ", true, "fr"),
            new("trimmed tabs", "\tde\r\n", true, "de"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("length 1", "e", false, string.Empty),
            new("length 3", "eng", false, string.Empty),
            new("digit", "e1", false, string.Empty),
            new("separator", "e-", false, string.Empty),
            new("digit prefix", "1e", false, string.Empty),
            new("trimmed invalid", " e1 ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? LanguageCode, bool ExpectedSuccess, string ExpectedAlpha2)
            : TryCase<string, string>(Name, LanguageCode, ExpectedSuccess, ExpectedAlpha2);

        #endregion Cases
    }

    public static class TryParseAlpha3
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("eng", "eng", true, "eng"),
            new("ENG", "ENG", true, "ENG"),
            new("trimmed", " fra ", true, "fra"),
            new("trimmed tabs", "\tdeu\r\n", true, "deu"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("length 2", "en", false, string.Empty),
            new("length 4", "engl", false, string.Empty),
            new("digit", "en1", false, string.Empty),
            new("separator", "en-", false, string.Empty),
            new("digit prefix", "1en", false, string.Empty),
            new("trimmed invalid", " en1 ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? LanguageCode, bool ExpectedSuccess, string ExpectedAlpha3)
            : TryCase<string, string>(Name, LanguageCode, ExpectedSuccess, ExpectedAlpha3);

        #endregion Cases
    }
}
