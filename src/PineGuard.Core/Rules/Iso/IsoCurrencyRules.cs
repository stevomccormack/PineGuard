using PineGuard.Iso.Currencies;
using PineGuard.Utils.Iso;

namespace PineGuard.Rules.Iso;

public static class IsoCurrencyRules
{
    private static readonly IIsoCurrencyProvider DefaultProvider = DefaultIsoCurrencyProvider.Instance;

    public static bool IsIsoCurrencyAlpha3(string? value, IIsoCurrencyProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!IsoCurrencyUtility.TryParseAlpha3(value, out var alpha3))
            return false;

        return provider.IsValidAlpha3Code(alpha3);
    }

    public static bool IsIsoCurrencyNumeric(string? value, IIsoCurrencyProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!IsoCurrencyUtility.TryParseNumeric(value, out var numeric))
            return false;

        return provider.IsValidNumericCode(numeric);
    }
}
