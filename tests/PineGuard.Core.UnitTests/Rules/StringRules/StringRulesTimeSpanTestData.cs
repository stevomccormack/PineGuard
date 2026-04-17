using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTimeSpanTestData
{
    public static class IsDurationBetween
    {
        public static TheoryData<RuleCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases => F.TimeSpanIsDurationBetween.AllScenarios.ToRuleCases();
    }

    public static class IsGreaterThan
    {
        public static TheoryData<RuleCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> Cases => F.TimeSpanIsGreaterThan.AllScenarios.ToRuleCases();
    }

    public static class IsLessThan
    {
        public static TheoryData<RuleCase<(string? value, TimeSpan threshold, Inclusion inclusion)>> Cases => F.TimeSpanIsLessThan.AllScenarios.ToRuleCases();
    }
}
