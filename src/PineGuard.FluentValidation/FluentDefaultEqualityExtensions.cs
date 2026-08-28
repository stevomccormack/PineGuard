using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for default-value equality validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/object">Fluent Object Extensions documentation</seealso>
public static class FluentDefaultEqualityExtensions
{
    /// <summary>
    /// Validates that the property value is the default value for its type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDefaultEqualityClauses.Default"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Priority).Default();
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.Default"/>
    public static IRuleBuilderOptions<TModel, T?> Default<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Default(val, paramName: null),
            message, MustCodes.Value.State.NotDefault);

    /// <summary>
    /// Validates that the property value is not the default value for its type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDefaultEqualityClauses.NotDefault"/>. For a reference type, <see langword="null"/>
    /// <em>is</em> <see langword="default"/>(<typeparamref name="T"/>), so a <see langword="null"/> value fails
    /// this check rather than passing; use <c>.NullOrDefault()</c> or a separate <c>.NotNull()</c> rule if
    /// <see langword="null"/> should be treated differently.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CreatedAt).NotDefault();
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.NotDefault"/>
    public static IRuleBuilderOptions<TModel, T?> NotDefault<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotDefault(val, paramName: null),
            message, MustCodes.Value.State.Default);

    /// <summary>
    /// Validates that the property value is <see langword="null"/> or the default value for its type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDefaultEqualityClauses.NullOrDefault"/>. Passes if the value is either
    /// <see langword="null"/> or equal to the default value of <typeparamref name="T"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.OptionalId).NullOrDefault();
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.NullOrDefault"/>
    public static IRuleBuilderOptions<TModel, T?> NullOrDefault<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NullOrDefault(val, paramName: null),
            message, MustCodes.Value.State.NotNullOrDefault);

    /// <summary>
    /// Validates that the property value is not <see langword="null"/> and not the default value for its type.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustDefaultEqualityClauses.NotNullOrDefault"/>. Fails if the value is either
    /// <see langword="null"/> or equal to the default value of <typeparamref name="T"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UserId).NotNullOrDefault();
    /// </code>
    /// </example>
    /// <seealso cref="MustDefaultEqualityClauses.NotNullOrDefault"/>
    public static IRuleBuilderOptions<TModel, T?> NotNullOrDefault<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotNullOrDefault(val, paramName: null),
            message, MustCodes.Value.State.NullOrDefault);
}
