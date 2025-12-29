using System;

namespace PineGuard.Cldr.TimeZones;

public static class TimeZoneInfoWindowsIdUtility
{
    /// <summary>
    /// Gets the Windows time zone ID for a <see cref="TimeZoneInfo"/>.
    /// 
    /// On Windows, <see cref="TimeZoneInfo.Id"/> is already a Windows time zone ID.
    /// On non-Windows platforms, <see cref="TimeZoneInfo.Id"/> is typically an IANA tzdb ID and is mapped via CLDR.
    /// </summary>
    public static bool TryGetWindowsTimeZoneId(
        TimeZoneInfo? timeZone,
        string? territory,
        out string windowsTimeZoneId,
        ICldrWindowsTimeZoneProvider? provider = null)
    {
        windowsTimeZoneId = string.Empty;

        if (timeZone is null)
            return false;

        if (OperatingSystem.IsWindows())
        {
            if (string.IsNullOrWhiteSpace(timeZone.Id))
                return false;

            windowsTimeZoneId = timeZone.Id.Trim();
            return true;
        }

        provider ??= DefaultCldrWindowsTimeZoneProvider.Instance;
        return provider.TryGetWindowsTimeZoneId(timeZone.Id, territory, out windowsTimeZoneId);
    }
}
