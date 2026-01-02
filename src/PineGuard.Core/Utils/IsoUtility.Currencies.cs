using PineGuard.Iso.Currencies;

namespace PineGuard.Utils;

public static partial class IsoUtility
{
    public static class Currencies
    {
        public static bool TryParseCurrencyAlpha3(string? value, out string alpha3) =>
            Iso.IsoCurrencyUtility.TryParseAlpha3(value, out alpha3);

        public static bool TryParseCurrencyNumeric(string? value, out string numeric) =>
            Iso.IsoCurrencyUtility.TryParseNumeric(value, out numeric);

        public static bool IsIsoCurrencyAlpha3(string? value, IIsoCurrencyProvider? provider = null) =>
            Rules.Iso.IsoCurrencyRules.IsIsoCurrencyAlpha3(value, provider);

        public static bool IsIsoCurrencyNumeric(string? value, IIsoCurrencyProvider? provider = null) =>
            Rules.Iso.IsoCurrencyRules.IsIsoCurrencyNumeric(value, provider);
    }
}
