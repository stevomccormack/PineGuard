using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// (d) violation fixture: declares <c>Widget.State.Broken</c> but its dispatch
/// (<c>Must.Be.Broken</c>) actually produces <c>Widget.State.Assembled</c> — the DataAnnotations
/// code and the Must clause it wraps have drifted apart.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BrokenAttribute() : ValidationAttributeBase(typeof(bool), MustCodes.Widget.State.Broken)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var boolValue = (bool)value!;

        var result = Must.Be.Broken(boolValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
