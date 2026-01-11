using PineGuard.Externals.Iso.Countries;

namespace PineGuard.Rules.Iso;

public static class IsoCountryRules
{
    private static readonly IIsoCountryProvider DefaultProvider = DefaultIsoCountryProvider.Instance;

    public static bool IsIsoAlpha2Code(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        return IsoCountry.Alpha2CodeRegex().IsMatch(trimmed) && provider.ContainsAlpha2Code(trimmed);
    }

    public static bool IsIsoAlpha3Code(string? value, IIsoCountryProvider? provider = null)
    {
        provider ??= DefaultProvider;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoCountry.Alpha3CodeRegex().IsMatch(trimmed))
            return false;

        return provider.ContainsAlpha3Code(trimmed);
    }

    public static bool IsIsoNumericCode(string? value, IIsoCountryProvider? provider = null)
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
