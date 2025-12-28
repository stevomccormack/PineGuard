using PineGuard.Utils;

namespace PineGuard.Rules;

public static class EmailRules
{
    public static bool IsEmail(string? value) =>
        EmailUtility.TryCreate(value, out _);
}
