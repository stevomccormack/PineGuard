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
/// Delegates to <see cref="MustStringNumbersClauses.PositiveDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.PositiveDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PositiveStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.NotPositive, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.PositiveDecimal(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a negative number
/// (less than zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NegativeDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.NegativeDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NegativeStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.NotNegative, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NegativeDecimal(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// equal to zero.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.ZeroDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.ZeroDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.NotZero, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.ZeroDecimal(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// not equal to zero.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotZeroDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.NotZeroDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotZeroStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.Zero, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NotZeroDecimal(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents an even integer.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.EvenInt32"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.EvenInt32"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EvenStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Parity.Odd, allowNull: false)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.EvenInt32(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents an odd integer.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.OddInt32"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.OddInt32"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OddStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Parity.Even, allowNull: false)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.OddInt32(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is zero or positive (greater than or equal to zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.ZeroOrPositiveDecimal"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.ZeroOrPositiveDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroOrPositiveStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.Negative, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.ZeroOrPositiveDecimal(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is zero or negative (less than or equal to zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.ZeroOrNegativeDecimal"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.ZeroOrNegativeDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroOrNegativeStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Sign.Positive, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.ZeroOrNegativeDecimal(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// greater than or equal to the specified minimum.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.GreaterThanOrEqualDecimal"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OrderModel
/// {
///     [GreaterThanOrEqualString("1")]
///     public string Quantity { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LessThanOrEqualStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.GreaterThanOrEqualDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class GreaterThanOrEqualStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.BelowMinimum, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the minimum allowed numeric value (inclusive).</summary>
    public string Other { get; } = other;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedOther = DecimalAttributeConfiguration.Parse(Other, nameof(Other));
        if (parsedOther.Failed)
            return FromMustResult(parsedOther, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.GreaterThanOrEqualDecimal(strValue, parsedOther.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// less than or equal to the specified maximum.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.LessThanOrEqualDecimal"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RatingModel
/// {
///     [LessThanOrEqualString("100")]
///     public string Score { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="GreaterThanOrEqualStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.LessThanOrEqualDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LessThanOrEqualStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.Exceeded, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the maximum allowed numeric value (inclusive).</summary>
    public string Other { get; } = other;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedOther = DecimalAttributeConfiguration.Parse(Other, nameof(Other));
        if (parsedOther.Failed)
            return FromMustResult(parsedOther, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.LessThanOrEqualDecimal(strValue, parsedOther.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that falls within the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.InRangeDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PercentageModel
/// {
///     [InRangeString("0", "100")]
///     public string Percentage { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OutOfRangeStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.InRangeDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.OutOfRange, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the invariant decimal configuration string, parsed without conversion through double.</summary>
    public string Min { get; } = min;

    /// <summary>Gets the invariant decimal configuration string, parsed without conversion through double.</summary>
    public string Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedMin = DecimalAttributeConfiguration.Parse(Min, nameof(Min));
        if (parsedMin.Failed)
            return FromMustResult(parsedMin, validationContext);

        var parsedMax = DecimalAttributeConfiguration.Parse(Max, nameof(Max));
        if (parsedMax.Failed)
            return FromMustResult(parsedMax, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.InRangeDecimal(strValue, parsedMin.Result, parsedMax.Result, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that falls outside the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.OutOfRangeDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TemperatureModel
/// {
///     [OutOfRangeString("-40", "60")]
///     public string Temperature { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InRangeStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.OutOfRangeDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeStringAttribute(string min, string max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.InRange, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the invariant decimal configuration string, parsed without conversion through double.</summary>
    public string Min { get; } = min;

    /// <summary>Gets the invariant decimal configuration string, parsed without conversion through double.</summary>
    public string Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the excluded range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedMin = DecimalAttributeConfiguration.Parse(Min, nameof(Min));
        if (parsedMin.Failed)
            return FromMustResult(parsedMin, validationContext);

        var parsedMax = DecimalAttributeConfiguration.Parse(Max, nameof(Max));
        if (parsedMax.Failed)
            return FromMustResult(parsedMax, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.OutOfRangeDecimal(strValue, parsedMin.Result, parsedMax.Result, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is a multiple of the specified factor.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.MultipleOfDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class GridModel
/// {
///     [MultipleOfString("5")]
///     public string Step { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotMultipleOfStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.MultipleOfDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class MultipleOfStringAttribute(string factor) : ValidationAttributeBase(typeof(string), MustCodes.Number.Divisibility.NotMultiple, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the factor that the numeric value must be a multiple of.</summary>
    public string Factor { get; } = factor;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedFactor = DecimalAttributeConfiguration.Parse(Factor, nameof(Factor));
        if (parsedFactor.Failed)
            return FromMustResult(parsedFactor, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.MultipleOfDecimal(strValue, parsedFactor.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is not a multiple of the specified factor.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotMultipleOfDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PrimeModel
/// {
///     [NotMultipleOfString("2")]
///     public string Candidate { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MultipleOfStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.NotMultipleOfDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotMultipleOfStringAttribute(string factor) : ValidationAttributeBase(typeof(string), MustCodes.Number.Divisibility.Multiple, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the factor that the numeric value must not be a multiple of.</summary>
    public string Factor { get; } = factor;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedFactor = DecimalAttributeConfiguration.Parse(Factor, nameof(Factor));
        if (parsedFactor.Failed)
            return FromMustResult(parsedFactor, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.NotMultipleOfDecimal(strValue, parsedFactor.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// approximately equal to the specified target within the required tolerance.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.ApproximatelyDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Target and tolerance are required invariant decimal strings; tolerance must be non-negative.
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MeasurementModel
/// {
///     [ApproximatelyString("3.14", "0.01")]
///     public string Pi { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotApproximatelyStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.ApproximatelyDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ApproximatelyStringAttribute(string target, string tolerance) : ValidationAttributeBase(typeof(string), MustCodes.Number.Proximity.NotApproximate, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the invariant decimal configuration string, parsed without conversion through double.</summary>
    public string Target { get; } = target;

    /// <summary>Gets the required non-negative tolerance as an invariant decimal string.</summary>
    public string Tolerance { get; } = tolerance;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedTarget = DecimalAttributeConfiguration.Parse(Target, nameof(Target));
        if (parsedTarget.Failed)
            return FromMustResult(parsedTarget, validationContext);

        var parsedTolerance = DecimalAttributeConfiguration.Parse(Tolerance, nameof(Tolerance));
        if (parsedTolerance.Failed)
            return FromMustResult(parsedTolerance, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.ApproximatelyDecimal(strValue, parsedTarget.Result, parsedTolerance.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is not approximately equal to the specified target within the required tolerance.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotApproximatelyDecimal"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// Target and tolerance are required invariant decimal strings; tolerance must be non-negative.
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SensorModel
/// {
///     [NotApproximatelyString("0", "0.01")]
///     public string Reading { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ApproximatelyStringAttribute"/>
/// <seealso cref="MustStringNumbersClauses.NotApproximatelyDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotApproximatelyStringAttribute(string target, string tolerance) : ValidationAttributeBase(typeof(string), MustCodes.Number.Proximity.Approximate, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the invariant decimal configuration string, parsed without conversion through double.</summary>
    public string Target { get; } = target;

    /// <summary>Gets the required non-negative tolerance as an invariant decimal string.</summary>
    public string Tolerance { get; } = tolerance;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedTarget = DecimalAttributeConfiguration.Parse(Target, nameof(Target));
        if (parsedTarget.Failed)
            return FromMustResult(parsedTarget, validationContext);

        var parsedTolerance = DecimalAttributeConfiguration.Parse(Tolerance, nameof(Tolerance));
        if (parsedTolerance.Failed)
            return FromMustResult(parsedTolerance, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.NotApproximatelyDecimal(strValue, parsedTarget.Result, parsedTolerance.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a finite numeric
/// value (not infinity or NaN).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.FiniteDouble"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Float"/> | <see cref="NumberStyles.AllowLeadingWhite"/> |
/// <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.FiniteDouble"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FiniteStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Form.NotFinite, allowNull: false)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Float | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.FiniteDouble(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a non-finite
/// numeric value (positive infinity, negative infinity, or NaN).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotFiniteDouble"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Float"/> | <see cref="NumberStyles.AllowLeadingWhite"/> |
/// <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.NotFiniteDouble"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotFiniteStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Form.Finite, allowNull: false)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Float | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NotFiniteDouble(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a numeric value
/// that is not NaN (Not a Number).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.NotNaNDouble"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Float"/> | <see cref="NumberStyles.AllowLeadingWhite"/> |
/// <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.NotNaNDouble"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNaNStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Form.Nan, allowNull: false)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Float | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.NotNaNDouble(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a percentage
/// between 0 and 100 inclusive.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumbersClauses.PercentageDecimal"/>. Supported on properties, fields,
/// and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The scale is 0–100, not 0–1: a value of <c>"0.5"</c> is half a percent, not fifty percent.
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.AllowLeadingSign"/> | <see cref="NumberStyles.AllowDecimalPoint"/> |
/// <see cref="NumberStyles.AllowLeadingWhite"/> | <see cref="NumberStyles.AllowTrailingWhite"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
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
/// <seealso cref="MustStringNumbersClauses.PercentageDecimal"/>
/// <seealso cref="MustStringNumbersClauses.PercentageDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PercentageStringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.NotPercentage, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.PercentageDecimal(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>Validates a numeric string using a strict GreaterThan comparison.</summary>
/// <param name="other">The comparison value as an invariant decimal string; invalid configuration fails validation.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class GreaterThanStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.NotGreater, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the exclusive comparison bound as an invariant decimal string.</summary>
    public string Other { get; } = other;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedOther = DecimalAttributeConfiguration.Parse(Other, nameof(Other));
        if (parsedOther.Failed)
            return FromMustResult(parsedOther, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.GreaterThanDecimal(strValue, parsedOther.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>Validates a numeric string using a strict LessThan comparison.</summary>
/// <param name="other">The comparison value as an invariant decimal string; invalid configuration fails validation.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LessThanStringAttribute(string other) : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.NotLess, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the exclusive comparison bound as an invariant decimal string.</summary>
    public string Other { get; } = other;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var parsedOther = DecimalAttributeConfiguration.Parse(Other, nameof(Other));
        if (parsedOther.Failed)
            return FromMustResult(parsedOther, validationContext);

        var strValue = (string?)value;
        var result = Must.Be.LessThanDecimal(strValue, parsedOther.Result, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}
