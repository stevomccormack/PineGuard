namespace PineGuard.MustClauses;

/// <summary>
/// The <c>object? -&gt; T</c> cast behind the non-generic <see cref="IMustValidator"/> members.
/// </summary>
internal static class MustValidatorCast
{
    /// <summary>
    /// Casts <paramref name="value"/> to <typeparamref name="T"/>, or returns <see langword="default"/>
    /// when <paramref name="value"/> is <see langword="null"/> and <typeparamref name="T"/> permits null at runtime.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is neither <see langword="null"/> nor a <typeparamref name="T"/>.</exception>
    public static T To<T>(object? value)
        where T : notnull
    {
        if (value is T typed)
            return typed;

        if (value is null && default(T) is null)
            return default!;

        throw new ArgumentException(
            $"Expected a value of type '{typeof(T)}' but received '{value?.GetType().ToString() ?? "null"}'.",
            nameof(value));
    }
}
