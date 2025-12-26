using System.Globalization;

namespace PineGuard.Iso.Dates;

/// <summary>
/// ISO 8601 local date-time formats (no offset).
/// Use <see cref="IsoDateTimeOffset"/> for offset-aware ISO 8601 parsing/formatting.
/// https://www.iso.org/iso-8601-date-and-time-format.html
/// </summary>
public static class IsoDateTime
{
    public const string BasicFormat = "yyyy-MM-ddTHH:mm:ss";
    public const string WithFractionalSecondsFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFF";

    public static readonly string[] AllFormats =
    [
        BasicFormat,
        WithFractionalSecondsFormat
    ];

    public static string ToIsoString(this DateTime value)
        => value.ToString("O", CultureInfo.InvariantCulture);

    public static bool TryParseIsoDateTime(string? value, out DateTime result)
        => DateTime.TryParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out result);

    public static DateTime ParseIsoDateTime(string value)
        => DateTime.ParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None);
}
