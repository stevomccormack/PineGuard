using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoCountryUtilityTestData
{
    public static class TryParseAlpha2
    {
        private static Case V(string name, string? countryCode, bool expectedSuccess, string expectedAlpha2) => new(name, countryCode, expectedSuccess, expectedAlpha2);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("US", "US", true, "US") },
            { V("gb", "gb", true, "gb") },
            { V("trimmed lower", " fr ", true, "fr") },
            { V("trimmed upper", "\tDE\r\n", true, "DE") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("length 1", "U", false, string.Empty) },
            { V("length 3", "USA", false, string.Empty) },
            { V("digit", "U1", false, string.Empty) },
            { V("separator", "U-", false, string.Empty) },
            { V("digit prefix", "1U", false, string.Empty) },
            { V("trimmed invalid", " U1 ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? CountryCode, bool ExpectedSuccess, string ExpectedAlpha2)
            : TryCase<string, string>(Name, CountryCode, ExpectedSuccess, ExpectedAlpha2);

        #endregion Cases
    }

    public static class TryParseAlpha3
    {
        private static Case V(string name, string? countryCode, bool expectedSuccess, string expectedAlpha3) => new(name, countryCode, expectedSuccess, expectedAlpha3);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("USA", "USA", true, "USA") },
            { V("gbr", "gbr", true, "gbr") },
            { V("trimmed lower", " fra ", true, "fra") },
            { V("trimmed upper", "\tDEU\r\n", true, "DEU") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("length 2", "US", false, string.Empty) },
            { V("length 4", "USAA", false, string.Empty) },
            { V("digit", "US1", false, string.Empty) },
            { V("separator", "US-", false, string.Empty) },
            { V("digit prefix", "1US", false, string.Empty) },
            { V("trimmed invalid", " US1 ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? CountryCode, bool ExpectedSuccess, string ExpectedAlpha3)
            : TryCase<string, string>(Name, CountryCode, ExpectedSuccess, ExpectedAlpha3);

        #endregion Cases
    }

    public static class TryParseNumeric
    {
        private static Case V(string name, string? countryNumber, bool expectedSuccess, string expectedNumeric) => new(name, countryNumber, expectedSuccess, expectedNumeric);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("840", "840", true, "840") },
            { V("trimmed", " 826 ", true, "826") },
            { V("trimmed tabs", "\t250\r\n", true, "250") },
            { V("000", "000", true, "000") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("length 2", "84", false, string.Empty) },
            { V("length 4", "8400", false, string.Empty) },
            { V("alpha", "84A", false, string.Empty) },
            { V("separator", "84-", false, string.Empty) },
            { V("alpha prefix", "A84", false, string.Empty) },
            { V("trimmed invalid", " 84A ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? CountryNumber, bool ExpectedSuccess, string ExpectedNumeric)
            : TryCase<string, string>(Name, CountryNumber, ExpectedSuccess, ExpectedNumeric);

        #endregion Cases
    }
}
