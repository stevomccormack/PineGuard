using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class BufferUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsHexString.ValidCases), MemberType = typeof(BufferUtilityTestData.IsHexString))]
    [MemberData(nameof(BufferUtilityTestData.IsHexString.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsHexString))]
    public void IsHexString_ReturnsExpected(BufferUtilityTestData.IsHexString.ValidCase testCase)
    {
        // Act
        var result = BufferUtility.IsHexString(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsBase64String.ValidCases), MemberType = typeof(BufferUtilityTestData.IsBase64String))]
    [MemberData(nameof(BufferUtilityTestData.IsBase64String.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsBase64String))]
    public void IsBase64String_ReturnsExpected(BufferUtilityTestData.IsBase64String.ValidCase testCase)
    {
        // Act
        var result = BufferUtility.IsBase64String(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsBase64UrlString.ValidCases), MemberType = typeof(BufferUtilityTestData.IsBase64UrlString))]
    [MemberData(nameof(BufferUtilityTestData.IsBase64UrlString.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsBase64UrlString))]
    public void IsBase64UrlString_ReturnsExpected(BufferUtilityTestData.IsBase64UrlString.ValidCase testCase)
    {
        // Act
        var result = BufferUtility.IsBase64UrlString(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(BufferUtilityTestData.TryDecodeUtf8.ValidCases), MemberType = typeof(BufferUtilityTestData.TryDecodeUtf8))]
    [MemberData(nameof(BufferUtilityTestData.TryDecodeUtf8.EdgeCases), MemberType = typeof(BufferUtilityTestData.TryDecodeUtf8))]
    public void TryDecodeUtf8_ReturnsExpected(BufferUtilityTestData.TryDecodeUtf8.ValidCase testCase)
    {
        // Act
        var result = BufferUtility.TryDecodeUtf8(testCase.Value, out var text);

        // Assert
        Assert.Equal(testCase.Expected, result);
        Assert.Equal(testCase.ExpectedOutValue, text);
    }
}
