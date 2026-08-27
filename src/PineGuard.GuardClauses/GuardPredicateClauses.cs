using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for custom predicate-based validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/predicate">Guard Predicate Clauses documentation</seealso>
public static class GuardPredicateClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> does not satisfy <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="predicate">The predicate that <paramref name="value"/> must satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustPredicateClauses.Satisfies{T}"/>.
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
    /// Thrown when <paramref name="value"/> does not satisfy the predicate and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustPredicateClauses.Satisfies{T}"/>:
    /// <c>Guard.Against.NotSatisfies</c> passes when the predicate returns <see langword="true"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotSatisfies(age, x => x >= 18);
    /// </code>
    /// </example>
    /// <seealso cref="MustPredicateClauses.Satisfies{T}"/>
    public static T NotSatisfies<T>(this IGuardClause _,
        T? value,
        Func<T, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Satisfies(value, predicate, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> satisfies <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="predicate">The predicate that <paramref name="value"/> must not satisfy.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustPredicateClauses.NotSatisfies{T}"/>.
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
    /// Thrown when <paramref name="value"/> satisfies the predicate and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustPredicateClauses.NotSatisfies{T}"/>:
    /// <c>Guard.Against.Satisfies</c> passes when the predicate returns <see langword="false"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Satisfies(value, x => x.IsDeleted);
    /// </code>
    /// </example>
    /// <seealso cref="MustPredicateClauses.NotSatisfies{T}"/>
    public static T Satisfies<T>(this IGuardClause _,
        T? value,
        Func<T, bool> predicate,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotSatisfies(value, predicate, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result!;
    }
}
