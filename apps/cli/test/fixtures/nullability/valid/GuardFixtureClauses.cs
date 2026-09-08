using System;
using System.Runtime.CompilerServices;

namespace PineGuard.GuardClauses;

/// <summary>
/// Fixture Guard clauses for the nullability VIBE valid/ case (plan
/// docs/ai/plans/audit-cli-rebuild.md §4.6): the Guard family shares the
/// same "second parameter" primary-parameter position as Must clauses (both
/// put the receiver first), so the same policy applies.
/// </summary>
public static class GuardFixtureClauses
{
    /// <summary>Reference type (string), correctly nullable.</summary>
    public static string NotNullOrEmpty(this IGuardClause _,
        string? value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();

    /// <summary>Value type (Guid), correctly non-nullable.</summary>
    public static Guid NotEmpty(this IGuardClause _,
        Guid value,
        string? message = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null) =>
        throw new NotSupportedException();
}
