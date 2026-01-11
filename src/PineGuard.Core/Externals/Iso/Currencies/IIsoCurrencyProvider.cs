namespace PineGuard.Externals.Iso.Currencies;

/// <summary>
/// Provides validation and lookup services for ISO 4217 currency codes.
/// Thread-safe for concurrent reads. All implementations must be immutable.
/// https://www.iso.org/iso-4217-currency-codes.html
/// </summary>
public interface IIsoCurrencyProvider
{
    /// <summary>
    /// Validates a 3-letter ISO 4217 alphabetic currency code (case-insensitive).
    /// </summary>
    bool ContainsAlpha3Code(string? value);

    /// <summary>
    /// Validates a 3-digit ISO 4217 numeric currency code.
    /// </summary>
    bool ContainsNumericCode(string? value);

    /// <summary>
    /// Attempts to retrieve a currency by its 3-letter alphabetic code (case-insensitive).
    /// </summary>
    bool TryGetByAlpha3Code(string? value, out IsoCurrency? currency);

    /// <summary>
    /// Attempts to retrieve a currency by its 3-digit numeric code.
    /// </summary>
    bool TryGetByNumericCode(string? value, out IsoCurrency? currency);

    /// <summary>
    /// Attempts to retrieve a currency by any ISO 4217 code format.
    /// Tries alphabetic, then numeric in sequence.
    /// </summary>
    bool TryGet(string? value, out IsoCurrency? currency);

    /// <summary>
    /// Gets all currencies as a read-only collection.
    /// </summary>
    IReadOnlyCollection<IsoCurrency> GetAll();
}
