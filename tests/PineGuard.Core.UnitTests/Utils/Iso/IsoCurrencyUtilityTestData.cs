using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Utils.Iso;

public static class IsoCurrencyUtilityTestData
{
    public static class TryParseAlpha3
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("usd", "usd", true, "usd"),
            new("USD", "USD", true, "USD"),
            new("trimmed", " eur ", true, "eur"),
            new("trimmed tabs", "\tgbp\r\n", true, "gbp"),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false, string.Empty),
            new("empty", string.Empty, false, string.Empty),
            new("space", " ", false, string.Empty),
            new("whitespace", "\t\r\n", false, string.Empty),
            new("length 2", "us", false, string.Empty),
            new("length 4", "usdd", false, string.Empty),
            new("digit", "us1", false, string.Empty),
            new("symbol", "u$d", false, string.Empty),
            new("digit prefix", "1us", false, string.Empty),
            new("trimmed invalid", " us1 ", false, string.Empty),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record Case(string Name, string? CurrencyCode, bool ExpectedSuccess, string ExpectedAlpha3)
            : TryCase<string, string>(Name, CurrencyCode, ExpectedSuccess, ExpectedAlpha3);

        #endregion Cases
    }

    public static class TryParseNumeric
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("840", "840", true, "840"),
            new("trimmed", " 826 ", true, "826"),
            new("trimmed tabs", "\t978\r\n", true, "978"),
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

        public sealed record Case(string Name, string? CurrencyNumber, bool ExpectedSuccess, string ExpectedNumeric)
            : TryCase<string, string>(Name, CurrencyNumber, ExpectedSuccess, ExpectedNumeric);

        #endregion Cases
    }
}
