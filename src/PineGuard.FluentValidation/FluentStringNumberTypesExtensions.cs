#if NET8_0_OR_GREATER
using System.Globalization;
using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for numeric type parsing validation of string values.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string-number-types">Fluent String Number Types Extensions documentation</seealso>
public static class FluentStringNumberTypesExtensions
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>
    /// Validates that the property value is a parseable decimal number with at most the specified number of decimal places.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="decimalPlaces">The maximum number of decimal places allowed. Defaults to 2.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumberTypesClauses.Decimal"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Price).Decimal(2);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Decimal"/>
    public static IRuleBuilderOptions<TModel, string?> Decimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int decimalPlaces = 2,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Decimal(val, decimalPlaces, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message, MustCodes.Number.Format.NotDecimal);

    /// <summary>
    /// Validates that the property value is a parseable decimal number with exactly the specified number of decimal places.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="exactDecimalPlaces">The exact number of decimal places required. Defaults to 2.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumberTypesClauses.ExactDecimal"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.MoneyAmount).ExactDecimal(2);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.ExactDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> ExactDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int exactDecimalPlaces = 2,
        string? message = null,
        NumberStyles styles = DefaultStyles) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ExactDecimal(val, exactDecimalPlaces, styles, paramName: null) : MustResult<decimal>.Ok(0),
            message, MustCodes.Number.Scale.Mismatch);

    /// <summary>
    /// Validates that the property value is parseable as a 32-bit integer.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumberTypesClauses.Int32"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PageNumber).Int32();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int32"/>
    public static IRuleBuilderOptions<TModel, string?> Int32<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Int32(val, styles, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Number.Format.NotInt32);

    /// <summary>
    /// Validates that the property value is parseable as a 64-bit integer.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumberTypesClauses.Int64"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LargeId).Int64();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int64"/>
    public static IRuleBuilderOptions<TModel, string?> Int64<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Int64(val, styles, paramName: null) : MustResult<long>.Ok(0),
            message, MustCodes.Number.Format.NotInt64);

    /// <summary>
    /// Validates that the property value is parseable as a 32-bit integer within the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the allowed range.</param>
    /// <param name="max">The upper bound of the allowed range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumberTypesClauses.Int32InRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Age).Int32InRange(0, 150);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int32InRange"/>
    public static IRuleBuilderOptions<TModel, string?> Int32InRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Int32InRange(val, min, max, inclusion, styles, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Number.Range.OutOfRange);

    /// <summary>
    /// Validates that the property value is parseable as a 32-bit integer outside the specified range.
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
    /// Delegates to <see cref="MustStringNumberTypesClauses.Int32OutOfRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ExcludedPort).Int32OutOfRange(49152, 65535);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int32OutOfRange"/>
    public static IRuleBuilderOptions<TModel, string?> Int32OutOfRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Int32OutOfRange(val, min, max, inclusion, styles, paramName: null) : MustResult<int>.Ok(0),
            message, MustCodes.Number.Range.InRange);

    /// <summary>
    /// Validates that the property value is parseable as a 64-bit integer within the specified range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the allowed range.</param>
    /// <param name="max">The upper bound of the allowed range.</param>
    /// <param name="inclusion">Specifies whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <param name="styles">The <see cref="NumberStyles"/> used when parsing the string value.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringNumberTypesClauses.Int64InRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Timestamp).Int64InRange(0, long.MaxValue);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int64InRange"/>
    public static IRuleBuilderOptions<TModel, string?> Int64InRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Int64InRange(val, min, max, inclusion, styles, paramName: null) : MustResult<long>.Ok(0),
            message, MustCodes.Number.Range.OutOfRange);

    /// <summary>
    /// Validates that the property value is parseable as a 64-bit integer outside the specified range.
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
    /// Delegates to <see cref="MustStringNumberTypesClauses.Int64OutOfRange"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ExcludedId).Int64OutOfRange(1000, 9999);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int64OutOfRange"/>
    public static IRuleBuilderOptions<TModel, string?> Int64OutOfRange<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null,
        NumberStyles styles = NumberStyles.Integer) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Int64OutOfRange(val, min, max, inclusion, styles, paramName: null) : MustResult<long>.Ok(0),
            message, MustCodes.Number.Range.InRange);
}
#endif
