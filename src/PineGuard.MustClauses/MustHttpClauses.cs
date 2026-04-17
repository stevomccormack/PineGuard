using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate HTTP-related values such as headers and status codes,
/// delegating to <see cref="HttpRules"/> for core validation logic.
/// </summary>
/// <seealso cref="HttpRules"/>
/// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
public static class MustHttpClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified value must be a valid HTTP header name.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="name">The name to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid HTTP header name."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<string> HeaderName(this IMustClause _,
        string? name,
        [CallerArgumentExpression(nameof(name))] string? paramName = null)
    {
        if (name is null)
            return MustResult<string>.Fail(NullMessage, paramName, name);

        const string messageTemplate = "{paramName} must be a valid HTTP header name.";

        var ok = HttpRules.IsHeaderName(name);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, name, name);
    }

    /// <summary>
    /// Validates that the specified value must be a valid HTTP header value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid HTTP header value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<string> HeaderValue(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid HTTP header value.";

        var ok = HttpRules.IsHeaderValue(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must be a valid HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a valid HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> HttpStatusCode(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a valid HTTP status code.";

        var ok = HttpRules.IsHttpStatusCode(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must be an informational HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be an informational HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> HttpStatusInformational(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be an informational HTTP status code.";

        var ok = HttpRules.IsHttpStatusInformational(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must be a successful HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a successful HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> HttpStatusSuccess(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a successful HTTP status code.";

        var ok = HttpRules.IsHttpStatusSuccess(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must be a redirect HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a redirect HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> HttpStatusRedirect(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a redirect HTTP status code.";

        var ok = HttpRules.IsHttpStatusRedirect(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must be a client error HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a client error HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> HttpStatusClientError(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a client error HTTP status code.";

        var ok = HttpRules.IsHttpStatusClientError(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must be a server error HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must be a server error HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> HttpStatusServerError(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be a server error HTTP status code.";

        var ok = HttpRules.IsHttpStatusServerError(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain the specified header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> HasHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified header.";

        var ok = HttpRules.HasHeader(headers, name);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a value for the specified header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a value for the specified header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> HasHeaderValue(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain a value for the specified header.";

        var ok = HttpRules.HasHeaderValue(headers, name);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain the specified header value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain the specified header value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> HasHeaderValueEqualTo(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        string? expectedValue,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain the specified header value.";

        var ok = HttpRules.HasHeaderValue(headers, name, expectedValue, comparison);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain a single value for the specified header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain a single value for the specified header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> HasSingleHeaderValue(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain a single value for the specified header.";

        var ok = HttpRules.HasSingleHeaderValue(headers, name);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must contain an allowed Content-Type.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must contain an allowed Content-Type."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> HasContentType(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string[]? allowed,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain an allowed Content-Type.";

        var ok = HttpRules.HasContentType(headers, allowed);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid HTTP header name.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="name">The name to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid HTTP header name."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<string> NotHeaderName(this IMustClause _,
        string? name,
        [CallerArgumentExpression(nameof(name))] string? paramName = null)
    {
        if (name is null)
            return MustResult<string>.Fail(NullMessage, paramName, name);

        const string messageTemplate = "{paramName} must not be a valid HTTP header name.";

        var ok = !HttpRules.IsHeaderName(name);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, name, name);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid HTTP header value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid HTTP header value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<string> NotHeaderValue(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid HTTP header value.";

        var ok = !HttpRules.IsHeaderValue(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must not be a valid HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a valid HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> NotHttpStatusCode(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a valid HTTP status code.";

        var ok = !HttpRules.IsHttpStatusCode(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must not be an informational HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be an informational HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> NotHttpStatusInformational(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be an informational HTTP status code.";

        var ok = !HttpRules.IsHttpStatusInformational(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must not be a successful HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a successful HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> NotHttpStatusSuccess(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a successful HTTP status code.";

        var ok = !HttpRules.IsHttpStatusSuccess(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must not be a redirect HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a redirect HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> NotHttpStatusRedirect(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a redirect HTTP status code.";

        var ok = !HttpRules.IsHttpStatusRedirect(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must not be a client error HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a client error HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> NotHttpStatusClientError(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a client error HTTP status code.";

        var ok = !HttpRules.IsHttpStatusClientError(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must not be a server error HTTP status code.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="status">The HTTP status code to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not be a server error HTTP status code."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<int> NotHttpStatusServerError(this IMustClause _,
        int status,
        [CallerArgumentExpression(nameof(status))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not be a server error HTTP status code.";

        var ok = !HttpRules.IsHttpStatusServerError(status);
        return MustResult<int>.FromBool(ok, messageTemplate, paramName, status, result: status);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasHeader(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified header.";

        var ok = !HttpRules.HasHeader(headers, name);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a value for the specified header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a value for the specified header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasHeaderValue(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a value for the specified header.";

        var ok = !HttpRules.HasHeaderValue(headers, name);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain the specified header value.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain the specified header value."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasHeaderValueEqualTo(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        string? expectedValue,
        StringComparison comparison = StringComparison.OrdinalIgnoreCase,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain the specified header value.";

        var ok = !HttpRules.HasHeaderValue(headers, name, expectedValue, comparison);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain a single value for the specified header.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain a single value for the specified header."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasSingleHeaderValue(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? name,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain a single value for the specified header.";

        var ok = !HttpRules.HasSingleHeaderValue(headers, name);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified value must not contain an allowed Content-Type.
    /// </summary>
    /// <remarks>
    /// The failure message follows the pattern <c>"{paramName} must not contain an allowed Content-Type."</c>
    /// </remarks>
    /// <seealso href="https://pineguard.ai/docs/must/http">Http Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> NotHasContentType(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string[]? allowed,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must not contain an allowed Content-Type.";

        var ok = !HttpRules.HasContentType(headers, allowed);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }
}

