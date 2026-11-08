using System.Globalization;
using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringGeoLocationClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.Styles.ValidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.Styles))]
    public void Styles_ReturnParsedCoordinates(GuardCase<(string? value, NumberStyles styles, double parsed)> tc)
    {
        // Arrange
        var value = tc.Value.value;

        // Act
        var latitude = Guard.Against.NotLatitude(value, tc.Value.styles);
        var longitude = Guard.Against.NotLongitude(value, tc.Value.styles);
        var pair = Guard.Against.NotGeoLocation(value, value, tc.Value.styles);

        // Assert
        Assert.Equal(tc.Value.parsed, latitude);
        Assert.Equal(tc.Value.parsed, longitude);
        Assert.Equal((tc.Value.parsed, tc.Value.parsed), pair);
    }

    [Theory]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.InvalidStyles.InvalidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.InvalidStyles))]
    public void Styles_AttributeConfigurationFailures(GuardCase<(string? value, NumberStyles styles)> tc)
    {
        // Arrange
        var value = tc.Value.value;

        // Act
        Func<double> latitude = () => Guard.Against.NotLatitude(value, tc.Value.styles);
        Func<double> longitude = () => Guard.Against.NotLongitude(value, tc.Value.styles);
        Func<(double, double)> pair = () => Guard.Against.NotGeoLocation(value, value, tc.Value.styles);

        // Assert
        AssertResult(tc, latitude);
        AssertResult(tc, longitude);
        AssertResult(tc, pair);
    }

    [Theory]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.NotLatitude.ValidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.NotLatitude))]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.NotLatitude.InvalidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.NotLatitude))]
    public void NotLatitude_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NotLatitude(value));
        AssertCustomMessage(tc, () => Guard.Against.NotLatitude(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.NotLongitude.ValidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.NotLongitude))]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.NotLongitude.InvalidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.NotLongitude))]
    public void NotLongitude_BehavesAsExpected(GuardCase<string?> tc)
    {
        var value = tc.Value;
        AssertResult(tc, () => Guard.Against.NotLongitude(value));
        AssertCustomMessage(tc, () => Guard.Against.NotLongitude(value, message: CustomMessage));
    }

    [Theory]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.NotGeoLocation.ValidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.NotGeoLocation))]
    [MemberData(nameof(GuardStringGeoLocationClausesTestData.NotGeoLocation.InvalidCases), MemberType = typeof(GuardStringGeoLocationClausesTestData.NotGeoLocation))]
    public void NotGeoLocation_BehavesAsExpected(GuardCase<(string? latitude, string? longitude)> tc)
    {
        var latitude = tc.Value.latitude;
        var longitude = tc.Value.longitude;
        AssertResult(tc, () => Guard.Against.NotGeoLocation(latitude, longitude));
        AssertCustomMessage(tc, () => Guard.Against.NotGeoLocation(latitude, longitude, message: CustomMessage));
    }
}
