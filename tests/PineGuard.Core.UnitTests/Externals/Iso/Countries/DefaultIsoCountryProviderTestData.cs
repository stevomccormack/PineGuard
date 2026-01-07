namespace PineGuard.Core.UnitTests.Externals.Iso.Countries;

public static class DefaultIsoCountryProviderTestData
{
    public static class ContainsAlpha2Code
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("US", value: "US", expected: true),
            V("GB", value: "GB", expected: true),
            V("FR", value: "FR", expected: true),
            V("CA", value: "CA", expected: true),
            V("JP", value: "JP", expected: true),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Whitespace", value: "\t\r\n", expected: false),
            V("ZZ", value: "ZZ", expected: false),
            V("AA", value: "AA", expected: false),
            V("U1", value: "U1", expected: false),
            V("USA", value: "USA", expected: false),
            V("8400", value: "8400", expected: false),
        };

        private static ValidCase V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class ContainsAlpha3Code
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("USA", value: "USA", expected: true),
            V("GBR", value: "GBR", expected: true),
            V("FRA", value: "FRA", expected: true),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Whitespace", value: "\t\r\n", expected: false),
            V("US1", value: "US1", expected: false),
            V("U$A", value: "U$A", expected: false),
        };

        private static ValidCase V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class ContainsNumericCode
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("840", value: "840", expected: true),
            V("826", value: "826", expected: true),
            V("250", value: "250", expected: true),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Whitespace", value: "\t\r\n", expected: false),
            V("84A", value: "84A", expected: false),
            V("84-", value: "84-", expected: false),
        };

        private static ValidCase V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class TryGetByCodes
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("US", alpha2: "US", alpha3: "USA", numeric: "840"),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("GB lowercase", alpha2: "gb", alpha3: "gbr", numeric: "826"),
        };

        private static ValidCase V(string name, string alpha2, string alpha3, string numeric) => new(name, alpha2, alpha3, numeric);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Alpha2, string Alpha3, string Numeric) : Case(Name);

        #endregion
    }

    public static class TryGetByAlpha2Code
    {
        public static TheoryData<EdgeCase> EdgeCases => new()
        {
            E("Null", value: null),
            E("Empty", value: string.Empty),
            E("Space", value: " "),
            E("Tab", value: "\t"),
            E("Has whitespace", value: " US "),
            E("U1", value: "U1"),
            E("U-", value: "U-"),
            E("??", value: "??"),
        };

        private static EdgeCase E(string name, string? value) => new(name, value, Expected: false);

        #region Cases

        public abstract record Case(string Name);

        public sealed record EdgeCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class TryGetByAlpha3Code
    {
        public static TheoryData<EdgeCase> EdgeCases => new()
        {
            E("Null", value: null),
            E("Empty", value: string.Empty),
            E("Space", value: " "),
            E("Whitespace", value: "\t\r\n"),
            E("US1", value: "US1"),
            E("U$A", value: "U$A"),
            E("???", value: "???"),
        };

        private static EdgeCase E(string name, string? value) => new(name, value, Expected: false);

        #region Cases

        public abstract record Case(string Name);

        public sealed record EdgeCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class TryGetByNumericCode
    {
        public static TheoryData<EdgeCase> EdgeCases => new()
        {
            E("Null", value: null),
            E("Empty", value: string.Empty),
            E("Space", value: " "),
            E("Whitespace", value: "\t\r\n"),
            E("84A", value: "84A"),
            E("84-", value: "84-"),
            E("???", value: "???"),
        };

        private static EdgeCase E(string name, string? value) => new(name, value, Expected: false);

        #region Cases

        public abstract record Case(string Name);

        public sealed record EdgeCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class TryGet
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("By alpha3", value: "USA", expected: true, expectedAlpha2: "US"),
            V("By alpha2", value: "US", expected: true, expectedAlpha2: "US"),
            V("By numeric", value: "840", expected: true, expectedAlpha2: "US"),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false, expectedAlpha2: null),
            V("Empty", value: string.Empty, expected: false, expectedAlpha2: null),
            V("Space", value: " ", expected: false, expectedAlpha2: null),
            V("Whitespace", value: "\t\r\n", expected: false, expectedAlpha2: null),
            V("Not found", value: "US1", expected: false, expectedAlpha2: null),
        };

        private static ValidCase V(string name, string? value, bool expected, string? expectedAlpha2) => new(name, value, expected, expectedAlpha2);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string? ExpectedAlpha2) : Case(Name);

        #endregion
    }
}
