using PineGuard.Iso.Currencies;

namespace PineGuard.Rules.Iso;

public static class IsoCurrencyRules
{
    private static readonly IIsoCurrencyProvider DefaultProvider = new DefaultIsoCurrencyProvider();

    public static bool IsIsoCurrencyAlpha3(string? value, IIsoCurrencyProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!StringRules.IsExactLength(value, IsoCurrency.Alpha3ExactLength))
            return false;

        if (!StringRules.IsAlphabetic(value))
            return false;

        return provider.IsValidAlpha3Code(value!);
    }

    public static bool IsIsoCurrencyNumeric(string? value, IIsoCurrencyProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (!StringRules.IsExactLength(value, IsoCurrency.NumericExactLength))
            return false;

        if (!NumberStringRules.IsDigitsOnly(value))
            return false;

        return provider.IsValidNumericCode(value!);
    }
}
