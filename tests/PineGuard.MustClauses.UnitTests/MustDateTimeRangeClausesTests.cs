using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustDateTimeRangeClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Chronological))]
    public void Chronological_Checks(MustDateTimeRangeClausesTestData.Chronological.ValidCase testCase)
    {
        var range = new DateTimeRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.Chronological(range, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Overlapping))]
    public void Overlapping_Checks(MustDateTimeRangeClausesTestData.Overlapping.ValidCase testCase)
    {
        var range1 = new DateTimeRange(testCase.Value.S1, testCase.Value.E1);
        var range2 = new DateTimeRange(testCase.Value.S2, testCase.Value.E2);

        var result = Must.Be.Overlapping(range1, range2, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_Checks(MustDateTimeRangeClausesTestData.NotOverlapping.ValidCase testCase)
    {
        var range1 = new DateTimeRange(testCase.Value.S1, testCase.Value.E1);
        var range2 = new DateTimeRange(testCase.Value.S2, testCase.Value.E2);

        var result = Must.Be.NotOverlapping(range1, range2, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Contains))]
    public void Contains_Checks(MustDateTimeRangeClausesTestData.Contains.ValidCase testCase)
    {
        var range = new DateTimeRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.Contains(range, testCase.Value.Target, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.NotContains))]
    public void NotContains_Checks(MustDateTimeRangeClausesTestData.NotContains.ValidCase testCase)
    {
        var range = new DateTimeRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.NotContains(range, testCase.Value.Target, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }
}
