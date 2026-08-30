using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate decimal shape (precision and scale),
/// delegating to <see cref="DecimalRules"/> for core validation logic.
/// </summary>
/// <remarks>
/// Precision and scale are read as a <c>decimal(p, s)</c> column reads them: <c>scale</c> is the number
/// of digits after the decimal point and <c>precision</c> is the total number of stored digits. Trailing
/// zeros are not stored digits, so <c>1.500m</c> is shaped exactly like <c>1.5m</c>.
/// </remarks>
/// <seealso cref="DecimalRules"/>
/// <seealso href="https://pineguard.ai/docs/must/decimal">Decimal Must Clauses documentation</seealso>
public static class MustDecimalClauses
{
    /// <summary>
    /// Validates that the specified value must have no more than <paramref name="scale"/> digits after the decimal point.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="scale">
    /// The maximum number of digits allowed after the decimal point, between <c>0</c> and
    /// <see cref="DecimalRules.MaxScale"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// Returns a failure attributed to <paramref name="scale"/> when it falls outside
    /// <c>0</c>–<see cref="DecimalRules.MaxScale"/>, since that is programmer misuse rather than bad input.
    /// Trailing zeros are ignored, so <c>1.500m</c> has a scale of <c>1</c>.
    /// The failure message follows the pattern <c>"{paramName} must have no more than the allowed number of decimal places."</c>
    /// </remarks>
    /// <seealso cref="DecimalRules.HasMaxScale"/>
    /// <seealso href="https://pineguard.ai/docs/must/decimal">Decimal Must Clauses documentation</seealso>
    public static MustResult<decimal> ScaleAtMost(this IMustClause _,
        decimal value,
        int scale,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (scale is < 0 or > DecimalRules.MaxScale)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Invalid, "{paramName} requires a value between 0 and 28.", nameof(scale), scale);

        const string messageTemplate = "{paramName} must have no more than the allowed number of decimal places.";

        var ok = DecimalRules.HasMaxScale(value, scale);
        return MustResult<decimal>.FromBool(ok, MustCodes.Number.Scale.Exceeded, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must have no more than <paramref name="precision"/> significant digits.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="precision">
    /// The maximum number of digits allowed in total, between <c>1</c> and
    /// <see cref="DecimalRules.MaxPrecision"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// Returns a failure attributed to <paramref name="precision"/> when it falls outside
    /// <c>1</c>–<see cref="DecimalRules.MaxPrecision"/>, since that is programmer misuse rather than bad input.
    /// Trailing zeros are ignored, so <c>1.500m</c> has a precision of <c>2</c>.
    /// The failure message follows the pattern <c>"{paramName} must have no more than the allowed number of digits."</c>
    /// </remarks>
    /// <seealso cref="DecimalRules.HasMaxPrecision"/>
    /// <seealso href="https://pineguard.ai/docs/must/decimal">Decimal Must Clauses documentation</seealso>
    public static MustResult<decimal> PrecisionAtMost(this IMustClause _,
        decimal value,
        int precision,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is < 1 or > DecimalRules.MaxPrecision)
            return MustResult<decimal>.Fail(MustCodes.Number.Precision.Invalid, "{paramName} requires a value between 1 and 29.", nameof(precision), precision);

        const string messageTemplate = "{paramName} must have no more than the allowed number of digits.";

        var ok = DecimalRules.HasMaxPrecision(value, precision);
        return MustResult<decimal>.FromBool(ok, MustCodes.Number.Precision.Exceeded, messageTemplate, paramName, value, value);
    }

    /// <summary>
    /// Validates that the specified value must fit a <c>decimal(<paramref name="precision"/>, <paramref name="scale"/>)</c> budget.
    /// </summary>
    /// <param name="_">The <see cref="IMustClause"/> entry point (used via <c>Must.Be</c>).</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="precision">
    /// The total number of digits the budget allows, between <c>1</c> and <see cref="DecimalRules.MaxPrecision"/>.
    /// </param>
    /// <param name="scale">
    /// The number of those digits the budget allows after the decimal point, between <c>0</c> and
    /// <see cref="DecimalRules.MaxScale"/>, and never greater than <paramref name="precision"/>.
    /// </param>
    /// <param name="paramName">
    /// The name of the calling parameter. Automatically captured via
    /// <see cref="CallerArgumentExpressionAttribute"/> — do not pass explicitly.
    /// </param>
    /// <returns>
    /// A <see cref="MustResult{T}"/> indicating whether validation succeeded.
    /// </returns>
    /// <remarks>
    /// The budget is spent the way a database column spends it: at most <paramref name="scale"/> digits after
    /// the decimal point, and at most <c>precision - scale</c> digits before it. So <c>123.4m</c> fits
    /// <c>decimal(18, 2)</c> but not <c>decimal(5, 3)</c>. Returns a failure attributed to the offending
    /// configuration parameter when the budget itself is unusable, since that is programmer misuse rather
    /// than bad input.
    /// The failure message follows the pattern <c>"{paramName} must fit within the allowed precision and scale."</c>
    /// </remarks>
    /// <seealso cref="DecimalRules.IsWithinPrecision"/>
    /// <seealso href="https://pineguard.ai/docs/must/decimal">Decimal Must Clauses documentation</seealso>
    public static MustResult<decimal> WithinPrecision(this IMustClause _,
        decimal value,
        int precision,
        int scale,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (precision is < 1 or > DecimalRules.MaxPrecision)
            return MustResult<decimal>.Fail(MustCodes.Number.Precision.Invalid, "{paramName} requires a value between 1 and 29.", nameof(precision), precision);

        if (scale is < 0 or > DecimalRules.MaxScale)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Invalid, "{paramName} requires a value between 0 and 28.", nameof(scale), scale);

        if (scale > precision)
            return MustResult<decimal>.Fail(MustCodes.Number.Scale.Invalid, "{paramName} requires a value no greater than the precision.", nameof(scale), scale);

        const string messageTemplate = "{paramName} must fit within the allowed precision and scale.";

        var ok = DecimalRules.IsWithinPrecision(value, precision, scale);
        return MustResult<decimal>.FromBool(ok, MustCodes.Number.Precision.OutOfRange, messageTemplate, paramName, value, value);
    }
}
