using PineGuard.Testing.UnitTests.MustClauses;
using Xunit.Abstractions;

namespace PineGuard.MustClauses.UnitTests;

public sealed class MustStringGeoLocationClausesTests(ITestOutputHelper output) : BaseMustUnitTest(output)
{
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
