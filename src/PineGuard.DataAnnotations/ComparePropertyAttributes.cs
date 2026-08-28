using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PineGuard.Codes;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Base class for attributes that compare the annotated value against another property or field on
/// the same object, resolved by name at validation time.
/// </summary>
/// <remarks>
/// The framework's own <see cref="System.ComponentModel.DataAnnotations.CompareAttribute"/> is
/// equality-only, so PineGuard's cross-property attributes are named <c>&lt;Comparison&gt;Property</c>
/// rather than reusing or extending it (see <c>docs/ai/specs/language/naming-collisions.md</c>).
/// </remarks>
/// <param name="otherProperty">
/// The name of the property or field on the validated object to compare the annotated value against.
/// </param>
/// <param name="code">The <c>MustCodes</c> catalogue constant identifying the clause the attribute adapts.</param>
/// <seealso cref="ValidationAttributeBase"/>
/// <seealso href="https://pineguard.ai/docs/annotations">Annotation documentation</seealso>
public abstract class ComparePropertyAttributeBase(string otherProperty, string code) : ValidationAttributeBase(typeof(object), code, allowNull: true)
{
    /// <summary>Gets the name of the property or field to compare the annotated value against.</summary>
    public string OtherProperty { get; } = otherProperty;

    /// <summary>
    /// Resolves <see cref="OtherProperty"/>'s current value on the object being validated.
    /// </summary>
    /// <param name="context">The validation context for the current member.</param>
    /// <returns>The value of the named property or field, which may itself be <see langword="null"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// <see cref="OtherProperty"/> does not name a public instance property or field on
    /// <see cref="ValidationContext.ObjectType"/> — a configuration error, not a validation outcome.
    /// </exception>
    protected object? GetOtherValue(ValidationContext context)
    {
        var objectType = context.ObjectType;

        var property = objectType.GetProperty(OtherProperty, BindingFlags.Public | BindingFlags.Instance);
        if (property is not null)
            return property.GetValue(context.ObjectInstance);

        var field = objectType.GetField(OtherProperty, BindingFlags.Public | BindingFlags.Instance);
        if (field is not null)
            return field.GetValue(context.ObjectInstance);

        throw new InvalidOperationException(
            $"[{GetType().Name}] could not find a public property or field named '{OtherProperty}' on type {objectType.Name}.");
    }

    /// <summary>
    /// Invokes the named generic method on <paramref name="mustClausesType"/> for the runtime type of
    /// <paramref name="value"/> and maps the result to a <see cref="ValidationResult"/>.
    /// </summary>
    /// <param name="mustClausesType">The static must-clauses class declaring the method (e.g. <see cref="MustObjectClauses"/>).</param>
    /// <param name="methodName">The name of the generic method to invoke.</param>
    /// <param name="value">The value to validate.</param>
    /// <param name="ctx">The validation context for the current member.</param>
    /// <param name="args">Additional method arguments (e.g. the other property's resolved value).</param>
    /// <returns><see langword="null"/> on success, or a <see cref="ValidationResult"/> describing the failure.</returns>
    protected ValidationResult? InvokeGenericMust(Type mustClausesType, string methodName, object? value, ValidationContext ctx, params object?[] args)
    {
        var type = value!.GetType();
        var method = mustClausesType.GetMethod(methodName)!;

        var genericMethod = method.MakeGenericMethod(type);
        var invokeArgs = BuildInvokeArgs(genericMethod, value, args);

        return InvokeAndMapResult(genericMethod, invokeArgs, ctx);
    }
}

// The After/OnOrAfter/Before/OnOrBefore family spans both the "date" domain (DateTime, DateOnly,
// DateTimeOffset) and the "time" domain (TimeOnly) — deliberately reusing the Date.Order.* code
// throughout, the same way TimeAttributes.cs's own Date-only polymorphic family already uses one
// code across three types: one rule ("chronological order against another property"), one code,
// regardless of which concrete temporal type it runs against.

/// <summary>
/// Validates that the annotated temporal property or field is after the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.After</c> overload based on the runtime type of the
/// annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>, <c>TimeOnly</c>. <see cref="ComparePropertyAttributeBase.OtherProperty"/>
/// must resolve to the same type as the annotated value.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <example>
/// <code>
/// public class Booking
/// {
///     public DateTime CheckIn { get; set; }
///
///     [AfterProperty(nameof(CheckIn))]
///     public DateTime CheckOut { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="BeforePropertyAttribute"/>
/// <seealso cref="MustDateTimeClauses.After"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AfterPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Date.Order.NotAfter)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        return (value, other) switch
        {
#if NET8_0_OR_GREATER
            (DateOnly dateOnly, DateOnly otherDateOnly) => FromMustResult(Must.Be.After(dateOnly, otherDateOnly, paramName: null), validationContext),
            (TimeOnly timeOnly, TimeOnly otherTimeOnly) => FromMustResult(Must.Be.After(timeOnly, otherTimeOnly, paramName: null), validationContext),
#endif
            (DateTime dateTime, DateTime otherDateTime) => FromMustResult(Must.Be.After(dateTime, otherDateTime, paramName: null), validationContext),
            (DateTimeOffset dateTimeOffset, DateTimeOffset otherDateTimeOffset) => FromMustResult(Must.Be.After(dateTimeOffset, otherDateTimeOffset, paramName: null), validationContext),
            _ => throw UnsupportedComparison(nameof(AfterPropertyAttribute), OtherProperty, value, other)
        };
    }

    internal static InvalidOperationException UnsupportedComparison(string attributeName, string otherProperty, object? value, object? other) =>
        new($"[{attributeName}] requires '{otherProperty}' to resolve to the same type as the annotated value " +
            $"({value!.GetType().Name} vs {other?.GetType().Name ?? "null"}). Supported types: DateOnly, DateTime, DateTimeOffset, TimeOnly.");
}

/// <summary>
/// Validates that the annotated temporal property or field is on or after the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.OnOrAfter</c> overload based on the runtime type of the
/// annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>, <c>TimeOnly</c>. <see cref="ComparePropertyAttributeBase.OtherProperty"/>
/// must resolve to the same type as the annotated value.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="OnOrBeforePropertyAttribute"/>
/// <seealso cref="MustDateTimeClauses.OnOrAfter"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrAfterPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Date.Order.Before)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        return (value, other) switch
        {
#if NET8_0_OR_GREATER
            (DateOnly dateOnly, DateOnly otherDateOnly) => FromMustResult(Must.Be.OnOrAfter(dateOnly, otherDateOnly, paramName: null), validationContext),
            (TimeOnly timeOnly, TimeOnly otherTimeOnly) => FromMustResult(Must.Be.OnOrAfter(timeOnly, otherTimeOnly, paramName: null), validationContext),
#endif
            (DateTime dateTime, DateTime otherDateTime) => FromMustResult(Must.Be.OnOrAfter(dateTime, otherDateTime, paramName: null), validationContext),
            (DateTimeOffset dateTimeOffset, DateTimeOffset otherDateTimeOffset) => FromMustResult(Must.Be.OnOrAfter(dateTimeOffset, otherDateTimeOffset, paramName: null), validationContext),
            _ => throw AfterPropertyAttribute.UnsupportedComparison(nameof(OnOrAfterPropertyAttribute), OtherProperty, value, other)
        };
    }
}

/// <summary>
/// Validates that the annotated temporal property or field is before the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.Before</c> overload based on the runtime type of the
/// annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>, <c>TimeOnly</c>. <see cref="ComparePropertyAttributeBase.OtherProperty"/>
/// must resolve to the same type as the annotated value.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="AfterPropertyAttribute"/>
/// <seealso cref="MustDateTimeClauses.Before"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class BeforePropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Date.Order.NotBefore)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        return (value, other) switch
        {
#if NET8_0_OR_GREATER
            (DateOnly dateOnly, DateOnly otherDateOnly) => FromMustResult(Must.Be.Before(dateOnly, otherDateOnly, paramName: null), validationContext),
            (TimeOnly timeOnly, TimeOnly otherTimeOnly) => FromMustResult(Must.Be.Before(timeOnly, otherTimeOnly, paramName: null), validationContext),
#endif
            (DateTime dateTime, DateTime otherDateTime) => FromMustResult(Must.Be.Before(dateTime, otherDateTime, paramName: null), validationContext),
            (DateTimeOffset dateTimeOffset, DateTimeOffset otherDateTimeOffset) => FromMustResult(Must.Be.Before(dateTimeOffset, otherDateTimeOffset, paramName: null), validationContext),
            _ => throw AfterPropertyAttribute.UnsupportedComparison(nameof(BeforePropertyAttribute), OtherProperty, value, other)
        };
    }
}

/// <summary>
/// Validates that the annotated temporal property or field is on or before the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to the appropriate <c>Must.Be.OnOrBefore</c> overload based on the runtime type of the
/// annotated value. Supported types: <c>DateOnly</c>, <see cref="DateTime"/>,
/// <see cref="DateTimeOffset"/>, <c>TimeOnly</c>. <see cref="ComparePropertyAttributeBase.OtherProperty"/>
/// must resolve to the same type as the annotated value.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="OnOrAfterPropertyAttribute"/>
/// <seealso cref="MustDateTimeClauses.OnOrBefore"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class OnOrBeforePropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Date.Order.After)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        return (value, other) switch
        {
#if NET8_0_OR_GREATER
            (DateOnly dateOnly, DateOnly otherDateOnly) => FromMustResult(Must.Be.OnOrBefore(dateOnly, otherDateOnly, paramName: null), validationContext),
            (TimeOnly timeOnly, TimeOnly otherTimeOnly) => FromMustResult(Must.Be.OnOrBefore(timeOnly, otherTimeOnly, paramName: null), validationContext),
#endif
            (DateTime dateTime, DateTime otherDateTime) => FromMustResult(Must.Be.OnOrBefore(dateTime, otherDateTime, paramName: null), validationContext),
            (DateTimeOffset dateTimeOffset, DateTimeOffset otherDateTimeOffset) => FromMustResult(Must.Be.OnOrBefore(dateTimeOffset, otherDateTimeOffset, paramName: null), validationContext),
            _ => throw AfterPropertyAttribute.UnsupportedComparison(nameof(OnOrBeforePropertyAttribute), OtherProperty, value, other)
        };
    }
}

#if NET8_0_OR_GREATER

/// <summary>
/// Validates that the annotated numeric property or field is greater than the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.GreaterThan{T}"/>. Supported on any primitive numeric type.
/// <see cref="ComparePropertyAttributeBase.OtherProperty"/> must resolve to the same numeric type as the
/// annotated value. Not available on <c>netstandard2.1</c> — <see cref="MustNumberClauses"/> requires
/// <c>net8.0</c> or later.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="LessThanPropertyAttribute"/>
/// <seealso cref="MustNumberClauses.GreaterThan{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class GreaterThanPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Number.Range.NotGreater)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        if (other is null || other.GetType() != value!.GetType())
            throw AfterPropertyAttribute.UnsupportedComparison(nameof(GreaterThanPropertyAttribute), OtherProperty, value, other);

        return InvokeGenericMust(typeof(MustNumberClauses), nameof(MustNumberClauses.GreaterThan), value, validationContext, other);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is greater than or equal to the value of
/// <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.GreaterThanOrEqual{T}"/>. Supported on any primitive
/// numeric type. <see cref="ComparePropertyAttributeBase.OtherProperty"/> must resolve to the same
/// numeric type as the annotated value. Not available on <c>netstandard2.1</c> —
/// <see cref="MustNumberClauses"/> requires <c>net8.0</c> or later.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="LessThanOrEqualPropertyAttribute"/>
/// <seealso cref="MustNumberClauses.GreaterThanOrEqual{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class GreaterThanOrEqualPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Number.Range.BelowMinimum)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        if (other is null || other.GetType() != value!.GetType())
            throw AfterPropertyAttribute.UnsupportedComparison(nameof(GreaterThanOrEqualPropertyAttribute), OtherProperty, value, other);

        return InvokeGenericMust(typeof(MustNumberClauses), nameof(MustNumberClauses.GreaterThanOrEqual), value, validationContext, other);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is less than the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.LessThan{T}"/>. Supported on any primitive numeric type.
/// <see cref="ComparePropertyAttributeBase.OtherProperty"/> must resolve to the same numeric type as the
/// annotated value. Not available on <c>netstandard2.1</c> — <see cref="MustNumberClauses"/> requires
/// <c>net8.0</c> or later.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="GreaterThanPropertyAttribute"/>
/// <seealso cref="MustNumberClauses.LessThan{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LessThanPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Number.Range.NotLess)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        if (other is null || other.GetType() != value!.GetType())
            throw AfterPropertyAttribute.UnsupportedComparison(nameof(LessThanPropertyAttribute), OtherProperty, value, other);

        return InvokeGenericMust(typeof(MustNumberClauses), nameof(MustNumberClauses.LessThan), value, validationContext, other);
    }
}

/// <summary>
/// Validates that the annotated numeric property or field is less than or equal to the value of
/// <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustNumberClauses.LessThanOrEqual{T}"/>. Supported on any primitive numeric
/// type. <see cref="ComparePropertyAttributeBase.OtherProperty"/> must resolve to the same numeric type
/// as the annotated value. Not available on <c>netstandard2.1</c> — <see cref="MustNumberClauses"/>
/// requires <c>net8.0</c> or later.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="GreaterThanOrEqualPropertyAttribute"/>
/// <seealso cref="MustNumberClauses.LessThanOrEqual{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class LessThanOrEqualPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Number.Range.Exceeded)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var other = GetOtherValue(validationContext);
        if (other is null || other.GetType() != value!.GetType())
            throw AfterPropertyAttribute.UnsupportedComparison(nameof(LessThanOrEqualPropertyAttribute), OtherProperty, value, other);

        return InvokeGenericMust(typeof(MustNumberClauses), nameof(MustNumberClauses.LessThanOrEqual), value, validationContext, other);
    }
}

#endif

/// <summary>
/// Validates that the annotated property or field equals the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustObjectClauses.EqualTo{T}"/>. Supported on any type.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <example>
/// <code>
/// public class PasswordChange
/// {
///     public string NewPassword { get; set; }
///
///     [EqualToProperty(nameof(NewPassword))]
///     public string ConfirmPassword { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotEqualToPropertyAttribute"/>
/// <seealso cref="MustObjectClauses.EqualTo{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class EqualToPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Value.Equality.NotEqual)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeGenericMust(typeof(MustObjectClauses), nameof(MustObjectClauses.EqualTo), value, validationContext, GetOtherValue(validationContext));
}

/// <summary>
/// Validates that the annotated property or field does not equal the value of <see cref="ComparePropertyAttributeBase.OtherProperty"/>.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustObjectClauses.NotEqualTo{T}"/>. Supported on any type.
/// </para>
/// </remarks>
/// <param name="otherProperty">The name of the property or field to compare against.</param>
/// <seealso cref="EqualToPropertyAttribute"/>
/// <seealso cref="MustObjectClauses.NotEqualTo{T}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/compare-property">Compare Property Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotEqualToPropertyAttribute(string otherProperty) : ComparePropertyAttributeBase(otherProperty, MustCodes.Value.Equality.Equal)
{
    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext) =>
        InvokeGenericMust(typeof(MustObjectClauses), nameof(MustObjectClauses.NotEqualTo), value, validationContext, GetOtherValue(validationContext));
}
