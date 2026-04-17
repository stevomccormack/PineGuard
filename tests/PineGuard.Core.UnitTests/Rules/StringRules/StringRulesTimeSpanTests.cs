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
    [MemberData(nameof(StringRulesTimeSpanTestData.IsGreaterThan.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsGreaterThan))]
    public void IsGreaterThan_BehavesAsExpected(RuleCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsGreaterThan(value, threshold, inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeSpanTestData.IsLessThan.Cases), MemberType = typeof(StringRulesTimeSpanTestData.IsLessThan))]
    public void IsLessThan_BehavesAsExpected(RuleCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, threshold, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.TimeSpan.IsLessThan(value, threshold, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
