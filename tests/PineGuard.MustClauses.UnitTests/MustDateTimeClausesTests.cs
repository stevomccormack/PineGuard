using PineGuard.Testing.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public class MustDateTimeClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Past.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Past))]
    public void Past_Checks(MustDateTimeClausesTestData.Past.ValidCase testCase)
    {
        var result = Must.Be.Past(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Past.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Past))]
    public void Past_EdgeChecks(MustDateTimeClausesTestData.Past.EdgeCase testCase)
    {
        var result = Must.Be.Past(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.PastOrPresent.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.PastOrPresent))]
    public void PastOrPresent_Checks(MustDateTimeClausesTestData.PastOrPresent.ValidCase testCase)
    {
        var result = Must.Be.PastOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.PastOrPresent.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.PastOrPresent))]
    public void PastOrPresent_EdgeChecks(MustDateTimeClausesTestData.PastOrPresent.EdgeCase testCase)
    {
        var result = Must.Be.PastOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Future.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Future))]
    public void Future_Checks(MustDateTimeClausesTestData.Future.ValidCase testCase)
    {
        var result = Must.Be.Future(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Future.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Future))]
    public void Future_EdgeChecks(MustDateTimeClausesTestData.Future.EdgeCase testCase)
    {
        var result = Must.Be.Future(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.FutureOrPresent.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.FutureOrPresent))]
    public void FutureOrPresent_Checks(MustDateTimeClausesTestData.FutureOrPresent.ValidCase testCase)
    {
        var result = Must.Be.FutureOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.FutureOrPresent.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.FutureOrPresent))]
    public void FutureOrPresent_EdgeChecks(MustDateTimeClausesTestData.FutureOrPresent.EdgeCase testCase)
    {
        var result = Must.Be.FutureOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Between.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Between))]
    public void Between_Checks(MustDateTimeClausesTestData.Between.ValidCase testCase)
    {
        var result = Must.Be.Between(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Between.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Between))]
    public void Between_EdgeChecks(MustDateTimeClausesTestData.Between.EdgeCase testCase)
    {
        var result = Must.Be.Between(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotBetween.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotBetween))]
    public void NotBetween_Checks(MustDateTimeClausesTestData.NotBetween.ValidCase testCase)
    {
        var result = Must.Be.NotBetween(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotBetween.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.NotBetween))]
    public void NotBetween_EdgeChecks(MustDateTimeClausesTestData.NotBetween.EdgeCase testCase)
    {
        var result = Must.Be.NotBetween(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Before.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Before))]
    public void Before_Checks(MustDateTimeClausesTestData.Before.ValidCase testCase)
    {
        var result = Must.Be.Before(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Before.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Before))]
    public void Before_EdgeChecks(MustDateTimeClausesTestData.Before.EdgeCase testCase)
    {
        var result = Must.Be.Before(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.OnOrBefore.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.OnOrBefore))]
    public void OnOrBefore_Checks(MustDateTimeClausesTestData.OnOrBefore.ValidCase testCase)
    {
        var result = Must.Be.OnOrBefore(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.OnOrBefore.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.OnOrBefore))]
    public void OnOrBefore_EdgeChecks(MustDateTimeClausesTestData.OnOrBefore.EdgeCase testCase)
    {
        var result = Must.Be.OnOrBefore(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.After.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.After))]
    public void After_Checks(MustDateTimeClausesTestData.After.ValidCase testCase)
    {
        var result = Must.Be.After(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.After.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.After))]
    public void After_EdgeChecks(MustDateTimeClausesTestData.After.EdgeCase testCase)
    {
        var result = Must.Be.After(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.OnOrAfter.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.OnOrAfter))]
    public void OnOrAfter_Checks(MustDateTimeClausesTestData.OnOrAfter.ValidCase testCase)
    {
        var result = Must.Be.OnOrAfter(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.OnOrAfter.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.OnOrAfter))]
    public void OnOrAfter_EdgeChecks(MustDateTimeClausesTestData.OnOrAfter.EdgeCase testCase)
    {
        var result = Must.Be.OnOrAfter(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Same.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Same))]
    public void Same_Checks(MustDateTimeClausesTestData.Same.ValidCase testCase)
    {
        var result = Must.Be.Same(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Same.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Same))]
    public void Same_EdgeChecks(MustDateTimeClausesTestData.Same.EdgeCase testCase)
    {
        var result = Must.Be.Same(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotSame.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotSame))]
    public void NotSame_Checks(MustDateTimeClausesTestData.NotSame.ValidCase testCase)
    {
        var result = Must.Be.NotSame(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotSame.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.NotSame))]
    public void NotSame_EdgeChecks(MustDateTimeClausesTestData.NotSame.EdgeCase testCase)
    {
        var result = Must.Be.NotSame(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Chronological))]
    public void Chronological_Checks(MustDateTimeClausesTestData.Chronological.ValidCase testCase)
    {
        var result = Must.Be.Chronological(testCase.Value.start, testCase.Value.end);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Overlapping))]
    public void Overlapping_Checks(MustDateTimeClausesTestData.Overlapping.ValidCase testCase)
    {
        var result = Must.Be.Overlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Overlapping.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Overlapping))]
    public void Overlapping_EdgeChecks(MustDateTimeClausesTestData.Overlapping.EdgeCase testCase)
    {
        var result = Must.Be.Overlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotOverlapping))]
    public void NotOverlapping_Checks(MustDateTimeClausesTestData.NotOverlapping.ValidCase testCase)
    {
        var result = Must.Be.NotOverlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotOverlapping.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.NotOverlapping))]
    public void NotOverlapping_EdgeChecks(MustDateTimeClausesTestData.NotOverlapping.EdgeCase testCase)
    {
        var result = Must.Be.NotOverlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.WithinDaysFromNow.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.WithinDaysFromNow))]
    public void WithinDaysFromNow_Checks(MustDateTimeClausesTestData.WithinDaysFromNow.ValidCase testCase)
    {
        var result = Must.Be.WithinDaysFromNow(testCase.Value.value, testCase.Value.days);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotWithinDaysFromNow.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotWithinDaysFromNow))]
    public void NotWithinDaysFromNow_Checks(MustDateTimeClausesTestData.NotWithinDaysFromNow.ValidCase testCase)
    {
        var result = Must.Be.NotWithinDaysFromNow(testCase.Value.value, testCase.Value.days);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Within.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Within))]
    public void Within_Checks(MustDateTimeClausesTestData.Within.ValidCase testCase)
    {
        var result = Must.Be.Within(testCase.Value.value, testCase.Value.reference, testCase.Value.window);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotWithin.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotWithin))]
    public void NotWithin_Checks(MustDateTimeClausesTestData.NotWithin.ValidCase testCase)
    {
        var result = Must.Be.NotWithin(testCase.Value.value, testCase.Value.reference, testCase.Value.window);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.WithinCalendarMonths.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.WithinCalendarMonths))]
    public void WithinCalendarMonths_Checks(MustDateTimeClausesTestData.WithinCalendarMonths.ValidCase testCase)
    {
        var result = Must.Be.WithinCalendarMonths(testCase.Value.value, testCase.Value.reference, testCase.Value.months);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotWithinCalendarMonths.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_Checks(MustDateTimeClausesTestData.NotWithinCalendarMonths.ValidCase testCase)
    {
        var result = Must.Be.NotWithinCalendarMonths(testCase.Value.value, testCase.Value.reference, testCase.Value.months);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Weekday.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Weekday))]
    public void Weekday_Checks(MustDateTimeClausesTestData.Weekday.ValidCase testCase)
    {
        var result = Must.Be.Weekday(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Weekend.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Weekend))]
    public void Weekend_Checks(MustDateTimeClausesTestData.Weekend.ValidCase testCase)
    {
        var result = Must.Be.Weekend(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.FirstDayOfMonth.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.FirstDayOfMonth))]
    public void FirstDayOfMonth_Checks(MustDateTimeClausesTestData.FirstDayOfMonth.ValidCase testCase)
    {
        var result = Must.Be.FirstDayOfMonth(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotFirstDayOfMonth.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotFirstDayOfMonth))]
    public void NotFirstDayOfMonth_Checks(MustDateTimeClausesTestData.NotFirstDayOfMonth.ValidCase testCase)
    {
        var result = Must.Be.NotFirstDayOfMonth(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.LastDayOfMonth.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.LastDayOfMonth))]
    public void LastDayOfMonth_Checks(MustDateTimeClausesTestData.LastDayOfMonth.ValidCase testCase)
    {
        var result = Must.Be.LastDayOfMonth(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotLastDayOfMonth.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotLastDayOfMonth))]
    public void NotLastDayOfMonth_Checks(MustDateTimeClausesTestData.NotLastDayOfMonth.ValidCase testCase)
    {
        var result = Must.Be.NotLastDayOfMonth(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.SameDay.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.SameDay))]
    public void SameDay_Checks(MustDateTimeClausesTestData.SameDay.ValidCase testCase)
    {
        var result = Must.Be.SameDay(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotSameDay.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotSameDay))]
    public void NotSameDay_Checks(MustDateTimeClausesTestData.NotSameDay.ValidCase testCase)
    {
        var result = Must.Be.NotSameDay(testCase.Value.value, testCase.Value.other);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Utc.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Utc))]
    public void Utc_Checks(MustDateTimeClausesTestData.Utc.ValidCase testCase)
    {
        var result = Must.Be.Utc(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Utc.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Utc))]
    public void Utc_EdgeChecks(MustDateTimeClausesTestData.Utc.EdgeCase testCase)
    {
        var result = Must.Be.Utc(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotUtc.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotUtc))]
    public void NotUtc_Checks(MustDateTimeClausesTestData.NotUtc.ValidCase testCase)
    {
        var result = Must.Be.NotUtc(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotUtc.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.NotUtc))]
    public void NotUtc_EdgeChecks(MustDateTimeClausesTestData.NotUtc.EdgeCase testCase)
    {
        var result = Must.Be.NotUtc(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Local.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Local))]
    public void Local_Checks(MustDateTimeClausesTestData.Local.ValidCase testCase)
    {
        var result = Must.Be.Local(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Local.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Local))]
    public void Local_EdgeChecks(MustDateTimeClausesTestData.Local.EdgeCase testCase)
    {
        var result = Must.Be.Local(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotLocal.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotLocal))]
    public void NotLocal_Checks(MustDateTimeClausesTestData.NotLocal.ValidCase testCase)
    {
        var result = Must.Be.NotLocal(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotLocal.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.NotLocal))]
    public void NotLocal_EdgeChecks(MustDateTimeClausesTestData.NotLocal.EdgeCase testCase)
    {
        var result = Must.Be.NotLocal(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Unspecified.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.Unspecified))]
    public void Unspecified_Checks(MustDateTimeClausesTestData.Unspecified.ValidCase testCase)
    {
        var result = Must.Be.Unspecified(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.Unspecified.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.Unspecified))]
    public void Unspecified_EdgeChecks(MustDateTimeClausesTestData.Unspecified.EdgeCase testCase)
    {
        var result = Must.Be.Unspecified(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotUnspecified.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotUnspecified))]
    public void NotUnspecified_Checks(MustDateTimeClausesTestData.NotUnspecified.ValidCase testCase)
    {
        var result = Must.Be.NotUnspecified(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotUnspecified.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.NotUnspecified))]
    public void NotUnspecified_EdgeChecks(MustDateTimeClausesTestData.NotUnspecified.EdgeCase testCase)
    {
        var result = Must.Be.NotUnspecified(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.ExplicitKind.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.ExplicitKind))]
    public void ExplicitKind_Checks(MustDateTimeClausesTestData.ExplicitKind.ValidCase testCase)
    {
        var result = Must.Be.ExplicitKind(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.ExplicitKind.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.ExplicitKind))]
    public void ExplicitKind_EdgeChecks(MustDateTimeClausesTestData.ExplicitKind.EdgeCase testCase)
    {
        var result = Must.Be.ExplicitKind(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotExplicitKind.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.NotExplicitKind))]
    public void NotExplicitKind_Checks(MustDateTimeClausesTestData.NotExplicitKind.ValidCase testCase)
    {
        var result = Must.Be.NotExplicitKind(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.NotExplicitKind.EdgeCases), MemberType = typeof(MustDateTimeClausesTestData.NotExplicitKind))]
    public void NotExplicitKind_EdgeChecks(MustDateTimeClausesTestData.NotExplicitKind.EdgeCase testCase)
    {
        var result = Must.Be.NotExplicitKind(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.MinimumAge.ValidCases), MemberType = typeof(MustDateTimeClausesTestData.MinimumAge))]
    [MemberData(nameof(MustDateTimeClausesTestData.MinimumAge.InvalidCases), MemberType = typeof(MustDateTimeClausesTestData.MinimumAge))]
    public void MinimumAge_BehavesAsExpected(MustCase<(DateTime value, int years)> tc)
    {
        // Arrange
        var (value, years) = tc.Value;

        // Act
        var result = Must.Be.MinimumAge(value, years, FixedTimeProvider.Default, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeClausesTestData.MinimumAgeOnLeapDay.Cases), MemberType = typeof(MustDateTimeClausesTestData.MinimumAgeOnLeapDay))]
    public void MinimumAge_LeapDayBirthDate_MaturesOnTheFirstOfMarch(MustCase<(DateTime value, int years, DateTimeOffset utcNow)> tc)
    {
        // Arrange
        var (value, years, utcNow) = tc.Value;

        // Act
        var result = Must.Be.MinimumAge(value, years, new FixedTimeProvider(utcNow), paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
