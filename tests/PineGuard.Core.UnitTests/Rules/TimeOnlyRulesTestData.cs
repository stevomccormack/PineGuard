using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeOnlyRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeOnlyRulesTestData
{
    public static class IsBetween
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>> Cases => F.IsBetween.AllScenarios.ToRuleCases();
    }

    public static class IsBefore
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>> Cases => F.IsBefore.AllScenarios.ToRuleCases();
    }

    public static class IsBeforeDefaultInclusion
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly? other)>> Cases => F.IsBeforeDefaultInclusion.AllScenarios.ToRuleCases();
    }

    public static class IsAfter
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)>> Cases => F.IsAfter.AllScenarios.ToRuleCases();
    }

    public static class IsAfterDefaultInclusion
    {
        public static TheoryData<RuleCase<(TimeOnly? value, TimeOnly? other)>> Cases => F.IsAfterDefaultInclusion.AllScenarios.ToRuleCases();
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
