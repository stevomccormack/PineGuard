using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static class DateTimeOffset
    {
        public static bool IsInPast(string? value)
        {
            if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed))
                return false;

            return DateTimeOffsetRules.IsInPast(parsed);
        }

        public static bool IsInFuture(string? value)
        {
            if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed))
                return false;

            return DateTimeOffsetRules.IsInFuture(parsed);
        }

        public static bool IsBetween(string? value, global::System.DateTimeOffset min, global::System.DateTimeOffset max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        {
            if (!StringUtility.DateTimeOffset.TryParse(value, out var parsed))
                return false;

            return DateTimeOffsetRules.IsBetween(parsed, min, max, inclusion);
        }
    }
}
