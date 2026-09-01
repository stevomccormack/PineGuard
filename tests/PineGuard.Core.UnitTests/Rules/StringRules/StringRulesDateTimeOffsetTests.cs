using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesDateTimeOffsetTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInPast.Cases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInPast))]
    public void IsInPast_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsInPast(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInPast.PinnedClockCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInPast))]
    public void IsInPast_PinnedTimeProvider_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsInPast(tc.Value, timeProvider: FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInFuture.Cases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInFuture))]
    public void IsInFuture_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsInFuture(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsInFuture.PinnedClockCases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsInFuture))]
    public void IsInFuture_PinnedTimeProvider_BehavesAsExpected(RuleCase<string?> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsInFuture(tc.Value, timeProvider: FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesDateTimeOffsetTestData.IsBetween.Cases), MemberType = typeof(StringRulesDateTimeOffsetTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(RuleCase<(string? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)> tc)
    {
        // Arrange
        var (value, min, max, inclusion) = tc.Value;

        // Act
        var result = PineGuard.Rules.StringRules.DateTimeOffset.IsBetween(value, min, max, inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
