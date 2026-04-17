using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.DateOnlyRangeRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class DateOnlyRangeRulesTestData
{
    public static class IsChronological
    {
        public static TheoryData<RuleCase<(DateOnlyRange? range, Inclusion inclusion)>> Cases =>
            F.IsChronological.AllScenarios.ToRuleCases();
    }

    public static class IsOverlapping
    {
        public static TheoryData<RuleCase<(DateOnlyRange? range1, DateOnlyRange? range2, Inclusion inclusion)>> Cases =>
            F.IsOverlapping.AllScenarios.ToRuleCases();
    }

    public static class Contains
    {
        public static TheoryData<RuleCase<(DateOnlyRange? range, DateOnly? value, Inclusion inclusion)>> Cases =>
            F.Contains.AllScenarios.ToRuleCases();
    }
}
