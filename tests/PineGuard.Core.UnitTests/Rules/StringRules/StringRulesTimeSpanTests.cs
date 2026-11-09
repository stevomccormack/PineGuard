using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesTimeSpanTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsInRange.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsInRange))]
    public void IsInRange_BehavesAsExpected(RuleCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsInRange(value, min, max, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsLongerThan.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsLongerThan))]
    public void IsLongerThan_BehavesAsExpected(RuleCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, other, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsLongerThan(value, other, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsShorterThan.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsShorterThan))]
    public void IsShorterThan_BehavesAsExpected(RuleCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, other, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsShorterThan(value, other, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
