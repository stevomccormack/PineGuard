using FluentValidation;
using PineGuard.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation.Common;

/// <summary>
/// Provides the bridge between FluentValidation's <see cref="IRuleBuilder{T, TProperty}"/> and PineGuard's
/// <see cref="MustResult{T}"/>-based validation rules.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/common">Fluent Common Extensions documentation</seealso>
public static class FluentExtension
{
    private const string ParamNameToken = "{paramName}";

    /// <summary>
    /// Validates the property value by delegating to a PineGuard <see cref="MustResult{T}"/>-returning check function.
    /// </summary>
    /// <typeparam name="T">The type of the model being validated.</typeparam>
    /// <typeparam name="TProp">The type of the property being validated.</typeparam>
    /// <typeparam name="TResult">The result type returned by the PineGuard check.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="check">A function that accepts the property value and returns a <see cref="MustResult{T}"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the message from the <see cref="MustResult{T}"/>.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{T, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// This overload validates the property value in isolation. The model instance is not available to the check function.
    /// </remarks>
    /// <example>
    /// <code>
    /// ruleBuilder.MustBe(val => Must.Be.NotNull(val, paramName: null), message);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, TProp> MustBe<T, TProp, TResult>(this IRuleBuilder<T, TProp> ruleBuilder,
        Func<TProp, MustResult<TResult>> check,
        string? message)
    {
        ThrowHelper.ThrowIfNull(ruleBuilder);
        ThrowHelper.ThrowIfNull(check);

        return ruleBuilder
            .Must((_, value, context) =>
            {
                var result = check(value);
                if (result.Success)
                    return true;

                var propertyName = GetPropertyName(context);
                var errorMessage = FormatMessage(message ?? result.Message, propertyName);
                context.MessageFormatter.AppendArgument("ErrorMessage", errorMessage);
                return false;
            })
            .WithMessage("{ErrorMessage}");
    }

    /// <summary>
    /// Validates the property value by delegating to a PineGuard <see cref="MustResult{T}"/>-returning check function
    /// that also receives the model instance.
    /// </summary>
    /// <typeparam name="T">The type of the model being validated.</typeparam>
    /// <typeparam name="TProp">The type of the property being validated.</typeparam>
    /// <typeparam name="TResult">The result type returned by the PineGuard check.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="check">A function that accepts the model and property value and returns a <see cref="MustResult{T}"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the message from the <see cref="MustResult{T}"/>.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{T, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// This overload provides access to the model instance, enabling cross-property validation scenarios
    /// such as comparing start and end dates on the same model.
    /// </remarks>
    /// <example>
    /// <code>
    /// ruleBuilder.MustBe((model, val) => Must.Be.Chronological(val, model.EndDate, paramName: null), message);
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, TProp> MustBe<T, TProp, TResult>(this IRuleBuilder<T, TProp> ruleBuilder,
        Func<T, TProp, MustResult<TResult>> check,
        string? message)
    {
        ThrowHelper.ThrowIfNull(ruleBuilder);
        ThrowHelper.ThrowIfNull(check);

        return ruleBuilder
            .Must((model, value, context) =>
            {
                var result = check(model, value);
                if (result.Success)
                    return true;

                var propertyName = GetPropertyName(context);
                var errorMessage = FormatMessage(message ?? result.Message, propertyName);
                context.MessageFormatter.AppendArgument("ErrorMessage", errorMessage);
                return false;
            })
            .WithMessage("{ErrorMessage}");
    }

    /// <summary>
    /// Validates a nullable value-type property by delegating to a PineGuard <see cref="MustResult{T}"/>-returning check function.
    /// </summary>
    /// <typeparam name="T">The type of the model being validated.</typeparam>
    /// <typeparam name="TProp">The underlying value type of the nullable property being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="check">A function that accepts the nullable property value and returns a <see cref="MustResult{T}"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the message from the <see cref="MustResult{T}"/>.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{T, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// This overload forwards to <see cref="MustBe{T, TProp, TResult}(IRuleBuilder{T, TProp}, Func{TProp, MustResult{TResult}}, string?)"/>
    /// and exists to simplify the generic inference for nullable struct properties.
    /// </remarks>
    public static IRuleBuilderOptions<T, TProp?> MustBe<T, TProp>(
        this IRuleBuilder<T, TProp?> ruleBuilder,
        Func<TProp?, MustResult<TProp?>> check,
        string? message)
        where TProp : struct
        => ruleBuilder.MustBe<T, TProp?, TProp?>(check, message);

    private static string FormatMessage(string template, string paramName) =>
        template.Replace(ParamNameToken, paramName, StringComparison.Ordinal);

    /// <remarks>
    /// Falls back to <see cref="ValidationContext{T}.PropertyPath"/> (never <see langword="null"/> once a
    /// property validator is initialized) when <see cref="ValidationContext{T}.DisplayName"/> is blank.
    /// </remarks>
    private static string GetPropertyName<T>(ValidationContext<T> context)
    {
        var displayName = context.DisplayName;
        return string.IsNullOrWhiteSpace(displayName) ? context.PropertyPath : displayName;
    }
}
