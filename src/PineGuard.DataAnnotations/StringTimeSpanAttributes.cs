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
/// Delegates to <see cref="MustStringTimeSpanClauses.InRangeTimeSpan"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeSpan"/> string format (e.g., <c>"00:01:00"</c>) using invariant culture. If the value is
/// <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SessionModel
/// {
///     [InRangeTimeSpanString("00:05:00", "02:00:00")]
///     public string Duration { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OutOfRangeTimeSpanStringAttribute"/>
/// <seealso cref="MustStringTimeSpanClauses.InRangeTimeSpan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeTimeSpanStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.OutOfRange, allowNull: false)
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
        var strValue = (string?)value;
        var result = Must.Be.InRangeTimeSpan(strValue, Min, Max, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="TimeSpan"/>
/// duration longer than (or equal to, depending on <see cref="Inclusion"/>) the specified other.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeSpanClauses.LongerThanTimeSpan"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly greater
/// than). If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringTimeSpanClauses.LongerThanTimeSpan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LongerThanTimeSpanStringAttribute(string other, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.NotLonger, allowNull: false)
{
    /// <summary>Gets the lower other that the duration must exceed.</summary>
    public TimeSpan Other { get; } = TimeSpan.Parse(other, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the other boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.LongerThanTimeSpan(strValue, Other, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="TimeSpan"/>
/// duration shorter than (or equal to, depending on <see cref="Inclusion"/>) the specified other.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeSpanClauses.ShorterThanTimeSpan"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="TimeSpan"/> string
/// format using invariant culture. By default, uses <see cref="Inclusion.Exclusive"/> (strictly shorter than).
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringTimeSpanClauses.ShorterThanTimeSpan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ShorterThanTimeSpanStringAttribute(string other, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.NotShorter, allowNull: false)
{
    /// <summary>Gets the upper other that the duration must be below.</summary>
    public TimeSpan Other { get; } = TimeSpan.Parse(other, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the other boundary is inclusive or exclusive.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.ShorterThanTimeSpan(strValue, Other, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a <see cref="TimeSpan"/>
/// duration that falls outside the specified range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringTimeSpanClauses.OutOfRangeTimeSpan"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="TimeSpan"/> string format using invariant culture. If the value is <see langword="null"/>,
/// validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SessionModel
/// {
///     [OutOfRangeTimeSpanString("00:00:00", "00:00:01")]
///     public string IdleTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InRangeTimeSpanStringAttribute"/>
/// <seealso cref="MustStringTimeSpanClauses.OutOfRangeTimeSpan"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeTimeSpanStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Time.Duration.InRange, allowNull: false)
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
        var strValue = (string?)value;
        var result = Must.Be.OutOfRangeTimeSpan(strValue, Min, Max, Inclusion, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}
