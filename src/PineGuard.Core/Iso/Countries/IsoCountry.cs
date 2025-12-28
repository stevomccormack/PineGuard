using PineGuard.Rules;
using PineGuard.Utils.Iso;

namespace PineGuard.Iso.Countries;

/// <summary>
/// Represents an ISO 3166 country with all standard code formats.
/// https://www.iso.org/iso-3166-country-codes.html
/// </summary>
public sealed record IsoCountry
{
    public const int Alpha2ExactLength = 2;
    public const int Alpha3ExactLength = 3;
    public const int NumericExactLength = 3;

    /// <summary>
    /// 2-letter country code (e.g., US, GB, FR)
    /// </summary>
    public string Alpha2Code { get; }

    /// <summary>
    /// 3-letter country code (e.g., USA, GBR, FRA)
    /// </summary>
    public string Alpha3Code { get; }

    /// <summary>
    /// 3-digit numeric code (e.g., 840, 826, 250)
    /// </summary>
    public string NumericCode { get; }

    /// <summary>
    /// Official country name
    /// </summary>
    public string Name { get; }

    public IsoCountry(string alpha2Code, string alpha3Code, string numericCode, string name)
    {
        ArgumentNullException.ThrowIfNull(alpha2Code);
        ArgumentNullException.ThrowIfNull(alpha3Code);
        ArgumentNullException.ThrowIfNull(numericCode);
        ArgumentNullException.ThrowIfNull(name);

        if (alpha2Code.Length != Alpha2ExactLength || !StringRules.IsAlphabetic(alpha2Code))
            throw new ArgumentException($"Alpha2Code must be exactly {Alpha2ExactLength} alphabetic characters.", nameof(alpha2Code));

        if (alpha3Code.Length != Alpha3ExactLength || !StringRules.IsAlphabetic(alpha3Code))
            throw new ArgumentException($"Alpha3Code must be exactly {Alpha3ExactLength} alphabetic characters.", nameof(alpha3Code));

        if (numericCode.Length != NumericExactLength || !StringRules.IsNumeric(numericCode))
            throw new ArgumentException($"NumericCode must be exactly {NumericExactLength} digits.", nameof(numericCode));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        Alpha2Code = alpha2Code.ToUpperInvariant();
        Alpha3Code = alpha3Code.ToUpperInvariant();
        NumericCode = numericCode;
        Name = name;
    }

    public static bool TryParse(string? value, out IsoCountry country)
    {
        country = null!;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var provider = new DefaultIsoCountryProvider();

        if (IsoCountryUtility.TryParseAlpha2(value, out var alpha2)
            && provider.TryGetCountryByAlpha2Code(alpha2, out var alpha2Country)
            && alpha2Country is not null)
        {
            country = alpha2Country;
            return true;
        }

        if (IsoCountryUtility.TryParseAlpha3(value, out var alpha3)
            && provider.TryGetCountryByAlpha3Code(alpha3, out var alpha3Country)
            && alpha3Country is not null)
        {
            country = alpha3Country;
            return true;
        }

        if (IsoCountryUtility.TryParseNumeric(value, out var numeric)
            && provider.TryGetCountryByNumericCode(numeric, out var numericCountry)
            && numericCountry is not null)
        {
            country = numericCountry;
            return true;
        }

        country = null!;
        return false;
    }

    public static IsoCountry Parse(string? value)
    {
        if (TryParse(value, out var country))
            return country;

        throw new FormatException("Value must be an ISO 3166-1 alpha-2, alpha-3, or numeric country code.");
    }

    public override string ToString() => $"{Name} ({Alpha3Code})";
}
