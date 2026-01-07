using PineGuard.Externals.Iso.Currencies;
using PineGuard.Iso.Countries;

namespace PineGuard.Rules.Iso;

public static partial class IsoCurrencyRules
{
    private static readonly IIsoCurrencyProvider DefaultProvider = DefaultIsoCurrencyProvider.Instance;

    public static bool IsIsoAlpha3Code(string? value, IIsoCurrencyProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoCountry.Alpha3CodeRegex().IsMatch(trimmed))
            return false;

        return provider.ContainsAlpha3Code(trimmed);
    }

    public static bool IsIsoNumericCode(string? value, IIsoCurrencyProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoCountry.NumericCodeRegex().IsMatch(trimmed))
            return false;

        return provider.ContainsNumericCode(trimmed);
    }
}
