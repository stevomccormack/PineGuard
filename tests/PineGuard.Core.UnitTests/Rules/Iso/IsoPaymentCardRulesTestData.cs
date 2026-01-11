using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules.Iso;

public static class IsoPaymentCardRulesTestData
{
    public static class DefaultSeparators
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("visa digits", "4111111111111111", true),
            new("visa spaces", "4111 1111 1111 1111", true),
            new("visa dashes", "4111-1111-1111-1111", true),
            new("visa padded", " 4111 1111 1111 1111 ", true),
            new("amex digits", "378282246310005", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("empty", string.Empty, false),
            new("space", " ", false),
            new("luhn fail", "4111111111111112", false),
            new("invalid char", "4111-1111-1111-111X", false),
            new("too short", "41111111111", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class DigitsOnlySeparators
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("visa digits", "4111111111111111", true),
            new("amex digits", "378282246310005", true)
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("contains spaces", "4111 1111 1111 1111", false),
            new("contains dashes", "4111-1111-1111-1111", false),
            new("too short", "41111111111", false),
            new("very short", "123", false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }

    public static class CustomAllowedSeparators
    {
        public static TheoryData<Case> Cases =>
        [
            new("spaces allowed", "4111 1111 1111 1111", [' '], true),
            new("spaces not allowed", "4111-1111-1111-1111", [' '], false),
            new("dashes allowed", "4111-1111-1111-1111", ['-'], true),
            new("dashes not allowed", "4111 1111 1111 1111", ['-'], false)
        ];

        #region Case Records

        public sealed record Case(string Name, string? Value, char[] AllowedSeparators, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion Cases
    }
}
