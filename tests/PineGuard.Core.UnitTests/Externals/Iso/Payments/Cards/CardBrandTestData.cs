using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public static class CardBrandTestData
{
    public static class FromPan
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("visa", "4000000000000000", "Visa"),
            new("mastercard", "5100000000000000", "Mastercard"),
            new("amex", "340000000000000", "American Express"),
            new("discover", "6011000000000000", "Discover"),
            new("diners", "30000000000000", "Diners Club"),
            new("jcb", "3528000000000000", "JCB"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, null),
            new("empty", "", null),
            new("space", " ", null),
            new("unknown", "9999999999999999", null),
            new("visa trims + sanitizes", "  4000 0000 0000 0000  ", "Visa"),
            new("underscores", "4000_0000_0000_0000", null),
        ];

        public sealed record ValidCase(string Name, string? Value, string? ExpectedReturn)
            : ReturnCase<string?, string?>(Name, Value, ExpectedReturn);
    }

    public static class ToIsoPaymentCardBrand
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("visa", "Visa", "Visa"),
            new("mastercard", "Mastercard", "Mastercard"),
            new("amex", "American Express", "American Express"),
            new("discover", "Discover", "Discover"),
            new("diners", "Diners Club", "Diners Club"),
            new("jcb", "JCB", "JCB"),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("null", null, null),
            new("unknown", "Unknown", null),
            new("lowercase", "visa", "Visa"),
            new("tabs/newlines", "\t\r\n", null),
        ];

        public sealed record ValidCase(string Name, string? Value, string? ExpectedReturn)
            : ReturnCase<string?, string?>(Name, Value, ExpectedReturn);
    }
}
