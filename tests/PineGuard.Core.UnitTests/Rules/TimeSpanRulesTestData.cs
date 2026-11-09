using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeSpanRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeSpanRulesTestData
{
    public static class IsInRange
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan min, TimeSpan max, Inclusion inclusion)>> Cases => F.IsInRange.AllScenarios.ToRuleCases();
    }

    public static class IsLongerThan
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan other, Inclusion inclusion)>> Cases => F.IsLongerThan.AllScenarios.Where(scenario => scenario.Inputs.other.HasValue).Select(scenario => new RuleScenario<(TimeSpan? value, TimeSpan other, Inclusion inclusion)>(scenario.Name, (scenario.Inputs.value, scenario.Inputs.other.GetValueOrDefault(), scenario.Inputs.inclusion), scenario.IsValid)).ToArray().ToRuleCases();
    }

    public static class IsShorterThan
    {
        public static TheoryData<RuleCase<(TimeSpan? value, TimeSpan other, Inclusion inclusion)>> Cases => F.IsShorterThan.AllScenarios.Where(scenario => scenario.Inputs.other.HasValue).Select(scenario => new RuleScenario<(TimeSpan? value, TimeSpan other, Inclusion inclusion)>(scenario.Name, (scenario.Inputs.value, scenario.Inputs.other.GetValueOrDefault(), scenario.Inputs.inclusion), scenario.IsValid)).ToArray().ToRuleCases();
    }
}
