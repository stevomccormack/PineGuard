using PineGuard.Iso.Countries;

namespace PineGuard.Rules.Iso;

public static class IsoCountryRules
{
    private static readonly IIsoCountryProvider DefaultProvider = new DefaultIsoCountryProvider();

    public static bool IsIsoCountryAlpha2(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!StringRules.IsExactLength(value, IsoCountry.Alpha2ExactLength))
            return false;

        if (!StringRules.IsAlphabetic(value))
            return false;

        return provider.IsValidAlpha2Code(value!);
    }

    public static bool IsIsoCountryAlpha3(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!StringRules.IsExactLength(value, IsoCountry.Alpha3ExactLength))
            return false;

        if (!StringRules.IsAlphabetic(value))
            return false;

        return provider.IsValidAlpha3Code(value!);
    }

    public static bool IsIsoCountryNumeric(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!StringRules.IsExactLength(value, IsoCountry.NumericExactLength))
            return false;

        if (!NumberStringRules.IsDigitsOnly(value))
            return false;

        return provider.IsValidNumericCode(value!);
    }
}
