using System.IO;

namespace PineGuard.Utils;

public static class UriUtility
{
    public static bool TryParseAbsolute(string? value, out Uri? uri)
    {
        uri = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return Uri.TryCreate(trimmed, UriKind.Absolute, out uri);
    }

    public static bool TryParseRelative(string? value, out Uri? uri)
    {
        uri = null;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        return Uri.TryCreate(trimmed, UriKind.Relative, out uri);
    }

    public static bool TryParseUrl(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }

    public static bool TryParseHttpsUrl(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return uri.Scheme == Uri.UriSchemeHttps;
    }

    public static bool TryParseHttpUrl(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return uri.Scheme == Uri.UriSchemeHttp;
    }

    public static bool TryParseFileUri(string? value, out Uri? uri)
    {
        if (!TryParseAbsolute(value, out uri) || uri is null)
            return false;

        return uri.IsFile;
    }

    public static bool TryParseFilePath(string? value, out string path)
    {
        path = string.Empty;

        if (!StringUtility.TryGetTrimmed(value, out var trimmed))
            return false;

        if (!Path.IsPathFullyQualified(trimmed))
            return false;

        path = trimmed;
        return true;
    }

    public static bool TryParseScheme(string? value, out string scheme)
    {
        scheme = string.Empty;

        if (!TryParseAbsolute(value, out var uri) || uri is null)
            return false;

        scheme = uri.Scheme;
        return scheme.Length != 0;
    }
}
