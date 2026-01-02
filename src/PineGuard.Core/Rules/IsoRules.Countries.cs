using PineGuard.Iso.Countries;
using PineGuard.Rules.Iso;

namespace PineGuard.Rules;

public static partial class IsoRules
{
    public static class Countries
    {
        public static bool IsAlpha2(string? value, IIsoCountryProvider? provider = null) =>
            IsoCountryRules.IsIsoCountryAlpha2(value, provider);

        public static bool IsAlpha3(string? value, IIsoCountryProvider? provider = null) =>
            IsoCountryRules.IsIsoCountryAlpha3(value, provider);

        public static bool IsNumeric(string? value, IIsoCountryProvider? provider = null) =>
            IsoCountryRules.IsIsoCountryNumeric(value, provider);
    }
}
