using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardTimeSpanClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.NotDurationBetween.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.NotDurationBetween))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.NotDurationBetween.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.NotDurationBetween))]
    public void NotDurationBetween_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotDurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.DurationBetween.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.DurationBetween))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.DurationBetween.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.DurationBetween))]
    public void DurationBetween_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.DurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.LessThan.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.LessThan))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.LessThan.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.LessThan(value, tc.Value.threshold, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.GreaterThan.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.GreaterThan))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan threshold, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.GreaterThan(value, tc.Value.threshold, tc.Value.inclusion));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
