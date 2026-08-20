namespace PineGuard.Rules;

/// <summary>
/// Provides pure default-value validation predicates using <see cref="EqualityComparer{T}.Default"/>.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/default">Default Equality Rules documentation</seealso>
public static class DefaultEqualityRules
{
    /// <summary>
    /// Determines whether the specified value equals the default value for its type.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to validate. <see langword="null"/> for reference types equals <see langword="default"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> equals <c>default(T)</c> according to
    /// <see cref="EqualityComparer{T}.Default"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool isDefault = DefaultEqualityRules.IsDefault(0);   // true for int
    /// bool isDefault = DefaultEqualityRules.IsDefault(null); // true for reference types
    /// </code>
    /// </example>
    public static bool IsDefault<T>(T? value) => EqualityComparer<T>.Default.Equals(value!, default!);

    /// <summary>
    /// Determines whether the specified value is <see langword="null"/> or equals the default value for its type,
    /// unwrapping <see cref="Nullable{T}"/> so a non-null nullable value that wraps its underlying type's default
    /// (e.g. <c>(int?)0</c>) is also treated as null-or-default.
    /// </summary>
    /// <typeparam name="T">The type of the value to check.</typeparam>
    /// <param name="value">The value to validate.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is <see langword="null"/>, equals <c>default(T)</c>, or
    /// (when <typeparamref name="T"/> is <see cref="Nullable{T}"/>) wraps a value equal to its underlying type's
    /// default; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool nullOrDefault = DefaultEqualityRules.IsNullOrDefault(null);    // true
    /// bool nullOrDefault = DefaultEqualityRules.IsNullOrDefault(0);      // true for int
    /// bool nullOrDefault = DefaultEqualityRules.IsNullOrDefault((int?)0); // true (Nullable&lt;T&gt; unwrapped)
    /// </code>
    /// </example>
    public static bool IsNullOrDefault<T>(T? value)
    {
        if (value is null)
            return true;

        var underlyingType = Nullable.GetUnderlyingType(typeof(T));
        return underlyingType is not null
            ? Equals(value, Activator.CreateInstance(underlyingType))
            : IsDefault(value);
    }
}
