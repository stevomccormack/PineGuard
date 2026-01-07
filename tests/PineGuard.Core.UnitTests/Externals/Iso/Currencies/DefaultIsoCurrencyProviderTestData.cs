namespace PineGuard.Core.UnitTests.Externals.Iso.Currencies;

public static class DefaultIsoCurrencyProviderTestData
{
    public static class TryGet
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("USD", value: "USD", expected: true, expectedAlpha3: "USD"),
            V("EUR", value: "EUR", expected: true, expectedAlpha3: "EUR"),
            V("JPY", value: "JPY", expected: true, expectedAlpha3: "JPY"),
            V("usd lowercase", value: "usd", expected: true, expectedAlpha3: "USD"),
            V("840 numeric", value: "840", expected: true, expectedAlpha3: "USD"),
            V("978 numeric", value: "978", expected: true, expectedAlpha3: "EUR"),
            V("392 numeric", value: "392", expected: true, expectedAlpha3: "JPY"),
            V("999 numeric", value: "999", expected: true, expectedAlpha3: "XXX"),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false, expectedAlpha3: null),
            V("Empty", value: string.Empty, expected: false, expectedAlpha3: null),
            V("Space", value: " ", expected: false, expectedAlpha3: null),
            V("Tab", value: "\t", expected: false, expectedAlpha3: null),
            V("Newline", value: "\r\n", expected: false, expectedAlpha3: null),
            V("US", value: "US", expected: false, expectedAlpha3: null),
            V("EURO", value: "EURO", expected: false, expectedAlpha3: null),
            V("84", value: "84", expected: false, expectedAlpha3: null),
            V("ZZZ", value: "ZZZ", expected: false, expectedAlpha3: null),
            V("USD with whitespace", value: " USD ", expected: false, expectedAlpha3: null),
            V("840 with whitespace", value: " 840 ", expected: false, expectedAlpha3: null),
        };

        private static ValidCase V(string name, string? value, bool expected, string? expectedAlpha3)
            => new(name, value, expected, expectedAlpha3);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string? ExpectedAlpha3) : Case(Name);

        #endregion
    }

    public static class ContainsAlpha3Code
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("USD", value: "USD", expected: true),
            V("EUR", value: "EUR", expected: true),
            V("usd lowercase", value: "usd", expected: true),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Tab", value: "\t", expected: false),
            V("Newline", value: "\r\n", expected: false),
            V("US", value: "US", expected: false),
            V("EURO", value: "EURO", expected: false),
            V("84", value: "84", expected: false),
            V("ZZZ", value: "ZZZ", expected: false),
            V("999", value: "999", expected: false),
            V("USD with whitespace", value: " USD ", expected: false),
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
            V("978", value: "978", expected: true),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false),
            V("Empty", value: string.Empty, expected: false),
            V("Space", value: " ", expected: false),
            V("Tab", value: "\t", expected: false),
            V("Newline", value: "\r\n", expected: false),
            V("84", value: "84", expected: false),
            V("84A", value: "84A", expected: false),
            V("840 with whitespace", value: " 840 ", expected: false),
        };

        private static ValidCase V(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class TryGetByAlpha3Code
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("USD", value: "USD", expected: true, expectedAlpha3: "USD"),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false, expectedAlpha3: null),
            V("Empty", value: string.Empty, expected: false, expectedAlpha3: null),
            V("Space", value: " ", expected: false, expectedAlpha3: null),
            V("Tab", value: "\t", expected: false, expectedAlpha3: null),
            V("Newline", value: "\r\n", expected: false, expectedAlpha3: null),
            V("ZZZ", value: "ZZZ", expected: false, expectedAlpha3: null),
            V("US1", value: "US1", expected: false, expectedAlpha3: null),
            V("U$D", value: "U$D", expected: false, expectedAlpha3: null),
            V("USD with whitespace", value: " USD ", expected: false, expectedAlpha3: null),
        };

        private static ValidCase V(string name, string? value, bool expected, string? expectedAlpha3) => new(name, value, expected, expectedAlpha3);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string? ExpectedAlpha3) : Case(Name);

        #endregion
    }

    public static class TryGetByNumericCode
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("840", value: "840", expected: true, expectedNumeric: "840"),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("Null", value: null, expected: false, expectedNumeric: null),
            V("Empty", value: string.Empty, expected: false, expectedNumeric: null),
            V("Space", value: " ", expected: false, expectedNumeric: null),
            V("Tab", value: "\t", expected: false, expectedNumeric: null),
            V("Newline", value: "\r\n", expected: false, expectedNumeric: null),
            V("84", value: "84", expected: false, expectedNumeric: null),
            V("84A", value: "84A", expected: false, expectedNumeric: null),
            V("840 with whitespace", value: " 840 ", expected: false, expectedNumeric: null),
        };

        private static ValidCase V(string name, string? value, bool expected, string? expectedNumeric) => new(name, value, expected, expectedNumeric);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string? Value, bool Expected, string? ExpectedNumeric) : Case(Name);

        #endregion
    }
}
