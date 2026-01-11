using PineGuard.Externals.Iso.Languages;

namespace PineGuard.Rules.Iso;

public static class IsoLanguageRules
{
    private static readonly IIsoLanguageProvider DefaultProvider = DefaultIsoLanguageProvider.Instance;

    public static bool IsIsoAlpha2Code(string? value, IIsoLanguageProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoLanguage.Alpha2CodeRegex().IsMatch(trimmed))
            return false;

        return provider.ContainsAlpha2Code(trimmed);
    }

    public static bool IsIsoAlpha3Code(string? value, IIsoLanguageProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoLanguage.Alpha3CodeRegex().IsMatch(trimmed))
            return false;

        return provider.ContainsAlpha3Code(trimmed);
    }
}
