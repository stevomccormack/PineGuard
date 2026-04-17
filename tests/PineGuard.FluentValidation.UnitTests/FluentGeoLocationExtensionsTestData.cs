using PineGuard.Testing.UnitTests.FluentValidation;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GeoLocationRulesFixtures;

namespace PineGuard.FluentValidation.UnitTests;

public static class FluentGeoLocationExtensionsTestData
{
    public static class Latitude
    {
        public static TheoryData<FluentCase<double>> Cases => F.IsLatitude.AllScenarios
            .Except(nameof(F.IsLatitude.Null), nameof(F.IsLatitude.NaN), nameof(F.IsLatitude.PosInfinity), nameof(F.IsLatitude.NegInfinity))
            .Project(v => v!.Value)
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be a valid latitude."));

        public static TheoryData<FluentCase<double?>> NullCases =>
        [
            new(nameof(F.IsLatitude.Null), null, new FluentExpected(true))
        ];
    }

    public static class Longitude
    {
        public static TheoryData<FluentCase<double>> Cases => F.IsLongitude.AllScenarios
            .Except(nameof(F.IsLongitude.Null), nameof(F.IsLongitude.NaN), nameof(F.IsLongitude.PosInfinity), nameof(F.IsLongitude.NegInfinity))
            .Project(v => v!.Value)
            .ToFluentCases(s => s.IsValid ? new FluentExpected(true) : new FluentExpected(false, "Value must be a valid longitude."));

        public static TheoryData<FluentCase<double?>> NullCases =>
        [
            new(nameof(F.IsLongitude.Null), null, new FluentExpected(true))
        ];
    }

    public static class GeoLocation
    {
        public static TheoryData<FluentCase<(double latitude, double longitude)>> Cases =>
        [
            new(nameof(F.IsGeoLocation.Valid),       (F.IsGeoLocation.Valid.latitude!.Value,       F.IsGeoLocation.Valid.longitude!.Value),       new FluentExpected(true)),
            new(nameof(F.IsGeoLocation.BadLatitude),  (F.IsGeoLocation.BadLatitude.latitude!.Value,  F.IsGeoLocation.BadLatitude.longitude!.Value),  new FluentExpected(false, "Value must be a valid geo location.")),
            new(nameof(F.IsGeoLocation.BadLongitude), (F.IsGeoLocation.BadLongitude.latitude!.Value, F.IsGeoLocation.BadLongitude.longitude!.Value), new FluentExpected(false, "Value must be a valid geo location."))
        ];

        public static TheoryData<FluentCase<(double? latitude, double? longitude)>> NullCases =>
        [
            new(nameof(F.IsGeoLocation.LatNull), F.IsGeoLocation.LatNull, new FluentExpected(true)),
            new(nameof(F.IsGeoLocation.LonNull), F.IsGeoLocation.LonNull, new FluentExpected(true))
        ];
    }
}
