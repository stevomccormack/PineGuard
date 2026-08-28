#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field falls within the specified
/// range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.Between"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ShiftModel
/// {
///     [BetweenTimeOnly("08:00", "17:00")]
///     public TimeOnly StartTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotBetweenTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.Between"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BetweenTimeOnlyAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Range.OutOfRange)
{
    /// <summary>Gets the lower boundary of the valid time range.</summary>
    public TimeOnly Min { get; } = TimeOnly.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper boundary of the valid time range.</summary>
    public TimeOnly Max { get; } = TimeOnly.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.Between(timeValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field falls outside the specified
/// range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.NotBetween"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class QuietHoursModel
/// {
///     [NotBetweenTimeOnly("22:00", "06:00")]
///     public TimeOnly AlertTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BetweenTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.NotBetween"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBetweenTimeOnlyAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Range.InRange)
{
    /// <summary>Gets the lower boundary of the excluded time range.</summary>
    public TimeOnly Min { get; } = TimeOnly.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper boundary of the excluded time range.</summary>
    public TimeOnly Max { get; } = TimeOnly.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary values are included or excluded in the excluded range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.NotBetween(timeValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is before the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.Before"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MorningModel
/// {
///     [BeforeTimeOnly("12:00")]
///     public TimeOnly WakeUpTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AfterTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.Before"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BeforeTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.NotBefore)
{
    /// <summary>Gets the time boundary that the value must precede.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.Before(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is after the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.After"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EveningModel
/// {
///     [AfterTimeOnly("18:00")]
///     public TimeOnly DinnerTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BeforeTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.After"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AfterTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.NotAfter)
{
    /// <summary>Gets the time boundary that the value must follow.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.After(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is not before the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.NotBefore"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OpeningModel
/// {
///     [NotBeforeTimeOnly("09:00")]
///     public TimeOnly ServiceStart { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BeforeTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.NotBefore"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBeforeTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.Before)
{
    /// <summary>Gets the time boundary that the value must not precede.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.NotBefore(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is on or before the
/// specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.OnOrBefore"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DeadlineModel
/// {
///     [OnOrBeforeTimeOnly("23:59")]
///     public TimeOnly Submission { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOnOrBeforeTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.OnOrBefore"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrBeforeTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.After)
{
    /// <summary>Gets the time boundary that the value must be on or before.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.OnOrBefore(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is not on or before the
/// specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.NotOnOrBefore"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [NotOnOrBeforeTimeOnly("08:00")]
///     public TimeOnly MeetingTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrBeforeTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.NotOnOrBefore"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOnOrBeforeTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.NotAfter)
{
    /// <summary>Gets the time boundary that the value must not be on or before.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.NotOnOrBefore(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is not after the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.NotAfter"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CurfewModel
/// {
///     [NotAfterTimeOnly("22:00")]
///     public TimeOnly ReturnTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AfterTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.NotAfter"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotAfterTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.After)
{
    /// <summary>Gets the time boundary that the value must not exceed.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.NotAfter(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is on or after the
/// specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.OnOrAfter"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BusinessModel
/// {
///     [OnOrAfterTimeOnly("09:00")]
///     public TimeOnly OpenTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOnOrAfterTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.OnOrAfter"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrAfterTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.Before)
{
    /// <summary>Gets the time boundary that the value must be on or after.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.OnOrAfter(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field is not on or after the
/// specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.NotOnOrAfter"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AppointmentModel
/// {
///     [NotOnOrAfterTimeOnly("17:00")]
///     public TimeOnly CheckIn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrAfterTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.NotOnOrAfter"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOnOrAfterTimeOnlyAttribute(string other) : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.NotBefore)
{
    /// <summary>Gets the time boundary that the value must not be on or after.</summary>
    public TimeOnly Other { get; } = TimeOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeValue = (TimeOnly)value!;
        var result = Must.Be.NotOnOrAfter(timeValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field (as start time) is
/// chronologically before the specified end time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.Chronological"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="end"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The annotated value represents the start time.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MeetingModel
/// {
///     [ChronologicalTimeOnly("17:00")]
///     public TimeOnly StartTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotChronologicalTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.Chronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ChronologicalTimeOnlyAttribute(string end, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.NotChronological)
{
    /// <summary>Gets the end time that the annotated start time must precede.</summary>
    public TimeOnly End { get; } = TimeOnly.Parse(end, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the end boundary is included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var start = (TimeOnly)value!;
        var result = Must.Be.Chronological(start, End, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field (as start time) is not
/// chronologically before the specified end time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.NotChronological"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The <paramref name="end"/> constructor argument is parsed from <see cref="TimeOnly"/> string
/// format using invariant culture. The annotated value represents the start time.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReversedModel
/// {
///     [NotChronologicalTimeOnly("08:00")]
///     public TimeOnly EndTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ChronologicalTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.NotChronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotChronologicalTimeOnlyAttribute(string end, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Order.Chronological)
{
    /// <summary>Gets the end time that the annotated start time must not precede.</summary>
    public TimeOnly End { get; } = TimeOnly.Parse(end, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the end boundary is included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var start = (TimeOnly)value!;
        var result = Must.Be.NotChronological(start, End, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field (as the start of the first
/// interval) overlaps with the specified second time interval.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.Overlapping"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The annotated value is treated as <c>start1</c>. The <paramref name="end1"/>,
/// <paramref name="start2"/>, and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OverlapModel
/// {
///     [OverlappingTimeOnly("10:00", "09:00", "11:00")]
///     public TimeOnly Start1 { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOverlappingTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.Overlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OverlappingTimeOnlyAttribute(string end1, string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Overlap.Missing)
{
    /// <summary>Gets the end of the first time interval.</summary>
    public TimeOnly End1 { get; } = TimeOnly.Parse(end1, CultureInfo.InvariantCulture);

    /// <summary>Gets the start of the second time interval.</summary>
    public TimeOnly Start2 { get; } = TimeOnly.Parse(start2, CultureInfo.InvariantCulture);

    /// <summary>Gets the end of the second time interval.</summary>
    public TimeOnly End2 { get; } = TimeOnly.Parse(end2, CultureInfo.InvariantCulture);

    /// <summary>Gets whether interval endpoints are included or excluded when testing overlap.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var start1 = (TimeOnly)value!;
        var result = Must.Be.Overlapping(start1, End1, Start2, End2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnly"/> property or field (as the start of the first
/// interval) does not overlap with the specified second time interval.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyClauses.NotOverlapping"/>. Supported on properties, fields,
/// and parameters of type <see cref="TimeOnly"/>.
/// </para>
/// <para>
/// The annotated value is treated as <c>start1</c>. The <paramref name="end1"/>,
/// <paramref name="start2"/>, and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class NonOverlapModel
/// {
///     [NotOverlappingTimeOnly("10:00", "11:00", "12:00")]
///     public TimeOnly Start1 { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OverlappingTimeOnlyAttribute"/>
/// <seealso cref="MustTimeOnlyClauses.NotOverlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOverlappingTimeOnlyAttribute(string end1, string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeOnly), MustCodes.Time.Overlap.Present)
{
    /// <summary>Gets the end of the first time interval.</summary>
    public TimeOnly End1 { get; } = TimeOnly.Parse(end1, CultureInfo.InvariantCulture);

    /// <summary>Gets the start of the second time interval.</summary>
    public TimeOnly Start2 { get; } = TimeOnly.Parse(start2, CultureInfo.InvariantCulture);

    /// <summary>Gets the end of the second time interval.</summary>
    public TimeOnly End2 { get; } = TimeOnly.Parse(end2, CultureInfo.InvariantCulture);

    /// <summary>Gets whether interval endpoints are included or excluded when testing overlap.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var start1 = (TimeOnly)value!;
        var result = Must.Be.NotOverlapping(start1, End1, Start2, End2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
