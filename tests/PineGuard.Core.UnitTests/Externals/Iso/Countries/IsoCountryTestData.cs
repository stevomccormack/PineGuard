using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Countries;

public static class IsoCountryTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("US", (Alpha2: "US", Alpha3: "USA", Numeric: "840", CountryName: "United States")),
            new("FR", (Alpha2: "fr", Alpha3: "FRA", Numeric: "250", CountryName: "France")),
            new("JP", (Alpha2: "jp", Alpha3: "JPN", Numeric: "392", CountryName: "Japan")),
            new("CA", (Alpha2: "ca", Alpha3: "CAN", Numeric: "124", CountryName: "Canada")),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("GB lowercase", (Alpha2: "gb", Alpha3: "gbr", Numeric: "826", CountryName: "United Kingdom")),
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Alpha2 too short", (Alpha2: "U", Alpha3: "USA", Numeric: "840", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "alpha2Code")),
            new("Alpha2 too long", (Alpha2: "USA", Alpha3: "USA", Numeric: "840", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "alpha2Code")),
            new("Alpha2 non-alpha", (Alpha2: "U1", Alpha3: "USA", Numeric: "840", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "alpha2Code")),
            new("Alpha2 has surrounding whitespace", (Alpha2: "  us ", Alpha3: "usa", Numeric: "840", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "alpha2Code")),
            new("Alpha2 tab-wrapped", (Alpha2: "\tGB\t", Alpha3: "GBR", Numeric: "826", CountryName: "United Kingdom"), new ExpectedException(typeof(ArgumentException), "alpha2Code")),
            new("Alpha3 too short", (Alpha2: "US", Alpha3: "US", Numeric: "840", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "alpha3Code")),
            new("Alpha3 non-alpha", (Alpha2: "US", Alpha3: "US1", Numeric: "840", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "alpha3Code")),
            new("Alpha3 too long", (Alpha2: "US", Alpha3: "USAA", Numeric: "840", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "alpha3Code")),
            new("Numeric too short", (Alpha2: "US", Alpha3: "USA", Numeric: "84", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "numericCode")),
            new("Numeric non-digit", (Alpha2: "US", Alpha3: "USA", Numeric: "84A", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "numericCode")),
            new("Numeric too long", (Alpha2: "US", Alpha3: "USA", Numeric: "8400", CountryName: "United States"), new ExpectedException(typeof(ArgumentException), "numericCode")),
            new("Name whitespace", (Alpha2: "US", Alpha3: "USA", Numeric: "840", CountryName: " "), new ExpectedException(typeof(ArgumentException), "name")),
        ];

        public sealed record ValidCase(
            string Name,
            (string Alpha2, string Alpha3, string Numeric, string CountryName) Value)
            : ValueCase<(string Alpha2, string Alpha3, string Numeric, string CountryName)>(Name, Value);

        public sealed record InvalidCase(
            string Name,
            (string Alpha2, string Alpha3, string Numeric, string CountryName) Value,
            ExpectedException ExpectedException)
            : ThrowsCase<(string Alpha2, string Alpha3, string Numeric, string CountryName)>(Name, Value, ExpectedException);
    }

    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("US", (Alpha2: "US", Alpha3: "USA", Numeric: "840")),
            new("GB", (Alpha2: "gb", Alpha3: "gbr", Numeric: "826")),
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Not a code", "not-a-code", false),
        ];

        public sealed record ValidCase(string Name, (string Alpha2, string Alpha3, string Numeric) Value)
            : ValueCase<(string Alpha2, string Alpha3, string Numeric)>(Name, Value);

        public sealed record EdgeCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class Parse
    {
        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null", null, new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Empty", string.Empty, new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Space", " ", new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Tab", "\t", new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Not a code", "not-a-code", new ExpectedException(typeof(FormatException), null, "ISO")),
        ];

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);
    }
}
