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
    [MemberData(nameof(TD.OutOfRangeInt32.ValidCases), MemberType = typeof(TD.OutOfRangeInt32))]
    [MemberData(nameof(TD.OutOfRangeInt32.InvalidCases), MemberType = typeof(TD.OutOfRangeInt32))]
    public void OutOfRangeInt32_BehavesAsExpected(GuardCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.OutOfRangeInt32(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.OutOfRangeInt32(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.InRangeInt32.ValidCases), MemberType = typeof(TD.InRangeInt32))]
    [MemberData(nameof(TD.InRangeInt32.InvalidCases), MemberType = typeof(TD.InRangeInt32))]
    public void InRangeInt32_BehavesAsExpected(GuardCase<(string text, int min, int max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.InRangeInt32(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.InRangeInt32(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.OutOfRangeInt64.ValidCases), MemberType = typeof(TD.OutOfRangeInt64))]
    [MemberData(nameof(TD.OutOfRangeInt64.InvalidCases), MemberType = typeof(TD.OutOfRangeInt64))]
    public void OutOfRangeInt64_BehavesAsExpected(GuardCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.OutOfRangeInt64(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.OutOfRangeInt64(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }

    [Theory]
    [MemberData(nameof(TD.InRangeInt64.ValidCases), MemberType = typeof(TD.InRangeInt64))]
    [MemberData(nameof(TD.InRangeInt64.InvalidCases), MemberType = typeof(TD.InRangeInt64))]
    public void InRangeInt64_BehavesAsExpected(GuardCase<(string text, long min, long max, Inclusion inclusion)> tc)
    {
        var value = tc.Value.text;
        var result = AssertResult(tc, () => Guard.Against.InRangeInt64(value, tc.Value.min, tc.Value.max, tc.Value.inclusion));
        AssertCustomMessage(tc, () => Guard.Against.InRangeInt64(value, tc.Value.min, tc.Value.max, tc.Value.inclusion, message: CustomMessage));
        if (tc.Expected.IsValid) Assert.Equal(value, result.ToString());
    }
}
