namespace PineGuard.Iana.TimeZones;

/// <summary>
/// Provides validation and lookup services for IANA tzdb time zones.
/// Thread-safe for concurrent reads. All implementations must be immutable.
/// </summary>
public interface IIanaTimeZoneProvider
{
    /// <summary>
    /// Validates an IANA tzdb identifier (case-sensitive).
    /// </summary>
    bool IsValidTimeZoneId(string? value);

    /// <summary>
    /// Attempts to retrieve a timezone by its tzdb identifier (case-sensitive).
    /// </summary>
    bool TryGetById(string? value, out IanaTimeZone? timeZone);

    /// <summary>
    /// Returns all known time zones.
    /// </summary>
    IReadOnlyCollection<IanaTimeZone> GetAll();

    /// <summary>
    /// Attempts to retrieve time zone IDs for a given ISO 3166-1 alpha-2 country code (case-insensitive).
    /// </summary>
    bool TryGetTimeZoneIdsByCountryAlpha2Code(string? isoCountryAlpha2Code, out IReadOnlyCollection<string> timeZoneIds);
}
