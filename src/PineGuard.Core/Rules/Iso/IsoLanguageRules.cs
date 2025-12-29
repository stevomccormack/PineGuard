using PineGuard.Iso.Languages;
using PineGuard.Utils.Iso;

namespace PineGuard.Rules.Iso;

public static class IsoLanguageRules
{
    private static readonly IIsoLanguageProvider DefaultProvider = DefaultIsoLanguageProvider.Instance;

    public static bool IsIsoLanguageAlpha2(string? value, IIsoLanguageProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!IsoLanguageUtility.TryParseAlpha2(value, out var alpha2))
            return false;

        return provider.IsValidAlpha2Code(alpha2);
    }

    public static bool IsIsoLanguageAlpha3(string? value, IIsoLanguageProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!IsoLanguageUtility.TryParseAlpha3(value, out var alpha3))
            return false;

        return provider.IsValidAlpha3Code(alpha3);
    }
}
