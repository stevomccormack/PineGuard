using PineGuard.Iana.TimeZones;
using PineGuard.Iso.Countries;
using PineGuard.Cldr.TimeZones;

namespace PineGuard.Utils;

public static class TimeZoneUtility
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
        if (!StringUtility.TryGetTrimmed(isoCountryAlpha2Code, out var alpha2))
            return [];

        alpha2 = alpha2.ToUpperInvariant();

        var ianaProvider = DefaultIanaTimeZoneProvider.Instance;
        if (!ianaProvider.TryGetTimeZoneIdsByCountryAlpha2Code(alpha2, out var ianaTimeZoneIds))
            return [];

        var results = new List<TimeZoneInfo>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var ianaId in ianaTimeZoneIds)
        {
            if (string.IsNullOrWhiteSpace(ianaId))
                continue;

            if (OperatingSystem.IsWindows())
            {
                if (!ianaProvider.TryGetById(ianaId, out var ianaTimeZone) || ianaTimeZone is null)
                    continue;

                var windowsId = ianaTimeZone.ToWindowsTimeZone(territory: alpha2);
                if (string.IsNullOrWhiteSpace(windowsId))
                    windowsId = ianaTimeZone.ToWindowsTimeZone();

                if (string.IsNullOrWhiteSpace(windowsId))
                    continue;

                try
                {
                    var tz = TimeZoneInfo.FindSystemTimeZoneById(windowsId);
                    if (seen.Add(tz.Id))
                        results.Add(tz);
                }
                catch
                {
                    // Ignore unknown/unavailable system time zones.
                }

                continue;
            }

            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(ianaId);
                if (seen.Add(tz.Id))
                    results.Add(tz);
            }
            catch
            {
                // Ignore unknown/unavailable system time zones.
            }
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
}
