using PineGuard.Iso.Dates;
using PineGuard.Utils.Iso;

namespace PineGuard.Rules.Iso;

public static class IsoDateRules
{
    public static bool IsIsoDateOnly(string? value) =>
        IsoDateUtility.TryParseDateOnly(value, out _);

    public static bool IsIsoDateTime(string? value) =>
        IsoDateUtility.TryParseDateTime(value, out _);

    public static bool IsIsoDateTimeOffset(string? value) =>
        IsoDateUtility.TryParseDateTimeOffset(value, out _);
}
