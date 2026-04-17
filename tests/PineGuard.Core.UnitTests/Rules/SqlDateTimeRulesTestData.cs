using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.SqlDateTimeRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class SqlDateTimeRulesTestData
{
    public static class IsInSqlDateRange
    {
        public static TheoryData<RuleCase<DateOnly?>> Cases => F.IsInSqlDateRange.AllScenarios.ToRuleCases();
    }

    public static class IsInSqlDateTimeRangeDateTime
    {
        public static TheoryData<RuleCase<DateTime?>> Cases => F.IsInSqlDateTimeRangeDateTime.AllScenarios.ToRuleCases();
    }

    public static class IsInSqlDateTimeRangeDateTimeOffset
    {
        public static TheoryData<RuleCase<DateTimeOffset?>> Cases => F.IsInSqlDateTimeRangeDateTimeOffset.AllScenarios.ToRuleCases();
    }
}
