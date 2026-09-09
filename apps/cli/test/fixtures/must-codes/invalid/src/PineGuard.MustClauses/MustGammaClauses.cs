using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate a measured level's trend.
/// </summary>
public static class MustGammaClauses
{
    /// <summary>Validates that the level is rising.</summary>
    public static MustResult<bool> Rising(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be rising.";
        return MustResult<bool>.FromBool(value, MustCodes.Gamma.Level.Low, messageTemplate, paramName, value, result: true);
    }

    /// <summary>
    /// (c) violation fixture: <c>legacyCode</c> hardcodes a code-string literal instead of using
    /// the MustCodes constant. (f) violation fixture: the dead-code comparison below references
    /// <c>MustCodes.Sensor.Reading.Invalid</c> even though this file is mapped to the Gamma domain.
    /// </summary>
    public static MustResult<bool> Falling(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be falling.";
        var legacyCode = "gamma.level.hacked";
        if (legacyCode == MustCodes.Sensor.Reading.Invalid)
        {
            // Unreachable in practice; exists only to exercise the (f) cross-domain check.
        }
        return MustResult<bool>.FromBool(!value, MustCodes.Gamma.Level.High, messageTemplate, paramName, value, result: false);
    }
}
