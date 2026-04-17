using FluentValidation;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for <see cref="Task"/> state validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/task">Fluent Task Extensions documentation</seealso>
public static class FluentTaskExtensions
{
    /// <summary>
    /// Validates that the property value is a completed <see cref="Task"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTaskClauses.Completed"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.BackgroundTask).Completed();
    /// </code>
    /// </example>
    /// <seealso cref="MustTaskClauses.Completed"/>
    public static IRuleBuilderOptions<TModel, Task?> Completed<TModel>(this IRuleBuilder<TModel, Task?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Completed(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is not a completed <see cref="Task"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTaskClauses.NotCompleted"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.BackgroundTask).NotCompleted();
    /// </code>
    /// </example>
    /// <seealso cref="MustTaskClauses.NotCompleted"/>
    public static IRuleBuilderOptions<TModel, Task?> NotCompleted<TModel>(this IRuleBuilder<TModel, Task?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotCompleted(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is a canceled <see cref="Task"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTaskClauses.Canceled"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UploadTask).Canceled();
    /// </code>
    /// </example>
    /// <seealso cref="MustTaskClauses.Canceled"/>
    public static IRuleBuilderOptions<TModel, Task?> Canceled<TModel>(this IRuleBuilder<TModel, Task?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Canceled(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is not a canceled <see cref="Task"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTaskClauses.NotCanceled"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.UploadTask).NotCanceled();
    /// </code>
    /// </example>
    /// <seealso cref="MustTaskClauses.NotCanceled"/>
    public static IRuleBuilderOptions<TModel, Task?> NotCanceled<TModel>(this IRuleBuilder<TModel, Task?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotCanceled(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is a faulted <see cref="Task"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTaskClauses.Faulted"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ProcessingTask).Faulted();
    /// </code>
    /// </example>
    /// <seealso cref="MustTaskClauses.Faulted"/>
    public static IRuleBuilderOptions<TModel, Task?> Faulted<TModel>(this IRuleBuilder<TModel, Task?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Faulted(val, paramName: null),
            message);

    /// <summary>
    /// Validates that the property value is not a faulted <see cref="Task"/>.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustTaskClauses.NotFaulted"/>. If the value is <see langword="null"/>,
    /// validation passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.ProcessingTask).NotFaulted();
    /// </code>
    /// </example>
    /// <seealso cref="MustTaskClauses.NotFaulted"/>
    public static IRuleBuilderOptions<TModel, Task?> NotFaulted<TModel>(this IRuleBuilder<TModel, Task?> ruleBuilder, string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotFaulted(val, paramName: null),
            message);
}
