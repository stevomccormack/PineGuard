using System.Runtime.CompilerServices;
using PineGuard.Codes;
using PineGuard.Rules;

namespace PineGuard.MustClauses;

/// <summary>
/// Provides <see cref="IMustClause"/> extension methods that validate sensor readings.
/// </summary>
public static class MustSensorClauses
{
    /// <summary>Validates that the sensor is calibrated.</summary>
    public static MustResult<bool> Calibrated(this IMustClause _,
        bool value,
        [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        const string messageTemplate = "{paramName} must be calibrated.";
        return MustResult<bool>.FromBool(value, MustCodes.Sensor.Reading.Invalid, messageTemplate, paramName, value, result: true);
    }
}
