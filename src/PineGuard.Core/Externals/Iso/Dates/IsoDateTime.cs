using System.Globalization;
using System.Text.RegularExpressions;

namespace PineGuard.Iso.Dates;

/// <summary>
/// ISO 8601 local date-time formats (no offset).
/// Use <see cref="IsoDateTimeOffset"/> for offset-aware ISO 8601 parsing/formatting.
/// https://www.iso.org/iso-8601-date-and-time-format.html
/// </summary>
public static partial class IsoDateTime
{
    public const string IsoStandard = "ISO 8601";

    public const string BasicFormat = "yyyy-MM-ddTHH:mm:ss";
    public const string WithFractionalSecondsFormat = "yyyy-MM-ddTHH:mm:ss.FFFFFFF";
    public static readonly string[] AllFormats =
    [
        BasicFormat,
        WithFractionalSecondsFormat
    ];

    public const string BasicPattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}$";
    public const string WithFractionalSecondsPattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\.\\d{1,7}$";
    public const string AllPatterns = "^(?:" +
        "\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}" +
        "(?:\\.\\d{1,7})?" +
        ")$";

    [GeneratedRegex(AllPatterns, RegexOptions.CultureInvariant)]
    public static partial Regex AllPatternsRegex();

    public static bool TryParse(string? value, out DateTime result)
        => DateTime.TryParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out result);

    public static DateTime Parse(string value)
        => DateTime.ParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None);

    public static string ToIsoString(this DateTime value)
        => value.ToString("O", CultureInfo.InvariantCulture);
}
