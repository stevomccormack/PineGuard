using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated widget value is not empty.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEmptyWidgetAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Widget.Emptiness.Empty)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.NotEmpty((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}

/// <summary>
/// Validates that the annotated widget value is not a duplicate.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDuplicateWidgetAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Widget.Duplication.Duplicate)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.NotDuplicate((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}
