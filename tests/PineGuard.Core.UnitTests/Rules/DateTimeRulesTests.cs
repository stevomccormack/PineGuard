using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Rules;

public sealed class DateTimeRulesTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsInPast.Cases), MemberType = typeof(DateTimeRulesTestData.IsInPast))]
    public void IsInPast_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsInPast(tc.Value, timeProvider: FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsInPastSystemClock.Cases), MemberType = typeof(DateTimeRulesTestData.IsInPastSystemClock))]
    public void IsInPast_DefaultTimeProvider_ReadsTheSystemClock(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsInPast(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsInFuture.Cases), MemberType = typeof(DateTimeRulesTestData.IsInFuture))]
    public void IsInFuture_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsInFuture(tc.Value, timeProvider: FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsInFutureSystemClock.Cases), MemberType = typeof(DateTimeRulesTestData.IsInFutureSystemClock))]
    public void IsInFuture_DefaultTimeProvider_ReadsTheSystemClock(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsInFuture(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWithinDaysFromNow.Cases), MemberType = typeof(DateTimeRulesTestData.IsWithinDaysFromNow))]
    public void IsWithinDaysFromNow_BehavesAsExpected(RuleCase<(DateTime? value, int days)> tc)
    {
        // Act
        var result = DateTimeRules.IsWithinDaysFromNow(tc.Value.value, tc.Value.days, FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWithinDaysFromNowSystemClock.Cases), MemberType = typeof(DateTimeRulesTestData.IsWithinDaysFromNowSystemClock))]
    public void IsWithinDaysFromNow_DefaultTimeProvider_ReadsTheSystemClock(RuleCase<(DateTime? value, int days)> tc)
    {
        // Act
        var result = DateTimeRules.IsWithinDaysFromNow(tc.Value.value, tc.Value.days);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsBetween.Cases), MemberType = typeof(DateTimeRulesTestData.IsBetween))]
    public void IsBetween_BehavesAsExpected(RuleCase<(DateTime? value, DateTime min, DateTime max, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateTimeRules.IsBetween(tc.Value.value, tc.Value.min, tc.Value.max, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsBefore.Cases), MemberType = typeof(DateTimeRulesTestData.IsBefore))]
    public void IsBefore_BehavesAsExpected(RuleCase<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)> tc)
    {
        // Act
        var result = DateTimeRules.IsBefore(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsBeforeDefaultInclusion.Cases), MemberType = typeof(DateTimeRulesTestData.IsBeforeDefaultInclusion))]
    public void IsBefore_DefaultInclusion_IsExclusive(RuleCase<(DateTime? value, DateTime? other)> tc)
    {
        // Act
        var result = DateTimeRules.IsBefore(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsAfter.Cases), MemberType = typeof(DateTimeRulesTestData.IsAfter))]
    public void IsAfter_BehavesAsExpected(RuleCase<(DateTime? value, DateTime? other, Inclusion inclusion, DateTimePrecision? precision)> tc)
    {
        // Act
        var result = DateTimeRules.IsAfter(tc.Value.value, tc.Value.other, tc.Value.inclusion, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsAfterDefaultInclusion.Cases), MemberType = typeof(DateTimeRulesTestData.IsAfterDefaultInclusion))]
    public void IsAfter_DefaultInclusion_IsExclusive(RuleCase<(DateTime? value, DateTime? other)> tc)
    {
        // Act
        var result = DateTimeRules.IsAfter(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsSame.Cases), MemberType = typeof(DateTimeRulesTestData.IsSame))]
    public void IsSame_BehavesAsExpected(RuleCase<(DateTime? value, DateTime? other, DateTimePrecision? precision)> tc)
    {
        // Act
        var result = DateTimeRules.IsSame(tc.Value.value, tc.Value.other, tc.Value.precision);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsChronological.Cases), MemberType = typeof(DateTimeRulesTestData.IsChronological))]
    public void IsChronological_BehavesAsExpected(RuleCase<(DateTime? start, DateTime? end, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateTimeRules.IsChronological(tc.Value.start, tc.Value.end, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsOverlapping.Cases), MemberType = typeof(DateTimeRulesTestData.IsOverlapping))]
    public void IsOverlapping_BehavesAsExpected(RuleCase<(DateTime? start1, DateTime? end1, DateTime? start2, DateTime? end2, Inclusion inclusion)> tc)
    {
        // Act
        var result = DateTimeRules.IsOverlapping(tc.Value.start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWithin.Cases), MemberType = typeof(DateTimeRulesTestData.IsWithin))]
    public void IsWithin_BehavesAsExpected(RuleCase<(DateTime? value, DateTime? reference, TimeSpan window)> tc)
    {
        // Act
        var result = DateTimeRules.IsWithin(tc.Value.value, tc.Value.reference, tc.Value.window);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWithinCalendarMonths.Cases), MemberType = typeof(DateTimeRulesTestData.IsWithinCalendarMonths))]
    public void IsWithinCalendarMonths_BehavesAsExpected(RuleCase<(DateTime? value, DateTime? reference, int months)> tc)
    {
        // Act
        var result = DateTimeRules.IsWithinCalendarMonths(tc.Value.value, tc.Value.reference, tc.Value.months);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWeekday.Cases), MemberType = typeof(DateTimeRulesTestData.IsWeekday))]
    public void IsWeekday_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsWeekday(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsWeekend.Cases), MemberType = typeof(DateTimeRulesTestData.IsWeekend))]
    public void IsWeekend_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsWeekend(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsFirstDayOfMonth.Cases), MemberType = typeof(DateTimeRulesTestData.IsFirstDayOfMonth))]
    public void IsFirstDayOfMonth_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsFirstDayOfMonth(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsLastDayOfMonth.Cases), MemberType = typeof(DateTimeRulesTestData.IsLastDayOfMonth))]
    public void IsLastDayOfMonth_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsLastDayOfMonth(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsSameDay.Cases), MemberType = typeof(DateTimeRulesTestData.IsSameDay))]
    public void IsSameDay_BehavesAsExpected(RuleCase<(DateTime? value, DateTime? other)> tc)
    {
        // Act
        var result = DateTimeRules.IsSameDay(tc.Value.value, tc.Value.other);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsUtc.Cases), MemberType = typeof(DateTimeRulesTestData.IsUtc))]
    public void IsUtc_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsUtc(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsLocal.Cases), MemberType = typeof(DateTimeRulesTestData.IsLocal))]
    public void IsLocal_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsLocal(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.IsUnspecified.Cases), MemberType = typeof(DateTimeRulesTestData.IsUnspecified))]
    public void IsUnspecified_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.IsUnspecified(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.HasExplicitKind.Cases), MemberType = typeof(DateTimeRulesTestData.HasExplicitKind))]
    public void HasExplicitKind_BehavesAsExpected(RuleCase<DateTime?> tc)
    {
        // Act
        var result = DateTimeRules.HasExplicitKind(tc.Value);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.HasMinimumAge.Cases), MemberType = typeof(DateTimeRulesTestData.HasMinimumAge))]
    public void HasMinimumAge_BehavesAsExpected(RuleCase<(DateTime? value, int years)> tc)
    {
        // Arrange
        var (value, years) = tc.Value;

        // Act
        var result = DateTimeRules.HasMinimumAge(value, years, timeProvider: FixedTimeProvider.Default);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.HasMinimumAgeSystemClock.Cases), MemberType = typeof(DateTimeRulesTestData.HasMinimumAgeSystemClock))]
    public void HasMinimumAge_DefaultTimeProvider_ReadsTheSystemClock(RuleCase<(DateTime? value, int years)> tc)
    {
        // Arrange
        var (value, years) = tc.Value;

        // Act
        var result = DateTimeRules.HasMinimumAge(value, years);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRulesTestData.HasMinimumAgeOnLeapDay.Cases), MemberType = typeof(DateTimeRulesTestData.HasMinimumAgeOnLeapDay))]
    public void HasMinimumAge_LeapDayBirthDate_MaturesOnTheFirstOfMarch(RuleCase<(DateTime? value, int years, DateTimeOffset utcNow)> tc)
    {
        // Arrange
        var (value, years, utcNow) = tc.Value;

        // Act
        var result = DateTimeRules.HasMinimumAge(value, years, timeProvider: new FixedTimeProvider(utcNow));

        // Assert
        AssertResult(tc, result);
    }
}
