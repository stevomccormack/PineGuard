using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for <see cref="bool"/> values.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/bool">Guard Bool Clauses documentation</seealso>
public static class GuardBoolClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is <see langword="false"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The boolean value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBoolClauses.True"/>.
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
    /// Thrown when <paramref name="value"/> is <see langword="false"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBoolClauses.True"/>:
    /// <c>Guard.Against.False</c> passes when the value is <see langword="true"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.False(isActive);
    /// Guard.Against.False(isActive, "Must be active.");
    /// </code>
    /// </example>
    /// <seealso cref="MustBoolClauses.True"/>
    public static bool False(this IGuardClause _,
        bool value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.True(value, paramName); // Guard.Against.False => Must.Be.True (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is <see langword="true"/>.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The boolean value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustBoolClauses.False"/>.
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
    /// Thrown when <paramref name="value"/> is <see langword="true"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustBoolClauses.False"/>:
    /// <c>Guard.Against.True</c> passes when the value is <see langword="false"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.True(isDeleted);
    /// Guard.Against.True(isDeleted, "Must not be deleted.");
    /// </code>
    /// </example>
    /// <seealso cref="MustBoolClauses.False"/>
    public static bool True(this IGuardClause _,
        bool value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.False(value, paramName); // Guard.Against.True => Must.Be.False (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result;
    }
}
