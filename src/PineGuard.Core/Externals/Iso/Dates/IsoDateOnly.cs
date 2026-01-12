using System.Globalization;
using System.Text.RegularExpressions;

namespace PineGuard.Externals.Iso.Dates;

/// <summary>
/// ISO 8601 date format constants and utilities.
/// https://www.iso.org/iso-8601-date-and-time-format.html
/// </summary>
public static partial class IsoDateOnly
{
    public const string IsoStandard = "ISO 8601";

    public const string ExactFormat = "yyyy-MM-dd";
    public const string ExactPattern = @"^\d{4}-\d{2}-\d{2}$";

    [GeneratedRegex(ExactPattern, RegexOptions.CultureInvariant)]
    public static partial Regex ExactPatternRegex();

    public static bool TryParse(string? value, out DateOnly result) =>
        DateOnly.TryParseExact(
            value,
            ExactFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out result);

    public static DateOnly Parse(string value) =>
        DateOnly.ParseExact(
            value,
            ExactFormat,
            CultureInfo.InvariantCulture);

    public static string ToIsoString(this DateOnly value) =>
        value.ToString(ExactFormat, CultureInfo.InvariantCulture);
}
