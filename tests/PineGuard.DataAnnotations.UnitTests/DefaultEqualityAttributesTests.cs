using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class DefaultEqualityAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, DefaultEqualityAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };
        var result = attribute.GetValidationResult(testCase.Value, ctx);
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(DefaultEqualityAttributesTestData.NullOrDefault.ValidCases), MemberType = typeof(DefaultEqualityAttributesTestData.NullOrDefault))]
    [MemberData(nameof(DefaultEqualityAttributesTestData.NullOrDefault.EdgeCases), MemberType = typeof(DefaultEqualityAttributesTestData.NullOrDefault))]
    [MemberData(nameof(DefaultEqualityAttributesTestData.NullOrDefault.InvalidCases), MemberType = typeof(DefaultEqualityAttributesTestData.NullOrDefault))]
    public void NullOrDefault_ShouldReturnExpected(DefaultEqualityAttributesTestData.ValidCase testCase)
        => Verify(new NullOrDefaultAttribute(), testCase);

    [Theory]
    [MemberData(nameof(DefaultEqualityAttributesTestData.NotNullOrDefault.ValidCases), MemberType = typeof(DefaultEqualityAttributesTestData.NotNullOrDefault))]
    [MemberData(nameof(DefaultEqualityAttributesTestData.NotNullOrDefault.EdgeCases), MemberType = typeof(DefaultEqualityAttributesTestData.NotNullOrDefault))]
    [MemberData(nameof(DefaultEqualityAttributesTestData.NotNullOrDefault.InvalidCases), MemberType = typeof(DefaultEqualityAttributesTestData.NotNullOrDefault))]
    public void NotNullOrDefault_ShouldReturnExpected(DefaultEqualityAttributesTestData.ValidCase testCase)
        => Verify(new NotNullOrDefaultAttribute(), testCase);
}
