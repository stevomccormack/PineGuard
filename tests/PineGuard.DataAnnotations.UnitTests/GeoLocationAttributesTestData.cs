using PineGuard.Codes;
using PineGuard.Testing.UnitTests.DataAnnotations;
using F = PineGuard.Testing.Fixtures.GeoLocationRulesFixtures;

namespace PineGuard.DataAnnotations.UnitTests;

public static class GeoLocationAttributesTestData
{
    public static class Latitude
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLatitude.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsLatitude.Null) => new DataAnnotationExpected(true),
            nameof(F.IsLatitude.NaN) => new DataAnnotationExpected(false),
            nameof(F.IsLatitude.PosInfinity) => new DataAnnotationExpected(false),
            nameof(F.IsLatitude.NegInfinity) => new DataAnnotationExpected(false),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid latitude.", Code: MustCodes.Geo.Latitude.Invalid)
        });
    }

    public static class Longitude
    {
        public static TheoryData<DataAnnotationCase> Cases => F.IsLongitude.AllScenarios.ToDataAnnotationCases(v => v, s => s.Name switch
        {
            nameof(F.IsLongitude.Null) => new DataAnnotationExpected(true),
            nameof(F.IsLongitude.NaN) => new DataAnnotationExpected(false),
            nameof(F.IsLongitude.PosInfinity) => new DataAnnotationExpected(false),
            nameof(F.IsLongitude.NegInfinity) => new DataAnnotationExpected(false),
            _ when s.IsValid => new DataAnnotationExpected(true),
            _ => new DataAnnotationExpected(false, "Value must be a valid longitude.")
        });
    }
}
