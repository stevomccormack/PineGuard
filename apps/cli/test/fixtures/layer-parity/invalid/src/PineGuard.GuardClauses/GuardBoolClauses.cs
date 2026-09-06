namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for boolean values (layer-parity invalid/ fixture).
/// Deliberately implements only "True" — "NullOrEmpty" (present in Must)
/// is never wired up here.
/// </summary>
public static class GuardBoolClauses
{
    public static bool True(this IGuardClause _, bool value)
    {
        return value;
    }
}
