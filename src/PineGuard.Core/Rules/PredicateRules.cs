namespace PineGuard.Rules;

public static class PredicateRules
{
    public static bool Satisfies<T>(T? value, Func<T, bool> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);

        if (value is null)
            return false;

        return predicate(value);
    }
}
