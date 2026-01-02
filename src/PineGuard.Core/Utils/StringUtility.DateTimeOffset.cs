using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class DateTimeOffset
    {
        public static bool TryParse(string? value, out global::System.DateTimeOffset? dateTimeOffset)
        {
            dateTimeOffset = null;

            if (!StringUtility.TryGetTrimmed(value, out var trimmed))
                return false;

            if (global::System.DateTimeOffset.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var parsed))
            {
                dateTimeOffset = parsed;
                return true;
            }

            return false;
        }
    }
}
