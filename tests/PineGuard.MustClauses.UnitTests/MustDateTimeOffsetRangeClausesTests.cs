using PineGuard.Testing.UnitTests;

namespace PineGuard.MustClauses.UnitTests;

public class MustDateTimeOffsetRangeClausesTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Chronological))]
    public void Chronological_Checks(MustDateTimeOffsetRangeClausesTestData.Chronological.ValidCase testCase)
    {
        var result = Must.Be.Chronological(testCase.Value.range, testCase.Value.inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Overlapping))]
    public void Overlapping_Checks(MustDateTimeOffsetRangeClausesTestData.Overlapping.ValidCase testCase)
    {
        var result = Must.Be.Overlapping(testCase.Value.range1, testCase.Value.range2, testCase.Value.inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_Checks(MustDateTimeOffsetRangeClausesTestData.NotOverlapping.ValidCase testCase)
    {
        var result = Must.Be.NotOverlapping(testCase.Value.range1, testCase.Value.range2, testCase.Value.inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Contains))]
    public void Contains_Checks(MustDateTimeOffsetRangeClausesTestData.Contains.ValidCase testCase)
    {
        var result = Must.Be.Contains(testCase.Value.range, testCase.Value.target, testCase.Value.inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.NotContains))]
    public void NotContains_Checks(MustDateTimeOffsetRangeClausesTestData.NotContains.ValidCase testCase)
    {
        var result = Must.Be.NotContains(testCase.Value.range, testCase.Value.target, testCase.Value.inclusion);
        Assert.Equal(testCase.Expected, result.Success);
    }
}
