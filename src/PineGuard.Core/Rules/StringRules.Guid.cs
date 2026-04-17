using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Provides GUID string parsing and validation predicates.
    /// </summary>
    /// <seealso href="https://pineguard.ai/docs/rules/string/guid">String GUID Rules documentation</seealso>
    public static class Guid
    {
        /// <summary>
        /// Determines whether the specified string is a valid GUID.
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid GUID string, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> parses to a valid <see cref="System.Guid"/>; otherwise, <see langword="false"/>.</returns>
        /// <example>
        /// <code>
        /// bool valid = StringRules.Guid.IsGuid("d3b07384-d113-4ec7-8479-2eaf2dc7c5a2"); // true
        /// bool invalid = StringRules.Guid.IsGuid("not-a-guid");                           // false
        /// </code>
        /// </example>
        public static bool IsGuid(string? value)
            => StringUtility.Guid.TryParse(value, out System.Guid _);

        /// <summary>
        /// Determines whether the specified string parses to a non-empty GUID (not <see cref="System.Guid.Empty"/>).
        /// </summary>
        /// <param name="value">The value to validate. If <see langword="null"/> or not a valid GUID string, returns <see langword="false"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> parses to a GUID that is not <see cref="System.Guid.Empty"/>; otherwise, <see langword="false"/>.</returns>
        public static bool IsNotEmpty(string? value) =>
            StringUtility.Guid.TryParse(value, out System.Guid? parsed) && GuidRules.IsNotEmpty(parsed);
    }
}
