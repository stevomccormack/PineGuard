using System.Runtime.CompilerServices;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// (f) violation fixture: no MustCodes.&lt;Domain&gt;.cs file's "// Serves:" comment lists this
/// file, so the derived domain map has nowhere to route it.
/// </summary>
public static class MustDeltaClauses
{
    /// <summary>Validates that the widget is locked.</summary>
    public static MustResult<bool> Locked(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be locked.";
        return MustResult<bool>.FromBool(value, "delta.state.unlocked", messageTemplate, paramName, value, result: true);
    }
}
