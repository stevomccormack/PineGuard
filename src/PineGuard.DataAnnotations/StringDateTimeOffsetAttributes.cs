using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="DateTimeOffset"/> in the past.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateTimeOffsetClauses.PastDateTimeOffset"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="DateTimeStyles.RoundtripKind"/> | <see cref="DateTimeStyles.AssumeUniversal"/> |
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>, so offset-less input is treated as UTC regardless of
/// the host time zone.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// <para>
/// The current instant is read from the <see cref="TimeProvider"/> registered in the
/// <see cref="ValidationContext"/> service provider; when none is registered, <see cref="TimeProvider.System"/> is
/// used. Instants are compared, so the parsed value's offset does not affect the outcome.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [PastDateTimeOffsetString]
///     public string CreatedAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureDateTimeOffsetStringAttribute"/>
/// <seealso cref="MustStringDateTimeOffsetClauses.PastDateTimeOffset"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastDateTimeOffsetStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Date.Relative.NotPast, allowNull: false)
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.PastDateTimeOffset(strValue, Styles, ResolveTimeProvider(validationContext), paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="DateTimeOffset"/> in the future.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateTimeOffsetClauses.FutureDateTimeOffset"/>. Supported on
/// properties, fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="DateTimeStyles.RoundtripKind"/> | <see cref="DateTimeStyles.AssumeUniversal"/> |
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>, so offset-less input is treated as UTC regardless of
/// the host time zone.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// <para>
/// The current instant is read from the <see cref="TimeProvider"/> registered in the
/// <see cref="ValidationContext"/> service provider; when none is registered, <see cref="TimeProvider.System"/> is
/// used. Instants are compared, so the parsed value's offset does not affect the outcome.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TokenModel
/// {
///     [FutureDateTimeOffsetString]
///     public string ExpiresAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastDateTimeOffsetStringAttribute"/>
/// <seealso cref="MustStringDateTimeOffsetClauses.FutureDateTimeOffset"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureDateTimeOffsetStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Date.Relative.NotFuture, allowNull: false)
{
    private const DateTimeStyles DefaultStyles = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.FutureDateTimeOffset(strValue, Styles, ResolveTimeProvider(validationContext), paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a
/// <see cref="DateTimeOffset"/> that falls within the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringDateTimeOffsetClauses.InRangeDateTimeOffset"/>. Supported on
/// properties, fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="DateTimeOffset"/> string format using invariant culture. The <see cref="Styles"/> property
/// controls how the annotated value is parsed; defaults to
/// <see cref="DateTimeStyles.RoundtripKind"/> | <see cref="DateTimeStyles.AssumeUniversal"/> |
/// <see cref="DateTimeStyles.AllowWhiteSpaces"/>, so offset-less input is treated as UTC regardless of
/// the host time zone.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EventModel
/// {
///     [InRangeDateTimeOffsetString("2024-01-01T00:00:00+00:00", "2024-12-31T23:59:59+00:00")]
///     public string EventAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustStringDateTimeOffsetClauses.InRangeDateTimeOffset"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeDateTimeOffsetStringAttribute(
    string min,
    string max,
    Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Date.Range.OutOfRange, allowNull: false)
{
    /// <summary>Gets the lower boundary of the valid range.</summary>
    public DateTimeOffset Min { get; } = DateTimeOffset.Parse(min, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces);

    /// <summary>Gets the upper boundary of the valid range.</summary>
    public DateTimeOffset Max { get; } = DateTimeOffset.Parse(max, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces);

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    private const DateTimeStyles DefaultStyles = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;

    /// <summary>Gets or sets the <see cref="DateTimeStyles"/> used when parsing the string value.</summary>
    public DateTimeStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.InRangeDateTimeOffset(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>Validates that the string represents a DateTimeOffset outside the configured range.</summary>
/// <param name="min">The invariant-culture lower boundary.</param>
/// <param name="max">The invariant-culture upper boundary.</param>
/// <param name="inclusion">Whether range boundaries are included.</param>
/// <remarks>Null values are skipped after configuration is validated. Non-null values delegate configuration and value validation to Must.</remarks>
/// <example><code>[OutOfRangeDateTimeOffsetString("2024-01-01", "2024-12-31")]</code></example>
/// <seealso cref="MustStringDateTimeOffsetClauses.OutOfRangeDateTimeOffset"/>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeDateTimeOffsetStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Date.Range.InRange, allowNull: false)
{
    /// <summary>Gets the lower boundary.</summary>
    public DateTimeOffset Min { get; } = DateTimeOffset.Parse(min, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces);

    /// <summary>Gets the upper boundary.</summary>
    public DateTimeOffset Max { get; } = DateTimeOffset.Parse(max, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces);

    /// <summary>Gets whether the boundaries are included.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the parsing styles.</summary>
    public DateTimeStyles Styles { get; set; } = DateTimeStyles.RoundtripKind | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.OutOfRangeDateTimeOffset((string?)value, Min, Max, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}
