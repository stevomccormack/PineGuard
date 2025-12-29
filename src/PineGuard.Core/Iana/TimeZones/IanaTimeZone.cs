using PineGuard.Rules;
using PineGuard.Cldr.TimeZones;

namespace PineGuard.Iana.TimeZones;

/// <summary>
/// Represents an IANA tzdb time zone ID and its associated metadata.
/// https://www.iana.org/time-zones
/// </summary>
public sealed record IanaTimeZone
{
    /// <summary>
    /// The canonical tzdb identifier (e.g., "Europe/London").
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// One or more ISO 3166-1 alpha-2 country codes that overlap this timezone.
    /// </summary>
    public IReadOnlyList<string> CountryAlpha2Codes { get; }

    /// <summary>
    /// Latitude/longitude in ISO 6709 sign-degrees-minutes-seconds format.
    /// </summary>
    public string Coordinates { get; }

    /// <summary>
    /// Optional comment from tzdb zone1970.tab.
    /// </summary>
    public string? Comment { get; }

    public IanaTimeZone(string id, IReadOnlyList<string> countryAlpha2Codes, string coordinates, string? comment)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(countryAlpha2Codes);
        ArgumentNullException.ThrowIfNull(coordinates);

        var trimmedId = id.Trim();
        if (string.IsNullOrWhiteSpace(trimmedId))
            throw new ArgumentException("Id cannot be null or whitespace.", nameof(id));

        var trimmedCoordinates = coordinates.Trim();
        if (string.IsNullOrWhiteSpace(trimmedCoordinates))
            throw new ArgumentException("Coordinates cannot be null or whitespace.", nameof(coordinates));

        if (countryAlpha2Codes.Count < 1)
            throw new ArgumentException("At least one country alpha-2 code is required.", nameof(countryAlpha2Codes));

        var normalizedCountries = new string[countryAlpha2Codes.Count];
        for (var i = 0; i < countryAlpha2Codes.Count; i++)
        {
            var code = countryAlpha2Codes[i];
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Country alpha-2 codes cannot contain null/whitespace values.", nameof(countryAlpha2Codes));

            var trimmed = code.Trim();
            if (trimmed.Length != 2 || !StringRules.IsAlphabetic(trimmed))
                throw new ArgumentException("Country alpha-2 codes must be exactly 2 alphabetic characters.", nameof(countryAlpha2Codes));

            normalizedCountries[i] = trimmed.ToUpperInvariant();
        }

        Id = trimmedId;
        Coordinates = trimmedCoordinates;
        Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();
        CountryAlpha2Codes = normalizedCountries;
    }

    public override string ToString() => Id;
}

public static class IanaTimeZoneExtensions
{
    /// <summary>
    /// Maps an <see cref="IanaTimeZone"/> to a Windows time zone ID using the embedded CLDR windowsZones mapping.
    /// Returns null when no mapping is found.
    /// </summary>
    public static string? ToWindowsTimeZone(
        this IanaTimeZone? timeZone,
        string? territory = null,
        ICldrWindowsTimeZoneProvider? cldrProvider = null)
    {
        if (timeZone is null)
            return null;

        cldrProvider ??= DefaultCldrWindowsTimeZoneProvider.Instance;
        return cldrProvider.TryGetWindowsTimeZoneId(timeZone.Id, territory, out var windowsId)
            ? windowsId
            : null;
    }
}
