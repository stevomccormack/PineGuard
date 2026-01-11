using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Externals.Iso.Payments;

public static class PanAlgorithmTestData
{
    public static class IsValid
    {
        public static TheoryData<ValidCase> ValidCases =>
        [
            new("min 12", new string('1', 12), true),
            new("13", new string('2', 13), true),
            new("16", new string('3', 16), true),
            new("max 19", new string('4', 19), true)
        ];

        public static TheoryData<ValidCase> EdgeCases =>
        [
            new("min 12 other digit", new string('9', 12), true),
            new("max 19 other digit", new string('9', 19), true),
            new("too short 11", new string('1', 11), false),
            new("too long 20", new string('1', 20), false),
            new("letters", "1234abcd5678", false),
            new("spaces", "1234 5678 9012", false),
            new("dashes", "1234-5678-9012", false),
            new("null", null, false),
            new("empty", "", false),
            new("space", " ", false),
            new("tab", "\t", false)
        ];

        public static TheoryData<IThrowsCase> InvalidCases => [];

        #region Case Records

        public sealed record ValidCase(string Name, string? Value, bool ExpectedReturn)
            : IsCase<string?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
