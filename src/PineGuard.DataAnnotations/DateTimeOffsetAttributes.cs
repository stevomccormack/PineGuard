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

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field falls on a weekday (Monday through Friday).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.Weekday"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [WeekdayDateTimeOffset]
///     public DateTimeOffset OccursAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="WeekendDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.Weekday"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WeekdayDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Calendar.NotWeekday)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.Weekday(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field falls on a weekend day (Saturday or Sunday).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.Weekend"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [WeekendDateTimeOffset]
///     public DateTimeOffset OccursAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="WeekdayDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.Weekend"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WeekendDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Calendar.NotWeekend)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.Weekend(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field is the first day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.FirstDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [FirstDayOfMonthDateTimeOffset]
///     public DateTimeOffset OccursAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotFirstDayOfMonthDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.FirstDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FirstDayOfMonthDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Calendar.NotFirstDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.FirstDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field is not the first day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.NotFirstDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [NotFirstDayOfMonthDateTimeOffset]
///     public DateTimeOffset OccursAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FirstDayOfMonthDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.NotFirstDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotFirstDayOfMonthDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Calendar.FirstDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.NotFirstDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field is the last day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.LastDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [LastDayOfMonthDateTimeOffset]
///     public DateTimeOffset OccursAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotLastDayOfMonthDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.LastDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LastDayOfMonthDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Calendar.NotLastDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.LastDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTimeOffset"/> property or field is not the last day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeOffsetClauses.NotLastDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTimeOffset"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [NotLastDayOfMonthDateTimeOffset]
///     public DateTimeOffset OccursAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LastDayOfMonthDateTimeOffsetAttribute"/>
/// <seealso cref="MustDateTimeOffsetClauses.NotLastDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetimeoffset">DateTimeOffset Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotLastDayOfMonthDateTimeOffsetAttribute() : ValidationAttributeBase(typeof(DateTimeOffset), MustCodes.Date.Calendar.LastDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTimeOffset)value!;

        var result = Must.Be.NotLastDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
