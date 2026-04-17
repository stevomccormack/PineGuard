using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardDateTimeRangeClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardDateTimeRangeClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.NotChronological.Cases), MemberType = typeof(TD.NotChronological))]
    public void NotChronological_BehavesAsExpected(GuardCase<(DateTimeRange range, Inclusion inclusion)> tc)
    { var range = tc.Value.range; var result = AssertResult(tc, () => Guard.Against.NotChronological(range, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(range, result); }

    [Theory]
    [MemberData(nameof(TD.Overlapping.Cases), MemberType = typeof(TD.Overlapping))]
    public void Overlapping_BehavesAsExpected(GuardCase<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)> tc)
    { var range1 = tc.Value.range1; var result = AssertResult(tc, () => Guard.Against.Overlapping(range1, tc.Value.range2, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(range1, result); }

    [Theory]
    [MemberData(nameof(TD.NotOverlapping.Cases), MemberType = typeof(TD.NotOverlapping))]
    public void NotOverlapping_BehavesAsExpected(GuardCase<(DateTimeRange range1, DateTimeRange range2, Inclusion inclusion)> tc)
    { var range1 = tc.Value.range1; var result = AssertResult(tc, () => Guard.Against.NotOverlapping(range1, tc.Value.range2, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(range1, result); }

    [Theory]
    [MemberData(nameof(TD.NotContains.Cases), MemberType = typeof(TD.NotContains))]
    public void NotContains_BehavesAsExpected(GuardCase<(DateTimeRange range, DateTime value, Inclusion inclusion)> tc)
    { var range = tc.Value.range; var result = AssertResult(tc, () => Guard.Against.NotContains(range, tc.Value.value, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(range, result); }

    [Theory]
    [MemberData(nameof(TD.Contains.Cases), MemberType = typeof(TD.Contains))]
    public void Contains_BehavesAsExpected(GuardCase<(DateTimeRange range, DateTime value, Inclusion inclusion)> tc)
    { var range = tc.Value.range; var result = AssertResult(tc, () => Guard.Against.Contains(range, tc.Value.value, tc.Value.inclusion)); if (tc.Expected.IsValid) Assert.Equal(range, result); }
}
