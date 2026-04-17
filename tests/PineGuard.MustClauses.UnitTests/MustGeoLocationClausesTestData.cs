using PineGuard.Testing.UnitTests.MustClauses;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GeoLocationRulesFixtures;

namespace PineGuard.MustClauses.UnitTests;

public static class MustGeoLocationClausesTestData
{
    public static class Latitude
    {
        public static TheoryData<MustCase<double>> ValidCases =>
            F.IsLatitude.AllValid.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<double>> InvalidCases =>
            F.IsLatitude.AllInvalid.Except(nameof(F.IsLatitude.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, "value must be a valid latitude."));
    }

    public static class Longitude
    {
        public static TheoryData<MustCase<double>> ValidCases =>
            F.IsLongitude.AllValid.Project(v => v!.Value).ToMustCases();

        public static TheoryData<MustCase<double>> InvalidCases =>
            F.IsLongitude.AllInvalid.Except(nameof(F.IsLongitude.Null)).Project(v => v!.Value)
            .ToMustCases(_ => new MustExpected(false, "value must be a valid longitude."));
    }

    public static class GeoLocation
    {
        public static TheoryData<MustCase<(double latitude, double longitude)>> ValidCases =>
            F.IsGeoLocation.AllValid.Project(v => (v.latitude!.Value, v.longitude!.Value))
            .ToMustCases();

        public static TheoryData<MustCase<(double latitude, double longitude)>> InvalidCases =>
            F.IsGeoLocation.AllInvalid
            .Except(nameof(F.IsGeoLocation.LatNull), nameof(F.IsGeoLocation.LonNull))
            .Project(v => (v.latitude!.Value, v.longitude!.Value))
            .ToMustCases(_ => new MustExpected(false, "value must be a valid geo location."));
    }
}
