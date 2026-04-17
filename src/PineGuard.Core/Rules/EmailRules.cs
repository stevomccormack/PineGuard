using PineGuard.Utils;

namespace PineGuard.Rules;

/// <summary>
/// Provides pure email address validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/email">Email Rules documentation</seealso>
public static class EmailRules
{
    /// <summary>
    /// Determines whether the specified value is a valid email address.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid email address; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Uses a permissive validation strategy via <see cref="EmailUtility.TryCreate"/>. For strict
    /// RFC-5321/RFC-5322 validation, use <see cref="IsStrictEmail"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = EmailRules.IsEmail("user@example.com");  // true
    /// bool invalid = EmailRules.IsEmail("not-an-email");    // false
    /// </code>
    /// </example>
    /// <seealso cref="IsStrictEmail"/>
    public static bool IsEmail(string? value) =>
        EmailUtility.TryCreate(value, out _);

    /// <summary>
    /// Determines whether the specified value is a strict (RFC-compliant) email address.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> passes strict RFC email validation; otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// Applies stricter validation than <see cref="IsEmail"/>. Delegates to <see cref="EmailUtility.TryStrictCreate"/>.
    /// </remarks>
    /// <example>
    /// <code>
    /// bool valid = EmailRules.IsStrictEmail("user@example.com"); // true
    /// </code>
    /// </example>
    /// <seealso cref="IsEmail"/>
    public static bool IsStrictEmail(string? value) =>
        EmailUtility.TryStrictCreate(value, out _);

    /// <summary>
    /// Checks whether the specified email address contains an alias (sub-address) after a <c>+</c> sign.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or whitespace, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is a valid email with a <c>+alias</c> sub-address;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool hasAlias = EmailRules.HasAlias("user+newsletter@example.com"); // true
    /// bool noAlias  = EmailRules.HasAlias("user@example.com");            // false
    /// </code>
    /// </example>
    public static bool HasAlias(string? value) =>
        EmailUtility.TryGetAlias(value, out _);
}
