using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for widget state.
/// </summary>
public static class GuardWidgetClauses
{
    /// <summary>
    /// (e) violation fixture: GuardFailure.Throw's first argument is a string literal, not the
    /// IMustResult that Must.Be.Assembled just produced.
    /// </summary>
    public static bool Broken(this IGuardClause _,
        bool value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.Assembled(value, paramName);
        if (result.Failed)
            GuardFailure.Throw("value must be assembled.", exceptionCreator);

        return result.Result;
    }
}
