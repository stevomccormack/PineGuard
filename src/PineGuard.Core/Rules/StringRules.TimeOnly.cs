using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static class TimeOnly
    {
        public static bool IsBetween(string? value, global::System.TimeOnly min, global::System.TimeOnly max, RangeInclusion inclusion = RangeInclusion.Inclusive)
        {
            if (!StringUtility.TimeOnly.TryParse(value, out var parsed))
                return false;

            return TimeOnlyRules.IsBetween(parsed, min, max, inclusion);
        }

        public static bool IsChronological(string? start, string? end, RangeInclusion inclusion = RangeInclusion.Exclusive)
        {
            if (!StringUtility.TimeOnly.TryParse(start, out var parsedStart))
                return false;

            if (!StringUtility.TimeOnly.TryParse(end, out var parsedEnd))
                return false;

            return TimeOnlyRules.IsChronological(parsedStart, parsedEnd, inclusion);
        }

        public static bool IsOverlapping(
            string? start1,
            string? end1,
            string? start2,
            string? end2,
            RangeInclusion inclusion = RangeInclusion.Exclusive)
        {
            if (!StringUtility.TimeOnly.TryParse(start1, out var s1))
                return false;

            if (!StringUtility.TimeOnly.TryParse(end1, out var e1))
                return false;

            if (!StringUtility.TimeOnly.TryParse(start2, out var s2))
                return false;

            if (!StringUtility.TimeOnly.TryParse(end2, out var e2))
                return false;

            return TimeOnlyRules.IsOverlapping(s1, e1, s2, e2, inclusion);
        }
    }
}
