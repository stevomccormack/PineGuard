using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;
using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure enumeration validation predicates including definition checks, flags validation,
/// and attribute presence detection.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/enum">Enum Rules documentation</seealso>
public static class EnumRules
{
    private static readonly ConcurrentDictionary<(Type EnumType, string MemberName, Type AttributeType), bool>
        AttributeCache = new();

    /// <summary>
    /// Determines whether the specified enum value is a defined member of its enumeration.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a defined enum member; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool defined = EnumRules.IsDefined(DayOfWeek.Monday);  // true
    /// bool undefined = EnumRules.IsDefined((DayOfWeek)99);   // false
    /// </code>
    /// </example>
    public static bool IsDefined<TEnum>(TEnum? value) where TEnum : struct, Enum =>
#if NET8_0_OR_GREATER
        value is not null && Enum.IsDefined(value.Value);
#else
        value is not null && Enum.IsDefined(typeof(TEnum), value.Value);
#endif

    /// <summary>
    /// Determines whether the specified integer value corresponds to a defined member of the enumeration.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="value">The integer value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> maps to a defined enum member; otherwise, <see langword="false"/>.</returns>
    public static bool IsDefinedValue<TEnum>(int? value) where TEnum : struct, Enum =>
#if NET8_0_OR_GREATER
        value is not null && Enum.IsDefined((TEnum)(object)value.Value);
#else
        value is not null && Enum.IsDefined(typeof(TEnum), (TEnum)(object)value.Value);
#endif

    /// <summary>
    /// Determines whether the specified string is a valid member name of the enumeration.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="name">The name to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="ignoreCase">
    /// When <see langword="true"/> (default), performs a case-insensitive comparison.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="name"/> is a valid enum member name; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code><![CDATA[
    /// bool valid = EnumRules.IsDefinedName<DayOfWeek>("monday");   // true (case-insensitive)
    /// bool invalid = EnumRules.IsDefinedName<DayOfWeek>("Holiday"); // false
    /// ]]></code>
    /// </example>
    public static bool IsDefinedName<TEnum>(string? name, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (!StringUtility.TryGetTrimmed(name, out var trimmed))
            return false;

        return Enum.TryParse<TEnum>(trimmed, ignoreCase, out var parsed)
#if NET8_0_OR_GREATER
               && Enum.IsDefined(parsed);
#else
               && Enum.IsDefined(typeof(TEnum), parsed);
#endif
    }

    /// <summary>
    /// Determines whether the specified enum type is decorated with <see cref="FlagsAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to check.</typeparam>
    /// <returns><see langword="true"/> if <typeparamref name="TEnum"/> has the <see cref="FlagsAttribute"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsFlagsEnum<TEnum>() where TEnum : struct, Enum =>
        typeof(TEnum).IsDefined(typeof(FlagsAttribute), inherit: false);

    /// <summary>
    /// Determines whether the specified value is a valid combination of defined flags for a <see cref="FlagsAttribute"/> enum.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if every set bit in <paramref name="value"/> corresponds to a defined member;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// If <typeparamref name="TEnum"/> is not a flags enum, falls back to <see cref="Enum.IsDefined"/>.
    /// </remarks>
    public static bool IsFlagsEnumCombination<TEnum>(TEnum? value)
        where TEnum : struct, Enum
    {
        if (value is null)
            return false;

        if (!IsFlagsEnum<TEnum>())
#if NET8_0_OR_GREATER
            return Enum.IsDefined(value.Value);
#else
            return Enum.IsDefined(typeof(TEnum), value.Value);
#endif

        var definedBits = GetDefinedBits<TEnum>();
        var valBits = ToUInt64(value.Value);

        return (valBits & ~definedBits) == 0;
    }

    /// <summary>
    /// Determines whether the specified enum member is decorated with an attribute of type <typeparamref name="TAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <typeparam name="TAttribute">The attribute type to check for.</typeparam>
    /// <param name="value">The enum value to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the member has the specified attribute; otherwise, <see langword="false"/>.</returns>
    public static bool HasAttribute<TEnum, TAttribute>(TEnum? value)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        if (value is null)
            return false;

#if NET8_0_OR_GREATER
        var name = Enum.GetName(value.Value);
#else
        var name = Enum.GetName(typeof(TEnum), value.Value);
#endif
        if (name is null)
            return false;

        var key = (typeof(TEnum), name, typeof(TAttribute));
        return AttributeCache.GetOrAdd(key, static k =>
        {
            var member = k.EnumType.GetMember(k.MemberName, BindingFlags.Public | BindingFlags.Static);
            return member[0].IsDefined(k.AttributeType, inherit: false);
        });
    }

    /// <summary>
    /// Determines whether the specified enum value includes the given flag.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="flag">The flag to check for.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> includes <paramref name="flag"/>; otherwise, <see langword="false"/>.</returns>
    public static bool HasFlag<TEnum>(TEnum? value, TEnum flag)
        where TEnum : struct, Enum =>
        value is not null && value.Value.HasFlag(flag);

    /// <summary>
    /// Determines whether the specified enum member has a <see cref="DescriptionAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the member has a <see cref="DescriptionAttribute"/>; otherwise, <see langword="false"/>.</returns>
    public static bool HasDescription<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, DescriptionAttribute>(value);

    /// <summary>
    /// Determines whether the specified enum member has a <see cref="DisplayAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the member has a <see cref="DisplayAttribute"/>; otherwise, <see langword="false"/>.</returns>
    public static bool HasDisplay<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, DisplayAttribute>(value);

    /// <summary>
    /// Determines whether the specified enum member has an <see cref="EnumMemberAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the member has an <see cref="EnumMemberAttribute"/>; otherwise, <see langword="false"/>.</returns>
    public static bool HasEnumMember<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, EnumMemberAttribute>(value);

    /// <summary>
    /// Determines whether the specified enum member is marked with <see cref="ObsoleteAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the member is obsolete; otherwise, <see langword="false"/>.</returns>
    public static bool IsObsolete<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, ObsoleteAttribute>(value);

    private static ulong GetDefinedBits<TEnum>() where TEnum : struct, Enum
    {
#if NET8_0_OR_GREATER
        var values = Enum.GetValues<TEnum>();
#else
        var values = (TEnum[])Enum.GetValues(typeof(TEnum));
#endif

        return values.Aggregate<TEnum, ulong>(0, (current, v) => current | ToUInt64(v));
    }

    private static ulong ToUInt64<TEnum>(TEnum value) where TEnum : struct, Enum =>
        Convert.ToUInt64(value);
}
