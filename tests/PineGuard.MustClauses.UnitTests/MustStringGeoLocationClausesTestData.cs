using PineGuard.Testing.UnitTests.MustClauses;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustStringGeoLocationClausesTestData
{
    public static class Latitude
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsLatitude.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsLatitude.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsLatitude.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid latitude.")
        });
    }

    public static class Longitude
    {
        public static TheoryData<MustCase<string?>> ValidCases => F.IsLongitude.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<string?>> InvalidCases => F.IsLongitude.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsLongitude.NullValue) => new MustExpected(false, "value must not be null.", "value"),
            _ => new MustExpected(false, "value must be a valid longitude.")
        });
    }

    public static class GeoLocation
    {
        public static TheoryData<MustCase<(string? latitude, string? longitude)>> ValidCases => F.IsGeoLocation.ValidScenarios.ToMustCases();

        public static TheoryData<MustCase<(string? latitude, string? longitude)>> InvalidCases => F.IsGeoLocation.InvalidScenarios.ToMustCases(s => s.Name switch
        {
            nameof(F.IsGeoLocation.NullLatitude) => new MustExpected(false, "latitude must not be null.", "latitude"),
            nameof(F.IsGeoLocation.NullLongitude) => new MustExpected(false, "longitude must not be null.", "longitude"),
            nameof(F.IsGeoLocation.LonNotNumber) => new MustExpected(false, "longitude must be a valid geo location."),
            _ => new MustExpected(false, "latitude must be a valid geo location.")
        });
    }
}
