using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class TimeOnly
    {
        public static bool TryParse(string? value, out global::System.TimeOnly? time)
        {
            time = null;

            if (!TryGetTrimmed(value, out var trimmed))
                return false;

            if (global::System.TimeOnly.TryParse(trimmed, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
            {
                time = parsed;
                return true;
            }

            return false;
        }
    }
}
