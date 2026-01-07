namespace PineGuard.Core.UnitTests.Rules.Iana;

public static class IanaTimeZoneRulesTestData
{
    public static class IsIanaTimeZoneId
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case("valid", "America/New_York", true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case("invalid", "Not/AZone", false) },
            { new Case("null", null, false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion Cases
    }
}
