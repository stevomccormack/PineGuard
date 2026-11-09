using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTimeSpanTestData
{
    public static class IsInRange
    {
        public static TheoryData<RuleCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases => F.TimeSpanIsInRange.AllScenarios.ToRuleCases();
    }

    public static class IsLongerThan
    {
        public static TheoryData<RuleCase<(string? value, TimeSpan other, Inclusion inclusion)>> Cases => F.TimeSpanIsLongerThan.AllScenarios.ToRuleCases();
    }

    public static class IsShorterThan
    {
        public static TheoryData<RuleCase<(string? value, TimeSpan other, Inclusion inclusion)>> Cases => F.TimeSpanIsShorterThan.AllScenarios.ToRuleCases();
    }
}
