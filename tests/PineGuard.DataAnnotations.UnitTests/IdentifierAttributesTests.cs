using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class IdentifierAttributesTests(ITestOutputHelper output) : BaseDataAnnotationUnitTest(output)
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

    [Theory]
    [MemberData(nameof(IdentifierAttributesTestData.Ulid.Cases), MemberType = typeof(IdentifierAttributesTestData.Ulid))]
    public void Ulid_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new UlidAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result, attr.Code);
    }
}
