namespace PineGuard.Externals.Cldr.TimeZones;

/// <summary>
/// Provides lookup services for CLDR Windows time zone mappings.
/// Thread-safe for concurrent reads. All implementations must be immutable.
/// </summary>
public interface ICldrWindowsTimeZoneProvider
{
    /// <summary>
    /// Returns true if a Windows time zone ID is known.
    /// </summary>
    bool IsValidWindowsTimeZoneId(string? windowsTimeZoneId);

    /// <summary>
    /// Attempts to retrieve IANA time zone IDs for a given Windows time zone ID.
    /// If territory is null/empty, "001" (world) is used.
    /// </summary>
    bool TryGetIanaTimeZoneIds(string? windowsTimeZoneId, string? territory, out IReadOnlyCollection<string> ianaTimeZoneIds);

    /// <summary>
    /// Attempts to retrieve a Windows time zone ID for a given IANA time zone ID.
    /// If territory is null/empty, "001" (world) is used.
    /// </summary>
    bool TryGetWindowsTimeZoneId(string? ianaTimeZoneId, string? territory, out string windowsTimeZoneId);
}
