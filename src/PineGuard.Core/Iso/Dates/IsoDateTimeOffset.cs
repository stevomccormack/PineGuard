using System.Globalization;

namespace PineGuard.Iso.Dates;

/// <summary>
/// ISO 8601 date-time formats with offset/UTC (recommended for representing an instant in time).
/// https://www.iso.org/iso-8601-date-and-time-format.html
/// </summary>
public static class IsoDateTimeOffset
{
    public const string BasicFormat = "yyyy-MM-ddTHH:mm:ss";
    public const string WithFractionalSecondsFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFF";
    public const string UtcFormat = "yyyy-MM-ddTHH:mm:ssZ";
    public const string UtcWithFractionalSecondsFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFZ";
    public const string WithTimezoneFormat = "yyyy-MM-ddTHH:mm:sszzz";
    public const string WithTimezoneAndFractionalSecondsFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFFzzz";

    public static readonly string[] AllFormats =
    [
        BasicFormat,
        WithFractionalSecondsFormat,
        UtcFormat,
        UtcWithFractionalSecondsFormat,
        WithTimezoneFormat,
        WithTimezoneAndFractionalSecondsFormat
    ];

    public static bool TryParseIsoDateTime(string? value, out DateTimeOffset result)
        => DateTimeOffset.TryParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out result);

    public static DateTimeOffset ParseIsoDateTime(string value)
        => DateTimeOffset.ParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind);

    public static string ToIsoString(this DateTimeOffset value)
        => value.ToString("O", CultureInfo.InvariantCulture);
}
