using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffsetRange"/> property or field is chronological,
/// meaning its start instant precedes (or equals, when inclusive) its end instant.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetRangeClauses.Chronological"/>. Supported on properties, fields,
/// and parameters of type <see cref="DateTimeOffsetRange"/>.
/// </para>
/// <para>
/// Defaults to <see cref="Inclusion.Exclusive"/> (the start must be strictly before the end).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class WindowModel
/// {
///     [ChronologicalDateTimeOffsetRange]
///     public DateTimeOffsetRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustDateTimeOffsetRangeClauses.Chronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffsetrange">DateTimeOffsetRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ChronologicalDateTimeOffsetRangeAttribute(Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateTimeOffsetRange), MustCodes.Range.Order.NotChronological)
{
    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateTimeOffsetRange)value!;
        var result = Must.Be.Chronological(range, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffsetRange"/> property or field overlaps with the
/// reference range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetRangeClauses.Overlapping"/>. Supported on properties, fields,
/// and parameters of type <see cref="DateTimeOffsetRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="DateTimeOffset"/> string format (e.g., <c>"2024-01-01T00:00:00+00:00"</c>) using invariant
/// culture. Defaults to <see cref="Inclusion.Exclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BookingModel
/// {
///     [OverlappingDateTimeOffsetRange("2024-06-08T00:00:00+00:00", "2024-06-12T00:00:00+00:00")]
///     public DateTimeOffsetRange Slot { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOverlappingDateTimeOffsetRangeAttribute"/>
/// <seealso cref="MustDateTimeOffsetRangeClauses.Overlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffsetrange">DateTimeOffsetRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OverlappingDateTimeOffsetRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateTimeOffsetRange), MustCodes.Range.Overlap.Missing)
{
    /// <summary>Gets the reference range that the annotated range must overlap.</summary>
    public DateTimeOffsetRange Range2 { get; } = new(
        DateTimeOffset.Parse(start2, CultureInfo.InvariantCulture),
        DateTimeOffset.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (DateTimeOffsetRange)value!;
        var result = Must.Be.Overlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffsetRange"/> property or field does not overlap with
/// the reference range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetRangeClauses.NotOverlapping"/>. Supported on properties,
/// fields, and parameters of type <see cref="DateTimeOffsetRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="DateTimeOffset"/> string format using invariant culture. Defaults to
/// <see cref="Inclusion.Exclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MaintenanceModel
/// {
///     [NotOverlappingDateTimeOffsetRange("2024-06-15T00:00:00+00:00", "2024-06-20T00:00:00+00:00")]
///     public DateTimeOffsetRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OverlappingDateTimeOffsetRangeAttribute"/>
/// <seealso cref="MustDateTimeOffsetRangeClauses.NotOverlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffsetrange">DateTimeOffsetRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOverlappingDateTimeOffsetRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateTimeOffsetRange), MustCodes.Range.Overlap.Present)
{
    /// <summary>Gets the reference range that the annotated range must not overlap.</summary>
    public DateTimeOffsetRange Range2 { get; } = new(
        DateTimeOffset.Parse(start2, CultureInfo.InvariantCulture),
        DateTimeOffset.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (DateTimeOffsetRange)value!;
        var result = Must.Be.NotOverlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffsetRange"/> property or field contains the specified
/// date/time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetRangeClauses.Contains"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffsetRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="DateTimeOffset"/> string
/// format using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CoverageModel
/// {
///     [ContainsDateTimeOffsetRange("2024-06-15T12:00:00+00:00")]
///     public DateTimeOffsetRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsDateTimeOffsetRangeAttribute"/>
/// <seealso cref="MustDateTimeOffsetRangeClauses.Contains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffsetrange">DateTimeOffsetRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsDateTimeOffsetRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateTimeOffsetRange), MustCodes.Range.Bounds.NotContains)
{
    /// <summary>Gets the date/time that the annotated range must contain.</summary>
    public DateTimeOffset Value { get; } = DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateTimeOffsetRange)value!;
        var result = Must.Be.Contains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffsetRange"/> property or field does not contain the
/// specified date/time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetRangeClauses.NotContains"/>. Supported on properties, fields,
/// and parameters of type <see cref="DateTimeOffsetRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="DateTimeOffset"/> string
/// format using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ExclusionModel
/// {
///     [NotContainsDateTimeOffsetRange("2024-12-25T00:00:00+00:00")]
///     public DateTimeOffsetRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ContainsDateTimeOffsetRangeAttribute"/>
/// <seealso cref="MustDateTimeOffsetRangeClauses.NotContains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffsetrange">DateTimeOffsetRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsDateTimeOffsetRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateTimeOffsetRange), MustCodes.Range.Bounds.Contains)
{
    /// <summary>Gets the date/time that the annotated range must not contain.</summary>
    public DateTimeOffset Value { get; } = DateTimeOffset.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateTimeOffsetRange)value!;
        var result = Must.Be.NotContains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
