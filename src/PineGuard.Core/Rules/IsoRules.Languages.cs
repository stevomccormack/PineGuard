using PineGuard.Iso.Languages;
using PineGuard.Rules.Iso;

namespace PineGuard.Rules;

public static partial class IsoRules
{
    public static class Languages
    {
        public static bool IsAlpha2(string? value, IIsoLanguageProvider? provider = null) =>
            IsoLanguageRules.IsIsoAlpha2Code(value, provider);

        public static bool IsAlpha3(string? value, IIsoLanguageProvider? provider = null) =>
            IsoLanguageRules.IsIsoAlpha3Code(value, provider);
    }
}
