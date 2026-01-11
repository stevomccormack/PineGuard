using System.Globalization;
using System.Text.RegularExpressions;

namespace PineGuard.Externals.Iso.Dates;

/// <summary>
/// ISO 8601 date-time formats with offset/UTC (recommended for representing an instant in time).
/// https://www.iso.org/iso-8601-date-and-time-format.html
/// </summary>
public static partial class IsoDateTimeOffset
{
    public const string IsoStandard = "ISO 8601";

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

    public const string BasicPattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}$";
    public const string WithFractionalSecondsPattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\.\\d{1,7}$";
    public const string UtcPattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}Z$";
    public const string UtcWithFractionalSecondsPattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\.\\d{1,7}Z$";
    public const string WithTimezonePattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}[+\\-]\\d{2}:\\d{2}$";
    public const string WithTimezoneAndFractionalSecondsPattern = "^\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}\\.\\d{1,7}[+\\-]\\d{2}:\\d{2}$";
    public const string AllPatterns = "^(?:" +
        "\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}" +
        "(?:\\.\\d{1,7})?" +
        "(?:Z|[+\\-]\\d{2}:\\d{2})?" +
        ")$";

    [GeneratedRegex(AllPatterns, RegexOptions.CultureInvariant)]
    public static partial Regex AllPatternsRegex();

    public static bool TryParse(string? value, out DateTimeOffset result)
        => DateTimeOffset.TryParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out result);

    public static DateTimeOffset Parse(string value)
        => DateTimeOffset.ParseExact(
            value,
            AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind);

    public static string ToIsoString(this DateTimeOffset value)
        => value.ToString("O", CultureInfo.InvariantCulture);
}
