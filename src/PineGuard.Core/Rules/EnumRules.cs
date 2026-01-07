using PineGuard.Utils;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.Serialization;

namespace PineGuard.Rules;

public static class EnumRules
{
    public static bool IsDefined<TEnum>(TEnum? value) where TEnum : struct, Enum
    {
        if (value is null)
            return false;

        return Enum.IsDefined(typeof(TEnum), value.Value);
    }

    public static bool IsDefinedValue<TEnum>(int? value) where TEnum : struct, Enum
    {
        if (value is null)
            return false;

        return Enum.IsDefined(typeof(TEnum), value.Value);
    }

    public static bool IsDefinedName<TEnum>(string? name, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (!StringUtility.TryGetTrimmed(name, out var trimmed))
            return false;

        return Enum.TryParse<TEnum>(trimmed, ignoreCase, out var parsed)
            && Enum.IsDefined(typeof(TEnum), parsed);
    }

    public static bool IsFlagsEnum<TEnum>() where TEnum : struct, Enum =>
        typeof(TEnum).IsDefined(typeof(FlagsAttribute), inherit: false);

    public static bool IsFlagsEnumCombination<TEnum>(TEnum? value)
        where TEnum : struct, Enum
    {
        if (value is null)
            return false;

        if (!IsFlagsEnum<TEnum>())
            return Enum.IsDefined(typeof(TEnum), value.Value);

        var definedBits = GetDefinedBits<TEnum>();
        var valBits = ToUInt64(value.Value);

        return (valBits & ~definedBits) == 0;
    }

    public static bool HasFlag<TEnum>(TEnum? value, TEnum flag)
        where TEnum : struct, Enum =>
        value is not null && value.Value.HasFlag(flag);

    public static bool HasAttribute<TEnum, TAttribute>(TEnum? value)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        if (value is null)
            return false;

        var name = Enum.GetName(typeof(TEnum), value.Value);
        if (name is null)
            return false;

        var member = typeof(TEnum).GetMember(name, BindingFlags.Public | BindingFlags.Static);
        return member[0].IsDefined(typeof(TAttribute), inherit: false);
    }

    public static bool HasDescription<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, DescriptionAttribute>(value);

    public static bool HasDisplay<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, DisplayAttribute>(value);

    public static bool HasEnumMember<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, EnumMemberAttribute>(value);

    public static bool IsObsolete<TEnum>(TEnum? value) where TEnum : struct, Enum =>
        HasAttribute<TEnum, ObsoleteAttribute>(value);

    private static ulong GetDefinedBits<TEnum>() where TEnum : struct, Enum
    {
        ulong bits = 0;
        var values = (TEnum[])Enum.GetValues(typeof(TEnum));
        foreach (var v in values)
            bits |= ToUInt64(v);

        return bits;
    }

    private static ulong ToUInt64<TEnum>(TEnum value) where TEnum : struct, Enum =>
        Convert.ToUInt64(value);
}
