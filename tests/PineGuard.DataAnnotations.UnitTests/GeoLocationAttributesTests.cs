using System.ComponentModel.DataAnnotations;
using PineGuard.Testing.UnitTests.DataAnnotations;
using Xunit.Abstractions;

namespace PineGuard.DataAnnotations.UnitTests;

public sealed class GeoLocationAttributesTests(ITestOutputHelper output)
    : BaseDataAnnotationUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GeoLocationAttributesTestData.Latitude.Cases), MemberType = typeof(GeoLocationAttributesTestData.Latitude))]
    public void Latitude_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LatitudeAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(GeoLocationAttributesTestData.Longitude.Cases), MemberType = typeof(GeoLocationAttributesTestData.Longitude))]
    public void Longitude_BehavesAsExpected(DataAnnotationCase tc)
    {
        // Arrange
        var attr = new LongitudeAttribute();
        var ctx = new ValidationContext(new object()) { MemberName = "Value" };

        // Act
        var result = attr.GetValidationResult(tc.Value, ctx);

        // Assert
        AssertResult(tc, result);
    }
}
