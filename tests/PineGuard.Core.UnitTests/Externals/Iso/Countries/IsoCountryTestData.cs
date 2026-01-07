using PineGuard.Testing;

namespace PineGuard.Core.UnitTests.Externals.Iso.Countries;

public static class IsoCountryTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("US", alpha2: "US", alpha3: "USA", numeric: "840", countryName: "United States"),
            V("FR", alpha2: "fr", alpha3: "FRA", numeric: "250", countryName: "France"),
            V("JP", alpha2: "jp", alpha3: "JPN", numeric: "392", countryName: "Japan"),
            V("CA", alpha2: "ca", alpha3: "CAN", numeric: "124", countryName: "Canada"),
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            V("GB lowercase", alpha2: "gb", alpha3: "gbr", numeric: "826", countryName: "United Kingdom"),
        };

        public static TheoryData<InvalidCase> InvalidCases => new()
        {
            I("Alpha2 too short", alpha2: "U", alpha3: "USA", numeric: "840", nameValue: "United States", expectedParamName: "alpha2Code"),
            I("Alpha2 too long", alpha2: "USA", alpha3: "USA", numeric: "840", nameValue: "United States", expectedParamName: "alpha2Code"),
            I("Alpha2 non-alpha", alpha2: "U1", alpha3: "USA", numeric: "840", nameValue: "United States", expectedParamName: "alpha2Code"),
            I("Alpha2 has surrounding whitespace", alpha2: "  us ", alpha3: "usa", numeric: "840", nameValue: "United States", expectedParamName: "alpha2Code"),
            I("Alpha2 tab-wrapped", alpha2: "\tGB\t", alpha3: "GBR", numeric: "826", nameValue: "United Kingdom", expectedParamName: "alpha2Code"),
            I("Alpha3 too short", alpha2: "US", alpha3: "US", numeric: "840", nameValue: "United States", expectedParamName: "alpha3Code"),
            I("Alpha3 non-alpha", alpha2: "US", alpha3: "US1", numeric: "840", nameValue: "United States", expectedParamName: "alpha3Code"),
            I("Alpha3 too long", alpha2: "US", alpha3: "USAA", numeric: "840", nameValue: "United States", expectedParamName: "alpha3Code"),
            I("Numeric too short", alpha2: "US", alpha3: "USA", numeric: "84", nameValue: "United States", expectedParamName: "numericCode"),
            I("Numeric non-digit", alpha2: "US", alpha3: "USA", numeric: "84A", nameValue: "United States", expectedParamName: "numericCode"),
            I("Numeric too long", alpha2: "US", alpha3: "USA", numeric: "8400", nameValue: "United States", expectedParamName: "numericCode"),
            I("Name whitespace", alpha2: "US", alpha3: "USA", numeric: "840", nameValue: " ", expectedParamName: "name"),
        };

        private static ValidCase V(string name, string alpha2, string alpha3, string numeric, string countryName)
            => new(name, alpha2, alpha3, numeric, countryName);

        private static InvalidCase I(string name, string alpha2, string alpha3, string numeric, string nameValue, string expectedParamName)
            => new(
                name,
                alpha2,
                alpha3,
                numeric,
                nameValue,
                ExpectedException: new ExpectedException(typeof(ArgumentException), ParamName: expectedParamName));

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Alpha2, string Alpha3, string Numeric, string CountryName) : Case(Name);

        public sealed record InvalidCase(
            string Name,
            string Alpha2,
            string Alpha3,
            string Numeric,
            string CountryName,
            ExpectedException ExpectedException) : Case(Name);

        #endregion
    }

    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases => new()
        {
            V("US", alpha2: "US", alpha3: "USA", numeric: "840"),
            V("GB", alpha2: "gb", alpha3: "gbr", numeric: "826"),
        };

        public static TheoryData<EdgeCase> EdgeCases => new()
        {
            E("Null", value: null, expected: false),
            E("Empty", value: string.Empty, expected: false),
            E("Space", value: " ", expected: false),
            E("Tab", value: "\t", expected: false),
            E("Not a code", value: "not-a-code", expected: false),
        };

        private static ValidCase V(string name, string alpha2, string alpha3, string numeric) => new(name, alpha2, alpha3, numeric);

        private static EdgeCase E(string name, string? value, bool expected) => new(name, value, expected);

        #region Cases

        public abstract record Case(string Name);

        public sealed record ValidCase(string Name, string Alpha2, string Alpha3, string Numeric) : Case(Name);

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
