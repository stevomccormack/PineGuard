using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeOnlyRangeRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TimeOnlyRangeRulesTestData.IsChronological.Cases), MemberType = typeof(TimeOnlyRangeRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(TimeOnlyRange? range, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, inclusion) = tc.Value;

        // Act
        var result = TimeOnlyRangeRules.IsChronological(range, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeRulesTestData.IsOverlapping.Cases), MemberType = typeof(TimeOnlyRangeRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(TimeOnlyRange? range1, TimeOnlyRange? range2, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range1, range2, inclusion) = tc.Value;

        // Act
        var result = TimeOnlyRangeRules.IsOverlapping(range1, range2, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeRulesTestData.Contains.Cases), MemberType = typeof(TimeOnlyRangeRulesTestData.Contains))]
    public void Contains_BehavesAsExpected(RuleCase<(TimeOnlyRange? range, TimeOnly? value, Inclusion inclusion)> tc)
    {
        // Arrange
        var (range, value, inclusion) = tc.Value;

        // Act
        var result = TimeOnlyRangeRules.Contains(range, value, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
