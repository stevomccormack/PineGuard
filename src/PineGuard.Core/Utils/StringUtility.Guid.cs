namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class Guid
    {
        public static bool TryParse(string? value, out System.Guid? guid)
        {
            guid = null;

            if (!StringUtility.TryGetTrimmed(value, out var trimmed))
                return false;

            if (System.Guid.TryParse(trimmed, out var parsed))
            {
                guid = parsed;
                return true;
            }

            return false;
        }
    }
}
