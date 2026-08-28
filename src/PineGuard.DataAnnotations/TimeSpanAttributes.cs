using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="TimeSpan"/> property or field represents a duration within the
/// specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeSpanClauses.DurationBetween"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeSpan"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeSpan"/> string format (e.g., <c>"00:01:00"</c>) using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SessionModel
/// {
///     [DurationBetweenTimeSpan("00:05:00", "02:00:00")]
///     public TimeSpan Duration { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotDurationBetweenTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.DurationBetween"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DurationBetweenTimeSpanAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.OutOfRange)
{
    /// <summary>Gets the lower duration boundary.</summary>
    public TimeSpan Min { get; } = TimeSpan.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper duration boundary.</summary>
    public TimeSpan Max { get; } = TimeSpan.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeSpanValue = (TimeSpan)value!;
        var result = Must.Be.DurationBetween(timeSpanValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeSpan"/> property or field does not represent a duration
/// within the specified range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeSpanClauses.NotDurationBetween"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeSpan"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeSpan"/> string format using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SessionModel
/// {
///     [NotDurationBetweenTimeSpan("00:00:00", "00:00:01")]
///     public TimeSpan IdleTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="DurationBetweenTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.NotDurationBetween"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDurationBetweenTimeSpanAttribute(
    string min,
    string max,
    Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.InRange)
{
    /// <summary>Gets the lower duration boundary of the excluded range.</summary>
    public TimeSpan Min { get; } = TimeSpan.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper duration boundary of the excluded range.</summary>
    public TimeSpan Max { get; } = TimeSpan.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary values are included or excluded in the forbidden range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeSpanValue = (TimeSpan)value!;
        var result = Must.Be.NotDurationBetween(timeSpanValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeSpan"/> property or field represents a duration greater
/// than (or equal to, depending on <see cref="Inclusion"/>) the specified threshold.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeSpanClauses.GreaterThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeSpan"/>.
/// </para>
/// <para>
/// The <paramref name="threshold"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly greater than).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TimeoutModel
/// {
///     [GreaterThanTimeSpan("00:00:00")]
///     public TimeSpan Timeout { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LessThanTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.GreaterThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class GreaterThanTimeSpanAttribute(string threshold, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.NotGreater)
{
    /// <summary>Gets the lower threshold that the duration must exceed.</summary>
    public TimeSpan Threshold { get; } = TimeSpan.Parse(threshold, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the threshold boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeSpanValue = (TimeSpan)value!;
        var result = Must.Be.GreaterThan(timeSpanValue, Threshold, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeSpan"/> property or field represents a duration less than
/// (or equal to, depending on <see cref="Inclusion"/>) the specified threshold.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeSpanClauses.LessThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeSpan"/>.
/// </para>
/// <para>
/// The <paramref name="threshold"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly less than).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TimeoutModel
/// {
///     [LessThanTimeSpan("01:00:00")]
///     public TimeSpan Duration { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="GreaterThanTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.LessThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LessThanTimeSpanAttribute(string threshold, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.NotLess)
{
    /// <summary>Gets the upper threshold that the duration must be below.</summary>
    public TimeSpan Threshold { get; } = TimeSpan.Parse(threshold, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the threshold boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeSpanValue = (TimeSpan)value!;
        var result = Must.Be.LessThan(timeSpanValue, Threshold, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
