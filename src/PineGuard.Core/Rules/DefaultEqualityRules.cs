namespace PineGuard.Rules;

public static class DefaultEqualityRules
{
    public static bool IsDefault<T>(T? value) => EqualityComparer<T>.Default.Equals(value, default);
}
