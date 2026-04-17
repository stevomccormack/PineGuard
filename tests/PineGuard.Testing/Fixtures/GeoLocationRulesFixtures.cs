using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static class GeoLocationRulesFixtures
{
    public static class IsLatitude
    {
        public static readonly double? ZeroOk = 0.0;
        public static readonly double? MinOk = GeoLocationRules.MinLatitude;
        public static readonly double? MaxOk = GeoLocationRules.MaxLatitude;
        public static readonly double? BelowMin = -90.0001;
        public static readonly double? AboveMax = 90.0001;
        public static readonly double? Null = null;
        public static readonly double? NaN = double.NaN;
        public static readonly double? PosInfinity = double.PositiveInfinity;
        public static readonly double? NegInfinity = double.NegativeInfinity;

        public static RuleScenario<double?>[] ValidScenarios =>
        [
            new(nameof(ZeroOk), ZeroOk, true),
            new(nameof(MinOk),  MinOk,  true),
            new(nameof(MaxOk),  MaxOk,  true)
        ];

        public static RuleScenario<double?>[] ValidEdgeScenarios => [];

        public static RuleScenario<double?>[] InvalidScenarios =>
        [
            new(nameof(BelowMin), BelowMin, false),
            new(nameof(AboveMax), AboveMax, false)
        ];

        public static RuleScenario<double?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null),        Null,        false),
            new(nameof(NaN),         NaN,         false),
            new(nameof(PosInfinity), PosInfinity, false),
            new(nameof(NegInfinity), NegInfinity, false)
        ];

        public static RuleScenario<double?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<double?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<double?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsLongitude
    {
        public static readonly double? ZeroOk = 0.0;
        public static readonly double? MinOk = GeoLocationRules.MinLongitude;
        public static readonly double? MaxOk = GeoLocationRules.MaxLongitude;
        public static readonly double? BelowMin = -180.0001;
        public static readonly double? AboveMax = 180.0001;
        public static readonly double? Null = null;
        public static readonly double? NaN = double.NaN;
        public static readonly double? PosInfinity = double.PositiveInfinity;
        public static readonly double? NegInfinity = double.NegativeInfinity;

        public static RuleScenario<double?>[] ValidScenarios =>
        [
            new(nameof(ZeroOk), ZeroOk, true),
            new(nameof(MinOk),  MinOk,  true),
            new(nameof(MaxOk),  MaxOk,  true)
        ];

        public static RuleScenario<double?>[] ValidEdgeScenarios => [];

        public static RuleScenario<double?>[] InvalidScenarios =>
        [
            new(nameof(BelowMin), BelowMin, false),
            new(nameof(AboveMax), AboveMax, false)
        ];

        public static RuleScenario<double?>[] InvalidEdgeScenarios =>
        [
            new(nameof(Null),        Null,        false),
            new(nameof(NaN),         NaN,         false),
            new(nameof(PosInfinity), PosInfinity, false),
            new(nameof(NegInfinity), NegInfinity, false)
        ];

        public static RuleScenario<double?>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<double?>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<double?>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }

    public static class IsGeoLocation
    {
        public static readonly (double? latitude, double? longitude) Valid = (0.0, 0.0);
        public static readonly (double? latitude, double? longitude) BadLatitude = (100.0, 0.0);
        public static readonly (double? latitude, double? longitude) BadLongitude = (0.0, 200.0);
        public static readonly (double? latitude, double? longitude) LatNull = (null, 0.0);
        public static readonly (double? latitude, double? longitude) LonNull = (0.0, null);

        public static RuleScenario<(double? latitude, double? longitude)>[] ValidScenarios =>
        [
            new(nameof(Valid), Valid, true)
        ];

        public static RuleScenario<(double? latitude, double? longitude)>[] ValidEdgeScenarios => [];

        public static RuleScenario<(double? latitude, double? longitude)>[] InvalidScenarios =>
        [
            new(nameof(BadLatitude),  BadLatitude,  false),
            new(nameof(BadLongitude), BadLongitude, false)
        ];

        public static RuleScenario<(double? latitude, double? longitude)>[] InvalidEdgeScenarios =>
        [
            new(nameof(LatNull), LatNull, false),
            new(nameof(LonNull), LonNull, false)
        ];

        public static RuleScenario<(double? latitude, double? longitude)>[] AllValid => [.. ValidScenarios, .. ValidEdgeScenarios];
        public static RuleScenario<(double? latitude, double? longitude)>[] AllInvalid => [.. InvalidScenarios, .. InvalidEdgeScenarios];
        public static RuleScenario<(double? latitude, double? longitude)>[] AllScenarios => [.. AllValid, .. AllInvalid];
    }
}
