using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardDateTimeOffsetRangeClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardDateTimeOffsetRangeClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.NotChronological.ValidCases), MemberType = typeof(TD.NotChronological))]
    [MemberData(nameof(TD.NotChronological.InvalidCases), MemberType = typeof(TD.NotChronological))]
    public void NotChronological_BehavesAsExpected(GuardCase<(DateTimeOffsetRange range, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = AssertResult(tc, () => Guard.Against.NotChronological(range, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(range, result);
    }

    [Theory]
    [MemberData(nameof(TD.Overlapping.ValidCases), MemberType = typeof(TD.Overlapping))]
    [MemberData(nameof(TD.Overlapping.InvalidCases), MemberType = typeof(TD.Overlapping))]
    public void Overlapping_BehavesAsExpected(GuardCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = AssertResult(tc, () => Guard.Against.Overlapping(range1, tc.Value.range2, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(range1, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotOverlapping.ValidCases), MemberType = typeof(TD.NotOverlapping))]
    [MemberData(nameof(TD.NotOverlapping.InvalidCases), MemberType = typeof(TD.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(GuardCase<(DateTimeOffsetRange range1, DateTimeOffsetRange range2, Inclusion inclusion)> tc)
    {
        var range1 = tc.Value.range1;
        var result = AssertResult(tc, () => Guard.Against.NotOverlapping(range1, tc.Value.range2, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(range1, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotContains.ValidCases), MemberType = typeof(TD.NotContains))]
    [MemberData(nameof(TD.NotContains.InvalidCases), MemberType = typeof(TD.NotContains))]
    public void NotContains_BehavesAsExpected(GuardCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = AssertResult(tc, () => Guard.Against.NotContains(range, tc.Value.value, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(range, result);
    }

    [Theory]
    [MemberData(nameof(TD.Contains.ValidCases), MemberType = typeof(TD.Contains))]
    [MemberData(nameof(TD.Contains.InvalidCases), MemberType = typeof(TD.Contains))]
    public void Contains_BehavesAsExpected(GuardCase<(DateTimeOffsetRange range, DateTimeOffset value, Inclusion inclusion)> tc)
    {
        var range = tc.Value.range;
        var result = AssertResult(tc, () => Guard.Against.Contains(range, tc.Value.value, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(range, result);
    }
}
