using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static class Guid
    {
        public static bool IsEmpty(string? value)
        {
            if (!StringUtility.Guid.TryParse(value, out var parsed))
                return false;

            return GuidRules.IsEmpty(parsed);
        }

        public static bool IsNullOrEmpty(string? value)
        {
            if (value is null)
                return GuidRules.IsNullOrEmpty(null);

            if (!StringUtility.Guid.TryParse(value, out var parsed))
                return false;

            return GuidRules.IsNullOrEmpty(parsed);
        }
    }
}
