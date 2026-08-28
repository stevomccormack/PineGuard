using PineGuard.Testing.UnitTests.GuardClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardStringGeoLocationClausesTestData
{
    public static class NotLatitude
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLatitude.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLatitude.InvalidScenarios.ToGuardCases("value");
    }

    public static class NotLongitude
    {
        public static TheoryData<GuardCase<string?>> ValidCases => F.IsLongitude.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));
        public static TheoryData<GuardCase<string?>> InvalidCases => F.IsLongitude.InvalidScenarios.ToGuardCases("value");
    }

    // Guard.Against.NotGeoLocation calls Must.Be.GeoLocation, which checks "latitude" and "longitude" for
    // null as two separate preconditions (each attributing to its own parameter), then parses "latitude"
    // before "longitude" — so only an unparsable longitude (with a parsable latitude) attributes to
    // "longitude"; every other failure (including a numerically out-of-range latitude/longitude, and an
    // unparsable latitude) attributes to "latitude", the guard's own outer paramName (see MustStringGeoLocationClauses.GeoLocation).
    public static class NotGeoLocation
    {
        public static TheoryData<GuardCase<(string? latitude, string? longitude)>> ValidCases => F.IsGeoLocation.ValidScenarios.ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(string? latitude, string? longitude)>> InvalidCases => F.IsGeoLocation.InvalidScenarios.ToGuardCases(s => s.Name switch
        {
            nameof(F.IsGeoLocation.NullLatitude) => new GuardExpected(false, typeof(ArgumentNullException), "latitude"),
            nameof(F.IsGeoLocation.NullLongitude) => new GuardExpected(false, typeof(ArgumentNullException), "longitude"),
            nameof(F.IsGeoLocation.LonNotNumber) => new GuardExpected(false, typeof(ArgumentException), "longitude"),
            _ => new GuardExpected(false, typeof(ArgumentException), "latitude")
        });
    }
}
