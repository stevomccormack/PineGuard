using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustGeoLocationClausesTests(ITestOutputHelper output)
    : BaseMustUnitTest(output)
{
    [Theory]
    [MemberData(nameof(MustGeoLocationClausesTestData.Latitude.ValidCases), MemberType = typeof(MustGeoLocationClausesTestData.Latitude))]
    [MemberData(nameof(MustGeoLocationClausesTestData.Latitude.InvalidCases), MemberType = typeof(MustGeoLocationClausesTestData.Latitude))]
    public void Latitude_BehavesAsExpected(MustCase<double> tc)
    {
        // Act
        var result = Must.Be.Latitude(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustGeoLocationClausesTestData.Longitude.ValidCases), MemberType = typeof(MustGeoLocationClausesTestData.Longitude))]
    [MemberData(nameof(MustGeoLocationClausesTestData.Longitude.InvalidCases), MemberType = typeof(MustGeoLocationClausesTestData.Longitude))]
    public void Longitude_BehavesAsExpected(MustCase<double> tc)
    {
        // Act
        var result = Must.Be.Longitude(tc.Value, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }

    [Theory]
    [MemberData(nameof(MustGeoLocationClausesTestData.GeoLocation.ValidCases), MemberType = typeof(MustGeoLocationClausesTestData.GeoLocation))]
    [MemberData(nameof(MustGeoLocationClausesTestData.GeoLocation.InvalidCases), MemberType = typeof(MustGeoLocationClausesTestData.GeoLocation))]
    public void GeoLocation_BehavesAsExpected(MustCase<(double latitude, double longitude)> tc)
    {
        // Act
        var result = Must.Be.GeoLocation(tc.Value.latitude, tc.Value.longitude, paramName: "value");

        // Assert
        AssertResult(tc, result);
    }
}
