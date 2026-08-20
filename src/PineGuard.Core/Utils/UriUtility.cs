namespace PineGuard.Utils;

/// <summary>
/// Provides URI parsing and validation utility methods.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/utils/uri">URI Utility documentation</seealso>
public static class UriUtility
{
    /// <summary>
    /// Attempts to parse the specified string as an absolute URI.
    /// </summary>
    /// <param name="value">The URI string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="uri">When this method returns, contains the parsed URI if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the URI was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseAbsolute(string? value, out Uri? uri)
    {
        uri = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (!Uri.TryCreate(trimmed, UriKind.Absolute, out uri))
            return false;

        // On Linux, bare paths such as "/foo/bar" are parsed as absolute URIs by Uri.TryCreate.
        // Require the original string to explicitly start with the scheme to ensure cross-platform consistency.
        return trimmed.StartsWith(uri.Scheme + ":", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Attempts to parse the specified string as a relative URI.
    /// </summary>
    /// <param name="value">The URI string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="uri">When this method returns, contains the parsed URI if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the URI was parsed successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseRelative(string? value, out Uri? uri)
    {
        uri = null;

        return StringUtility.TryGetTrimmed(value, out var trimmed) && Uri.TryCreate(trimmed, UriKind.Relative, out uri);
    }

    /// <summary>
    /// Attempts to parse the specified string as an HTTP or HTTPS URL.
    /// </summary>
    /// <param name="value">The URL string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="uri">When this method returns, contains the parsed URI if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the URL was parsed and uses HTTP or HTTPS; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseUrl(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return IsHttpOrHttps(uri);
    }

    /// <summary>
    /// Attempts to parse the specified string as an HTTPS URL.
    /// </summary>
    /// <param name="value">The URL string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="uri">When this method returns, contains the parsed URI if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the URL was parsed and uses HTTPS; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseHttpsUrl(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return IsHttps(uri);
    }

    /// <summary>
    /// Attempts to parse the specified string as an HTTP URL.
    /// </summary>
    /// <param name="value">The URL string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="uri">When this method returns, contains the parsed URI if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the URL was parsed and uses HTTP; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseHttpUrl(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return IsHttp(uri);
    }

    /// <summary>
    /// Attempts to parse the specified string as a file URI.
    /// </summary>
    /// <param name="value">The URI string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="uri">When this method returns, contains the parsed URI if successful; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the URI was parsed and is a file URI; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseFileUri(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return uri.IsFile;
    }

    /// <summary>
    /// Attempts to validate the specified string as a fully qualified file path.
    /// </summary>
    /// <param name="value">The path to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="path">When this method returns, contains the trimmed path if successful; otherwise, <see cref="string.Empty"/>.</param>
    /// <returns><see langword="true"/> if the path is fully qualified; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseFilePath(string? value, out string path)
    {
        path = string.Empty;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        // Path.IsPathFullyQualified is platform-specific and returns false for Windows-style
        // paths (e.g. "C:\..." or "\\server\share") when running on Linux/macOS.
        // Check both the platform-native result and explicit Windows path patterns.
        var isWindowsAbsolute = IsWindowsAbsolutePath(trimmed);
        if (!Path.IsPathFullyQualified(trimmed) && !isWindowsAbsolute)
            return false;

        path = trimmed;
        return true;
    }

    private static bool IsWindowsAbsolutePath(string value) =>
        (value.Length >= 3 && char.IsLetter(value[0]) && value[1] == ':' && (value[2] == '\\' || value[2] == '/')) ||
        value.StartsWith(@"\\", StringComparison.Ordinal);

    /// <summary>
    /// Attempts to extract the URI scheme from the specified string.
    /// </summary>
    /// <param name="value">The URI string to parse. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="scheme">When this method returns, contains the scheme if successful; otherwise, <see cref="string.Empty"/>.</param>
    /// <returns><see langword="true"/> if the scheme was extracted; otherwise, <see langword="false"/>.</returns>
    public static bool TryParseScheme(string? value, out string scheme)
    {
        scheme = string.Empty;

        if (!TryParseAbsolute(value, out var uri) || uri is null)
            return false;

        scheme = uri.Scheme;
        return scheme.Length != 0;
    }

    private static bool IsHttp(Uri uri) =>
        uri.Scheme == Uri.UriSchemeHttp;

    private static bool IsHttps(Uri uri) =>
        uri.Scheme == Uri.UriSchemeHttps;

    private static bool IsHttpOrHttps(Uri uri) =>
        IsHttp(uri) || IsHttps(uri);
}
