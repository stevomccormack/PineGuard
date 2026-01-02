using PineGuard.Cldr.TimeZones;
using PineGuard.Iana.TimeZones;

namespace PineGuard.Utils.Cldr;

public static class CldrTimeZoneUtility
{
    public const string DefaultTerritory = "001"; // CLDR territory code for "World" (global/default mapping)

    public static bool TryParseWindowsTimeZoneId(
        string? value,
        out string windowsTimeZoneId,
        ICldrWindowsTimeZoneProvider? provider = null)
    {
        windowsTimeZoneId = string.Empty;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        provider ??= DefaultCldrWindowsTimeZoneProvider.Instance;

        if (!provider.TryGetIanaTimeZoneIds(trimmed, territory: null, out _))
            return false;

        windowsTimeZoneId = trimmed;
        return true;
    }

    public static string? ToWindowsTimeZoneId(
        string? ianaTimeZoneId,
        string? territory = null,
        ICldrWindowsTimeZoneProvider? provider = null)
    {
        if (string.IsNullOrWhiteSpace(ianaTimeZoneId))
            return null;

        provider ??= DefaultCldrWindowsTimeZoneProvider.Instance;
        territory = string.IsNullOrWhiteSpace(territory) ? DefaultTerritory : territory.Trim();

        return provider.TryGetWindowsTimeZoneId(ianaTimeZoneId.Trim(), territory, out var windowsId)
            ? windowsId
            : null;
    }

    /// <summary>
    /// Maps a Windows time zone ID to an <see cref="IanaTimeZone"/> using the embedded CLDR windowsZones mapping.
    /// Returns null when no mapping is found.
    /// </summary>
    public static IanaTimeZone? ToIanaTimeZone(
        string? windowsTimeZoneId,
        string? territory = null,
        ICldrWindowsTimeZoneProvider? cldrProvider = null,
        IIanaTimeZoneProvider? ianaProvider = null)
    {
        if (string.IsNullOrWhiteSpace(windowsTimeZoneId))
            return null;

        cldrProvider ??= DefaultCldrWindowsTimeZoneProvider.Instance;
        ianaProvider ??= DefaultIanaTimeZoneProvider.Instance;

        if (!cldrProvider.TryGetIanaTimeZoneIds(windowsTimeZoneId, territory, out var ianaIds))
            return null;

        string? firstId = null;
        foreach (var id in ianaIds)
        {
            firstId = id;
            break;
        }

        if (string.IsNullOrWhiteSpace(firstId))
            return null;

        return ianaProvider.TryGetById(firstId, out var tz) ? tz : null;
    }

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
        territory = string.IsNullOrWhiteSpace(territory) ? DefaultTerritory : territory.Trim();
        return provider.TryGetWindowsTimeZoneId(timeZone.Id, territory, out windowsTimeZoneId);
    }

    public static bool TryGetSystemTimeZone(
        string? windowsOrIanaTimeZoneId,
        string? territory,
        out TimeZoneInfo? timeZone,
        ICldrWindowsTimeZoneProvider? cldrProvider = null,
        IIanaTimeZoneProvider? ianaProvider = null)
    {
        timeZone = null;

        if (string.IsNullOrWhiteSpace(windowsOrIanaTimeZoneId))
            return false;

        var id = windowsOrIanaTimeZoneId.Trim();

        if (TimeZoneInfo.TryFindSystemTimeZoneById(id, out timeZone))
            return true;

        if (!OperatingSystem.IsWindows())
        {
            timeZone = null;
            return false;
        }

        ianaProvider ??= DefaultIanaTimeZoneProvider.Instance;
        if (!ianaProvider.TryGetById(id, out var ianaTz) || ianaTz is null)
            return false;

        cldrProvider ??= DefaultCldrWindowsTimeZoneProvider.Instance;
        territory = string.IsNullOrWhiteSpace(territory) ? DefaultTerritory : territory.Trim();

        if (!cldrProvider.TryGetWindowsTimeZoneId(ianaTz.Id, territory, out var windowsId))
            return false;

        return TimeZoneInfo.TryFindSystemTimeZoneById(windowsId, out timeZone);
    }
}
