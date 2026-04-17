using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for binary buffer encoding validation (Hex and Base64).
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/buffer">Fluent Buffer Extensions documentation</seealso>
public static class FluentBufferExtensions
{
    /// <summary>
    /// Validates that the property value is a valid hexadecimal string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBufferClauses.Hex"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Hash).Hex();
    /// </code>
    /// </example>
    /// <seealso cref="MustBufferClauses.Hex"/>
    public static IRuleBuilderOptions<TModel, string?> Hex<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Hex(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not a valid hexadecimal string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBufferClauses.NotHex"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Token).NotHex();
    /// </code>
    /// </example>
    /// <seealso cref="MustBufferClauses.NotHex"/>
    public static IRuleBuilderOptions<TModel, string?> NotHex<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotHex(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is a valid Base64-encoded string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBufferClauses.Base64"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.EncodedPayload).Base64();
    /// </code>
    /// </example>
    /// <seealso cref="MustBufferClauses.Base64"/>
    public static IRuleBuilderOptions<TModel, string?> Base64<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.Base64(val, paramName: null) : MustResult<string>.Ok(null!),
            message);

    /// <summary>
    /// Validates that the property value is not a valid Base64-encoded string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustBufferClauses.NotBase64"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.RawData).NotBase64();
    /// </code>
    /// </example>
    /// <seealso cref="MustBufferClauses.NotBase64"/>
    public static IRuleBuilderOptions<TModel, string?> NotBase64<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.NotBase64(val, paramName: null) : MustResult<string>.Ok(null!),
            message);
}
