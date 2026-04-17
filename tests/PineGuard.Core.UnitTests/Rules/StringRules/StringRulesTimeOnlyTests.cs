using PineGuard.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public sealed class StringRulesTimeOnlyTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsBetween.Cases), MemberType = typeof(StringRulesTimeOnlyTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(RuleCase<(string? value, TimeOnly min, TimeOnly max, Inclusion inclusion)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.TimeOnly.IsBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsBefore.Cases), MemberType = typeof(StringRulesTimeOnlyTestData.IsBefore))]
    public void IsBefore_BehavesAsExpected(RuleCase<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.TimeOnly.IsBefore(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsAfter.Cases), MemberType = typeof(StringRulesTimeOnlyTestData.IsAfter))]
    public void IsAfter_BehavesAsExpected(RuleCase<(string? value, TimeOnly other, Inclusion inclusion, TimePrecision? precision)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.TimeOnly.IsAfter(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsSame.Cases), MemberType = typeof(StringRulesTimeOnlyTestData.IsSame))]
    public void IsSame_BehavesAsExpected(RuleCase<(string? value, TimeOnly other, TimePrecision? precision)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.TimeOnly.IsSame(tc.Value.value, tc.Value.other, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsWithin.Cases), MemberType = typeof(StringRulesTimeOnlyTestData.IsWithin))]
    public void IsWithin_BehavesAsExpected(RuleCase<(string? value, string? reference, TimeSpan window)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.TimeOnly.IsWithin(tc.Value.value, tc.Value.reference, tc.Value.window);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsChronological.Cases), MemberType = typeof(StringRulesTimeOnlyTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(string? start, string? end, Inclusion inclusion)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.TimeOnly.IsChronological(tc.Value.start, tc.Value.end, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(StringRulesTimeOnlyTestData.IsOverlapping.Cases), MemberType = typeof(StringRulesTimeOnlyTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(string? start1, string? end1, string? start2, string? end2, Inclusion inclusion)> tc)
    {
        // Act
        var result = PineGuard.Rules.StringRules.TimeOnly.IsOverlapping(tc.Value.start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
