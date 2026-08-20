using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateOnlyRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsInPast.Cases), MemberType = typeof(DateOnlyRulesTestData.IsInPast))]
    public void IsInPast_BehavesAsExpected(RuleCase<DateOnly?> tc)
    {
        // Act
        var result = DateOnlyRules.IsInPast(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsInFuture.Cases), MemberType = typeof(DateOnlyRulesTestData.IsInFuture))]
    public void IsInFuture_BehavesAsExpected(RuleCase<DateOnly?> tc)
    {
        // Act
        var result = DateOnlyRules.IsInFuture(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsBetween.Cases), MemberType = typeof(DateOnlyRulesTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(RuleCase<(DateOnly? value, DateOnly min, DateOnly max, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateOnlyRules.IsBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsBefore.Cases), MemberType = typeof(DateOnlyRulesTestData.IsBefore))]
    public void IsBefore_BehavesAsExpected(RuleCase<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)> tc)
    {
        // Act
        var result = DateOnlyRules.IsBefore(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsBeforeDefaultInclusion.Cases), MemberType = typeof(DateOnlyRulesTestData.IsBeforeDefaultInclusion))]
    public void IsBefore_DefaultInclusion_IsExclusive(RuleCase<(DateOnly? value, DateOnly? other)> tc)
    {
        // Act
        var result = DateOnlyRules.IsBefore(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsAfter.Cases), MemberType = typeof(DateOnlyRulesTestData.IsAfter))]
    public void IsAfter_BehavesAsExpected(RuleCase<(DateOnly? value, DateOnly? other, Inclusion inclusion, DatePrecision? precision)> tc)
    {
        // Act
        var result = DateOnlyRules.IsAfter(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsAfterDefaultInclusion.Cases), MemberType = typeof(DateOnlyRulesTestData.IsAfterDefaultInclusion))]
    public void IsAfter_DefaultInclusion_IsExclusive(RuleCase<(DateOnly? value, DateOnly? other)> tc)
    {
        // Act
        var result = DateOnlyRules.IsAfter(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsSame.Cases), MemberType = typeof(DateOnlyRulesTestData.IsSame))]
    public void IsSame_BehavesAsExpected(RuleCase<(DateOnly? value, DateOnly? other, DatePrecision? precision)> tc)
    {
        // Act
        var result = DateOnlyRules.IsSame(tc.Value.value, tc.Value.other, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsChronological.Cases), MemberType = typeof(DateOnlyRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(DateOnly? start, DateOnly? end, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateOnlyRules.IsChronological(tc.Value.start, tc.Value.end, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsOverlapping.Cases), MemberType = typeof(DateOnlyRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(DateOnly? start1, DateOnly? end1, DateOnly? start2, DateOnly? end2, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateOnlyRules.IsOverlapping(tc.Value.start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsWithinCalendarMonths.Cases), MemberType = typeof(DateOnlyRulesTestData.IsWithinCalendarMonths))]
    public void IsWithinCalendarMonths_BehavesAsExpected(RuleCase<(DateOnly? value, DateOnly? reference, int months)> tc)
    {
        // Act
        var result = DateOnlyRules.IsWithinCalendarMonths(tc.Value.value, tc.Value.reference, tc.Value.months);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRulesTestData.IsWithin.Cases), MemberType = typeof(DateOnlyRulesTestData.IsWithin))]
    public void IsWithin_BehavesAsExpected(RuleCase<(DateOnly? value, DateOnly? reference, int days)> tc)
    {
        // Act
        var result = DateOnlyRules.IsWithin(tc.Value.value, tc.Value.reference, tc.Value.days);

        // Assert
        AssertResult(tc, result);
    }
}
