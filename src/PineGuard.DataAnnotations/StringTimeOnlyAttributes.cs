#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that falls within the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.BetweenTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format using invariant culture. The <see cref="Styles"/> property
/// controls how the annotated value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ShiftModel
/// {
///     [BetweenTimeOnlyString("08:00", "17:00")]
///     public string StartTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotBetweenTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.BetweenTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BetweenTimeOnlyStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the lower boundary of the valid time range.</summary>
    public TimeOnly Min { get; } = TimeOnly.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper boundary of the valid time range.</summary>
    public TimeOnly Max { get; } = TimeOnly.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.BetweenTimeOnly(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is before the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.BeforeTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MorningModel
/// {
///     [BeforeTimeOnlyString("12:00")]
///     public string WakeUpTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AfterTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.BeforeTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BeforeTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must precede.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.BeforeTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that falls outside the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.NotBetweenTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format using invariant culture. The <see cref="Styles"/> property
/// controls how the annotated value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class QuietHoursModel
/// {
///     [NotBetweenTimeOnlyString("22:00", "06:00")]
///     public string AlertTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BetweenTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.NotBetweenTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBetweenTimeOnlyStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the lower boundary of the excluded time range.</summary>
    public TimeOnly Min { get; } = TimeOnly.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper boundary of the excluded time range.</summary>
    public TimeOnly Max { get; } = TimeOnly.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary values are included or excluded in the excluded range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotBetweenTimeOnly(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is after the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.AfterTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EveningModel
/// {
///     [AfterTimeOnlyString("18:00")]
///     public string DinnerTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BeforeTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.AfterTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AfterTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must follow.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.AfterTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is not before the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.NotBeforeTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OpeningModel
/// {
///     [NotBeforeTimeOnlyString("09:00")]
///     public string ServiceStart { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BeforeTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.NotBeforeTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBeforeTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must not precede.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotBeforeTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is on or before the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.OnOrBeforeTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DeadlineModel
/// {
///     [OnOrBeforeTimeOnlyString("23:59")]
///     public string Submission { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOnOrBeforeTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.OnOrBeforeTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrBeforeTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must be on or before.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OnOrBeforeTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is not on or before the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.NotOnOrBeforeTimeOnly"/>. Supported on
/// properties, fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [NotOnOrBeforeTimeOnlyString("08:00")]
///     public string MeetingTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrBeforeTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.NotOnOrBeforeTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOnOrBeforeTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must not be on or before.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotOnOrBeforeTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is not after the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.NotAfterTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CurfewModel
/// {
///     [NotAfterTimeOnlyString("22:00")]
///     public string ReturnTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AfterTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.NotAfterTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotAfterTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must not exceed.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotAfterTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is on or after the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.OnOrAfterTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BusinessModel
/// {
///     [OnOrAfterTimeOnlyString("09:00")]
///     public string OpenTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOnOrAfterTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.OnOrAfterTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrAfterTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must be on or after.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OnOrAfterTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is not on or after the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.NotOnOrAfterTimeOnly"/>. Supported on
/// properties, fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AppointmentModel
/// {
///     [NotOnOrAfterTimeOnlyString("17:00")]
///     public string CheckIn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrAfterTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.NotOnOrAfterTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOnOrAfterTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time boundary that the parsed value must not be on or after.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotOnOrAfterTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents the same
/// <see cref="TimeOnly"/> as the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.SameTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FixedTimeModel
/// {
///     [SameTimeOnlyString("12:00")]
///     public string NoonEvent { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotSameTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.SameTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SameTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time that the parsed value must equal.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.SameTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="TimeOnly"/> that is not the same as the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.NotSameTimeOnly"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The <see cref="Styles"/> property controls how the annotated
/// value is parsed; defaults to <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AvoidTimeModel
/// {
///     [NotSameTimeOnlyString("00:00")]
///     public string EventTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="SameTimeOnlyStringAttribute"/>
/// <seealso cref="MustStringTimeOnlyClauses.NotSameTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotSameTimeOnlyStringAttribute(string other) : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the time that the parsed value must not equal.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotSameTimeOnly(strValue, Other, precision: null, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field (as start time) does not
/// represent a <see cref="TimeOnly"/> that is chronologically before the specified end time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeOnlyClauses.NotChronologicalTimeOnly"/>. Supported on
/// properties, fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls how the annotated value is parsed; defaults to
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReversedModel
/// {
///     [NotChronologicalTimeOnlyString("08:00")]
///     public string EndTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustStringTimeOnlyClauses.NotChronologicalTimeOnly"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotChronologicalTimeOnlyStringAttribute(string end, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string))
{
    /// <summary>Gets the end time string used for the chronological comparison.</summary>
    public string End { get; } = end;

    /// <summary>Gets whether the end boundary is included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotChronologicalTimeOnly(strValue, End, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
