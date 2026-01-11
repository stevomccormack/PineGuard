using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public static class IsoPaymentCardBrandUtilityTestData
{
    public static class FromPan
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("visa 13", "4000000000000", "Visa"),
            new("visa 16", "4000000000000000", "Visa"),
            new("visa 19", "4000000000000000000", "Visa"),
            new("mc 51", "5100000000000000", "Mastercard"),
            new("mc 2221", "2221000000000000", "Mastercard"),
            new("amex 34", "340000000000000", "American Express"),
            new("amex 37", "370000000000000", "American Express"),
            new("discover 6011", "6011000000000000", "Discover"),
            new("discover 65", "6500000000000000", "Discover"),
            new("discover 644", "6440000000000000", "Discover"),
            new("diners 300", "30000000000000", "Diners Club"),
            new("diners 360", "36000000000000", "Diners Club"),
            new("jcb 3528", "3528000000000000", "JCB"),
            new("jcb 3589", "3589000000000000", "JCB"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, null),
            new("empty", "", null),
            new("space", " ", null),
            new("unknown", "9999999999999999", null),
            new("too short", "4", null),
            new("visa trims", "  4000000000000000  ", "Visa"),
            new("visa spaces", "4000 0000 0000 0000", "Visa"),
            new("visa dashes", "4000-0000-0000-0000", "Visa"),
            new("visa underscores", "4000_0000_0000_0000", null),
        ];

        public sealed record ValidCase(string Name, string? Value, string? ExpectedReturn)
            : ReturnCase<string?, string?>(Name, Value, ExpectedReturn);
    }

    public static class FromBrandName
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("visa", "Visa", "Visa"),
            new("visa lowercase", "visa", "Visa"),
            new("mastercard", "Mastercard", "Mastercard"),
            new("amex", "American Express", "American Express"),
            new("discover", "Discover", "Discover"),
            new("diners", "Diners Club", "Diners Club"),
            new("jcb", "JCB", "JCB"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, null),
            new("empty", "", null),
            new("space", " ", null),
            new("unknown", "Unknown", null),
            new("trims", "  Visa  ", "Visa"),
            new("tabs/newlines", "\t\r\n", null),
        ];

        public sealed record ValidCase(string Name, string? Value, string? ExpectedReturn)
            : ReturnCase<string?, string?>(Name, Value, ExpectedReturn);
    }
}
