namespace PineGuard.Rules;

public static class UriRules
{
    public static bool IsAbsoluteUri(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Uri.TryCreate(value.Trim(), UriKind.Absolute, out _);
    }

    public static bool IsRelativeUri(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Uri.TryCreate(value.Trim(), UriKind.Relative, out _);
    }

    public static bool IsUrl(string? value)
    {
        if (!TryCreateAbsolute(value, out var uri))
            return false;

        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }

    public static bool IsHttpsUrl(string? value)
    {
        if (!TryCreateAbsolute(value, out var uri))
            return false;

        return uri.Scheme == Uri.UriSchemeHttps;
    }

    public static bool IsHttpUrl(string? value)
    {
        if (!TryCreateAbsolute(value, out var uri))
            return false;

        return uri.Scheme == Uri.UriSchemeHttp;
    }

    public static bool IsFileUri(string? value)
    {
        if (!TryCreateAbsolute(value, out var uri))
            return false;

        return uri.IsFile;
    }

    public static bool IsFilePath(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Path.IsPathFullyQualified(value.Trim());
    }

    public static bool HasScheme(string? value, string scheme)
    {
        ArgumentNullException.ThrowIfNull(scheme);

        if (!TryCreateAbsolute(value, out var uri))
            return false;

        return string.Equals(uri.Scheme, scheme, StringComparison.OrdinalIgnoreCase);
    }

    private static bool TryCreateAbsolute(string? value, out Uri uri)
    {
        uri = null!;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return Uri.TryCreate(value.Trim(), UriKind.Absolute, out uri);
    }
}
