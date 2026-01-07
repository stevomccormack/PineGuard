namespace PineGuard.Core.UnitTests.Rules;

public static class PredicateRulesTestData
{
    public static class Satisfies
    {
        public static TheoryData<Case> ValidCases => new()
        {
            { new Case(Name: "matches", Value: 5, Expected: true) },
        };

        public static TheoryData<Case> EdgeCases => new()
        {
            { new Case(Name: "null", Value: null, Expected: false) },
            { new Case(Name: "not match", Value: 5, Expected: false) },
        };

        #region Cases

        public sealed record Case(string Name, int? Value, bool Expected)
        {
            public override string ToString() => Name;
        }

        #endregion
    }
}
