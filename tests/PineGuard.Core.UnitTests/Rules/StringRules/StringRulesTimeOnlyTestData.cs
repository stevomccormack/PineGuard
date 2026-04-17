using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesTimeOnlyTestData
{
    public static class IsBetween
    {
        public static TheoryData<RuleCase<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)>> Cases => F.StringTimeOnly.IsBetween.AllScenarios.ToRuleCases();
    }

    public static class IsBefore
    {
        public static TheoryData<RuleCase<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>> Cases => F.StringTimeOnly.IsBefore.AllScenarios.ToRuleCases();
    }

    public static class IsAfter
    {
        public static TheoryData<RuleCase<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)>> Cases => F.StringTimeOnly.IsAfter.AllScenarios.ToRuleCases();
    }

    public static class IsSame
    {
        public static TheoryData<RuleCase<(string? value, TimeOnly other, TimePrecision? precision)>> Cases => F.StringTimeOnly.IsSame.AllScenarios.ToRuleCases();
    }

    public static class IsWithin
    {
        public static TheoryData<RuleCase<(string? value, string? reference, TimeSpan window)>> Cases => F.StringTimeOnly.IsWithin.AllScenarios.ToRuleCases();
    }

    public static class IsChronological
    {
        public static TheoryData<RuleCase<(string? start, string? end, Inclusion inclusion)>> Cases => F.StringTimeOnly.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)>> Cases => F.StringTimeOnly.IsOverlapping.AllScenarios.ToRuleCases();
    }
}
