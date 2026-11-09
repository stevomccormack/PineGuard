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
/// Delegates to <see cref="MustTimeSpanClauses.InRange"/>. Supported on properties, fields, and
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
///     [InRangeTimeSpan("00:05:00", "02:00:00")]
///     public TimeSpan Duration { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OutOfRangeTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.InRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeTimeSpanAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.OutOfRange, allowNull: false)
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
        var timeSpanValue = ((TimeSpan?)value).GetValueOrDefault();
        var result = Must.Be.InRange(timeSpanValue, Min, Max, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeSpan"/> property or field does not represent a duration
/// within the specified range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeSpanClauses.OutOfRange"/>. Supported on properties, fields, and
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
///     [OutOfRangeTimeSpan("00:00:00", "00:00:01")]
///     public TimeSpan IdleTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InRangeTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.OutOfRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeTimeSpanAttribute(
    string min,
    string max,
    Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.InRange, allowNull: false)
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
        var timeSpanValue = ((TimeSpan?)value).GetValueOrDefault();
        var result = Must.Be.OutOfRange(timeSpanValue, Min, Max, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeSpan"/> property or field represents a duration longer
/// than (or equal to, depending on <see cref="Inclusion"/>) the specified other.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeSpanClauses.LongerThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeSpan"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly longer than).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TimeoutModel
/// {
///     [LongerThanTimeSpan("00:00:00")]
///     public TimeSpan Timeout { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ShorterThanTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.LongerThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LongerThanTimeSpanAttribute(string other, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.NotLonger, allowNull: false)
{
    /// <summary>Gets the lower other that the duration must exceed.</summary>
    public TimeSpan Other { get; } = TimeSpan.Parse(other, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the other boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeSpanValue = ((TimeSpan?)value).GetValueOrDefault();
        var result = Must.Be.LongerThan(timeSpanValue, Other, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="TimeSpan"/> property or field represents a duration shorter than
/// (or equal to, depending on <see cref="Inclusion"/>) the specified other.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustTimeSpanClauses.ShorterThan"/>. Supported on properties, fields, and
/// parameters of type <see cref="TimeSpan"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly shorter than).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TimeoutModel
/// {
///     [ShorterThanTimeSpan("01:00:00")]
///     public TimeSpan Duration { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LongerThanTimeSpanAttribute"/>
/// <seealso cref="MustTimeSpanClauses.ShorterThan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/timespan">TimeSpan Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ShorterThanTimeSpanAttribute(string other, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(TimeSpan), MustCodes.Time.Duration.NotShorter, allowNull: false)
{
    /// <summary>Gets the upper other that the duration must be below.</summary>
    public TimeSpan Other { get; } = TimeSpan.Parse(other, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the other boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var timeSpanValue = ((TimeSpan?)value).GetValueOrDefault();
        var result = Must.Be.ShorterThan(timeSpanValue, Other, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}
