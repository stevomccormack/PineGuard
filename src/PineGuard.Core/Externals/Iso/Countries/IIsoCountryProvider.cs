using PineGuard.Iso.Countries;

namespace PineGuard.Externals.Iso.Countries;

/// <summary>
/// Provides validation and lookup services for ISO 3166 country codes.
/// Thread-safe for concurrent reads. All implementations must be immutable.
/// https://www.iso.org/iso-3166-country-codes.html
/// </summary>
public interface IIsoCountryProvider
{
    /// <summary>
    /// Validates a 2-letter ISO 3166-1 alpha-2 country code (case-insensitive).
    /// </summary>
    /// <param name="value">The country code to validate (e.g., "US", "GB", "FR")</param>
    /// <returns>true if the code is valid; otherwise, false</returns>
    bool ContainsAlpha2Code(string? value);

    /// <summary>
    /// Validates a 3-letter ISO 3166-1 alpha-3 country code (case-insensitive).
    /// </summary>
    /// <param name="value">The country code to validate (e.g., "USA", "GBR", "FRA")</param>
    /// <returns>true if the code is valid; otherwise, false</returns>
    bool ContainsAlpha3Code(string? value);

    /// <summary>
    /// Validates a 3-digit ISO 3166-1 numeric country code.
    /// </summary>
    /// <param name="value">The country code to validate (e.g., "840", "826", "250")</param>
    /// <returns>true if the code is valid; otherwise, false</returns>
    bool ContainsNumericCode(string? value);

    /// <summary>
    /// Attempts to retrieve a country by its 2-letter alpha-2 code (case-insensitive).
    /// </summary>
    /// <param name="value">The 2-letter country code</param>
    /// <param name="country">The country if found; otherwise, null</param>
    /// <returns>true if the country was found; otherwise, false</returns>
    bool TryGetByAlpha2Code(string? value, out IsoCountry? country);

    /// <summary>
    /// Attempts to retrieve a country by its 3-letter alpha-3 code (case-insensitive).
    /// </summary>
    /// <param name="value">The 3-letter country code</param>
    /// <param name="country">The country if found; otherwise, null</param>
    /// <returns>true if the country was found; otherwise, false</returns>
    bool TryGetByAlpha3Code(string? value, out IsoCountry? country);

    /// <summary>
    /// Attempts to retrieve a country by its 3-digit numeric code.
    /// </summary>
    /// <param name="value">The 3-digit numeric country code</param>
    /// <param name="country">The country if found; otherwise, null</param>
    /// <returns>true if the country was found; otherwise, false</returns>
    bool TryGetByNumericCode(string? value, out IsoCountry? country);

    /// <summary>
    /// Attempts to retrieve a country by any valid ISO 3166 code format.
    /// Tries alpha-2, alpha-3, then numeric in sequence.
    /// </summary>
    /// <param name="value">The country code in any valid format</param>
    /// <param name="country">The country if found; otherwise, null</param>
    /// <returns>true if the country was found; otherwise, false</returns>
    bool TryGet(string? value, out IsoCountry? country);

    /// <summary>
    /// Gets all countries as a read-only collection.
    /// </summary>
    /// <returns>A read-only collection of all ISO 3166 countries</returns>
    IReadOnlyCollection<IsoCountry> GetAll();
}
