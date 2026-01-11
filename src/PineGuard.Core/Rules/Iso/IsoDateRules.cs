using PineGuard.Externals.Iso.Dates;
using PineGuard.Utils.Iso;

namespace PineGuard.Rules.Iso;

public static class IsoDateRules
{
    public static bool IsIsoDateOnly(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoDateOnly.ExactPatternRegex().IsMatch(trimmed))
            return false;

        return IsoDateUtility.TryParseDateOnly(trimmed, out _);
    }

    public static bool IsIsoDateTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoDateTime.AllPatternsRegex().IsMatch(trimmed))
            return false;

        return IsoDateUtility.TryParseDateTime(trimmed, out _);
    }

    public static bool IsIsoDateTimeOffset(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        var trimmed = value.Trim();
        if (!IsoDateTimeOffset.AllPatternsRegex().IsMatch(trimmed))
            return false;

        return IsoDateUtility.TryParseDateTimeOffset(trimmed, out _);
    }
}
