using PineGuard.Rules;

namespace PineGuard.Iso.Currencies;

/// <summary>
/// Represents an ISO 4217 currency with all standard formats.
/// https://www.iso.org/iso-4217-currency-codes.html
/// </summary>
public sealed record IsoCurrency
{
    public const int Alpha3ExactLength = 3;
    public const int NumericExactLength = 3;
    public const int MinDecimalPlaces = 0;
    public const int MaxDecimalPlaces = 4;

    /// <summary>
    /// 3-letter currency code (e.g., USD, EUR, GBP)
    /// </summary>
    public string Alpha3Code { get; }

    /// <summary>
    /// 3-digit numeric code (e.g., 840, 978, 826)
    /// </summary>
    public string NumericCode { get; }

    /// <summary>
    /// Number of decimal places (e.g., 2 for USD, 0 for JPY, 3 for BHD)
    /// </summary>
    public int DecimalPlaces { get; }

    /// <summary>
    /// Official currency name (e.g., US Dollar, Euro, Pound Sterling)
    /// </summary>
    public string Name { get; }

    public IsoCurrency(string alpha3Code, string numericCode, int decimalPlaces, string name)
    {
        ArgumentNullException.ThrowIfNull(alpha3Code);
        ArgumentNullException.ThrowIfNull(numericCode);
        ArgumentNullException.ThrowIfNull(name);

        if (alpha3Code.Length != Alpha3ExactLength || !StringRules.IsAlphabetic(alpha3Code))
            throw new ArgumentException($"Alpha3Code must be exactly {Alpha3ExactLength} alphabetic characters.", nameof(alpha3Code));

        if (numericCode.Length != NumericExactLength || !StringRules.IsNumeric(numericCode))
            throw new ArgumentException($"NumericCode must be exactly {NumericExactLength} digits.", nameof(numericCode));

        if (decimalPlaces < MinDecimalPlaces || decimalPlaces > MaxDecimalPlaces)
            throw new ArgumentException($"DecimalPlaces must be between {MinDecimalPlaces} and {MaxDecimalPlaces}.", nameof(decimalPlaces));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));

        Alpha3Code = alpha3Code.ToUpperInvariant();
        NumericCode = numericCode;
        DecimalPlaces = decimalPlaces;
        Name = name;
    }
}
