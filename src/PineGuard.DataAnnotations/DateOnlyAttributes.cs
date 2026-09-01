#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date in the past.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Past"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EventModel
/// {
///     [PastDateOnly]
///     public DateOnly OccurredOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Past"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Relative.NotPast)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.Past(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date in the past or
/// equal to today.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.PastOrPresent"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProfileModel
/// {
///     [PastOrPresentDateOnly]
///     public DateOnly DateOfBirth { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FutureOrPresentDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.PastOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PastOrPresentDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Relative.Future)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.PastOrPresent(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date in the future.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Future"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [FutureDateOnly]
///     public DateOnly ScheduledOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Future"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Relative.NotFuture)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.Future(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date in the future or
/// equal to today.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.FutureOrPresent"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SubscriptionModel
/// {
///     [FutureOrPresentDateOnly]
///     public DateOnly ExpiresOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PastOrPresentDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.FutureOrPresent"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FutureOrPresentDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Relative.Past)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.FutureOrPresent(dateValue, ResolveTimeProvider(validationContext), paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field falls within the specified date
/// range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Between"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="DateOnly"/> string format (e.g., <c>"2024-01-01"</c>) using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BookingModel
/// {
///     [BetweenDateOnly("2024-01-01", "2024-12-31")]
///     public DateOnly CheckIn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotBetweenDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Between"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BetweenDateOnlyAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Range.OutOfRange)
{
    /// <summary>Gets the lower date boundary.</summary>
    public DateOnly Min { get; } = DateOnly.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper date boundary.</summary>
    public DateOnly Max { get; } = DateOnly.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary dates are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.Between(dateValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field does not fall within the specified
/// date range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.NotBetween"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="min"/> and <paramref name="max"/> constructor arguments are parsed from
/// <see cref="DateOnly"/> string format using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class HolidayModel
/// {
///     [NotBetweenDateOnly("2024-12-24", "2024-12-26")]
///     public DateOnly WorkDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BetweenDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.NotBetween"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotBetweenDateOnlyAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Range.InRange)
{
    /// <summary>Gets the lower boundary of the excluded date range.</summary>
    public DateOnly Min { get; } = DateOnly.Parse(min, CultureInfo.InvariantCulture);

    /// <summary>Gets the upper boundary of the excluded date range.</summary>
    public DateOnly Max { get; } = DateOnly.Parse(max, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the boundary dates are included or excluded in the forbidden range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.NotBetween(dateValue, Min, Max, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date strictly before
/// the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Before"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ContractModel
/// {
///     [BeforeDateOnly("2025-01-01")]
///     public DateOnly StartDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="AfterDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Before"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BeforeDateOnlyAttribute(string other) : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Order.NotBefore)
{
    /// <summary>Gets the reference date that the value must precede.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.Before(dateValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date on or before
/// the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.OnOrBefore"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CouponModel
/// {
///     [OnOrBeforeDateOnly("2024-12-31")]
///     public DateOnly ValidUntil { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrAfterDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.OnOrBefore"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrBeforeDateOnlyAttribute(string other) : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Order.After)
{
    /// <summary>Gets the reference date that the value must not exceed.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.OnOrBefore(dateValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date strictly after
/// the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.After"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TrialModel
/// {
///     [AfterDateOnly("2020-01-01")]
///     public DateOnly StartDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BeforeDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.After"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AfterDateOnlyAttribute(string other) : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Order.NotAfter)
{
    /// <summary>Gets the reference date that the value must follow.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.After(dateValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents a date on or after
/// the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.OnOrAfter"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class EligibilityModel
/// {
///     [OnOrAfterDateOnly("2000-01-01")]
///     public DateOnly EffectiveDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OnOrBeforeDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.OnOrAfter"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrAfterDateOnlyAttribute(string other) : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Order.Before)
{
    /// <summary>Gets the reference date that the value must meet or follow.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.OnOrAfter(dateValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field represents the same date as the
/// specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Same"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [SameDateOnly("2024-03-17")]
///     public DateOnly ReportDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotSameDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Same"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class SameDateOnlyAttribute(string other) : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Equality.NotEqual)
{
    /// <summary>Gets the reference date that the value must equal.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.Same(dateValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field does not represent the same date
/// as the specified reference date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.NotSame"/>. Supported on properties, fields, and parameters
/// of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The <paramref name="other"/> constructor argument is parsed from <see cref="DateOnly"/> string format
/// using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SessionModel
/// {
///     [NotSameDateOnly("2024-01-01")]
///     public DateOnly SessionDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="SameDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.NotSame"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotSameDateOnlyAttribute(string other) : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Equality.Equal)
{
    /// <summary>Gets the reference date that the value must not equal.</summary>
    public DateOnly Other { get; } = DateOnly.Parse(other, CultureInfo.InvariantCulture);

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.NotSame(dateValue, Other, precision: null, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field (treated as a start date) is
/// chronologically before the specified end date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Chronological"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The annotated property is treated as the start of the range. The <paramref name="end"/> constructor
/// argument is parsed from <see cref="DateOnly"/> string format using invariant culture. Defaults to
/// <see cref="Inclusion.Exclusive"/> (start must be strictly before end).
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RangeModel
/// {
///     [ChronologicalDateOnly("2024-12-31")]
///     public DateOnly StartDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotChronologicalDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Chronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ChronologicalDateOnlyAttribute(string end, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Order.NotChronological)
{
    /// <summary>Gets the end date of the chronological range.</summary>
    public DateOnly End { get; } = DateOnly.Parse(end, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the end boundary is included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.Chronological(dateValue, End, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field (treated as a start date) is not
/// chronologically before the specified end date.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.NotChronological"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The annotated property is treated as the start of the range. The <paramref name="end"/> constructor
/// argument is parsed from <see cref="DateOnly"/> string format using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AuditModel
/// {
///     [NotChronologicalDateOnly("2024-01-01")]
///     public DateOnly UpdatedOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ChronologicalDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.NotChronological"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotChronologicalDateOnlyAttribute(string end, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Order.Chronological)
{
    /// <summary>Gets the end date used to check the non-chronological constraint.</summary>
    public DateOnly End { get; } = DateOnly.Parse(end, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the end boundary is included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.NotChronological(dateValue, End, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the date range [annotated property, <see cref="End1"/>] overlaps with the range
/// [<see cref="Start2"/>, <see cref="End2"/>].
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Overlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The annotated property is treated as the start of the first interval. All constructor arguments are
/// parsed from <see cref="DateOnly"/> string format using invariant culture. Defaults to
/// <see cref="Inclusion.Exclusive"/> boundaries.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReservationModel
/// {
///     [OverlappingDateOnly("2024-06-10", "2024-06-08", "2024-06-12")]
///     public DateOnly CheckIn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOverlappingDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Overlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OverlappingDateOnlyAttribute(string end1, string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Overlap.Missing)
{
    /// <summary>Gets the end of the first interval.</summary>
    public DateOnly End1 { get; } = DateOnly.Parse(end1, CultureInfo.InvariantCulture);

    /// <summary>Gets the start of the second interval.</summary>
    public DateOnly Start2 { get; } = DateOnly.Parse(start2, CultureInfo.InvariantCulture);

    /// <summary>Gets the end of the second interval.</summary>
    public DateOnly End2 { get; } = DateOnly.Parse(end2, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the interval boundaries are included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var start1 = (DateOnly)value!;
        var result = Must.Be.Overlapping(start1, End1, Start2, End2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the date range [annotated property, <see cref="End1"/>] does not overlap with the range
/// [<see cref="Start2"/>, <see cref="End2"/>].
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.NotOverlapping"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// <para>
/// The annotated property is treated as the start of the first interval. All constructor arguments are
/// parsed from <see cref="DateOnly"/> string format using invariant culture.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BlockoutModel
/// {
///     [NotOverlappingDateOnly("2024-06-10", "2024-06-15", "2024-06-20")]
///     public DateOnly PeriodStart { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OverlappingDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.NotOverlapping"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOverlappingDateOnlyAttribute(string end1, string start2, string end2, Inclusion inclusion = Inclusion.Exclusive)
    : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Overlap.Present)
{
    /// <summary>Gets the end of the first interval.</summary>
    public DateOnly End1 { get; } = DateOnly.Parse(end1, CultureInfo.InvariantCulture);

    /// <summary>Gets the start of the second interval.</summary>
    public DateOnly Start2 { get; } = DateOnly.Parse(start2, CultureInfo.InvariantCulture);

    /// <summary>Gets the end of the second interval.</summary>
    public DateOnly End2 { get; } = DateOnly.Parse(end2, CultureInfo.InvariantCulture);

    /// <summary>Gets whether the interval boundaries are included or excluded.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var start1 = (DateOnly)value!;
        var result = Must.Be.NotOverlapping(start1, End1, Start2, End2, Inclusion, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field falls on a weekday (Monday through Friday).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Weekday"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [WeekdayDateOnly]
///     public DateOnly OccursOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="WeekendDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Weekday"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WeekdayDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Calendar.NotWeekday)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.Weekday(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field falls on a weekend day (Saturday or Sunday).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.Weekend"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [WeekendDateOnly]
///     public DateOnly OccursOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="WeekdayDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.Weekend"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WeekendDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Calendar.NotWeekend)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.Weekend(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field is the first day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.FirstDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [FirstDayOfMonthDateOnly]
///     public DateOnly OccursOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotFirstDayOfMonthDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.FirstDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FirstDayOfMonthDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Calendar.NotFirstDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.FirstDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field is not the first day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.NotFirstDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [NotFirstDayOfMonthDateOnly]
///     public DateOnly OccursOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FirstDayOfMonthDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.NotFirstDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotFirstDayOfMonthDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Calendar.FirstDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.NotFirstDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field is the last day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.LastDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [LastDayOfMonthDateOnly]
///     public DateOnly OccursOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotLastDayOfMonthDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.LastDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LastDayOfMonthDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Calendar.NotLastDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.LastDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field is not the last day of its month.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDateOnlyClauses.NotLastDayOfMonth"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [NotLastDayOfMonthDateOnly]
///     public DateOnly OccursOn { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LastDayOfMonthDateOnlyAttribute"/>
/// <seealso cref="MustDateOnlyClauses.NotLastDayOfMonth"/>
/// <seealso href="https://pineguard.ai/docs/annotations/dateonly">DateOnly Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotLastDayOfMonthDateOnlyAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Calendar.LastDayOfMonth)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;

        var result = Must.Be.NotLastDayOfMonth(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
