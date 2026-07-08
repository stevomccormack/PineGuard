using System.Collections.Concurrent;
using System.Reflection;
using PineGuard.Extensions;

namespace PineGuard.Common;

/// <summary>
/// A generic, type-safe smart-enum base class that supports value and name lookups.
/// </summary>
/// <typeparam name="TValue">The type of the enumeration value. Must implement <see cref="IComparable{T}"/>.</typeparam>
public abstract class Enumeration<TValue> : IEquatable<Enumeration<TValue>>, IComparable<Enumeration<TValue>>
    where TValue : IComparable<TValue>
{
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Enumeration<TValue>>> NameRegistries = new();
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<TValue, Enumeration<TValue>>> ValueRegistries = new();

    /// <summary>
    /// Gets the value of the enumeration member.
    /// </summary>
    public TValue Value { get; }

    /// <summary>
    /// Gets the display name of the enumeration member.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration{TValue}"/> class.
    /// </summary>
    /// <param name="value">The value of the enumeration member. Must be unique within the derived type.</param>
    /// <param name="name">The display name. Must be unique within the derived type.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is <see langword="null"/> or whitespace, or when a duplicate value or name is registered.</exception>
    protected Enumeration(TValue value, string name)
    {
        ThrowHelper.ThrowIfNull(value);
        ThrowHelper.ThrowIfNullOrWhiteSpace(name);

        var type = GetType();
        var nameRegistry = NameRegistries.GetOrAdd(type, _ => new ConcurrentDictionary<string, Enumeration<TValue>>(StringComparer.OrdinalIgnoreCase));
        if (!nameRegistry.TryAdd(name, this)) throw new ArgumentException($"{nameof(name).TitleCase()} '{name}' already exists in {type.Name}.", nameof(name));

        var valueRegistry = ValueRegistries.GetOrAdd(type, _ => new ConcurrentDictionary<TValue, Enumeration<TValue>>());
        if (!valueRegistry.TryAdd(value, this))
        {
            nameRegistry.TryRemove(name, out _); // Rollback name registration
            throw new ArgumentException($"{nameof(value).TitleCase()} '{value}' already exists in {type.Name}.", nameof(value));
        }

        Value = value;
        Name = name;
    }

    /// <summary>
    /// Gets all declared members of the specified enumeration type.
    /// </summary>
    /// <typeparam name="T">The concrete enumeration type.</typeparam>
    /// <returns>A read-only list of all declared members.</returns>
    public static IReadOnlyList<T> GetAll<T>() where T : Enumeration<TValue> =>
    [.. typeof(T)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
        .Where(f => typeof(T).IsAssignableFrom(f.FieldType))
        .Select(f => (T)f.GetValue(null)!)];

    /// <summary>
    /// Finds an enumeration member by its value.
    /// </summary>
    /// <typeparam name="T">The concrete enumeration type.</typeparam>
    /// <param name="value">The value to search for.</param>
    /// <returns>The matching member, or <see langword="null"/> if not found.</returns>
    public static T? FromValue<T>(TValue value) where T : Enumeration<TValue>
    {
        ThrowHelper.ThrowIfNull(value);
        return GetAll<T>().FirstOrDefault(e => EqualityComparer<TValue>.Default.Equals(e.Value, value));
    }

    /// <summary>
    /// Attempts to find an enumeration member by its value.
    /// </summary>
    /// <typeparam name="T">The concrete enumeration type.</typeparam>
    /// <param name="value">The value to search for.</param>
    /// <param name="result">When this method returns, contains the matching member if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if a matching member was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromValue<T>(TValue? value, out T? result) where T : Enumeration<TValue>
    {
        if (EqualityComparer<TValue>.Default.Equals(value!, default!))
        {
            result = null;
            return false;
        }

        result = FromValue<T>(value!);
        return result != null;
    }

    /// <summary>
    /// Finds an enumeration member by its name (case-insensitive).
    /// </summary>
    /// <typeparam name="T">The concrete enumeration type.</typeparam>
    /// <param name="name">The name to search for.</param>
    /// <returns>The matching member, or <see langword="null"/> if not found.</returns>
    public static T? FromName<T>(string name) where T : Enumeration<TValue>
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(name);
        return GetAll<T>().FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Attempts to find an enumeration member by its name (case-insensitive).
    /// </summary>
    /// <typeparam name="T">The concrete enumeration type.</typeparam>
    /// <param name="name">The name to search for.</param>
    /// <param name="result">When this method returns, contains the matching member if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if a matching member was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromName<T>(string? name, out T? result) where T : Enumeration<TValue>
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            result = null;
            return false;
        }

        result = FromName<T>(name);
        return result != null;
    }

    /// <inheritdoc />
    public bool Equals(Enumeration<TValue>? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return GetType() == other.GetType() && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Enumeration<TValue> other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => Value.GetHashCode();

    /// <inheritdoc />
    public override string ToString() => Name;

    /// <inheritdoc />
    public int CompareTo(Enumeration<TValue>? other) => other is null ? 1 : Value.CompareTo(other.Value);

    /// <summary>
    /// Determines whether two enumeration instances are equal.
    /// </summary>
    public static bool operator ==(Enumeration<TValue>? left, Enumeration<TValue>? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    /// Determines whether two enumeration instances are not equal.
    /// </summary>
    public static bool operator !=(Enumeration<TValue>? left, Enumeration<TValue>? right) => !(left == right);

    /// <summary>
    /// Determines whether one enumeration instance sorts before another by value.
    /// </summary>
    public static bool operator <(Enumeration<TValue>? left, Enumeration<TValue>? right) => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Determines whether one enumeration instance sorts before or equal to another by value.
    /// </summary>
    public static bool operator <=(Enumeration<TValue>? left, Enumeration<TValue>? right) => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Determines whether one enumeration instance sorts after another by value.
    /// </summary>
    public static bool operator >(Enumeration<TValue>? left, Enumeration<TValue>? right) => left is not null && left.CompareTo(right) > 0;

    /// <summary>
    /// Determines whether one enumeration instance sorts after or equal to another by value.
    /// </summary>
    public static bool operator >=(Enumeration<TValue>? left, Enumeration<TValue>? right) => left is null ? right is null : left.CompareTo(right) >= 0;

    /// <summary>
    /// Implicitly converts an enumeration member to its name string.
    /// </summary>
    /// <param name="enumeration">The enumeration member to convert.</param>
    public static implicit operator string(Enumeration<TValue> enumeration) => enumeration.Name;

    /// <summary>
    /// Implicitly converts an enumeration member to its underlying value.
    /// </summary>
    /// <param name="enumeration">The enumeration member to convert.</param>
    public static implicit operator TValue(Enumeration<TValue> enumeration) => enumeration.Value;
}
