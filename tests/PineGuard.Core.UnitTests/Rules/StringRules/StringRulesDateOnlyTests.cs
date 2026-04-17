using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesDateOnlyTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesDateOnlyTestData.IsInPast.Cases), MemberType = typeof(StringRulesDateOnlyTestData.IsInPast))]
    public void IsInPast_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateOnly.IsInPast(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateOnlyTestData.IsInPast.DynamicCases), MemberType = typeof(StringRulesDateOnlyTestData.IsInPast))]
    public void IsInPast_Dynamic_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateOnly.IsInPast(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateOnlyTestData.IsInFuture.Cases), MemberType = typeof(StringRulesDateOnlyTestData.IsInFuture))]
    public void IsInFuture_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateOnly.IsInFuture(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateOnlyTestData.IsInFuture.DynamicCases), MemberType = typeof(StringRulesDateOnlyTestData.IsInFuture))]
    public void IsInFuture_Dynamic_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateOnly.IsInFuture(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateOnlyTestData.IsBetween.Cases), MemberType = typeof(StringRulesDateOnlyTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(RuleCase<(string? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.DateOnly.IsBetween(value, min, max, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
