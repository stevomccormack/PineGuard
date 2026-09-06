using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate alpha values.
/// Canonical order: Empty, then Duplicate.
/// </summary>
public static class MustAlphaClauses
{
    public static MustResult<string> NotEmpty(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = !AlphaRules.IsEmpty(value);
        return MustResult<string>.FromBool(ok, MustCodes.Alpha.Emptiness.Empty, "{paramName} must not be empty.", paramName, value, value);
    }

    public static MustResult<string> NotDuplicate(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = !AlphaRules.IsDuplicate(value);
        return MustResult<string>.FromBool(ok, MustCodes.Alpha.Duplication.Duplicate, "{paramName} must not be a duplicate.", paramName, value, value);
    }
}
