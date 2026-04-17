using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure URI and URL validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/uri">URI Rules documentation</seealso>
public static class UriRules
{
    /// <summary>
    /// Determines whether the specified value is a valid absolute URI.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid absolute URI; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool valid = UriRules.IsAbsoluteUri("https://example.com/path"); // true
    /// bool invalid = UriRules.IsAbsoluteUri("/relative/path");         // false
    /// </code>
    /// </example>
    public static bool IsAbsoluteUri(string? value) =>
        UriUtility.TryParseAbsolute(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid relative URI.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid relative URI; otherwise, <see langword="false"/>.</returns>
    public static bool IsRelativeUri(string? value) =>
        UriUtility.TryParseRelative(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid URL (HTTP or HTTPS).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid HTTP or HTTPS URL; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool valid = UriRules.IsUrl("https://example.com"); // true
    /// bool valid = UriRules.IsUrl("http://example.com");  // true
    /// </code>
    /// </example>
    public static bool IsUrl(string? value) =>
        UriUtility.TryParseUrl(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid HTTPS URL.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid HTTPS URL; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpsUrl(string? value) =>
        UriUtility.TryParseHttpsUrl(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid HTTP URL.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid HTTP URL; otherwise, <see langword="false"/>.</returns>
    public static bool IsHttpUrl(string? value) =>
        UriUtility.TryParseHttpUrl(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid <c>file://</c> URI.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid file URI; otherwise, <see langword="false"/>.</returns>
    public static bool IsFileUri(string? value) =>
        UriUtility.TryParseFileUri(value, out _);

    /// <summary>
    /// Determines whether the specified value is a valid file system path.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a valid file path; otherwise, <see langword="false"/>.</returns>
    public static bool IsFilePath(string? value) =>
        UriUtility.TryParseFilePath(value, out _);

    /// <summary>
    /// Determines whether the specified URI has the given scheme.
    /// </summary>
    /// <param name="value">The URI string to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="scheme">The URI scheme to check (e.g., <c>"https"</c>, <c>"ftp"</c>). Case-insensitive.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid URI with the specified <paramref name="scheme"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="scheme"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// bool ftpScheme = UriRules.HasScheme("ftp://files.example.com", "ftp"); // true
    /// </code>
    /// </example>
    public static bool HasScheme(string? value, string scheme)
    {
        ThrowHelper.ThrowIfNull(scheme);

        return UriUtility.TryParseScheme(value, out var parsedScheme) && string.Equals(parsedScheme, scheme, StringComparison.OrdinalIgnoreCase);
    }
}
