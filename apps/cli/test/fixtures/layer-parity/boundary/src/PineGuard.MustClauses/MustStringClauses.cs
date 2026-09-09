namespace PineGuard.MustClauses;

/// <summary>Must clauses for strings (layer-parity boundary/ fixture).</summary>
public static class MustStringClauses
{
    public static MustResult<string> NullOrEmpty(this IMustClause _, string? value)
    {
        return MustResult<string>.FromBool(string.IsNullOrEmpty(value), value);
    }

    public static MustResult<string> Null(this IMustClause _, string? value)
    {
        return MustResult<string>.FromBool(value is null, value);
    }

    /// <summary>
    /// No alias or strip-prefix rule resolves this to anything Guard
    /// implements below — this is the genuine gap the boundary probe
    /// contrasts against the two resolved-via-vocabulary concepts above.
    /// </summary>
    public static MustResult<string> Extra(this IMustClause _, string? value)
    {
        return MustResult<string>.FromBool(true, value);
    }
}
