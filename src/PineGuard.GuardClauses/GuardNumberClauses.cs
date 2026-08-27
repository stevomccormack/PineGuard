#if NET8_0_OR_GREATER
using System.Numerics;
using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for numeric value validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/number">Guard Number Clauses documentation</seealso>
public static class GuardNumberClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is zero or negative.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Positive{T}"/>.
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
    /// Thrown when <paramref name="value"/> is zero or negative and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNumberClauses.Positive{T}"/>:
    /// <c>Guard.Against.ZeroOrNegative</c> passes when the value is strictly positive.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.ZeroOrNegative(quantity);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Positive{T}"/>
    public static T ZeroOrNegative<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.Positive(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is zero or positive.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Negative{T}"/>.
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
    /// Thrown when <paramref name="value"/> is zero or positive and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNumberClauses.Negative{T}"/>:
    /// <c>Guard.Against.ZeroOrPositive</c> passes when the value is strictly negative.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.ZeroOrPositive(adjustment);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Negative{T}"/>
    public static T ZeroOrPositive<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.Negative(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not zero.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Zero{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="value"/> (always zero) if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not zero and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNumberClauses.Zero{T}"/>:
    /// <c>Guard.Against.NotZero</c> passes when the value is zero.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotZero(balance);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Zero{T}"/>
    public static T NotZero<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.Zero(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is zero.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NotZero{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (non-zero) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is zero and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNumberClauses.NotZero{T}"/>:
    /// <c>Guard.Against.Zero</c> passes when the value is not zero.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Zero(divisor);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotZero{T}"/>
    public static T Zero<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.NotZero(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is negative.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.ZeroOrPositive{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (non-negative) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is negative and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNumberClauses.ZeroOrPositive{T}"/>:
    /// <c>Guard.Against.Negative</c> passes when the value is zero or positive.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Negative(count);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.ZeroOrPositive{T}"/>
    public static T Negative<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.ZeroOrPositive(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is positive.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.ZeroOrNegative{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (non-positive) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is positive and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustNumberClauses.ZeroOrNegative{T}"/>:
    /// <c>Guard.Against.Positive</c> passes when the value is zero or negative.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.Positive(debt);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.ZeroOrNegative{T}"/>
    public static T Positive<T>(this IGuardClause _,
        T value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.ZeroOrNegative(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is less than or equal to <paramref name="min"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum threshold.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.GreaterThan{T}"/>.
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
    /// Thrown when <paramref name="value"/> is less than or equal to <paramref name="min"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.GreaterThan{T}"/>
    public static T LessThanOrEqual<T>(this IGuardClause _,
        T value,
        T min,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.GreaterThan(value, min, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is less than <paramref name="min"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum threshold.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.GreaterThanOrEqual{T}"/>.
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
    /// Thrown when <paramref name="value"/> is less than <paramref name="min"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.GreaterThanOrEqual{T}"/>
    public static T LessThan<T>(this IGuardClause _,
        T value,
        T min,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.GreaterThanOrEqual(value, min, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is greater than or equal to <paramref name="max"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="max">The maximum threshold.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.LessThan{T}"/>.
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
    /// Thrown when <paramref name="value"/> is greater than or equal to <paramref name="max"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.LessThan{T}"/>
    public static T GreaterThanOrEqual<T>(this IGuardClause _,
        T value,
        T max,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.LessThan(value, max, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is greater than <paramref name="min"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The upper bound threshold.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.LessThanOrEqual{T}"/>.
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
    /// Thrown when <paramref name="value"/> is greater than <paramref name="min"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.LessThanOrEqual{T}"/>
    public static T GreaterThan<T>(this IGuardClause _,
        T value,
        T min,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.LessThanOrEqual(value, min, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is outside the specified range.
    /// </summary>
    /// <typeparam name="T">The comparable type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum bound of the range.</param>
    /// <param name="max">The maximum bound of the range.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.InRange{T}"/>.
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
    /// Thrown when <paramref name="value"/> is outside the range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.InRange{T}"/>
    public static T OutOfRange<T>(this IGuardClause _,
        T value,
        T min,
        T max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        var result = Must.Be.InRange(value, min, max, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is inside the specified range.
    /// </summary>
    /// <typeparam name="T">The comparable type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum bound of the range.</param>
    /// <param name="max">The maximum bound of the range.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.OutOfRange{T}"/>.
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
    /// Thrown when <paramref name="value"/> is inside the range and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.OutOfRange{T}"/>
    public static T InRange<T>(this IGuardClause _,
        T value,
        T min,
        T max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, IComparable<T>
    {
        var result = Must.Be.OutOfRange(value, min, max, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not approximately equal to <paramref name="target"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="target">The target value for approximation.</param>
    /// <param name="tolerance">The tolerance for the approximation comparison.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Approximately{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not approximately equal to <paramref name="target"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.Approximately{T}"/>
    public static T NotApproximately<T>(this IGuardClause _,
        T value,
        T target,
        T? tolerance,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.Approximately(value, target, tolerance, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is approximately equal to <paramref name="target"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="target">The target value for approximation.</param>
    /// <param name="tolerance">The tolerance for the approximation comparison.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NotApproximately{T}"/>.
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
    /// Thrown when <paramref name="value"/> is approximately equal to <paramref name="target"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NotApproximately{T}"/>
    public static T Approximately<T>(this IGuardClause _,
        T value,
        T target,
        T? tolerance,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.NotApproximately(value, target, tolerance, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not a multiple of <paramref name="factor"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="factor">The factor to check divisibility against.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.MultipleOf{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not a multiple of <paramref name="factor"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.MultipleOf{T}"/>
    public static T NotMultipleOf<T>(this IGuardClause _,
        T value,
        T factor,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.MultipleOf(value, factor, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is a multiple of <paramref name="factor"/>.
    /// </summary>
    /// <typeparam name="T">The numeric type to guard.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="factor">The factor to check divisibility against.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NotMultipleOf{T}"/>.
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
    /// Thrown when <paramref name="value"/> is a multiple of <paramref name="factor"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NotMultipleOf{T}"/>
    public static T MultipleOf<T>(this IGuardClause _,
        T value,
        T factor,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
        where T : struct, INumber<T>
    {
        var result = Must.Be.NotMultipleOf(value, factor, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is odd.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="int"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Even(IMustClause, int, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (even) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is odd and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.Even(IMustClause, int, string?)"/>
    public static int Odd(this IGuardClause _,
        int value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Even(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is odd.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="long"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Even(IMustClause, long, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (even) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is odd and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.Even(IMustClause, long, string?)"/>
    public static long Odd(this IGuardClause _,
        long value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Even(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is even.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="int"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Odd(IMustClause, int, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (odd) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is even and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.Odd(IMustClause, int, string?)"/>
    public static int Even(this IGuardClause _,
        int value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Odd(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is even.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="long"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Odd(IMustClause, long, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (odd) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is even and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.Odd(IMustClause, long, string?)"/>
    public static long Even(this IGuardClause _,
        long value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Odd(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is finite (not infinity and not NaN).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="float"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NotFinite(IMustClause, float, string?)"/>.
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
    /// Thrown when <paramref name="value"/> is finite and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NotFinite(IMustClause, float, string?)"/>
    public static float Finite(this IGuardClause _,
        float value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotFinite(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is finite (not infinity and not NaN).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="double"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NotFinite(IMustClause, double, string?)"/>.
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
    /// Thrown when <paramref name="value"/> is finite and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NotFinite(IMustClause, double, string?)"/>
    public static double Finite(this IGuardClause _,
        double value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotFinite(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not finite (infinity or NaN).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="float"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Finite(IMustClause, float, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (finite) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not finite and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.Finite(IMustClause, float, string?)"/>
    public static float NotFinite(this IGuardClause _,
        float value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Finite(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not finite (infinity or NaN).
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="double"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.Finite(IMustClause, double, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (finite) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is not finite and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.Finite(IMustClause, double, string?)"/>
    public static double NotFinite(this IGuardClause _,
        double value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Finite(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is NaN.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="float"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NotNaN(IMustClause, float, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (non-NaN) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is NaN and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NotNaN(IMustClause, float, string?)"/>
    public static float NaN(this IGuardClause _,
        float value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotNaN(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is NaN.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="double"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NotNaN(IMustClause, double, string?)"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated (non-NaN) value of <paramref name="value"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="value"/> is NaN and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NotNaN(IMustClause, double, string?)"/>
    public static double NaN(this IGuardClause _,
        double value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotNaN(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not NaN.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="float"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NaN(IMustClause, float, string?)"/>.
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
    /// Thrown when <paramref name="value"/> is not NaN and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NaN(IMustClause, float, string?)"/>
    public static float NotNaN(this IGuardClause _,
        float value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NaN(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not NaN.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The <see cref="double"/> value to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustNumberClauses.NaN(IMustClause, double, string?)"/>.
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
    /// Thrown when <paramref name="value"/> is not NaN and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustNumberClauses.NaN(IMustClause, double, string?)"/>
    public static double NotNaN(this IGuardClause _,
        double value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NaN(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
#endif
