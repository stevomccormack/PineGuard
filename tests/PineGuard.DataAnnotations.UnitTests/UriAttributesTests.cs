using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;
using WebUrlCases = PineGuard.DataAnnotations.UnitTests.UriAttributesTestData.WebUrl;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class UriAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(UriAttributesTestData.AbsoluteUri.Cases), MemberType = typeof(UriAttributesTestData.AbsoluteUri))]
    public void AbsoluteUri_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new AbsoluteUriAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.RelativeUri.Cases), MemberType = typeof(UriAttributesTestData.RelativeUri))]
    public void RelativeUri_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new RelativeUriAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(WebUrlCases.Cases), MemberType = typeof(WebUrlCases))]
    public void WebUrl_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new WebUrlAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.HttpsUrl.Cases), MemberType = typeof(UriAttributesTestData.HttpsUrl))]
    public void HttpsUrl_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new HttpsUrlAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.HttpUrl.Cases), MemberType = typeof(UriAttributesTestData.HttpUrl))]
    public void HttpUrl_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new HttpUrlAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.FileUri.Cases), MemberType = typeof(UriAttributesTestData.FileUri))]
    public void FileUri_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FileUriAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.FilePath.Cases), MemberType = typeof(UriAttributesTestData.FilePath))]
    public void FilePath_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new FilePathAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.NotFilePath.Cases), MemberType = typeof(UriAttributesTestData.NotFilePath))]
    public void NotFilePath_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotFilePathAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.HasScheme.Cases), MemberType = typeof(UriAttributesTestData.HasScheme))]
    public void HasScheme_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new HasSchemeAttribute(UriAttributesTestData.HasScheme.Scheme);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }

    [Theory]
    [MemberData(nameof(UriAttributesTestData.NotHasScheme.Cases), MemberType = typeof(UriAttributesTestData.NotHasScheme))]
    public void NotHasScheme_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new NotHasSchemeAttribute(UriAttributesTestData.NotHasScheme.Scheme);
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
