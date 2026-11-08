namespace PineGuard.MustClauses;

/// <summary>Must clauses for boolean values (layer-parity valid/ fixture).</summary>
public static class MustBoolClauses
{
    public static MustResult<bool> True(this IMustClause _, bool value)
    {
        return MustResult<bool>.FromBool(value);
    }

    public static bool HasXmlRoot(this IMustClause _, string value) => true;
    public static bool Positive(this IMustClause _, int value) => true;

}
