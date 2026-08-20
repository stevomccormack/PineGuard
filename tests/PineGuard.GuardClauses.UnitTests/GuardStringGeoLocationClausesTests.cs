using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardStringGeoLocationClausesTests(ITestOutputHelper output) : BaseGuardUnitTest(output)
{
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
