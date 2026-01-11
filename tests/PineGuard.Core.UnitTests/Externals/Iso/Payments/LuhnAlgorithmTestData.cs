using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments;

public static class LuhnAlgorithmTestData
{
    public static class IsValid
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("single zero", "0", true),
            new("double zero", "00", true),
            new("wikipedia sample", "79927398713", true),
            new("visa 4242", "4242424242424242", true),
            new("visa 4111", "4111111111111111", true),
            new("stripe sample", "4000000000000002", true),
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("2 digits", "18", true),
            new("2 digits other", "59", true),
            new("leading zeros", "091", true),
            new("single digit", "1", false),
            new("invalid checksum", "79927398714", false),
            new("invalid checksum visa", "4242424242424241", false),
            new("invalid checksum visa 4111", "4111111111111112", false),
            new("invalid checksum stripe sample", "4000000000000001", false),
            new("null", null, false),
            new("empty", "", false),
            new("space", " ", false),
            new("tab", "\t", false),
            new("internal space", "12 34", false),
            new("dash", "12-34", false),
            new("letters", "abcd", false),
            new("trailing letter", "1234x", false),
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
