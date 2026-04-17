using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate email address strings.
/// </summary>
/// <seealso cref="EmailRules"/>
/// <seealso href="https://pineguard.ai/docs/must/email">Email Must Clauses documentation</seealso>
public static class MustEmailClauses
{
    private const string NullMessage = "{paramName} must not be null.";

    /// <summary>
    /// Validates that the specified string is a valid email address.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as an email address.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> is a valid email address, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="EmailRules.IsEmail"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid email address."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.Email(emailAddress);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="EmailRules.IsEmail"/>
    /// <seealso href="https://pineguard.ai/docs/must/email">Email Must Clauses documentation</seealso>
    public static MustResult<string> Email(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid email address.";

        var ok = EmailRules.IsEmail(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified string is a valid strict email address (applying more rigorous RFC-compliance rules).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The string to validate as a strict email address.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> passes strict email validation, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="EmailRules.IsStrictEmail"/>. The failure message follows the pattern
    /// <c>"{paramName} must be a valid strict email address."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.StrictEmail(userEmail);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="EmailRules.IsStrictEmail"/>
    /// <seealso href="https://pineguard.ai/docs/must/email">Email Must Clauses documentation</seealso>
    public static MustResult<string> StrictEmail(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must be a valid strict email address.";

        var ok = EmailRules.IsStrictEmail(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified email address string contains a sub-address alias (e.g., <c>user+tag@example.com</c>).
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The email address string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> contains an email alias, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="EmailRules.HasAlias"/>. The failure message follows the pattern
    /// <c>"{paramName} must contain an email alias."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.HasEmailAlias(taggedEmail);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="EmailRules.HasAlias"/>
    /// <seealso href="https://pineguard.ai/docs/must/email">Email Must Clauses documentation</seealso>
    public static MustResult<string> HasEmailAlias(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must contain an email alias.";

        var ok = EmailRules.HasAlias(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }

    /// <summary>
    /// Validates that the specified email address string does not contain a sub-address alias.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The email address string to validate.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> where <see cref="MustResult{T}.Success"/> is <see langword="true"/>
    /// if <paramref name="value"/> does not contain an email alias, or <see langword="false"/> with a descriptive
    /// <see cref="MustResult{T}.Message"/>.
    /// </returns>
    /// <remarks>
    /// Returns a failed result immediately if <paramref name="value"/> is <see langword="null"/>.
    /// Delegates to <see cref="EmailRules.HasAlias"/>. The failure message follows the pattern
    /// <c>"{paramName} must not contain an email alias."</c>
    /// </remarks>
    /// <example>
    /// <code>
    /// var result = Must.Be.NotHasEmailAlias(canonicalEmail);
    /// if (result.Failed)
    ///     Console.WriteLine(result.Message);
    /// </code>
    /// </example>
    /// <seealso cref="EmailRules.HasAlias"/>
    /// <seealso href="https://pineguard.ai/docs/must/email">Email Must Clauses documentation</seealso>
    public static MustResult<string> NotHasEmailAlias(this IMustClause _,
        string? value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value is null)
            return MustResult<string>.Fail(NullMessage, paramName, value);

        const string messageTemplate = "{paramName} must not contain an email alias.";

        var ok = !EmailRules.HasAlias(value);
        return MustResult<string>.FromBool(ok, messageTemplate, paramName, value, result: value);
    }
}
