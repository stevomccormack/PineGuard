using System.Reflection;

namespace PineGuard.Rules;

public static class EnumRules
{
    public static bool IsDefined<TEnum>(TEnum value) where TEnum : struct, Enum =>
        Enum.IsDefined(typeof(TEnum), value);

    public static bool IsNotDefined<TEnum>(TEnum value) where TEnum : struct, Enum =>
        !IsDefined(value);

    public static bool IsDefinedValue<TEnum>(int value) where TEnum : struct, Enum =>
        Enum.IsDefined(typeof(TEnum), value);

    public static bool IsNotDefinedValue<TEnum>(int value) where TEnum : struct, Enum =>
        !IsDefinedValue<TEnum>(value);

    public static bool IsDefinedName<TEnum>(string? name, bool ignoreCase = true)
        where TEnum : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        return Enum.TryParse<TEnum>(name, ignoreCase, out var parsed)
               && Enum.IsDefined(typeof(TEnum), parsed);
    }

    public static bool IsNotDefinedName<TEnum>(string? name, bool ignoreCase = true)
        where TEnum : struct, Enum =>
        !IsDefinedName<TEnum>(name, ignoreCase);

    public static bool IsFlagsEnum<TEnum>() where TEnum : struct, Enum =>
        typeof(TEnum).IsDefined(typeof(FlagsAttribute), inherit: false);

    public static bool IsNotFlagsEnum<TEnum>() where TEnum : struct, Enum =>
        !IsFlagsEnum<TEnum>();

    public static bool IsDefinedFlagsCombination<TEnum>(TEnum value)
        where TEnum : struct, Enum
    {
        if (!IsFlagsEnum<TEnum>())
        {
            return Enum.IsDefined(typeof(TEnum), value);
        }

        var definedBits = GetDefinedBits<TEnum>();
        var valBits = ToUInt64(value);

        return (valBits & ~definedBits) == 0;
    }

    public static bool IsNotDefinedFlagsCombination<TEnum>(TEnum value)
        where TEnum : struct, Enum =>
        !IsDefinedFlagsCombination(value);

    public static bool HasFlag<TEnum>(TEnum value, TEnum flag)
        where TEnum : struct, Enum
    {
        var valueBits = ToUInt64(value);
        var flagBits = ToUInt64(flag);

        return (valueBits & flagBits) == flagBits;
    }

    public static bool HasNotFlag<TEnum>(TEnum value, TEnum flag)
        where TEnum : struct, Enum =>
        !HasFlag(value, flag);

    public static bool HasAttribute<TEnum, TAttribute>(TEnum value)
        where TEnum : struct, Enum
        where TAttribute : Attribute
    {
        var name = Enum.GetName(typeof(TEnum), value);
        if (name is null)
        {
            return false;
        }

        var member = typeof(TEnum).GetMember(name, BindingFlags.Public | BindingFlags.Static);
        if (member.Length == 0)
        {
            return false;
        }

        return member[0].IsDefined(typeof(TAttribute), inherit: false);
    }

    public static bool HasNotAttribute<TEnum, TAttribute>(TEnum value)
        where TEnum : struct, Enum
        where TAttribute : Attribute =>
        !HasAttribute<TEnum, TAttribute>(value);

    private static ulong GetDefinedBits<TEnum>() where TEnum : struct, Enum
    {
        ulong bits = 0;
        var values = (TEnum[])Enum.GetValues(typeof(TEnum));
        foreach (var v in values)
        {
            bits |= ToUInt64(v);
        }

        return bits;
    }

    private static ulong ToUInt64<TEnum>(TEnum value) where TEnum : struct, Enum =>
        Convert.ToUInt64(value);
}
