#if NET8_0_OR_GREATER
using System.Globalization;
using FluentValidation;
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
    /// Delegates to <see cref="MustStringNumbersClauses.Positive"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Amount).Positive();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.Positive"/>
    public static IRuleBuilderOptions<TModel, string?> Positive<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Positive(val, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is negative (less than zero).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.Negative"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Temperature).Negative();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.Negative"/>
    public static IRuleBuilderOptions<TModel, string?> Negative<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Negative(val, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, equals zero.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.Zero"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Balance).Zero();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.Zero"/>
    public static IRuleBuilderOptions<TModel, string?> Zero<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Zero(val, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, does not equal zero.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotZero"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Divisor).NotZero();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotZero"/>
    public static IRuleBuilderOptions<TModel, string?> NotZero<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotZero(val, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is zero or positive.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.ZeroOrPositive"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).ZeroOrPositive();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.ZeroOrPositive"/>
    public static IRuleBuilderOptions<TModel, string?> ZeroOrPositive<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ZeroOrPositive(val, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is zero or negative.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.ZeroOrNegative"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Offset).ZeroOrNegative();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.ZeroOrNegative"/>
    public static IRuleBuilderOptions<TModel, string?> ZeroOrNegative<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ZeroOrNegative(val, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is greater than the specified minimum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The exclusive lower bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.GreaterThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Price).GreaterThan(0);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.GreaterThan"/>
    public static IRuleBuilderOptions<TModel, string?> GreaterThan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal min,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.GreaterThan(val, min, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is greater than or equal to the specified minimum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The inclusive lower bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.GreaterThanOrEqual"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.MinimumAge).GreaterThanOrEqual(18);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.GreaterThanOrEqual"/>
    public static IRuleBuilderOptions<TModel, string?> GreaterThanOrEqual<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal min,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.GreaterThanOrEqual(val, min, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is less than the specified maximum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="max">The exclusive upper bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.LessThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Percentage).LessThan(100);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.LessThan"/>
    public static IRuleBuilderOptions<TModel, string?> LessThan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal max,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.LessThan(val, max, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a decimal number, is less than or equal to the specified maximum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="max">The inclusive upper bound.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.LessThanOrEqual"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.MaxDiscount).LessThanOrEqual(50);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.LessThanOrEqual"/>
    public static IRuleBuilderOptions<TModel, string?> LessThanOrEqual<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal max,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.LessThanOrEqual(val, max, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

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
    /// Delegates to <see cref="MustStringNumbersClauses.InRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Score).InRange(0, 100);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.InRange"/>
    public static IRuleBuilderOptions<TModel, string?> InRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal min,
        decimal max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.InRange(val, min, max, inclusion, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

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
    /// Delegates to <see cref="MustStringNumbersClauses.OutOfRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ExcludedValue).OutOfRange(1, 10);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.OutOfRange"/>
    public static IRuleBuilderOptions<TModel, string?> OutOfRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal min,
        decimal max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.OutOfRange(val, min, max, inclusion, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

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
    /// Delegates to <see cref="MustStringNumbersClauses.Approximately"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Measurement).Approximately(3.14m, 0.01m);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.Approximately"/>
    public static IRuleBuilderOptions<TModel, string?> Approximately<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal target,
        decimal? tolerance,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Approximately(val, target, tolerance, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

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
    /// Delegates to <see cref="MustStringNumbersClauses.NotApproximately"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Measurement).NotApproximately(0, 0.001m);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotApproximately"/>
    public static IRuleBuilderOptions<TModel, string?> NotApproximately<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal target,
        decimal? tolerance,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotApproximately(val, target, tolerance, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

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
    /// Delegates to <see cref="MustStringNumbersClauses.MultipleOf"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).MultipleOf(5);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.MultipleOf"/>
    public static IRuleBuilderOptions<TModel, string?> MultipleOf<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal factor,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.MultipleOf(val, factor, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

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
    /// Delegates to <see cref="MustStringNumbersClauses.NotMultipleOf"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).NotMultipleOf(3);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotMultipleOf"/>
    public static IRuleBuilderOptions<TModel, string?> NotMultipleOf<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        decimal factor,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotMultipleOf(val, factor, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as an integer, is even.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.Even"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).Even();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.Even"/>
    public static IRuleBuilderOptions<TModel, string?> Even<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Even(val, styles, paramName: null) : MustResult<int>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as an integer, is odd.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.Odd"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).Odd();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.Odd"/>
    public static IRuleBuilderOptions<TModel, string?> Odd<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Odd(val, styles, paramName: null) : MustResult<int>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a floating-point number, is finite (not infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.Finite"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Ratio).Finite();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.Finite"/>
    public static IRuleBuilderOptions<TModel, string?> Finite<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Finite(val, styles, paramName: null) : MustResult<double>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a floating-point number, is not finite (is infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotFinite"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Value).NotFinite();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotFinite"/>
    public static IRuleBuilderOptions<TModel, string?> NotFinite<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotFinite(val, styles, paramName: null) : MustResult<double>.Ok(0),
            message);

    /// <summary>
    /// Validates that the property value, parsed as a floating-point number, is not NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumbersClauses.NotNaN"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Coefficient).NotNaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumbersClauses.NotNaN"/>
    public static IRuleBuilderOptions<TModel, string?> NotNaN<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotNaN(val, styles, paramName: null) : MustResult<double>.Ok(0),
            message);
}
#endif
