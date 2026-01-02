using PineGuard.Iso.Currencies;
using PineGuard.Rules.Iso;

namespace PineGuard.Rules;

public static partial class IsoRules
{
    public static class Currencies
    {
        public static bool IsAlpha3(string? value, IIsoCurrencyProvider? provider = null) =>
            IsoCurrencyRules.IsIsoCurrencyAlpha3(value, provider);

        public static bool IsNumeric(string? value, IIsoCurrencyProvider? provider = null) =>
            IsoCurrencyRules.IsIsoCurrencyNumeric(value, provider);
    }
}
