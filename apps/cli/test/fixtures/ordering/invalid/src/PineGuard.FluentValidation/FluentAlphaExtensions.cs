using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for alpha property validation.
/// Kept in Must's Empty-then-Duplicate order so this layer stays clean —
/// only GuardAlphaClauses is reordered in this fixture.
/// </summary>
public static class FluentAlphaExtensions
{
    public static IRuleBuilderOptions<TModel, string> NotEmpty<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotEmpty(val, paramName: null),
            message, MustCodes.Alpha.Emptiness.Empty);

    public static IRuleBuilderOptions<TModel, string> NotDuplicate<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotDuplicate(val, paramName: null),
            message, MustCodes.Alpha.Duplication.Duplicate);
}
