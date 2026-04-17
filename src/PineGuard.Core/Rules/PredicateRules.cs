using PineGuard.Common;

namespace PineGuard.Rules;

/// <summary>
/// Provides a general-purpose predicate validation rule.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/predicate">Predicate Rules documentation</seealso>
public static class PredicateRules
{
    /// <summary>
    /// Determines whether the specified value satisfies the given predicate.
    /// </summary>
    /// <typeparam name="T">The type of the value to validate.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="predicate">
    /// The predicate function that the value must satisfy.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is not <see langword="null"/> and
    /// <paramref name="predicate"/> returns <see langword="true"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="predicate"/> is <see langword="null"/>.
    /// </exception>
    /// <example>
    /// <code>
    /// bool result = PredicateRules.Satisfies(42, x => x > 0); // true
    /// </code>
    /// </example>
    public static bool Satisfies<T>(T? value, Func<T, bool> predicate)
    {
        ThrowHelper.ThrowIfNull(predicate);

        return value is not null && predicate(value);
    }
}
