using PineGuard.Utils.Iso;
using System.Text.RegularExpressions;

namespace PineGuard.Iso.Countries;

/// <summary>
/// Represents an ISO 3166 country with all standard code formats.
/// https://www.iso.org/iso-3166-country-codes.html
/// https://en.wikipedia.org/wiki/List_of_ISO_3166_country_codes    
/// </summary>
public sealed partial record IsoCountry
{
    public const string IsoStandard = "ISO 3166";

    public const int Alpha2CodeExactLength = 2;
    public const int Alpha3CodeExactLength = 3;
    public const int NumericCodeExactLength = 3;

    public const string Alpha2CodePattern = "^[A-Za-z]{2}$";
    public const string Alpha3CodePattern = "^[A-Za-z]{3}$";
    public const string NumericCodePattern = "^[0-9]{3}$";

    [GeneratedRegex(Alpha2CodePattern, RegexOptions.CultureInvariant)]
    public static partial Regex Alpha2CodeRegex();

    [GeneratedRegex(Alpha3CodePattern, RegexOptions.CultureInvariant)]
    public static partial Regex Alpha3CodeRegex();

    [GeneratedRegex(NumericCodePattern, RegexOptions.CultureInvariant)]
    public static partial Regex NumericCodeRegex();

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

        if (!Alpha2CodeRegex().IsMatch(alpha2Code))
            throw new ArgumentException($"Alpha2Code should be alphabetical with exact length of {Alpha2CodeExactLength} characters.", nameof(alpha2Code));

        if (!Alpha3CodeRegex().IsMatch(alpha3Code))
            throw new ArgumentException($"Alpha3Code should be alphabetical with exact length of {Alpha3CodeExactLength} characters.", nameof(alpha3Code));

        if (!NumericCodeRegex().IsMatch(numericCode))
            throw new ArgumentException($"NumericCode should be numeric digits only with exact length of {NumericCodeExactLength} characters.", nameof(numericCode));

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

        var provider = DefaultIsoCountryProvider.Instance;

        if (IsoCountryUtility.TryParseAlpha2(value, out var alpha2)
            && provider.TryGetByAlpha2Code(alpha2, out var alpha2Country)
            && alpha2Country is not null)
        {
            country = alpha2Country;
            return true;
        }

        if (IsoCountryUtility.TryParseAlpha3(value, out var alpha3)
            && provider.TryGetByAlpha3Code(alpha3, out var alpha3Country)
            && alpha3Country is not null)
        {
            country = alpha3Country;
            return true;
        }

        if (IsoCountryUtility.TryParseNumeric(value, out var numeric)
            && provider.TryGetByNumericCode(numeric, out var numericCountry)
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

    public override string ToString() => $"[{IsoStandard}] {Name} ({Alpha3Code})";
}
