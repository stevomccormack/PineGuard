using PineGuard.Externals.Cldr.TimeZones;
using PineGuard.Externals.Iana.TimeZones;

namespace PineGuard.Rules;

public static partial class TimeZoneRules
{
    public static class Iana
    {
        public static bool IsIanaTimeZoneId(string? value, IIanaTimeZoneProvider? provider = null) =>
            Rules.Iana.IanaTimeZoneRules.IsIanaTimeZoneId(value, provider);
    }

    public static class Cldr
    {
        public static bool IsWindowsTimeZoneId(string? value, ICldrWindowsTimeZoneProvider? provider = null) =>
            Rules.Cldr.CldrTimeZoneRules.IsWindowsTimeZoneId(value, provider);
    }
}
