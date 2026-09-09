using System.Runtime.CompilerServices;
using PineGuard.MustClauses;

namespace PineGuard.GuardClauses;

/// <summary>
/// Guard clauses for alpha values.
/// INVALID: declared as Duplicate, then Empty — the reverse of
/// MustAlphaClauses' Empty-then-Duplicate order — so this must produce an
/// `ordering` order-mismatch finding for the GuardClauses layer.
/// </summary>
public static class GuardAlphaClauses
{
    public static string Duplicate(
        this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotDuplicate(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }

    public static string Empty(
        this IGuardClause _,
        string value,
        string? message = null,
        Func<Exception>? exceptionCreator = null,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        var result = Must.Be.NotEmpty(value, paramName);
        if (result.Failed)
            GuardFailure.Throw(result, message, exceptionCreator);

        return result.Result;
    }
}
