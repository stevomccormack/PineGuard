using PineGuard.Iso.Languages;
using PineGuard.Rules;

namespace PineGuard.Utils.Iso;

public static class IsoLanguageUtility
{
    public static bool TryParseAlpha2(string? value, out string alpha2)
    {
        alpha2 = string.Empty;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (trimmed.Length != IsoLanguage.Alpha2CodeExactLength)
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
        if (trimmed.Length != IsoLanguage.Alpha3CodeExactLength)
            return false;

        if (!StringRules.IsAlphabetic(trimmed))
            return false;

        alpha3 = trimmed;
        return true;
    }
}
