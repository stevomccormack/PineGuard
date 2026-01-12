namespace PineGuard.Utils;

public static partial class StringUtility
{
    public static class Guid
    {
        public static bool TryParse(string? value, out System.Guid? guid)
        {
            guid = null;

            if (!TryGetTrimmed(value, out var trimmed))
                return false;

            if (!System.Guid.TryParse(trimmed, out var parsed)) 
                return false;

            guid = parsed;
            return true;

        }
    }
}
