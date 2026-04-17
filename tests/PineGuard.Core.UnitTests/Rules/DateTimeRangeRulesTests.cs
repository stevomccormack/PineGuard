using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeRangeRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DateTimeRangeRulesTestData.IsChronological.Cases), MemberType = typeof(DateTimeRangeRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(DateTimeRange? range, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, inclusion) = tc.Value;

        // Act
        var result = DateTimeRangeRules.IsChronological(range, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeRulesTestData.IsOverlapping.Cases), MemberType = typeof(DateTimeRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(DateTimeRange? range1, DateTimeRange? range2, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range1, range2, inclusion) = tc.Value;

        // Act
        var result = DateTimeRangeRules.IsOverlapping(range1, range2, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeRulesTestData.Contains.Cases), MemberType = typeof(DateTimeRangeRulesTestData.Contains))]
    public void Contains_BehavesAsExpected(RuleCase<(DateTimeRange? range, DateTime? value, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, value, inclusion) = tc.Value;

        // Act
        var result = DateTimeRangeRules.Contains(range, value, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
