using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid phone number using
/// default digit-length constraints (7–25 digits).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustPhoneClauses.PhoneNumberString"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// For custom digit-length requirements, use <see cref="CustomPhoneNumberAttribute"/>.
/// For ITU-T E.164 international phone numbers, use <see cref="InternationalPhoneNumberAttribute"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ContactModel
/// {
///     [PhoneNumber]
///     public string Phone { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="CustomPhoneNumberAttribute"/>
/// <seealso cref="MustPhoneClauses.PhoneNumberString"/>
/// <seealso href="https://pineguard.ai/docs/annotations/phone">Phone Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PhoneNumberAttribute() : ValidationAttributeBase(typeof(string))
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.PhoneNumberString(strValue, minDigits: 7, maxDigits: 25, allowedNonDigitCharacters: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a valid phone number with
/// the specified minimum and maximum digit counts.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustPhoneClauses.PhoneNumberString"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ContactModel
/// {
///     [CustomPhoneNumber(MinDigits = 10, MaxDigits = 11)]
///     public string PhoneNumber { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PhoneNumberAttribute"/>
/// <seealso cref="MustPhoneClauses.PhoneNumberString"/>
/// <seealso href="https://pineguard.ai/docs/annotations/phone">Phone Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CustomPhoneNumberAttribute(int minDigits, int maxDigits) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the minimum number of digits required in the phone number.</summary>
    public int MinDigits { get; } = minDigits;

    /// <summary>Gets the maximum number of digits permitted in the phone number.</summary>
    public int MaxDigits { get; } = maxDigits;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.PhoneNumberString(strValue, MinDigits, MaxDigits, allowedNonDigitCharacters: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
