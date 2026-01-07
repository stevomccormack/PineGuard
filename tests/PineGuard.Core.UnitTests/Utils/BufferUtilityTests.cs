using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class BufferUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsHexString.ValidCases), MemberType = typeof(BufferUtilityTestData.IsHexString))]
    [MemberData(nameof(BufferUtilityTestData.IsHexString.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsHexString))]
    public void IsHexString_ReturnsExpected(BufferUtilityTestData.IsHexString.Case testCase)
    {
        // Act
        var result = BufferUtility.IsHexString(testCase.Input);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsBase64String.ValidCases), MemberType = typeof(BufferUtilityTestData.IsBase64String))]
    [MemberData(nameof(BufferUtilityTestData.IsBase64String.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsBase64String))]
    public void IsBase64String_ReturnsExpected(BufferUtilityTestData.IsBase64String.Case testCase)
    {
        // Act
        var result = BufferUtility.IsBase64String(testCase.Input);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }
}
