using PineGuard.Iana.TimeZones;
using System.Collections.Frozen;

namespace PineGuard.Externals.Iana.TimeZones;

/// <summary>
/// Default IANA tzdb time zone provider backed by the embedded zone1970.tab dataset.
/// </summary>
public sealed class DefaultIanaTimeZoneProvider : IIanaTimeZoneProvider
{
    public static DefaultIanaTimeZoneProvider Instance { get; } = new();

    private readonly FrozenDictionary<string, IanaTimeZone> _zonesById;
    private readonly FrozenDictionary<string, FrozenSet<string>> _zoneIdsByCountryAlpha2;
    private readonly IReadOnlyCollection<IanaTimeZone> _allZones;

    public DefaultIanaTimeZoneProvider(
        FrozenDictionary<string, IanaTimeZone>? zonesById = null,
        FrozenDictionary<string, FrozenSet<string>>? zoneIdsByCountryAlpha2 = null)
    {
        _zonesById = zonesById ?? DefaultIanaTimeZoneData.ZonesById;
        _zoneIdsByCountryAlpha2 = zoneIdsByCountryAlpha2 ?? DefaultIanaTimeZoneData.ZoneIdsByCountryAlpha2;

        _allZones = _zonesById.Values;
    }

    public bool IsValidTimeZoneId(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return _zonesById.ContainsKey(value.Trim());
    }

    public bool TryGetById(string? value, out IanaTimeZone? timeZone)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            timeZone = null;
            return false;
        }

        if (_zonesById.TryGetValue(value.Trim(), out var result))
        {
            timeZone = result;
            return true;
        }

        timeZone = null;
        return false;
    }

    public IReadOnlyCollection<IanaTimeZone> GetAll() => _allZones;

    public bool TryGetTimeZoneIdsByCountryAlpha2Code(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds)
    {
        timeZoneIds = [];

        if (string.IsNullOrWhiteSpace(isoCountryAlpha2Code))
            return false;

        var key = isoCountryAlpha2Code.Trim().ToUpperInvariant();

        if (_zoneIdsByCountryAlpha2.TryGetValue(key, out var result))
        {
            timeZoneIds = result;
            return true;
        }

        return false;
    }
}
