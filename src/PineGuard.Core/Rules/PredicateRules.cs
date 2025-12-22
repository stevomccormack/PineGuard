namespace PineGuard.Rules;

public static class PredicateRules
{
    public static bool Satisfies<T>(T value, Func<T, bool> predicate)
    {
        if (predicate is null)
        {
            throw new ArgumentNullException(nameof(predicate));
        }

        return predicate(value);
    }

    public static bool DoesNotSatisfy<T>(T value, Func<T, bool> predicate) =>
        !Satisfies(value, predicate);
}
