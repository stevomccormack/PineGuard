using PineGuard.Cldr.TimeZones;
using PineGuard.Iana.TimeZones;

namespace PineGuard.Utils.Cldr;

public static class CldrTimeZoneExtension
{
    /// <summary>
    /// Maps a <see cref="TimeZoneInfo"/> to an <see cref="IanaTimeZone"/>.
    /// On Windows, <see cref="TimeZoneInfo.Id"/> is typically a Windows time zone ID.
    /// On non-Windows platforms, <see cref="TimeZoneInfo.Id"/> is typically an IANA tzdb ID.
    /// </summary>
    public static IanaTimeZone? ToIanaTimeZone(
        this string? windowsTimeZoneId,
        string? territory = null,
        ICldrWindowsTimeZoneProvider? cldrProvider = null,
        IIanaTimeZoneProvider? ianaProvider = null)
    {
        return CldrTimeZoneUtility.ToIanaTimeZone(windowsTimeZoneId, territory, cldrProvider, ianaProvider);
    }

    /// <summary>
    /// Maps a <see cref="TimeZoneInfo"/> to an <see cref="IanaTimeZone"/>.
    /// On Windows, <see cref="TimeZoneInfo.Id"/> is typically a Windows time zone ID.  
    /// On non-Windows platforms, <see cref="TimeZoneInfo.Id"/> is typically an IANA tzdb ID.
    /// </summary>
    public static IanaTimeZone? ToIanaTimeZone(
        this TimeZoneInfo? windowsTimeZone,
        string? territory = null,
        ICldrWindowsTimeZoneProvider? cldrProvider = null,
        IIanaTimeZoneProvider? ianaProvider = null)
    {
        if (windowsTimeZone is null)
            return null;

        if (OperatingSystem.IsWindows())
            return CldrTimeZoneUtility.ToIanaTimeZone(windowsTimeZone.Id, territory, cldrProvider, ianaProvider);

        ianaProvider ??= DefaultIanaTimeZoneProvider.Instance;
        return ianaProvider.TryGetById(windowsTimeZone.Id, out var tz) ? tz : null;
    }
}
