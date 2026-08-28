using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for general object equality and type validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/object">Fluent Object Extensions documentation</seealso>
public static class FluentObjectExtensions
{
    /// <summary>
    /// Validates that the property value is equal to <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.EqualTo"/>, which compares using
    /// <see cref="EqualityComparer{T}.Default"/> semantics. A <see langword="null"/> value satisfies this check
    /// only when <paramref name="other"/> is also <see langword="null"/>; there is no unconditional null
    /// pass-through, so this is not a substitute for a separate <c>.NotNull()</c> rule.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).EqualTo("Active");
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.EqualTo"/>
    public static IRuleBuilderOptions<TModel, T?> EqualTo<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T? other,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.EqualTo(val, other, paramName: null),
            message, MustCodes.Value.Equality.NotEqual);

    /// <summary>
    /// Validates that the property value is not equal to <paramref name="other"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="other">The value to compare against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.NotEqualTo"/>, which compares using
    /// <see cref="EqualityComparer{T}.Default"/> semantics. A <see langword="null"/> value satisfies this check
    /// unless <paramref name="other"/> is also <see langword="null"/>; there is no unconditional null
    /// pass-through, so this is not a substitute for a separate <c>.NotNull()</c> rule.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Status).NotEqualTo("Banned");
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotEqualTo"/>
    public static IRuleBuilderOptions<TModel, T?> NotEqualTo<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T? other,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotEqualTo(val, other, paramName: null),
            message, MustCodes.Value.Equality.Equal);

    /// <summary>
    /// Validates that the property value is of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The expected runtime type.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.OfType"/>. If the value is <see langword="null"/>,
    /// validation fails, since <see langword="null"/> has no runtime type to compare against
    /// <typeparamref name="T"/>; use a separate <c>.NotNull()</c> rule beforehand if <see langword="null"/>
    /// should be reported as a distinct failure.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Payload).OfType<JsonElement>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.OfType"/>
    public static IRuleBuilderOptions<TModel, object?> OfType<TModel, T>(this IRuleBuilder<TModel, object?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.OfType<T>(val, paramName: null),
            message, MustCodes.Value.Identity.WrongType);

    /// <summary>
    /// Validates that the property value is not of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The type that the value must not be.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.NotOfType"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Payload).NotOfType<string>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotOfType"/>
    public static IRuleBuilderOptions<TModel, object?> NotOfType<TModel, T>(this IRuleBuilder<TModel, object?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotOfType<T>(val, paramName: null),
            message, MustCodes.Value.Identity.SameType);

    /// <summary>
    /// Validates that the property value is assignable to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The target type to check assignability against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.AssignableToType"/>. If the value is <see langword="null"/>,
    /// validation fails, since <see langword="null"/> is not an instance of <typeparamref name="T"/>; use a
    /// separate <c>.NotNull()</c> rule beforehand if <see langword="null"/> should be reported as a distinct
    /// failure.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Handler).AssignableToType<ICommandHandler>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.AssignableToType"/>
    public static IRuleBuilderOptions<TModel, object?> AssignableToType<TModel, T>(this IRuleBuilder<TModel, object?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.AssignableToType<T>(val, paramName: null),
            message, MustCodes.Value.Identity.NotAssignable);

    /// <summary>
    /// Validates that the property value is not assignable to type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The target type to check assignability against.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.NotAssignableToType"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// RuleFor(x => x.Handler).NotAssignableToType<IObsoleteHandler>();
    /// ]]></code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotAssignableToType"/>
    public static IRuleBuilderOptions<TModel, object?> NotAssignableToType<TModel, T>(this IRuleBuilder<TModel, object?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotAssignableToType<T>(val, paramName: null),
            message, MustCodes.Value.Identity.Assignable);

    /// <summary>
    /// Validates that the property value is the same reference as <paramref name="b"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The reference type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="b">The reference to compare against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.SameReferenceAs"/>, which compares using
    /// <c>ReferenceEquals</c> semantics. A <see langword="null"/> value satisfies this check only when
    /// <paramref name="b"/> is also <see langword="null"/>; there is no unconditional null pass-through, so
    /// this is not a substitute for a separate <c>.NotNull()</c> rule.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Context).SameReferenceAs(expectedContext);
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.SameReferenceAs"/>
    public static IRuleBuilderOptions<TModel, T?> SameReferenceAs<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T? b,
        string? message = null)
        where T : class =>
        ruleBuilder.MustBe(val => Must.Be.SameReferenceAs(val, b, paramName: null),
            message, MustCodes.Value.Identity.NotSameReference);

    /// <summary>
    /// Validates that the property value is not the same reference as <paramref name="b"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <typeparam name="T">The reference type of the property value.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="b">The reference to compare against.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustObjectClauses.NotSameReferenceAs"/>, which compares using
    /// <c>ReferenceEquals</c> semantics. A <see langword="null"/> value satisfies this check unless
    /// <paramref name="b"/> is also <see langword="null"/>; there is no unconditional null pass-through, so
    /// this is not a substitute for a separate <c>.NotNull()</c> rule.
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.NewItem).NotSameReferenceAs(existingItem);
    /// </code>
    /// </example>
    /// <seealso cref="MustObjectClauses.NotSameReferenceAs"/>
    public static IRuleBuilderOptions<TModel, T?> NotSameReferenceAs<TModel, T>(this IRuleBuilder<TModel, T?> ruleBuilder,
        T? b,
        string? message = null)
        where T : class =>
        ruleBuilder.MustBe(val => Must.Be.NotSameReferenceAs(val, b, paramName: null),
            message, MustCodes.Value.Identity.SameReference);
}
