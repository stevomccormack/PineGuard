using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for decimal shape (precision and scale) validation.
/// </summary>
/// <remarks>
/// Precision and scale are read as a <c>decimal(p, s)</c> column reads them: <c>scale</c> is the number
/// of digits after the decimal point and <c>precision</c> is the total number of stored digits. Trailing
/// zeros are not stored digits, so <c>1.500m</c> is shaped exactly like <c>1.5m</c>.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/fluent/decimal">Fluent Decimal Extensions documentation</seealso>
public static class FluentDecimalExtensions
{
    /// <summary>
    /// Validates that the property value has no more than <paramref name="scale"/> digits after the decimal point.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="scale">
    /// The maximum number of digits allowed after the decimal point, between <c>0</c> and
    /// <see cref="DecimalRules.MaxScale"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDecimalClauses.ScaleAtMost"/>. Trailing zeros are ignored, so <c>1.500m</c>
    /// has a scale of <c>1</c>. A <paramref name="scale"/> outside <c>0</c>–<see cref="DecimalRules.MaxScale"/>
    /// fails the rule with the Must clause's configuration message, since that is programmer misuse rather than
    /// bad input. If the value is <see langword="null"/>, validation passes (null values should be handled by a
    /// separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Price).ScaleAtMost(2);
    /// </code>
    /// </example>
    /// <seealso cref="MustDecimalClauses.ScaleAtMost"/>
    public static IRuleBuilderOptions<TModel, decimal?> ScaleAtMost<TModel>(this IRuleBuilder<TModel, decimal?> ruleBuilder,
        int scale,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.ScaleAtMost(val.Value, scale, paramName: null) : MustResult<decimal>.Ok(default),
            message, MustCodes.Number.Scale.Exceeded);

    /// <summary>
    /// Validates that the property value has no more than <paramref name="precision"/> significant digits.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="precision">
    /// The maximum number of digits allowed in total, between <c>1</c> and
    /// <see cref="DecimalRules.MaxPrecision"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDecimalClauses.PrecisionAtMost"/>. Trailing zeros are ignored, so <c>1.500m</c>
    /// has a precision of <c>2</c>. A <paramref name="precision"/> outside
    /// <c>1</c>–<see cref="DecimalRules.MaxPrecision"/> fails the rule with the Must clause's configuration
    /// message, since that is programmer misuse rather than bad input. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Amount).PrecisionAtMost(18);
    /// </code>
    /// </example>
    /// <seealso cref="MustDecimalClauses.PrecisionAtMost"/>
    public static IRuleBuilderOptions<TModel, decimal?> PrecisionAtMost<TModel>(this IRuleBuilder<TModel, decimal?> ruleBuilder,
        int precision,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.PrecisionAtMost(val.Value, precision, paramName: null) : MustResult<decimal>.Ok(default),
            message, MustCodes.Number.Precision.Exceeded);

    /// <summary>
    /// Validates that the property value fits a <c>decimal(<paramref name="precision"/>, <paramref name="scale"/>)</c> budget.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="precision">
    /// The total number of digits the budget allows, between <c>1</c> and <see cref="DecimalRules.MaxPrecision"/>.
    /// </param>
    /// <param name="scale">
    /// The number of those digits the budget allows after the decimal point, between <c>0</c> and
    /// <see cref="DecimalRules.MaxScale"/>, and never greater than <paramref name="precision"/>.
    /// </param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDecimalClauses.WithinPrecision"/>. The budget is spent the way a database
    /// column spends it: at most <paramref name="scale"/> digits after the decimal point, and at most
    /// <c>precision - scale</c> digits before it. So <c>123.4m</c> fits <c>decimal(18, 2)</c> but not
    /// <c>decimal(5, 3)</c>. An unusable budget fails the rule with the Must clause's configuration message,
    /// since that is programmer misuse rather than bad input. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Total).WithinPrecision(18, 2);
    /// </code>
    /// </example>
    /// <seealso cref="MustDecimalClauses.WithinPrecision"/>
    public static IRuleBuilderOptions<TModel, decimal?> WithinPrecision<TModel>(this IRuleBuilder<TModel, decimal?> ruleBuilder,
        int precision,
        int scale,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.WithinPrecision(val.Value, precision, scale, paramName: null) : MustResult<decimal>.Ok(default),
            message, MustCodes.Number.Precision.OutOfRange);
}
