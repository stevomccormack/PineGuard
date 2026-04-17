using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustDateTimeOffsetClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Past.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Past))]
    public void Past_Checks(MustDateTimeOffsetClausesTestData.Past.ValidCase testCase)
    {
        var input = testCase.Value;
        var result = Must.Be.Past(input);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.PastOrPresent.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.PastOrPresent))]
    public void PastOrPresent_Checks(MustDateTimeOffsetClausesTestData.PastOrPresent.ValidCase testCase)
    {
        var input = testCase.Value;
        var result = Must.Be.PastOrPresent(input);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Future.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Future))]
    public void Future_Checks(MustDateTimeOffsetClausesTestData.Future.ValidCase testCase)
    {
        var input = testCase.Value;
        var result = Must.Be.Future(input);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.FutureOrPresent.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.FutureOrPresent))]
    public void FutureOrPresent_Checks(MustDateTimeOffsetClausesTestData.FutureOrPresent.ValidCase testCase)
    {
        var input = testCase.Value;
        var result = Must.Be.FutureOrPresent(input);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Between.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Between))]
    public void Between_Checks(MustDateTimeOffsetClausesTestData.Between.ValidCase testCase)
    {
        var (input, min, max) = testCase.Value;
        var result = Must.Be.Between(input, min, max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.NotBetween.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.NotBetween))]
    public void NotBetween_Checks(MustDateTimeOffsetClausesTestData.NotBetween.ValidCase testCase)
    {
        var (input, min, max) = testCase.Value;
        var result = Must.Be.NotBetween(input, min, max);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Before.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Before))]
    public void Before_Checks(MustDateTimeOffsetClausesTestData.Before.ValidCase testCase)
    {
        var (input, target) = testCase.Value;
        var result = Must.Be.Before(input, target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.OnOrBefore.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.OnOrBefore))]
    public void OnOrBefore_Checks(MustDateTimeOffsetClausesTestData.OnOrBefore.ValidCase testCase)
    {
        var (input, target) = testCase.Value;
        var result = Must.Be.OnOrBefore(input, target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.After.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.After))]
    public void After_Checks(MustDateTimeOffsetClausesTestData.After.ValidCase testCase)
    {
        var (input, target) = testCase.Value;
        var result = Must.Be.After(input, target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.OnOrAfter.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.OnOrAfter))]
    public void OnOrAfter_Checks(MustDateTimeOffsetClausesTestData.OnOrAfter.ValidCase testCase)
    {
        var (input, target) = testCase.Value;
        var result = Must.Be.OnOrAfter(input, target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Same.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Same))]
    public void Same_Checks(MustDateTimeOffsetClausesTestData.Same.ValidCase testCase)
    {
        var (input, target) = testCase.Value;
        var result = Must.Be.Same(input, target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.NotSame.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.NotSame))]
    public void NotSame_Checks(MustDateTimeOffsetClausesTestData.NotSame.ValidCase testCase)
    {
        var (input, target) = testCase.Value;
        var result = Must.Be.NotSame(input, target);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Chronological))]
    public void Chronological_Checks(MustDateTimeOffsetClausesTestData.Chronological.ValidCase testCase)
    {
        var (start, end) = testCase.Value;
        var result = Must.Be.Chronological(start, end);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Overlapping))]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Overlapping.EdgeCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Overlapping))]
    public void Overlapping_Checks(MustDateTimeOffsetClausesTestData.Overlapping.ValidCase testCase)
    {
        var (s1, e1, s2, e2) = testCase.Value;
        var result = Must.Be.Overlapping(s1, e1, s2, e2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.NotOverlapping))]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.NotOverlapping.EdgeCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.NotOverlapping))]
    public void NotOverlapping_Checks(MustDateTimeOffsetClausesTestData.NotOverlapping.ValidCase testCase)
    {
        var (s1, e1, s2, e2) = testCase.Value;
        var result = Must.Be.NotOverlapping(s1, e1, s2, e2);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.Within.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.Within))]
    public void Within_Checks(MustDateTimeOffsetClausesTestData.Within.ValidCase testCase)
    {
        var (val, reference, window) = testCase.Value;
        var result = Must.Be.Within(val, reference, window);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.NotWithin.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.NotWithin))]
    public void NotWithin_Checks(MustDateTimeOffsetClausesTestData.NotWithin.ValidCase testCase)
    {
        var (val, reference, window) = testCase.Value;
        var result = Must.Be.NotWithin(val, reference, window);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.WithinCalendarMonths.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.WithinCalendarMonths))]
    public void WithinCalendarMonths_Checks(MustDateTimeOffsetClausesTestData.WithinCalendarMonths.ValidCase testCase)
    {
        var (val, reference, months) = testCase.Value;
        var result = Must.Be.WithinCalendarMonths(val, reference, months);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetClausesTestData.NotWithinCalendarMonths.ValidCases), MemberType = typeof(MustDateTimeOffsetClausesTestData.NotWithinCalendarMonths))]
    public void NotWithinCalendarMonths_Checks(MustDateTimeOffsetClausesTestData.NotWithinCalendarMonths.ValidCase testCase)
    {
        var (val, reference, months) = testCase.Value;
        var result = Must.Be.NotWithinCalendarMonths(val, reference, months);
        Assert.Equal(testCase.Expected, result.Success);
    }
}
