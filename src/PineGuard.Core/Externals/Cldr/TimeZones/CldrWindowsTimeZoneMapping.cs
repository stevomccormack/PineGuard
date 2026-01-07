namespace PineGuard.Externals.Cldr.TimeZones;

/// <summary>
/// Represents a mapping from a Windows time zone ID to one or more IANA tzdb IDs for a given territory.
/// </summary>
public sealed record CldrWindowsTimeZoneMapping(
    string WindowsId,
    string Territory,
    string[] IanaTimeZoneIds);
