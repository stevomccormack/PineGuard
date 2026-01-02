using PineGuard.Cldr.TimeZones;
using PineGuard.Iana.TimeZones;
using PineGuard.Utils.Cldr;

namespace PineGuard.Utils;

public static partial class TimeZoneUtility
{
    public static class Cldr
    {
        public static bool TryParseWindowsTimeZoneId(string? value, out string windowsTimeZoneId) =>
            CldrTimeZoneUtility.TryParseWindowsTimeZoneId(value, out windowsTimeZoneId);

        public static bool IsWindowsTimeZoneId(string? value, ICldrWindowsTimeZoneProvider? provider = null) =>
            Rules.Cldr.CldrTimeZoneRules.IsWindowsTimeZoneId(value, provider);

        public static string? ToWindowsTimeZoneId(
            string? ianaTimeZoneId,
            string? territory = null,
            ICldrWindowsTimeZoneProvider? provider = null) =>
            CldrTimeZoneUtility.ToWindowsTimeZoneId(ianaTimeZoneId, territory, provider);

        public static bool TryGetWindowsTimeZoneId(
            TimeZoneInfo? timeZone,
            string? territory,
            out string windowsTimeZoneId,
            ICldrWindowsTimeZoneProvider? provider = null) =>
            CldrTimeZoneUtility.TryGetWindowsTimeZoneId(timeZone, territory, out windowsTimeZoneId, provider);

        public static bool TryGetSystemTimeZone(
            string? windowsOrIanaTimeZoneId,
            string? territory,
            out TimeZoneInfo? timeZone,
            ICldrWindowsTimeZoneProvider? cldrProvider = null,
            IIanaTimeZoneProvider? ianaProvider = null) =>
            CldrTimeZoneUtility.TryGetSystemTimeZone(windowsOrIanaTimeZoneId, territory, out timeZone, cldrProvider, ianaProvider);

        public static IanaTimeZone? ToIanaTimeZone(
            string? windowsTimeZoneId,
            string? territory = null,
            ICldrWindowsTimeZoneProvider? cldrProvider = null,
            IIanaTimeZoneProvider? ianaProvider = null) =>
            CldrTimeZoneUtility.ToIanaTimeZone(windowsTimeZoneId, territory, cldrProvider, ianaProvider);
    }
}
