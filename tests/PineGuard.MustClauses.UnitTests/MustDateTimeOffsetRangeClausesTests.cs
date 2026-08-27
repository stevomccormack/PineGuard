using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustDateTimeOffsetRangeClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Chronological))]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Chronological.InvalidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Chronological))]
    public void Chronological_BehavesAsExpected(MustCase<(DateTimeOffsetRange range, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Chronological(range, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Overlapping))]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Overlapping.InvalidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(MustCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.Overlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.NotOverlapping))]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.NotOverlapping.InvalidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(MustCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.NotOverlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Contains))]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.Contains.InvalidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.Contains))]
    public void Contains_BehavesAsExpected(MustCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Contains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.NotContains))]
    [MemberData(nameof(MustDateTimeOffsetRangeClausesTestData.NotContains.InvalidCases), MemberType = typeof(MustDateTimeOffsetRangeClausesTestData.NotContains))]
    public void NotContains_BehavesAsExpected(MustCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.NotContains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }
}
