using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringTimeSpanClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.NotDurationBetween.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.NotDurationBetween))]
    public void NotDurationBetween_BehavesAsExpected(GuardCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.NotDurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.NotDurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.DurationBetween.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.DurationBetween))]
    public void DurationBetween_BehavesAsExpected(GuardCase<(string? value, TimeSpan min, TimeSpan max, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.DurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.DurationBetween(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.ShorterThan.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.ShorterThan))]
    public void ShorterThan_BehavesAsExpected(GuardCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.ShorterThan(value, tc.Value.threshold, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.ShorterThan(value, tc.Value.threshold, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.LongerThan.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.LongerThan))]
    public void LongerThan_BehavesAsExpected(GuardCase<(string? value, TimeSpan threshold, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.LongerThan(value, tc.Value.threshold, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.LongerThan(value, tc.Value.threshold, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }
}
