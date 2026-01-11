using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Rules;

public static class PredicateRulesTestData
{
    public static class Satisfies
    {
        public static TheoryData<Case> ValidCases =>
        [
            new("matches", 5, true),
        ];

        public static TheoryData<Case> EdgeCases =>
        [
            new("null", null, false),
            new("not match", 5, false),
        ];

        #region Case Records

        public sealed record Case(string Name, int? Value, bool ExpectedReturn)
            : IsCase<int?>(Name, Value, ExpectedReturn);

        #endregion
    }
}
