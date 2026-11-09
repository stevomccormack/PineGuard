using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringTimeSpanClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.OutOfRange.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.OutOfRange))]
    public void OutOfRange_BehavesAsExpected(GuardCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.OutOfRangeTimeSpan(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.OutOfRangeTimeSpan(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.InRange.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.InRange))]
    public void InRange_BehavesAsExpected(GuardCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.InRangeTimeSpan(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.InRangeTimeSpan(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.ShorterThan.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(GuardCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.ShorterThanTimeSpan(value, tc.Value.other, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.ShorterThanTimeSpan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.LongerThan.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(GuardCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.LongerThanTimeSpan(value, tc.Value.other, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.LongerThanTimeSpan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }
}
