namespace PineGuard.MustClauses;

/// <summary>Must clauses for strings (layer-parity invalid/ fixture).</summary>
public static class MustStringClauses
{
    public static MustResult<bool> True(this IMustClause _, bool value)
    {
        return MustResult<bool>.FromBool(value);
    }

    /// <summary>
    /// Implemented here in Must but never wired up in Guard or
    /// DataAnnotations below — the genuine parity gap this fixture exists
    /// to trip.
    /// </summary>
    public static MustResult<string> NullOrEmpty(this IMustClause _, string? value)
    {
        return MustResult<string>.FromBool(string.IsNullOrEmpty(value), value);
    }
}
