using PineGuard.Iso.Countries;
using PineGuard.Rules;

namespace PineGuard.Utils.Iso;

public static class IsoCountryUtility
{
    public static bool TryParseAlpha2(string? value, out string alpha2)
    {
        alpha2 = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (trimmed.Length != IsoCountry.Alpha2ExactLength)
            return false;

        if (!StringRules.IsAlphabetic(trimmed))
            return false;

        alpha2 = trimmed;
        return true;
    }

    public static bool TryParseAlpha3(string? value, out string alpha3)
    {
        alpha3 = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (trimmed.Length != IsoCountry.Alpha3ExactLength)
            return false;

        if (!StringRules.IsAlphabetic(trimmed))
            return false;

        alpha3 = trimmed;
        return true;
    }

    public static bool TryParseNumeric(string? value, out string numeric)
    {
        numeric = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (trimmed.Length != IsoCountry.NumericExactLength)
            return false;

        if (!StringRules.IsDigitsOnly(trimmed))
            return false;

        numeric = trimmed;
        return true;
    }
}
