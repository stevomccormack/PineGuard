#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="TimeOnlyRange"/> property or field is chronological, meaning its
/// start time precedes (or equals, when inclusive) its end time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyRangeClauses.Chronological"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeOnlyRange"/>.
/// </para>
/// <para>
/// Defaults to <see cref="Inclusion.Exclusive"/> (the start must be strictly before the end).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ShiftModel
/// {
///     [ChronologicalTimeOnlyRange]
///     public TimeOnlyRange Shift { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustTimeOnlyRangeClauses.Chronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timeonlyrange">TimeOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ChronologicalTimeOnlyRangeAttribute(Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeOnlyRange))
{
    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (TimeOnlyRange)value!;
        var result = Must.Be.Chronological(range, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnlyRange"/> property or field overlaps with the reference
/// range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyRangeClauses.Overlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format (e.g., <c>"08:00"</c>) using invariant culture. Defaults to
/// <see cref="Inclusion.Exclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [OverlappingTimeOnlyRange("09:00", "12:00")]
///     public TimeOnlyRange Slot { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOverlappingTimeOnlyRangeAttribute"/>
/// <seealso cref="MustTimeOnlyRangeClauses.Overlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timeonlyrange">TimeOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OverlappingTimeOnlyRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeOnlyRange))
{
    /// <summary>Gets the reference range that the annotated range must overlap.</summary>
    public TimeOnlyRange Range2 { get; } = new(
        TimeOnly.Parse(start2, CultureInfo.InvariantCulture),
        TimeOnly.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (TimeOnlyRange)value!;
        var result = Must.Be.Overlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnlyRange"/> property or field does not overlap with the
/// reference range defined by the constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyRangeClauses.NotOverlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="start2"/> and <paramref name="end2"/> constructor arguments are parsed from
/// <see cref="TimeOnly"/> string format using invariant culture. Defaults to <see cref="Inclusion.Exclusive"/>
/// boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BreakModel
/// {
///     [NotOverlappingTimeOnlyRange("12:00", "13:00")]
///     public TimeOnlyRange WorkBlock { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OverlappingTimeOnlyRangeAttribute"/>
/// <seealso cref="MustTimeOnlyRangeClauses.NotOverlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timeonlyrange">TimeOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOverlappingTimeOnlyRangeAttribute(string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeOnlyRange))
{
    /// <summary>Gets the reference range that the annotated range must not overlap.</summary>
    public TimeOnlyRange Range2 { get; } = new(
        TimeOnly.Parse(start2, CultureInfo.InvariantCulture),
        TimeOnly.Parse(end2, CultureInfo.InvariantCulture));

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range1 = (TimeOnlyRange)value!;
        var result = Must.Be.NotOverlapping(range1, Range2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnlyRange"/> property or field contains the specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyRangeClauses.Contains"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="TimeOnly"/> string format
/// using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CoverageModel
/// {
///     [ContainsTimeOnlyRange("10:30")]
///     public TimeOnlyRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotContainsTimeOnlyRangeAttribute"/>
/// <seealso cref="MustTimeOnlyRangeClauses.Contains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timeonlyrange">TimeOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ContainsTimeOnlyRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeOnlyRange))
{
    /// <summary>Gets the time that the annotated range must contain.</summary>
    public TimeOnly Value { get; } = TimeOnly.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (TimeOnlyRange)value!;
        var result = Must.Be.Contains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeOnlyRange"/> property or field does not contain the
/// specified time.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeOnlyRangeClauses.NotContains"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeOnlyRange"/>.
/// </para>
/// <para>
/// The <paramref name="value"/> constructor argument is parsed from <see cref="TimeOnly"/> string format
/// using invariant culture. Defaults to <see cref="Inclusion.Inclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ExclusionModel
/// {
///     [NotContainsTimeOnlyRange("12:30")]
///     public TimeOnlyRange Window { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ContainsTimeOnlyRangeAttribute"/>
/// <seealso cref="MustTimeOnlyRangeClauses.NotContains"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timeonlyrange">TimeOnlyRange Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotContainsTimeOnlyRangeAttribute(string value, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeOnlyRange))
{
    /// <summary>Gets the time that the annotated range must not contain.</summary>
    public TimeOnly Value { get; } = TimeOnly.Parse(value, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the range boundaries are included or excluded when evaluating the constraint.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var range = (TimeOnlyRange)value!;
        var result = Must.Be.NotContains(range, Value, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
