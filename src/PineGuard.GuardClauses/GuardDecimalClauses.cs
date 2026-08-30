using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for decimal shape (precision and scale).
/// </summary>
/// <remarks>
/// Precision and scale are read as a <c>decimal(p, s)</c> column reads them: <c>scale</c> is the number
/// of digits after the decimal point and <c>precision</c> is the total number of stored digits. Trailing
/// zeros are not stored digits, so <c>1.500m</c> is shaped exactly like <c>1.5m</c>.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/guard/decimal">Guard Decimal Clauses documentation</seealso>
public static class GuardDecimalClauses
{
    /// <summary>
    /// Throws if <paramref name="value"/> has more than <paramref name="scale"/> digits after the decimal point.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="scale">
    /// The maximum number of digits allowed after the decimal point, between <c>0</c> and <c>28</c>.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDecimalClauses.ScaleAtMost"/>.
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
    /// Thrown when <paramref name="value"/> has too many decimal places, or when
    /// <paramref name="scale"/> itself is outside <c>0</c>–<c>28</c>, and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// Trailing zeros are ignored, so <c>1.500m</c> has a scale of <c>1</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.ScaleAbove(price, 2);
    /// </code>
    /// </example>
    /// <seealso cref="MustDecimalClauses.ScaleAtMost"/>
    public static decimal ScaleAbove(this IGuardClause _,
        decimal value,
        int scale,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.ScaleAtMost(value, scale, paramName); // Guard.Against.ScaleAbove => Must.Be.ScaleAtMost (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> has more than <paramref name="precision"/> significant digits.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="precision">
    /// The maximum number of digits allowed in total, between <c>1</c> and <c>29</c>.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDecimalClauses.PrecisionAtMost"/>.
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
    /// Thrown when <paramref name="value"/> has too many digits, or when
    /// <paramref name="precision"/> itself is outside <c>1</c>–<c>29</c>, and no
    /// <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// Trailing zeros are ignored, so <c>1.500m</c> has a precision of <c>2</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.PrecisionAbove(amount, 18);
    /// </code>
    /// </example>
    /// <seealso cref="MustDecimalClauses.PrecisionAtMost"/>
    public static decimal PrecisionAbove(this IGuardClause _,
        decimal value,
        int precision,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.PrecisionAtMost(value, precision, paramName); // Guard.Against.PrecisionAbove => Must.Be.PrecisionAtMost (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    /// <summary>
    /// Throws if <paramref name="value"/> does not fit a
    /// <c>decimal(<paramref name="precision"/>, <paramref name="scale"/>)</c> budget.
    /// </summary>
    /// <param name="_">The <see cref="IGuardClause"/> entry point (used via <c>Guard.Against</c>).</param>
    /// <param name="value">The value to guard.</param>
    /// <param name="precision">
    /// The total number of digits the budget allows, between <c>1</c> and <c>29</c>.
    /// </param>
    /// <param name="scale">
    /// The number of those digits the budget allows after the decimal point, between <c>0</c> and
    /// <c>28</c>, and never greater than <paramref name="precision"/>.
    /// </param>
    /// <param name="message">
    /// An optional custom error message. If <see langword="null"/>, uses the default message
    /// from <see cref="MustDecimalClauses.WithinPrecision"/>.
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
    /// Thrown when <paramref name="value"/> misses the budget, or when the budget itself is
    /// unusable, and no <paramref name="exceptionCreator"/> is provided.
    /// </exception>
    /// <remarks>
    /// The budget is spent the way a database column spends it: at most <paramref name="scale"/> digits after
    /// the decimal point, and at most <c>precision - scale</c> digits before it. So <c>123.4m</c> fits
    /// <c>decimal(18, 2)</c> but not <c>decimal(5, 3)</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// Guard.Against.NotWithinPrecision(total, 18, 2);
    /// </code>
    /// </example>
    /// <seealso cref="MustDecimalClauses.WithinPrecision"/>
    public static decimal NotWithinPrecision(this IGuardClause _,
        decimal value,
        int precision,
        int scale,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.WithinPrecision(value, precision, scale, paramName); // Guard.Against.NotWithinPrecision => Must.Be.WithinPrecision (complement)
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
