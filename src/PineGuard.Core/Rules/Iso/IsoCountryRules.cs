using PineGuard.Iso.Countries;
using PineGuard.Utils.Iso;

namespace PineGuard.Rules.Iso;

public static partial class IsoCountryRules
{
    private static readonly IIsoCountryProvider DefaultProvider = DefaultIsoCountryProvider.Instance;

    public static bool IsIsoAlpha2Code(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoCountry.Alpha2CodeRegex().IsMatch(trimmed))
            return false;

        if (!IsoCountryUtility.TryParseAlpha2(trimmed, out var alpha2))
            return false;

        return provider.ContainsAlpha2Code(alpha2);
    }

    public static bool IsIsoAlpha3Code(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoCountry.Alpha3CodeRegex().IsMatch(trimmed))
            return false;

        if (!IsoCountryUtility.TryParseAlpha3(trimmed, out var alpha3))
            return false;

        return provider.ContainsAlpha3Code(alpha3);
    }

    public static bool IsIsoNumericCode(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoCountry.NumericCodeRegex().IsMatch(trimmed))
            return false;

        if (!IsoCountryUtility.TryParseNumeric(trimmed, out var numeric))
            return false;

        return provider.ContainsNumericCode(numeric);
    }
}
