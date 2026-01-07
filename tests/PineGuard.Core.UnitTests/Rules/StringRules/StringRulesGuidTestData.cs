namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesGuidTestData
{
    public static class IsEmpty
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "Guid.Empty => true", Value: Guid.Empty.ToString("D"), Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Guid.NewGuid => false", Value: Guid.NewGuid().ToString("D"), Expected: false) },
            { new Case(Name: "not-a-guid => false", Value: "not-a-guid", Expected: false) },
            { new Case(Name: "null => false", Value: null, Expected: false) },
            { new Case(Name: "space => false", Value: " ", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }

    public static class IsNullOrEmpty
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "null => true", Value: null, Expected: true) },
            { new Case(Name: "Guid.Empty => true", Value: Guid.Empty.ToString("D"), Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "Guid.NewGuid => false", Value: Guid.NewGuid().ToString("D"), Expected: false) },
            { new Case(Name: "not-a-guid => false", Value: "not-a-guid", Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, string? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
