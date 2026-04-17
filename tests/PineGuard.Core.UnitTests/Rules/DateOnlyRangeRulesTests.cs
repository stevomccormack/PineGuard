using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateOnlyRangeRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DateOnlyRangeRulesTestData.IsChronological.Cases), MemberType = typeof(DateOnlyRangeRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(DateOnlyRange? range, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, inclusion) = tc.Value;

        // Act
        var result = DateOnlyRangeRules.IsChronological(range, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeRulesTestData.IsOverlapping.Cases), MemberType = typeof(DateOnlyRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(DateOnlyRange? range1, DateOnlyRange? range2, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range1, range2, inclusion) = tc.Value;

        // Act
        var result = DateOnlyRangeRules.IsOverlapping(range1, range2, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeRulesTestData.Contains.Cases), MemberType = typeof(DateOnlyRangeRulesTestData.Contains))]
    public void Contains_BehavesAsExpected(RuleCase<(DateOnlyRange? range, DateOnly? value, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, value, inclusion) = tc.Value;

        // Act
        var result = DateOnlyRangeRules.Contains(range, value, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
