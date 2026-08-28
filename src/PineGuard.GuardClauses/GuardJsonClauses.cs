using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for JSON string and content-type validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/json">Guard JSON Clauses documentation</seealso>
public static class GuardJsonClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid JSON string.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustJsonClauses.Json"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not valid JSON and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustJsonClauses.Json"/>:
    /// <c>Guard.Against.NotJson</c> passes when the value is well-formed JSON.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotJson(responseBody);
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.Json"/>
    public static string NotJson(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Json(value, paramName); // Guard.Against.NotJson => Must.Be.Json
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="headers"/> do not include a JSON content-type header.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="headers">The HTTP response headers to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustJsonClauses.JsonContentType"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated headers if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the headers lack a JSON content-type and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustJsonClauses.JsonContentType"/>:
    /// <c>Guard.Against.NotJsonContentType</c> passes when <c>Content-Type: application/json</c> is present.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotJsonContentType(response.Headers);
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.JsonContentType"/>
    public static IReadOnlyDictionary<string, IEnumerable<string>>? NotJsonContentType(this IGuardClause _,
        IReadOnlyDictionary<string, IEnumerable<string>>? headers,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(headers))] string? paramName = null)
    {
        var result = Must.Be.JsonContentType(headers, paramName); // Guard.Against.NotJsonContentType => Must.Be.JsonContentType
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid JSON object (must start with <c>{</c>).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustJsonClauses.JsonObject"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a JSON object and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustJsonClauses.JsonObject"/>:
    /// <c>Guard.Against.NotJsonObject</c> passes when the value is a valid JSON object.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotJsonObject(body);
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.JsonObject"/>
    public static string NotJsonObject(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.JsonObject(value, paramName); // Guard.Against.NotJsonObject => Must.Be.JsonObject
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid JSON array (must start with <c>[</c>).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustJsonClauses.JsonArray"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not a JSON array and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustJsonClauses.JsonArray"/>:
    /// <c>Guard.Against.NotJsonArray</c> passes when the value is a valid JSON array.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotJsonArray(body);
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.JsonArray"/>
    public static string NotJsonArray(this IGuardClause _,
        string? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.JsonArray(value, paramName); // Guard.Against.NotJsonArray => Must.Be.JsonArray
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
