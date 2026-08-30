using System.ComponentModel.DataAnnotations;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;
using PineGuard.Rules;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="decimal"/> property or field has no more than the specified
/// number of digits after the decimal point.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDecimalClauses.ScaleAtMost"/>. Supported on properties, fields, and parameters
/// of type <see cref="decimal"/>.
/// </para>
/// <para>
/// Trailing zeros are not stored digits, so <c>1.500m</c> has a scale of <c>1</c>. A <see cref="Scale"/> outside
/// <c>0</c>–<see cref="DecimalRules.MaxScale"/> fails validation with the clause's configuration message, since
/// that is programmer misuse rather than bad input.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PriceModel
/// {
///     [ScaleAtMost(2)]
///     public decimal Price { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PrecisionAtMostAttribute"/>
/// <seealso cref="WithinPrecisionAttribute"/>
/// <seealso cref="MustDecimalClauses.ScaleAtMost"/>
/// <seealso href="https://pineguard.ai/docs/annotations/decimal">Decimal Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ScaleAtMostAttribute(int scale) : ValidationAttributeBase(typeof(decimal), MustCodes.Number.Scale.Exceeded)
{
    /// <summary>Gets the maximum number of digits allowed after the decimal point.</summary>
    public int Scale { get; } = scale;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var decimalValue = (decimal)value!;
        var result = Must.Be.ScaleAtMost(decimalValue, Scale, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="decimal"/> property or field has no more than the specified
/// number of significant digits.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDecimalClauses.PrecisionAtMost"/>. Supported on properties, fields, and parameters
/// of type <see cref="decimal"/>.
/// </para>
/// <para>
/// Trailing zeros are not stored digits, so <c>1.500m</c> has a precision of <c>2</c>. A <see cref="Precision"/>
/// outside <c>1</c>–<see cref="DecimalRules.MaxPrecision"/> fails validation with the clause's configuration
/// message, since that is programmer misuse rather than bad input.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LedgerModel
/// {
///     [PrecisionAtMost(18)]
///     public decimal Amount { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ScaleAtMostAttribute"/>
/// <seealso cref="WithinPrecisionAttribute"/>
/// <seealso cref="MustDecimalClauses.PrecisionAtMost"/>
/// <seealso href="https://pineguard.ai/docs/annotations/decimal">Decimal Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PrecisionAtMostAttribute(int precision) : ValidationAttributeBase(typeof(decimal), MustCodes.Number.Precision.Exceeded)
{
    /// <summary>Gets the maximum number of digits allowed in total.</summary>
    public int Precision { get; } = precision;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var decimalValue = (decimal)value!;
        var result = Must.Be.PrecisionAtMost(decimalValue, Precision, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="decimal"/> property or field fits a
/// <c>decimal(<see cref="Precision"/>, <see cref="Scale"/>)</c> budget.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDecimalClauses.WithinPrecision"/>. Supported on properties, fields, and parameters
/// of type <see cref="decimal"/>.
/// </para>
/// <para>
/// The budget is spent the way a database column spends it: at most <see cref="Scale"/> digits after the decimal
/// point, and at most <c>precision - scale</c> digits before it. So <c>123.4m</c> fits <c>decimal(18, 2)</c> but
/// not <c>decimal(5, 3)</c>. An unusable budget fails validation with the clause's configuration message, since
/// that is programmer misuse rather than bad input.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InvoiceModel
/// {
///     [WithinPrecision(18, 2)]
///     public decimal Total { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ScaleAtMostAttribute"/>
/// <seealso cref="PrecisionAtMostAttribute"/>
/// <seealso cref="MustDecimalClauses.WithinPrecision"/>
/// <seealso href="https://pineguard.ai/docs/annotations/decimal">Decimal Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class WithinPrecisionAttribute(int precision, int scale) : ValidationAttributeBase(typeof(decimal), MustCodes.Number.Precision.OutOfRange)
{
    /// <summary>Gets the total number of digits the budget allows.</summary>
    public int Precision { get; } = precision;

    /// <summary>Gets the number of those digits the budget allows after the decimal point.</summary>
    public int Scale { get; } = scale;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var decimalValue = (decimal)value!;
        var result = Must.Be.WithinPrecision(decimalValue, Precision, Scale, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
