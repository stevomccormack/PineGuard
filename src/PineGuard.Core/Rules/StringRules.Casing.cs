using PineGuard.Common;
using PineGuard.Utils;

namespace PineGuard.Rules;

public static partial class StringRules
{
    /// <summary>
    /// Determines whether the specified string conforms to the given casing style.
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <param name="style">The <see cref="StringCasing"/> style to validate against.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> conforms to <paramref name="style"/>; otherwise, <see langword="false"/>.</returns>
    public static bool IsCaseStyle(string? value, StringCasing style)
        => StringUtility.TryCreateWords(value, style, out _);

    /// <summary>
    /// Determines whether the specified string is in camelCase format (e.g., <c>"myVariableName"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in camelCase; otherwise, <see langword="false"/>.</returns>
    public static bool IsCamelCase(string? value)
        => IsCaseStyle(value, StringCasing.CamelCase);

    /// <summary>
    /// Determines whether the specified string is in PascalCase format (e.g., <c>"MyVariableName"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in PascalCase; otherwise, <see langword="false"/>.</returns>
    public static bool IsPascalCase(string? value)
        => IsCaseStyle(value, StringCasing.PascalCase);

    /// <summary>
    /// Determines whether the specified string is in snake_case format (e.g., <c>"my_variable_name"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in snake_case; otherwise, <see langword="false"/>.</returns>
    public static bool IsSnakeCase(string? value)
        => IsCaseStyle(value, StringCasing.SnakeCase);

    /// <summary>
    /// Determines whether the specified string is in UPPER_SNAKE_CASE format (e.g., <c>"MY_VARIABLE_NAME"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in UPPER_SNAKE_CASE; otherwise, <see langword="false"/>.</returns>
    public static bool IsUpperSnakeCase(string? value)
        => IsCaseStyle(value, StringCasing.UpperSnakeCase);

    /// <summary>
    /// Determines whether the specified string is in kebab-case format (e.g., <c>"my-variable-name"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in kebab-case; otherwise, <see langword="false"/>.</returns>
    public static bool IsKebabCase(string? value)
        => IsCaseStyle(value, StringCasing.KebabCase);

    /// <summary>
    /// Determines whether the specified string is in Train-Case format (e.g., <c>"My-Variable-Name"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in Train-Case; otherwise, <see langword="false"/>.</returns>
    public static bool IsTrainCase(string? value)
        => IsCaseStyle(value, StringCasing.TrainCase);

    /// <summary>
    /// Determines whether the specified string is in <c>dot.case</c> format (e.g., <c>"my.variable.name"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in <c>dot.case</c>; otherwise, <see langword="false"/>.</returns>
    public static bool IsDotCase(string? value)
        => IsCaseStyle(value, StringCasing.DotCase);

    /// <summary>
    /// Determines whether the specified string is in space case format (e.g., <c>"my variable name"</c>).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is in space case; otherwise, <see langword="false"/>.</returns>
    public static bool IsSpaceCase(string? value)
        => IsCaseStyle(value, StringCasing.SpaceCase);

    /// <summary>
    /// Determines whether all letter characters in the specified string are uppercase (culture-invariant).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if every letter in <paramref name="value"/> is uppercase using invariant comparison; otherwise, <see langword="false"/>.</returns>
    public static bool IsUpperInvariant(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && trimmed.Where(char.IsLetter).All(ch => char.ToUpperInvariant(ch) == ch);

    /// <summary>
    /// Determines whether all letter characters in the specified string are lowercase (culture-invariant).
    /// </summary>
    /// <param name="value">The value to validate. If <see langword="null"/> or empty, returns <see langword="false"/>.</param>
    /// <returns><see langword="true"/> if every letter in <paramref name="value"/> is lowercase using invariant comparison; otherwise, <see langword="false"/>.</returns>
    public static bool IsLowerInvariant(string? value) =>
        StringUtility.TryGetTrimmed(value, out var trimmed) && trimmed.Where(char.IsLetter).All(ch => char.ToLowerInvariant(ch) == ch);
}
