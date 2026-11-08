using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class StringGeoLocationAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(StringGeoLocationAttributesTestData.Styles.Cases), MemberType = typeof(StringGeoLocationAttributesTestData.Styles))]
    public void Styles_ReachTheCanonicalClauses(DataAnnotationCase tc)
    {
        // Arrange
        var (value, styles, _) = ((string?, NumberStyles, double))tc.Value!;
        var context = new ValidationContext(new object()) { MemberName = "Value" };
        var latitude = new LatitudeStringAttribute { Styles = styles };
        var longitude = new LongitudeStringAttribute { Styles = styles };

        // Act
        var latitudeResult = latitude.GetValidationResult(value, context);
        var longitudeResult = longitude.GetValidationResult(value, context);

        // Assert
        AssertResult(tc, latitudeResult);
        AssertResult(tc, longitudeResult);
    }

    [Theory]
    [MemberData(nameof(StringGeoLocationAttributesTestData.InvalidStyles.Cases), MemberType = typeof(StringGeoLocationAttributesTestData.InvalidStyles))]
    public void Styles_RejectInvalidConfigurationExceptIntentionalNullSkipping(DataAnnotationCase tc)
    {
        // Arrange
        var (value, styles) = ((string?, NumberStyles))tc.Value!;
        var context = new ValidationContext(new object()) { MemberName = "Value" };
        var latitude = new LatitudeStringAttribute { Styles = styles };
        var longitude = new LongitudeStringAttribute { Styles = styles };

        // Act
        var latitudeResult = latitude.GetValidationResult(value, context);
        var longitudeResult = longitude.GetValidationResult(value, context);

        // Assert
        AssertResult(tc, latitudeResult);
        AssertResult(tc, longitudeResult);
    }

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
    {
        var attribute = new LatitudeStringAttribute();
        Assert.Equal(MustCodes.Geo.Latitude.Invalid, attribute.Code);
        Verify(attribute, testCase);
    }

    [Theory]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LongitudeString.ValidCases), MemberType = typeof(StringGeoLocationAttributesTestData.LongitudeString))]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LongitudeString.EdgeCases), MemberType = typeof(StringGeoLocationAttributesTestData.LongitudeString))]
    [MemberData(nameof(StringGeoLocationAttributesTestData.LongitudeString.InvalidCases), MemberType = typeof(StringGeoLocationAttributesTestData.LongitudeString))]
    public void LongitudeString_ShouldReturnExpected(StringGeoLocationAttributesTestData.ValidCase testCase)
    {
        // Act
        var attribute = new LongitudeStringAttribute();

        // Assert
        Verify(attribute, testCase);
    }
}
