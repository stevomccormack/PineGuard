using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustDateTimeRangeClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Chronological))]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Chronological.InvalidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Chronological))]
    public void Chronological_BehavesAsExpected(MustCase<(DateTimeRange range, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Chronological(range, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Overlapping))]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Overlapping.InvalidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(MustCase<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.Overlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.NotOverlapping))]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.NotOverlapping.InvalidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(MustCase<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.NotOverlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Contains))]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.Contains.InvalidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.Contains))]
    public void Contains_BehavesAsExpected(MustCase<(DateTimeRange range, DateTime value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Contains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.NotContains))]
    [MemberData(nameof(MustDateTimeRangeClausesTestData.NotContains.InvalidCases), MemberType = typeof(MustDateTimeRangeClausesTestData.NotContains))]
    public void NotContains_BehavesAsExpected(MustCase<(DateTimeRange range, DateTime value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.NotContains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }
}
