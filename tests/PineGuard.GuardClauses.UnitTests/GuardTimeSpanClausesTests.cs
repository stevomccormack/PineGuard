using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardTimeSpanClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.OutOfRange.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.OutOfRange))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.OutOfRange.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.InRange.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.InRange))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.InRange.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.InRange))]
    public void InRange_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.ShorterThan.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.ShorterThan))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.ShorterThan.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.ShorterThan(value, tc.Value.other, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.ShorterThan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.LongerThan.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.LongerThan))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.LongerThan.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.LongerThan(value, tc.Value.other, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.LongerThan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
