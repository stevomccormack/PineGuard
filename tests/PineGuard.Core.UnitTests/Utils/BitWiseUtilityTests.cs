using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class BitWiseUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(BitWiseUtilityTestData.TryParseNonNegativeMaskByte.ValidCases), MemberType = typeof(BitWiseUtilityTestData.TryParseNonNegativeMaskByte))]
    [MemberData(nameof(BitWiseUtilityTestData.TryParseNonNegativeMaskByte.EdgeCases), MemberType = typeof(BitWiseUtilityTestData.TryParseNonNegativeMaskByte))]
    public void TryParseNonNegativeMask_Byte_ReturnsExpected(BitWiseUtilityTestData.TryParseNonNegativeMaskByte.ValidCase testCase)
    {
        // Act
        var ok = BitWiseUtility.TryParseNonNegativeMask<byte>(testCase.Value, out var parsed);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, parsed);
    }

    [Theory]
    [MemberData(nameof(BitWiseUtilityTestData.TryParseNonNegativeMaskUInt16.ValidCases), MemberType = typeof(BitWiseUtilityTestData.TryParseNonNegativeMaskUInt16))]
    [MemberData(nameof(BitWiseUtilityTestData.TryParseNonNegativeMaskUInt16.EdgeCases), MemberType = typeof(BitWiseUtilityTestData.TryParseNonNegativeMaskUInt16))]
    public void TryParseNonNegativeMask_UInt16_ReturnsExpected(BitWiseUtilityTestData.TryParseNonNegativeMaskUInt16.ValidCase testCase)
    {
        // Act
        var ok = BitWiseUtility.TryParseNonNegativeMask<ushort>(testCase.Value, out var parsed);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, parsed);
    }
}
