using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoCurrencyUtilityTestData
{
    public static class TryParseAlpha3
    {
        private static Case V(string name, string? currencyCode, bool expectedSuccess, string expectedAlpha3) => new(name, currencyCode, expectedSuccess, expectedAlpha3);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("usd", "usd", true, "usd") },
            { V("USD", "USD", true, "USD") },
            { V("trimmed", " eur ", true, "eur") },
            { V("trimmed tabs", "\tgbp\r\n", true, "gbp") },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { V("null", null, false, string.Empty) },
            { V("empty", string.Empty, false, string.Empty) },
            { V("space", " ", false, string.Empty) },
            { V("whitespace", "\t\r\n", false, string.Empty) },
            { V("length 2", "us", false, string.Empty) },
            { V("length 4", "usdd", false, string.Empty) },
            { V("digit", "us1", false, string.Empty) },
            { V("symbol", "u$d", false, string.Empty) },
            { V("digit prefix", "1us", false, string.Empty) },
            { V("trimmed invalid", " us1 ", false, string.Empty) },
        };

        public static TheoryData<ThrowsCase> InvalidCases => [];

        #region Cases

        public sealed record Case(string Name, string? CurrencyCode, bool ExpectedSuccess, string ExpectedAlpha3)
            : TryCase<string, string>(Name, CurrencyCode, ExpectedSuccess, ExpectedAlpha3);

        #endregion Cases
    }

    public static class TryParseNumeric
    {
        private static Case V(string name, string? currencyNumber, bool expectedSuccess, string expectedNumeric) => new(name, currencyNumber, expectedSuccess, expectedNumeric);

        public static TheoryData<Case> ValidCases => new()
        {
            { V("840", "840", true, "840") },
            { V("trimmed", " 826 ", true, "826") },
            { V("trimmed tabs", "\t978\r\n", true, "978") },
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

        public sealed record Case(string Name, string? CurrencyNumber, bool ExpectedSuccess, string ExpectedNumeric)
            : TryCase<string, string>(Name, CurrencyNumber, ExpectedSuccess, ExpectedNumeric);

        #endregion Cases
    }
}
