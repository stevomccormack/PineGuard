namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated value is true (layer-parity invalid/
/// fixture). Only calls Must.Be.True — Must.Be.NullOrEmpty is never called
/// anywhere in this layer, exercising the must-call extraction path
/// alongside the extension-method path (Guard, above).
/// </summary>
public sealed class TrueAttribute : ValidationAttributeBase
{
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.True((bool)value!, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
