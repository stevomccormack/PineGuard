using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for JSON content validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/json">Fluent JSON Extensions documentation</seealso>
public static class FluentJsonExtensions
{
    /// <summary>
    /// Validates that the property value is a well-formed JSON string.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustJsonClauses.Json"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Payload).Json();
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.Json"/>
    public static IRuleBuilderOptions<TModel, string?> Json<TModel>(
        this IRuleBuilder<TModel, string?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Json(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the HTTP headers dictionary contains a JSON-compatible Content-Type header.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustJsonClauses.JsonContentType"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ResponseHeaders).JsonContentType();
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.JsonContentType"/>
    public static IRuleBuilderOptions<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> JsonContentType<TModel>(
        this IRuleBuilder<TModel, IReadOnlyDictionary<string, IEnumerable<string>>?> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.JsonContentType(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is a well-formed JSON object (not an array or primitive).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustJsonClauses.JsonObject"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.RequestBody).JsonObject();
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.JsonObject"/>
    public static IRuleBuilderOptions<TModel, string?> JsonObject<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.JsonObject(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is a well-formed JSON array.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustJsonClauses.JsonArray"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ItemsList).JsonArray();
    /// </code>
    /// </example>
    /// <seealso cref="MustJsonClauses.JsonArray"/>
    public static IRuleBuilderOptions<TModel, string?> JsonArray<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.JsonArray(val, paramName: null),
            message);
}
