using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesGuidTestData
{
    public static class IsGuid
    {
        public static TheoryData<RuleCase<string?>> Cases => F.GuidIsGuid.AllScenarios.ToRuleCases();
    }

    public static class IsNotEmpty
    {
        public static TheoryData<RuleCase<string?>> Cases => F.GuidIsNotEmpty.AllScenarios.ToRuleCases();
    }

    public static class HasVersion
    {
        public static TheoryData<RuleCase<(string? value, int version)>> Cases => F.GuidHasVersion.AllScenarios.ToRuleCases();
    }
}
