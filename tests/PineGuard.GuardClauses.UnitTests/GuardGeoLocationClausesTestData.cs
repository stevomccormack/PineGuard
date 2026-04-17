using PineGuard.Testing.UnitTests.GuardClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GeoLocationRulesFixtures;

namespace PineGuard.GuardClauses.UnitTests;

public static class GuardGeoLocationClausesTestData
{
    // Guard.Against.NotLatitude — throws when value is NOT a valid latitude
    public static class NotLatitude
    {
        public static TheoryData<GuardCase<double>> ValidCases =>
            F.IsLatitude.AllValid.Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<double>> InvalidCases =>
            F.IsLatitude.AllInvalid.Where(s => s.Inputs.HasValue).Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotLongitude — throws when value is NOT a valid longitude
    public static class NotLongitude
    {
        public static TheoryData<GuardCase<double>> ValidCases =>
            F.IsLongitude.AllValid.Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<double>> InvalidCases =>
            F.IsLongitude.AllInvalid.Where(s => s.Inputs.HasValue).Select(s => new RuleScenario<double>(s.Name, s.Inputs!.Value, s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "value"));
    }

    // Guard.Against.NotGeoLocation — throws when values are NOT a valid geolocation
    public static class NotGeoLocation
    {
        public static TheoryData<GuardCase<(double latitude, double longitude)>> ValidCases =>
            F.IsGeoLocation.AllValid.Select(s => new RuleScenario<(double latitude, double longitude)>(s.Name, (s.Inputs.latitude!.Value, s.Inputs.longitude!.Value), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(true));

        public static TheoryData<GuardCase<(double latitude, double longitude)>> InvalidCases =>
            F.IsGeoLocation.AllInvalid.Where(s => s.Inputs is { latitude: not null, longitude: not null })
            .Select(s => new RuleScenario<(double latitude, double longitude)>(s.Name, (s.Inputs.latitude!.Value, s.Inputs.longitude!.Value), s.IsValid)).ToArray()
            .ToGuardCases(_ => new GuardExpected(false, typeof(ArgumentException), "latitude"));
    }
}
