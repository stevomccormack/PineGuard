using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustDateOnlyClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Past.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.Past))]
    public void Past_Checks(MustDateOnlyClausesTestData.Past.ValidCase testCase)
    {
        var result = Must.Be.Past(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Past.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.Past))]
    public void Past_EdgeChecks(MustDateOnlyClausesTestData.Past.EdgeCase testCase)
    {
        var result = Must.Be.Past(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.PastOrPresent.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.PastOrPresent))]
    public void PastOrPresent_Checks(MustDateOnlyClausesTestData.PastOrPresent.ValidCase testCase)
    {
        var result = Must.Be.PastOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.PastOrPresent.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.PastOrPresent))]
    public void PastOrPresent_EdgeChecks(MustDateOnlyClausesTestData.PastOrPresent.EdgeCase testCase)
    {
        var result = Must.Be.PastOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Future.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.Future))]
    public void Future_Checks(MustDateOnlyClausesTestData.Future.ValidCase testCase)
    {
        var result = Must.Be.Future(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Future.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.Future))]
    public void Future_EdgeChecks(MustDateOnlyClausesTestData.Future.EdgeCase testCase)
    {
        var result = Must.Be.Future(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.FutureOrPresent.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.FutureOrPresent))]
    public void FutureOrPresent_Checks(MustDateOnlyClausesTestData.FutureOrPresent.ValidCase testCase)
    {
        var result = Must.Be.FutureOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.FutureOrPresent.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.FutureOrPresent))]
    public void FutureOrPresent_EdgeChecks(MustDateOnlyClausesTestData.FutureOrPresent.EdgeCase testCase)
    {
        var result = Must.Be.FutureOrPresent(testCase.Value);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Between.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.Between))]
    public void Between_Checks(MustDateOnlyClausesTestData.Between.ValidCase testCase)
    {
        var result = Must.Be.Between(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Between.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.Between))]
    public void Between_EdgeChecks(MustDateOnlyClausesTestData.Between.EdgeCase testCase)
    {
        var result = Must.Be.Between(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotBetween.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.NotBetween))]
    public void NotBetween_Checks(MustDateOnlyClausesTestData.NotBetween.ValidCase testCase)
    {
        var result = Must.Be.NotBetween(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotBetween.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.NotBetween))]
    public void NotBetween_EdgeChecks(MustDateOnlyClausesTestData.NotBetween.EdgeCase testCase)
    {
        var result = Must.Be.NotBetween(testCase.Value.value, testCase.Value.min, testCase.Value.max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Before.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.Before))]
    public void Before_Checks(MustDateOnlyClausesTestData.Before.ValidCase testCase)
    {
        var result = Must.Be.Before(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Before.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.Before))]
    public void Before_EdgeChecks(MustDateOnlyClausesTestData.Before.EdgeCase testCase)
    {
        var result = Must.Be.Before(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.OnOrBefore.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.OnOrBefore))]
    public void OnOrBefore_Checks(MustDateOnlyClausesTestData.OnOrBefore.ValidCase testCase)
    {
        var result = Must.Be.OnOrBefore(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.OnOrBefore.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.OnOrBefore))]
    public void OnOrBefore_EdgeChecks(MustDateOnlyClausesTestData.OnOrBefore.EdgeCase testCase)
    {
        var result = Must.Be.OnOrBefore(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.After.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.After))]
    public void After_Checks(MustDateOnlyClausesTestData.After.ValidCase testCase)
    {
        var result = Must.Be.After(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.After.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.After))]
    public void After_EdgeChecks(MustDateOnlyClausesTestData.After.EdgeCase testCase)
    {
        var result = Must.Be.After(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.OnOrAfter.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.OnOrAfter))]
    public void OnOrAfter_Checks(MustDateOnlyClausesTestData.OnOrAfter.ValidCase testCase)
    {
        var result = Must.Be.OnOrAfter(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.OnOrAfter.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.OnOrAfter))]
    public void OnOrAfter_EdgeChecks(MustDateOnlyClausesTestData.OnOrAfter.EdgeCase testCase)
    {
        var result = Must.Be.OnOrAfter(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Same.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.Same))]
    public void Same_Checks(MustDateOnlyClausesTestData.Same.ValidCase testCase)
    {
        var result = Must.Be.Same(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Same.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.Same))]
    public void Same_EdgeChecks(MustDateOnlyClausesTestData.Same.EdgeCase testCase)
    {
        var result = Must.Be.Same(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotSame.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.NotSame))]
    public void NotSame_Checks(MustDateOnlyClausesTestData.NotSame.ValidCase testCase)
    {
        var result = Must.Be.NotSame(testCase.Value.value, testCase.Value.target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.Chronological))]
    public void Chronological_Checks(MustDateOnlyClausesTestData.Chronological.ValidCase testCase)
    {
        var min = testCase.Value.min;
        var max = testCase.Value.max;
        var result = Must.Be.Chronological(min, max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Chronological.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.Chronological))]
    public void Chronological_EdgeChecks(MustDateOnlyClausesTestData.Chronological.EdgeCase testCase)
    {
        var min = testCase.Value.min;
        var max = testCase.Value.max;
        var result = Must.Be.Chronological(min, max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotChronological.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.NotChronological))]
    public void NotChronological_Checks(MustDateOnlyClausesTestData.NotChronological.ValidCase testCase)
    {
        var min = testCase.Value.min;
        var max = testCase.Value.max;
        var result = Must.Be.NotChronological(min, max);
        Assert.Equal(testCase.Expected, result.Success);
    }


    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.Overlapping))]
    public void Overlapping_Checks(MustDateOnlyClausesTestData.Overlapping.ValidCase testCase)
    {
        var result = Must.Be.Overlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.Overlapping.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.Overlapping))]
    public void Overlapping_EdgeChecks(MustDateOnlyClausesTestData.Overlapping.EdgeCase testCase)
    {
        var result = Must.Be.Overlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.NotOverlapping))]
    public void NotOverlapping_Checks(MustDateOnlyClausesTestData.NotOverlapping.ValidCase testCase)
    {
        var result = Must.Be.NotOverlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotOverlapping.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.NotOverlapping))]
    public void NotOverlapping_EdgeChecks(MustDateOnlyClausesTestData.NotOverlapping.EdgeCase testCase)
    {
        var result = Must.Be.NotOverlapping(testCase.Value.start1, testCase.Value.end1, testCase.Value.start2, testCase.Value.end2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.WithinDays.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.WithinDays))]
    public void WithinDays_Checks(MustDateOnlyClausesTestData.WithinDays.ValidCase testCase)
    {
        var result = Must.Be.WithinDays(testCase.Value.value, testCase.Value.target, testCase.Value.days);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.WithinDays.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.WithinDays))]
    public void WithinDays_EdgeChecks(MustDateOnlyClausesTestData.WithinDays.EdgeCase testCase)
    {
        var result = Must.Be.WithinDays(testCase.Value.value, testCase.Value.target, testCase.Value.days);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotWithinDays.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.NotWithinDays))]
    public void NotWithinDays_Checks(MustDateOnlyClausesTestData.NotWithinDays.ValidCase testCase)
    {
        var result = Must.Be.NotWithinDays(testCase.Value.value, testCase.Value.target, testCase.Value.days);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotWithinDays.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.NotWithinDays))]
    public void NotWithinDays_EdgeChecks(MustDateOnlyClausesTestData.NotWithinDays.EdgeCase testCase)
    {
        var result = Must.Be.NotWithinDays(testCase.Value.value, testCase.Value.target, testCase.Value.days);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.WithinCalendarMonths.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.WithinCalendarMonths))]
    public void WithinCalendarMonths_Checks(MustDateOnlyClausesTestData.WithinCalendarMonths.ValidCase testCase)
    {
        var result = Must.Be.WithinCalendarMonths(testCase.Value.value, testCase.Value.target, testCase.Value.months);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.WithinCalendarMonths.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.WithinCalendarMonths))]
    public void WithinCalendarMonths_EdgeChecks(MustDateOnlyClausesTestData.WithinCalendarMonths.EdgeCase testCase)
    {
        var result = Must.Be.WithinCalendarMonths(testCase.Value.value, testCase.Value.target, testCase.Value.months);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotWithinCalendarMonths.ValidCases), MemberType = typeof(MustDateOnlyClausesTestData.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_Checks(MustDateOnlyClausesTestData.NotWithinCalendarMonths.ValidCase testCase)
    {
        var result = Must.Be.NotWithinCalendarMonths(testCase.Value.value, testCase.Value.target, testCase.Value.months);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyClausesTestData.NotWithinCalendarMonths.EdgeCases), MemberType = typeof(MustDateOnlyClausesTestData.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_EdgeChecks(MustDateOnlyClausesTestData.NotWithinCalendarMonths.EdgeCase testCase)
    {
        var result = Must.Be.NotWithinCalendarMonths(testCase.Value.value, testCase.Value.target, testCase.Value.months);
        Assert.Equal(testCase.Expected, result.Success);
    }
}
