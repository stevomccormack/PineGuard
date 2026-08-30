#if NET8_0_OR_GREATER
using System.Numerics;
using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for numeric value validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/number">Fluent Number Extensions documentation</seealso>
public static class FluentNumberExtensions
{
    /// <summary>
    /// Validates that the property value is positive (greater than zero).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Positive"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Amount).Positive();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Positive"/>
    public static IRuleBuilderOptions<TModel, T?> Positive<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Positive(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Sign.NotPositive);

    /// <summary>
    /// Validates that the property value is negative (less than zero).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Negative"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Temperature).Negative();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Negative"/>
    public static IRuleBuilderOptions<TModel, T?> Negative<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Negative(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Sign.NotNegative);

    /// <summary>
    /// Validates that the property value equals zero.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Zero"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Balance).Zero();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Zero"/>
    public static IRuleBuilderOptions<TModel, T?> Zero<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Zero(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Sign.NotZero);

    /// <summary>
    /// Validates that the property value does not equal zero.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotZero"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Divisor).NotZero();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotZero"/>
    public static IRuleBuilderOptions<TModel, T?> NotZero<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotZero(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Sign.Zero);

    /// <summary>
    /// Validates that the property value is zero or positive.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.ZeroOrPositive"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).ZeroOrPositive();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.ZeroOrPositive"/>
    public static IRuleBuilderOptions<TModel, T?> ZeroOrPositive<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.ZeroOrPositive(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Sign.Negative);

    /// <summary>
    /// Validates that the property value is zero or negative.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.ZeroOrNegative"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Offset).ZeroOrNegative();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.ZeroOrNegative"/>
    public static IRuleBuilderOptions<TModel, T?> ZeroOrNegative<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.ZeroOrNegative(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Sign.Positive);

    /// <summary>
    /// Validates that the property value falls within the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the allowed range.</param>
    /// <param name="max">The upper bound of the allowed range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.InRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Score).InRange(0, 100);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.InRange"/>
    public static IRuleBuilderOptions<TModel, T?> InRange<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T min,
        T max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null)
        where T : struct, IComparable<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.InRange(val.Value, min, max, inclusion, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Range.OutOfRange);

    /// <summary>
    /// Validates that the property value falls outside the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The comparable value type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the excluded range.</param>
    /// <param name="max">The upper bound of the excluded range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.OutOfRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ExcludedValue).OutOfRange(1, 10);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.OutOfRange"/>
    public static IRuleBuilderOptions<TModel, T?> OutOfRange<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T min,
        T max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null)
        where T : struct, IComparable<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.OutOfRange(val.Value, min, max, inclusion, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Range.InRange);

    /// <summary>
    /// Validates that the property value is a percentage between 0 and 100.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Percentage"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DiscountRate).Percentage();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Percentage"/>
    public static IRuleBuilderOptions<TModel, T?> Percentage<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Percentage(val.Value, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Range.NotPercentage);

    /// <summary>
    /// Validates that the property value is approximately equal to the target within the specified tolerance.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="target">The expected approximate value.</param>
    /// <param name="tolerance">The maximum allowed deviation from the target, or <see langword="null"/> to use the default tolerance.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Approximately"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Pi).Approximately(3.14m, 0.01m);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Approximately"/>
    public static IRuleBuilderOptions<TModel, T?> Approximately<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T target,
        T? tolerance,
        string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Approximately(val.Value, target, tolerance, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Proximity.NotApproximate);

    /// <summary>
    /// Validates that the property value is not approximately equal to the target within the specified tolerance.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="target">The value to compare against.</param>
    /// <param name="tolerance">The maximum allowed deviation, or <see langword="null"/> to use the default tolerance.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotApproximately"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Coefficient).NotApproximately(0, 0.001m);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotApproximately"/>
    public static IRuleBuilderOptions<TModel, T?> NotApproximately<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T target,
        T? tolerance,
        string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotApproximately(val.Value, target, tolerance, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Proximity.Approximate);

    /// <summary>
    /// Validates that the property value is a multiple of the specified factor.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="factor">The factor that the value must be a multiple of.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.MultipleOf"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Quantity).MultipleOf(5);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.MultipleOf"/>
    public static IRuleBuilderOptions<TModel, T?> MultipleOf<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T factor,
        string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.MultipleOf(val.Value, factor, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Divisibility.NotMultiple);

    /// <summary>
    /// Validates that the property value is not a multiple of the specified factor.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The numeric type, which must implement <see cref="INumber{T}"/>.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="factor">The factor that the value must not be a multiple of.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotMultipleOf"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).NotMultipleOf(3);
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotMultipleOf"/>
    public static IRuleBuilderOptions<TModel, T?> NotMultipleOf<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T factor,
        string? message = null)
        where T : struct, INumber<T> =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotMultipleOf(val.Value, factor, paramName: null) : MustResult<T>.Ok(default),
            message, MustCodes.Number.Divisibility.Multiple);

    /// <summary>
    /// Validates that the nullable <see cref="int"/> property value is even.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Even(IMustClause, int, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).Even();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Even(IMustClause, int, string)"/>
    public static IRuleBuilderOptions<TModel, int?> Even<TModel>(
        this IRuleBuilder<TModel, int?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Even(val.Value, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Number.Parity.Odd);

    /// <summary>
    /// Validates that the non-nullable <see cref="int"/> property value is even.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Even(IMustClause, int, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).Even();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Even(IMustClause, int, string)"/>
    public static IRuleBuilderOptions<TModel, int> Even<TModel>(
        this IRuleBuilder<TModel, int> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Even(val, paramName: null),
            message, MustCodes.Number.Parity.Odd);

    /// <summary>
    /// Validates that the nullable <see cref="long"/> property value is even.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Even(IMustClause, long, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LargeCount).Even();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Even(IMustClause, long, string)"/>
    public static IRuleBuilderOptions<TModel, long?> Even<TModel>(
        this IRuleBuilder<TModel, long?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Even(val.Value, paramName: null) : MustResult<long>.Ok(0),
            message, MustCodes.Number.Parity.Odd);

    /// <summary>
    /// Validates that the non-nullable <see cref="long"/> property value is even.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Even(IMustClause, long, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LargeCount).Even();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Even(IMustClause, long, string)"/>
    public static IRuleBuilderOptions<TModel, long> Even<TModel>(
        this IRuleBuilder<TModel, long> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Even(val, paramName: null),
            message, MustCodes.Number.Parity.Odd);

    /// <summary>
    /// Validates that the nullable <see cref="int"/> property value is odd.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Odd(IMustClause, int, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).Odd();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Odd(IMustClause, int, string)"/>
    public static IRuleBuilderOptions<TModel, int?> Odd<TModel>(
        this IRuleBuilder<TModel, int?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Odd(val.Value, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Number.Parity.Even);

    /// <summary>
    /// Validates that the non-nullable <see cref="int"/> property value is odd.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Odd(IMustClause, int, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Count).Odd();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Odd(IMustClause, int, string)"/>
    public static IRuleBuilderOptions<TModel, int> Odd<TModel>(
        this IRuleBuilder<TModel, int> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Odd(val, paramName: null),
            message, MustCodes.Number.Parity.Even);

    /// <summary>
    /// Validates that the nullable <see cref="long"/> property value is odd.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Odd(IMustClause, long, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LargeCount).Odd();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Odd(IMustClause, long, string)"/>
    public static IRuleBuilderOptions<TModel, long?> Odd<TModel>(
        this IRuleBuilder<TModel, long?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Odd(val.Value, paramName: null) : MustResult<long>.Ok(0),
            message, MustCodes.Number.Parity.Even);

    /// <summary>
    /// Validates that the non-nullable <see cref="long"/> property value is odd.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Odd(IMustClause, long, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LargeCount).Odd();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Odd(IMustClause, long, string)"/>
    public static IRuleBuilderOptions<TModel, long> Odd<TModel>(
        this IRuleBuilder<TModel, long> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Odd(val, paramName: null),
            message, MustCodes.Number.Parity.Even);

    /// <summary>
    /// Validates that the nullable <see cref="float"/> property value is finite (not infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Finite(IMustClause, float, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Ratio).Finite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Finite(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float?> Finite<TModel>(
        this IRuleBuilder<TModel, float?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Finite(val.Value, paramName: null) : MustResult<float>.Ok(0),
            message, MustCodes.Number.Form.NotFinite);

    /// <summary>
    /// Validates that the non-nullable <see cref="float"/> property value is finite (not infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Finite(IMustClause, float, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Ratio).Finite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Finite(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float> Finite<TModel>(
        this IRuleBuilder<TModel, float> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Finite(val, paramName: null),
            message, MustCodes.Number.Form.NotFinite);

    /// <summary>
    /// Validates that the nullable <see cref="double"/> property value is finite (not infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Finite(IMustClause, double, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Coefficient).Finite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Finite(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double?> Finite<TModel>(
        this IRuleBuilder<TModel, double?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.Finite(val.Value, paramName: null) : MustResult<double>.Ok(0),
            message, MustCodes.Number.Form.NotFinite);

    /// <summary>
    /// Validates that the non-nullable <see cref="double"/> property value is finite (not infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.Finite(IMustClause, double, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Coefficient).Finite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.Finite(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double> Finite<TModel>(
        this IRuleBuilder<TModel, double> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Finite(val, paramName: null),
            message, MustCodes.Number.Form.NotFinite);

    /// <summary>
    /// Validates that the nullable <see cref="float"/> property value is not finite (is infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotFinite(IMustClause, float, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelValue).NotFinite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotFinite(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float?> NotFinite<TModel>(
        this IRuleBuilder<TModel, float?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotFinite(val.Value, paramName: null) : MustResult<float>.Ok(0),
            message, MustCodes.Number.Form.Finite);

    /// <summary>
    /// Validates that the non-nullable <see cref="float"/> property value is not finite (is infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotFinite(IMustClause, float, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelValue).NotFinite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotFinite(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float> NotFinite<TModel>(
        this IRuleBuilder<TModel, float> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotFinite(val, paramName: null),
            message, MustCodes.Number.Form.Finite);

    /// <summary>
    /// Validates that the nullable <see cref="double"/> property value is not finite (is infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotFinite(IMustClause, double, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelValue).NotFinite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotFinite(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double?> NotFinite<TModel>(
        this IRuleBuilder<TModel, double?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotFinite(val.Value, paramName: null) : MustResult<double>.Ok(0),
            message, MustCodes.Number.Form.Finite);

    /// <summary>
    /// Validates that the non-nullable <see cref="double"/> property value is not finite (is infinity or NaN).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotFinite(IMustClause, double, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelValue).NotFinite();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotFinite(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double> NotFinite<TModel>(
        this IRuleBuilder<TModel, double> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotFinite(val, paramName: null),
            message, MustCodes.Number.Form.Finite);

    /// <summary>
    /// Validates that the nullable <see cref="float"/> property value is not NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotNaN(IMustClause, float, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FloatValue).NotNaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotNaN(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float?> NotNaN<TModel>(this IRuleBuilder<TModel, float?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotNaN(val.Value, paramName: null) : MustResult<float>.Ok(0),
            message, MustCodes.Number.Form.Nan);

    /// <summary>
    /// Validates that the non-nullable <see cref="float"/> property value is not NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotNaN(IMustClause, float, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FloatValue).NotNaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotNaN(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float> NotNaN<TModel>(this IRuleBuilder<TModel, float> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotNaN(val, paramName: null),
            message, MustCodes.Number.Form.Nan);

    /// <summary>
    /// Validates that the nullable <see cref="double"/> property value is not NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotNaN(IMustClause, double, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DoubleValue).NotNaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotNaN(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double?> NotNaN<TModel>(this IRuleBuilder<TModel, double?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NotNaN(val.Value, paramName: null) : MustResult<double>.Ok(0),
            message, MustCodes.Number.Form.Nan);

    /// <summary>
    /// Validates that the non-nullable <see cref="double"/> property value is not NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NotNaN(IMustClause, double, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DoubleValue).NotNaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NotNaN(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double> NotNaN<TModel>(this IRuleBuilder<TModel, double> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotNaN(val, paramName: null),
            message, MustCodes.Number.Form.Nan);

    /// <summary>
    /// Validates that the nullable <see cref="float"/> property value is NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NaN(IMustClause, float, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelFloat).NaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NaN(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float?> NaN<TModel>(this IRuleBuilder<TModel, float?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NaN(val.Value, paramName: null) : MustResult<float>.Ok(0),
            message, MustCodes.Number.Form.NotNan);

    /// <summary>
    /// Validates that the non-nullable <see cref="float"/> property value is NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NaN(IMustClause, float, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelFloat).NaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NaN(IMustClause, float, string)"/>
    public static IRuleBuilderOptions<TModel, float> NaN<TModel>(this IRuleBuilder<TModel, float> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NaN(val, paramName: null),
            message, MustCodes.Number.Form.NotNan);

    /// <summary>
    /// Validates that the nullable <see cref="double"/> property value is NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NaN(IMustClause, double, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelDouble).NaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NaN(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double?> NaN<TModel>(this IRuleBuilder<TModel, double?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val.HasValue ? Must.Be.NaN(val.Value, paramName: null) : MustResult<double>.Ok(0),
            message, MustCodes.Number.Form.NotNan);

    /// <summary>
    /// Validates that the non-nullable <see cref="double"/> property value is NaN (Not a Number).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustNumberClauses.NaN(IMustClause, double, string)"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SentinelDouble).NaN();
    /// </code>
    /// </example>
    /// <seealso cref="MustNumberClauses.NaN(IMustClause, double, string)"/>
    public static IRuleBuilderOptions<TModel, double> NaN<TModel>(this IRuleBuilder<TModel, double> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NaN(val, paramName: null),
            message, MustCodes.Number.Form.NotNan);
}
#endif
