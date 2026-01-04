using PineGuard.Iso.Currencies;
using PineGuard.Rules.Iso;

namespace PineGuard.Rules;

public static partial class IsoRules
{
    public static class Currencies
    {
        public static bool IsAlpha3(string? value, IIsoCurrencyProvider? provider = null) =>
            IsoCurrencyRules.IsIsoAlpha3Code(value, provider);

        public static bool IsNumeric(string? value, IIsoCurrencyProvider? provider = null) =>
            IsoCurrencyRules.IsIsoNumericCode(value, provider);
    }
}
