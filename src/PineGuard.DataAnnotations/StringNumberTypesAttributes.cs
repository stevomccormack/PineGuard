using System.ComponentModel.DataAnnotations;
using System.Globalization;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid decimal
/// number with at most the specified number of decimal places.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.Decimal"/>. Supported on properties, fields,
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
/// public class PriceModel
/// {
///     [DecimalString(2)]
///     public string Price { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ExactDecimalStringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.Decimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DecimalStringAttribute(int decimalPlaces = 2) : ValidationAttributeBase(typeof(string), MustCodes.Number.Format.NotDecimal, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the maximum number of decimal places allowed.</summary>
    public int DecimalPlaces { get; } = decimalPlaces;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.Decimal(strValue, DecimalPlaces, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid decimal
/// number with exactly the specified number of decimal places.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.ExactDecimal"/>. Supported on properties,
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
/// public class CurrencyModel
/// {
///     [ExactDecimalString(2)]
///     public string Amount { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="DecimalStringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.ExactDecimal"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ExactDecimalStringAttribute(int exactDecimalPlaces = 2) : ValidationAttributeBase(typeof(string), MustCodes.Number.Scale.Mismatch, allowNull: false)
{
    private const NumberStyles DefaultStyles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;

    /// <summary>Gets the exact number of decimal places required.</summary>
    public int ExactDecimalPlaces { get; } = exactDecimalPlaces;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = DefaultStyles;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.ExactDecimal(strValue, ExactDecimalPlaces, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid
/// <see cref="int"/> (32-bit signed integer).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.Int32"/>. Supported on properties, fields,
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
/// public class ConfigModel
/// {
///     [Int32String]
///     public string MaxRetries { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="Int64StringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.Int32"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class Int32StringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Format.NotInt32, allowNull: false)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.Int32(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid
/// <see cref="long"/> (64-bit signed integer).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.Int64"/>. Supported on properties, fields,
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
/// public class EntityModel
/// {
///     [Int64String]
///     public string Id { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="Int32StringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.Int64"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class Int64StringAttribute() : ValidationAttributeBase(typeof(string), MustCodes.Number.Format.NotInt64, allowNull: false)
{
    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.Int64(strValue, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid
/// <see cref="int"/> within the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.InRangeInt32"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PortModel
/// {
///     [InRangeInt32String(1, 65535)]
///     public string Port { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OutOfRangeInt32StringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.InRangeInt32"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeInt32StringAttribute(int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.OutOfRange, allowNull: false)
{
    /// <summary>Gets the lower boundary of the valid range.</summary>
    public int Min { get; } = min;

    /// <summary>Gets the upper boundary of the valid range.</summary>
    public int Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.InRangeInt32(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid
/// <see cref="int"/> outside the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.OutOfRangeInt32"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ReservedPortModel
/// {
///     [OutOfRangeInt32String(0, 1023)]
///     public string Port { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InRangeInt32StringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.OutOfRangeInt32"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeInt32StringAttribute(int min, int max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.InRange, allowNull: false)
{
    /// <summary>Gets the lower boundary of the excluded range.</summary>
    public int Min { get; } = min;

    /// <summary>Gets the upper boundary of the excluded range.</summary>
    public int Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the excluded range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.OutOfRangeInt32(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid
/// <see cref="long"/> within the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.InRangeInt64"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TimestampModel
/// {
///     [InRangeInt64String(0, 253402300799)]
///     public string UnixTimestamp { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OutOfRangeInt64StringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.InRangeInt64"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeInt64StringAttribute(long min, long max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.OutOfRange, allowNull: false)
{
    /// <summary>Gets the lower boundary of the valid range.</summary>
    public long Min { get; } = min;

    /// <summary>Gets the upper boundary of the valid range.</summary>
    public long Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.InRangeInt64(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="string"/> property or field represents a valid
/// <see cref="long"/> outside the specified range (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustStringNumberTypesClauses.OutOfRangeInt64"/>. Supported on properties,
/// fields, and parameters of type <see cref="string"/>.
/// </para>
/// <para>
/// The <see cref="Styles"/> property controls parsing flags; defaults to
/// <see cref="NumberStyles.Integer"/>.
/// If the value is <see langword="null"/>, validation is skipped after configuration is validated.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SequenceModel
/// {
///     [OutOfRangeInt64String(0, 999)]
///     public string SequenceId { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InRangeInt64StringAttribute"/>
/// <seealso cref="MustStringNumberTypesClauses.OutOfRangeInt64"/>
/// <seealso href="https://pineguard.ai/docs/annotations/string">String Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeInt64StringAttribute(long min, long max, Inclusion inclusion = Inclusion.Inclusive)
    : ValidationAttributeBase(typeof(string), MustCodes.Number.Range.InRange, allowNull: false)
{
    /// <summary>Gets the lower boundary of the excluded range.</summary>
    public long Min { get; } = min;

    /// <summary>Gets the upper boundary of the excluded range.</summary>
    public long Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the excluded range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <summary>Gets or sets the <see cref="NumberStyles"/> used when parsing the string value.</summary>
    public NumberStyles Styles { get; set; } = NumberStyles.Integer;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var strValue = (string?)value;
        var result = Must.Be.OutOfRangeInt64(strValue, Min, Max, Inclusion, Styles, paramName: null);
        return value is null && result.ParamName is null ? ValidationResult.Success : FromMustResult(result, validationContext);
    }
}
