using PineGuard.Rules.Iso;

namespace PineGuard.Rules;

public static partial class IsoRules
{
    public static class Dates
    {
        public static bool IsDateOnly(string? value) =>
            IsoDateRules.IsIsoDateOnly(value);

        public static bool IsDateTime(string? value) =>
            IsoDateRules.IsIsoDateTime(value);

        public static bool IsDateTimeOffset(string? value) =>
            IsoDateRules.IsIsoDateTimeOffset(value);
    }
}
