namespace PineGuard.Cldr.TimeZones;

using System;
using System.Collections.Frozen;
using System.Collections.Generic;

/// <summary>
/// Default provider for CLDR Windows -> IANA time zone mappings.
/// </summary>
public sealed class DefaultCldrWindowsTimeZoneProvider : ICldrWindowsTimeZoneProvider
{
    public static DefaultCldrWindowsTimeZoneProvider Instance { get; } = new();

    public const string DefaultTerritory = "001";

    private static readonly Lazy<FrozenDictionary<string, FrozenDictionary<string, string[]>>> IanaTimeZoneIdsByWindowsIdIndex =
        new(() => DefaultCldrWindowsTimeZoneData.IanaTimeZoneIdsByWindowsId, isThreadSafe: true);

    // IANA tzdb id -> (territory -> windows id)
    private static readonly Lazy<FrozenDictionary<string, FrozenDictionary<string, string>>> WindowsIdByIanaIdAndTerritoryIndex =
        new(BuildWindowsIdByIanaIdAndTerritoryIndex, isThreadSafe: true);

    private static FrozenDictionary<string, FrozenDictionary<string, string>> BuildWindowsIdByIanaIdAndTerritoryIndex()
    {
        var ianaToTerritoryToWindows = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var winKvp in DefaultCldrWindowsTimeZoneData.IanaTimeZoneIdsByWindowsId)
        {
            var windowsId = winKvp.Key;
            var byTerritory = winKvp.Value;

            foreach (var terrKvp in byTerritory)
            {
                var territory = terrKvp.Key;
                var ianaIds = terrKvp.Value;

                for (var i = 0; i < ianaIds.Length; i++)
                {
                    var ianaId = ianaIds[i];

                    if (!ianaToTerritoryToWindows.TryGetValue(ianaId, out var terrMap))
                    {
                        terrMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        ianaToTerritoryToWindows[ianaId] = terrMap;
                    }

                    // If duplicates ever exist, prefer the lexicographically smallest Windows ID for determinism.
                    if (terrMap.TryGetValue(territory, out var existing))
                    {
                        if (string.Compare(existing, windowsId, StringComparison.OrdinalIgnoreCase) > 0)
                            terrMap[territory] = windowsId;
                    }
                    else
                    {
                        terrMap[territory] = windowsId;
                    }
                }
            }
        }

        return ianaToTerritoryToWindows.ToFrozenDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.ToFrozenDictionary(i => i.Key, i => i.Value, StringComparer.OrdinalIgnoreCase),
            StringComparer.OrdinalIgnoreCase);
    }

    public bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId)
    {
        if (string.IsNullOrWhiteSpace(windowsTimeZoneId))
            return false;

        return IanaTimeZoneIdsByWindowsIdIndex.Value.ContainsKey(windowsTimeZoneId.Trim());
    }

    public bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds)
    {
        ianaTimeZoneIds = [];

        if (string.IsNullOrWhiteSpace(windowsTimeZoneId))
            return false;

        var winId = windowsTimeZoneId.Trim();
        if (!IanaTimeZoneIdsByWindowsIdIndex.Value.TryGetValue(winId, out var byTerritory))
            return false;

        var terr = string.IsNullOrWhiteSpace(territory) ? DefaultTerritory : territory.Trim();

        if (byTerritory.TryGetValue(terr, out var idsForTerritory))
        {
            ianaTimeZoneIds = idsForTerritory;
            return true;
        }

        if (!string.Equals(terr, DefaultTerritory, StringComparison.OrdinalIgnoreCase)
            && byTerritory.TryGetValue(DefaultTerritory, out var idsForDefault))
        {
            ianaTimeZoneIds = idsForDefault;
            return true;
        }

        return false;
    }

    public bool TryGetWindowsTimeZoneId(string? ianaTimeZoneId, string? territory, out string windowsTimeZoneId)
    {
        windowsTimeZoneId = string.Empty;

        if (string.IsNullOrWhiteSpace(ianaTimeZoneId))
            return false;

        var ianaId = ianaTimeZoneId.Trim();

        if (!WindowsIdByIanaIdAndTerritoryIndex.Value.TryGetValue(ianaId, out var byTerritory))
            return false;

        var terr = string.IsNullOrWhiteSpace(territory) ? DefaultTerritory : territory.Trim();

        if (byTerritory.TryGetValue(terr, out var winForTerr))
        {
            windowsTimeZoneId = winForTerr;
            return true;
        }

        if (!string.Equals(terr, DefaultTerritory, StringComparison.OrdinalIgnoreCase)
            && byTerritory.TryGetValue(DefaultTerritory, out var winForDefault))
        {
            windowsTimeZoneId = winForDefault;
            return true;
        }

        // Last resort: choose a deterministic value (the first one in FrozenDictionary enumeration).
        foreach (var kvp in byTerritory)
        {
            windowsTimeZoneId = kvp.Value;
            return true;
        }

        return false;
    }
}
