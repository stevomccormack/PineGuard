using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesGraphemesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesGraphemesTestData.HasExactCount.Cases), MemberType = typeof(StringRulesGraphemesTestData.HasExactCount))]
    public void HasExactCount_BehavesAsExpected(RuleCase<(string? value, int count)> tc)
    {
        // Arrange
        var (value, count) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.Graphemes.HasExactCount(value, count);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesGraphemesTestData.HasMinCount.Cases), MemberType = typeof(StringRulesGraphemesTestData.HasMinCount))]
    public void HasMinCount_BehavesAsExpected(RuleCase<(string? value, int min)> tc)
    {
        // Arrange
        var (value, min) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.Graphemes.HasMinCount(value, min);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesGraphemesTestData.HasMaxCount.Cases), MemberType = typeof(StringRulesGraphemesTestData.HasMaxCount))]
    public void HasMaxCount_BehavesAsExpected(RuleCase<(string? value, int max)> tc)
    {
        // Arrange
        var (value, max) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.Graphemes.HasMaxCount(value, max);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesGraphemesTestData.HasCountBetween.Cases), MemberType = typeof(StringRulesGraphemesTestData.HasCountBetween))]
    public void HasCountBetween_BehavesAsExpected(RuleCase<(string? value, int min, int max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.Graphemes.HasCountBetween(value, min, max, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
