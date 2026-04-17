using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid email address.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEmailClauses.Email"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// Uses a permissive email format check. For strict RFC compliance, use <see cref="StrictEmailAttribute"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ContactModel
/// {
///     [Email]
///     public string EmailAddress { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="StrictEmailAttribute"/>
/// <seealso cref="MustEmailClauses.Email"/>
/// <seealso href="https://pineguard.ai/docs/annotations/email">Email Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EmailAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.Email(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid email address using strict
/// RFC 5321/5322 format checking.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEmailClauses.StrictEmail"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// Uses a stricter email format check than <see cref="EmailAttribute"/>. Some technically valid but unusual
/// addresses may be rejected.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ContactModel
/// {
///     [StrictEmail]
///     public string EmailAddress { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="EmailAttribute"/>
/// <seealso cref="MustEmailClauses.StrictEmail"/>
/// <seealso href="https://pineguard.ai/docs/annotations/email">Email Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class StrictEmailAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.StrictEmail(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is an email address that contains a
/// plus-sign alias (e.g., <c>user+alias@example.com</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEmailClauses.HasEmailAlias"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FilterModel
/// {
///     [HasEmailAlias]
///     public string AliasedEmail { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHasEmailAliasAttribute"/>
/// <seealso cref="MustEmailClauses.HasEmailAlias"/>
/// <seealso href="https://pineguard.ai/docs/annotations/email">Email Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasEmailAliasAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.HasEmailAlias(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is an email address that does not
/// contain a plus-sign alias.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEmailClauses.NotHasEmailAlias"/>. Supported on properties, fields, and parameters
/// of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RegistrationModel
/// {
///     [NotHasEmailAlias]
///     public string Email { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasEmailAliasAttribute"/>
/// <seealso cref="MustEmailClauses.NotHasEmailAlias"/>
/// <seealso href="https://pineguard.ai/docs/annotations/email">Email Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHasEmailAliasAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.NotHasEmailAlias(strValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
