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
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Price).Decimal(2);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Decimal"/>
    public static IRuleBuilderOptions<TModel, string?> Decimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int decimalPlaces = 2,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Decimal(val, decimalPlaces, styles, paramName: null)),
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
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.MoneyAmount).ExactDecimal(2);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.ExactDecimal"/>
    public static IRuleBuilderOptions<TModel, string?> ExactDecimal<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int exactDecimalPlaces = 2,
        NumberStyles styles = DefaultStyles,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.ExactDecimal(val, exactDecimalPlaces, styles, paramName: null)),
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
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PageNumber).Int32();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int32"/>
    public static IRuleBuilderOptions<TModel, string?> Int32<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Int32(val, styles, paramName: null)),
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
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.LargeId).Int64();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.Int64"/>
    public static IRuleBuilderOptions<TModel, string?> Int64<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.Int64(val, styles, paramName: null)),
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
    /// Delegates to <see cref="MustStringNumberTypesClauses.InRangeInt32"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Age).InRangeInt32(0, 150);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.InRangeInt32"/>
    public static IRuleBuilderOptions<TModel, string?> InRangeInt32<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.InRangeInt32(val, min, max, inclusion, styles, paramName: null)),
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
    /// Delegates to <see cref="MustStringNumberTypesClauses.OutOfRangeInt32"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ExcludedPort).OutOfRangeInt32(49152, 65535);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.OutOfRangeInt32"/>
    public static IRuleBuilderOptions<TModel, string?> OutOfRangeInt32<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OutOfRangeInt32(val, min, max, inclusion, styles, paramName: null)),
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
    /// Delegates to <see cref="MustStringNumberTypesClauses.InRangeInt64"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Timestamp).InRangeInt64(0, long.MaxValue);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.InRangeInt64"/>
    public static IRuleBuilderOptions<TModel, string?> InRangeInt64<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.InRangeInt64(val, min, max, inclusion, styles, paramName: null)),
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
    /// Delegates to <see cref="MustStringNumberTypesClauses.OutOfRangeInt64"/>. If the value is <see langword="null"/>,
    /// validation passes when configuration is valid (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ExcludedId).OutOfRangeInt64(1000, 9999);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringNumberTypesClauses.OutOfRangeInt64"/>
    public static IRuleBuilderOptions<TModel, string?> OutOfRangeInt64<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        long min,
        long max,
        Inclusion inclusion = Inclusion.Inclusive,
        NumberStyles styles = NumberStyles.Integer,
        string? message = null) =>
        ruleBuilder.MustBe(val => SkipAbsentValue(val, Must.Be.OutOfRangeInt64(val, min, max, inclusion, styles, paramName: null)),
            message, MustCodes.Number.Range.InRange);

    private static MustResult<T> SkipAbsentValue<T>(string? value, MustResult<T> result) =>
        value is null && result.ParamName is null ? MustResult<T>.Ok(default) : result;
}
