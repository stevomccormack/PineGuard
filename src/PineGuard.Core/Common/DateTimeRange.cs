using PineGuard.Extensions;

namespace PineGuard.Common;

/// <summary>
/// Represents an immutable, inclusive date/time range defined by a start and end <see cref="DateTime"/>.
/// </summary>
public readonly struct DateTimeRange : IEquatable<DateTimeRange>
{
    /// <summary>
    /// Gets the start date/time of the range.
    /// </summary>
    public DateTime Start { get; }

    /// <summary>
    /// Gets the end date/time of the range.
    /// </summary>
    public DateTime End { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeRange"/> struct.
    /// </summary>
    /// <param name="start">The start date/time. Must be less than or equal to <paramref name="end"/>.</param>
    /// <param name="end">The end date/time.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="start"/> is greater than <paramref name="end"/> or when the <see cref="DateTimeKind"/> values are incompatible.</exception>
    public DateTimeRange(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException($"{nameof(start).TitleCase()} must be less than or equal to {nameof(end).TitleCase()}.", nameof(start));

        if (start.Kind != end.Kind &&
            start.Kind != DateTimeKind.Unspecified &&
            end.Kind != DateTimeKind.Unspecified)
            throw new ArgumentException(
                $"DateTime values must have compatible Kind. {nameof(start).TitleCase()}.Kind={start.Kind}, {nameof(end).TitleCase()}.Kind={end.Kind}.",
                nameof(start));

        Start = start;
        End = end;
    }

    /// <summary>
    /// Attempts to create a <see cref="DateTimeRange"/> from nullable start and end date/times.
    /// </summary>
    /// <param name="start">The start date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end date/time. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="range">When this method returns, contains the created range if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the range was created successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreate(DateTime? start, DateTime? end, out DateTimeRange range)
    {
        range = default;

        if (start is null || end is null)
            return false;

        var s = start.Value;
        var e = end.Value;

        if (s > e)
            return false;

        if (s.Kind != e.Kind && s.Kind != DateTimeKind.Unspecified && e.Kind != DateTimeKind.Unspecified)
            return false;

        range = new DateTimeRange(s, e);
        return true;
    }

    /// <summary>
    /// Gets the duration of the range as a <see cref="TimeSpan"/>.
    /// </summary>
    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Determines whether the specified date/time falls within this range (inclusive).
    /// </summary>
    /// <param name="value">The date/time to check.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(DateTime value) => value >= Start && value <= End;

    /// <summary>
    /// Determines whether this range overlaps with another range (exclusive boundaries).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateTimeRange other)
        => Start < other.End && other.Start < End;

    /// <summary>
    /// Determines whether this range overlaps with another range using the specified boundary inclusion.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateTimeRange other, Inclusion inclusion)
    {
        if (inclusion == Inclusion.Exclusive)
            return Start < other.End && other.Start < End;

        return Start <= other.End && other.Start <= End;
    }

    /// <summary>
    /// Determines whether this range is adjacent to another range (touching at a single point).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges are adjacent; otherwise, <see langword="false"/>.</returns>
    public bool IsAdjacentTo(DateTimeRange other)
        => Start == other.End || End == other.Start;

    /// <summary>
    /// Computes the intersection of this range with another range.
    /// </summary>
    /// <param name="other">The other range to intersect with.</param>
    /// <returns>The intersecting <see cref="DateTimeRange"/>, or <see langword="null"/> if the ranges do not overlap.</returns>
    public DateTimeRange? Intersect(DateTimeRange other)
    {
        if (!Overlaps(other))
            return null;

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;

        return new DateTimeRange(start, end);
    }

    /// <summary>
    /// Computes the union of this range with another range (smallest range encompassing both).
    /// </summary>
    /// <param name="other">The other range to union with.</param>
    /// <returns>A <see cref="DateTimeRange"/> encompassing both ranges.</returns>
    public DateTimeRange Union(DateTimeRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateTimeRange(start, end);
    }

    /// <inheritdoc />
    public bool Equals(DateTimeRange other)
        => Start == other.Start && End == other.End;

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is DateTimeRange other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(Start, End);

    /// <inheritdoc />
    public override string ToString()
        => $"{Start:O} - {End:O}";

    /// <summary>
    /// Determines whether two <see cref="DateTimeRange"/> instances are equal.
    /// </summary>
    public static bool operator ==(DateTimeRange left, DateTimeRange right)
        => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="DateTimeRange"/> instances are not equal.
    /// </summary>
    public static bool operator !=(DateTimeRange left, DateTimeRange right)
        => !left.Equals(right);
}
