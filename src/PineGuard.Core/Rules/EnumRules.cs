using System.Collections.Concurrent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
    /// <remarks>
    /// Converts <paramref name="value"/> to <typeparamref name="TEnum"/> via <see cref="Enum.ToObject(Type, int)"/>
    /// so that enums with a non-<see cref="int"/> underlying type (e.g. <see cref="byte"/>, <see cref="long"/>) are
    /// handled correctly instead of throwing <see cref="InvalidCastException"/>. Because <c>ToObject</c> truncates
    /// rather than range-checks, <paramref name="value"/> is first verified to fit within the underlying type's
    /// range; an out-of-range value that would otherwise wrap onto a defined member's bit pattern (e.g. <c>257</c>
    /// wrapping to <c>1</c> for a <see cref="byte"/>-backed enum) returns <see langword="false"/> instead of a
    /// false positive.
    /// </remarks>
    public static bool IsDefinedValue<TEnum>(int? value) where TEnum : struct, Enum
    {
        if (value is null)
            return false;

        if (!IsWithinUnderlyingRange<TEnum>(value.Value))
            return false;

        var boxed = Enum.ToObject(typeof(TEnum), value.Value);
#if NET8_0_OR_GREATER
        return Enum.IsDefined((TEnum)boxed);
#else
        return Enum.IsDefined(typeof(TEnum), boxed);
#endif
    }

    /// <summary>
    /// Determines whether <paramref name="value"/> fits within the numeric range of <typeparamref name="TEnum"/>'s
    /// underlying integral type, so that <see cref="Enum.ToObject(Type, int)"/> cannot silently truncate an
    /// out-of-range value onto a defined member's bit pattern.
    /// </summary>
    private static bool IsWithinUnderlyingRange<TEnum>(int value) where TEnum : struct, Enum
    {
        try
        {
            Convert.ChangeType(value, Enum.GetUnderlyingType(typeof(TEnum)), CultureInfo.InvariantCulture);
            return true;
        }
        catch (OverflowException)
        {
            return false;
        }
    }

    /// <summary>
    /// Determines whether the specified string is a valid member name of the enumeration.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to validate against.</typeparam>
    /// <param name="name">The name to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <param name="ignoreCase">
    /// When <see langword="true"/> (default), performs a case-insensitive comparison.
    /// </param>
    /// <returns><see langword="true"/> if <paramref name="name"/> is a valid enum member name; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// Compares <paramref name="name"/> against the enumeration's declared member names directly rather than via
    /// <see cref="Enum.TryParse{TEnum}(string, bool, out TEnum)"/>, because <c>TryParse</c> also accepts numeric
    /// strings (e.g. <c>"1"</c>) and comma-separated value lists (e.g. <c>"Monday, Tuesday"</c>), neither of which
    /// is a member name.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// bool valid = EnumRules.IsDefinedName<DayOfWeek>("monday");   // true (case-insensitive)
    /// bool invalid = EnumRules.IsDefinedName<DayOfWeek>("Holiday"); // false
    /// bool invalid = EnumRules.IsDefinedName<DayOfWeek>("1");       // false (not a member name)
    /// ]]></code>
    /// </example>
    public static bool IsDefinedName<TEnum>(string? name, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (!StringUtility.TryGetTrimmed(name, out var trimmed))
            return false;

        var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

#if NET8_0_OR_GREATER
        var names = Enum.GetNames<TEnum>();
#else
        var names = Enum.GetNames(typeof(TEnum));
#endif

        foreach (var candidate in names)
        {
            if (string.Equals(candidate, trimmed, comparison))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether the specified enum type is decorated with <see cref="FlagsAttribute"/>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type to check.</typeparam>
    /// <returns><see langword="true"/> if <typeparamref name="TEnum"/> has the <see cref="FlagsAttribute"/>; otherwise, <see langword="false"/>.</returns>
    /// <remarks>
    /// The reflection lookup is performed once per closed <typeparamref name="TEnum"/> and cached in
    /// <see cref="DefinedBitsCache{TEnum}"/>.
    /// </remarks>
    public static bool IsFlagsEnum<TEnum>() where TEnum : struct, Enum =>
        DefinedBitsCache<TEnum>.IsFlags;

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
    /// <remarks>
    /// The member is resolved via <see cref="Enum.GetName(Type, object)"/>. If <typeparamref name="TEnum"/> declares
    /// multiple constants sharing the same underlying value (aliases), <c>GetName</c> can only return one alias's
    /// name for that value, so this method (and <see cref="HasDescription{TEnum}"/>, <see cref="HasDisplay{TEnum}"/>,
    /// <see cref="HasEnumMember{TEnum}"/>, <see cref="IsObsolete{TEnum}"/>) inspects that resolved alias rather than
    /// the specific constant the caller may have had in mind. This mirrors the runtime's own <c>Enum.GetName</c>/
    /// <c>ToString</c> behavior for aliased values and cannot be distinguished by any API that accepts only the
    /// enum value.
    /// </remarks>
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
    /// Determines whether the specified enum member is marked with <c>[Obsolete]</c>.
    /// </summary>
    /// <typeparam name="TEnum">The enum type.</typeparam>
    /// <param name="value">The enum value to inspect. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if the member is obsolete; otherwise, <see langword="false"/>.</returns>
    public static bool IsObsolete<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, ObsoleteAttribute>(value);

    private static ulong GetDefinedBits<TEnum>() where TEnum : struct, Enum =>
        DefinedBitsCache<TEnum>.Bits;

    /// <summary>
    /// Reinterprets an enum member's underlying value as a <see cref="ulong"/> bit pattern, sign-extending
    /// negative signed-underlying values (e.g. <c>-1</c>) instead of throwing <see cref="OverflowException"/>.
    /// </summary>
    private static ulong ToUInt64<TEnum>(TEnum value) where TEnum : struct, Enum =>
        Type.GetTypeCode(typeof(TEnum)) switch
        {
            TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64 =>
                unchecked((ulong)Convert.ToInt64(value, CultureInfo.InvariantCulture)),
            _ => Convert.ToUInt64(value, CultureInfo.InvariantCulture)
        };

    private static class DefinedBitsCache<TEnum> where TEnum : struct, Enum
    {
        public static readonly ulong Bits = Compute();
        public static readonly bool IsFlags = typeof(TEnum).IsDefined(typeof(FlagsAttribute), inherit: false);

        private static ulong Compute()
        {
#if NET8_0_OR_GREATER
            var values = Enum.GetValues<TEnum>();
#else
            var values = (TEnum[])Enum.GetValues(typeof(TEnum));
#endif

            return values.Aggregate<TEnum, ulong>(0, (current, v) => current | ToUInt64(v));
        }
    }
}
