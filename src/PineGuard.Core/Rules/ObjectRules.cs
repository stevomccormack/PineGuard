namespace PineGuard.Rules;

/// <summary>
/// Provides pure object identity and type validation predicates.
/// </summary>
/// <seealso href="https://pineguard.ai/docs/rules/object">Object Rules documentation</seealso>
public static class ObjectRules
{
    /// <summary>
    /// Determines whether the specified value is equal to <paramref name="other"/> using the
    /// default equality comparer for type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values to compare.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, compared using default equality.</param>
    /// <param name="other">The value to compare against.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> equals <paramref name="other"/> according to
    /// <see cref="EqualityComparer{T}.Default"/>; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// bool equal = ObjectRules.IsEqualTo("hello", "hello"); // true
    /// </code>
    /// </example>
    public static bool IsEqualTo<T>(T? value, T? other) =>
        EqualityComparer<T>.Default.Equals(value!, other!);

    /// <summary>
    /// Determines whether the runtime type of the specified value is exactly <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The exact type to check against.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is not <see langword="null"/> and its runtime type
    /// is exactly <typeparamref name="T"/> (not a derived type); otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code><![CDATA[
    /// bool exact = ObjectRules.IsOfType<string>(obj); // true only if obj.GetType() == typeof(string)
    /// ]]></code>
    /// </example>
    public static bool IsOfType<T>(object? value) =>
        value is not null && value.GetType() == typeof(T);

    /// <summary>
    /// Determines whether the specified value is assignable to type <typeparamref name="T"/>
    /// (i.e., is an instance of <typeparamref name="T"/> or a derived type).
    /// </summary>
    /// <typeparam name="T">The target type to check assignability to.</typeparam>
    /// <param name="value">The value to validate. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is an instance of <typeparamref name="T"/>
    /// or a type derived from it; otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code><![CDATA[
    /// bool assignable = ObjectRules.IsAssignableToType<IDisposable>(myStream); // true
    /// ]]></code>
    /// </example>
    public static bool IsAssignableToType<T>(object? value) =>
        value is T;

    /// <summary>
    /// Determines whether <paramref name="value"/> and <paramref name="other"/> refer to the same object instance.
    /// </summary>
    /// <typeparam name="T">The reference type of the values.</typeparam>
    /// <param name="value">The first value to compare. May be <see langword="null"/>.</param>
    /// <param name="other">The second value to compare. May be <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> and <paramref name="other"/> are the same object reference;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <example>
    /// <code>
    /// var obj = new object();
    /// bool same = ObjectRules.IsSameReferenceAs(obj, obj); // true
    /// </code>
    /// </example>
    public static bool IsSameReferenceAs<T>(T? value, T? other) where T : class =>
        ReferenceEquals(value, other);
}
