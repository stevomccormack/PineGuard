using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate binary-encoded string values
/// such as hexadecimal and Base64.
/// </summary>
/// <seealso cref="BufferRules"/>
/// <seealso href="https://pineguard.ai/docs/must/buffer">Buffer Must Clauses documentation</seealso>
public static class MustBufferClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified string is a valid hexadecimal-encoded value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a hex-encoded value.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid hex string, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="BufferRules.IsHex"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid hex string."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Hex(hashValue);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="BufferRules.IsHex"/>
    /// <seealso href="https://pineguard.ai/docs/must/buffer">Buffer Must Clauses documentation</seealso>
    public static MustResult<string> Hex(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid hex string.";

        var ok = BufferRules.IsHex(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified string is a valid Base64-encoded value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a Base64-encoded value.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid Base64 string, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="BufferRules.IsBase64"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid base64 string."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Base64(encodedData);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="BufferRules.IsBase64"/>
    /// <seealso href="https://pineguard.ai/docs/must/buffer">Buffer Must Clauses documentation</seealso>
    public static MustResult<string> Base64(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid base64 string.";

        var ok = BufferRules.IsBase64(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified string is not a valid Base64-encoded value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a valid Base64 string, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="BufferRules.IsBase64"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a valid base64 string."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotBase64(rawData);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="BufferRules.IsBase64"/>
    /// <seealso href="https://pineguard.ai/docs/must/buffer">Buffer Must Clauses documentation</seealso>
    public static MustResult<string> NotBase64(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid base64 string.";

        var ok = !BufferRules.IsBase64(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified string is not a valid hexadecimal-encoded value.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is not a valid hex string, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="BufferRules.IsHex"/>. The failure message follows the pattern
    /// <c>"{paramName} must not be a valid hex string."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotHex(rawData);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="BufferRules.IsHex"/>
    /// <seealso href="https://pineguard.ai/docs/must/buffer">Buffer Must Clauses documentation</seealso>
    public static MustResult<string> NotHex(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not be a valid hex string.";

        var ok = !BufferRules.IsHex(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }
}
