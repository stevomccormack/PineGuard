using PineGuard.Iso.Languages;

namespace PineGuard.Rules.Iso;

public static class IsoLanguageRules
{
    private static readonly IIsoLanguageProvider DefaultProvider = new DefaultIsoLanguageProvider();

    public static bool IsIsoLanguageAlpha2(string? value, IIsoLanguageProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!StringRules.IsExactLength(value, IsoLanguage.Alpha2ExactLength))
            return false;

        if (!StringRules.IsAlphabetic(value))
            return false;

        return provider.IsValidAlpha2Code(value!);
    }

    public static bool IsIsoLanguageAlpha3(string? value, IIsoLanguageProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!StringRules.IsExactLength(value, IsoLanguage.Alpha3ExactLength))
            return false;

        if (!StringRules.IsAlphabetic(value))
            return false;

        return provider.IsValidAlpha3Code(value!);
    }
}
