using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field represents a date and time in
/// the past.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Past"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateTime"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [PastDateTime]
///     public DateTime CreatedAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureDateTimeAttribute"/>
/// <seealso cref="MustDateTimeClauses.Past"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetime">DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastDateTimeAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Relative.NotPast)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.Past(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field represents a date and time in
/// the past or equal to now.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.PastOrPresent"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTime"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EventModel
/// {
///     [PastOrPresentDateTime]
///     public DateTime OccurredAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureOrPresentDateTimeAttribute"/>
/// <seealso cref="MustDateTimeClauses.PastOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetime">DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastOrPresentDateTimeAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Relative.Future)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.PastOrPresent(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field represents a date and time in
/// the future.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Future"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateTime"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReminderModel
/// {
///     [FutureDateTime]
///     public DateTime RemindAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastDateTimeAttribute"/>
/// <seealso cref="MustDateTimeClauses.Future"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetime">DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureDateTimeAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Relative.NotFuture)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.Future(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field represents a date and time in
/// the future or equal to now.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.FutureOrPresent"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTime"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SubscriptionModel
/// {
///     [FutureOrPresentDateTime]
///     public DateTime ExpiresAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastOrPresentDateTimeAttribute"/>
/// <seealso cref="MustDateTimeClauses.FutureOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetime">DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureOrPresentDateTimeAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Relative.Past)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.FutureOrPresent(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field has
/// <see cref="DateTimeKind.Utc"/> kind.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Utc"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateTime"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RecordModel
/// {
///     [UtcDateTime]
///     public DateTime CreatedAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LocalDateTimeAttribute"/>
/// <seealso cref="MustDateTimeClauses.Utc"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetime">DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UtcDateTimeAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Kind.NotUtc)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.Utc(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field has
/// <see cref="DateTimeKind.Local"/> kind.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Local"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateTime"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AppointmentModel
/// {
///     [LocalDateTime]
///     public DateTime AppointmentAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UtcDateTimeAttribute"/>
/// <seealso cref="MustDateTimeClauses.Local"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetime">DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LocalDateTimeAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Kind.NotLocal)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.Local(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field has
/// <see cref="DateTimeKind.Unspecified"/> kind.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Unspecified"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTime"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LegacyModel
/// {
///     [UnspecifiedDateTime]
///     public DateTime Timestamp { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UtcDateTimeAttribute"/>
/// <seealso cref="MustDateTimeClauses.Unspecified"/>
/// <seealso href="https://pineguard.ai/docs/annotations/datetime">DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UnspecifiedDateTimeAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Kind.NotUnspecified)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.Unspecified(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
