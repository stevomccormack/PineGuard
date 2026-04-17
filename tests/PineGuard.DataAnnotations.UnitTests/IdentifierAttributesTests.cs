using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public class IdentifierAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, IdentifierAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var result = attribute.GetValidationResult(testCase.Value, new ValidationContext(new object()));
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(IdentifierAttributesTestData.TypeMismatchCases), MemberType = typeof(IdentifierAttributesTestData))]
    public void Slug_ShouldThrow_WhenTypeIsInvalid(ThrowsCase testCase)
    {
        var attrib = new SlugAttribute();
        Assert.Throws<InvalidOperationException>(() => attrib.GetValidationResult(testCase.Value, new ValidationContext(new object())));
    }

    [Theory]
    [MemberData(nameof(IdentifierAttributesTestData.Slug.ValidCases), MemberType = typeof(IdentifierAttributesTestData.Slug))]
    [MemberData(nameof(IdentifierAttributesTestData.Slug.EdgeCases), MemberType = typeof(IdentifierAttributesTestData.Slug))]
    [MemberData(nameof(IdentifierAttributesTestData.Slug.InvalidCases), MemberType = typeof(IdentifierAttributesTestData.Slug))]
    public void Slug_ShouldReturnExpected(IdentifierAttributesTestData.ValidCase testCase)
        => Verify(new SlugAttribute(), testCase);
}
