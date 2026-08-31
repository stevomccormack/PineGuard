using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class BufferAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(BufferAttributesTestData.Hex.Cases), MemberType = typeof(BufferAttributesTestData.Hex))]
    public void Hex_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new HexAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(BufferAttributesTestData.NotHex.Cases), MemberType = typeof(BufferAttributesTestData.NotHex))]
    public void NotHex_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotHexAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(BufferAttributesTestData.Base64.Cases), MemberType = typeof(BufferAttributesTestData.Base64))]
    public void Base64_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Base64Attribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(BufferAttributesTestData.NotBase64.Cases), MemberType = typeof(BufferAttributesTestData.NotBase64))]
    public void NotBase64_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotBase64Attribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(BufferAttributesTestData.Base64Url.Cases), MemberType = typeof(BufferAttributesTestData.Base64Url))]
    public void Base64Url_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Base64UrlAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(BufferAttributesTestData.Utf8.Cases), MemberType = typeof(BufferAttributesTestData.Utf8))]
    public void Utf8_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new Utf8Attribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
