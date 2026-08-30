using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods that validate how many grapheme clusters — the characters a
/// reader sees — a string property holds.
/// </summary>
/// <remarks>
/// <see cref="string.Length"/> counts UTF-16 code units, so a family emoji reads as eleven characters and an
/// accented letter written with a combining mark reads as two. These rules count what a length limit shown to
/// a user is actually promising. Segmentation follows the host runtime's Unicode tables.
/// </remarks>
/// <seealso cref="MustStringGraphemesClauses"/>
/// <seealso href="https://pineguard.ai/docs/fluent/string-graphemes">Fluent String Graphemes Extensions documentation</seealso>
public static class FluentStringGraphemesExtensions
{
    /// <summary>
    /// Validates that the property value holds exactly the given number of grapheme clusters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="count">The required number of characters.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.HasExactGraphemeCount"/>. Validation fails if
    /// <paramref name="count"/> is negative. If the value is <see langword="null"/>, validation passes
    /// (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CountryCode).HasExactGraphemeCount(2);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasExactGraphemeCount"/>
    public static IRuleBuilderOptions<TModel, string?> HasExactGraphemeCount<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int count,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.HasExactGraphemeCount(val, count, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.Mismatch);

    /// <summary>
    /// Validates that the property value does not hold exactly the given number of grapheme clusters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="count">The number of characters that must not match.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.NotHasExactGraphemeCount"/>. Validation fails if
    /// <paramref name="count"/> is negative. If the value is <see langword="null"/>, validation passes
    /// (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Nickname).NotHasExactGraphemeCount(1);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.NotHasExactGraphemeCount"/>
    public static IRuleBuilderOptions<TModel, string?> NotHasExactGraphemeCount<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int count,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotHasExactGraphemeCount(val, count, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.Match);

    /// <summary>
    /// Validates that the property value holds at least the given number of grapheme clusters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum required number of characters.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.HasMinGraphemeCount"/>. Validation fails if
    /// <paramref name="min"/> is negative. If the value is <see langword="null"/>, validation passes
    /// (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayName).HasMinGraphemeCount(3);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasMinGraphemeCount"/>
    public static IRuleBuilderOptions<TModel, string?> HasMinGraphemeCount<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.HasMinGraphemeCount(val, min, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.TooFew);

    /// <summary>
    /// Validates that the property value does not hold at least the given number of grapheme clusters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The minimum number of characters that must not be reached.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.NotHasMinGraphemeCount"/>. Validation fails if
    /// <paramref name="min"/> is negative. If the value is <see langword="null"/>, validation passes
    /// (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Abbreviation).NotHasMinGraphemeCount(5);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.NotHasMinGraphemeCount"/>
    public static IRuleBuilderOptions<TModel, string?> NotHasMinGraphemeCount<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotHasMinGraphemeCount(val, min, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.TooMany);

    /// <summary>
    /// Validates that the property value holds at most the given number of grapheme clusters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="max">The maximum allowed number of characters.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.HasMaxGraphemeCount"/>. This is the rule a
    /// "your name is too long" limit wants: a family emoji costs eleven code units and one character.
    /// Validation fails if <paramref name="max"/> is negative. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayName).HasMaxGraphemeCount(50);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasMaxGraphemeCount"/>
    public static IRuleBuilderOptions<TModel, string?> HasMaxGraphemeCount<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int max,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.HasMaxGraphemeCount(val, max, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.TooMany);

    /// <summary>
    /// Validates that the property value does not hold at most the given number of grapheme clusters.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="max">The maximum number of characters that must be exceeded.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.NotHasMaxGraphemeCount"/>. Validation fails if
    /// <paramref name="max"/> is negative. If the value is <see langword="null"/>, validation passes
    /// (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Summary).NotHasMaxGraphemeCount(10);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.NotHasMaxGraphemeCount"/>
    public static IRuleBuilderOptions<TModel, string?> NotHasMaxGraphemeCount<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int max,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotHasMaxGraphemeCount(val, max, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.TooFew);

    /// <summary>
    /// Validates that the number of grapheme clusters in the property value falls within the given range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the acceptable number of characters.</param>
    /// <param name="max">The upper bound of the acceptable number of characters.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.HasGraphemeCountBetween"/>. Validation fails if
    /// either bound is negative or the range is inverted. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.DisplayName).HasGraphemeCountBetween(3, 50);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.HasGraphemeCountBetween"/>
    public static IRuleBuilderOptions<TModel, string?> HasGraphemeCountBetween<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.HasGraphemeCountBetween(val, min, max, inclusion, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.OutOfRange);

    /// <summary>
    /// Validates that the number of grapheme clusters in the property value falls outside the given range.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="min">The lower bound of the forbidden number of characters.</param>
    /// <param name="max">The upper bound of the forbidden number of characters.</param>
    /// <param name="inclusion">Whether the bounds are inclusive or exclusive.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustStringGraphemesClauses.NotHasGraphemeCountBetween"/>. Validation fails if
    /// either bound is negative or the range is inverted. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Handle).NotHasGraphemeCountBetween(1, 2);
    /// </code>
    /// </example>
    /// <seealso cref="MustStringGraphemesClauses.NotHasGraphemeCountBetween"/>
    public static IRuleBuilderOptions<TModel, string?> NotHasGraphemeCountBetween<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        int min,
        int max,
        Inclusion inclusion = Inclusion.Inclusive,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotHasGraphemeCountBetween(val, min, max, inclusion, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Text.Graphemes.InRange);
}
