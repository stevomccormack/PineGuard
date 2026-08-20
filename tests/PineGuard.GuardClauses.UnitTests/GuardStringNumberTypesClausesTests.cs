using PineGuard.Common;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardStringNumberTypesClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringNumberTypesClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.NotDecimal.ValidCases), MemberType = typeof(TD.NotDecimal))]
    [MemberData(nameof(TD.NotDecimal.InvalidCases), MemberType = typeof(TD.NotDecimal))]
    public void NotDecimal_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotDecimal(value));
        AssertCustomMessage(tc, () => Guard.Against.NotDecimal(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.NotExactDecimal.ValidCases), MemberType = typeof(TD.NotExactDecimal))]
    [MemberData(nameof(TD.NotExactDecimal.InvalidCases), MemberType = typeof(TD.NotExactDecimal))]
    public void NotExactDecimal_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotExactDecimal(value));
        AssertCustomMessage(tc, () => Guard.Against.NotExactDecimal(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.NotInt32.ValidCases), MemberType = typeof(TD.NotInt32))]
    [MemberData(nameof(TD.NotInt32.InvalidCases), MemberType = typeof(TD.NotInt32))]
    public void NotInt32_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotInt32(value));
        AssertCustomMessage(tc, () => Guard.Against.NotInt32(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.NotInt64.ValidCases), MemberType = typeof(TD.NotInt64))]
    [MemberData(nameof(TD.NotInt64.InvalidCases), MemberType = typeof(TD.NotInt64))]
    public void NotInt64_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotInt64(value));
        AssertCustomMessage(tc, () => Guard.Against.NotInt64(value, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.Int32OutOfRange.ValidCases), MemberType = typeof(TD.Int32OutOfRange))]
    [MemberData(nameof(TD.Int32OutOfRange.InvalidCases), MemberType = typeof(TD.Int32OutOfRange))]
    public void Int32OutOfRange_BehavesAsExpected(GuardCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.Int32OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.Int32OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.Int32InRange.ValidCases), MemberType = typeof(TD.Int32InRange))]
    [MemberData(nameof(TD.Int32InRange.InvalidCases), MemberType = typeof(TD.Int32InRange))]
    public void Int32InRange_BehavesAsExpected(GuardCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.Int32InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.Int32InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.Int64OutOfRange.ValidCases), MemberType = typeof(TD.Int64OutOfRange))]
    [MemberData(nameof(TD.Int64OutOfRange.InvalidCases), MemberType = typeof(TD.Int64OutOfRange))]
    public void Int64OutOfRange_BehavesAsExpected(GuardCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.Int64OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.Int64OutOfRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.Int64InRange.ValidCases), MemberType = typeof(TD.Int64InRange))]
    [MemberData(nameof(TD.Int64InRange.InvalidCases), MemberType = typeof(TD.Int64InRange))]
    public void Int64InRange_BehavesAsExpected(GuardCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.Int64InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.Int64InRange(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }
}
