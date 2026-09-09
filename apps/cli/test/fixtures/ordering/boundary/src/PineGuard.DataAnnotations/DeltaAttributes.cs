using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated delta value is a known instance.
/// Kept in Must's Instance-then-Past order.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InstanceDeltaAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Delta.Instancing.Instance)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.Instance((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}

/// <summary>
/// Validates that the annotated delta value is in the past.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastDeltaAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Delta.Timing.Past)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.Past((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}
