using System.Globalization;
using PineGuard.Codes;
using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringGeoLocationClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.Styles.ValidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.Styles))]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.Styles.InvalidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.Styles))]
    public void Styles_PreserveInputAndReturnParsedCoordinates(MustCase<(string? value, NumberStyles styles, double parsed)> tc)
    {
        // Arrange
        using var culture = UseCulture("fr-FR");

        // Act
        var latitude = Must.Be.Latitude(tc.Value.value, tc.Value.styles);
        var longitude = Must.Be.Longitude(tc.Value.value, tc.Value.styles);
        var pair = Must.Be.GeoLocation(tc.Value.value, tc.Value.value, tc.Value.styles);

        // Assert
        AssertResult(tc, latitude);
        AssertResult(tc, longitude);
        AssertResult(tc, pair);
        Assert.Equal(tc.Value.value, latitude.Value);
        Assert.Equal(tc.Value.value, longitude.Value);
        Assert.Equal((tc.Value.value, tc.Value.value), pair.Value);
        if (tc.Expected.IsValid)
        {
            Assert.Equal(tc.Value.parsed, latitude.Result);
            Assert.Equal(tc.Value.parsed, longitude.Result);
            Assert.Equal((tc.Value.parsed, tc.Value.parsed), pair.Result);
        }
    }

    [Theory]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.InvalidStyles.InvalidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.InvalidStyles))]
    public void Styles_FailBeforeInspectingTheInput(MustCase<(string? value, NumberStyles styles)> tc)
    {
        // Act
        var latitude = Must.Be.Latitude(tc.Value.value, tc.Value.styles);
        var longitude = Must.Be.Longitude(tc.Value.value, tc.Value.styles);
        var pair = Must.Be.GeoLocation(tc.Value.value, tc.Value.value, tc.Value.styles);

        // Assert
        AssertResult(tc, latitude);
        AssertResult(tc, longitude);
        AssertResult(tc, pair);
        Assert.Equal(tc.Value.styles, latitude.Value);
        Assert.Equal(tc.Value.styles, longitude.Value);
        Assert.Equal(tc.Value.styles, pair.Value);
        Assert.Equal(MustCodes.Geo.Latitude.Invalid, latitude.Code);
        Assert.Equal(MustCodes.Geo.Longitude.Invalid, longitude.Code);
        Assert.Equal(MustCodes.Geo.Coordinate.Invalid, pair.Code);
    }

    [Theory]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.Latitude.ValidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.Latitude))]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.Latitude.InvalidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.Latitude))]
    public void Latitude_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Latitude(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.Longitude.ValidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.Longitude))]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.Longitude.InvalidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.Longitude))]
    public void Longitude_BehavesAsExpected(MustCase<string?> tc)
    {
        var result = Must.Be.Longitude(tc.Value, paramName: "value");
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.GeoLocation.ValidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.GeoLocation))]
    [MemberData(nameof(MustStringGeoLocationClausesTestData.GeoLocation.InvalidCases), MemberType = typeof(MustStringGeoLocationClausesTestData.GeoLocation))]
    public void GeoLocation_BehavesAsExpected(MustCase<(string? latitude, string? longitude)> tc)
    {
        var (latitude, longitude) = tc.Value;
        var result = Must.Be.GeoLocation(latitude, longitude);
        AssertResult(tc, result);
    }
}
