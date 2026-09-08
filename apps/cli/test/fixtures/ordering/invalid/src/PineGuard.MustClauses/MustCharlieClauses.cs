using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate charlie values.
/// INVALID: no GuardCharlieClauses, FluentCharlieExtensions, CharlieAttributes,
/// or CharlieRules sibling exists anywhere in this fixture tree — this family
/// must produce a `missing-layer` finding for each of the four sibling
/// layers (the direct proof that a missing sibling layer is now a finding,
/// not a warning; the legacy tool only warned here).
/// </summary>
public static class MustCharlieClauses
{
    public static MustResult<string> NotCorrupt(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = !CharlieRules.IsCorrupt(value);
        return MustResult<string>.FromBool(ok, MustCodes.Charlie.Corruption.Corrupt, "{paramName} must not be corrupt.", paramName, value, value);
    }
}
