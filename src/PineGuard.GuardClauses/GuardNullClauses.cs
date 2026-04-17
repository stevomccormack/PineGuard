using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for <see langword="null"/> reference checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/null">Guard Null Clauses documentation</seealso>
public static class GuardNullClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNullClauses.Null{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> (always <see langword="null"/>) if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not <see langword="null"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNullClauses.Null{T}"/>:
    /// <c>Guard.Against.NotNull</c> passes when the value is <see langword="null"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotNull(optionalOverride);
    /// </code>
    /// </example>
    /// <seealso cref="MustNullClauses.Null{T}"/>
    public static T? NotNull<T>(this IGuardClause _,
        T? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Null(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNullClauses.NotNull{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (non-<see langword="null"/>) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNullClauses.NotNull{T}"/>:
    /// <c>Guard.Against.Null</c> passes when the value is not <see langword="null"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Null(entity);
    /// </code>
    /// </example>
    /// <seealso cref="MustNullClauses.NotNull{T}"/>
    public static T Null<T>(this IGuardClause _,
        T? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotNull(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }
}
