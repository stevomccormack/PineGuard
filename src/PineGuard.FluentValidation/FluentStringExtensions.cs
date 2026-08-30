using System.Text.RegularExpressions;
using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for string content and format validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/string">Fluent String Extensions documentation</seealso>
public static class FluentStringExtensions
{
    /// <summary>
    /// Validates that the property value is not <see langword="null"/> or an empty string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotNullOrEmpty"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Name).NotNullOrEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotNullOrEmpty"/>
    public static IRuleBuilderOptions<TModel, string?> NotNullOrEmpty<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotNullOrEmpty(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.NullOrEmpty);

    /// <summary>
    /// Validates that the property value is <see langword="null"/> or an empty string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NullOrEmpty"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.OptionalNote).NullOrEmpty();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NullOrEmpty"/>
    public static IRuleBuilderOptions<TModel, string?> NullOrEmpty<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NullOrEmpty(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.NotNullOrEmpty);

    /// <summary>
    /// Validates that the property value is not <see langword="null"/>, empty, or whitespace-only.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotNullOrWhiteSpace"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Description).NotNullOrWhiteSpace();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotNullOrWhiteSpace"/>
    public static IRuleBuilderOptions<TModel, string?> NotNullOrWhiteSpace<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotNullOrWhiteSpace(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.Blank);

    /// <summary>
    /// Validates that the property value is <see langword="null"/>, empty, or whitespace-only.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NullOrWhiteSpace"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Filler).NullOrWhiteSpace();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NullOrWhiteSpace"/>
    public static IRuleBuilderOptions<TModel, string?> NullOrWhiteSpace<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NullOrWhiteSpace(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.NotBlank);

    /// <summary>
    /// Validates that the property value has exactly the specified character length.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="length">The required exact length.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ExactLength"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PostalCode).ExactLength(5);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ExactLength"/>
    public static IRuleBuilderOptions<TModel, string?> ExactLength<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int length,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ExactLength(val, length, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Length.Mismatch);

    /// <summary>
    /// Validates that the property value has a character length between the specified minimum and maximum.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum allowed length (inclusive).</param>
    /// <param name="max">The maximum allowed length (inclusive).</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.LengthBetween"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Username).LengthBetween(3, 20);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.LengthBetween"/>
    public static IRuleBuilderOptions<TModel, string?> LengthBetween<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        int max,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.LengthBetween(val, min, max, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Length.OutOfRange);

    /// <summary>
    /// Validates that the property value has a character length greater than the specified length.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="length">The exclusive minimum length.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.LongerThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Password).LongerThan(8);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.LongerThan"/>
    public static IRuleBuilderOptions<TModel, string?> LongerThan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int length,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.LongerThan(val, length, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Length.TooShort);

    /// <summary>
    /// Validates that the property value has a character length greater than or equal to the specified length.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="length">The inclusive minimum length.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.LongerThanOrEqual"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Password).LongerThanOrEqual(8);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.LongerThanOrEqual"/>
    public static IRuleBuilderOptions<TModel, string?> LongerThanOrEqual<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int length,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.LongerThanOrEqual(val, length, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Length.TooShort);

    /// <summary>
    /// Validates that the property value has a character length less than the specified length.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="length">The exclusive maximum length.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ShorterThan"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ShortCode).ShorterThan(10);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ShorterThan"/>
    public static IRuleBuilderOptions<TModel, string?> ShorterThan<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int length,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ShorterThan(val, length, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Length.TooLong);

    /// <summary>
    /// Validates that the property value has a character length less than or equal to the specified length.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="length">The inclusive maximum length.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ShorterThanOrEqual"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ShortCode).ShorterThanOrEqual(10);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ShorterThanOrEqual"/>
    public static IRuleBuilderOptions<TModel, string?> ShorterThanOrEqual<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int length,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ShorterThanOrEqual(val, length, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Length.TooLong);

    /// <summary>
    /// Validates that the property value matches the specified regular expression pattern.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="pattern">The compiled <see cref="Regex"/> the value must match.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Match"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PhoneNumber).Match(MyRegexPatterns.PhoneNumber);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Match"/>
    public static IRuleBuilderOptions<TModel, string?> Match<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        Regex pattern,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Match(val, pattern, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Pattern.NoMatch);

    /// <summary>
    /// Validates that the property value does not match the specified regular expression pattern.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="pattern">The compiled <see cref="Regex"/> the value must not match.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotMatch"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UserInput).NotMatch(MyRegexPatterns.Blocked);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotMatch"/>
    public static IRuleBuilderOptions<TModel, string?> NotMatch<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        Regex pattern,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotMatch(val, pattern, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Pattern.Match);

    /// <summary>
    /// Validates that the property value is itself a well-formed regular expression pattern.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// This validates the pattern, not a value against a pattern — <see cref="Match{TModel}"/> does the latter.
    /// It is what a model carrying a caller-supplied or configured pattern needs, so a malformed one is reported
    /// against the property instead of thrown from deep inside whatever later compiles it. Delegates to
    /// <see cref="MustStringClauses.RegexPattern"/>, which checks syntax only: a pattern that compiles can still
    /// be catastrophically slow. If the value is <see langword="null"/>, validation passes (null values should be
    /// handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SearchPattern).RegexPattern();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.RegexPattern"/>
    public static IRuleBuilderOptions<TModel, string?> RegexPattern<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.RegexPattern(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Pattern.Invalid);

    /// <summary>
    /// Validates that the property value contains only alphabetic characters, optionally including specified additional characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusions">An optional array of non-alphabetic characters that are also allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Alphabetic"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FirstName).Alphabetic();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Alphabetic"/>
    public static IRuleBuilderOptions<TModel, string?> Alphabetic<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? inclusions = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Alphabetic(val, inclusions, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotAlpha);

    /// <summary>
    /// Validates that the property value does not contain only alphabetic characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusions">An optional array of non-alphabetic characters that are also considered allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotAlphabetic"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ProductCode).NotAlphabetic();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotAlphabetic"/>
    public static IRuleBuilderOptions<TModel, string?> NotAlphabetic<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? inclusions = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotAlphabetic(val, inclusions, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Alpha);

    /// <summary>
    /// Validates that the property value contains only numeric characters, optionally including specified additional characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusions">An optional array of non-numeric characters that are also allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Numeric"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Pin).Numeric();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Numeric"/>
    public static IRuleBuilderOptions<TModel, string?> Numeric<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? inclusions = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Numeric(val, inclusions, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotNumeric);

    /// <summary>
    /// Validates that the property value does not contain only numeric characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusions">An optional array of non-numeric characters that are also considered allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotNumeric"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Username).NotNumeric();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotNumeric"/>
    public static IRuleBuilderOptions<TModel, string?> NotNumeric<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? inclusions = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotNumeric(val, inclusions, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Numeric);

    /// <summary>
    /// Validates that the property value contains only alphanumeric characters, optionally including specified additional characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusions">An optional array of non-alphanumeric characters that are also allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Alphanumeric"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Username).Alphanumeric();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Alphanumeric"/>
    public static IRuleBuilderOptions<TModel, string?> Alphanumeric<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? inclusions = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Alphanumeric(val, inclusions, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotAlphanumeric);

    /// <summary>
    /// Validates that the property value does not contain only alphanumeric characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="inclusions">An optional array of non-alphanumeric characters that are also considered allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotAlphanumeric"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SpecialToken).NotAlphanumeric();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotAlphanumeric"/>
    public static IRuleBuilderOptions<TModel, string?> NotAlphanumeric<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? inclusions = null,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotAlphanumeric(val, inclusions, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Alphanumeric);

    /// <summary>
    /// Validates that the property value contains at least one of the specified characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="anyOf">An array of characters, at least one of which must be present.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ContainsAny"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Password).ContainsAny(new[] { '!', '@', '#', '$' });
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ContainsAny"/>
    public static IRuleBuilderOptions<TModel, string?> ContainsAny<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[] anyOf,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ContainsAny(val, anyOf, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotContainsAny);

    /// <summary>
    /// Validates that the property value contains only digit characters (0–9).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.DigitsOnly(IMustClause, string, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Pin).DigitsOnly();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.DigitsOnly(IMustClause, string, string)"/>
    public static IRuleBuilderOptions<TModel, string?> DigitsOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.DigitsOnly(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotDigits);

    /// <summary>
    /// Validates that the property value contains only digit characters (0–9) plus any explicitly allowed non-digit characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowedNonDigitChars">An optional array of non-digit characters that are permitted.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.DigitsOnly(IMustClause, string, char[], string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.PhoneNumber).DigitsOnly(new[] { '+', '-', ' ' });
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.DigitsOnly(IMustClause, string, char[], string)"/>
    public static IRuleBuilderOptions<TModel, string?> DigitsOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? allowedNonDigitChars,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.DigitsOnly(val, allowedNonDigitChars, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotDigits);

    /// <summary>
    /// Validates that the property value does not contain only digit characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotDigitsOnly(IMustClause, string, string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Username).NotDigitsOnly();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotDigitsOnly(IMustClause, string, string)"/>
    public static IRuleBuilderOptions<TModel, string?> NotDigitsOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotDigitsOnly(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Digits);

    /// <summary>
    /// Validates that the property value does not contain only digit characters when considering the allowed non-digit characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowedNonDigitChars">An optional array of non-digit characters that are also considered allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotDigitsOnly(IMustClause, string, char[], string)"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Code).NotDigitsOnly(new[] { '-' });
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotDigitsOnly(IMustClause, string, char[], string)"/>
    public static IRuleBuilderOptions<TModel, string?> NotDigitsOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[]? allowedNonDigitChars,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotDigitsOnly(val, allowedNonDigitChars, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Digits);

    /// <summary>
    /// Validates that the property value contains only uppercase letters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="lettersOnly">When <see langword="true"/>, only letter characters are evaluated; non-letter characters are ignored.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Uppercase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CountryCode).Uppercase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Uppercase"/>
    public static IRuleBuilderOptions<TModel, string?> Uppercase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool lettersOnly = false,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Uppercase(val, lettersOnly, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Casing.NotUpper);

    /// <summary>
    /// Validates that the property value does not contain only uppercase letters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="lettersOnly">When <see langword="true"/>, only letter characters are evaluated; non-letter characters are ignored.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotUppercase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayName).NotUppercase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotUppercase"/>
    public static IRuleBuilderOptions<TModel, string?> NotUppercase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool lettersOnly = false,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotUppercase(val, lettersOnly, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Casing.Upper);

    /// <summary>
    /// Validates that the property value contains only lowercase letters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="lettersOnly">When <see langword="true"/>, only letter characters are evaluated; non-letter characters are ignored.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Lowercase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.EmailAddress).Lowercase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Lowercase"/>
    public static IRuleBuilderOptions<TModel, string?> Lowercase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool lettersOnly = false,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Lowercase(val, lettersOnly, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Casing.NotLower);

    /// <summary>
    /// Validates that the property value does not contain only lowercase letters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="lettersOnly">When <see langword="true"/>, only letter characters are evaluated; non-letter characters are ignored.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotLowercase"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayName).NotLowercase();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotLowercase"/>
    public static IRuleBuilderOptions<TModel, string?> NotLowercase<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool lettersOnly = false,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotLowercase(val, lettersOnly, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Casing.Lower);

    /// <summary>
    /// Validates that the property value contains only ASCII characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Ascii"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Token).Ascii();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Ascii"/>
    public static IRuleBuilderOptions<TModel, string?> Ascii<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Ascii(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotAscii);

    /// <summary>
    /// Validates that the property value contains at least one non-ASCII character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotAscii"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.InternationalName).NotAscii();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotAscii"/>
    public static IRuleBuilderOptions<TModel, string?> NotAscii<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotAscii(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Ascii);

    /// <summary>
    /// Validates that the property value contains only printable ASCII characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowCommonWhitespace">When <see langword="true"/>, common whitespace characters (space, tab, newline) are also allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.PrintableAscii"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayText).PrintableAscii();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.PrintableAscii"/>
    public static IRuleBuilderOptions<TModel, string?> PrintableAscii<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool allowCommonWhitespace = false,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.PrintableAscii(val, allowCommonWhitespace, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotPrintable);

    /// <summary>
    /// Validates that the property value contains at least one non-printable ASCII character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowCommonWhitespace">When <see langword="true"/>, common whitespace characters are excluded from consideration.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotPrintableAscii"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.BinaryData).NotPrintableAscii();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotPrintableAscii"/>
    public static IRuleBuilderOptions<TModel, string?> NotPrintableAscii<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        bool allowCommonWhitespace = false,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotPrintableAscii(val, allowCommonWhitespace, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Printable);

    /// <summary>
    /// Validates that the property value does not consist entirely of whitespace characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotWhitespace"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Title).NotWhitespace();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotWhitespace"/>
    public static IRuleBuilderOptions<TModel, string?> NotWhitespace<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotWhitespace(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.Whitespace);

    /// <summary>
    /// Validates that the property value contains at least one whitespace character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ContainsWhitespace"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FullName).ContainsWhitespace();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ContainsWhitespace"/>
    public static IRuleBuilderOptions<TModel, string?> ContainsWhitespace<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ContainsWhitespace(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotContainsWhitespace);

    /// <summary>
    /// Validates that the property value contains at least one control character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ContainsControlChars"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.RawData).ContainsControlChars();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ContainsControlChars"/>
    public static IRuleBuilderOptions<TModel, string?> ContainsControlChars<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ContainsControlChars(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotContainsControl);

    /// <summary>
    /// Validates that the property value does not contain any control characters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotContainsControlChars"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UserInput).NotContainsControlChars();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotContainsControlChars"/>
    public static IRuleBuilderOptions<TModel, string?> NotContainsControlChars<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotContainsControlChars(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.ContainsControl);

    /// <summary>
    /// Validates that the property value contains only characters from the specified allowed set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowedChars">The set of characters that the value may contain.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ContainsAllowedOnly"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.HexString).ContainsAllowedOnly(new[] { '0','1','2','3','4','5','6','7','8','9','a','b','c','d','e','f' });
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ContainsAllowedOnly"/>
    public static IRuleBuilderOptions<TModel, string?> ContainsAllowedOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[] allowedChars,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ContainsAllowedOnly(val, allowedChars, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotSubset);

    /// <summary>
    /// Validates that the property value contains at least one character from the specified disallowed set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="disallowedChars">The set of characters that must appear in the value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.ContainsDisallowed"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Password).ContainsDisallowed(new[] { '!', '@', '#' });
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.ContainsDisallowed"/>
    public static IRuleBuilderOptions<TModel, string?> ContainsDisallowed<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[] disallowedChars,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.ContainsDisallowed(val, disallowedChars, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.NotContainsDisallowed);

    /// <summary>
    /// Validates that the property value contains characters outside the specified allowed set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowedChars">The set of characters considered allowed.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotContainsAllowedOnly"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.MixedInput).NotContainsAllowedOnly(new[] { 'a', 'b', 'c' });
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotContainsAllowedOnly"/>
    public static IRuleBuilderOptions<TModel, string?> NotContainsAllowedOnly<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[] allowedChars,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotContainsAllowedOnly(val, allowedChars, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.Subset);

    /// <summary>
    /// Validates that the property value does not contain any character from the specified disallowed set.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="disallowedChars">The set of characters that must not appear in the value.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotContainsDisallowed"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.SafeInput).NotContainsDisallowed(new[] { '<', '>', '&' });
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotContainsDisallowed"/>
    public static IRuleBuilderOptions<TModel, string?> NotContainsDisallowed<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        char[] disallowedChars,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotContainsDisallowed(val, disallowedChars, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.ContainsDisallowed);

    /// <summary>
    /// Validates that the property value does not contain any whitespace character.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotContainsWhitespace"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Username).NotContainsWhitespace();
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotContainsWhitespace"/>
    public static IRuleBuilderOptions<TModel, string?> NotContainsWhitespace<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotContainsWhitespace(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Charset.ContainsWhitespace);

    /// <summary>
    /// Validates that the property value contains the specified substring.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="substring">The substring the value must contain. An empty substring is always contained.</param>
    /// <param name="comparison">The comparison rule used to locate <paramref name="substring"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.Contains"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Description).Contains("PineGuard");
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.Contains"/>
    public static IRuleBuilderOptions<TModel, string?> Contains<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string substring,
        StringComparison comparison = StringComparison.Ordinal,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Contains(val, substring, comparison, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.NotContains);

    /// <summary>
    /// Validates that the property value does not contain the specified substring.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="substring">The substring that must be absent. An empty substring is always contained.</param>
    /// <param name="comparison">The comparison rule used to locate <paramref name="substring"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotContains"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Comment).NotContains("password");
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotContains"/>
    public static IRuleBuilderOptions<TModel, string?> NotContains<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string substring,
        StringComparison comparison = StringComparison.Ordinal,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotContains(val, substring, comparison, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.Contains);

    /// <summary>
    /// Validates that the property value starts with the specified prefix.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="prefix">The prefix the value must start with. An empty prefix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="prefix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.StartsWith"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.SkuCode).StartsWith("PG-");
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.StartsWith"/>
    public static IRuleBuilderOptions<TModel, string?> StartsWith<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string prefix,
        StringComparison comparison = StringComparison.Ordinal,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.StartsWith(val, prefix, comparison, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.NotStartsWith);

    /// <summary>
    /// Validates that the property value does not start with the specified prefix.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="prefix">The prefix that must be absent. An empty prefix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="prefix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotStartsWith"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Username).NotStartsWith("admin");
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotStartsWith"/>
    public static IRuleBuilderOptions<TModel, string?> NotStartsWith<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string prefix,
        StringComparison comparison = StringComparison.Ordinal,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotStartsWith(val, prefix, comparison, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.StartsWith);

    /// <summary>
    /// Validates that the property value ends with the specified suffix.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="suffix">The suffix the value must end with. An empty suffix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="suffix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.EndsWith"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FileName).EndsWith(".pdf");
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.EndsWith"/>
    public static IRuleBuilderOptions<TModel, string?> EndsWith<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string suffix,
        StringComparison comparison = StringComparison.Ordinal,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.EndsWith(val, suffix, comparison, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.NotEndsWith);

    /// <summary>
    /// Validates that the property value does not end with the specified suffix.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="suffix">The suffix that must be absent. An empty suffix always matches.</param>
    /// <param name="comparison">The comparison rule used to test <paramref name="suffix"/>. Defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringClauses.NotEndsWith"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FileName).NotEndsWith(".exe");
    /// </code>
    /// </example>
    /// <seealso cref="MustStringClauses.NotEndsWith"/>
    public static IRuleBuilderOptions<TModel, string?> NotEndsWith<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string suffix,
        StringComparison comparison = StringComparison.Ordinal,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotEndsWith(val, suffix, comparison, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Content.EndsWith);
}
