using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Currencies;

public static class IsoCurrencyTestData
{
    public static class Constructor
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("USD", ("USD", "840", 2, "US Dollar")),
            new("eur lower", ("eur", "978", 2, "Euro")),
            new("JPY", ("JPY", "392", 0, "Yen")),
            new("BHD", ("BHD", "048", 3, "Bahraini Dinar"))
        ];

        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Alpha3 too short", ("US", "840", 2, "US Dollar"), new ExpectedException(typeof(ArgumentException), "alpha3Code")),
            new("Alpha3 non-alpha", ("US1", "840", 2, "US Dollar"), new ExpectedException(typeof(ArgumentException), "alpha3Code")),
            new("Numeric too short", ("USD", "84", 2, "US Dollar"), new ExpectedException(typeof(ArgumentException), "numericCode")),
            new("Numeric non-digit", ("USD", "84A", 2, "US Dollar"), new ExpectedException(typeof(ArgumentException), "numericCode")),
            new("DecimalPlaces too low", ("USD", "840", -2, "US Dollar"), new ExpectedException(typeof(ArgumentOutOfRangeException), "decimalPlaces")),
            new("DecimalPlaces too high", ("USD", "840", 5, "US Dollar"), new ExpectedException(typeof(ArgumentOutOfRangeException), "decimalPlaces")),
            new("Name whitespace", ("USD", "840", 2, " "), new ExpectedException(typeof(ArgumentException), "name"))
        ];

        #region Case Records

        public sealed record ValidCase(
            string Name,
            (string Alpha3, string Numeric, int DecimalPlaces, string CurrencyName) Value)
            : ValueCase<(string Alpha3, string Numeric, int DecimalPlaces, string CurrencyName)>(Name, Value);

        public sealed record InvalidCase(
            string Name,
            (string Alpha3, string Numeric, int DecimalPlaces, string CurrencyName) Value,
            ExpectedException ExpectedException)
            : ThrowsCase<(string Alpha3, string Numeric, int DecimalPlaces, string CurrencyName)>(Name, Value, ExpectedException);

        #endregion
    }

    public static class TryParse
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("Alpha3 with spaces", " usd ", "USD"),
            new("Numeric EUR", "978", "EUR"),
            new("Numeric JPY", "392", "JPY")
        ];

        public static TheoryData<EdgeCase> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Not a code", "not-a-code", false)
        ];

        #region Case Records

        public sealed record ValidCase(string Name, string Value, string ExpectedOutValue)
            : TryCase<string, string>(Name, Value, true, ExpectedOutValue);

        public sealed record EdgeCase(string Name, string? Value, bool ExpectedReturn)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, null);

        #endregion
    }

    public static class Parse
    {
        public static TheoryData<InvalidCase> InvalidCases =>
        [
            new("Null", null, new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Empty", string.Empty, new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Space", " ", new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Tab", "\t", new ExpectedException(typeof(FormatException), null, "ISO")),
            new("Not a code", "not-a-code", new ExpectedException(typeof(FormatException), null, "ISO"))
        ];

        #region Case Records

        public sealed record InvalidCase(string Name, string? Value, ExpectedException ExpectedException)
            : ThrowsCase<string?>(Name, Value, ExpectedException);

        #endregion
    }
}
