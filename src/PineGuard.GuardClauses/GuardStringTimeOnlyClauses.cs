#if NET8_0_OR_GREATER
using System.Globalization;
using System.Runtime.CompilerServices;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for string-to-TimeOnly parsing guard clauses.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/guard/string-time-only">Guard StringTimeOnly documentation</seealso>
public static class GuardStringTimeOnlyClauses
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotBetweenTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.BetweenTimeOnly"/>
    public static TimeOnly NotBetweenTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.BetweenTimeOnly(value, min, max, inclusion, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the BetweenTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="min">The minimum bound.</param>
    /// <param name="max">The maximum bound.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotBetweenTimeOnly"/>
    public static TimeOnly BetweenTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly min,
        TimeOnly max,
        Inclusion inclusion = Inclusion.Inclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotBetweenTimeOnly(value, min, max, inclusion, styles, paramName); // Guard.Against.BetweenTimeOnly => Must.Be.NotBetweenTimeOnly (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotWithinTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="reference">The reference value to measure from.</param>
    /// <param name="window">The time window for proximity.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.WithinTimeOnly"/>
    public static TimeOnly NotWithinTimeOnly(this IGuardClause _,
        string? value,
        string reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.WithinTimeOnly(value, reference, window, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the WithinTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="reference">The reference value to measure from.</param>
    /// <param name="window">The time window for proximity.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotWithinTimeOnly"/>
    public static TimeOnly WithinTimeOnly(this IGuardClause _,
        string? value,
        string reference,
        TimeSpan window,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotWithinTimeOnly(value, reference, window, styles, paramName); // Guard.Against.WithinTimeOnly => Must.Be.NotWithinTimeOnly (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotBeforeTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.BeforeTimeOnly"/>
    public static TimeOnly NotBeforeTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.BeforeTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotOnOrBeforeTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.OnOrBeforeTimeOnly"/>
    public static TimeOnly NotOnOrBeforeTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.OnOrBeforeTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotAfterTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.AfterTimeOnly"/>
    public static TimeOnly NotAfterTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.AfterTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotOnOrAfterTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.OnOrAfterTimeOnly"/>
    public static TimeOnly NotOnOrAfterTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.OnOrAfterTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the NotSameTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.SameTimeOnly"/>
    public static TimeOnly NotSameTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.SameTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the SameTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotSameTimeOnly"/>
    public static TimeOnly SameTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotSameTimeOnly(value, other, precision, styles, paramName); // Guard.Against.SameTimeOnly => Must.Be.NotSameTimeOnly (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the BeforeTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotBeforeTimeOnly"/>
    public static TimeOnly BeforeTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotBeforeTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the OnOrBeforeTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotOnOrBeforeTimeOnly"/>
    public static TimeOnly OnOrBeforeTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotOnOrBeforeTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the AfterTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotAfterTimeOnly"/>
    public static TimeOnly AfterTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotAfterTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> violates the OnOrAfterTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="precision">The comparison precision.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotOnOrAfterTimeOnly"/>
    public static TimeOnly OnOrAfterTimeOnly(this IGuardClause _,
        string? value,
        TimeOnly other,
        TimePrecision? precision = null,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotOnOrAfterTimeOnly(value, other, precision, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="start"/> violates the NotChronologicalTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="start">The start of the range or interval.</param>
    /// <param name="end">The end of the range or interval.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.ChronologicalTimeOnly"/>
    public static TimeOnly NotChronologicalTimeOnly(this IGuardClause _,
        string start,
        string end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        var result = Must.Be.ChronologicalTimeOnly(start, end, inclusion, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="start"/> violates the ChronologicalTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="start">The start of the range or interval.</param>
    /// <param name="end">The end of the range or interval.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotChronologicalTimeOnly"/>
    public static TimeOnly ChronologicalTimeOnly(this IGuardClause _,
        string start,
        string end,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(start))] string? paramName = null)
    {
        var result = Must.Be.NotChronologicalTimeOnly(start, end, inclusion, styles, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="start1"/> violates the OverlappingTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="start1">The start of the first range.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.NotOverlappingTimeOnly"/>
    public static TimeOnly OverlappingTimeOnly(this IGuardClause _,
        string start1,
        string end1,
        string start2,
        string end2,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        var result = Must.Be.NotOverlappingTimeOnly(start1, end1, start2, end2, inclusion, styles, paramName); // Guard.Against.OverlappingTimeOnly => Must.Be.NotOverlappingTimeOnly (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="start1"/> violates the NotOverlappingTimeOnly constraint.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="start1">The start of the first range.</param>
    /// <param name="end1">The end of the first range.</param>
    /// <param name="start2">The start of the second range.</param>
    /// <param name="end2">The end of the second range.</param>
    /// <param name="inclusion">The boundary inclusion mode.</param>
    /// <param name="styles">The number or date-time parsing styles.</param>
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
    /// <seealso cref="MustStringTimeOnlyClauses.OverlappingTimeOnly"/>
    public static TimeOnly NotOverlappingTimeOnly(this IGuardClause _,
        string start1,
        string end1,
        string start2,
        string end2,
        Inclusion inclusion = Inclusion.Exclusive,
        DateTimeStyles styles = DefaultStyles,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(start1))] string? paramName = null)
    {
        var result = Must.Be.OverlappingTimeOnly(start1, end1, start2, end2, inclusion, styles, paramName); // Guard.Against.NotOverlappingTimeOnly => Must.Be.OverlappingTimeOnly (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
#endif
