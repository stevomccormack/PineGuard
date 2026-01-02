using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static class DateOnly
    {
        public static bool IsInPast(string? value)
        {
            if (!StringUtility.DateOnly.TryParse(value, out var parsed))
                return false;

            return DateOnlyRules.IsInPast(parsed);
        }

        public static bool IsInFuture(string? value)
        {
            if (!StringUtility.DateOnly.TryParse(value, out var parsed))
                return false;

            return DateOnlyRules.IsInFuture(parsed);
        }

        public static bool IsBetween(string? value, global::System.DateOnly min, global::System.DateOnly max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        {
            if (!StringUtility.DateOnly.TryParse(value, out var parsed))
                return false;

            return DateOnlyRules.IsBetween(parsed, min, max, inclusion);
        }
    }
}
