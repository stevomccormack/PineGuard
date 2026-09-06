using FluentValidation;
using PineGuard.Codes;
using PineGuard.FluentValidation.Common;
using PineGuard.MustClauses;

namespace PineGuard.FluentValidation;

/// <summary>
/// Provides FluentValidation extension methods for bravo property validation.
/// Kept in Must's Stale-then-Archived order — this fixture isolates the
/// ambiguous-delegation finding to GuardBravoClauses only.
/// </summary>
public static class FluentBravoExtensions
{
    public static IRuleBuilderOptions<TModel, string> NotStale<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotStale(val, paramName: null),
            message, MustCodes.Bravo.Staleness.Stale);

    public static IRuleBuilderOptions<TModel, string> NotArchived<TModel>(
        this IRuleBuilder<TModel, string> ruleBuilder,
        string? message = null) =>
        ruleBuilder.MustBe(val => Must.Be.NotArchived(val, paramName: null),
            message, MustCodes.Bravo.Archival.Archived);
}
