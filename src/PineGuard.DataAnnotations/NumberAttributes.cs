#if NET8_0_OR_GREATER
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using PineGuard.Codes;
using PineGuard.Common;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Base class for numeric validation attributes that support multiple numeric types via reflection.
/// </summary>
/// <remarks>
/// <para>
/// Resolves the appropriate <see cref="MustNumberClauses"/> overload at runtime based on the actual
/// runtime type of the validated value. Supports all primitive numeric types (e.g., <see cref="int"/>,
/// <see cref="long"/>, <see cref="double"/>, <see cref="decimal"/>, <see cref="float"/>).
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
public abstract class NumberAttributeBase(string code) : ValidationAttributeBase(typeof(object), code, allowNull: true)
{
    private static readonly HashSet<TypeCode> IntegralTypeCodes =
    [
        TypeCode.SByte, TypeCode.Byte, TypeCode.Int16, TypeCode.UInt16,
        TypeCode.Int32, TypeCode.UInt32, TypeCode.Int64, TypeCode.UInt64
    ];

    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) =>
        value is null ? ValidationResult.Success : ValidateValue(value, validationContext);

    /// <summary>
    /// Converts a numeric bound declared as <see cref="double"/> on the attribute to the runtime type of
    /// the value being validated.
    /// </summary>
    /// <param name="bound">The bound value declared on the attribute (e.g. <c>Min</c>, <c>Max</c>, <c>Factor</c>).</param>
    /// <param name="type">The runtime type of the validated value.</param>
    /// <param name="boundName">The name of the bound, used in exception messages.</param>
    /// <returns>The bound converted to <paramref name="type"/>.</returns>
    /// <remarks>
    /// For integral target types (e.g. <see cref="int"/>, <see cref="long"/>, <see cref="byte"/>), the
    /// conversion is verified to be exact. <see cref="Convert.ChangeType(object, Type, IFormatProvider)"/>
    /// alone would otherwise silently round a fractional bound (e.g. <c>10.99</c> becomes <c>11</c> for
    /// <see cref="int"/>) or throw an undocumented <see cref="OverflowException"/> for an out-of-range
    /// bound; both cases now fail loudly with a clear message instead.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="bound"/> does not fit in <paramref name="type"/>, or (for an integral
    /// <paramref name="type"/>) cannot be represented exactly without rounding.
    /// </exception>
    private protected static object ConvertBound(double bound, Type type, string boundName)
    {
        object converted;
        try
        {
            converted = Convert.ChangeType(bound, type, CultureInfo.InvariantCulture);
        }
        catch (OverflowException ex)
        {
            throw new InvalidOperationException(
                $"The {boundName} value {bound.ToString(CultureInfo.InvariantCulture)} does not fit in {type.Name}; " +
                "specify a bound within the annotated property's numeric range.", ex);
        }

        if (IntegralTypeCodes.Contains(Type.GetTypeCode(type)) &&
            !Convert.ToDouble(converted, CultureInfo.InvariantCulture).Equals(bound))
            throw new InvalidOperationException(
                $"The {boundName} value {bound.ToString(CultureInfo.InvariantCulture)} cannot be represented exactly as " +
                $"{type.Name}; specify a whole-number bound that fits the annotated property's numeric type.");

        return converted;
    }

    /// <summary>
    /// Invokes the named <see cref="MustNumberClauses"/> method for the runtime type of
    /// <paramref name="value"/> and maps the result to a <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="methodName">The name of the method on <see cref="MustNumberClauses"/> to invoke.</param>
    /// <param name="value">The numeric value to validate.</param>
    /// <param name="ctx">The validation context for the current member.</param>
    /// <param name="args">Additional method arguments beyond the value (e.g., min, max, factor).</param>
    /// <returns>
    /// <see langword="null"/> on success, or a <see cref="ValidationResult"/> describing the failure.
    /// </returns>
    protected ValidationResult? InvokeAndMap(string methodName, object? value, ValidationContext ctx, params object?[] args)
    {
        var type = value!.GetType();
        var method = ResolveMethod(methodName, type, args);

        if (method.IsGenericMethodDefinition)
            try { method = method.MakeGenericMethod(type); }
            catch (ArgumentException)
            {
                throw new InvalidOperationException($"Type {type.Name} is not supported by {methodName} (constraint violation).");
            }

        var invokeArgs = new object?[args.Length + 3];
        invokeArgs[0] = null;
        invokeArgs[1] = value;
        Array.Copy(args, 0, invokeArgs, 2, args.Length);
        invokeArgs[^1] = null;

        return InvokeAndMapResult(method, invokeArgs, ctx);
    }

    private static MethodInfo ResolveMethod(string methodName, Type valueType, object?[] args) =>
        TryResolveBySignature(methodName, valueType, args)
        ?? TryResolveByOverloadScan(methodName, valueType)
        ?? throw new InvalidOperationException(
            $"Method {methodName} compatible with type {valueType.Name} not found on MustNumberClauses.");

    private static MethodInfo? TryResolveBySignature(string methodName, Type valueType, object?[] args)
    {
        var valueArgType = valueType.IsValueType ? typeof(Nullable<>).MakeGenericType(valueType) : valueType;

        var methodArgs = new[] { typeof(IMustClause), valueArgType }
            .Concat(args.Select(a => a?.GetType() ?? typeof(object)))
            .Append(typeof(string))
            .ToArray();

        return typeof(MustNumberClauses).GetMethod(methodName, BindingFlags.Public | BindingFlags.Static, methodArgs);
    }

    private static MethodInfo? TryResolveByOverloadScan(string methodName, Type valueType)
    {
        var methods = typeof(MustNumberClauses)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == methodName);

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var m in methods)
        {
            var parameters = m.GetParameters();
            var targetType = Nullable.GetUnderlyingType(parameters[1].ParameterType) ?? parameters[1].ParameterType;

            if (targetType == valueType || (m.IsGenericMethod && valueType.IsValueType))
                return m;
        }

        return null;
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is a positive number (greater than zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.Positive"/>. Supported on properties, fields, and parameters
/// of any primitive numeric type. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ProductModel
/// {
///     [PositiveNumber]
///     public decimal Price { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NegativeNumberAttribute"/>
/// <seealso cref="MustNumberClauses.Positive"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class PositiveNumberAttribute() : NumberAttributeBase(MustCodes.Number.Sign.NotPositive)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.Positive), value, validationContext);
}

/// <summary>
/// Validates that the annotated numeric property or field is a negative number (less than zero).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.Negative"/>. Supported on properties, fields, and parameters
/// of any primitive numeric type. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AdjustmentModel
/// {
///     [NegativeNumber]
///     public double Offset { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="PositiveNumberAttribute"/>
/// <seealso cref="MustNumberClauses.Negative"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NegativeNumberAttribute() : NumberAttributeBase(MustCodes.Number.Sign.NotNegative)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.Negative), value, validationContext);
}

/// <summary>
/// Validates that the annotated numeric property or field equals zero.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.Zero"/>. Supported on properties, fields, and parameters
/// of any primitive numeric type. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BalanceModel
/// {
///     [ZeroNumber]
///     public decimal Balance { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotZeroNumberAttribute"/>
/// <seealso cref="MustNumberClauses.Zero"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroNumberAttribute() : NumberAttributeBase(MustCodes.Number.Sign.NotZero)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.Zero), value, validationContext);
}

/// <summary>
/// Validates that the annotated numeric property or field does not equal zero.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.NotZero"/>. Supported on properties, fields, and parameters
/// of any primitive numeric type. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DivisionModel
/// {
///     [NotZeroNumber]
///     public double Divisor { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ZeroNumberAttribute"/>
/// <seealso cref="MustNumberClauses.NotZero"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotZeroNumberAttribute() : NumberAttributeBase(MustCodes.Number.Sign.Zero)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.NotZero), value, validationContext);
}

/// <summary>
/// Validates that the annotated numeric property or field is zero or greater.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.ZeroOrPositive"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. If the value is <see langword="null"/>, validation passes
/// silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class QuantityModel
/// {
///     [ZeroOrPositiveNumber]
///     public int Quantity { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ZeroOrNegativeNumberAttribute"/>
/// <seealso cref="MustNumberClauses.ZeroOrPositive"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroOrPositiveNumberAttribute() : NumberAttributeBase(MustCodes.Number.Sign.Negative)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.ZeroOrPositive), value, validationContext);
}

/// <summary>
/// Validates that the annotated numeric property or field is zero or less.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.ZeroOrNegative"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. If the value is <see langword="null"/>, validation passes
/// silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PenaltyModel
/// {
///     [ZeroOrNegativeNumber]
///     public double Penalty { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ZeroOrPositiveNumberAttribute"/>
/// <seealso cref="MustNumberClauses.ZeroOrNegative"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ZeroOrNegativeNumberAttribute() : NumberAttributeBase(MustCodes.Number.Sign.Positive)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.ZeroOrNegative), value, validationContext);
}

/// <summary>
/// Validates that the annotated <see cref="int"/> or <see cref="long"/> property or field is an even
/// number.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.Even(IMustClause, int, string)"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/> or <see cref="long"/> only. If the value is <see langword="null"/>,
/// validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PairingModel
/// {
///     [EvenNumber]
///     public int Count { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OddNumberAttribute"/>
/// <seealso cref="MustNumberClauses.Even(IMustClause, int, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EvenNumberAttribute() : NumberAttributeBase(MustCodes.Number.Parity.Odd)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case int intVal:
                {
                    var result = Must.Be.Even(intVal, paramName: null);
                    return FromMustResult(result, validationContext);
                }
            case long longVal:
                {
                    var result = Must.Be.Even(longVal, paramName: null);
                    return FromMustResult(result, validationContext);
                }
            default:
                throw new InvalidOperationException($"[EvenNumberAttribute] only supports int and long. Type: {value!.GetType().Name}");
        }
    }
}

/// <summary>
/// Validates that the annotated <see cref="int"/> or <see cref="long"/> property or field is an odd
/// number.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.Odd(IMustClause, int, string)"/>. Supported on properties, fields, and parameters
/// of type <see cref="int"/> or <see cref="long"/> only. If the value is <see langword="null"/>,
/// validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class GridModel
/// {
///     [OddNumber]
///     public int ColumnCount { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="EvenNumberAttribute"/>
/// <seealso cref="MustNumberClauses.Odd(IMustClause, int, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OddNumberAttribute() : NumberAttributeBase(MustCodes.Number.Parity.Even)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        switch (value)
        {
            case int intVal:
                {
                    var result = Must.Be.Odd(intVal, paramName: null);
                    return FromMustResult(result, validationContext);
                }
            case long longVal:
                {
                    var result = Must.Be.Odd(longVal, paramName: null);
                    return FromMustResult(result, validationContext);
                }
            default:
                throw new InvalidOperationException($"[OddNumberAttribute] only supports int and long. Type: {value!.GetType().Name}");
        }
    }
}

/// <summary>
/// Validates that the annotated floating-point property or field is a finite number (not infinity and
/// not NaN).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.Finite(IMustClause, double, string)"/>. Supported on properties, fields, and parameters
/// of floating-point numeric types. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SensorModel
/// {
///     [FiniteNumber]
///     public double Reading { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotFiniteNumberAttribute"/>
/// <seealso cref="MustNumberClauses.Finite(IMustClause, double, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FiniteNumberAttribute() : NumberAttributeBase(MustCodes.Number.Form.NotFinite)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.Finite), value, validationContext);
}

/// <summary>
/// Validates that the annotated floating-point property or field is not a finite number (is infinity or
/// NaN).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.NotFinite(IMustClause, double, string)"/>. Supported on properties, fields, and parameters
/// of floating-point numeric types. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SignalModel
/// {
///     [NotFiniteNumber]
///     public float Value { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="FiniteNumberAttribute"/>
/// <seealso cref="MustNumberClauses.NotFinite(IMustClause, double, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotFiniteNumberAttribute() : NumberAttributeBase(MustCodes.Number.Form.Finite)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.NotFinite), value, validationContext);
}

/// <summary>
/// Validates that the annotated floating-point property or field is not <c>NaN</c> (Not a Number).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.NotNaN(IMustClause, double, string)"/>. Supported on properties, fields, and parameters
/// of floating-point numeric types. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ComputationModel
/// {
///     [NotNaNNumber]
///     public double Result { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NaNNumberAttribute"/>
/// <seealso cref="MustNumberClauses.NotNaN(IMustClause, double, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNaNNumberAttribute() : NumberAttributeBase(MustCodes.Number.Form.Nan)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.NotNaN), value, validationContext);
}

/// <summary>
/// Validates that the annotated floating-point property or field is <c>NaN</c> (Not a Number).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.NaN(IMustClause, double, string)"/>. Supported on properties, fields, and parameters
/// of floating-point numeric types. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class SentinelModel
/// {
///     [NaNNumber]
///     public float Placeholder { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotNaNNumberAttribute"/>
/// <seealso cref="MustNumberClauses.NaN(IMustClause, double, string)"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NaNNumberAttribute() : NumberAttributeBase(MustCodes.Number.Form.NotNan)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeAndMap(nameof(MustNumberClauses.NaN), value, validationContext);
}

/// <summary>
/// Validates that the annotated numeric property or field is greater than or equal to the specified
/// minimum.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.GreaterThanOrEqual"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. The <see cref="Min"/> value is converted to the runtime type
/// at validation time. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class AgeModel
/// {
///     [GreaterThanOrEqualNumber(18)]
///     public int Age { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="LessThanOrEqualNumberAttribute"/>
/// <seealso cref="MustNumberClauses.GreaterThanOrEqual"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class GreaterThanOrEqualNumberAttribute(double min) : NumberAttributeBase(MustCodes.Number.Range.BelowMinimum)
{
    /// <summary>Gets the minimum value (inclusive) that the property must meet.</summary>
    public double Min { get; } = min;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedMin = ConvertBound(Min, type, nameof(Min));
        return InvokeAndMap(nameof(MustNumberClauses.GreaterThanOrEqual), value, validationContext, typedMin);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is less than or equal to the specified maximum.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.LessThanOrEqual"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. The <see cref="Max"/> value is converted to the runtime type
/// at validation time. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RatingModel
/// {
///     [LessThanOrEqualNumber(5)]
///     public int Rating { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="GreaterThanOrEqualNumberAttribute"/>
/// <seealso cref="MustNumberClauses.LessThanOrEqual"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LessThanOrEqualNumberAttribute(double max) : NumberAttributeBase(MustCodes.Number.Range.Exceeded)
{
    /// <summary>Gets the maximum value (inclusive) that the property must not exceed.</summary>
    public double Max { get; } = max;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedMax = ConvertBound(Max, type, nameof(Max));
        return InvokeAndMap(nameof(MustNumberClauses.LessThanOrEqual), value, validationContext, typedMax);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field falls within the specified numeric range
/// (inclusive or exclusive).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.InRange"/>. Supported on properties, fields, and parameters
/// of any primitive numeric type. The <see cref="Min"/> and <see cref="Max"/> values are converted to
/// the runtime type at validation time. If the value is <see langword="null"/>, validation passes
/// silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class TemperatureModel
/// {
///     [InRangeNumber(-40, 100)]
///     public double Celsius { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OutOfRangeNumberAttribute"/>
/// <seealso cref="MustNumberClauses.InRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class InRangeNumberAttribute(double min, double max, Inclusion inclusion = Inclusion.Inclusive)
    : NumberAttributeBase(MustCodes.Number.Range.OutOfRange)
{
    /// <summary>Gets the lower boundary of the valid range.</summary>
    public double Min { get; } = min;

    /// <summary>Gets the upper boundary of the valid range.</summary>
    public double Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the valid range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedMin = ConvertBound(Min, type, nameof(Min));
        var typedMax = ConvertBound(Max, type, nameof(Max));
        return InvokeAndMap(nameof(MustNumberClauses.InRange), value, validationContext, typedMin, typedMax, Inclusion);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field falls outside the specified numeric range.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.OutOfRange"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. The <see cref="Min"/> and <see cref="Max"/> values are
/// converted to the runtime type at validation time. If the value is <see langword="null"/>, validation
/// passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class FilterModel
/// {
///     [OutOfRangeNumber(0, 100)]
///     public double Gain { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="InRangeNumberAttribute"/>
/// <seealso cref="MustNumberClauses.OutOfRange"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OutOfRangeNumberAttribute(double min, double max, Inclusion inclusion = Inclusion.Inclusive)
    : NumberAttributeBase(MustCodes.Number.Range.InRange)
{
    /// <summary>Gets the lower boundary of the excluded range.</summary>
    public double Min { get; } = min;

    /// <summary>Gets the upper boundary of the excluded range.</summary>
    public double Max { get; } = max;

    /// <summary>Gets whether the boundary values are included or excluded in the forbidden range.</summary>
    public Inclusion Inclusion { get; } = inclusion;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedMin = ConvertBound(Min, type, nameof(Min));
        var typedMax = ConvertBound(Max, type, nameof(Max));
        return InvokeAndMap(nameof(MustNumberClauses.OutOfRange), value, validationContext, typedMin, typedMax, Inclusion);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is a multiple of the specified factor.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.MultipleOf"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. The <see cref="Factor"/> is converted to the runtime type
/// at validation time. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class BatchModel
/// {
///     [MultipleOfNumber(5)]
///     public int BatchSize { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotMultipleOfNumberAttribute"/>
/// <seealso cref="MustNumberClauses.MultipleOf"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class MultipleOfNumberAttribute(double factor) : NumberAttributeBase(MustCodes.Number.Divisibility.NotMultiple)
{
    /// <summary>Gets the factor that the value must be a multiple of.</summary>
    public double Factor { get; } = factor;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedFactor = ConvertBound(Factor, type, nameof(Factor));
        return InvokeAndMap(nameof(MustNumberClauses.MultipleOf), value, validationContext, typedFactor);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is not a multiple of the specified factor.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.NotMultipleOf"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. The <see cref="Factor"/> is converted to the runtime type
/// at validation time. If the value is <see langword="null"/>, validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class StepModel
/// {
///     [NotMultipleOfNumber(3)]
///     public int Step { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MultipleOfNumberAttribute"/>
/// <seealso cref="MustNumberClauses.NotMultipleOf"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotMultipleOfNumberAttribute(double factor) : NumberAttributeBase(MustCodes.Number.Divisibility.Multiple)
{
    /// <summary>Gets the factor that the value must not be a multiple of.</summary>
    public double Factor { get; } = factor;

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedFactor = ConvertBound(Factor, type, nameof(Factor));
        return InvokeAndMap(nameof(MustNumberClauses.NotMultipleOf), value, validationContext, typedFactor);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is approximately equal to the specified target
/// value within an optional tolerance.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.Approximately"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. The <see cref="Target"/> and optional <see cref="Tolerance"/>
/// are converted to the runtime type at validation time. If the value is <see langword="null"/>,
/// validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MeasurementModel
/// {
///     [ApproximatelyNumber(3.14159)]
///     public double Pi { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotApproximatelyNumberAttribute"/>
/// <seealso cref="MustNumberClauses.Approximately"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class ApproximatelyNumberAttribute(double target) : NumberAttributeBase(MustCodes.Number.Proximity.NotApproximate)
{
    /// <summary>Gets the target value to approximate.</summary>
    public double Target { get; } = target;

    /// <summary>Gets or sets the optional tolerance. When <see langword="null"/>, a type-default tolerance is used.</summary>
    public double? Tolerance { get; set; }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedTarget = ConvertBound(Target, type, nameof(Target));
        var typedTolerance = Tolerance is null ? null : ConvertBound(Tolerance.Value, type, nameof(Tolerance));
        return InvokeAndMap(nameof(MustNumberClauses.Approximately), value, validationContext, typedTarget, typedTolerance);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is not approximately equal to the specified
/// target value within an optional tolerance.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.NotApproximately"/>. Supported on properties, fields, and
/// parameters of any primitive numeric type. The <see cref="Target"/> and optional <see cref="Tolerance"/>
/// are converted to the runtime type at validation time. If the value is <see langword="null"/>,
/// validation passes silently.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ThresholdModel
/// {
///     [NotApproximatelyNumber(0)]
///     public double Signal { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="ApproximatelyNumberAttribute"/>
/// <seealso cref="MustNumberClauses.NotApproximately"/>
/// <seealso href="https://pineguard.ai/docs/annotations/number">Number Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotApproximatelyNumberAttribute(double target) : NumberAttributeBase(MustCodes.Number.Proximity.Approximate)
{
    /// <summary>Gets the target value to compare against.</summary>
    public double Target { get; } = target;

    /// <summary>Gets or sets the optional tolerance. When <see langword="null"/>, a type-default tolerance is used.</summary>
    public double? Tolerance { get; set; }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException">
    /// A configured bound does not fit in the property's runtime numeric type, or (for an integral
    /// type) cannot be represented exactly without rounding.
    /// </exception>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var type = value!.GetType();
        var typedTarget = ConvertBound(Target, type, nameof(Target));
        var typedTolerance = Tolerance is null ? null : ConvertBound(Tolerance.Value, type, nameof(Tolerance));
        return InvokeAndMap(nameof(MustNumberClauses.NotApproximately), value, validationContext, typedTarget, typedTolerance);
    }
}
#endif
