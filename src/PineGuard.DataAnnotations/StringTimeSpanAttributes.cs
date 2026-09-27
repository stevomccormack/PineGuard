using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="TimeSpan"/>
/// duration within the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeSpanClauses.DurationBetween"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeSpan"/> string format (e.g., <c>"00:01:00"</c>) using invariant culture. If the value is
/// <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SessionModel
/// {
///     [DurationBetweenTimeSpanString("00:05:00", "02:00:00")]
///     public string Duration { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotDurationBetweenTimeSpanStringAttribute"/>
/// <seealso cref="MustStringTimeSpanClauses.DurationBetween"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DurationBetweenTimeSpanStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.OutOfRange)
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
        var strValue = (string)value!;
        var result = Must.Be.DurationBetween(strValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="TimeSpan"/>
/// duration longer than (or equal to, depending on <see cref="Inclusion"/>) the specified threshold.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeSpanClauses.LongerThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="threshold"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly longer
/// than). If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TimeoutModel
/// {
///     [LongerThanTimeSpanString("00:00:00")]
///     public string Timeout { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ShorterThanTimeSpanStringAttribute"/>
/// <seealso cref="MustStringTimeSpanClauses.LongerThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LongerThanTimeSpanStringAttribute(string threshold, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.NotLonger)
{
    /// <summary>Gets the lower threshold that the duration must exceed.</summary>
    public TimeSpan Threshold { get; } = TimeSpan.Parse(threshold, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the threshold boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.LongerThan(strValue, Threshold, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="TimeSpan"/>
/// duration shorter than (or equal to, depending on <see cref="Inclusion"/>) the specified threshold.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeSpanClauses.ShorterThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="threshold"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly shorter than).
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TimeoutModel
/// {
///     [ShorterThanTimeSpanString("01:00:00")]
///     public string Duration { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LongerThanTimeSpanStringAttribute"/>
/// <seealso cref="MustStringTimeSpanClauses.ShorterThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ShorterThanTimeSpanStringAttribute(string threshold, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.NotShorter)
{
    /// <summary>Gets the upper threshold that the duration must be below.</summary>
    public TimeSpan Threshold { get; } = TimeSpan.Parse(threshold, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the threshold boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ShorterThan(strValue, Threshold, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="TimeSpan"/>
/// duration that falls outside the specified range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeSpanClauses.NotDurationBetween"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeSpan"/> string format using invariant culture. If the value is <see langword="null"/>,
/// validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SessionModel
/// {
///     [NotDurationBetweenTimeSpanString("00:00:00", "00:00:01")]
///     public string IdleTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="DurationBetweenTimeSpanStringAttribute"/>
/// <seealso cref="MustStringTimeSpanClauses.NotDurationBetween"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDurationBetweenTimeSpanStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.InRange)
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
        var strValue = (string)value!;
        var result = Must.Be.NotDurationBetween(strValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
