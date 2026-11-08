using System.Globalization;
using PineGuard.Testing.UnitTests.Rules;
using F = PineGuard.Testing.Fixtures.StringRulesFixtures;

namespace PineGuard.Core.UnitTests.Rules.StringRules;

public static class StringRulesGeoLocationTestData
{
    public static class Styles
    {
        public static TheoryData<RuleCase<(string? value, NumberStyles styles, double parsed)>> Cases => F.GeoLocationStyles.AllScenarios.ToRuleCases();
    }

    public static class InvalidStyles
    {
        public static TheoryData<RuleCase<(string? value, NumberStyles styles)>> Cases => F.GeoLocationInvalidStyles.AllScenarios.ToRuleCases();
    }

    public static class IsLatitude
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsLatitude.AllScenarios.ToRuleCases();
    }

    public static class IsLongitude
    {
        public static TheoryData<RuleCase<string?>> Cases => F.IsLongitude.AllScenarios.ToRuleCases();
    }

    public static class IsGeoLocation
    {
        public static TheoryData<RuleCase<(string? latitude, string? longitude)>> Cases => F.IsGeoLocation.AllScenarios.ToRuleCases();
    }
}
