using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.TimeOnlyRangeRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class TimeOnlyRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<RuleCase<(TimeOnlyRange? range, Inclusion inclusion)>> Cases =>
            F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(TimeOnlyRange? range1, TimeOnlyRange? range2, Inclusion inclusion)>> Cases =>
            F.IsOverlapping.AllScenarios.ToRuleCases();
    }

    public static class Contains
    {
        public static TheoryData<RuleCase<(TimeOnlyRange? range, TimeOnly? value, Inclusion inclusion)>> Cases =>
            F.Contains.AllScenarios.ToRuleCases();
    }
}
