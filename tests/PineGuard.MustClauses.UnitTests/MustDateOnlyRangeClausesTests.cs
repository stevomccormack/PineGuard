using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustDateOnlyRangeClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Chronological))]
    public void Chronological_Checks(MustDateOnlyRangeClausesTestData.Chronological.ValidCase testCase)
    {
        // Assuming we can create range directly via constructor or implicit creation for testing
        // DateOnlyRange likely structurally similar to TimeOnlyRange
        var range = new DateOnlyRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.Chronological(range, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Overlapping))]
    public void Overlapping_Checks(MustDateOnlyRangeClausesTestData.Overlapping.ValidCase testCase)
    {
        var range1 = new DateOnlyRange(testCase.Value.S1, testCase.Value.E1);
        var range2 = new DateOnlyRange(testCase.Value.S2, testCase.Value.E2);

        var result = Must.Be.Overlapping(range1, range2, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_Checks(MustDateOnlyRangeClausesTestData.NotOverlapping.ValidCase testCase)
    {
        var range1 = new DateOnlyRange(testCase.Value.S1, testCase.Value.E1);
        var range2 = new DateOnlyRange(testCase.Value.S2, testCase.Value.E2);

        var result = Must.Be.NotOverlapping(range1, range2, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Contains))]
    public void Contains_Checks(MustDateOnlyRangeClausesTestData.Contains.ValidCase testCase)
    {
        var range = new DateOnlyRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.Contains(range, testCase.Value.Target, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.NotContains))]
    public void NotContains_Checks(MustDateOnlyRangeClausesTestData.NotContains.ValidCase testCase)
    {
        var range = new DateOnlyRange(testCase.Value.Start, testCase.Value.End);
        var result = Must.Be.NotContains(range, testCase.Value.Target, testCase.Value.Inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }
}
