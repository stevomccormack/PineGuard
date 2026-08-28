using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class FilePathAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, FilePathAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(FilePathAttributesTestData.SafeFileName.ValidCases), MemberType = typeof(FilePathAttributesTestData.SafeFileName))]
    [MemberData(nameof(FilePathAttributesTestData.SafeFileName.EdgeCases), MemberType = typeof(FilePathAttributesTestData.SafeFileName))]
    [MemberData(nameof(FilePathAttributesTestData.SafeFileName.InvalidCases), MemberType = typeof(FilePathAttributesTestData.SafeFileName))]
    public void SafeFileName_ShouldReturnExpected(FilePathAttributesTestData.ValidCase testCase)
    {
        var attribute = new SafeFileNameAttribute();
        Assert.Equal(MustCodes.File.Name.Unsafe, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(FilePathAttributesTestData.HasFileExtension.ValidCases), MemberType = typeof(FilePathAttributesTestData.HasFileExtension))]
    [MemberData(nameof(FilePathAttributesTestData.HasFileExtension.EdgeCases), MemberType = typeof(FilePathAttributesTestData.HasFileExtension))]
    [MemberData(nameof(FilePathAttributesTestData.HasFileExtension.InvalidCases), MemberType = typeof(FilePathAttributesTestData.HasFileExtension))]
    public void HasFileExtension_ShouldReturnExpected(FilePathAttributesTestData.ValidCase testCase)
    {
        var attribute = new HasFileExtensionAttribute("txt", "png");
        Assert.Equal(MustCodes.File.Extension.Mismatch, attribute.Code);
        Verify(attribute, testCase);
    }
}
