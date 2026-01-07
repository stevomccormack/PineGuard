namespace PineGuard.Core.UnitTests.Externals.Iso.Payments;

public static class PanAlgorithmTestData
{
    public static class IsValid
    {
        private static ValidCase V(string name, string? value, bool expected) => new(Name: name, Value: value, Expected: expected);

        public static TheoryData<ValidCase> ValidCases => new()
        {
            { V("min 12", new string('1', 12), expected: true) },
            { V("13", new string('2', 13), expected: true) },
            { V("16", new string('3', 16), expected: true) },
            { V("max 19", new string('4', 19), expected: true) },
        };

        public static TheoryData<ValidCase> EdgeCases => new()
        {
            { V("min 12 other digit", new string('9', 12), expected: true) },
            { V("max 19 other digit", new string('9', 19), expected: true) },
            { V("too short 11", new string('1', 11), expected: false) },
            { V("too long 20", new string('1', 20), expected: false) },
            { V("letters", "1234abcd5678", expected: false) },
            { V("spaces", "1234 5678 9012", expected: false) },
            { V("dashes", "1234-5678-9012", expected: false) },
            { V("null", null, expected: false) },
            { V("empty", "", expected: false) },
            { V("space", " ", expected: false) },
            { V("tab", "\t", expected: false) },
        };

        #region Cases

        public record Case(string Name, string? Value);

        public sealed record ValidCase(string Name, string? Value, bool Expected)
            : Case(Name, Value);

        #endregion
    }
}
