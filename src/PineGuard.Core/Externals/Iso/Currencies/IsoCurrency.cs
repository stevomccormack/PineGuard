using PineGuard.Extensions;
using PineGuard.Externals.Iso.Currencies;
using PineGuard.Utils.Iso;
using System.Text.RegularExpressions;

namespace PineGuard.Iso.Currencies;

/// <summary>
/// Represents an ISO 4217 currency with all standard formats.
/// https://www.iso.org/iso-4217-currency-codes.html
/// https://en.wikipedia.org/wiki/ISO_4217
/// </summary>
public sealed partial record IsoCurrency
{
    public const string IsoStandard = "ISO 4217";

    public const int Alpha3CodeExactLength = 3;
    public const int NumericCodeExactLength = 3;
    public const int NotApplicableDecimalPlaces = -1;
    public const int MinDecimalPlaces = 0;
    public const int MaxDecimalPlaces = 4;

    public const string Alpha3CodePattern = "^[A-Za-z]{3}$";
    public const string NumericCodePattern = "^[0-9]{3}$";

    [GeneratedRegex(Alpha3CodePattern, RegexOptions.CultureInvariant)]
    public static partial Regex Alpha3CodeRegex();

    [GeneratedRegex(NumericCodePattern, RegexOptions.CultureInvariant)]
    public static partial Regex NumericCodeRegex();

    /// <summary>
    /// 3-letter currency code (e.g., USD, EUR, GBP)
    /// </summary>
    public string Alpha3Code { get; }

    /// <summary>
    /// 3-digit numeric code (e.g., 840, 978, 826)
    /// </summary>
    public string NumericCode { get; }

    /// <summary>
    /// Number of decimal places (e.g., 2 for USD, 0 for JPY, 3 for BHD).
    /// Some ISO 4217 entries use <see cref="NotApplicableDecimalPlaces"/> (e.g., precious metals, testing codes).
    /// </summary>
    public int DecimalPlaces { get; } = MinDecimalPlaces;

    /// <summary>
    /// Official currency name (e.g., US Dollar, Euro, Pound Sterling)
    /// </summary>
    public string Name { get; }

    public IsoCurrency(string alpha3Code, string numericCode, int decimalPlaces, string name)
    {
        ArgumentNullException.ThrowIfNull(alpha3Code);
        ArgumentNullException.ThrowIfNull(numericCode);
        ArgumentNullException.ThrowIfNull(name);

        if (!Alpha3CodeRegex().IsMatch(alpha3Code))
            throw new ArgumentException($"{nameof(alpha3Code).TitleCase()} must be alphabetic with exact length of {Alpha3CodeExactLength} characters.", nameof(alpha3Code));

        if (!NumericCodeRegex().IsMatch(numericCode))
            throw new ArgumentException($"{nameof(numericCode).TitleCase()} must contain digits only with exact length of {NumericCodeExactLength} characters.", nameof(numericCode));

        if (decimalPlaces != NotApplicableDecimalPlaces
            && (decimalPlaces < MinDecimalPlaces || decimalPlaces > MaxDecimalPlaces))
            throw new ArgumentOutOfRangeException(nameof(decimalPlaces), decimalPlaces, $"{nameof(decimalPlaces).TitleCase()} must be between {MinDecimalPlaces} and {MaxDecimalPlaces}.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{nameof(name).TitleCase()} cannot be null or whitespace.", nameof(name));

        Alpha3Code = alpha3Code.ToUpperInvariant();
        NumericCode = numericCode;
        DecimalPlaces = decimalPlaces;
        Name = name;
    }

    public static bool TryParse(string? value, out IsoCurrency currency)
    {
        currency = null!;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var provider = DefaultIsoCurrencyProvider.Instance;

        if (IsoCurrencyUtility.TryParseAlpha3(value, out var alpha3)
            && provider.TryGetByAlpha3Code(alpha3, out var alpha3Currency)
            && alpha3Currency is not null)
        {
            currency = alpha3Currency;
            return true;
        }

        if (IsoCurrencyUtility.TryParseNumeric(value, out var numeric)
            && provider.TryGetByNumericCode(numeric, out var numericCurrency)
            && numericCurrency is not null)
        {
            currency = numericCurrency;
            return true;
        }

        currency = null!;
        return false;
    }

    public static IsoCurrency Parse(string? value)
    {
        if (TryParse(value, out var currency))
            return currency;

        throw new FormatException($"{nameof(value).TitleCase()} must be an ISO 4217 alpha-3 or numeric currency code.");
    }

    public override string ToString() => $"[{IsoStandard}] {Name} ({Alpha3Code})";
}
