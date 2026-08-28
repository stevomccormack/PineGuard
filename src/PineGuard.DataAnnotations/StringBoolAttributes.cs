using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see langword="true"/> boolean value (e.g., <c>"true"</c>, <c>"yes"</c>, <c>"1"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringBoolClauses.True"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ConsentModel
/// {
///     [TrueString]
///     public string ConsentGiven { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FalseStringAttribute"/>
/// <seealso cref="MustStringBoolClauses.True"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class TrueStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Boolean.Value.False)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.True(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see langword="false"/> boolean value (e.g., <c>"false"</c>, <c>"no"</c>, <c>"0"</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringBoolClauses.False"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OptOutModel
/// {
///     [FalseString]
///     public string MarketingOptOut { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="TrueStringAttribute"/>
/// <seealso cref="MustStringBoolClauses.False"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FalseStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Boolean.Value.True)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.False(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
