using System.Globalization;

namespace PineGuard.Iso.Dates;

/// <summary>
/// ISO 8601 date format constants and utilities.
/// https://www.iso.org/iso-8601-date-and-time-format.html
/// </summary>
public static class IsoDateOnly
{
    public const string ExactFormat = "yyyy-MM-dd";

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
            CultureInfo.InvariantCulture,
            DateTimeStyles.None);

    public static string ToIsoString(this DateOnly value) =>
        value.ToString(ExactFormat, CultureInfo.InvariantCulture);
}
