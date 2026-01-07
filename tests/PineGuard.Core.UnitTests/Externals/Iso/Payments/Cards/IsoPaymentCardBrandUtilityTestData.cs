namespace PineGuard.Core.UnitTests.Externals.Iso.Payments.Cards;

public static class IsoPaymentCardBrandUtilityTestData
{
    public static class FromPan
    {
        private static ValidCase V(string name, string? pan, bool expected, string? expectedBrandName) => new(Name: name, Pan: pan, Expected: expected, ExpectedBrandName: expectedBrandName);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("visa 13", "4000000000000", expected: true, expectedBrandName: "Visa") },
            { V("visa 16", "4000000000000000", expected: true, expectedBrandName: "Visa") },
            { V("visa 19", "4000000000000000000", expected: true, expectedBrandName: "Visa") },
            { V("mc 51", "5100000000000000", expected: true, expectedBrandName: "Mastercard") },
            { V("mc 2221", "2221000000000000", expected: true, expectedBrandName: "Mastercard") },
            { V("amex 34", "340000000000000", expected: true, expectedBrandName: "American Express") },
            { V("amex 37", "370000000000000", expected: true, expectedBrandName: "American Express") },
            { V("discover 6011", "6011000000000000", expected: true, expectedBrandName: "Discover") },
            { V("discover 65", "6500000000000000", expected: true, expectedBrandName: "Discover") },
            { V("discover 644", "6440000000000000", expected: true, expectedBrandName: "Discover") },
            { V("diners 300", "30000000000000", expected: true, expectedBrandName: "Diners Club") },
            { V("diners 360", "36000000000000", expected: true, expectedBrandName: "Diners Club") },
            { V("jcb 3528", "3528000000000000", expected: true, expectedBrandName: "JCB") },
            { V("jcb 3589", "3589000000000000", expected: true, expectedBrandName: "JCB") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false, expectedBrandName: null) },
            { V("empty", "", expected: false, expectedBrandName: null) },
            { V("space", " ", expected: false, expectedBrandName: null) },
            { V("unknown", "9999999999999999", expected: false, expectedBrandName: null) },
            { V("too short", "4", expected: false, expectedBrandName: null) },
            { V("visa trims", "  4000000000000000  ", expected: true, expectedBrandName: "Visa") },
            { V("visa spaces", "4000 0000 0000 0000", expected: true, expectedBrandName: "Visa") },
            { V("visa dashes", "4000-0000-0000-0000", expected: true, expectedBrandName: "Visa") },
            { V("visa underscores", "4000_0000_0000_0000", expected: false, expectedBrandName: null) },
        };

        #region Cases

        public record Case(string Name, string? Pan);

        public sealed record ValidCase(string Name, string? Pan, bool Expected, string? ExpectedBrandName)
            : Case(Name, Pan);

        #endregion
    }

    public static class FromBrandName
    {
        private static ValidCase V(string name, string? brandName, bool expected, string? expectedBrandName) => new(Name: name, BrandName: brandName, Expected: expected, ExpectedBrandName: expectedBrandName);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("visa", "Visa", expected: true, expectedBrandName: "Visa") },
            { V("visa lowercase", "visa", expected: true, expectedBrandName: "Visa") },
            { V("mastercard", "Mastercard", expected: true, expectedBrandName: "Mastercard") },
            { V("amex", "American Express", expected: true, expectedBrandName: "American Express") },
            { V("discover", "Discover", expected: true, expectedBrandName: "Discover") },
            { V("diners", "Diners Club", expected: true, expectedBrandName: "Diners Club") },
            { V("jcb", "JCB", expected: true, expectedBrandName: "JCB") },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("null", null, expected: false, expectedBrandName: null) },
            { V("empty", "", expected: false, expectedBrandName: null) },
            { V("space", " ", expected: false, expectedBrandName: null) },
            { V("unknown", "Unknown", expected: false, expectedBrandName: null) },
            { V("trims", "  Visa  ", expected: true, expectedBrandName: "Visa") },
            { V("tabs/newlines", "\t\r\n", expected: false, expectedBrandName: null) },
        };

        #region Cases

        public record Case(string Name, string? BrandName);

        public sealed record ValidCase(string Name, string? BrandName, bool Expected, string? ExpectedBrandName)
            : Case(Name, BrandName);

        #endregion
    }
}
