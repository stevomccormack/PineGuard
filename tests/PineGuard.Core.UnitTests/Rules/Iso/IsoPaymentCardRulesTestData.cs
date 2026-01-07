namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoPaymentCardRulesTestData
{
    public static class DefaultSeparators
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("visa digits", "4111111111111111", true) },
            { new Case("visa spaces", "4111 1111 1111 1111", true) },
            { new Case("visa dashes", "4111-1111-1111-1111", true) },
            { new Case("visa padded", " 4111 1111 1111 1111 ", true) },
            { new Case("amex digits", "378282246310005", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("luhn fail", "4111111111111112", false) },
            { new Case("invalid char", "4111-1111-1111-111X", false) },
            { new Case("too short", "41111111111", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class DigitsOnlySeparators
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("visa digits", "4111111111111111", true) },
            { new Case("amex digits", "378282246310005", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("contains spaces", "4111 1111 1111 1111", false) },
            { new Case("contains dashes", "4111-1111-1111-1111", false) },
            { new Case("too short", "41111111111", false) },
            { new Case("very short", "123", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }

    public static class CustomAllowedSeparators
    {
        public static TheoryData<Case> Cases => new()
        {
            { new Case("spaces allowed", "4111 1111 1111 1111", [' '], true) },
            { new Case("spaces not allowed", "4111-1111-1111-1111", [' '], false) },
            { new Case("dashes allowed", "4111-1111-1111-1111", ['-'], true) },
            { new Case("dashes not allowed", "4111 1111 1111 1111", ['-'], false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, char[] AllowedSeparators, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
