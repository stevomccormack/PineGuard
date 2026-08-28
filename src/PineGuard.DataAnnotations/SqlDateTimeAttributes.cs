#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="DateOnly"/> property or field falls within the SQL Server
/// <c>date</c> type range (<c>0001-01-01</c> to <c>9999-12-31</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustSqlDateTimeClauses.InSqlDateRange"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateOnly"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RecordModel
/// {
///     [InSqlDateRange]
///     public DateOnly CreatedDate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InSqlDateTimeRangeAttribute"/>
/// <seealso cref="MustSqlDateTimeClauses.InSqlDateRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/sqldatetime">SQL DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InSqlDateRangeAttribute() : ValidationAttributeBase(typeof(DateOnly), MustCodes.Date.Sql.OutOfRange)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var dateValue = (DateOnly)value!;
        var result = Must.Be.InSqlDateRange(dateValue, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="DateTime"/> or <see cref="DateTimeOffset"/> property or field
/// falls within the SQL Server <c>datetime2</c> type range
/// (<c>0001-01-01 00:00:00</c> to <c>9999-12-31 23:59:59.9999999</c>).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>InSqlDateTimeRange</c> overload on
/// <see cref="MustSqlDateTimeClauses"/>. Supported on properties, fields, and
/// parameters of type <see cref="DateTime"/> or <see cref="DateTimeOffset"/>.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// <para>
/// If the value is non-<see langword="null"/> but is neither <see cref="DateTime"/> nor
/// <see cref="DateTimeOffset"/>, the attribute is misapplied and an <see cref="InvalidOperationException"/>
/// is thrown rather than silently reporting the value as valid.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RecordModel
/// {
///     [InSqlDateTimeRange]
///     public DateTime CreatedAt { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InSqlDateRangeAttribute"/>
/// <seealso cref="MustSqlDateTimeClauses"/>
/// <seealso href="https://pineguard.ai/docs/annotations/sqldatetime">SQL DateTime Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InSqlDateTimeRangeAttribute() : ValidationAttributeBase(typeof(object), MustCodes.Date.Sql.OutOfRange)
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="value"/> is neither <see cref="DateTime"/> nor <see cref="DateTimeOffset"/>.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case DateTime dt:
                {
                    var result = Must.Be.InSqlDateTimeRange(dt, paramName: null);
                    return FromMustResult(result, validationContext);
                }
            case DateTimeOffset dto:
                {
                    var result = Must.Be.InSqlDateTimeRange(dto, paramName: null);
                    return FromMustResult(result, validationContext);
                }
            default:
                throw new InvalidOperationException($"[InSqlDateTimeRangeAttribute] does not support type {value!.GetType().Name}. Supported types: DateTime, DateTimeOffset.");
        }
    }
}
#endif
