using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesGraphemesTestData
{
    public static class HasExactCount
    {
        public static TheoryData<RuleCase<(string? value, int count)>> Cases => F.GraphemesHasExactCount.AllScenarios.ToRuleCases();
    }

    public static class HasMinCount
    {
        public static TheoryData<RuleCase<(string? value, int min)>> Cases => F.GraphemesHasMinCount.AllScenarios.ToRuleCases();
    }

    public static class HasMaxCount
    {
        public static TheoryData<RuleCase<(string? value, int max)>> Cases => F.GraphemesHasMaxCount.AllScenarios.ToRuleCases();
    }

    public static class HasCountBetween
    {
        public static TheoryData<RuleCase<(string? value, int min, int max, Inclusion inclusion)>> Cases => F.GraphemesHasCountBetween.AllScenarios.ToRuleCases();
    }
}
