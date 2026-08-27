using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustDateOnlyRangeClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Chronological))]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Chronological.InvalidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Chronological))]
    public void Chronological_BehavesAsExpected(MustCase<(DateOnlyRange range, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Chronological(range, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Overlapping))]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Overlapping.InvalidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(MustCase<(DateOnlyRange range1, DateOnlyRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.Overlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.NotOverlapping))]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.NotOverlapping.InvalidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(MustCase<(DateOnlyRange range1, DateOnlyRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.NotOverlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Contains))]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.Contains.InvalidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.Contains))]
    public void Contains_BehavesAsExpected(MustCase<(DateOnlyRange range, DateOnly value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Contains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.NotContains))]
    [MemberData(nameof(MustDateOnlyRangeClausesTestData.NotContains.InvalidCases), MemberType = typeof(MustDateOnlyRangeClausesTestData.NotContains))]
    public void NotContains_BehavesAsExpected(MustCase<(DateOnlyRange range, DateOnly value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.NotContains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }
}
