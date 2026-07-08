using System.ComponentModel.DataAnnotations;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringGeoLocationAttributesTests
{
    private static void Verify<TAttribute>(TAttribute attribute, StringGeoLocationAttributesTestData.ValidCase testCase)
        where TAttribute : ValidationAttribute
    {
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };
        var result = attribute.GetValidationResult(testCase.Value, ctx);
        Assert.Equal(testCase.Expected, result == ValidationResult.Success);
    }

    [Theory]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LatitudeString.ValidCases), MemberType = typeof(StringGeoLocationAttributesTestData.LatitudeString))]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LatitudeString.EdgeCases), MemberType = typeof(StringGeoLocationAttributesTestData.LatitudeString))]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LatitudeString.InvalidCases), MemberType = typeof(StringGeoLocationAttributesTestData.LatitudeString))]
    public void LatitudeString_ShouldReturnExpected(StringGeoLocationAttributesTestData.ValidCase testCase)
        => Verify(new LatitudeStringAttribute(), testCase);

    [Theory]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LongitudeString.ValidCases), MemberType = typeof(StringGeoLocationAttributesTestData.LongitudeString))]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LongitudeString.EdgeCases), MemberType = typeof(StringGeoLocationAttributesTestData.LongitudeString))]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LongitudeString.InvalidCases), MemberType = typeof(StringGeoLocationAttributesTestData.LongitudeString))]
    public void LongitudeString_ShouldReturnExpected(StringGeoLocationAttributesTestData.ValidCase testCase)
        => Verify(new LongitudeStringAttribute(), testCase);
}
