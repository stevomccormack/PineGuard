using PineGuard.Testing.UnitTests;
using PineGuard.Utils;

namespace PineGuard.Core.UnitTests.Utils;

public sealed class BufferUtilityTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsHex.ValidCases), MemberType = typeof(BufferUtilityTestData.IsHex))]
    [MemberData(nameof(BufferUtilityTestData.IsHex.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsHex))]
    public void IsHex_ReturnsExpected(BufferUtilityTestData.IsHex.ValidCase testCase)
    {
        // Act
        var result = BufferUtility.IsHex(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsBase64.ValidCases), MemberType = typeof(BufferUtilityTestData.IsBase64))]
    [MemberData(nameof(BufferUtilityTestData.IsBase64.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsBase64))]
    public void IsBase64_ReturnsExpected(BufferUtilityTestData.IsBase64.ValidCase testCase)
    {
        // Act
        var result = BufferUtility.IsBase64(testCase.Value);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(BufferUtilityTestData.IsBase64Url.ValidCases), MemberType = typeof(BufferUtilityTestData.IsBase64Url))]
    [MemberData(nameof(BufferUtilityTestData.IsBase64Url.EdgeCases), MemberType = typeof(BufferUtilityTestData.IsBase64Url))]
    public void IsBase64Url_ReturnsExpected(BufferUtilityTestData.IsBase64Url.ValidCase testCase)
    {
        // Act
        var result = BufferUtility.IsBase64Url(testCase.Value);

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
