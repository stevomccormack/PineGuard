using System.Globalization;
using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for numeric validation of string-encoded numbers.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-numbers">Fluent String Numbers Extensions documentation</seealso>
public static class FluentStringNumbersExtensions
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is positive (greater than zero).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.PositiveDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Amount).PositiveDecimal();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.PositiveDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> PositiveDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.PositiveDecimal(val, styles, paramName: null)),
            message, MustCodes.Number.Sign.NotPositive);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is negative (less than zero).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NegativeDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Temperature).NegativeDecimal();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NegativeDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> NegativeDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NegativeDecimal(val, styles, paramName: null)),
            message, MustCodes.Number.Sign.NotNegative);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, equals zero.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.ZeroDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Balance).ZeroDecimal();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.ZeroDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> ZeroDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.ZeroDecimal(val, styles, paramName: null)),
            message, MustCodes.Number.Sign.NotZero);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, does not equal zero.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotZeroDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Divisor).NotZeroDecimal();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotZeroDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> NotZeroDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotZeroDecimal(val, styles, paramName: null)),
            message, MustCodes.Number.Sign.Zero);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is zero or positive.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.ZeroOrPositiveDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).ZeroOrPositiveDecimal();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.ZeroOrPositiveDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> ZeroOrPositiveDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.ZeroOrPositiveDecimal(val, styles, paramName: null)),
            message, MustCodes.Number.Sign.Negative);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is zero or negative.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.ZeroOrNegativeDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Offset).ZeroOrNegativeDecimal();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.ZeroOrNegativeDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> ZeroOrNegativeDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.ZeroOrNegativeDecimal(val, styles, paramName: null)),
            message, MustCodes.Number.Sign.Positive);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is greater than the specified minimum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The exclusive lower bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.GreaterThanDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Price).GreaterThanDecimal(0);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.GreaterThanDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> GreaterThanDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal other,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.GreaterThanDecimal(val, other, styles, paramName: null)),
            message, MustCodes.Number.Range.NotGreater);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The inclusive lower bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.GreaterThanOrEqualDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.MinimumAge).GreaterThanOrEqualDecimal(18);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.GreaterThanOrEqualDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> GreaterThanOrEqualDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal other,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.GreaterThanOrEqualDecimal(val, other, styles, paramName: null)),
            message, MustCodes.Number.Range.BelowMinimum);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is less than the specified maximum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The exclusive upper bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.LessThanDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Percentage).LessThanDecimal(100);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.LessThanDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> LessThanDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal other,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.LessThanDecimal(val, other, styles, paramName: null)),
            message, MustCodes.Number.Range.NotLess);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The inclusive upper bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.LessThanOrEqualDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.MaxDiscount).LessThanOrEqualDecimal(50);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.LessThanOrEqualDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> LessThanOrEqualDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal other,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.LessThanOrEqualDecimal(val, other, styles, paramName: null)),
            message, MustCodes.Number.Range.Exceeded);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, falls within the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the range.</param>
    /// <param name="max">The upper bound of the range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.InRangeDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Score).InRangeDecimal(0, 100);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.InRangeDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> InRangeDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal min,
        decimal max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.InRangeDecimal(val, min, max, inclusion, styles, paramName: null)),
            message, MustCodes.Number.Range.OutOfRange);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, falls outside the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.OutOfRangeDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ExcludedValue).OutOfRangeDecimal(1, 10);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.OutOfRangeDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> OutOfRangeDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal min,
        decimal max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OutOfRangeDecimal(val, min, max, inclusion, styles, paramName: null)),
            message, MustCodes.Number.Range.InRange);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is a percentage between 0 and 100.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.PercentageDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DiscountRate).PercentageDecimal();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.PercentageDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> PercentageDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.PercentageDecimal(val, styles, paramName: null)),
            message, MustCodes.Number.Range.NotPercentage);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is approximately equal to the target within the specified tolerance.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="target">The expected approximate value.</param>
    /// <param name="tolerance">The maximum allowed deviation from the target, or <see langword="null"/> to use the default tolerance.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.ApproximatelyDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Measurement).ApproximatelyDecimal(3.14m, 0.01m);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.ApproximatelyDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> ApproximatelyDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal target,
        decimal? tolerance,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.ApproximatelyDecimal(val, target, tolerance, styles, paramName: null)),
            message, MustCodes.Number.Proximity.NotApproximate);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is not approximately equal to the target within the specified tolerance.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="target">The value to compare against.</param>
    /// <param name="tolerance">The maximum allowed deviation from the target, or <see langword="null"/> to use the default tolerance.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotApproximatelyDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Measurement).NotApproximatelyDecimal(0, 0.001m);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotApproximatelyDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> NotApproximatelyDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal target,
        decimal? tolerance,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotApproximatelyDecimal(val, target, tolerance, styles, paramName: null)),
            message, MustCodes.Number.Proximity.Approximate);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is a multiple of the specified factor.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="factor">The factor that the value must be a multiple of.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.MultipleOfDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).MultipleOfDecimal(5);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.MultipleOfDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> MultipleOfDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal factor,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.MultipleOfDecimal(val, factor, styles, paramName: null)),
            message, MustCodes.Number.Divisibility.NotMultiple);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is not a multiple of the specified factor.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="factor">The factor that the value must not be a multiple of.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotMultipleOfDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).NotMultipleOfDecimal(3);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotMultipleOfDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> NotMultipleOfDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal factor,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotMultipleOfDecimal(val, factor, styles, paramName: null)),
            message, MustCodes.Number.Divisibility.Multiple);

    /// <summary>
    /// Validates that the property value, parsed as an integer, is even.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.EvenInt32"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).EvenInt32();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.EvenInt32"/>
    public static IRuleBuilderOptions<TModel, string?> EvenInt32<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.EvenInt32(val, styles, paramName: null)),
            message, MustCodes.Number.Parity.Odd);

    /// <summary>
    /// Validates that the property value, parsed as an integer, is odd.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.OddInt32"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).OddInt32();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.OddInt32"/>
    public static IRuleBuilderOptions<TModel, string?> OddInt32<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OddInt32(val, styles, paramName: null)),
            message, MustCodes.Number.Parity.Even);

    /// <summary>
    /// Validates that the property value, parsed as a floating-point number, is finite (not infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.FiniteDouble"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Ratio).FiniteDouble();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.FiniteDouble"/>
    public static IRuleBuilderOptions<TModel, string?> FiniteDouble<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.FiniteDouble(val, styles, paramName: null)),
            message, MustCodes.Number.Form.NotFinite);

    /// <summary>
    /// Validates that the property value, parsed as a floating-point number, is not finite (is infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotFiniteDouble"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotFiniteDouble();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotFiniteDouble"/>
    public static IRuleBuilderOptions<TModel, string?> NotFiniteDouble<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotFiniteDouble(val, styles, paramName: null)),
            message, MustCodes.Number.Form.Finite);

    /// <summary>
    /// Validates that the property value, parsed as a floating-point number, is not NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotNaNDouble"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Coefficient).NotNaNDouble();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotNaNDouble"/>
    public static IRuleBuilderOptions<TModel, string?> NotNaNDouble<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.NotNaNDouble(val, styles, paramName: null)),
            message, MustCodes.Number.Form.Nan);

    private static MustResult<T> SkipAbsentValue<T>(string? value, MustResult<T> result) =>
        value is null && result.ParamName is null ? MustResult<T>.Ok(default) : result;
}
