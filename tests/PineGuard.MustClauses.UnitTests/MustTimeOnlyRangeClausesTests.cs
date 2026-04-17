using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustTimeOnlyRangeClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Chronological))]
    public void Chronological_Checks(MustTimeOnlyRangeClausesTestData.Chronological.ValidCase testCase)
    {
        var range = new TimeOnlyRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.Chronological(range, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Overlapping))]
    public void Overlapping_Checks(MustTimeOnlyRangeClausesTestData.Overlapping.ValidCase testCase)
    {
        var range1 = new TimeOnlyRange(testCase.Value.S1, testCase.Value.E1);
        var range2 = new TimeOnlyRange(testCase.Value.S2, testCase.Value.E2);

        var result = Must.Be.Overlapping(range1, range2, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_Checks(MustTimeOnlyRangeClausesTestData.NotOverlapping.ValidCase testCase)
    {
        var range1 = new TimeOnlyRange(testCase.Value.S1, testCase.Value.E1);
        var range2 = new TimeOnlyRange(testCase.Value.S2, testCase.Value.E2);

        var result = Must.Be.NotOverlapping(range1, range2, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Contains))]
    public void Contains_Checks(MustTimeOnlyRangeClausesTestData.Contains.ValidCase testCase)
    {
        var range = new TimeOnlyRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.Contains(range, testCase.Value.Target, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.NotContains))]
    public void NotContains_Checks(MustTimeOnlyRangeClausesTestData.NotContains.ValidCase testCase)
    {
        var range = new TimeOnlyRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.NotContains(range, testCase.Value.Target, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }
}
