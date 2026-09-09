namespace PineGuard.MustClauses;

/// <summary>Must clauses for boolean values (layer-parity valid/ fixture).</summary>
public static class MustBoolClauses
{
    public static MustResult<bool> True(this IMustClause _, bool value)
    {
        return MustResult<bool>.FromBool(value);
    }
}
