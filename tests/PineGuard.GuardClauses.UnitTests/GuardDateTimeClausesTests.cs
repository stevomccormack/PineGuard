using PineGuard.Common;
using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardDateTimeClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardDateTimeClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.FutureOrPresent.ValidCases), MemberType = typeof(TD.FutureOrPresent))]
    [MemberData(nameof(TD.FutureOrPresent.InvalidCases), MemberType = typeof(TD.FutureOrPresent))]
    public void FutureOrPresent_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.FutureOrPresent(value)); AssertCustomMessage(tc, () => Guard.Against.FutureOrPresent(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Future.ValidCases), MemberType = typeof(TD.Future))]
    [MemberData(nameof(TD.Future.InvalidCases), MemberType = typeof(TD.Future))]
    public void Future_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.Future(value)); AssertCustomMessage(tc, () => Guard.Against.Future(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.PastOrPresent.ValidCases), MemberType = typeof(TD.PastOrPresent))]
    [MemberData(nameof(TD.PastOrPresent.InvalidCases), MemberType = typeof(TD.PastOrPresent))]
    public void PastOrPresent_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.PastOrPresent(value)); AssertCustomMessage(tc, () => Guard.Against.PastOrPresent(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Past.ValidCases), MemberType = typeof(TD.Past))]
    [MemberData(nameof(TD.Past.InvalidCases), MemberType = typeof(TD.Past))]
    public void Past_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.Past(value)); AssertCustomMessage(tc, () => Guard.Against.Past(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotBetween.ValidCases), MemberType = typeof(TD.NotBetween))]
    [MemberData(nameof(TD.NotBetween.InvalidCases), MemberType = typeof(TD.NotBetween))]
    public void NotBetween_BehavesAsExpected(GuardCase<(DateTime value, DateTime min, DateTime max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.NotBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Between.ValidCases), MemberType = typeof(TD.Between))]
    [MemberData(nameof(TD.Between.InvalidCases), MemberType = typeof(TD.Between))]
    public void Between_BehavesAsExpected(GuardCase<(DateTime value, DateTime min, DateTime max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.Between(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.Between(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.OnOrAfter.ValidCases), MemberType = typeof(TD.OnOrAfter))]
    [MemberData(nameof(TD.OnOrAfter.InvalidCases), MemberType = typeof(TD.OnOrAfter))]
    public void OnOrAfter_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.OnOrAfter(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.OnOrAfter(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.After.ValidCases), MemberType = typeof(TD.After))]
    [MemberData(nameof(TD.After.InvalidCases), MemberType = typeof(TD.After))]
    public void After_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.After(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.After(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.OnOrBefore.ValidCases), MemberType = typeof(TD.OnOrBefore))]
    [MemberData(nameof(TD.OnOrBefore.InvalidCases), MemberType = typeof(TD.OnOrBefore))]
    public void OnOrBefore_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.OnOrBefore(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.OnOrBefore(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Before.ValidCases), MemberType = typeof(TD.Before))]
    [MemberData(nameof(TD.Before.InvalidCases), MemberType = typeof(TD.Before))]
    public void Before_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.Before(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.Before(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotSame.ValidCases), MemberType = typeof(TD.NotSame))]
    [MemberData(nameof(TD.NotSame.InvalidCases), MemberType = typeof(TD.NotSame))]
    public void NotSame_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotSame(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.NotSame(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Same.ValidCases), MemberType = typeof(TD.Same))]
    [MemberData(nameof(TD.Same.InvalidCases), MemberType = typeof(TD.Same))]
    public void Same_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.Same(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.Same(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotChronological.ValidCases), MemberType = typeof(TD.NotChronological))]
    [MemberData(nameof(TD.NotChronological.InvalidCases), MemberType = typeof(TD.NotChronological))]
    public void NotChronological_BehavesAsExpected(GuardCase<(DateTime start, DateTime end, Inclusion inclusion)> tc)
    { var start = tc.Value.start; var result = AssertResult(tc, () => Guard.Against.NotChronological(start, tc.Value.end, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.NotChronological(start, tc.Value.end, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(start, result); }

    [Theory]
    [MemberData(nameof(TD.Overlapping.ValidCases), MemberType = typeof(TD.Overlapping))]
    [MemberData(nameof(TD.Overlapping.InvalidCases), MemberType = typeof(TD.Overlapping))]
    public void Overlapping_BehavesAsExpected(GuardCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2, Inclusion inclusion)> tc)
    { var start1 = tc.Value.start1; var result = AssertResult(tc, () => Guard.Against.Overlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.Overlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(start1, result); }

    [Theory]
    [MemberData(nameof(TD.NotOverlapping.ValidCases), MemberType = typeof(TD.NotOverlapping))]
    [MemberData(nameof(TD.NotOverlapping.InvalidCases), MemberType = typeof(TD.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(GuardCase<(DateTime start1, DateTime end1, DateTime start2, DateTime end2, Inclusion inclusion)> tc)
    { var start1 = tc.Value.start1; var result = AssertResult(tc, () => Guard.Against.NotOverlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.NotOverlapping(start1, tc.Value.end1, tc.Value.start2, tc.Value.end2, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(start1, result); }

    [Theory]
    [MemberData(nameof(TD.NotWithin.ValidCases), MemberType = typeof(TD.NotWithin))]
    [MemberData(nameof(TD.NotWithin.InvalidCases), MemberType = typeof(TD.NotWithin))]
    public void NotWithin_BehavesAsExpected(GuardCase<(DateTime value, DateTime reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotWithin(value, tc.Value.reference, tc.Value.window)); AssertCustomMessage(tc, () => Guard.Against.NotWithin(value, tc.Value.reference, tc.Value.window, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Within.ValidCases), MemberType = typeof(TD.Within))]
    [MemberData(nameof(TD.Within.InvalidCases), MemberType = typeof(TD.Within))]
    public void Within_BehavesAsExpected(GuardCase<(DateTime value, DateTime reference, TimeSpan window)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.Within(value, tc.Value.reference, tc.Value.window)); AssertCustomMessage(tc, () => Guard.Against.Within(value, tc.Value.reference, tc.Value.window, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotWithinDaysFromNow.ValidCases), MemberType = typeof(TD.NotWithinDaysFromNow))]
    [MemberData(nameof(TD.NotWithinDaysFromNow.InvalidCases), MemberType = typeof(TD.NotWithinDaysFromNow))]
    public void NotWithinDaysFromNow_BehavesAsExpected(GuardCase<(DateTime value, int days)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotWithinDaysFromNow(value, tc.Value.days)); AssertCustomMessage(tc, () => Guard.Against.NotWithinDaysFromNow(value, tc.Value.days, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.WithinDaysFromNow.ValidCases), MemberType = typeof(TD.WithinDaysFromNow))]
    [MemberData(nameof(TD.WithinDaysFromNow.InvalidCases), MemberType = typeof(TD.WithinDaysFromNow))]
    public void WithinDaysFromNow_BehavesAsExpected(GuardCase<(DateTime value, int days)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.WithinDaysFromNow(value, tc.Value.days)); AssertCustomMessage(tc, () => Guard.Against.WithinDaysFromNow(value, tc.Value.days, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotWithinCalendarMonths.ValidCases), MemberType = typeof(TD.NotWithinCalendarMonths))]
    [MemberData(nameof(TD.NotWithinCalendarMonths.InvalidCases), MemberType = typeof(TD.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_BehavesAsExpected(GuardCase<(DateTime value, DateTime reference, int months)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotWithinCalendarMonths(value, tc.Value.reference, tc.Value.months)); AssertCustomMessage(tc, () => Guard.Against.NotWithinCalendarMonths(value, tc.Value.reference, tc.Value.months, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.WithinCalendarMonths.ValidCases), MemberType = typeof(TD.WithinCalendarMonths))]
    [MemberData(nameof(TD.WithinCalendarMonths.InvalidCases), MemberType = typeof(TD.WithinCalendarMonths))]
    public void WithinCalendarMonths_BehavesAsExpected(GuardCase<(DateTime value, DateTime reference, int months)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.WithinCalendarMonths(value, tc.Value.reference, tc.Value.months)); AssertCustomMessage(tc, () => Guard.Against.WithinCalendarMonths(value, tc.Value.reference, tc.Value.months, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Weekend.ValidCases), MemberType = typeof(TD.Weekend))]
    [MemberData(nameof(TD.Weekend.InvalidCases), MemberType = typeof(TD.Weekend))]
    public void Weekend_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.Weekend(value)); AssertCustomMessage(tc, () => Guard.Against.Weekend(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Weekday.ValidCases), MemberType = typeof(TD.Weekday))]
    [MemberData(nameof(TD.Weekday.InvalidCases), MemberType = typeof(TD.Weekday))]
    public void Weekday_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.Weekday(value)); AssertCustomMessage(tc, () => Guard.Against.Weekday(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotFirstDayOfMonth.ValidCases), MemberType = typeof(TD.NotFirstDayOfMonth))]
    [MemberData(nameof(TD.NotFirstDayOfMonth.InvalidCases), MemberType = typeof(TD.NotFirstDayOfMonth))]
    public void NotFirstDayOfMonth_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.NotFirstDayOfMonth(value)); AssertCustomMessage(tc, () => Guard.Against.NotFirstDayOfMonth(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.FirstDayOfMonth.ValidCases), MemberType = typeof(TD.FirstDayOfMonth))]
    [MemberData(nameof(TD.FirstDayOfMonth.InvalidCases), MemberType = typeof(TD.FirstDayOfMonth))]
    public void FirstDayOfMonth_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.FirstDayOfMonth(value)); AssertCustomMessage(tc, () => Guard.Against.FirstDayOfMonth(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotLastDayOfMonth.ValidCases), MemberType = typeof(TD.NotLastDayOfMonth))]
    [MemberData(nameof(TD.NotLastDayOfMonth.InvalidCases), MemberType = typeof(TD.NotLastDayOfMonth))]
    public void NotLastDayOfMonth_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.NotLastDayOfMonth(value)); AssertCustomMessage(tc, () => Guard.Against.NotLastDayOfMonth(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.LastDayOfMonth.ValidCases), MemberType = typeof(TD.LastDayOfMonth))]
    [MemberData(nameof(TD.LastDayOfMonth.InvalidCases), MemberType = typeof(TD.LastDayOfMonth))]
    public void LastDayOfMonth_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.LastDayOfMonth(value)); AssertCustomMessage(tc, () => Guard.Against.LastDayOfMonth(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotSameDay.ValidCases), MemberType = typeof(TD.NotSameDay))]
    [MemberData(nameof(TD.NotSameDay.InvalidCases), MemberType = typeof(TD.NotSameDay))]
    public void NotSameDay_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotSameDay(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.NotSameDay(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.SameDay.ValidCases), MemberType = typeof(TD.SameDay))]
    [MemberData(nameof(TD.SameDay.InvalidCases), MemberType = typeof(TD.SameDay))]
    public void SameDay_BehavesAsExpected(GuardCase<(DateTime value, DateTime other)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.SameDay(value, tc.Value.other)); AssertCustomMessage(tc, () => Guard.Against.SameDay(value, tc.Value.other, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotUtc.ValidCases), MemberType = typeof(TD.NotUtc))]
    [MemberData(nameof(TD.NotUtc.InvalidCases), MemberType = typeof(TD.NotUtc))]
    public void NotUtc_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.NotUtc(value)); AssertCustomMessage(tc, () => Guard.Against.NotUtc(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Utc.ValidCases), MemberType = typeof(TD.Utc))]
    [MemberData(nameof(TD.Utc.InvalidCases), MemberType = typeof(TD.Utc))]
    public void Utc_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.Utc(value)); AssertCustomMessage(tc, () => Guard.Against.Utc(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotLocal.ValidCases), MemberType = typeof(TD.NotLocal))]
    [MemberData(nameof(TD.NotLocal.InvalidCases), MemberType = typeof(TD.NotLocal))]
    public void NotLocal_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.NotLocal(value)); AssertCustomMessage(tc, () => Guard.Against.NotLocal(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Local.ValidCases), MemberType = typeof(TD.Local))]
    [MemberData(nameof(TD.Local.InvalidCases), MemberType = typeof(TD.Local))]
    public void Local_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.Local(value)); AssertCustomMessage(tc, () => Guard.Against.Local(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotUnspecified.ValidCases), MemberType = typeof(TD.NotUnspecified))]
    [MemberData(nameof(TD.NotUnspecified.InvalidCases), MemberType = typeof(TD.NotUnspecified))]
    public void NotUnspecified_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.NotUnspecified(value)); AssertCustomMessage(tc, () => Guard.Against.NotUnspecified(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.Unspecified.ValidCases), MemberType = typeof(TD.Unspecified))]
    [MemberData(nameof(TD.Unspecified.InvalidCases), MemberType = typeof(TD.Unspecified))]
    public void Unspecified_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.Unspecified(value)); AssertCustomMessage(tc, () => Guard.Against.Unspecified(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.NotExplicitKind.ValidCases), MemberType = typeof(TD.NotExplicitKind))]
    [MemberData(nameof(TD.NotExplicitKind.InvalidCases), MemberType = typeof(TD.NotExplicitKind))]
    public void NotExplicitKind_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.NotExplicitKind(value)); AssertCustomMessage(tc, () => Guard.Against.NotExplicitKind(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.ExplicitKind.ValidCases), MemberType = typeof(TD.ExplicitKind))]
    [MemberData(nameof(TD.ExplicitKind.InvalidCases), MemberType = typeof(TD.ExplicitKind))]
    public void ExplicitKind_BehavesAsExpected(GuardCase<DateTime> tc)
    { var value = tc.Value; var result = AssertResult(tc, () => Guard.Against.ExplicitKind(value)); AssertCustomMessage(tc, () => Guard.Against.ExplicitKind(value, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.BelowMinimumAge.ValidCases), MemberType = typeof(TD.BelowMinimumAge))]
    [MemberData(nameof(TD.BelowMinimumAge.InvalidCases), MemberType = typeof(TD.BelowMinimumAge))]
    public void BelowMinimumAge_BehavesAsExpected(GuardCase<(DateTime value, int years)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, FixedTimeProvider.Default)); AssertCustomMessage(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, FixedTimeProvider.Default, message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }

    [Theory]
    [MemberData(nameof(TD.BelowMinimumAgeOnLeapDay.Cases), MemberType = typeof(TD.BelowMinimumAgeOnLeapDay))]
    public void BelowMinimumAge_LeapDayBirthDate_MaturesOnTheFirstOfMarch(GuardCase<(DateTime value, int years, DateTimeOffset utcNow)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, new FixedTimeProvider(tc.Value.utcNow))); AssertCustomMessage(tc, () => Guard.Against.BelowMinimumAge(value, tc.Value.years, new FixedTimeProvider(tc.Value.utcNow), message: CustomMessage)); if (tc.Expected.IsValid) Assert.Equal(value, result); }
}
