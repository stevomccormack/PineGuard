namespace PineGuard.Rules;

/// <summary>
/// Low-level predicates backing <c>MustBravoClauses</c>.
/// Kept in Must's Stale-then-Archived order — this fixture isolates the
/// ambiguous-delegation finding to GuardBravoClauses only.
/// </summary>
public static class BravoRules
{
    public static bool IsStale(string value) => false;

    public static bool IsArchived(string value) => false;
}
