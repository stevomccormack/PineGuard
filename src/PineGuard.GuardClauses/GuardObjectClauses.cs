using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for object equality, type, and reference identity checks.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/object">Guard Object Clauses documentation</seealso>
public static class GuardObjectClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> is not equal to <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.EqualTo{T}"/>.
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
    /// Thrown when <paramref name="value"/> does not equal <paramref name="other"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.EqualTo{T}"/>:
    /// <c>Guard.Against.NotEqualTo</c> passes when the values are equal.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotEqualTo(actual, expected);
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.EqualTo{T}"/>
    public static T NotEqualTo<T>(this IGuardClause _,
        T? value,
        T? other,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.EqualTo(value, other, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is equal to <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="T">The type of value to compare.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value that must differ.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.NotEqualTo{T}"/>.
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
    /// Thrown when <paramref name="value"/> equals <paramref name="other"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.NotEqualTo{T}"/>:
    /// <c>Guard.Against.EqualTo</c> passes when the values are not equal.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.EqualTo(id, Guid.Empty);
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotEqualTo{T}"/>
    public static T EqualTo<T>(this IGuardClause _,
        T? value,
        T? other,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotEqualTo(value, other, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type that the object must not be.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The object to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.OfType{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not of type <typeparamref name="T"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.OfType{T}"/>:
    /// <c>Guard.Against.NotOfType</c> passes when the object is exactly of type <typeparamref name="T"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.NotOfType<string>(value);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.OfType{T}"/>
    public static object NotOfType<T>(this IGuardClause _,
        object? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.OfType<T>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type that the object must be.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The object to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.NotOfType{T}"/>.
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
    /// Thrown when <paramref name="value"/> is of type <typeparamref name="T"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.NotOfType{T}"/>:
    /// <c>Guard.Against.OfType</c> passes when the object is not of type <typeparamref name="T"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.OfType<string>(obj);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotOfType{T}"/>
    public static object OfType<T>(this IGuardClause _,
        object? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotOfType<T>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is not assignable to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The base type or interface.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The object to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.AssignableToType{T}"/>.
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
    /// Thrown when <paramref name="value"/> is not assignable to <typeparamref name="T"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.AssignableToType{T}"/>:
    /// <c>Guard.Against.NotAssignableToType</c> passes when the object is assignable to <typeparamref name="T"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.NotAssignableToType<IDisposable>(service);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.AssignableToType{T}"/>
    public static object NotAssignableToType<T>(this IGuardClause _,
        object? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.AssignableToType<T>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is assignable to <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The base type or interface.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The object to guard.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.NotAssignableToType{T}"/>.
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
    /// Thrown when <paramref name="value"/> is assignable to <typeparamref name="T"/> and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.NotAssignableToType{T}"/>:
    /// <c>Guard.Against.AssignableToType</c> passes when the object is not assignable to <typeparamref name="T"/>.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// Guard.Against.AssignableToType<IDisposable>(obj);
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotAssignableToType{T}"/>
    public static object AssignableToType<T>(this IGuardClause _,
        object? value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotAssignableToType<T>(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, value, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="a"/> and <paramref name="b"/> are the same object reference.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="a">The first object to guard.</param>
    /// <param name="b">The object that must be a different reference.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.SameReferenceAs{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="a"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="a"/> and <paramref name="b"/> are the same reference and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.SameReferenceAs{T}"/>:
    /// <c>Guard.Against.NotSameReferenceAs</c> passes when both references point to the same instance.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotSameReferenceAs(current, expected);
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.SameReferenceAs{T}"/>
    public static T NotSameReferenceAs<T>(this IGuardClause _,
        T? a,
        T? b,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(a))] string? paramName = null)
        where T : class
    {
        var result = Must.Be.SameReferenceAs(a, b, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, a, exceptionCreator);

        return result.Result!;
    }

    /// <summary>
    /// Throws if <paramref name="a"/> and <paramref name="b"/> are different object references.
    /// </summary>
    /// <typeparam name="T">The reference type.</typeparam>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="a">The first object to guard.</param>
    /// <param name="b">The object that must not be the same reference.</param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustObjectClauses.NotSameReferenceAs{T}"/>.
    /// </param>
    /// <param name="exceptionCreator">
    /// An optional factory to create a custom exception. If <see langword="null"/>,
    /// throws <see cref="ArgumentException"/> via <see cref="GuardFailure.Throw"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value of <paramref name="a"/> if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="a"/> and <paramref name="b"/> are different references and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// This guard is the complement of <see cref="MustObjectClauses.NotSameReferenceAs{T}"/>:
    /// <c>Guard.Against.SameReferenceAs</c> passes when the references are different.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.SameReferenceAs(source, copy);
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotSameReferenceAs{T}"/>
    public static T SameReferenceAs<T>(this IGuardClause _,
        T? a,
        T? b,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(a))] string? paramName = null)
        where T : class
    {
        var result = Must.Be.NotSameReferenceAs(a, b, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, a, exceptionCreator);

        return result.Result!;
    }
}
