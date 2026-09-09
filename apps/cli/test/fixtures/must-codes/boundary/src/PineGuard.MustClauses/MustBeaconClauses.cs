using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate beacon signal state.
/// </summary>
public static class MustBeaconClauses
{
    /// <summary>Validates that the beacon is online.</summary>
    public static MustResult<bool> Online(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be online.";
        return MustResult<bool>.FromBool(value, MustCodes.Beacon.Signal.Lost, messageTemplate, paramName, value, result: true);
    }
}
