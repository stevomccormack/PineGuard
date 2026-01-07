using PineGuard.Externals.Cldr.TimeZones;
using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Utils.Cldr;

namespace PineGuard.Utils.Iana;

public static class IanaTimeZoneExtension
{
    /// <summary>
    /// Maps an IANA tzdb time zone ID (or a <see cref="TimeZoneInfo"/> whose <see cref="TimeZoneInfo.Id"/> is IANA on non-Windows)
    /// to a Windows time zone ID using CLDR windowsZones mapping.
    /// </summary>
    public static string? ToWindowsTimeZoneId(
        this string? ianaTimeZoneId,
        string? territory = null,
        ICldrWindowsTimeZoneProvider? cldrProvider = null)
    {
        return CldrTimeZoneUtility.ToWindowsTimeZoneId(ianaTimeZoneId, territory, cldrProvider);
    }

    /// <summary>
    /// Maps a <see cref="TimeZoneInfo"/> to a Windows time zone ID using CLDR windowsZones mapping.
    /// On Windows, <see cref="TimeZoneInfo.Id"/> is typically already the Windows time zone ID.
    /// </summary>
    public static string? ToWindowsTimeZoneId(
        this IanaTimeZone? ianaTimeZone,
        string? territory = null,
        ICldrWindowsTimeZoneProvider? cldrProvider = null)
    {
        if (ianaTimeZone is null)
            return null;

        return CldrTimeZoneUtility.ToWindowsTimeZoneId(ianaTimeZone.Id, territory, cldrProvider);
    }
}
