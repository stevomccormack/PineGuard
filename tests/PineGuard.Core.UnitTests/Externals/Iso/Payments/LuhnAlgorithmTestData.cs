namespace PineGuard.Core.UnitTests.Externals.Iso.Payments;

public static class LuhnAlgorithmTestData
{
    public static class IsValid
    {
        private static ValidCase V(string name, string? value, bool expected) => new(name, value, expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("single zero", "0", true) },
            { V("double zero", "00", true) },
            { V("wikipedia sample", "79927398713", true) },
            { V("visa 4242", "4242424242424242", true) },
            { V("visa 4111", "4111111111111111", true) },
            { V("stripe sample", "4000000000000002", true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("2 digits", "18", true) },
            { V("2 digits other", "59", true) },
            { V("leading zeros", "091", true) },
            { V("single digit", "1", false) },
            { V("invalid checksum", "79927398714", false) },
            { V("invalid checksum visa", "4242424242424241", false) },
            { V("invalid checksum visa 4111", "4111111111111112", false) },
            { V("invalid checksum stripe sample", "4000000000000001", false) },
            { V("null", null, false) },
            { V("empty", "", false) },
            { V("space", " ", false) },
            { V("tab", "\t", false) },
            { V("internal space", "12 34", false) },
            { V("dash", "12-34", false) },
            { V("letters", "abcd", false) },
            { V("trailing letter", "1234x", false) },
        };

        #region Cases

        public record Case(string Name, string? Value)
        {
            public override string ToString() => Name;
        }

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : Case(Name, Value);

        #endregion
    }
}
