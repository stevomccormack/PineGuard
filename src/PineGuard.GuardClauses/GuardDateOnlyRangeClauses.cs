#if NET8_0_OR_GREATER
using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for date-only range validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/date-only-range">Guard DateOnlyRange documentation</seealso>
public static class GuardDateOnlyRangeClauses
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
    /// <seealso cref="MustDateOnlyRangeClauses.Chronological"/>
    public static DateOnlyRange NotChronological(this IGuardClause _,
        DateOnlyRange range,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        var result = Must.Be.Chronological(range, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

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
    /// <seealso cref="MustDateOnlyRangeClauses.NotOverlapping"/>
    public static DateOnlyRange Overlapping(this IGuardClause _,
        DateOnlyRange range1,
        DateOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        var result = Must.Be.NotOverlapping(range1, range2, inclusion, paramName); // Guard.Against.Overlapping => Must.Be.NotOverlapping (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

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
    /// <seealso cref="MustDateOnlyRangeClauses.Overlapping"/>
    public static DateOnlyRange NotOverlapping(this IGuardClause _,
        DateOnlyRange range1,
        DateOnlyRange range2,
        Inclusion inclusion = Inclusion.Exclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range1))] string? paramName = null)
    {
        var result = Must.Be.Overlapping(range1, range2, inclusion, paramName); // Guard.Against.NotOverlapping => Must.Be.Overlapping (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

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
    /// <seealso cref="MustDateOnlyRangeClauses.Contains"/>
    public static DateOnlyRange NotContains(this IGuardClause _,
        DateOnlyRange range,
        DateOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        var result = Must.Be.Contains(range, value, inclusion, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

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
    /// <seealso cref="MustDateOnlyRangeClauses.NotContains"/>
    public static DateOnlyRange Contains(this IGuardClause _,
        DateOnlyRange range,
        DateOnly value,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(range))] string? paramName = null)
    {
        var result = Must.Be.NotContains(range, value, inclusion, paramName); // Guard.Against.Contains => Must.Be.NotContains (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
#endif
