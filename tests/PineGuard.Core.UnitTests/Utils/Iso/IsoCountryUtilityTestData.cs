using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoCountryUtilityTestData
{
    public static class TryParseAlpha2
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("US", "US", true, "US"),
            new("gb", "gb", true, "gb"),
            new("trimmed lower", " fr ", true, "fr"),
            new("trimmed upper", "\tDE\r\n", true, "DE"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("length 1", "U", false, string.Empty),
            new("length 3", "USA", false, string.Empty),
            new("digit", "U1", false, string.Empty),
            new("separator", "U-", false, string.Empty),
            new("digit prefix", "1U", false, string.Empty),
            new("trimmed invalid", " U1 ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? CountryCode, bool ExpectedSuccess, string ExpectedAlpha2)
            : TryCase<string, string>(Name, CountryCode, ExpectedSuccess, ExpectedAlpha2);

        #endregion Cases
    }

    public static class TryParseAlpha3
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("USA", "USA", true, "USA"),
            new("gbr", "gbr", true, "gbr"),
            new("trimmed lower", " fra ", true, "fra"),
            new("trimmed upper", "\tDEU\r\n", true, "DEU"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("length 2", "US", false, string.Empty),
            new("length 4", "USAA", false, string.Empty),
            new("digit", "US1", false, string.Empty),
            new("separator", "US-", false, string.Empty),
            new("digit prefix", "1US", false, string.Empty),
            new("trimmed invalid", " US1 ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? CountryCode, bool ExpectedSuccess, string ExpectedAlpha3)
            : TryCase<string, string>(Name, CountryCode, ExpectedSuccess, ExpectedAlpha3);

        #endregion Cases
    }

    public static class TryParseNumeric
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("840", "840", true, "840"),
            new("trimmed", " 826 ", true, "826"),
            new("trimmed tabs", "\t250\r\n", true, "250"),
            new("000", "000", true, "000"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("length 2", "84", false, string.Empty),
            new("length 4", "8400", false, string.Empty),
            new("alpha", "84A", false, string.Empty),
            new("separator", "84-", false, string.Empty),
            new("alpha prefix", "A84", false, string.Empty),
            new("trimmed invalid", " 84A ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? CountryNumber, bool ExpectedSuccess, string ExpectedNumeric)
            : TryCase<string, string>(Name, CountryNumber, ExpectedSuccess, ExpectedNumeric);

        #endregion Cases
    }
}
