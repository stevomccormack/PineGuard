namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for strings (layer-parity boundary/ fixture). "NotBlank"
/// is textually different from Must's "NullOrEmpty" but resolves to the
/// same concept via this fixture's vocabulary.json alias; "NotNull"
/// resolves to Must's "Null" via the same vocabulary's stripPrefixes
/// ("Not"). Neither should be flagged. "Extra" (Must) has no counterpart
/// here at all and should still be flagged.
/// </summary>
public static class GuardStringClauses
{
    public static string? NotBlank(this IGuardClause _, string? value)
    {
        return value;
    }

    public static string? NotNull(this IGuardClause _, string? value)
    {
        return value;
    }
}
