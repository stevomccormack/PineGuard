using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    public static class Bool
    {
        public static bool IsTrue(string? value)
        {
            if (!StringUtility.Bool.TryParse(value, out var parsed))
                return false;

            return BoolRules.IsTrue(parsed);
        }

        public static bool IsFalse(string? value)
        {
            if (!StringUtility.Bool.TryParse(value, out var parsed))
                return false;

            return BoolRules.IsFalse(parsed);
        }

        public static bool IsNullOrTrue(string? value)
        {
            if (value is null)
                return BoolRules.IsNullOrTrue(null);

            if (!StringUtility.Bool.TryParse(value, out var parsed))
                return false;

            return BoolRules.IsNullOrTrue(parsed);
        }

        public static bool IsNullOrFalse(string? value)
        {
            if (value is null)
                return BoolRules.IsNullOrFalse(null);

            if (!StringUtility.Bool.TryParse(value, out var parsed))
                return false;

            return BoolRules.IsNullOrFalse(parsed);
        }
    }
}
