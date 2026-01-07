using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iso.Currencies;

public static class IsoCurrencyTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("USD", alpha3: "USD", numeric: "840", decimalPlaces: 2, currencyName: "US Dollar"),
            V("eur lower", alpha3: "eur", numeric: "978", decimalPlaces: 2, currencyName: "Euro"),
            V("JPY", alpha3: "JPY", numeric: "392", decimalPlaces: 0, currencyName: "Yen"),
            V("BHD", alpha3: "BHD", numeric: "048", decimalPlaces: 3, currencyName: "Bahraini Dinar"),
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("Alpha3 too short", alpha3: "US", numeric: "840", decimalPlaces: 2, currencyName: "US Dollar", expectedParamName: "alpha3Code"),
            I("Alpha3 non-alpha", alpha3: "US1", numeric: "840", decimalPlaces: 2, currencyName: "US Dollar", expectedParamName: "alpha3Code"),
            I("Numeric too short", alpha3: "USD", numeric: "84", decimalPlaces: 2, currencyName: "US Dollar", expectedParamName: "numericCode"),
            I("Numeric non-digit", alpha3: "USD", numeric: "84A", decimalPlaces: 2, currencyName: "US Dollar", expectedParamName: "numericCode"),
            I("DecimalPlaces too low", alpha3: "USD", numeric: "840", decimalPlaces: -2, currencyName: "US Dollar", expectedParamName: "decimalPlaces", expectedExceptionType: typeof(ArgumentOutOfRangeException)),
            I("DecimalPlaces too high", alpha3: "USD", numeric: "840", decimalPlaces: 5, currencyName: "US Dollar", expectedParamName: "decimalPlaces", expectedExceptionType: typeof(ArgumentOutOfRangeException)),
            I("Name whitespace", alpha3: "USD", numeric: "840", decimalPlaces: 2, currencyName: " ", expectedParamName: "name"),
        };

        private static ValidCase V(string name, string alpha3, string numeric, int decimalPlaces, string currencyName)
            => new(name, alpha3, numeric, decimalPlaces, currencyName);

        private static InvalidCase I(
            string name,
            string alpha3,
            string numeric,
            int decimalPlaces,
            string currencyName,
            string expectedParamName,
            Type? expectedExceptionType = null)
            => new(
                name,
                alpha3,
                numeric,
                decimalPlaces,
                currencyName,
                ExpectedException: new ExpectedException(expectedExceptionType ?? typeof(ArgumentException), ParamName: expectedParamName));

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Alpha3, string Numeric, int DecimalPlaces, string CurrencyName) : Case(Name);

        public sealed record InvalidCase(
            string Name,
            string Alpha3,
            string Numeric,
            int DecimalPlaces,
            string CurrencyName,
            ExpectedException ExpectedException) : Case(Name);

        #endregion
    }

    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("Alpha3 with spaces", input: " usd ", expectedAlpha3: "USD"),
            V("Numeric EUR", input: "978", expectedAlpha3: "EUR"),
            V("Numeric JPY", input: "392", expectedAlpha3: "JPY"),
        };

        public static TheoryData<EdgeCase> EdgeCases => new()
        {
            E("Null", value: null, expected: false),
            E("Empty", value: string.Empty, expected: false),
            E("Space", value: " ", expected: false),
            E("Tab", value: "\t", expected: false),
            E("Not a code", value: "not-a-code", expected: false),
        };

        private static ValidCase V(string name, string input, string expectedAlpha3) => new(name, input, expectedAlpha3);

        private static EdgeCase E(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Input, string ExpectedAlpha3) : Case(Name);

        public sealed record EdgeCase(string Name, string? Value, bool Expected) : Case(Name);

        #endregion
    }

    public static class Parse
    {
        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("Null", value: null),
            I("Empty", value: string.Empty),
            I("Space", value: " "),
            I("Tab", value: "\t"),
            I("Not a code", value: "not-a-code"),
        };

        private static InvalidCase I(string name, string? value)
            => new(name, value, ExpectedException: new ExpectedException(typeof(FormatException), MessageContains: "ISO"));

        #region Cases

        public abstract record Case(string Name);

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException) : Case(Name);

        #endregion
    }
}
