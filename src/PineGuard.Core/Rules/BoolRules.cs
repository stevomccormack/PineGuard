namespace PineGuard.Rules;

/// <summary>
/// Provides pure boolean validation predicates.
/// </summary>
/// <remarks>
/// Used internally by MustClauses and GuardClauses for boolean validation.
/// </remarks>
/// <seealso href="https://pineguard.ai/docs/rules/bool">Bool Rules documentation</seealso>
public static class BoolRules
{
    /// <summary>
    /// Determines whether the specified value is <see langword="true"/>.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is <see langword="true"/>; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = BoolRules.IsTrue(isActive); // true when isActive == true
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/bool">Bool Rules documentation</seealso>
    public static bool IsTrue(bool? value) => value is true;

    /// <summary>
    /// Determines whether the specified value is <see langword="false"/>.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is <see langword="false"/>; otherwise, <see langword="false"/>.</returns>
    /// <example>
    /// <code>
    /// bool result = BoolRules.IsFalse(isDeleted); // true when isDeleted == false
    /// </code>
    /// </example>
    /// <seealso href="https://pineguard.ai/docs/rules/bool">Bool Rules documentation</seealso>
    public static bool IsFalse(bool? value) => value is false;
}
