using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.NullRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class NullRulesTestData
{
    public static class IsNull
    {
        public static TheoryData<RuleCase<object?>> Cases => F.IsNull.AllScenarios.ToRuleCases();
    }

    public static class IsNotNull
    {
        public static TheoryData<RuleCase<object?>> Cases => F.IsNotNull.AllScenarios.ToRuleCases();
    }
}
