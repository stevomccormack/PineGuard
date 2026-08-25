using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for email address property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/email">Fluent Email Extensions documentation</seealso>
public static class FluentEmailExtensions
{
    /// <summary>
    /// Validates that the property value is a valid email address.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEmailClauses.Email"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Email).Email();
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.Email"/>
    public static IRuleBuilderOptions<TModel, string?> Email<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Email(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is a valid email address using strict RFC-5321 rules.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEmailClauses.StrictEmail"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Email).StrictEmail();
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.StrictEmail"/>
    public static IRuleBuilderOptions<TModel, string?> StrictEmail<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.StrictEmail(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is an email address containing a plus-sign alias (e.g., <c>user+alias@domain.com</c>).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEmailClauses.HasEmailAlias"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Email).HasEmailAlias();
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.HasEmailAlias"/>
    public static IRuleBuilderOptions<TModel, string?> HasEmailAlias<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasEmailAlias(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is an email address that does not contain a plus-sign alias.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustEmailClauses.NotHasEmailAlias"/>. If the value is <see langword="null"/>,
    /// validation fails; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be
    /// reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Email).NotHasEmailAlias();
    /// </code>
    /// </example>
    /// <seealso cref="MustEmailClauses.NotHasEmailAlias"/>
    public static IRuleBuilderOptions<TModel, string?> NotHasEmailAlias<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotHasEmailAlias(val, paramName: null),
            message);
}
