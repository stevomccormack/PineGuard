using PineGuard.Iso.Languages;

namespace PineGuard.Utils;

public static partial class IsoUtility
{
    public static class Languages
    {
        public static bool TryParseLanguageAlpha2(string? value, out string alpha2) =>
            Iso.IsoLanguageUtility.TryParseAlpha2(value, out alpha2);

        public static bool TryParseLanguageAlpha3(string? value, out string alpha3) =>
            Iso.IsoLanguageUtility.TryParseAlpha3(value, out alpha3);

        public static bool IsIsoLanguageAlpha2(string? value, IIsoLanguageProvider? provider = null) =>
            Rules.Iso.IsoLanguageRules.IsIsoLanguageAlpha2(value, provider);

        public static bool IsIsoLanguageAlpha3(string? value, IIsoLanguageProvider? provider = null) =>
            Rules.Iso.IsoLanguageRules.IsIsoLanguageAlpha3(value, provider);
    }
}
