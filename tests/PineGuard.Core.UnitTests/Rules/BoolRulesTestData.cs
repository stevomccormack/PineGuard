using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.BoolRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class BoolRulesTestData
{
    public static class IsTrue
    {
        public static TheoryData<RuleCase<bool?>> Cases => F.IsTrue.AllScenarios.ToRuleCases();
    }

    public static class IsFalse
    {
        public static TheoryData<RuleCase<bool?>> Cases => F.IsFalse.AllScenarios.ToRuleCases();
    }
}
