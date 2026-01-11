namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class Bool
    {
        public static bool TryParse(string? value, out bool? result)
        {
            result = null;

            if (value is null)
                return true;

            if (!TryGetTrimmed(value, out var trimmed))
                return false;

            if (bool.TryParse(trimmed, out var parsed))
            {
                result = parsed;
                return true;
            }

            return false;
        }
    }
}
