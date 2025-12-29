namespace PineGuard.Utils.Cldr;

public static class CldrWindowsTimeZoneUtility
{
    public const string DefaultTerritory = "001";

    public static bool TryParseWindowsTimeZoneId(string? value, out string windowsTimeZoneId)
    {
        return StringUtility.TryGetTrimmed(value, out windowsTimeZoneId);
    }

    public static string NormalizeTerritory(string? territory)
    {
        return StringUtility.TryGetTrimmed(territory, out var trimmed)
            ? trimmed
            : DefaultTerritory;
    }
}
