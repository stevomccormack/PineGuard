using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides boolean string parsing and validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/bool">String Bool Rules documentation</seealso>
    public static class Bool
    {
        /// <summary>
        /// Determines whether the specified string parses to <see langword="true"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid boolean string, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> parses to the boolean value <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsTrue(string? value) =>
            StringUtility.Bool.TryParse(value, out var parsed) && BoolRules.IsTrue(parsed);

        /// <summary>
        /// Determines whether the specified string parses to <see langword="false"/>.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid boolean string, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> parses to the boolean value <see langword="false"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsFalse(string? value) =>
            StringUtility.Bool.TryParse(value, out var parsed) && BoolRules.IsFalse(parsed);
    }
}
