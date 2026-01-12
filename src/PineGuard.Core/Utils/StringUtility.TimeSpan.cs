using System.Globalization;

namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class TimeSpan
    {
        public static bool TryParse(string? value, out global::System.TimeSpan? timeSpan)
        {
            timeSpan = null;

            if (!TryGetTrimmed(value, out var trimmed))
                return false;

            if (!global::System.TimeSpan.TryParse(trimmed, CultureInfo.InvariantCulture, out var parsed)) 
                return false;
            
            timeSpan = parsed;
            return true;

        }
    }
}
