using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeRangeRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<RuleCase<(DateTimeRange? range, Inclusion inclusion)>> Cases =>
            F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(DateTimeRange? range1, DateTimeRange? range2, Inclusion inclusion)>> Cases =>
            F.IsOverlapping.AllScenarios.ToRuleCases();
    }

    public static class Contains
    {
        public static TheoryData<RuleCase<(DateTimeRange? range, DateTime? value, Inclusion inclusion)>> Cases =>
            F.Contains.AllScenarios.ToRuleCases();
    }
}
