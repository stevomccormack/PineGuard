using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate widget state.
/// </summary>
public static class MustWidgetClauses
{
    /// <summary>Validates that the widget is assembled.</summary>
    public static MustResult<bool> Assembled(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be assembled.";
        return MustResult<bool>.FromBool(value, MustCodes.Widget.State.Broken, messageTemplate, paramName, value, result: true);
    }

    /// <summary>Validates that the widget is broken.</summary>
    public static MustResult<bool> Broken(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be broken.";
        return MustResult<bool>.FromBool(!value, MustCodes.Widget.State.Assembled, messageTemplate, paramName, value, result: false);
    }
}
