using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    // ── GeoLocation ─────────────────────────────────────────────────

    public static class IsLatitude
    {
        public static readonly string? Min = "-90";
        public static readonly string? Max = "90";
        public static readonly string? Zero = "0";
        public static readonly string? Below = "-90.0001";
        public static readonly string? Above = "90.0001";
        public static readonly string? NullValue = null;
        public static readonly string? NotNumber = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Min), Min, true), new(nameof(Max), Max, true), new(nameof(Zero), Zero, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Below), Below, false), new(nameof(Above), Above, false), new(nameof(NullValue), NullValue, false), new(nameof(NotNumber), NotNumber, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsLongitude
    {
        public static readonly string? Min = "-180";
        public static readonly string? Max = "180";
        public static readonly string? Zero = "0";
        public static readonly string? Below = "-180.0001";
        public static readonly string? Above = "180.0001";
        public static readonly string? NullValue = null;
        public static readonly string? NotNumber = "abc";

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Min), Min, true), new(nameof(Max), Max, true), new(nameof(Zero), Zero, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Below), Below, false), new(nameof(Above), Above, false), new(nameof(NullValue), NullValue, false), new(nameof(NotNumber), NotNumber, false)];
        public static RuleScenario<string?>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class IsGeoLocation
    {
        public static readonly (string? latitude, string? longitude) Valid = ("0", "0");
        public static readonly (string? latitude, string? longitude) BadLatitude = ("100", "0");
        public static readonly (string? latitude, string? longitude) BadLongitude = ("0", "200");
        public static readonly (string? latitude, string? longitude) NullLatitude = (null, "0");
        public static readonly (string? latitude, string? longitude) NullLongitude = ("0", null);
        public static readonly (string? latitude, string? longitude) LatNotNumber = ("x", "0");
        public static readonly (string? latitude, string? longitude) LonNotNumber = ("0", "x");

        public static RuleScenario<(string? latitude, string? longitude)>[] ValidScenarios => [new(nameof(Valid), Valid, true)];
        public static RuleScenario<(string? latitude, string? longitude)>[] InvalidScenarios => [new(nameof(BadLatitude), BadLatitude, false), new(nameof(BadLongitude), BadLongitude, false), new(nameof(NullLatitude), NullLatitude, false), new(nameof(NullLongitude), NullLongitude, false), new(nameof(LatNotNumber), LatNotNumber, false), new(nameof(LonNotNumber), LonNotNumber, false)];
        public static RuleScenario<(string? latitude, string? longitude)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }
}
