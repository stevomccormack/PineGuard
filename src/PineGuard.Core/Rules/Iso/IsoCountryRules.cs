using PineGuard.Iso.Countries;
using PineGuard.Utils.Iso;

namespace PineGuard.Rules.Iso;

public static class IsoCountryRules
{
    private static readonly IIsoCountryProvider DefaultProvider = new DefaultIsoCountryProvider();

    public static bool IsIsoCountryAlpha2(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!IsoCountryUtility.TryParseAlpha2(value, out var alpha2))
            return false;

        return provider.IsValidAlpha2Code(alpha2);
    }

    public static bool IsIsoCountryAlpha3(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!IsoCountryUtility.TryParseAlpha3(value, out var alpha3))
            return false;

        return provider.IsValidAlpha3Code(alpha3);
    }

    public static bool IsIsoCountryNumeric(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!IsoCountryUtility.TryParseNumeric(value, out var numeric))
            return false;

        return provider.IsValidNumericCode(numeric);
    }
}
