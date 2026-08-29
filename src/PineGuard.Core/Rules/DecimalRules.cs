using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure decimal shape validation predicates (precision and scale).
/// </summary>
/// <remarks>
/// Precision and scale are read as a <c>decimal(p, s)</c> column reads them: <c>scale</c> is the number
/// of digits after the decimal point and <c>precision</c> is the total number of stored digits. Trailing
/// zeros are not stored digits, so <c>1.500m</c> is shaped exactly like <c>1.5m</c>.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/decimal">Decimal Rules documentation</seealso>
public static class DecimalRules
{
    /// <summary>
    /// The largest precision a <see cref="decimal"/> can carry (29 significant digits).
    /// </summary>
    public const int MaxPrecision = 29;

    /// <summary>
    /// The largest scale a <see cref="decimal"/> can carry (28 digits after the decimal point).
    /// </summary>
    public const int MaxScale = 28;

    /// <summary>
    /// Determines whether the specified value has no more than <paramref name="scale"/> digits after the decimal point.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="scale">
    /// The maximum number of digits allowed after the decimal point. Outside <c>0</c>–<see cref="MaxScale"/>,
    /// returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the scale of <paramref name="value"/> is at most <paramref name="scale"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasMaxScale(decimal? value, int scale) =>
        scale is >= 0 and <= MaxScale
        && DecimalUtility.TryGetPrecisionAndScale(value, out _, out var actualScale)
        && actualScale <= scale;

    /// <summary>
    /// Determines whether the specified value has no more than <paramref name="precision"/> significant digits.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="precision">
    /// The maximum number of digits allowed in total. Outside <c>1</c>–<see cref="MaxPrecision"/>,
    /// returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the precision of <paramref name="value"/> is at most <paramref name="precision"/>;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public static bool HasMaxPrecision(decimal? value, int precision) =>
        precision is >= 1 and <= MaxPrecision
        && DecimalUtility.TryGetPrecisionAndScale(value, out var actualPrecision, out _)
        && actualPrecision <= precision;

    /// <summary>
    /// Determines whether the specified value fits a <c>decimal(<paramref name="precision"/>, <paramref name="scale"/>)</c> budget.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="precision">
    /// The total number of digits the budget allows. Outside <c>1</c>–<see cref="MaxPrecision"/>,
    /// or smaller than <paramref name="scale"/>, returns <see langword="false"/>.
    /// </param>
    /// <param name="scale">
    /// The number of those digits the budget allows after the decimal point. Outside
    /// <c>0</c>–<see cref="MaxScale"/>, returns <see langword="false"/>.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> fits the budget; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// The budget is spent the way a database column spends it: at most <paramref name="scale"/> digits after
    /// the decimal point, and at most <c>precision - scale</c> digits before it. So <c>123.4m</c> fits
    /// <c>decimal(18, 2)</c> but not <c>decimal(5, 3)</c>, which leaves room for only two integral digits.
    /// </remarks>
    public static bool IsWithinPrecision(decimal? value, int precision, int scale) =>
        precision is >= 1 and <= MaxPrecision
        && scale is >= 0 and <= MaxScale
        && scale <= precision
        && DecimalUtility.TryGetPrecisionAndScale(value, out var actualPrecision, out var actualScale)
        && actualScale <= scale
        && actualPrecision - actualScale <= precision - scale;
}
