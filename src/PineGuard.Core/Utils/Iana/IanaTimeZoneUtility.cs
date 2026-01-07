using PineGuard.Externals.Iana.TimeZones;

namespace PineGuard.Utils.Iana;

public static class IanaTimeZoneUtility
{
    public static bool TryParseTimeZoneId(string? value, out string ianaTimeZoneId) =>
        StringUtility.TryGetTrimmed(value, out ianaTimeZoneId);

    public static bool IsValidTimeZoneId(string? value, IIanaTimeZoneProvider? provider = null)
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

    public static bool TryGetIanaTimeZoneIdsForCountryAlpha2(
        string? isoCountryAlpha2Code,
        out IReadOnlyCollection<string> timeZoneIds,
        IIanaTimeZoneProvider? provider = null)
    {
        provider ??= DefaultIanaTimeZoneProvider.Instance;
        return provider.TryGetTimeZoneIdsByCountryAlpha2Code(isoCountryAlpha2Code, out timeZoneIds);
    }
}
