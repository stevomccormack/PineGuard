using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class SqlDateTimeRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(SqlDateTimeRulesTestData.IsInSqlDateRange.Cases), MemberType = typeof(SqlDateTimeRulesTestData.IsInSqlDateRange))]
    public void IsInSqlDateRange_BehavesAsExpected(RuleCase<DateOnly?> tc)
    {
        // Act
        var result = SqlDateTimeRules.IsInSqlDateRange(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeRulesTestData.IsInSqlDateTimeRangeDateTime.Cases), MemberType = typeof(SqlDateTimeRulesTestData.IsInSqlDateTimeRangeDateTime))]
    public void IsInSqlDateTimeRangeDateTime_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = SqlDateTimeRules.IsInSqlDateTimeRange(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(SqlDateTimeRulesTestData.IsInSqlDateTimeRangeDateTimeOffset.Cases), MemberType = typeof(SqlDateTimeRulesTestData.IsInSqlDateTimeRangeDateTimeOffset))]
    public void IsInSqlDateTimeRangeOffset_BehavesAsExpected(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = SqlDateTimeRules.IsInSqlDateTimeRange(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
