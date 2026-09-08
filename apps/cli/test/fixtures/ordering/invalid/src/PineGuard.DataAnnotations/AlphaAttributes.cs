using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated alpha value is not empty.
/// Kept in Must's Empty-then-Duplicate order so this layer stays clean —
/// only GuardAlphaClauses is reordered in this fixture.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEmptyAlphaAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Alpha.Emptiness.Empty)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.NotEmpty((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}

/// <summary>
/// Validates that the annotated alpha value is not a duplicate.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDuplicateAlphaAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Alpha.Duplication.Duplicate)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.NotDuplicate((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}
