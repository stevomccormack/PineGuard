namespace PineGuard.Rules;

/// <summary>
/// Provides pure null-check validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/null">Null Rules documentation</seealso>
public static class NullRules
{
    /// <summary>
    /// Determines whether the specified value is <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = NullRules.IsNull(myObject); // true when myObject is null
    /// </code>
    /// </example>
    public static bool IsNull<T>(T? value) => value is null;

    /// <summary>
    /// Determines whether the specified value is not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = NullRules.IsNotNull(myObject); // true when myObject is not null
    /// </code>
    /// </example>
    public static bool IsNotNull<T>(T? value) => value is not null;
}
