using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate JSON strings and HTTP headers.
/// </summary>
/// <seealso cref="JsonRules"/>
/// <seealso href="https://pineguard.ai/docs/must/json">JSON Must Clauses documentation</seealso>
public static class MustJsonClauses
{
    /// <summary>
    /// Validates that the specified string is well-formed JSON.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as JSON.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is valid JSON, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="JsonRules.IsJson"/>. The failure message follows the pattern
    /// <c>"{paramName} must be JSON."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Json(responseBody);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="JsonRules.IsJson"/>
    /// <seealso href="https://pineguard.ai/docs/must/json">JSON Must Clauses documentation</seealso>
    public static MustResult<string> Json(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be JSON.";

        var ok = JsonRules.IsJson(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified HTTP headers dictionary contains a JSON <c>Content-Type</c> header value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="headers">The HTTP headers dictionary to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="headers"/> contains a JSON content-type, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Delegates to <see cref="JsonRules.IsJsonContentType"/>. The failure message follows the pattern
    /// <c>"{paramName} must contain a JSON Content-Type."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.JsonContentType(response.Headers);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="JsonRules.IsJsonContentType"/>
    /// <seealso href="https://pineguard.ai/docs/must/json">JSON Must Clauses documentation</seealso>
    public static MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?> JsonContentType(this IMustClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must contain a JSON Content-Type.";

        var ok = JsonRules.IsJsonContentType(headers);
        return MustResult<IReadOnlyDictionary<string, IEnumerable<string>>?>.FromBool(ok, messageTemplate, paramName, headers, headers);
    }

    /// <summary>
    /// Validates that the specified string is a well-formed JSON object (starts with <c>{</c>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a JSON object.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid JSON object, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="JsonRules.IsJsonObject"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a JSON object."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.JsonObject(payload);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="JsonRules.IsJsonObject"/>
    /// <seealso href="https://pineguard.ai/docs/must/json">JSON Must Clauses documentation</seealso>
    public static MustResult<string> JsonObject(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a JSON object.";

        var ok = JsonRules.IsJsonObject(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified string is a well-formed JSON array (starts with <c>[</c>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a JSON array.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid JSON array, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="JsonRules.IsJsonArray"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a JSON array."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.JsonArray(listPayload);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="JsonRules.IsJsonArray"/>
    /// <seealso href="https://pineguard.ai/docs/must/json">JSON Must Clauses documentation</seealso>
    public static MustResult<string> JsonArray(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail("{paramName} must not be null.", paramName, value);

        const string messageTemplate = "{paramName} must be a JSON array.";

        var ok = JsonRules.IsJsonArray(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, value);
    }
}
