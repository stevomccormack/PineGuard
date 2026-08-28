using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="DateTimeRange"/> property or field is chronological, meaning its
/// start instant precedes (or equals, when inclusive) its end instant.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeRangeClauses.Chronological"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeRange"/>.
/// </para>
/// <para>
/// Defaults to <see cref="Inclusion.Exclusive"/> (the start must be strictly before the end).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WindowModel
/// {
///     [ChronologicalDateTimeRange]
///     public DateTimeRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustDateTimeRangeClauses.Chronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimerange">DateTimeRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ChronologicalDateTimeRangeAttribute(Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateTimeRange), MustCodes.Range.Order.NotChronological)
{
    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateTimeRange)value!;
        var result = Must.Be.Chronological(range, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeRange"/> property or field overlaps with the reference
/// range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeRangeClauses.Overlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="DateTime"/> string format (e.g., <c>"2024-01-01T00:00:00"</c>) using invariant culture.
/// Defaults to <see cref="Inclusion.Exclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BookingModel
/// {
///     [OverlappingDateTimeRange("2024-06-08T00:00:00", "2024-06-12T00:00:00")]
///     public DateTimeRange Slot { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOverlappingDateTimeRangeAttribute"/>
/// <seealso cref="MustDateTimeRangeClauses.Overlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimerange">DateTimeRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OverlappingDateTimeRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateTimeRange), MustCodes.Range.Overlap.Missing)
{
    /// <summary>Gets the reference range that the annotated range must overlap.</summary>
    public DateTimeRange Range2 { get; } = new(
        DateTime.Parse(start2, CultureInfo.InvariantCulture),
        DateTime.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (DateTimeRange)value!;
        var result = Must.Be.Overlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeRange"/> property or field does not overlap with the
/// reference range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeRangeClauses.NotOverlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="DateTime"/> string format using invariant culture. Defaults to <see cref="Inclusion.Exclusive"/>
/// boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MaintenanceModel
/// {
///     [NotOverlappingDateTimeRange("2024-06-15T00:00:00", "2024-06-20T00:00:00")]
///     public DateTimeRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OverlappingDateTimeRangeAttribute"/>
/// <seealso cref="MustDateTimeRangeClauses.NotOverlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimerange">DateTimeRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOverlappingDateTimeRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateTimeRange), MustCodes.Range.Overlap.Present)
{
    /// <summary>Gets the reference range that the annotated range must not overlap.</summary>
    public DateTimeRange Range2 { get; } = new(
        DateTime.Parse(start2, CultureInfo.InvariantCulture),
        DateTime.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (DateTimeRange)value!;
        var result = Must.Be.NotOverlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeRange"/> property or field contains the specified
/// date/time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeRangeClauses.Contains"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="DateTime"/> string format
/// using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CoverageModel
/// {
///     [ContainsDateTimeRange("2024-06-15T12:00:00")]
///     public DateTimeRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsDateTimeRangeAttribute"/>
/// <seealso cref="MustDateTimeRangeClauses.Contains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimerange">DateTimeRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsDateTimeRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateTimeRange), MustCodes.Range.Bounds.NotContains)
{
    /// <summary>Gets the date/time that the annotated range must contain.</summary>
    public DateTime Value { get; } = DateTime.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateTimeRange)value!;
        var result = Must.Be.Contains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeRange"/> property or field does not contain the
/// specified date/time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeRangeClauses.NotContains"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="DateTime"/> string format
/// using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ExclusionModel
/// {
///     [NotContainsDateTimeRange("2024-12-25T00:00:00")]
///     public DateTimeRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ContainsDateTimeRangeAttribute"/>
/// <seealso cref="MustDateTimeRangeClauses.NotContains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimerange">DateTimeRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsDateTimeRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateTimeRange), MustCodes.Range.Bounds.Contains)
{
    /// <summary>Gets the date/time that the annotated range must not contain.</summary>
    public DateTime Value { get; } = DateTime.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateTimeRange)value!;
        var result = Must.Be.NotContains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
