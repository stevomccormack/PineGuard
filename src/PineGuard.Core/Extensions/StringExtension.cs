using PineGuard.Utils;

namespace PineGuard.Extensions;

/// <summary>
/// Provides extension methods for <see cref="string"/> values.
/// </summary>
public static class StringExtension
{
    /// <summary>
    /// Determines whether the specified string is in title case (first letter of each word capitalized).
    /// </summary>
    /// <param name="value">The string to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in title case; otherwise, <see langword="false"/>.</returns>
    public static bool TitleCase(this string? value)
        => StringUtility.TitleCase(value);
}
