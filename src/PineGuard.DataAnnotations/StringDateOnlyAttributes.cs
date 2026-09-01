#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// in the past.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.PastDateOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [PastDateOnlyString]
///     public string CreatedDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.PastDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastDateOnlyStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Date.Relative.NotPast)
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.PastDateOnly(strValue, Styles, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// in the future.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.FutureDateOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReminderModel
/// {
///     [FutureDateOnlyString]
///     public string ReminderDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.FutureDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureDateOnlyStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Date.Relative.NotFuture)
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.FutureDateOnly(strValue, Styles, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// in the past or equal to today.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.PastOrPresentDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BirthModel
/// {
///     [PastOrPresentDateOnlyString]
///     public string DateOfBirth { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureOrPresentDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.PastOrPresentDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastOrPresentDateOnlyStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Date.Relative.Future)
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.PastOrPresentDateOnly(strValue, Styles, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// in the future or equal to today.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.FutureOrPresentDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ExpiryModel
/// {
///     [FutureOrPresentDateOnlyString]
///     public string ExpiryDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastOrPresentDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.FutureOrPresentDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureOrPresentDateOnlyStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Date.Relative.Past)
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.FutureOrPresentDateOnly(strValue, Styles, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// strictly before the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.BeforeDateOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// The <see cref="Styles"/> property controls how the annotated value is parsed; defaults to
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ContractModel
/// {
///     [BeforeDateOnlyString("2025-01-01")]
///     public string StartDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotBeforeDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.BeforeDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BeforeDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.NotBefore)
{
    /// <summary>Gets the reference date that the parsed value must precede.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.BeforeDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// that is not strictly before the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.NotBeforeDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EligibilityModel
/// {
///     [NotBeforeDateOnlyString("2000-01-01")]
///     public string BirthDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BeforeDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.NotBeforeDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBeforeDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.Before)
{
    /// <summary>Gets the reference date; the parsed value must not precede it.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotBeforeDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// on or before the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.OnOrBeforeDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DeadlineModel
/// {
///     [OnOrBeforeDateOnlyString("2024-12-31")]
///     public string SubmissionDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOnOrBeforeDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.OnOrBeforeDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrBeforeDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.After)
{
    /// <summary>Gets the reference date that the parsed value must not exceed.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OnOrBeforeDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// that is not on or before the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.NotOnOrBeforeDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OpenPeriodModel
/// {
///     [NotOnOrBeforeDateOnlyString("2024-01-01")]
///     public string StartDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrBeforeDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.NotOnOrBeforeDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOnOrBeforeDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.NotAfter)
{
    /// <summary>Gets the reference date; the parsed value must be strictly after it.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotOnOrBeforeDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// strictly after the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.AfterDateOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PromoModel
/// {
///     [AfterDateOnlyString("2020-01-01")]
///     public string StartDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotAfterDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.AfterDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AfterDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.NotAfter)
{
    /// <summary>Gets the reference date that the parsed value must follow.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.AfterDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// that is not strictly after the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.NotAfterDateOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CutoffModel
/// {
///     [NotAfterDateOnlyString("2025-12-31")]
///     public string Cutoff { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AfterDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.NotAfterDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotAfterDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.After)
{
    /// <summary>Gets the reference date; the parsed value must not be strictly after it.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotAfterDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// on or after the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.OnOrAfterDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EligibilityModel
/// {
///     [OnOrAfterDateOnlyString("2000-01-01")]
///     public string EffectiveDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOnOrAfterDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.OnOrAfterDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrAfterDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.Before)
{
    /// <summary>Gets the reference date that the parsed value must meet or follow.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OnOrAfterDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="DateOnly"/>
/// that is not on or after the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.NotOnOrAfterDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PastRangeModel
/// {
///     [NotOnOrAfterDateOnlyString("2030-01-01")]
///     public string ValidUntil { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrAfterDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.NotOnOrAfterDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOnOrAfterDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.NotBefore)
{
    /// <summary>Gets the reference date; the parsed value must be strictly before it.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotOnOrAfterDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents the same
/// <see cref="DateOnly"/> as the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.SameDateOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class HolidayModel
/// {
///     [SameDateOnlyString("2024-01-01")]
///     public string HolidayDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotSameDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.SameDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SameDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Equality.NotEqual)
{
    /// <summary>Gets the reference date that the parsed value must equal.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.SameDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field does not represent the same
/// <see cref="DateOnly"/> as the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.NotSameDateOnly"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ExclusionModel
/// {
///     [NotSameDateOnlyString("2024-01-01")]
///     public string Date { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="SameDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.NotSameDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotSameDateOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Date.Equality.Equal)
{
    /// <summary>Gets the reference date that the parsed value must not equal.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotSameDateOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field (as a start date) is
/// chronologically before the specified end date string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.ChronologicalDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The annotated value is treated as the start; <see cref="End"/> is the raw end string. Both are parsed
/// at validation time using <see cref="Styles"/>. Defaults to <see cref="Inclusion.Exclusive"/> boundaries.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RangeModel
/// {
///     [ChronologicalDateOnlyString("2024-12-31")]
///     public string StartDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotChronologicalDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.ChronologicalDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ChronologicalDateOnlyStringAttribute(string end, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.NotChronological)
{
    /// <summary>Gets the end date string for the chronological range.</summary>
    public string End { get; } = end;

    /// <summary>Gets whether the end boundary is included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing both string values.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ChronologicalDateOnly(strValue, End, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field (as a start date) is not
/// chronologically before the specified end date string.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.NotChronologicalDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The annotated value is treated as the start; <see cref="End"/> is the raw end string. Both are parsed
/// at validation time using <see cref="Styles"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [NotChronologicalDateOnlyString("2024-01-01")]
///     public string UpdatedDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ChronologicalDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.NotChronologicalDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotChronologicalDateOnlyStringAttribute(string end, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Date.Order.Chronological)
{
    /// <summary>Gets the end date string used to check the non-chronological constraint.</summary>
    public string End { get; } = end;

    /// <summary>Gets whether the end boundary is included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing both string values.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotChronologicalDateOnly(strValue, End, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the date interval [annotated string start, <see cref="End1"/>] overlaps with the
/// interval [<see cref="Start2"/>, <see cref="End2"/>].
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.OverlappingDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The annotated value is treated as the start of the first interval. All boundary strings are passed
/// as-is and parsed at validation time using <see cref="Styles"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReservationModel
/// {
///     [OverlappingDateOnlyString("2024-06-10", "2024-06-08", "2024-06-12")]
///     public string CheckIn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOverlappingDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.OverlappingDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OverlappingDateOnlyStringAttribute(string end1, string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Date.Overlap.Missing)
{
    /// <summary>Gets the end of the first interval as a string.</summary>
    public string End1 { get; } = end1;

    /// <summary>Gets the start of the second interval as a string.</summary>
    public string Start2 { get; } = start2;

    /// <summary>Gets the end of the second interval as a string.</summary>
    public string End2 { get; } = end2;

    /// <summary>Gets whether the interval boundaries are included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing all string values.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OverlappingDateOnly(strValue, End1, Start2, End2, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the date interval [annotated string start, <see cref="End1"/>] does not overlap with
/// the interval [<see cref="Start2"/>, <see cref="End2"/>].
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.NotOverlappingDateOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The annotated value is treated as the start of the first interval. All boundary strings are passed
/// as-is and parsed at validation time using <see cref="Styles"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BlockoutModel
/// {
///     [NotOverlappingDateOnlyString("2024-06-10", "2024-06-15", "2024-06-20")]
///     public string PeriodStart { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OverlappingDateOnlyStringAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.NotOverlappingDateOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOverlappingDateOnlyStringAttribute(string end1, string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Date.Overlap.Present)
{
    /// <summary>Gets the end of the first interval as a string.</summary>
    public string End1 { get; } = end1;

    /// <summary>Gets the start of the second interval as a string.</summary>
    public string Start2 { get; } = start2;

    /// <summary>Gets the end of the second interval as a string.</summary>
    public string End2 { get; } = end2;

    /// <summary>Gets whether the interval boundaries are included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing all string values.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotOverlappingDateOnly(strValue, End1, Start2, End2, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="DateOnly"/> date of birth that meets the expected minimum age.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateOnlyClauses.MinimumAge"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>. A value that does not parse fails validation.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// <para>
/// The clock supplying today's date is resolved from the validation context's service provider: an
/// attribute argument must be a compile-time constant, which a <see cref="TimeProvider"/> is not. Register
/// one to validate against a fixed instant; with no registration the system clock applies.
/// </para>
/// <para>
/// A negative minimum age is a configuration error and fails validation.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RegistrationModel
/// {
///     [MinimumAgeString(18)]
///     public string DateOfBirth { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MinimumAgeAttribute"/>
/// <seealso cref="MustStringDateOnlyClauses.MinimumAge"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class MinimumAgeStringAttribute(int years) : ValidationAttributeBase(typeof(string), MustCodes.Date.Age.BelowMinimum)
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets the minimum age, in whole years, the date of birth must satisfy.</summary>
    public int Years { get; } = years;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.MinimumAge(strValue, Years, Styles, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
