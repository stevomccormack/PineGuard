using FluentValidation;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for cron expression property validation.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/fluent/cron">Fluent Cron Extensions documentation</seealso>
public static class FluentCronExtensions
{
    /// <summary>
    /// Validates that the property value is a cron expression in the specified format.
    /// </summary>
    /// <typeparam name="TModel">The type of the model being validated.</typeparam>
    /// <param name="ruleBuilder">The FluentValidation rule builder to extend.</param>
    /// <param name="format">The field layout to validate against. Defaults to <see cref="CronFormat.Standard"/>.</param>
    /// <param name="message">An optional custom error message. If <see langword="null"/>, uses the default PineGuard message.</param>
    /// <returns>An <see cref="IRuleBuilderOptions{TModel, TProperty}"/> for further rule chaining.</returns>
    /// <remarks>
    /// Delegates to <see cref="MustCronClauses.CronExpression"/>, so every field is checked against its own
    /// range and the <c>@yearly</c>-style macros and the Quartz-only characters are rejected. Pass
    /// <see cref="CronFormat.WithSeconds"/> when the scheduler reads a six-field expression, because the same
    /// text is valid in one layout and invalid in the other. If the value is <see langword="null"/>, validation
    /// passes (null values should be handled by a separate <c>.NotNull()</c> rule).
    /// </remarks>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Schedule).CronExpression();
    /// RuleFor(x => x.PreciseSchedule).CronExpression(CronFormat.WithSeconds);
    /// </code>
    /// </example>
    /// <seealso cref="MustCronClauses.CronExpression"/>
    public static IRuleBuilderOptions<TModel, string?> CronExpression<TModel>(this IRuleBuilder<TModel, string?> ruleBuilder,
        CronFormat format = CronFormat.Standard,
        string? message = null) =>
        ruleBuilder.MustBe(val => val is not null ? Must.Be.CronExpression(val, format, paramName: null) : MustResult<string>.Ok(null!),
            message, MustCodes.Cron.Expression.Invalid);
}
