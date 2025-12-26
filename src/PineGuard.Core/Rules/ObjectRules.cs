namespace PineGuard.Rules;

public static class ObjectRules
{
    public static bool IsNull<T>(T? value) => value is null;

    public static bool IsNotNull<T>(T? value) => value is not null;

    public static bool IsEqualTo<T>(T? value, T? other) => EqualityComparer<T>.Default.Equals(value, other);

    public static bool IsOfType<T>(object? value) => value is T;

    public static bool IsAssignableToType<T>(object? value) => value is T || (value is not null && typeof(T).IsAssignableFrom(value.GetType()));

    public static bool IsSameReferenceAs<T>(T? a, T? b) where T : class => ReferenceEquals(a, b);
}
