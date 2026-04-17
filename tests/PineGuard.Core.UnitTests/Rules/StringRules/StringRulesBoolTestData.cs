using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesBoolTestData
{
    public static class IsTrue
    {
        public static TheoryData<RuleCase<string?>> Cases => F.BoolIsTrue.AllScenarios.ToRuleCases();
    }

    public static class IsFalse
    {
        public static TheoryData<RuleCase<string?>> Cases => F.BoolIsFalse.AllScenarios.ToRuleCases();
    }
}
