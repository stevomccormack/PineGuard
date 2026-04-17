using PineGuard.Testing.UnitTests.FluentValidation;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentStringGeoLocationExtensionsTestData
{
    public static class Latitude
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsLatitude.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLatitude.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid latitude.")
        });
    }

    public static class Longitude
    {
        public static TheoryData<FluentCase<string?>> Cases => F.IsLongitude.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsLongitude.NullValue) => new FluentExpected(true),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid longitude.")
        });
    }

    public static class GeoLocation
    {
        public static TheoryData<FluentCase<(string? latitude, string? longitude)>> Cases => F.IsGeoLocation.AllScenarios.ToFluentCases(s => s.Name switch
        {
            nameof(F.IsGeoLocation.NullLatitude) => new FluentExpected(true),
            nameof(F.IsGeoLocation.NullLongitude) => new FluentExpected(false, "longitude must not be null."),
            nameof(F.IsGeoLocation.LonNotNumber) => new FluentExpected(false, "longitude must be a valid geo location."),
            _ when s.IsValid => new FluentExpected(true),
            _ => new FluentExpected(false, "Value must be a valid geo location.")
        });
    }
}
