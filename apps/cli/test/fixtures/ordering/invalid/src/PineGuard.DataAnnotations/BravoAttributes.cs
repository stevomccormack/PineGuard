using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated bravo value is not stale.
/// Kept in Must's Stale-then-Archived order — this fixture isolates the
/// ambiguous-delegation finding to GuardBravoClauses only.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotStaleBravoAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Bravo.Staleness.Stale)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.NotStale((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}

/// <summary>
/// Validates that the annotated bravo value is not archived.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotArchivedBravoAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Bravo.Archival.Archived)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.NotArchived((string?)value ?? string.Empty);
        return result.Failed ? new ValidationResult(result.Message) : ValidationResult.Success;
    }
}
