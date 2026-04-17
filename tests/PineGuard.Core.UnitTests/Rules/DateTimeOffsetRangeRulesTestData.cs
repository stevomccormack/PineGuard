using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateTimeOffsetRangeRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateTimeOffsetRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<RuleCase<(DateTimeOffsetRange? range, Inclusion inclusion)>> Cases =>
            F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(DateTimeOffsetRange? range1, DateTimeOffsetRange? range2, Inclusion inclusion)>> Cases =>
            F.IsOverlapping.AllScenarios.ToRuleCases();
    }

    public static class Contains
    {
        public static TheoryData<RuleCase<(DateTimeOffsetRange? range, DateTimeOffset? value, Inclusion inclusion)>> Cases =>
            F.Contains.AllScenarios.ToRuleCases();
    }
}
