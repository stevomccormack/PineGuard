using PineGuard.Iso.Dates;
using System.Globalization;

namespace PineGuard.Rules.Iso;

public static class IsoDateRules
{
    public static bool IsIsoDateOnly(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        value = value.Trim();

        return DateOnly.TryParseExact(
            value,
            IsoDateOnly.ExactFormat,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _);
    }

    public static bool IsIsoDateTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        value = value.Trim();

        return DateTimeOffset.TryParseExact(
            value,
            IsoDateTimeOffset.AllFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.RoundtripKind,
            out _);
    }
}
