using System.ComponentModel.DataAnnotations;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated property or field is <see langword="null"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNullClauses.Null{T}"/>. Supported on properties, fields, and parameters
/// of any reference or nullable type.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RequestModel
/// {
///     [Null]
///     public string? OptionalField { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotNullAttribute"/>
/// <seealso cref="MustNullClauses.Null{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NullAttribute() : ValidationAttributeBase(typeof(object), allowNull: true)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        // Must.Be.Null<T>(T value).
        // Logic: if value is null, success.
        // We can just call Must.Be.Null(value) treat as object?
        // But Must.Be.Null checks "value is null".

        // Reflection for generic T not really needed if we just pass object?
        // MustClauses usually: public static MustResult<T> Null<T>(this IMustClause _, T? value, ...)
        // If we call Must.Be.Null<object>(value), it checks if value is null.
        // Valid.

        var result = Must.Be.Null(value, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

/// <summary>
/// Validates that the annotated property or field is not <see langword="null"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNullClauses.NotNull{T}"/>. Supported on properties, fields, and
/// parameters of any reference or nullable type.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RequestModel
/// {
///     [NotNull]
///     public string RequiredField { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NullAttribute"/>
/// <seealso cref="MustNullClauses.NotNull{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotNullAttribute() : ValidationAttributeBase(typeof(object), allowNull: false)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var result = Must.Be.NotNull(value, paramName: null);
        return FromMustResult(result, validationContext);
    }
}

// Default/NotDefault usually requires T to know what default(T) is.
// If T is value type (int), default is 0. If reference type, null.
// At runtime, value.GetType() gives T.
// But if value is null, we don't know T from value.
// However, ValidationContext has ObjectType or property info?
// Attributes on property propertyInfo.PropertyType gives T.

/// <summary>
/// Base class for generic object validation attributes that resolve the runtime type at validation time.
/// </summary>
/// <remarks>
/// <para>
/// Uses reflection to invoke the generic <see cref="MustObjectClauses"/> or
/// <see cref="MustDefaultEqualityClauses"/> methods at runtime. Infers the value type from the property
/// value, constructor arguments, or the <see cref="ValidationContext"/> member.
/// </para>
/// </remarks>
public abstract class ObjectAttributeBase() : ValidationAttributeBase(typeof(object), allowNull: false)
{
    /// <summary>
    /// Invokes the named method on <see cref="MustObjectClauses"/> or
    /// <see cref="MustDefaultEqualityClauses"/> for the inferred runtime type and maps the result to a
    /// <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="methodName">The name of the method to invoke.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="ctx">The validation context for the current member.</param>
    /// <param name="args">Additional method arguments (e.g., the comparison value).</param>
    /// <returns>
    /// <see langword="null"/> on success, or a <see cref="ValidationResult"/> describing the failure.
    /// </returns>
    protected ValidationResult? InvokeGenericMust(string methodName, object? value, ValidationContext ctx, params object?[] args)
    {
        var type = InferValueType(value, args, ctx);

        var method = typeof(MustObjectClauses).GetMethod(methodName)
                     ?? typeof(MustDefaultEqualityClauses).GetMethod(methodName)
                     ?? throw new InvalidOperationException($"Method {methodName} not found.");

        var genericMethod = method.MakeGenericMethod(type);
        var invokeArgs = BuildInvokeArgs(genericMethod, value, args);

        return CheckArgCompatibility(type, args, ctx) ?? InvokeAndMapResult(genericMethod, invokeArgs, ctx);
    }

    private static Type InferValueType(object? value, object?[] args, ValidationContext ctx)
    {
        if (value != null)
            return value.GetType();

        var inferredType = args.FirstOrDefault(a => a != null)?.GetType();
        if (inferredType != null)
            return inferredType;

        if (ctx.MemberName is null)
            return typeof(object);

        var prop = ctx.ObjectInstance.GetType().GetProperty(ctx.MemberName);
        return prop != null ? prop.PropertyType : typeof(object);
    }

    private static ValidationResult? CheckArgCompatibility(Type type, object?[] args, ValidationContext ctx)
    {
        var mismatch = args.FirstOrDefault(arg => arg != null && !type.IsInstanceOfType(arg));
        return mismatch is not null
            ? new ValidationResult($"Type mismatch: Expected {type.Name}, got {mismatch.GetType().Name}.", [ctx.MemberName!])
            : null;
    }
}

/// <summary>
/// Validates that the annotated property or field equals the default value for its type (e.g.,
/// <c>0</c> for numeric types, <see langword="null"/> for reference types).
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDefaultEqualityClauses.Default{T}"/>. Supported on properties, fields,
/// and parameters of any type. The default value is inferred at runtime from the property's actual type.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class ResetModel
/// {
///     [IsDefault]
///     public int Counter { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotDefaultAttribute"/>
/// <seealso cref="MustDefaultEqualityClauses.Default{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class IsDefaultAttribute : ObjectAttributeBase
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        // MustDefaultEqualityClauses.Default
        InvokeGenericMust(nameof(MustDefaultEqualityClauses.Default), value, validationContext);
}

/// <summary>
/// Validates that the annotated property or field does not equal the default value for its type.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustDefaultEqualityClauses.NotDefault{T}"/>. Supported on properties, fields,
/// and parameters of any type. The default value is inferred at runtime from the property's actual type.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OrderModel
/// {
///     [NotDefault]
///     public Guid OrderId { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="IsDefaultAttribute"/>
/// <seealso cref="MustDefaultEqualityClauses.NotDefault{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotDefaultAttribute : ObjectAttributeBase
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeGenericMust(nameof(MustDefaultEqualityClauses.NotDefault), value, validationContext);
}

// EqualityAttributes (EqualTo, NotEqualTo) usually require a comparative value.
// [EqualTo(5)] -> int
// [EqualTo("abc")] -> string
// But attributes strictly typed.
// We can support primitives.

/// <summary>
/// Validates that the annotated property or field equals the specified comparison value.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustObjectClauses.EqualTo{T}"/>. Supported on properties, fields, and
/// parameters of primitive types. The comparison is performed using the runtime type of the value.
/// </para>
/// <para>
/// If the runtime type of the value and <see cref="ComparisonValue"/> differ, a type-mismatch validation
/// failure is returned.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class StatusModel
/// {
///     [EqualTo(1)]
///     public int StatusCode { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotEqualToAttribute"/>
/// <seealso cref="MustObjectClauses.EqualTo{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EqualToAttribute(object comparisonValue) : ObjectAttributeBase
{
    /// <summary>Gets the value that the property must equal.</summary>
    public object ComparisonValue { get; } = comparisonValue;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        // Must.Be.EqualTo<T>(T value, T other)
        InvokeGenericMust(nameof(MustObjectClauses.EqualTo), value, validationContext, ComparisonValue);
}

/// <summary>
/// Validates that the annotated property or field does not equal the specified comparison value.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustObjectClauses.NotEqualTo{T}"/>. Supported on properties, fields, and
/// parameters of primitive types. The comparison is performed using the runtime type of the value.
/// </para>
/// <para>
/// If the runtime type of the value and <see cref="ComparisonValue"/> differ, a type-mismatch validation
/// failure is returned.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class RoleModel
/// {
///     [NotEqualTo(0)]
///     public int RoleId { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="EqualToAttribute"/>
/// <seealso cref="MustObjectClauses.NotEqualTo{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEqualToAttribute(object comparisonValue) : ObjectAttributeBase
{
    /// <summary>Gets the value that the property must not equal.</summary>
    public object ComparisonValue { get; } = comparisonValue;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeGenericMust(nameof(MustObjectClauses.NotEqualTo), value, validationContext, ComparisonValue);
}

// OfType<TTarget> requires TTarget. Attribute cannot be generic.
// [OfType(typeof(string))]
/// <summary>
/// Validates that the annotated property or field is an instance of the specified target type.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustObjectClauses.OfType{TTarget}"/>. Supported on properties, fields, and
/// parameters of any type. The target type is specified via the <see cref="TargetType"/> constructor
/// argument and applied via reflection at validation time.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PayloadModel
/// {
///     [OfType(typeof(string))]
///     public object Value { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotOfTypeAttribute"/>
/// <seealso cref="MustObjectClauses.OfType{TTarget}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OfTypeAttribute(Type targetType) : ValidationAttributeBase(typeof(object), allowNull: false)
{
    /// <summary>Gets the type that the property value must be an instance of.</summary>
    public Type TargetType { get; } = targetType;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        // Invoke Must.Be.OfType(value, TargetType) ? No, Must.Be.OfType<TTarget>(object value)

        var method = typeof(MustObjectClauses).GetMethod(nameof(MustObjectClauses.OfType))!
            .MakeGenericMethod(TargetType);

        var resultObj = method.Invoke(null, [null, value, null])!;
        dynamic result = resultObj;

        if (result.Success) return ValidationResult.Success;

        string msg = result.Message;
        var errorTemplate = !string.IsNullOrWhiteSpace(ErrorMessage) || !string.IsNullOrWhiteSpace(ErrorMessageResourceName)
            ? FormatErrorMessage(validationContext.DisplayName)
            : msg.Replace("{paramName}", validationContext.DisplayName);

        return new ValidationResult(errorTemplate, [validationContext.MemberName!]);
    }
}

/// <summary>
/// Validates that the annotated property or field is not an instance of the specified target type.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustObjectClauses.NotOfType{TTarget}"/>. Supported on properties, fields, and
/// parameters of any type. The target type is specified via the <see cref="TargetType"/> constructor
/// argument and applied via reflection at validation time.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PayloadModel
/// {
///     [NotOfType(typeof(int))]
///     public object Value { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="OfTypeAttribute"/>
/// <seealso cref="MustObjectClauses.NotOfType{TTarget}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/object">Object Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotOfTypeAttribute(Type targetType) : ValidationAttributeBase(typeof(object), allowNull: false)
{
    /// <summary>Gets the type that the property value must not be an instance of.</summary>
    public Type TargetType { get; } = targetType;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var method = typeof(MustObjectClauses).GetMethod(nameof(MustObjectClauses.NotOfType))!
            .MakeGenericMethod(TargetType);

        var resultObj = method.Invoke(null, [null, value, null])!;
        dynamic result = resultObj;

        if (result.Success) return ValidationResult.Success;

        string msg = result.Message;
        var errorTemplate = !string.IsNullOrWhiteSpace(ErrorMessage) || !string.IsNullOrWhiteSpace(ErrorMessageResourceName)
            ? FormatErrorMessage(validationContext.DisplayName)
            : msg.Replace("{paramName}", validationContext.DisplayName);

        return new ValidationResult(errorTemplate, [validationContext.MemberName!]);
    }
}
