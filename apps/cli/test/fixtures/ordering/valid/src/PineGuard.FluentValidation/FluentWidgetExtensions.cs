using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for widget property validation.
/// </summary>
public static class FluentWidgetExtensions
{
    public static IRuleBuilderOptions<TModel, string> NotEmpty<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotEmpty(val, paramName: null),
            message, MustCodes.Widget.Emptiness.Empty);

    public static IRuleBuilderOptions<TModel, string> NotDuplicate<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotDuplicate(val, paramName: null),
            message, MustCodes.Widget.Duplication.Duplicate);
}
