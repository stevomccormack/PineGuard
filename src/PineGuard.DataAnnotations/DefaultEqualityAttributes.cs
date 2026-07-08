using System.ComponentModel.DataAnnotations;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

// NOTE: The Default / NotDefault members of MustDefaultEqualityClauses are already surfaced as
// IsDefaultAttribute and NotDefaultAttribute in ObjectAttributes.cs. Only the NullOrDefault and
// NotNullOrDefault members lacked DataAnnotations wrappers, so they are added here. All four attributes
// share the reflection-based ObjectAttributeBase, which resolves the value's runtime type at validation
// time — a boxed value type such as int cannot be validated as default(object) (always null), so the
// concrete type is required for a correct comparison.

/// <summary>
/// Validates that the annotated property or field is <see langword="null"/> or equal to the default value
/// for its type (e.g., <c>0</c> for numeric types, <see langword="null"/> for reference types).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDefaultEqualityClauses.NullOrDefault{T}"/>. Supported on properties, fields,
/// and parameters of any type. The default value is inferred at runtime from the value's actual type, or,
/// when the value is <see langword="null"/>, from the annotated member's declared type.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FilterModel
/// {
///     [NullOrDefault]
///     public int? MaxResults { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotNullOrDefaultAttribute"/>
/// <seealso cref="MustDefaultEqualityClauses.NullOrDefault{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NullOrDefaultAttribute : ObjectAttributeBase
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeGenericMust(nameof(MustDefaultEqualityClauses.NullOrDefault), value, validationContext);
}

/// <summary>
/// Validates that the annotated property or field is neither <see langword="null"/> nor equal to the
/// default value for its type.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDefaultEqualityClauses.NotNullOrDefault{T}"/>. Supported on properties,
/// fields, and parameters of any type. The default value is inferred at runtime from the value's actual
/// type, or, when the value is <see langword="null"/>, from the annotated member's declared type.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OrderModel
/// {
///     [NotNullOrDefault]
///     public Guid OrderId { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NullOrDefaultAttribute"/>
/// <seealso cref="MustDefaultEqualityClauses.NotNullOrDefault{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNullOrDefaultAttribute : ObjectAttributeBase
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeGenericMust(nameof(MustDefaultEqualityClauses.NotNullOrDefault), value, validationContext);
}
