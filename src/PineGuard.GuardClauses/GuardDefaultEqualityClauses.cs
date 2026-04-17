using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for default-value equality checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/default-equality">Guard Default Equality Clauses documentation</seealso>
public static class GuardDefaultEqualityClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is equal to <see langword="default"/>(<typeparamref name="T"/>).
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDefaultEqualityClauses.NotDefault{T}"/>.
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
    /// Thrown when <paramref name="value"/> equals <see langword="default"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDefaultEqualityClauses.NotDefault{T}"/>:
    /// <c>Guard.Against.Default</c> passes when the value is not the default.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Default(id);
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.NotDefault{T}"/>
    public static T Default<T>(this IGuardClause _,
        T? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotDefault(value, paramName); // Guard.Against.Default => Must.Be.NotDefault (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not equal to <see langword="default"/>(<typeparamref name="T"/>).
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDefaultEqualityClauses.Default{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not the default and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDefaultEqualityClauses.Default{T}"/>:
    /// <c>Guard.Against.NotDefault</c> passes when the value equals <see langword="default"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotDefault(resetValue);
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.Default{T}"/>
    public static T NotDefault<T>(this IGuardClause _,
        T? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Default(value, paramName); // Guard.Against.NotDefault => Must.Be.Default (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is <see langword="null"/> or equal to <see langword="default"/>(<typeparamref name="T"/>).
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDefaultEqualityClauses.NotNullOrDefault{T}"/>.
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
    /// Thrown when <paramref name="value"/> is <see langword="null"/> or default and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDefaultEqualityClauses.NotNullOrDefault{T}"/>:
    /// <c>Guard.Against.NullOrDefault</c> passes when the value is neither <see langword="null"/> nor default.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NullOrDefault(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.NotNullOrDefault{T}"/>
    public static T? NullOrDefault<T>(this IGuardClause _,
        T? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotNullOrDefault(value, paramName); // Guard.Against.NullOrDefault => Must.Be.NotNullOrDefault (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is neither <see langword="null"/> nor equal to <see langword="default"/>(<typeparamref name="T"/>).
    /// </summary>
    /// <typeparam name="T">The type of value to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDefaultEqualityClauses.NullOrDefault{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not <see langword="null"/> and not default, and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustDefaultEqualityClauses.NullOrDefault{T}"/>:
    /// <c>Guard.Against.NotNullOrDefault</c> passes when the value is <see langword="null"/> or default.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotNullOrDefault(value);
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.NullOrDefault{T}"/>
    public static T? NotNullOrDefault<T>(this IGuardClause _,
        T? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NullOrDefault(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result;
    }
}
