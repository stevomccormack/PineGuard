using PineGuard.Common;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustTimeOnlyRangeClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Chronological.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Chronological))]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Chronological.InvalidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Chronological))]
    public void Chronological_BehavesAsExpected(MustCase<(TimeOnlyRange range, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Chronological(range, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Overlapping.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Overlapping))]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Overlapping.InvalidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Overlapping))]
    public void Overlapping_BehavesAsExpected(MustCase<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.Overlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.NotOverlapping.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.NotOverlapping))]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.NotOverlapping.InvalidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(MustCase<(TimeOnlyRange range1, TimeOnlyRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = Must.Be.NotOverlapping(range1, tc.Value.range2, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Contains.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Contains))]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.Contains.InvalidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.Contains))]
    public void Contains_BehavesAsExpected(MustCase<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.Contains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.NotContains.ValidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.NotContains))]
    [MemberData(nameof(MustTimeOnlyRangeClausesTestData.NotContains.InvalidCases), MemberType = typeof(MustTimeOnlyRangeClausesTestData.NotContains))]
    public void NotContains_BehavesAsExpected(MustCase<(TimeOnlyRange range, TimeOnly value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = Must.Be.NotContains(range, tc.Value.value, tc.Value.inclusion);
        AssertResult(tc, result);
    }
}
