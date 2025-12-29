using PineGuard.Iana.TimeZones;

namespace PineGuard.Rules.Iana;

public static class IanaTimeZoneRules
{
    public static bool IsIanaTimeZoneId(string? value, IIanaTimeZoneProvider? provider = null)
    {
        provider ??= DefaultIanaTimeZoneProvider.Instance;
        return provider.IsValidTimeZoneId(value);
    }

    public static bool TryGetIanaTimeZone(string? value, out IanaTimeZone? timeZone, IIanaTimeZoneProvider? provider = null)
    {
        provider ??= DefaultIanaTimeZoneProvider.Instance;
        return provider.TryGetById(value, out timeZone);
    }

    public static IReadOnlyCollection<IanaTimeZone> GetAll(IIanaTimeZoneProvider? provider = null)
    {
        provider ??= DefaultIanaTimeZoneProvider.Instance;
        return provider.GetAll();
    }

    public static bool TryGetIanaTimeZoneIdsForCountryAlpha2(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds, IIanaTimeZoneProvider? provider = null)
    {
        provider ??= DefaultIanaTimeZoneProvider.Instance;
        return provider.TryGetTimeZoneIdsByCountryAlpha2Code(isoCountryAlpha2Code, out timeZoneIds);
    }
}
