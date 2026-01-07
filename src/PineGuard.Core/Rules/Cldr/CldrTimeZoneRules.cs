using PineGuard.Externals.Cldr.TimeZones;

namespace PineGuard.Rules.Cldr;

public static class CldrTimeZoneRules
{
    public static bool IsWindowsTimeZoneId(string? value, ICldrWindowsTimeZoneProvider? provider = null)
    {
        provider ??= DefaultCldrWindowsTimeZoneProvider.Instance;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return provider.IsValidWindowsTimeZoneId(value);
    }
}
