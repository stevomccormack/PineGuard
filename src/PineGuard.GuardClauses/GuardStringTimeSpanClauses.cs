using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for string-to-TimeSpan parsing guard clauses.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/string-time-span">Guard StringTimeSpan documentation</seealso>
public static class GuardStringTimeSpanClauses
{
    /// <summary>
    /// Throws when <paramref name="value"/> cannot be parsed or its parsed value is outside the specified range.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringTimeSpanClauses.InRangeTimeSpan"/>
    public static TimeSpan OutOfRangeTimeSpan(this IGuardClause _,
        string? value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.InRangeTimeSpan(value, min, max, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws when <paramref name="value"/> cannot be parsed or its parsed value is inside the specified range.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringTimeSpanClauses.OutOfRangeTimeSpan"/>
    public static TimeSpan InRangeTimeSpan(this IGuardClause _,
        string? value,
        TimeSpan min,
        TimeSpan max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.OutOfRangeTimeSpan(value, min, max, inclusion, paramName); // Guard.Against.InRange => Must.Be.OutOfRange (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is shorter than <paramref name="other"/>.
    /// <see cref="Inclusion.Inclusive"/> also throws when the two are equal.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The other value.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringTimeSpanClauses.LongerThanTimeSpan"/>
    public static TimeSpan ShorterThanTimeSpan(this IGuardClause _,
        string? value,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.LongerThanTimeSpan(value, other, inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : inclusion == Inclusion.Exclusive ? Inclusion.Inclusive : inclusion, paramName); // Guard.Against.ShorterThanTimeSpan => Must.Be.LongerThanTimeSpan (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> is longer than <paramref name="other"/>.
    /// <see cref="Inclusion.Inclusive"/> also throws when the two are equal.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The other value.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="message">An optional custom error message.</param>
    /// <param name="exceptionCreator">An optional factory to create a custom exception.</param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>The validated value if the guard passes.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the guard condition is violated and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <seealso cref="MustStringTimeSpanClauses.ShorterThanTimeSpan"/>
    public static TimeSpan LongerThanTimeSpan(this IGuardClause _,
        string? value,
        TimeSpan other,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.ShorterThanTimeSpan(value, other, inclusion == Inclusion.Inclusive ? Inclusion.Exclusive : inclusion == Inclusion.Exclusive ? Inclusion.Inclusive : inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
