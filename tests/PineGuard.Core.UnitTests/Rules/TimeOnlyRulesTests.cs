using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class TimeOnlyRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsBetween.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(RuleCase<(TimeOnly? value, TimeOnly min, TimeOnly max, Inclusion inclusion)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsBefore.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsBefore))]
    public void IsBefore_BehavesAsExpected(RuleCase<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsBefore(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsBeforeDefaultInclusion.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsBeforeDefaultInclusion))]
    public void IsBefore_DefaultInclusion_IsExclusive(RuleCase<(TimeOnly? value, TimeOnly? other)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsBefore(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsAfter.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsAfter))]
    public void IsAfter_BehavesAsExpected(RuleCase<(TimeOnly? value, TimeOnly? other, Inclusion inclusion, TimePrecision? precision)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsAfter(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsAfterDefaultInclusion.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsAfterDefaultInclusion))]
    public void IsAfter_DefaultInclusion_IsExclusive(RuleCase<(TimeOnly? value, TimeOnly? other)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsAfter(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsSame.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsSame))]
    public void IsSame_BehavesAsExpected(RuleCase<(TimeOnly? value, TimeOnly? other, TimePrecision? precision)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsSame(tc.Value.value, tc.Value.other, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsWithin.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsWithin))]
    public void IsWithin_BehavesAsExpected(RuleCase<(TimeOnly? value, TimeOnly? reference, TimeSpan window)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsWithin(tc.Value.value, tc.Value.reference, tc.Value.window);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsChronological.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(TimeOnly? start, TimeOnly? end, Inclusion inclusion)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsChronological(tc.Value.start, tc.Value.end, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRulesTestData.IsOverlapping.Cases), MemberType = typeof(TimeOnlyRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(TimeOnly? start1, TimeOnly? end1, TimeOnly? start2, TimeOnly? end2, Inclusion inclusion)> tc)
    {
        // Act
        var result = TimeOnlyRules.IsOverlapping(tc.Value.start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }
}
