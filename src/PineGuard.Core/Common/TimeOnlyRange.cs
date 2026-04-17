#if NET8_0_OR_GREATER
using PineGuard.Extensions;

namespace PineGuard.Common;

/// <summary>
/// Represents an immutable, inclusive time range defined by a start and end <see cref="TimeOnly"/>.
/// </summary>
public readonly struct TimeOnlyRange : IEquatable<TimeOnlyRange>
{
    /// <summary>
    /// Gets the start time of the range.
    /// </summary>
    public TimeOnly Start { get; }

    /// <summary>
    /// Gets the end time of the range.
    /// </summary>
    public TimeOnly End { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TimeOnlyRange"/> struct.
    /// </summary>
    /// <param name="start">The start time. Must be less than or equal to <paramref name="end"/>.</param>
    /// <param name="end">The end time.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="start"/> is greater than <paramref name="end"/>.</exception>
    public TimeOnlyRange(TimeOnly start, TimeOnly end)
    {
        if (start > end)
            throw new ArgumentException($"{nameof(start).TitleCase()} must be less than or equal to {nameof(end).TitleCase()}.", nameof(start));

        Start = start;
        End = end;
    }

    /// <summary>
    /// Attempts to create a <see cref="TimeOnlyRange"/> from nullable start and end times.
    /// </summary>
    /// <param name="start">The start time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="range">When this method returns, contains the created range if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the range was created successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreate(TimeOnly? start, TimeOnly? end, out TimeOnlyRange range)
    {
        range = default;

        if (start is null || end is null)
            return false;

        var s = start.Value;
        var e = end.Value;

        if (s > e)
            return false;

        range = new TimeOnlyRange(s, e);
        return true;
    }

    /// <summary>
    /// Gets the duration of the range as a <see cref="TimeSpan"/>.
    /// </summary>
    public TimeSpan Duration => End.ToTimeSpan() - Start.ToTimeSpan();

    /// <summary>
    /// Determines whether the specified time falls within this range (inclusive).
    /// </summary>
    /// <param name="value">The time to check.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(TimeOnly value) => value >= Start && value <= End;

    /// <summary>
    /// Determines whether this range overlaps with another range (exclusive boundaries).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(TimeOnlyRange other) => Start < other.End && other.Start < End;

    /// <summary>
    /// Determines whether this range overlaps with another range using the specified boundary inclusion.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(TimeOnlyRange other, Inclusion inclusion)
    {
        if (inclusion == Inclusion.Exclusive)
            return Start < other.End && other.Start < End;

        return Start <= other.End && other.Start <= End;
    }

    /// <inheritdoc />
    public bool Equals(TimeOnlyRange other) => Start == other.Start && End == other.End;

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is TimeOnlyRange other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(Start, End);

    /// <inheritdoc />
    public override string ToString() => $"{Start:O} - {End:O}";

    /// <summary>
    /// Determines whether two <see cref="TimeOnlyRange"/> instances are equal.
    /// </summary>
    public static bool operator ==(TimeOnlyRange left, TimeOnlyRange right) => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="TimeOnlyRange"/> instances are not equal.
    /// </summary>
    public static bool operator !=(TimeOnlyRange left, TimeOnlyRange right) => !left.Equals(right);
}
#endif
