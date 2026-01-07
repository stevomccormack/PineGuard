namespace PineGuard.Core.UnitTests.Rules.Cldr;

public static class CldrTimeZoneRulesTestData
{
    public static class IsWindowsTimeZoneId
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("valid", "Valid", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("null", null, false) },
            { new Case("empty", string.Empty, false) },
            { new Case("space", " ", false) },
            { new Case("whitespace", "\t\r\n", false) },
            { new Case("invalid", "Invalid", false) },
            { new Case("padded", " Valid ", false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
