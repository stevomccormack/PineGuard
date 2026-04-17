using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for file name and file extension validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/filepath">Fluent File Path Extensions documentation</seealso>
public static class FluentFilePathExtensions
{
    /// <summary>
    /// Validates that the property value is a safe file name (no path traversal characters or reserved names).
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustFilePathClauses.SafeFileName"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UploadedFileName).SafeFileName();
    /// </code>
    /// </example>
    /// <seealso cref="MustFilePathClauses.SafeFileName"/>
    public static IRuleBuilderOptions<TModel, string?> SafeFileName<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.SafeFileName(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value has a file extension included in the allowed list.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="allowed">An array of permitted file extensions (e.g., <c>".pdf"</c>, <c>".png"</c>). If <see langword="null"/>, all extensions are permitted.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustFilePathClauses.HasFileExtension"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.FileName).HasFileExtension(new[] { ".pdf", ".docx" });
    /// </code>
    /// </example>
    /// <seealso cref="MustFilePathClauses.HasFileExtension"/>
    public static IRuleBuilderOptions<TModel, string?> HasFileExtension<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        string[]? allowed,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.HasFileExtension(val, allowed, paramName: null),
            message);
}
