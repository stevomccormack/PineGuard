using PineGuard.Iso.Languages;
using PineGuard.Utils.Iso;

namespace PineGuard.Rules.Iso;

public static partial class IsoLanguageRules
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

        if (!IsoLanguageUtility.TryParseAlpha2(trimmed, out var alpha2))
            return false;

        return provider.ContainsAlpha2Code(alpha2);
    }

    public static bool IsIsoAlpha3Code(string? value, IIsoLanguageProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoLanguage.Alpha3CodeRegex().IsMatch(trimmed))
            return false;

        if (!IsoLanguageUtility.TryParseAlpha3(trimmed, out var alpha3))
            return false;

        return provider.ContainsAlpha3Code(alpha3);
    }
}
