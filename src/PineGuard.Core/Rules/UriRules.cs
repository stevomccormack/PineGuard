using PineGuard.Utils;

namespace PineGuard.Rules;

public static class UriRules
{
    public static bool IsAbsoluteUri(string? value) =>
        UriUtility.TryParseAbsolute(value, out _);

    public static bool IsRelativeUri(string? value) =>
        UriUtility.TryParseRelative(value, out _);

    public static bool IsUrl(string? value) =>
        UriUtility.TryParseUrl(value, out _);

    public static bool IsHttpsUrl(string? value) =>
        UriUtility.TryParseHttpsUrl(value, out _);

    public static bool IsHttpUrl(string? value) =>
        UriUtility.TryParseHttpUrl(value, out _);

    public static bool IsFileUri(string? value) =>
        UriUtility.TryParseFileUri(value, out _);

    public static bool IsFilePath(string? value) =>
        UriUtility.TryParseFilePath(value, out _);

    public static bool HasScheme(string? value, string scheme)
    {
        ArgumentNullException.ThrowIfNull(scheme);

        if (!UriUtility.TryParseScheme(value, out var parsedScheme))
            return false;

        return string.Equals(parsedScheme, scheme, StringComparison.OrdinalIgnoreCase);
    }
}
