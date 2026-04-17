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

    public static class IsGreaterThan
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>> Cases => F.IsGreaterThan.AllScenarios.ToRuleCases();
    }

    public static class IsLessThan
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan? threshold, Inclusion inclusion)>> Cases => F.IsLessThan.AllScenarios.ToRuleCases();
    }
}
