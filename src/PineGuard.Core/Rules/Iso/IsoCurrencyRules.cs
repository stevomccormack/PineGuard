using PineGuard.Iso.Countries;
using PineGuard.Iso.Currencies;
using PineGuard.Utils.Iso;

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

        if (!IsoCurrencyUtility.TryParseAlpha3(trimmed, out var alpha3))
            return false;

        return provider.ContainsAlpha3Code(alpha3);
    }

    public static bool IsIsoNumericCode(string? value, IIsoCurrencyProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoCountry.NumericCodeRegex().IsMatch(trimmed))
            return false;

        if (!IsoCurrencyUtility.TryParseNumeric(trimmed, out var numeric))
            return false;

        return provider.ContainsNumericCode(numeric);
    }
}
