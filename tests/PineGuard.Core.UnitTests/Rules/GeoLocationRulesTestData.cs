using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.GeoLocationRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules;

public static class GeoLocationRulesTestData
{
    public static class IsLatitude
    {
        public static TheoryData<RuleCase<double?>> Cases => F.IsLatitude.AllScenarios.ToRuleCases();
    }

    public static class IsLongitude
    {
        public static TheoryData<RuleCase<double?>> Cases => F.IsLongitude.AllScenarios.ToRuleCases();
    }

    public static class IsGeoLocation
    {
        public static TheoryData<RuleCase<(double? latitude, double? longitude)>> Cases => F.IsGeoLocation.AllScenarios.ToRuleCases();
    }
}
