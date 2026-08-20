using PineGuard.Testing.UnitTests.GuardClauses;
using Xunit.Abstractions;

namespace PineGuard.GuardClauses.UnitTests;

public sealed class GuardGeoLocationClausesTests(ITestOutputHelper output)
    : BaseGuardUnitTest(output)
{
    [Theory]
    [MemberData(nameof(GuardGeoLocationClausesTestData.NotLatitude.ValidCases), MemberType = typeof(GuardGeoLocationClausesTestData.NotLatitude))]
    [MemberData(nameof(GuardGeoLocationClausesTestData.NotLatitude.InvalidCases), MemberType = typeof(GuardGeoLocationClausesTestData.NotLatitude))]
    public void NotLatitude_BehavesAsExpected(GuardCase<double> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.NotLatitude(tc.Value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotLatitude(tc.Value, paramName: "value", message: CustomMessage));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardGeoLocationClausesTestData.NotLongitude.ValidCases), MemberType = typeof(GuardGeoLocationClausesTestData.NotLongitude))]
    [MemberData(nameof(GuardGeoLocationClausesTestData.NotLongitude.InvalidCases), MemberType = typeof(GuardGeoLocationClausesTestData.NotLongitude))]
    public void NotLongitude_BehavesAsExpected(GuardCase<double> tc)
    {
        // Act
        var result = AssertResult(tc, () => Guard.Against.NotLongitude(tc.Value, paramName: "value"));
        AssertCustomMessage(tc, () => Guard.Against.NotLongitude(tc.Value, paramName: "value", message: CustomMessage));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, result);
    }

    [Theory]
    [MemberData(nameof(GuardGeoLocationClausesTestData.NotGeoLocation.ValidCases), MemberType = typeof(GuardGeoLocationClausesTestData.NotGeoLocation))]
    [MemberData(nameof(GuardGeoLocationClausesTestData.NotGeoLocation.InvalidCases), MemberType = typeof(GuardGeoLocationClausesTestData.NotGeoLocation))]
    public void NotGeoLocation_BehavesAsExpected(GuardCase<(double latitude, double longitude)> tc)
    {
        // Arrange
        var latitude = tc.Value.latitude;
        var longitude = tc.Value.longitude;

        // Act
        var result = AssertResult(tc, () => Guard.Against.NotGeoLocation(latitude, longitude));
        AssertCustomMessage(tc, () => Guard.Against.NotGeoLocation(latitude, longitude, message: CustomMessage));

        // Assert
        if (tc.Expected.IsValid)
            Assert.Equal(tc.Value, (result.Latitude, result.Longitude));
    }
}
