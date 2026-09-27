using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeSpanRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeSpanRulesTestData
{
    public static class IsDurationBetween
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases => F.IsDurationBetween.AllScenarios.ToRuleCases();
    }

    public static class IsLongerThan
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>> Cases => F.IsLongerThan.AllScenarios.ToRuleCases();
    }

    public static class IsShorterThan
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>> Cases => F.IsShorterThan.AllScenarios.ToRuleCases();
    }
}
