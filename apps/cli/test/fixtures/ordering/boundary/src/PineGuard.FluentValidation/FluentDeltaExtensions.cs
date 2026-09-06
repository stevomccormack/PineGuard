using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for delta property validation.
/// Kept in Must's Instance-then-Past order.
/// </summary>
public static class FluentDeltaExtensions
{
    public static IRuleBuilderOptions<TModel, string> Instance<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Instance(val, paramName: null),
            message, MustCodes.Delta.Instancing.Instance);

    public static IRuleBuilderOptions<TModel, string> Past<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.Past(val, paramName: null),
            message, MustCodes.Delta.Timing.Past);
}
