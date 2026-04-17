using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;
using TD = PineGuard.GuardClauses.UnitTests.GuardBitWiseClausesTestData;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardBitWiseClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TD.BitwiseEqualTo.ValidCases), MemberType = typeof(TD.BitwiseEqualTo))]
    [MemberData(nameof(TD.BitwiseEqualTo.InvalidCases), MemberType = typeof(TD.BitwiseEqualTo))]
    public void BitwiseEqualTo_BehavesAsExpected(GuardCase<(int value, int other, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.BitwiseEqualTo(value, tc.Value.other, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotBitwiseEqualTo.ValidCases), MemberType = typeof(TD.NotBitwiseEqualTo))]
    [MemberData(nameof(TD.NotBitwiseEqualTo.InvalidCases), MemberType = typeof(TD.NotBitwiseEqualTo))]
    public void NotBitwiseEqualTo_BehavesAsExpected(GuardCase<(int value, int other, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotBitwiseEqualTo(value, tc.Value.other, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasAllBits.ValidCases), MemberType = typeof(TD.NotHasAllBits))]
    [MemberData(nameof(TD.NotHasAllBits.InvalidCases), MemberType = typeof(TD.NotHasAllBits))]
    public void NotHasAllBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotHasAllBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasAnyBits.ValidCases), MemberType = typeof(TD.NotHasAnyBits))]
    [MemberData(nameof(TD.NotHasAnyBits.InvalidCases), MemberType = typeof(TD.NotHasAnyBits))]
    public void NotHasAnyBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotHasAnyBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasNoBits.ValidCases), MemberType = typeof(TD.NotHasNoBits))]
    [MemberData(nameof(TD.NotHasNoBits.InvalidCases), MemberType = typeof(TD.NotHasNoBits))]
    public void NotHasNoBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotHasNoBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotHasOnlyBits.ValidCases), MemberType = typeof(TD.NotHasOnlyBits))]
    [MemberData(nameof(TD.NotHasOnlyBits.InvalidCases), MemberType = typeof(TD.NotHasOnlyBits))]
    public void NotHasOnlyBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.NotHasOnlyBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.NotPowerOfTwo.ValidCases), MemberType = typeof(TD.NotPowerOfTwo))]
    [MemberData(nameof(TD.NotPowerOfTwo.InvalidCases), MemberType = typeof(TD.NotPowerOfTwo))]
    public void NotPowerOfTwo_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.NotPowerOfTwo(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.PowerOfTwo.ValidCases), MemberType = typeof(TD.PowerOfTwo))]
    [MemberData(nameof(TD.PowerOfTwo.InvalidCases), MemberType = typeof(TD.PowerOfTwo))]
    public void PowerOfTwo_BehavesAsExpected(GuardCase<int> tc)
    {
        var value = tc.Value;
        var result = AssertResult(tc, () => Guard.Against.PowerOfTwo(value));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasAllBits.ValidCases), MemberType = typeof(TD.HasAllBits))]
    [MemberData(nameof(TD.HasAllBits.InvalidCases), MemberType = typeof(TD.HasAllBits))]
    public void HasAllBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.HasAllBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasAnyBits.ValidCases), MemberType = typeof(TD.HasAnyBits))]
    [MemberData(nameof(TD.HasAnyBits.InvalidCases), MemberType = typeof(TD.HasAnyBits))]
    public void HasAnyBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.HasAnyBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasNoBits.ValidCases), MemberType = typeof(TD.HasNoBits))]
    [MemberData(nameof(TD.HasNoBits.InvalidCases), MemberType = typeof(TD.HasNoBits))]
    public void HasNoBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.HasNoBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }

    [Theory]
    [MemberData(nameof(TD.HasOnlyBits.ValidCases), MemberType = typeof(TD.HasOnlyBits))]
    [MemberData(nameof(TD.HasOnlyBits.InvalidCases), MemberType = typeof(TD.HasOnlyBits))]
    public void HasOnlyBits_BehavesAsExpected(GuardCase<(int value, string mask)> tc)
    {
        var value = tc.Value.value;
        var result = AssertResult(tc, () => Guard.Against.HasOnlyBits(value, tc.Value.mask));
        if (tc.Expected.IsValid) Assert.Equal(value, result);
    }
}
