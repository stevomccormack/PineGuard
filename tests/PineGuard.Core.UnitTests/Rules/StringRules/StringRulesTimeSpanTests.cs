using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesTimeSpanTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsDurationBetween.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsDurationBetween))]
    public void IsDurationBetween_BehavesAsExpected(RuleCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsDurationBetween(value, min, max, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsLongerThan.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsLongerThan))]
    public void IsLongerThan_BehavesAsExpected(RuleCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsLongerThan(value, threshold, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsShorterThan.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsShorterThan))]
    public void IsShorterThan_BehavesAsExpected(RuleCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsShorterThan(value, threshold, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
