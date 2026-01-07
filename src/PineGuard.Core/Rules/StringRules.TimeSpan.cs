using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static class TimeSpan
    {
        public static bool IsDurationBetween(string? value, global::System.TimeSpan min, global::System.TimeSpan max, Inclusion inclusion = Inclusion.Inclusive)
        {
            if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
                return false;

            return TimeSpanRules.IsDurationBetween(parsed, min, max, inclusion);
        }

        public static bool IsGreaterThan(string? value, global::System.TimeSpan threshold, Inclusion inclusion = Inclusion.Exclusive)
        {
            if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
                return false;

            return TimeSpanRules.IsGreaterThan(parsed, threshold, inclusion);
        }

        public static bool IsLessThan(string? value, global::System.TimeSpan threshold, Inclusion inclusion = Inclusion.Exclusive)
        {
            if (!StringUtility.TimeSpan.TryParse(value, out var parsed))
                return false;

            return TimeSpanRules.IsLessThan(parsed, threshold, inclusion);
        }
    }
}
