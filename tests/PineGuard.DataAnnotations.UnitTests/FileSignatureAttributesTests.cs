using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class FileSignatureAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(FileSignatureAttributesTestData.FileSignature.Cases), MemberType = typeof(FileSignatureAttributesTestData.FileSignature))]
    public void FileSignature_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FileSignatureAttribute(FileSignatureAttributesTestData.FileSignature.Extension);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(FileSignatureAttributesTestData.FileSignatureContainerExtension.Cases), MemberType = typeof(FileSignatureAttributesTestData.FileSignatureContainerExtension))]
    public void FileSignatureContainerExtension_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FileSignatureAttribute(FileSignatureAttributesTestData.FileSignatureContainerExtension.Extension);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(FileSignatureAttributesTestData.FileSignatureExtensionWithoutDot.Cases), MemberType = typeof(FileSignatureAttributesTestData.FileSignatureExtensionWithoutDot))]
    public void FileSignatureExtensionWithoutDot_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FileSignatureAttribute(FileSignatureAttributesTestData.FileSignatureExtensionWithoutDot.Extension);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(FileSignatureAttributesTestData.FileSignatureExtensionUppercase.Cases), MemberType = typeof(FileSignatureAttributesTestData.FileSignatureExtensionUppercase))]
    public void FileSignatureExtensionUppercase_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FileSignatureAttribute(FileSignatureAttributesTestData.FileSignatureExtensionUppercase.Extension);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(FileSignatureAttributesTestData.FileSignatureExtensionPadded.Cases), MemberType = typeof(FileSignatureAttributesTestData.FileSignatureExtensionPadded))]
    public void FileSignatureExtensionPadded_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FileSignatureAttribute(FileSignatureAttributesTestData.FileSignatureExtensionPadded.Extension);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(FileSignatureAttributesTestData.FileSignatureUnknownExtension.Cases), MemberType = typeof(FileSignatureAttributesTestData.FileSignatureUnknownExtension))]
    public void FileSignatureUnknownExtension_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FileSignatureAttribute(FileSignatureAttributesTestData.FileSignatureUnknownExtension.Extension);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(FileSignatureAttributesTestData.KnownFileSignature.Cases), MemberType = typeof(FileSignatureAttributesTestData.KnownFileSignature))]
    public void KnownFileSignature_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new KnownFileSignatureAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
