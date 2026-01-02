using PineGuard.Utils;

namespace PineGuard.Rules;

public static class EmailRules
{
    public static bool IsEmail(string? value) =>
        EmailUtility.TryCreate(value, out _);

    public static bool IsStrictEmail(string? value) =>
        EmailUtility.TryStrictCreate(value, out _);

    public static bool HasAlias(string? value) =>
        EmailUtility.TryGetAlias(value, out _);
}
