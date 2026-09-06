using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate ledger entries.
/// </summary>
public static class MustLedgerClauses
{
    /// <summary>Validates that the ledger entry is reconciled.</summary>
    public static MustResult<bool> Reconciled(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be reconciled.";
        return MustResult<bool>.FromBool(value, MustCodes.Ledger.Entry.Invalid, messageTemplate, paramName, value, result: true);
    }
}
