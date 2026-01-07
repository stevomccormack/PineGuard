using PineGuard.Externals.Iana.TimeZones;
using PineGuard.Externals.Iso.Countries;
using PineGuard.Utils.Cldr;

namespace PineGuard.Utils;

public static partial class TimeZoneUtility
{
    /// <summary>
    /// Returns the system time zones available on the current OS.
    /// 
    /// Important: .NET does not provide an ISO country (alpha-2/alpha-3) -> time zone mapping.
    /// To return country-specific time zones, embed a dataset such as IANA zone1970.tab and (on Windows)
    /// an IANA&lt;-&gt;Windows mapping.
    /// </summary>
    public static IReadOnlyCollection<TimeZoneInfo> GetTimeZones(string? isoCountryAlpha2Code)
    {
        return GetTimeZones(isoCountryAlpha2Code, DefaultIanaTimeZoneProvider.Instance);
    }

    internal static IReadOnlyCollection<TimeZoneInfo> GetTimeZones(string? isoCountryAlpha2Code, IIanaTimeZoneProvider ianaProvider)
    {
        ArgumentNullException.ThrowIfNull(ianaProvider);

        if (!StringUtility.TryGetTrimmed(isoCountryAlpha2Code, out var alpha2))
            return [];

        alpha2 = alpha2.ToUpperInvariant();

        if (!ianaProvider.TryGetTimeZoneIdsByCountryAlpha2Code(alpha2, out var ianaTimeZoneIds))
            return [];

        var results = new List<TimeZoneInfo>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var ianaTimeZoneId in ianaTimeZoneIds)
        {
            if (string.IsNullOrWhiteSpace(ianaTimeZoneId))
                continue;

            if (!TryGetSystemTimeZoneFromIanaTimeZoneId(ianaTimeZoneId, territory: alpha2, out var tz) || tz is null)
                continue;

            if (seen.Add(tz.Id))
                results.Add(tz);
        }

        return results;
    }

    public static IReadOnlyCollection<string> GetTimeZoneIds(string? isoCountryAlpha2Code)
    {
        var zones = GetTimeZones(isoCountryAlpha2Code);
        if (zones.Count == 0)
            return [];

        var ids = new string[zones.Count];
        var i = 0;
        foreach (var zone in zones)
            ids[i++] = zone.Id;

        return ids;
    }

    public static IReadOnlyCollection<TimeZoneInfo> GetTimeZonesFromIsoCountryAlpha3(string? isoCountryAlpha3Code)
    {
        var provider = DefaultIsoCountryProvider.Instance;
        if (!provider.TryGetByAlpha3Code(isoCountryAlpha3Code, out var country) || country is null)
            return [];

        return GetTimeZones(country.Alpha2Code);
    }

    public static IReadOnlyCollection<string> GetTimeZoneIdsFromIsoCountryAlpha3(string? isoCountryAlpha3Code)
    {
        var zones = GetTimeZonesFromIsoCountryAlpha3(isoCountryAlpha3Code);
        if (zones.Count == 0)
            return [];

        var ids = new string[zones.Count];
        var i = 0;
        foreach (var zone in zones)
            ids[i++] = zone.Id;

        return ids;
    }

    private static bool TryGetSystemTimeZoneFromIanaTimeZoneId(
        string ianaTimeZoneId,
        string? territory,
        out TimeZoneInfo? systemTimeZone)
    {
        systemTimeZone = null;

        var trimmedIana = ianaTimeZoneId.Trim();

        // First try direct system lookup (IANA on non-Windows, Windows IDs on Windows where applicable).
        if (TimeZoneInfo.TryFindSystemTimeZoneById(trimmedIana, out systemTimeZone))
            return true;

        // Then fall back to CLDR mapping (primarily helpful on Windows, but harmless elsewhere).
        return CldrTimeZoneUtility.TryGetSystemTimeZone(trimmedIana, territory, out systemTimeZone);
    }
}
