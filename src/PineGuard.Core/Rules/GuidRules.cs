namespace PineGuard.Rules;

/// <summary>
/// Provides pure <see cref="System.Guid"/> validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/guid">Guid Rules documentation</seealso>
public static class GuidRules
{
    /// <summary>
    /// Determines whether the specified value is empty (equal to <see cref="System.Guid.Empty"/>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/> (null is absent, not empty).</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is not <see langword="null"/> and equal to
    /// <see cref="System.Guid.Empty"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool empty = GuidRules.IsEmpty(Guid.Empty);   // true
    /// bool empty = GuidRules.IsEmpty(Guid.NewGuid()); // false
    /// bool empty = GuidRules.IsEmpty(null); // false
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/guid">Guid Rules documentation</seealso>
    public static bool IsEmpty(Guid? value) => value is not null && value.Value == Guid.Empty;

    /// <summary>
    /// Determines whether the specified value is not empty (not equal to <see cref="System.Guid.Empty"/>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is not <see langword="null"/> and not equal to
    /// <see cref="System.Guid.Empty"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool notEmpty = GuidRules.IsNotEmpty(Guid.NewGuid()); // true
    /// bool notEmpty = GuidRules.IsNotEmpty(Guid.Empty);     // false
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/guid">Guid Rules documentation</seealso>
    public static bool IsNotEmpty(Guid? value) => value is not null && value.Value != Guid.Empty;
}
