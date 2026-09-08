using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate widget values.
/// </summary>
public static class MustWidgetClauses
{
    public static MustResult<string> NotEmpty(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = !WidgetRules.IsEmpty(value);
        return MustResult<string>.FromBool(ok, MustCodes.Widget.Emptiness.Empty, "{paramName} must not be empty.", paramName, value, value);
    }

    public static MustResult<string> NotDuplicate(
        this IMustClause _,
        string value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var ok = !WidgetRules.IsDuplicate(value);
        return MustResult<string>.FromBool(ok, MustCodes.Widget.Duplication.Duplicate, "{paramName} must not be a duplicate.", paramName, value, value);
    }
}
