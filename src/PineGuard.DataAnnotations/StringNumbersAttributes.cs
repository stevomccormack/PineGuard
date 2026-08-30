#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a positive number
/// (greater than zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Positive"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PaymentModel
/// {
///     [PositiveString]
///     public string Amount { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NegativeStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Positive"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PositiveStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.NotPositive)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Positive(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a negative number
/// (less than zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Negative"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AdjustmentModel
/// {
///     [NegativeString]
///     public string Offset { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PositiveStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Negative"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NegativeStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.NotNegative)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Negative(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// equal to zero.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Zero"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BalanceModel
/// {
///     [ZeroString]
///     public string Balance { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotZeroStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Zero"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.NotZero)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Zero(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// not equal to zero.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotZero"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DivisorModel
/// {
///     [NotZeroString]
///     public string Divisor { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ZeroStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.NotZero"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotZeroStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.Zero)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotZero(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents an even integer.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Even"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PairModel
/// {
///     [EvenString]
///     public string Count { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OddStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Even"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EvenStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Parity.Odd)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Even(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents an odd integer.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Odd"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class LotteryModel
/// {
///     [OddString]
///     public string Pick { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="EvenStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Odd"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OddStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Parity.Even)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Odd(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is zero or positive (greater than or equal to zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.ZeroOrPositive"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class InventoryModel
/// {
///     [ZeroOrPositiveString]
///     public string Quantity { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ZeroOrNegativeStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.ZeroOrPositive"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroOrPositiveStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.Negative)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ZeroOrPositive(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is zero or negative (less than or equal to zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.ZeroOrNegative"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DebitModel
/// {
///     [ZeroOrNegativeString]
///     public string Adjustment { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ZeroOrPositiveStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.ZeroOrNegative"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroOrNegativeStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.Positive)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.ZeroOrNegative(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// greater than or equal to the specified minimum.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.GreaterThanOrEqual"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OrderModel
/// {
///     [GreaterThanOrEqualString(1)]
///     public string Quantity { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LessThanOrEqualStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.GreaterThanOrEqual"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class GreaterThanOrEqualStringAttribute(decimal min) : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.BelowMinimum)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the minimum allowed numeric value (inclusive).</summary>
    public decimal Min { get; } = min;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.GreaterThanOrEqual(strValue, Min, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// less than or equal to the specified maximum.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.LessThanOrEqual"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RatingModel
/// {
///     [LessThanOrEqualString(100)]
///     public string Score { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="GreaterThanOrEqualStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.LessThanOrEqual"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LessThanOrEqualStringAttribute(decimal max) : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.Exceeded)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the maximum allowed numeric value (inclusive).</summary>
    public decimal Max { get; } = max;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.LessThanOrEqual(strValue, Max, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that falls within the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.InRange"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PercentageModel
/// {
///     [InRangeString(0, 100)]
///     public string Percentage { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OutOfRangeStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.InRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeStringAttribute(decimal min, decimal max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.OutOfRange)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the lower boundary of the valid range.</summary>
    public decimal Min { get; } = min;

    /// <summary>Gets the upper boundary of the valid range.</summary>
    public decimal Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.InRange(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that falls outside the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.OutOfRange"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TemperatureModel
/// {
///     [OutOfRangeString(-40, 60)]
///     public string Temperature { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InRangeStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.OutOfRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeStringAttribute(decimal min, decimal max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.InRange)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the lower boundary of the excluded range.</summary>
    public decimal Min { get; } = min;

    /// <summary>Gets the upper boundary of the excluded range.</summary>
    public decimal Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the excluded range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.OutOfRange(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is a multiple of the specified factor.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.MultipleOf"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class GridModel
/// {
///     [MultipleOfString(5)]
///     public string Step { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotMultipleOfStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.MultipleOf"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class MultipleOfStringAttribute(decimal factor) : ValidationAttributeBase(typeof(string), MustCodes.Number.Divisibility.NotMultiple)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the factor that the numeric value must be a multiple of.</summary>
    public decimal Factor { get; } = factor;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.MultipleOf(strValue, Factor, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is not a multiple of the specified factor.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotMultipleOf"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PrimeModel
/// {
///     [NotMultipleOfString(2)]
///     public string Candidate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MultipleOfStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.NotMultipleOf"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotMultipleOfStringAttribute(decimal factor) : ValidationAttributeBase(typeof(string), MustCodes.Number.Divisibility.Multiple)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the factor that the numeric value must not be a multiple of.</summary>
    public decimal Factor { get; } = factor;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotMultipleOf(strValue, Factor, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// approximately equal to the specified target within an optional tolerance.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Approximately"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Tolerance"/> property specifies the maximum allowed deviation; when
/// <see langword="null"/>, a default tolerance is applied by the underlying rule.
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MeasurementModel
/// {
///     [ApproximatelyString(3.14)]
///     public string Pi { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotApproximatelyStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Approximately"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ApproximatelyStringAttribute(decimal target) : ValidationAttributeBase(typeof(string), MustCodes.Number.Proximity.NotApproximate)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the target numeric value to compare against.</summary>
    public decimal Target { get; } = target;

    /// <summary>Gets or sets the maximum allowed deviation from the target. When <see langword="null"/>, the underlying rule applies a default tolerance.</summary>
    public decimal? Tolerance { get; set; }

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Approximately(strValue, Target, Tolerance, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is not approximately equal to the specified target within an optional tolerance.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotApproximately"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Tolerance"/> property specifies the minimum required deviation; when
/// <see langword="null"/>, a default tolerance is applied by the underlying rule.
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SensorModel
/// {
///     [NotApproximatelyString(0)]
///     public string Reading { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ApproximatelyStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.NotApproximately"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotApproximatelyStringAttribute(decimal target) : ValidationAttributeBase(typeof(string), MustCodes.Number.Proximity.Approximate)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the target numeric value to compare against.</summary>
    public decimal Target { get; } = target;

    /// <summary>Gets or sets the minimum required deviation from the target. When <see langword="null"/>, the underlying rule applies a default tolerance.</summary>
    public decimal? Tolerance { get; set; }

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotApproximately(strValue, Target, Tolerance, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a finite numeric
/// value (not infinity or NaN).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Finite"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Float"/> | <see cref="NumberStyles.AllowLeadingWhite"/> |
/// <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class CalculationModel
/// {
///     [FiniteString]
///     public string Result { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotFiniteStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Finite"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FiniteStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Form.NotFinite)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Float | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Finite(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a non-finite
/// numeric value (positive infinity, negative infinity, or NaN).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotFinite"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Float"/> | <see cref="NumberStyles.AllowLeadingWhite"/> |
/// <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SpecialValueModel
/// {
///     [NotFiniteString]
///     public string SpecialValue { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FiniteStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.NotFinite"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotFiniteStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Form.Finite)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Float | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotFinite(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is not NaN (Not a Number).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotNaN"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Float"/> | <see cref="NumberStyles.AllowLeadingWhite"/> |
/// <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MetricModel
/// {
///     [NotNaNString]
///     public string Value { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FiniteStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.NotNaN"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNaNStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Form.Nan)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Float | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.NotNaN(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a percentage
/// between 0 and 100 inclusive.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.Percentage"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The scale is 0–100, not 0–1: a value of <c>"0.5"</c> is half a percent, not fifty percent.
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped by the base class.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DiscountModel
/// {
///     [PercentageString]
///     public string Rate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PercentageNumberAttribute"/>
/// <seealso cref="MustStringNumbersClauses.Percentage"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PercentageStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.NotPercentage)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string)value!;
        var result = Must.Be.Percentage(strValue, Styles, paramName: null);
        return FromMustResult(result, validationContext);
    }
}
#endif
