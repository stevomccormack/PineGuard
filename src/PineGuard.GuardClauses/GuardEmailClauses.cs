using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for email address validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/email">Guard Email Clauses documentation</seealso>
public static class GuardEmailClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid email address.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEmailClauses.Email"/>.
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
    /// Thrown when <paramref name="value"/> is not a valid email and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEmailClauses.Email"/>:
    /// <c>Guard.Against.NotEmail</c> passes when the value is a valid email address.
    /// Uses a lenient email validation that accepts most common formats.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotEmail(emailAddress);
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.Email"/>
    public static string NotEmail(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Email(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a valid strict-format email address.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEmailClauses.StrictEmail"/>.
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
    /// Thrown when <paramref name="value"/> is not a strict email and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEmailClauses.StrictEmail"/>:
    /// <c>Guard.Against.NotStrictEmail</c> passes when the value conforms to strict RFC email format.
    /// Applies stricter validation than <see cref="NotEmail"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotStrictEmail(emailAddress);
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.StrictEmail"/>
    public static string NotStrictEmail(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.StrictEmail(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not contain an email alias (the <c>+tag</c> portion).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The email address string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEmailClauses.HasEmailAlias"/>.
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
    /// Thrown when <paramref name="value"/> has no email alias and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEmailClauses.HasEmailAlias"/>:
    /// <c>Guard.Against.NotHasEmailAlias</c> passes when the email contains a <c>+alias</c> tag.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotHasEmailAlias(emailAddress);
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.HasEmailAlias"/>
    public static string NotHasEmailAlias(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.HasEmailAlias(value, paramName); // Guard.Against.NotHasEmailAlias => Must.Be.HasEmailAlias (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> contains an email alias (the <c>+tag</c> portion).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The email address string to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustEmailClauses.NotHasEmailAlias"/>.
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
    /// Thrown when <paramref name="value"/> contains an email alias and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustEmailClauses.NotHasEmailAlias"/>:
    /// <c>Guard.Against.HasEmailAlias</c> passes when the email has no <c>+alias</c> tag.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.HasEmailAlias(canonicalEmail);
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.NotHasEmailAlias"/>
    public static string HasEmailAlias(this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotHasEmailAlias(value, paramName); // Guard.Against.HasEmailAlias => Must.Be.NotHasEmailAlias (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
