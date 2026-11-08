using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeOnlyRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeOnlyRulesTestData
{
    public static class IsInRange
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>> Cases => F.IsInRange.AllScenarios.ToRuleCases();
    }

    public static class IsBefore
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>> Cases => F.IsBefore.AllScenarios.Where(scenario => scenario.Inputs.other.HasValue).Select(scenario => new RuleScenario<(TimeOnly? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>(scenario.Name, (scenario.Inputs.value, scenario.Inputs.other.GetValueOrDefault(), scenario.Inputs.inclusion, scenario.Inputs.precision), scenario.IsValid)).ToArray().ToRuleCases();
    }

    public static class IsBeforeDefaultInclusion
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly other)>> Cases => F.IsBeforeDefaultInclusion.AllScenarios.Where(scenario => scenario.Inputs.other.HasValue).Select(scenario => new RuleScenario<(TimeOnly? value, TimeOnly other)>(scenario.Name, (scenario.Inputs.value, scenario.Inputs.other.GetValueOrDefault()), scenario.IsValid)).ToArray().ToRuleCases();
    }

    public static class IsAfter
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>> Cases => F.IsAfter.AllScenarios.Where(scenario => scenario.Inputs.other.HasValue).Select(scenario => new RuleScenario<(TimeOnly? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>(scenario.Name, (scenario.Inputs.value, scenario.Inputs.other.GetValueOrDefault(), scenario.Inputs.inclusion, scenario.Inputs.precision), scenario.IsValid)).ToArray().ToRuleCases();
    }

    public static class IsAfterDefaultInclusion
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly other)>> Cases => F.IsAfterDefaultInclusion.AllScenarios.Where(scenario => scenario.Inputs.other.HasValue).Select(scenario => new RuleScenario<(TimeOnly? value, TimeOnly other)>(scenario.Name, (scenario.Inputs.value, scenario.Inputs.other.GetValueOrDefault()), scenario.IsValid)).ToArray().ToRuleCases();
    }

    public static class IsSame
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly? other, TimePrecision? precision)>> Cases => F.IsSame.AllScenarios.ToRuleCases();
    }

    public static class IsWithin
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly? reference, TimeSpan window)>> Cases => F.IsWithin.AllScenarios.ToRuleCases();
    }

    public static class IsChronological
    {
        public static TheoryData<RuleCase<(TimeOnly? start, TimeOnly? end, Inclusion inclusion)>> Cases => F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion)>> Cases => F.IsOverlapping.AllScenarios.ToRuleCases();
    }
}
