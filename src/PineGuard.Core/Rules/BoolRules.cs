namespace PineGuard.Rules;

public static class BoolRules
{
    public static bool IsTrue(bool? value) => value is true;

    public static bool IsFalse(bool? value) => value is false;

    public static bool IsNullOrTrue(bool? value) => value is null || value.Value;

    public static bool IsNullOrFalse(bool? value) => value is null || !value.Value;
}
