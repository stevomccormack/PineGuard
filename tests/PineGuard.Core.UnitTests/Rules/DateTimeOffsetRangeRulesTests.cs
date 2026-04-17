using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeOffsetRangeRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeRulesTestData.IsChronological.Cases), MemberType = typeof(DateTimeOffsetRangeRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(DateTimeOffsetRange? range, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, inclusion) = tc.Value;

        // Act
        var result = DateTimeOffsetRangeRules.IsChronological(range, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeRulesTestData.IsOverlapping.Cases), MemberType = typeof(DateTimeOffsetRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(DateTimeOffsetRange? range1, DateTimeOffsetRange? range2, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range1, range2, inclusion) = tc.Value;

        // Act
        var result = DateTimeOffsetRangeRules.IsOverlapping(range1, range2, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeRulesTestData.Contains.Cases), MemberType = typeof(DateTimeOffsetRangeRulesTestData.Contains))]
    public void Contains_BehavesAsExpected(RuleCase<(DateTimeOffsetRange? range, DateTimeOffset? value, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, value, inclusion) = tc.Value;

        // Act
        var result = DateTimeOffsetRangeRules.Contains(range, value, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
