using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;
using PineGuard.Utils;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate URI and URL strings.
/// </summary>
/// <seealso cref="UriRules"/>
/// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
public static class MustUriClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified string is a valid absolute URI and returns the parsed <see cref="Uri"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as an absolute URI.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid absolute URI, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="Uri"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="UriUtility.TryParseAbsolute"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid absolute URI."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.AbsoluteUri(requestUri);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<Uri> AbsoluteUri(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<Uri>.Fail(MustCodes.Uri.Form.NotAbsolute, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid absolute URI.";

        var ok = UriUtility.TryParseAbsolute(value, out var uri) && uri is not null;
        return MustResult<Uri>.FromBool(ok, MustCodes.Uri.Form.NotAbsolute, messageTemplate, paramName, value, result: uri!);
    }

    /// <summary>
    /// Validates that the specified string is a valid relative URI and returns the parsed <see cref="Uri"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a relative URI.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid relative URI, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="Uri"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="UriUtility.TryParseRelative"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid relative URI."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.RelativeUri(path);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<Uri> RelativeUri(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<Uri>.Fail(MustCodes.Uri.Form.NotRelative, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid relative URI.";

        var ok = UriUtility.TryParseRelative(value, out var uri) && uri is not null;
        return MustResult<Uri>.FromBool(ok, MustCodes.Uri.Form.NotRelative, messageTemplate, paramName, value, result: uri!);
    }

    /// <summary>
    /// Validates that the specified string is a valid HTTP or HTTPS URL and returns the parsed <see cref="Uri"/>.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a URL.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid URL, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="Uri"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="UriUtility.TryParseUrl"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid URL."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Url(callbackUrl);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<Uri> Url(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<Uri>.Fail(MustCodes.Uri.Form.NotUrl, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid URL.";

        var ok = UriUtility.TryParseUrl(value, out var uri) && uri is not null;
        return MustResult<Uri>.FromBool(ok, MustCodes.Uri.Form.NotUrl, messageTemplate, paramName, value, result: uri!);
    }

    /// <summary>
    /// Validates that the specified string is a valid HTTPS URL.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as an HTTPS URL.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid HTTPS URL, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="Uri"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/> or is not a valid URL.
    /// Delegates to <see cref="Url"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid HTTPS URL."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HttpsUrl(apiEndpoint);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<Uri> HttpsUrl(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<Uri>.Fail(MustCodes.Uri.Scheme.NotHttps, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid HTTPS URL.";

        var url = _.Url(value, paramName);
        if (url.Failed)
            return url;

        var uri = url.Result!;
        var ok = uri.Scheme == Uri.UriSchemeHttps;
        return MustResult<Uri>.FromBool(ok, MustCodes.Uri.Scheme.NotHttps, messageTemplate, paramName, value, result: uri);
    }

    /// <summary>
    /// Validates that the specified string is a valid HTTP URL.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as an HTTP URL.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid HTTP URL, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="Uri"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/> or is not a valid URL.
    /// Delegates to <see cref="Url"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid HTTP URL."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HttpUrl(insecureEndpoint);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<Uri> HttpUrl(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<Uri>.Fail(MustCodes.Uri.Scheme.NotHttp, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid HTTP URL.";

        var url = _.Url(value, paramName);
        if (url.Failed)
            return url;

        var uri = url.Result!;
        var ok = uri.Scheme == Uri.UriSchemeHttp;
        return MustResult<Uri>.FromBool(ok, MustCodes.Uri.Scheme.NotHttp, messageTemplate, paramName, value, result: uri);
    }

    /// <summary>
    /// Validates that the specified string is a valid <c>file://</c> URI.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a file URI.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid file URI, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>. On success, <see cref="MustResult{T}.Result"/> contains the
    /// parsed <see cref="Uri"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/> or is not a valid absolute URI.
    /// Delegates to <see cref="AbsoluteUri"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid file URI."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.FileUri(resourcePath);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<Uri> FileUri(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<Uri>.Fail(MustCodes.Uri.Scheme.NotFile, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid file URI.";

        var absoluteUri = _.AbsoluteUri(value, paramName);
        if (absoluteUri.Failed)
            return absoluteUri;

        var uri = absoluteUri.Result!;
        var ok = uri.IsFile;
        return MustResult<Uri>.FromBool(ok, MustCodes.Uri.Scheme.NotFile, messageTemplate, paramName, value, result: uri);
    }

    /// <summary>
    /// Validates that the specified string is a valid local file system path.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a file path.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid file path, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="UriRules.IsFilePath"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid file path."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.FilePath(outputPath);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules.IsFilePath"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<string> FilePath(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Uri.FilePath.Invalid, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid file path.";

        var ok = UriRules.IsFilePath(value);
        return MustResult<string>.FromBool(ok, MustCodes.Uri.FilePath.Invalid, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified URI string uses the given scheme.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The URI string to validate.</param>
    /// <param name="scheme">The expected URI scheme (e.g., <c>"https"</c>, <c>"ftp"</c>). Must not be <see langword="null"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> uses <paramref name="scheme"/>, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="scheme"/> is <see langword="null"/>.
    /// Delegates to <see cref="UriRules.HasScheme"/>. The failure message follows the pattern
    /// <c>"{paramName} must have the expected scheme."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HasScheme(resourceUrl, "https");
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules.HasScheme"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<string> HasScheme(this IMustClause _,
        string? value,
        string scheme,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Uri.Scheme.Mismatch, NullMessage, paramName, value);

        if (scheme is null)
            return MustResult<string>.Fail(MustCodes.Uri.Scheme.Mismatch, NullMessage, nameof(scheme), scheme);

        const string messageTemplate = "{paramName} must have the expected scheme.";

        var ok = UriRules.HasScheme(value, scheme);
        return MustResult<string>.FromBool(ok, MustCodes.Uri.Scheme.Mismatch, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified string is not a valid local file system path.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a valid file path, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="UriRules.IsFilePath"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a valid file path."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotFilePath(userInput);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules.IsFilePath"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<string> NotFilePath(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Uri.FilePath.WellFormed, NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid file path.";

        var ok = !UriRules.IsFilePath(value);
        return MustResult<string>.FromBool(ok, MustCodes.Uri.FilePath.WellFormed, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified URI string does not use the given scheme.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The URI string to validate.</param>
    /// <param name="scheme">The scheme to check against (e.g., <c>"http"</c>). Must not be <see langword="null"/>.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not use <paramref name="scheme"/>, or <see langword="false"/> with a
    /// descriptive <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> or <paramref name="scheme"/> is <see langword="null"/>.
    /// Delegates to <see cref="UriRules.HasScheme"/>. The failure message follows the pattern
    /// <c>"{paramName} must not have the expected scheme."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotHasScheme(endpoint, "http");
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="UriRules.HasScheme"/>
    /// <seealso href="https://pineguard.ai/docs/must/uri">URI Must Clauses documentation</seealso>
    public static MustResult<string> NotHasScheme(this IMustClause _,
        string? value,
        string scheme,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(MustCodes.Uri.Scheme.Match, NullMessage, paramName, value);

        if (scheme is null)
            return MustResult<string>.Fail(MustCodes.Uri.Scheme.Match, NullMessage, nameof(scheme), scheme);

        const string messageTemplate = "{paramName} must not have the expected scheme.";

        var ok = !UriRules.HasScheme(value, scheme);
        return MustResult<string>.FromBool(ok, MustCodes.Uri.Scheme.Match, messageTemplate, paramName, value, result: value);
    }
}
