using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate bravo values.
/// Canonical order: Stale, then Archived.
/// </summary>
public static class MustBravoClauses
{
    public static MustResult<string> NotStale(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = !BravoRules.IsStale(value);
        return MustResult<string>.FromBool(ok, MustCodes.Bravo.Staleness.Stale, "{paramName} must not be stale.", paramName, value, value);
    }

    public static MustResult<string> NotArchived(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = !BravoRules.IsArchived(value);
        return MustResult<string>.FromBool(ok, MustCodes.Bravo.Archival.Archived, "{paramName} must not be archived.", paramName, value, value);
    }
}
