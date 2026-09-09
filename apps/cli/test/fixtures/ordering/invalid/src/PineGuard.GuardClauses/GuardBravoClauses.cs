using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for bravo values.
/// INVALID: Archived's body invokes two distinct Must targets
/// (Must.Be.NotStale and Must.Be.NotArchived) — an ambiguous delegation the
/// `ordering` rule must flag, since a Guard method should delegate to
/// exactly one Must clause.
/// </summary>
public static class GuardBravoClauses
{
    public static string Stale(
        this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotStale(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    public static string Archived(
        this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var staleResult = Must.Be.NotStale(value, paramName);
        var archivedResult = Must.Be.NotArchived(value, paramName);
        if (staleResult.Failed || archivedResult.Failed)
            GuardFailure.Throw(archivedResult, message, exceptionCreator);

        return archivedResult.Result;
    }
}
