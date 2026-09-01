using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field represents a point in time
/// in the past.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.Past"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [PastDateTimeOffset]
///     public DateTimeOffset CreatedAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.Past"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Relative.NotPast)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.Past(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field represents a point in time
/// in the past or equal to now.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.PastOrPresent"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EventModel
/// {
///     [PastOrPresentDateTimeOffset]
///     public DateTimeOffset OccurredAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureOrPresentDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.PastOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastOrPresentDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Relative.Future)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.PastOrPresent(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field represents a point in time
/// in the future.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.Future"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [FutureDateTimeOffset]
///     public DateTimeOffset ScheduledAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.Future"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Relative.NotFuture)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.Future(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field represents a point in time
/// in the future or equal to now.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.FutureOrPresent"/>. Supported on properties, fields,
/// and parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TokenModel
/// {
///     [FutureOrPresentDateTimeOffset]
///     public DateTimeOffset ExpiresAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastOrPresentDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.FutureOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureOrPresentDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Relative.Past)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.FutureOrPresent(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}
