using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated widget property is assembled.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AssembledAttribute() : ValidationAttributeBase(typeof(bool), MustCodes.Widget.State.Broken)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var boolValue = (bool)value!;

        var result = Must.Be.Assembled(boolValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
