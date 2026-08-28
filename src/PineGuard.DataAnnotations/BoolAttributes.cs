using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="bool"/> property or field is <see langword="true"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBoolClauses.True"/>. Supported on properties, fields, and parameters
/// of type <see cref="bool"/>.
/// </para>
/// <para>
/// For nullable booleans, ensure the value is not <see langword="null"/> before this attribute runs
/// (e.g., combine with <c>[Required]</c>).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class UserModel
/// {
///     [True]
///     public bool AcceptedTerms { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FalseAttribute"/>
/// <seealso cref="MustBoolClauses.True"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bool">Bool Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TrueAttribute() : ValidationAttributeBase(typeof(bool), MustCodes.Boolean.Value.False)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var boolValue = (bool)value!;

        var result = Must.Be.True(boolValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="bool"/> property or field is <see langword="false"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustBoolClauses.False"/>. Supported on properties, fields, and parameters
/// of type <see cref="bool"/>.
/// </para>
/// <para>
/// For nullable booleans, ensure the value is not <see langword="null"/> before this attribute runs
/// (e.g., combine with <c>[Required]</c>).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SettingsModel
/// {
///     [False]
///     public bool IsDisabled { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TrueAttribute"/>
/// <seealso cref="MustBoolClauses.False"/>
/// <seealso href="https://pineguard.ai/docs/annotations/bool">Bool Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FalseAttribute() : ValidationAttributeBase(typeof(bool), MustCodes.Boolean.Value.True)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var boolValue = (bool)value!;

        var result = Must.Be.False(boolValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
