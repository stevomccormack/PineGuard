using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Utils.Iana;

namespace PineGuard.Rules.Iana;

public static class IanaTimeZoneRules
{
    public static bool IsIanaTimeZoneId(string? value, IIanaTimeZoneProvider? provider = null) =>
        IanaTimeZoneUtility.IsValidTimeZoneId(value, provider);
}
