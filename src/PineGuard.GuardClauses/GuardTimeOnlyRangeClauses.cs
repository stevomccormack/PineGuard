#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for time-only range validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/time-only-range">Guard TimeOnlyRange documentation</seealso>
public static class GuardTimeOnlyRangeClauses
{
    /// <summary>
    /// Throws if <paramref name="range"/> violates the NotChronological constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="range">The range to guard.</param>
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
    /// <seealso cref="MustTimeOnlyRangeClauses.Chronological"/>
    public static TimeOnlyRange NotChronological(this IGuardClause _,
        TimeOnlyRange range,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        var result = Must.Be.Chronological(range, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, range, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="range1"/> violates the Overlapping constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="range1">The first range to compare.</param>
    /// <param name="range2">The second range to compare.</param>
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
    /// <seealso cref="MustTimeOnlyRangeClauses.NotOverlapping"/>
    public static TimeOnlyRange Overlapping(this IGuardClause _,
        TimeOnlyRange range1,
        TimeOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        var result = Must.Be.NotOverlapping(range1, range2, inclusion, paramName); // Guard.Against.Overlapping => Must.Be.NotOverlapping (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, range1, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="range1"/> violates the NotOverlapping constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="range1">The first range to compare.</param>
    /// <param name="range2">The second range to compare.</param>
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
    /// <seealso cref="MustTimeOnlyRangeClauses.Overlapping"/>
    public static TimeOnlyRange NotOverlapping(this IGuardClause _,
        TimeOnlyRange range1,
        TimeOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        var result = Must.Be.Overlapping(range1, range2, inclusion, paramName); // Guard.Against.NotOverlapping => Must.Be.Overlapping (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, range1, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="range"/> violates the NotContains constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="range">The range to guard.</param>
    /// <param name="value">The value to guard.</param>
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
    /// <seealso cref="MustTimeOnlyRangeClauses.Contains"/>
    public static TimeOnlyRange NotContains(this IGuardClause _,
        TimeOnlyRange range,
        TimeOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        var result = Must.Be.Contains(range, value, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, range, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="range"/> violates the Contains constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="range">The range to guard.</param>
    /// <param name="value">The value to guard.</param>
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
    /// <seealso cref="MustTimeOnlyRangeClauses.NotContains"/>
    public static TimeOnlyRange Contains(this IGuardClause _,
        TimeOnlyRange range,
        TimeOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        var result = Must.Be.NotContains(range, value, inclusion, paramName); // Guard.Against.Contains => Must.Be.NotContains (complement)
        if (result.Failed)
            GuardFailure.Throw(message ?? result.Message, paramName, range, exceptionCreator);

        return result.Result;
    }
}
#endif
