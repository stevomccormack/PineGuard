using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeOffsetRulesTests(ITestOutputHelper output) : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInPast.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInPast))]
    public void IsInPast_BehavesAsExpected(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsInPast(tc.Value, timeProvider: FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInPastSystemClock.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInPastSystemClock))]
    public void IsInPast_DefaultTimeProvider_ReadsTheSystemClock(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsInPast(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInFuture.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInFuture))]
    public void IsInFuture_BehavesAsExpected(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsInFuture(tc.Value, timeProvider: FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsInFutureSystemClock.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsInFutureSystemClock))]
    public void IsInFuture_DefaultTimeProvider_ReadsTheSystemClock(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsInFuture(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsBetween.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(RuleCase<(DateTimeOffset? value, DateTimeOffset min, DateTimeOffset max, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsBefore.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsBefore))]
    public void IsBefore_BehavesAsExpected(RuleCase<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsBefore(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsBeforeDefaultInclusion.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsBeforeDefaultInclusion))]
    public void IsBefore_DefaultInclusion_IsExclusive(RuleCase<(DateTimeOffset? value, DateTimeOffset? other)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsBefore(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsAfter.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsAfter))]
    public void IsAfter_BehavesAsExpected(RuleCase<(DateTimeOffset? value, DateTimeOffset? other, Inclusion inclusion, DateTimePrecision? precision)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsAfter(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsAfterDefaultInclusion.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsAfterDefaultInclusion))]
    public void IsAfter_DefaultInclusion_IsExclusive(RuleCase<(DateTimeOffset? value, DateTimeOffset? other)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsAfter(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsSame.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsSame))]
    public void IsSame_BehavesAsExpected(RuleCase<(DateTimeOffset? value, DateTimeOffset? other, DateTimePrecision? precision)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsSame(tc.Value.value, tc.Value.other, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsChronological.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(DateTimeOffset? start, DateTimeOffset? end, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsChronological(tc.Value.start, tc.Value.end, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsOverlapping.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(DateTimeOffset? start1, DateTimeOffset? end1, DateTimeOffset? start2, DateTimeOffset? end2, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsOverlapping(tc.Value.start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsWithin.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsWithin))]
    public void IsWithin_BehavesAsExpected(RuleCase<(DateTimeOffset? value, DateTimeOffset? reference, TimeSpan window)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsWithin(tc.Value.value, tc.Value.reference, tc.Value.window);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsWithinCalendarMonths.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsWithinCalendarMonths))]
    public void IsWithinCalendarMonths_BehavesAsExpected(RuleCase<(DateTimeOffset? value, DateTimeOffset? reference, int months)> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsWithinCalendarMonths(tc.Value.value, tc.Value.reference, tc.Value.months);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsWeekday.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsWeekday))]
    public void IsWeekday_BehavesAsExpected(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsWeekday(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsWeekend.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsWeekend))]
    public void IsWeekend_BehavesAsExpected(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsWeekend(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsFirstDayOfMonth.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsFirstDayOfMonth))]
    public void IsFirstDayOfMonth_BehavesAsExpected(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsFirstDayOfMonth(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRulesTestData.IsLastDayOfMonth.Cases), MemberType = typeof(DateTimeOffsetRulesTestData.IsLastDayOfMonth))]
    public void IsLastDayOfMonth_BehavesAsExpected(RuleCase<DateTimeOffset?> tc)
    {
        // Act
        var result = DateTimeOffsetRules.IsLastDayOfMonth(tc.Value);

        // Assert
        AssertResult(tc, result);
    }
}
