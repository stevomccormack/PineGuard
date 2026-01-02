using PineGuard.Iana.TimeZones;

namespace PineGuard.Utils;

public static partial class TimeZoneUtility
{
    public static class Iana
    {
        public static bool TryParseTimeZoneId(string? value, out string ianaTimeZoneId) =>
            Utils.Iana.IanaTimeZoneUtility.TryParseTimeZoneId(value, out ianaTimeZoneId);

        public static bool IsValidTimeZoneId(string? value, IIanaTimeZoneProvider? provider = null) =>
            Utils.Iana.IanaTimeZoneUtility.IsValidTimeZoneId(value, provider);
    }
}
