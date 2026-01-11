using PineGuard.Externals.Iso.Countries;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Countries;

public static class DefaultIsoCountryProviderTestData
{
    public static class ContainsAlpha2Code
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("US", "US", true),
            new("GB", "GB", true),
            new("FR", "FR", true),
            new("CA", "CA", true),
            new("JP", "JP", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Whitespace", "\t\r\n", false),
            new("ZZ", "ZZ", false),
            new("AA", "AA", false),
            new("U1", "U1", false),
            new("USA", "USA", false),
            new("8400", "8400", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class ContainsAlpha3Code
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("USA", "USA", true),
            new("GBR", "GBR", true),
            new("FRA", "FRA", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Whitespace", "\t\r\n", false),
            new("US1", "US1", false),
            new("U$A", "U$A", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class ContainsNumericCode
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("840", "840", true),
            new("826", "826", true),
            new("250", "250", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Whitespace", "\t\r\n", false),
            new("84A", "84A", false),
            new("84-", "84-", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class TryGetByCodes
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("US", (Alpha2: "US", Alpha3: "USA", Numeric: "840")),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("GB lowercase", (Alpha2: "gb", Alpha3: "gbr", Numeric: "826")),
        ];

        public sealed record ValidCase(string Name, (string Alpha2, string Alpha3, string Numeric) Value)
            : ValueCase<(string Alpha2, string Alpha3, string Numeric)>(Name, Value);
    }

    public static class TryGetByAlpha2Code
    {
        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("Null", null),
            new("Empty", string.Empty),
            new("Space", " "),
            new("Tab", "\t"),
            new("Has whitespace", " US "),
            new("U1", "U1"),
            new("U-", "U-"),
            new("??", "??"),
        ];

        public sealed record EdgeCase(string Name, string? Value)
            : TryCase<string?, IsoCountry?>(Name, Value, false, null);
    }

    public static class TryGetByAlpha3Code
    {
        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("Null", null),
            new("Empty", string.Empty),
            new("Space", " "),
            new("Whitespace", "\t\r\n"),
            new("US1", "US1"),
            new("U$A", "U$A"),
            new("???", "???"),
        ];

        public sealed record EdgeCase(string Name, string? Value)
            : TryCase<string?, IsoCountry?>(Name, Value, false, null);
    }

    public static class TryGetByNumericCode
    {
        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("Null", null),
            new("Empty", string.Empty),
            new("Space", " "),
            new("Whitespace", "\t\r\n"),
            new("84A", "84A"),
            new("84-", "84-"),
            new("???", "???"),
        ];

        #region Case Records

        public sealed record EdgeCase(string Name, string? Value)
            : TryCase<string?, IsoCountry?>(Name, Value, false, null);

        #endregion
    }

    public static class TryGet
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
           new("By alpha3", "USA", true, "US"),
           new("By alpha2", "US", true, "US"),
           new("By numeric", "840", true, "US"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, null),
            new("Empty", string.Empty, false, null),
            new("Space", " ", false, null),
            new("Whitespace", "\t\r\n", false, null),
            new("Not found", "US1", false, null),
        ];

        #region Case Records

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string? ExpectedAlpha2)
            : TryCase<string?, IsoCountry?>(Name, Value, ExpectedReturn, ExpectedReturn
                    ? new IsoCountry(alpha2Code: ExpectedAlpha2!, alpha3Code: "USA", numericCode: "840", name: "United States")
                    : null);

        #endregion
    }
}
