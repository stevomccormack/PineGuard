using PineGuard.Iso.Dates;
using System.Globalization;

namespace PineGuard.Utils.Iso;

public static class IsoDateUtility
{
    public static bool TryParseDateOnly(string? value, out DateOnly date)
    {
        date = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();

        return DateOnly.TryParseExact(
            trimmed,
            IsoDateOnly.ExactFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out date);
    }

    public static bool TryParseDateTime(string? value, out DateTime dateTime)
    {
        dateTime = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();

        return DateTime.TryParseExact(
            trimmed,
            IsoDateTime.AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out dateTime);
    }

    public static bool TryParseDateTimeOffset(string? value, out DateTimeOffset dateTimeOffset)
    {
        dateTimeOffset = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();

        return DateTimeOffset.TryParseExact(
            trimmed,
            IsoDateTimeOffset.AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out dateTimeOffset);
    }
}
