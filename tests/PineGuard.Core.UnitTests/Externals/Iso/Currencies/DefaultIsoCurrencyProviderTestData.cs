using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Currencies;

public static class DefaultIsoCurrencyProviderTestData
{
    public static class TryGet
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("USD", "USD", true, "USD"),
            new("EUR", "EUR", true, "EUR"),
            new("JPY", "JPY", true, "JPY"),
            new("usd lowercase", "usd", true, "USD"),
            new("840 numeric", "840", true, "USD"),
            new("978 numeric", "978", true, "EUR"),
            new("392 numeric", "392", true, "JPY"),
            new("999 numeric", "999", true, "XXX"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, null),
            new("Empty", string.Empty, false, null),
            new("Space", " ", false, null),
            new("Tab", "\t", false, null),
            new("Newline", "\r\n", false, null),
            new("US", "US", false, null),
            new("EURO", "EURO", false, null),
            new("84", "84", false, null),
            new("ZZZ", "ZZZ", false, null),
            new("USD with whitespace", " USD ", false, null),
            new("840 with whitespace", " 840 ", false, null),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }

    public static class ContainsAlpha3Code
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("USD", "USD", true),
            new("EUR", "EUR", true),
            new("usd lowercase", "usd", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Newline", "\r\n", false),
            new("US", "US", false),
            new("EURO", "EURO", false),
            new("84", "84", false),
            new("ZZZ", "ZZZ", false),
            new("999", "999", false),
            new("USD with whitespace", " USD ", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class ContainsNumericCode
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("840", "840", true),
            new("978", "978", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false),
            new("Empty", string.Empty, false),
            new("Space", " ", false),
            new("Tab", "\t", false),
            new("Newline", "\r\n", false),
            new("84", "84", false),
            new("84A", "84A", false),
            new("840 with whitespace", " 840 ", false),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : HasCase<string?>(Name, Value, ExpectedReturn);
    }

    public static class TryGetByAlpha3Code
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("USD", "USD", true, "USD"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, null),
            new("Empty", string.Empty, false, null),
            new("Space", " ", false, null),
            new("Tab", "\t", false, null),
            new("Newline", "\r\n", false, null),
            new("ZZZ", "ZZZ", false, null),
            new("US1", "US1", false, null),
            new("U$D", "U$D", false, null),
            new("USD with whitespace", " USD ", false, null),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }

    public static class TryGetByNumericCode
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("840", "840", true, "840"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("Null", null, false, null),
            new("Empty", string.Empty, false, null),
            new("Space", " ", false, null),
            new("Tab", "\t", false, null),
            new("Newline", "\r\n", false, null),
            new("84", "84", false, null),
            new("84A", "84A", false, null),
            new("840 with whitespace", " 840 ", false, null),
        ];

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn, string? ExpectedOutValue)
            : TryCase<string?, string?>(Name, Value, ExpectedReturn, ExpectedOutValue);
    }
}
