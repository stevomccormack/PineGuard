using System.ComponentModel.DataAnnotations;
#if NET8_0_OR_GREATER
using System.Globalization;
using PineGuard.Common;
#endif
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

// This file is intentionally DataAnnotations-only, with no single matching Must*Clauses family:
// Must/Guard/Fluent dispatch on compile-time type via strongly-typed overloads (separate
// Must.Be.Past(DateOnly) / Must.Be.Past(DateTime) / Must.Be.Past(DateTimeOffset) methods), but a
// ValidationAttribute is applied to a property whose runtime type isn't known until validation
// time. PastAttribute/PastOrPresentAttribute/FutureAttribute/FutureOrPresentAttribute below give
// callers one attribute that dispatches across all three temporal types at runtime — an ergonomic
// convenience unique to this layer, not a gap to close.

// Polymorphic attributes for DateOnly, DateTime, DateTimeOffset

/// <summary>
/// Validates that the annotated <c>DateOnly</c>, <see cref="DateTime"/>, or
/// <see cref="DateTimeOffset"/> property or field represents a value in the past.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.Past</c> overload based on the runtime type of the
/// annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// An <see cref="InvalidOperationException"/> is thrown for unsupported types.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [Past]
///     public DateTime CreatedAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureAttribute"/>
/// <seealso cref="PastOrPresentAttribute"/>
/// <seealso cref="MustDateTimeClauses.Past"/>
/// <seealso cref="MustDateTimeOffsetClauses.Past"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastAttribute() : ValidationAttributeBase(typeof(object), MustCodes.Date.Relative.NotPast, allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
#if NET8_0_OR_GREATER
            case DateOnly dateOnly:
                {
                    var result = Must.Be.Past(dateOnly, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
#endif
            case DateTime dateTime:
                {
                    var result = Must.Be.Past(dateTime, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            case DateTimeOffset dateTimeOffset:
                {
                    var result = Must.Be.Past(dateTimeOffset, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            default:
                throw new InvalidOperationException($"[PastAttribute] does not support type {value!.GetType().Name}. Supported types: DateOnly, DateTime, DateTimeOffset.");
        }
    }
}

/// <summary>
/// Validates that the annotated <c>DateOnly</c>, <see cref="DateTime"/>, or
/// <see cref="DateTimeOffset"/> property or field represents a value in the past or present.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.PastOrPresent</c> overload based on the runtime type
/// of the annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// An <see cref="InvalidOperationException"/> is thrown for unsupported types.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RecordModel
/// {
///     [PastOrPresent]
///     public DateTimeOffset Timestamp { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureOrPresentAttribute"/>
/// <seealso cref="PastAttribute"/>
/// <seealso cref="MustDateTimeClauses.PastOrPresent"/>
/// <seealso cref="MustDateTimeOffsetClauses.PastOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastOrPresentAttribute() : ValidationAttributeBase(typeof(object), MustCodes.Date.Relative.Future, allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
#if NET8_0_OR_GREATER
            case DateOnly dateOnly:
                {
                    var result = Must.Be.PastOrPresent(dateOnly, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
#endif
            case DateTime dateTime:
                {
                    var result = Must.Be.PastOrPresent(dateTime, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            case DateTimeOffset dateTimeOffset:
                {
                    var result = Must.Be.PastOrPresent(dateTimeOffset, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            default:
                throw new InvalidOperationException($"[PastOrPresentAttribute] does not support type {value!.GetType().Name}.");
        }
    }
}

/// <summary>
/// Validates that the annotated <c>DateOnly</c>, <see cref="DateTime"/>, or
/// <see cref="DateTimeOffset"/> property or field represents a value in the future.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.Future</c> overload based on the runtime type of the
/// annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// An <see cref="InvalidOperationException"/> is thrown for unsupported types.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SubscriptionModel
/// {
///     [Future]
///     public DateOnly ExpiryDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastAttribute"/>
/// <seealso cref="FutureOrPresentAttribute"/>
/// <seealso cref="MustDateTimeClauses.Future"/>
/// <seealso cref="MustDateTimeClauses.Future"/>
/// <seealso cref="MustDateTimeOffsetClauses.Future"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureAttribute() : ValidationAttributeBase(typeof(object), MustCodes.Date.Relative.NotFuture, allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
#if NET8_0_OR_GREATER
            case DateOnly dateOnly:
                {
                    var result = Must.Be.Future(dateOnly, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
#endif
            case DateTime dateTime:
                {
                    var result = Must.Be.Future(dateTime, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            case DateTimeOffset dateTimeOffset:
                {
                    var result = Must.Be.Future(dateTimeOffset, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            default:
                throw new InvalidOperationException($"[FutureAttribute] does not support type {value!.GetType().Name}.");
        }
    }
}

/// <summary>
/// Validates that the annotated <c>DateOnly</c>, <see cref="DateTime"/>, or
/// <see cref="DateTimeOffset"/> property or field represents a value in the future or present.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.FutureOrPresent</c> overload based on the runtime type
/// of the annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// An <see cref="InvalidOperationException"/> is thrown for unsupported types.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TokenModel
/// {
///     [FutureOrPresent]
///     public DateTime ValidUntil { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastOrPresentAttribute"/>
/// <seealso cref="FutureAttribute"/>
/// <seealso cref="MustDateTimeClauses.FutureOrPresent"/>
/// <seealso cref="MustDateTimeClauses.FutureOrPresent"/>
/// <seealso cref="MustDateTimeOffsetClauses.FutureOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureOrPresentAttribute() : ValidationAttributeBase(typeof(object), MustCodes.Date.Relative.Past, allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
#if NET8_0_OR_GREATER
            case DateOnly dateOnly:
                {
                    var result = Must.Be.FutureOrPresent(dateOnly, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
#endif
            case DateTime dateTime:
                {
                    var result = Must.Be.FutureOrPresent(dateTime, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            case DateTimeOffset dateTimeOffset:
                {
                    var result = Must.Be.FutureOrPresent(dateTimeOffset, ResolveTimeProvider(validationContext), paramName: null);
                    return FromMustResult(result, validationContext);
                }
            default:
                throw new InvalidOperationException($"[FutureOrPresentAttribute] does not support type {value!.GetType().Name}.");
        }
    }
}

// Specific attributes that are non-ambiguous or require specific args

#if NET8_0_OR_GREATER
/// <summary>
/// Validates that the annotated <c>DateOnly</c> property or field falls within the specified
/// date range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <c>MustDateOnlyClauses.Between</c>. Supported on properties, fields,
/// and parameters of type <c>DateOnly</c>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <c>DateOnly</c> string format using invariant culture.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EventModel
/// {
///     [DateOnlyBetween("2024-01-01", "2024-12-31")]
///     public DateOnly EventDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustDateOnlyClauses.Between"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DateOnlyBetweenAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Range.OutOfRange)
{
    /// <summary>Gets the lower boundary of the valid date range.</summary>
    public DateOnly Min { get; } = DateOnly.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper boundary of the valid date range.</summary>
    public DateOnly Max { get; } = DateOnly.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.Between(dateValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> property or field has a
/// <see cref="DateTimeKind.Utc"/> kind.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Utc"/>. Supported on properties, fields,
/// and parameters of type <see cref="DateTime"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EventModel
/// {
///     [Utc]
///     public DateTime OccurredAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LocalAttribute"/>
/// <seealso cref="UnspecifiedAttribute"/>
/// <seealso cref="MustDateTimeClauses.Utc"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UtcAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Kind.NotUtc)
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
/// Validates that the annotated <see cref="DateTime"/> property or field has a
/// <see cref="DateTimeKind.Local"/> kind.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Local"/>. Supported on properties, fields,
/// and parameters of type <see cref="DateTime"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DisplayModel
/// {
///     [Local]
///     public DateTime DisplayTime { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UtcAttribute"/>
/// <seealso cref="UnspecifiedAttribute"/>
/// <seealso cref="MustDateTimeClauses.Local"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LocalAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Kind.NotLocal)
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
/// Validates that the annotated <see cref="DateTime"/> property or field has a
/// <see cref="DateTimeKind.Unspecified"/> kind.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateTimeClauses.Unspecified"/>. Supported on properties, fields,
/// and parameters of type <see cref="DateTime"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LegacyModel
/// {
///     [Unspecified]
///     public DateTime LegacyTimestamp { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="UtcAttribute"/>
/// <seealso cref="LocalAttribute"/>
/// <seealso cref="MustDateTimeClauses.Unspecified"/>
/// <seealso href="https://pineguard.ai/docs/annotations/time">Time Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class UnspecifiedAttribute() : ValidationAttributeBase(typeof(DateTime), MustCodes.Date.Kind.NotUnspecified)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateTime)value!;

        var result = Must.Be.Unspecified(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
