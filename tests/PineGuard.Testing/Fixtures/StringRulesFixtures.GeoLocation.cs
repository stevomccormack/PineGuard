using System.Globalization;
using PineGuard.Testing.UnitTests.Rules;

namespace PineGuard.Testing.Fixtures;

public static partial class StringRulesFixtures
{
    public static class GeoLocationStyles
    {
        public static readonly (string? value, NumberStyles styles, double parsed) Exponent = ("9e1", NumberStyles.Float, 90);
        public static readonly (string? value, NumberStyles styles, double parsed) Decimal = ("45.5", NumberStyles.Float, 45.5);
        public static readonly (string? value, NumberStyles styles, double parsed) ExponentDisallowed = ("9e1", NumberStyles.Number, 0);
        public static readonly (string? value, NumberStyles styles, double parsed) OutOfRange = ("1e3", NumberStyles.Float, 0);
        public static readonly (string? value, NumberStyles styles, double parsed) CultureSpecific = ("45,5", NumberStyles.Float, 0);
        public static readonly (string? value, NumberStyles styles, double parsed) NonFinite = ("NaN", NumberStyles.Float, 0);

        public static RuleScenario<(string? value, NumberStyles styles, double parsed)>[] ValidScenarios => [new(nameof(Exponent), Exponent, true), new(nameof(Decimal), Decimal, true)];
        public static RuleScenario<(string? value, NumberStyles styles, double parsed)>[] InvalidScenarios => [new(nameof(ExponentDisallowed), ExponentDisallowed, false), new(nameof(OutOfRange), OutOfRange, false), new(nameof(CultureSpecific), CultureSpecific, false), new(nameof(NonFinite), NonFinite, false)];
        public static RuleScenario<(string? value, NumberStyles styles, double parsed)>[] AllScenarios => [.. ValidScenarios, .. InvalidScenarios];
    }

    public static class GeoLocationInvalidStyles
    {
        public static readonly (string? value, NumberStyles styles) NullHex = (null, NumberStyles.HexNumber);
        public static readonly (string? value, NumberStyles styles) EmptyUndefined = (string.Empty, (NumberStyles)4096);
        public static readonly (string? value, NumberStyles styles) MalformedBinary = ("malformed", NumberStyles.BinaryNumber);
        public static readonly (string? value, NumberStyles styles) ValidValueInvalidFlags = ("0", (NumberStyles)(-1));

        public static RuleScenario<(string? value, NumberStyles styles)>[] AllScenarios => [new(nameof(NullHex), NullHex, false), new(nameof(EmptyUndefined), EmptyUndefined, false), new(nameof(MalformedBinary), MalformedBinary, false), new(nameof(ValidValueInvalidFlags), ValidValueInvalidFlags, false)];
    }

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
        public static readonly string? ExponentWithoutOptIn = GeoLocationStyles.Exponent.value;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Min), Min, true), new(nameof(Max), Max, true), new(nameof(Zero), Zero, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Below), Below, false), new(nameof(Above), Above, false), new(nameof(NullValue), NullValue, false), new(nameof(NotNumber), NotNumber, false), new(nameof(ExponentWithoutOptIn), ExponentWithoutOptIn, false)];
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
        public static readonly string? ExponentWithoutOptIn = GeoLocationStyles.Exponent.value;

        public static RuleScenario<string?>[] ValidScenarios => [new(nameof(Min), Min, true), new(nameof(Max), Max, true), new(nameof(Zero), Zero, true)];
        public static RuleScenario<string?>[] InvalidScenarios => [new(nameof(Below), Below, false), new(nameof(Above), Above, false), new(nameof(NullValue), NullValue, false), new(nameof(NotNumber), NotNumber, false), new(nameof(ExponentWithoutOptIn), ExponentWithoutOptIn, false)];
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
