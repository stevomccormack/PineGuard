using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for URL-safe identifier property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/identifier">Fluent Identifier Extensions documentation</seealso>
public static class FluentIdentifierExtensions
{
    /// <summary>
    /// Validates that the property value is a valid URL slug (lowercase letters, digits, and hyphens only).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustIdentifierClauses.Slug"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UrlSlug).Slug();
    /// </code>
    /// </example>
    /// <seealso cref="MustIdentifierClauses.Slug"/>
    public static IRuleBuilderOptions<TModel, string?> Slug<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Slug(val, paramName: null),
            message, MustCodes.Identifier.Slug.Invalid);

    /// <summary>
    /// Validates that the property value is a canonical ULID (Universally Unique Lexicographically Sortable Identifier).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustIdentifierClauses.Ulid"/>, which checks the textual form only and does not
    /// interpret the embedded timestamp. If the value is <see langword="null"/>, validation passes (null values
    /// should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.EventId).Ulid();
    /// </code>
    /// </example>
    /// <seealso cref="MustIdentifierClauses.Ulid"/>
    public static IRuleBuilderOptions<TModel, string?> Ulid<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Ulid(val, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Identifier.Ulid.Invalid);
}
