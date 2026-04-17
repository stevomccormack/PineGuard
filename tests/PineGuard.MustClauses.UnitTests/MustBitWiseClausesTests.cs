using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustBitWiseClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.BitwiseEqualTo.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.BitwiseEqualTo))]
    [MemberData(nameof(MustBitWiseClausesTestData.BitwiseEqualTo.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.BitwiseEqualTo))]
    public void BitwiseEqualTo_BehavesAsExpected(MustCase<(int input, int other, string? mask)> tc)
    {
        // Act
        var result = Must.Be.BitwiseEqualTo(tc.Value.input, tc.Value.other, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.NotBitwiseEqualTo.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.NotBitwiseEqualTo))]
    [MemberData(nameof(MustBitWiseClausesTestData.NotBitwiseEqualTo.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.NotBitwiseEqualTo))]
    public void NotBitwiseEqualTo_BehavesAsExpected(MustCase<(int input, int other, string? mask)> tc)
    {
        // Act
        var result = Must.Be.NotBitwiseEqualTo(tc.Value.input, tc.Value.other, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.HasAllBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.HasAllBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.HasAllBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.HasAllBits))]
    public void HasAllBits_BehavesAsExpected(MustCase<(int input, string? mask)> tc)
    {
        // Act
        var result = Must.Be.HasAllBits(tc.Value.input, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.HasAnyBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.HasAnyBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.HasAnyBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.HasAnyBits))]
    public void HasAnyBits_BehavesAsExpected(MustCase<(int input, string? mask)> tc)
    {
        // Act
        var result = Must.Be.HasAnyBits(tc.Value.input, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.HasNoBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.HasNoBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.HasNoBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.HasNoBits))]
    public void HasNoBits_BehavesAsExpected(MustCase<(int input, string? mask)> tc)
    {
        // Act
        var result = Must.Be.HasNoBits(tc.Value.input, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.HasOnlyBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.HasOnlyBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.HasOnlyBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.HasOnlyBits))]
    public void HasOnlyBits_BehavesAsExpected(MustCase<(int input, string? allowedMask)> tc)
    {
        // Act
        var result = Must.Be.HasOnlyBits(tc.Value.input, tc.Value.allowedMask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasAllBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasAllBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasAllBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasAllBits))]
    public void NotHasAllBits_BehavesAsExpected(MustCase<(int input, string? mask)> tc)
    {
        // Act
        var result = Must.Be.NotHasAllBits(tc.Value.input, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasAnyBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasAnyBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasAnyBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasAnyBits))]
    public void NotHasAnyBits_BehavesAsExpected(MustCase<(int input, string? mask)> tc)
    {
        // Act
        var result = Must.Be.NotHasAnyBits(tc.Value.input, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasNoBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasNoBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasNoBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasNoBits))]
    public void NotHasNoBits_BehavesAsExpected(MustCase<(int input, string? mask)> tc)
    {
        // Act
        var result = Must.Be.NotHasNoBits(tc.Value.input, tc.Value.mask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasOnlyBits.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasOnlyBits))]
    [MemberData(nameof(MustBitWiseClausesTestData.NotHasOnlyBits.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.NotHasOnlyBits))]
    public void NotHasOnlyBits_BehavesAsExpected(MustCase<(int input, string? allowedMask)> tc)
    {
        // Act
        var result = Must.Be.NotHasOnlyBits(tc.Value.input, tc.Value.allowedMask, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.PowerOfTwo.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.PowerOfTwo))]
    [MemberData(nameof(MustBitWiseClausesTestData.PowerOfTwo.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.PowerOfTwo))]
    public void PowerOfTwo_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.PowerOfTwo(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustBitWiseClausesTestData.NotPowerOfTwo.ValidCases), MemberType = typeof(MustBitWiseClausesTestData.NotPowerOfTwo))]
    [MemberData(nameof(MustBitWiseClausesTestData.NotPowerOfTwo.InvalidCases), MemberType = typeof(MustBitWiseClausesTestData.NotPowerOfTwo))]
    public void NotPowerOfTwo_BehavesAsExpected(MustCase<int> tc)
    {
        // Act
        var result = Must.Be.NotPowerOfTwo(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
