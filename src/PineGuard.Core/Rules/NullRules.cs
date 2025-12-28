namespace PineGuard.Rules;

public static class NullRules
{
    public static bool IsNull<T>(T? value) => value is null;

    public static bool IsNotNull<T>(T? value) => value is not null;
}
