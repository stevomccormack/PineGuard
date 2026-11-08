using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace PineGuard.Common;

/// <summary>
/// A generic, type-safe smart-enum base class that supports value and name lookups.
/// </summary>
/// <remarks>
/// Catalogues contain public static readonly fields whose declared types are assignable to the queried type,
/// including inherited fields. Constructing an instance does not register it. Values must retain stable equality
/// and hash codes. String values use ordinal identity; names are looked up case-insensitively.
/// Catalogue queries from an enumeration's own static initialization or construction path are unsupported.
/// Queries require successful type initialization; invalid catalogues fail without publishing partial results.
/// </remarks>
/// <typeparam name="TValue">The type of the enumeration value. Must implement <see cref="IComparable{T}"/>.</typeparam>
public abstract class Enumeration<TValue> : IEquatable<Enumeration<TValue>>, IComparable<Enumeration<TValue>>
    where TValue : IComparable<TValue>
{
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
    /// <param name="value">The value of the enumeration member. Must be unique within a catalogue.</param>
    /// <param name="name">The display name. Must be unique, ignoring case, within a catalogue.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is empty or whitespace.</exception>
    protected Enumeration(TValue value, string name)
    {
        ThrowHelper.ThrowIfNull(value);
        ThrowHelper.ThrowIfNullOrWhiteSpace(name);

        Value = value;
        Name = name;
    }

    /// <summary>
    /// Gets the cached, immutable catalogue of the specified enumeration type, ordered by ordinal name.
    /// </summary>
    /// <typeparam name="T">
    /// The concrete enumeration type. Annotated to preserve public fields and constructors so
    /// trimming tools preserve static members and the type initializer used by discovery; without these annotations, a
    /// trimmed publish may remove those fields when <typeparamref name="T"/> is reached only through this API.
    /// </typeparam>
    /// <returns>A read-only list of eligible declared and inherited members.</returns>
    /// <exception cref="InvalidOperationException">An eligible field is null, a name or value is duplicated, or catalogue construction is reentered.</exception>
    /// <exception cref="TypeInitializationException">An enumeration type's static initialization fails.</exception>
    public static IReadOnlyList<T> GetAll<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>() where T : Enumeration<TValue> =>
        Catalogue<T>.Instance.Value.Members;

    /// <summary>
    /// Finds an enumeration member by its value.
    /// </summary>
    /// <typeparam name="T">
    /// The concrete enumeration type. Annotated to preserve public fields and constructors; see
    /// <see cref="GetAll{T}"/> for why.
    /// </typeparam>
    /// <param name="value">The value to search for.</param>
    /// <returns>The matching member, or <see langword="null"/> if not found.</returns>
    public static T? FromValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(TValue value) where T : Enumeration<TValue>
    {
        ThrowHelper.ThrowIfNull(value);
        return Catalogue<T>.Instance.Value.ByValue.TryGetValue(value, out var member) ? member : null;
    }

    /// <summary>
    /// Attempts to find an enumeration member by its value.
    /// </summary>
    /// <typeparam name="T">
    /// The concrete enumeration type. Annotated to preserve public fields and constructors; see
    /// <see cref="GetAll{T}"/> for why.
    /// </typeparam>
    /// <param name="value">The value to search for.</param>
    /// <param name="result">When this method returns, contains the matching member if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if a matching member was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromValue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(TValue? value, [NotNullWhen(true)] out T? result) where T : Enumeration<TValue>
    {
        if (value is null)
        {
            result = null;
            return false;
        }

        result = FromValue<T>(value);
        return result != null;
    }

    /// <summary>
    /// Finds an enumeration member by its name (case-insensitive).
    /// </summary>
    /// <typeparam name="T">
    /// The concrete enumeration type. Annotated to preserve public fields and constructors; see
    /// <see cref="GetAll{T}"/> for why.
    /// </typeparam>
    /// <param name="name">The name to search for.</param>
    /// <returns>The matching member, or <see langword="null"/> if not found.</returns>
    public static T? FromName<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(string name) where T : Enumeration<TValue>
    {
        ThrowHelper.ThrowIfNullOrWhiteSpace(name);
        return Catalogue<T>.Instance.Value.ByName.TryGetValue(name, out var member) ? member : null;
    }

    /// <summary>
    /// Attempts to find an enumeration member by its name (case-insensitive).
    /// </summary>
    /// <typeparam name="T">
    /// The concrete enumeration type. Annotated to preserve public fields and constructors; see
    /// <see cref="GetAll{T}"/> for why.
    /// </typeparam>
    /// <param name="name">The name to search for.</param>
    /// <param name="result">When this method returns, contains the matching member if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if a matching member was found; otherwise, <see langword="false"/>.</returns>
    public static bool TryFromName<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(string? name, [NotNullWhen(true)] out T? result) where T : Enumeration<TValue>
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
    /// <remarks>
    /// String-valued members are compared with <see cref="string.CompareOrdinal(string?, string?)"/> rather than
    /// <see cref="IComparable{T}.CompareTo(T)"/>, so ordering is deterministic and independent of the current culture.
    /// </remarks>
    public int CompareTo(Enumeration<TValue>? other)
    {
        if (other is null)
            return 1;

        if (Value is string left && other.Value is string right)
            return string.CompareOrdinal(left, right);

        return Value.CompareTo(other.Value);
    }

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

    private sealed class Catalogue<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicFields | DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.NonPublicConstructors)] T>(
        IReadOnlyList<T> members, Dictionary<string, T> byName, Dictionary<TValue, T> byValue)
        where T : Enumeration<TValue>
    {
        internal static readonly Lazy<Catalogue<T>> Instance = new(Create);

        internal IReadOnlyList<T> Members { get; } = members;
        internal Dictionary<string, T> ByName { get; } = byName;
        internal Dictionary<TValue, T> ByValue { get; } = byValue;

        private static Catalogue<T> Create()
        {
            typeof(T).TypeInitializer?.Invoke(obj: null, parameters: null);
            var members = new List<T>();
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy))
            {
                if (!field.IsInitOnly || !typeof(T).IsAssignableFrom(field.FieldType))
                    continue;

                if (field.GetValue(null) is not T member)
                    throw new InvalidOperationException($"Enumeration member '{field.Name}' in {typeof(T).Name} is null.");

                members.Add(member);
            }

            members.Sort((left, right) => StringComparer.Ordinal.Compare(left.Name, right.Name));
            var byName = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);
            var byValue = new Dictionary<TValue, T>();
            foreach (var member in members)
            {
                if (!byName.TryAdd(member.Name, member))
                    throw new InvalidOperationException($"Name '{member.Name}' already exists in {typeof(T).Name}.");
                if (!byValue.TryAdd(member.Value, member))
                    throw new InvalidOperationException($"Value '{member.Value}' already exists in {typeof(T).Name}.");
            }

            return new Catalogue<T>(Array.AsReadOnly(members.ToArray()), byName, byValue);
        }
    }
}
