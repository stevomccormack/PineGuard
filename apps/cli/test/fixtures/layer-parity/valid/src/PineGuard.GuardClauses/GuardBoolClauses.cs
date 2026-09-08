namespace PineGuard.GuardClauses;

/// <summary>Guard clauses for boolean values (layer-parity valid/ fixture).</summary>
public static class GuardBoolClauses
{
    public static bool True(this IGuardClause _, bool value)
    {
        return value;
    }
}
