using PineGuard.Iana.TimeZones;

namespace PineGuard.Cldr.TimeZones;

public static class WindowsTimeZoneExtensions
{
    /// <summary>
    /// Maps a Windows time zone ID to an <see cref="IanaTimeZone"/> using the embedded CLDR windowsZones mapping.
    /// Returns null when no mapping is found.
    /// </summary>
    public static IanaTimeZone? ToIanaTimeZone(
        this string? windowsTimeZoneId,
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
}
