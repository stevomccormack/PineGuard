using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field is a cron expression.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustCronClauses.CronExpression"/>. Supported on properties, fields, and
/// parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Every field is checked against its own range, and lists, ranges, steps and the <c>JAN</c>–<c>DEC</c> /
/// <c>SUN</c>–<c>SAT</c> names are all accepted. The <c>@yearly</c>-style macros and the Quartz-only
/// characters (<c>?</c>, <c>L</c>, <c>W</c>, <c>#</c>) are not part of the supported grammar and fail. Set
/// <see cref="Format"/> to <see cref="CronFormat.WithSeconds"/> for the six-field layout. If the value is
/// <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ScheduleModel
/// {
///     [CronExpression]
///     public string Nightly { get; set; }
///
///     [CronExpression(Format = CronFormat.WithSeconds)]
///     public string Heartbeat { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustCronClauses.CronExpression"/>
/// <seealso href="https://pineguard.ai/docs/annotations/cron">Cron Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class CronExpressionAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Cron.Expression.Invalid)
{
    /// <summary>Gets the field layout the expression is validated against. Defaults to <see cref="CronFormat.Standard"/>.</summary>
    public CronFormat Format { get; init; } = CronFormat.Standard;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;

        var result = Must.Be.CronExpression(strValue, Format, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
