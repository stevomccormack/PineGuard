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
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.LessThan.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(GuardCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.LessThanTimeSpan(value, tc.Value.other, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.LessThanTimeSpan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }

    [Theory]
    [MemberData(nameof(GuardStringTimeSpanClausesTestData.GreaterThan.Cases), MemberType = typeof(GuardStringTimeSpanClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(GuardCase<(string? value, TimeSpan other, Inclusion inclusion)> tc)
    { var value = tc.Value.value; var result = AssertResult(tc, () => Guard.Against.GreaterThanTimeSpan(value, tc.Value.other, tc.Value.inclusion)); AssertCustomMessage(tc, () => Guard.Against.GreaterThanTimeSpan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage)); if (tc.Expected.IsValid && value is not null) Assert.Equal(TimeSpan.Parse(value), result); }
}
