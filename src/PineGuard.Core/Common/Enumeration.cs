using System.Collections.Concurrent;
using System.Reflection;

namespace PineGuard.Common;

public abstract class Enumeration<TValue> : IEquatable<Enumeration<TValue>>, IComparable<Enumeration<TValue>>
    where TValue : notnull, IComparable<TValue>
{
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, Enumeration<TValue>>> _nameRegistries = new();
    private static readonly ConcurrentDictionary<Type, ConcurrentDictionary<TValue, Enumeration<TValue>>> _valueRegistries = new();

    public TValue Value { get; }
    public string Name { get; }

    protected Enumeration(TValue value, string name)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var type = GetType();
        var nameRegistry = _nameRegistries.GetOrAdd(type, _ => new ConcurrentDictionary<string, Enumeration<TValue>>(StringComparer.OrdinalIgnoreCase));
        if (!nameRegistry.TryAdd(name, this))
        {
            throw new ArgumentException($"An enumeration with the name '{name}' already exists in {type.Name}.", nameof(name));
        }

        var valueRegistry = _valueRegistries.GetOrAdd(type, _ => new ConcurrentDictionary<TValue, Enumeration<TValue>>());
        if (!valueRegistry.TryAdd(value, this))
        {
            nameRegistry.TryRemove(name, out _); // Rollback name registration
            throw new ArgumentException($"An enumeration with the value '{value}' already exists in {type.Name}.", nameof(value));
        }

        Value = value;
        Name = name;
    }

    public static IReadOnlyList<T> GetAll<T>() where T : Enumeration<TValue>
    {
        return [.. typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(f => typeof(T).IsAssignableFrom(f.FieldType))
            .Select(f => (T)f.GetValue(null)!)];
    } 

    public static T? FromValue<T>(TValue value) where T : Enumeration<TValue>
    {
        ArgumentNullException.ThrowIfNull(value);
        return GetAll<T>().FirstOrDefault(e => EqualityComparer<TValue>.Default.Equals(e.Value, value));
    }

    public static bool TryFromValue<T>(TValue? value, out T? result) where T : Enumeration<TValue>
    {
        if (value == null)
        {
            result = null;
            return false;
        }

        result = FromValue<T>(value);
        return result != null;
    }

    public static T? FromName<T>(string name) where T : Enumeration<TValue>
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        return GetAll<T>().FirstOrDefault(e => string.Equals(e.Name, name, StringComparison.OrdinalIgnoreCase));
    }

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

    public bool Equals(Enumeration<TValue>? other)
    {
        if (other is null)
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return GetType() == other.GetType() && EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }

    public override bool Equals(object? obj) => obj is Enumeration<TValue> other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Name;

    public int CompareTo(Enumeration<TValue>? other)
    {
        return (other is null) ? 1 : Value.CompareTo(other.Value);
    }

    public static bool operator ==(Enumeration<TValue>? left, Enumeration<TValue>? right)
    {
        return (left is null)? right is null: left.Equals(right);
    }

    public static bool operator !=(Enumeration<TValue>? left, Enumeration<TValue>? right)
    {
        return !(left == right);
    }

    public static implicit operator string(Enumeration<TValue> enumeration) => enumeration.Name;

    public static implicit operator TValue(Enumeration<TValue> enumeration) => enumeration.Value;
}
