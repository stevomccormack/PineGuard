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
    [MemberData(nameof(GuardTimeSpanClausesTestData.LessThan.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.LessThan))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.LessThan.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.LessThan))]
    public void LessThan_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.LessThan(value, tc.Value.other, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.LessThan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(GuardTimeSpanClausesTestData.GreaterThan.ValidCases), MemberType = typeof(GuardTimeSpanClausesTestData.GreaterThan))]
    [MemberData(nameof(GuardTimeSpanClausesTestData.GreaterThan.InvalidCases), MemberType = typeof(GuardTimeSpanClausesTestData.GreaterThan))]
    public void GreaterThan_BehavesAsExpected(GuardCase<(TimeSpan value, TimeSpan other, Inclusion inclusion)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.GreaterThan(value, tc.Value.other, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.GreaterThan(value, tc.Value.other, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
