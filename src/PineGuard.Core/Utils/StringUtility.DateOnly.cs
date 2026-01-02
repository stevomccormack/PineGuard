using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class DateOnly
    {
        public static bool TryParse(string? value, out global::System.DateOnly? date)
        {
            date = null;

            if (!StringUtility.TryGetTrimmed(value, out var trimmed))
                return false;

            if (global::System.DateOnly.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                date = parsed;
                return true;
            }

            return false;
        }
    }
}
