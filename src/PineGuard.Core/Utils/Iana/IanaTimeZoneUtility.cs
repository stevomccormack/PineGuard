using PineGuard.Iana.TimeZones;

namespace PineGuard.Utils.Iana;

public static class IanaTimeZoneUtility
{
    public static bool TryParseTimeZoneId(string? value, out string timeZoneId)
    {
        return StringUtility.TryGetTrimmed(value, out timeZoneId);
    }

    public static bool IsValidTimeZoneId(string? value, IIanaTimeZoneProvider? provider = null)
    {
        provider ??= DefaultIanaTimeZoneProvider.Instance;
        return provider.IsValidTimeZoneId(value);
    }
}
