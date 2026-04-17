using System.ComponentModel.DataAnnotations;
using System.Reflection;
using PineGuard.DataAnnotations.Common;
using PineGuard.MustClauses;

namespace PineGuard.DataAnnotations;

/// <summary>
/// Validates that the annotated <see cref="Enum"/> property or field is a defined member of its enum type.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEnumClauses.Defined{TEnum}"/>. Supported on properties, fields, and
/// parameters of any enum type. The attribute resolves the enum type at runtime via reflection.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation passes silently. If the property type is not an
/// enum, an <see cref="InvalidOperationException"/> is thrown at validation time.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class OrderModel
/// {
///     [Defined]
///     public OrderStatus Status { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustEnumClauses.Defined{TEnum}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/enum">Enum Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class DefinedAttribute() : ValidationAttributeBase(typeof(object))
{
    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return ValidationResult.Success;
        return !value.GetType().IsEnum ? throw new InvalidOperationException($"[DefinedAttribute] can only be applied to Enum types. Property '{validationContext.DisplayName}' is {value.GetType().Name}.") : ValidateValue(value, validationContext);
    }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var enumType = value!.GetType();

        var method = typeof(MustEnumClauses).GetMethod(nameof(MustEnumClauses.Defined))!
            .MakeGenericMethod(enumType);

        var invokeArgs = BuildInvokeArgs(method, value, []);
        return EnumResultMapper.InvokeAndMap(method, invokeArgs, this, validationContext);
    }
}

/// <summary>
/// Validates that the annotated flags <see cref="Enum"/> property or field represents a valid combination
/// of defined flag members.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEnumClauses.FlagsEnumCombination{TEnum}"/>. Supported on properties,
/// fields, and parameters of any flags enum type. The attribute resolves the enum type at runtime via
/// reflection.
/// </para>
/// <para>
/// If the value is <see langword="null"/>, validation passes silently. If the property type is not an
/// enum, an <see cref="InvalidOperationException"/> is thrown at validation time.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PermissionModel
/// {
///     [FlagsEnumCombination]
///     public FileAccess Access { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="MustEnumClauses.FlagsEnumCombination{TEnum}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/enum">Enum Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class FlagsEnumCombinationAttribute() : ValidationAttributeBase(typeof(object))
{
    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null) return ValidationResult.Success;
        return !value.GetType().IsEnum ? throw new InvalidOperationException("[FlagsEnumCombinationAttribute] can only be applied to Enum types.") : ValidateValue(value, validationContext);
    }

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var enumType = value!.GetType();

        var method = typeof(MustEnumClauses).GetMethod(nameof(MustEnumClauses.FlagsEnumCombination))!
            .MakeGenericMethod(enumType);

        var invokeArgs = BuildInvokeArgs(method, value, []);
        return EnumResultMapper.InvokeAndMap(method, invokeArgs, this, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Enum"/> property or field has the specified flag set.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEnumClauses.HasFlag{TEnum}"/>. Supported on properties, fields, and
/// parameters of any enum type. The flag is resolved by name at validation time.
/// </para>
/// <para>
/// If the <see cref="FlagName"/> does not correspond to a defined member of the enum type, an
/// <see cref="InvalidOperationException"/> is thrown at validation time. Flag name matching is
/// case-insensitive.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PermissionModel
/// {
///     [HasFlag("Read")]
///     public FileAccess Access { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="NotHasFlagAttribute"/>
/// <seealso cref="MustEnumClauses.HasFlag{TEnum}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/enum">Enum Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class HasFlagAttribute(string flagName) : ValidationAttributeBase(typeof(Enum))
{
    /// <summary>Gets the case-insensitive name of the flag that must be set.</summary>
    public string FlagName { get; } = flagName;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var enumType = value!.GetType();

        object flagValue;
        try { flagValue = Enum.Parse(enumType, FlagName, ignoreCase: true); }
        catch (ArgumentException) { throw new InvalidOperationException($"Flag '{FlagName}' not found in enum type '{enumType.Name}'."); }

        var method = typeof(MustEnumClauses).GetMethod(nameof(MustEnumClauses.HasFlag))!
            .MakeGenericMethod(enumType);

        var invokeArgs = BuildInvokeArgs(method, value, [flagValue]);
        return EnumResultMapper.InvokeAndMap(method, invokeArgs, this, validationContext);
    }
}

/// <summary>
/// Validates that the annotated <see cref="Enum"/> property or field does not have the specified flag set.
/// </summary>
/// <remarks>
/// <para>
/// Delegates to <see cref="MustEnumClauses.NotHasFlag{TEnum}"/>. Supported on properties, fields, and
/// parameters of any enum type. The flag is resolved by name at validation time.
/// </para>
/// <para>
/// If the <see cref="FlagName"/> does not correspond to a defined member of the enum type, an
/// <see cref="InvalidOperationException"/> is thrown at validation time. Flag name matching is
/// case-insensitive.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class PermissionModel
/// {
///     [NotHasFlag("Delete")]
///     public FileAccess Access { get; set; }
/// }
/// </code>
/// </example>
/// <seealso cref="HasFlagAttribute"/>
/// <seealso cref="MustEnumClauses.NotHasFlag{TEnum}"/>
/// <seealso href="https://pineguard.ai/docs/annotations/enum">Enum Attribute documentation</seealso>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class NotHasFlagAttribute(string flagName) : ValidationAttributeBase(typeof(Enum))
{
    /// <summary>Gets the case-insensitive name of the flag that must not be set.</summary>
    public string FlagName { get; } = flagName;

    /// <inheritdoc/>
    protected override ValidationResult? ValidateValue(object? value, ValidationContext validationContext)
    {
        var enumType = value!.GetType();

        object flagValue;
        try { flagValue = Enum.Parse(enumType, FlagName, ignoreCase: true); }
        catch (ArgumentException) { throw new InvalidOperationException($"Flag '{FlagName}' not found in enum type '{enumType.Name}'."); }

        var method = typeof(MustEnumClauses).GetMethod(nameof(MustEnumClauses.NotHasFlag))!
            .MakeGenericMethod(enumType);

        var invokeArgs = BuildInvokeArgs(method, value, [flagValue]);
        return EnumResultMapper.InvokeAndMap(method, invokeArgs, this, validationContext);
    }
}

/// <summary>
/// Maps MustResult via reflection instead of dynamic binding.
/// The DLR fails when MustResult&lt;TEnum&gt; is parameterized with a non-public enum type.
/// </summary>
file static class EnumResultMapper
{
    internal static ValidationResult? InvokeAndMap(
        MethodInfo method, object?[] invokeArgs, ValidationAttribute attr, ValidationContext ctx)
    {
        var resultObj = method.Invoke(null, invokeArgs)!;
        var resultType = resultObj.GetType();

        var success = (bool)resultType.GetProperty("Success")!.GetValue(resultObj)!;
        if (success) return ValidationResult.Success;

        var msg = (string)resultType.GetProperty("Message")!.GetValue(resultObj)!;
        var errorTemplate = !string.IsNullOrWhiteSpace(attr.ErrorMessage) || !string.IsNullOrWhiteSpace(attr.ErrorMessageResourceName)
            ? attr.FormatErrorMessage(ctx.DisplayName)
            : msg.Replace("{paramName}", ctx.DisplayName);

        return new ValidationResult(errorTemplate, [ctx.MemberName!]);
    }
}
