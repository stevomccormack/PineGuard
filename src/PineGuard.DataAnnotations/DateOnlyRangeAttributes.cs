#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="DateOnlyRange"/> property or field is chronological, meaning its
/// start date precedes (or equals, when inclusive) its end date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyRangeClauses.Chronological"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnlyRange"/>.
/// </para>
/// <para>
/// Defaults to <see cref="Inclusion.Exclusive"/> (the start must be strictly before the end).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BookingModel
/// {
///     [ChronologicalDateOnlyRange]
///     public DateOnlyRange Period { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustDateOnlyRangeClauses.Chronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonlyrange">DateOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ChronologicalDateOnlyRangeAttribute(Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateOnlyRange), MustCodes.Range.Order.NotChronological)
{
    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateOnlyRange)value!;
        var result = Must.Be.Chronological(range, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnlyRange"/> property or field overlaps with the reference
/// range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyRangeClauses.Overlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="DateOnly"/> string format (e.g., <c>"2024-01-01"</c>) using invariant culture. Defaults to
/// <see cref="Inclusion.Exclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReservationModel
/// {
///     [OverlappingDateOnlyRange("2024-06-08", "2024-06-12")]
///     public DateOnlyRange Stay { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOverlappingDateOnlyRangeAttribute"/>
/// <seealso cref="MustDateOnlyRangeClauses.Overlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonlyrange">DateOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OverlappingDateOnlyRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateOnlyRange), MustCodes.Range.Overlap.Missing)
{
    /// <summary>Gets the reference range that the annotated range must overlap.</summary>
    public DateOnlyRange Range2 { get; } = new(
        DateOnly.Parse(start2, CultureInfo.InvariantCulture),
        DateOnly.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (DateOnlyRange)value!;
        var result = Must.Be.Overlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnlyRange"/> property or field does not overlap with the
/// reference range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyRangeClauses.NotOverlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="DateOnly"/> string format using invariant culture. Defaults to <see cref="Inclusion.Exclusive"/>
/// boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BlockoutModel
/// {
///     [NotOverlappingDateOnlyRange("2024-06-15", "2024-06-20")]
///     public DateOnlyRange Period { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OverlappingDateOnlyRangeAttribute"/>
/// <seealso cref="MustDateOnlyRangeClauses.NotOverlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonlyrange">DateOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOverlappingDateOnlyRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateOnlyRange), MustCodes.Range.Overlap.Present)
{
    /// <summary>Gets the reference range that the annotated range must not overlap.</summary>
    public DateOnlyRange Range2 { get; } = new(
        DateOnly.Parse(start2, CultureInfo.InvariantCulture),
        DateOnly.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (DateOnlyRange)value!;
        var result = Must.Be.NotOverlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnlyRange"/> property or field contains the specified date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyRangeClauses.Contains"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CoverageModel
/// {
///     [ContainsDateOnlyRange("2024-06-15")]
///     public DateOnlyRange Period { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsDateOnlyRangeAttribute"/>
/// <seealso cref="MustDateOnlyRangeClauses.Contains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonlyrange">DateOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsDateOnlyRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateOnlyRange), MustCodes.Range.Bounds.NotContains)
{
    /// <summary>Gets the date that the annotated range must contain.</summary>
    public DateOnly Value { get; } = DateOnly.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateOnlyRange)value!;
        var result = Must.Be.Contains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnlyRange"/> property or field does not contain the
/// specified date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyRangeClauses.NotContains"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ExclusionModel
/// {
///     [NotContainsDateOnlyRange("2024-12-25")]
///     public DateOnlyRange Period { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ContainsDateOnlyRangeAttribute"/>
/// <seealso cref="MustDateOnlyRangeClauses.NotContains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonlyrange">DateOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsDateOnlyRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateOnlyRange), MustCodes.Range.Bounds.Contains)
{
    /// <summary>Gets the date that the annotated range must not contain.</summary>
    public DateOnly Value { get; } = DateOnly.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (DateOnlyRange)value!;
        var result = Must.Be.NotContains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
