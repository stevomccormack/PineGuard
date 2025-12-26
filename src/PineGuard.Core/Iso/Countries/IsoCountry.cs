using PineGuard.Rules;

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
}
