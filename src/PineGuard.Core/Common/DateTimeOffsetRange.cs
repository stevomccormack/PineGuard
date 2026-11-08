namespace PineGuard.Common;

/// <summary>
/// Represents an immutable, inclusive date/time offset range defined by a start and end <see cref="DateTimeOffset"/>.
/// </summary>
public readonly struct DateTimeOffsetRange : IEquatable<DateTimeOffsetRange>
{
    /// <summary>
    /// Gets the start date/time offset of the range.
    /// </summary>
    public DateTimeOffset Start { get; }

    /// <summary>
    /// Gets the end date/time offset of the range.
    /// </summary>
    public DateTimeOffset End { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateTimeOffsetRange"/> struct.
    /// </summary>
    /// <param name="start">The start date/time offset. Must be less than or equal to <paramref name="end"/>.</param>
    /// <param name="end">The end date/time offset.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="start"/> is greater than <paramref name="end"/>.</exception>
    public DateTimeOffsetRange(DateTimeOffset start, DateTimeOffset end)
    {
        if (start > end)
            throw new ArgumentException("Start must be less than or equal to End.", nameof(start));

        Start = start;
        End = end;
    }

    /// <summary>
    /// Attempts to create a <see cref="DateTimeOffsetRange"/> from nullable start and end values.
    /// </summary>
    /// <param name="start">The start date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end date/time offset. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="range">When this method returns, contains the created range if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the range was created successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreate(DateTimeOffset? start, DateTimeOffset? end, out DateTimeOffsetRange range)
    {
        range = default;

        if (start is null || end is null)
            return false;

        var s = start.Value;
        var e = end.Value;

        if (s > e)
            return false;

        range = new DateTimeOffsetRange(s, e);
        return true;
    }

    /// <summary>
    /// Gets the duration of the range as a <see cref="TimeSpan"/>.
    /// </summary>
    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Determines whether the specified date/time offset falls within this range (inclusive).
    /// </summary>
    /// <param name="value">The date/time offset to check.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(DateTimeOffset value) => value >= Start && value <= End;

    /// <summary>
    /// Determines whether this range shares at least one included value with another closed range.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateTimeOffsetRange other) => Overlaps(other, Inclusion.Inclusive);

    /// <summary>
    /// Determines whether this range overlaps with another range using the specified boundary inclusion.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <param name="inclusion">Whether to intersect the closed ranges or their open interiors. Open interiors exclude both endpoints; singleton and one-step ranges have empty interiors.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateTimeOffsetRange other, Inclusion inclusion) =>
        TemporalRangeComparison.Overlaps(Start.UtcDateTime.Ticks, End.UtcDateTime.Ticks, other.Start.UtcDateTime.Ticks, other.End.UtcDateTime.Ticks, inclusion);

    /// <summary>
    /// Determines whether this range is adjacent to another range (disjoint, with no representable value between them).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges are adjacent; otherwise, <see langword="false"/>.</returns>
    public bool IsAdjacentTo(DateTimeOffsetRange other) =>
        TemporalRangeComparison.IsAdjacent(Start.UtcDateTime.Ticks, End.UtcDateTime.Ticks, other.Start.UtcDateTime.Ticks, other.End.UtcDateTime.Ticks);

    /// <summary>
    /// Computes the intersection of this range with another range.
    /// </summary>
    /// <param name="other">The other range to intersect with.</param>
    /// <returns>
    /// The intersecting <see cref="DateTimeOffsetRange"/>, or <see langword="null"/> if the ranges do not overlap.
    /// Because this type represents an inclusive range, two ranges that only touch at a single instant
    /// (e.g. one ends at the instant the other starts) produce a zero-length intersection rather than <see langword="null"/>.
    /// </returns>
    public DateTimeOffsetRange? Intersect(DateTimeOffsetRange other)
    {
        if (!Overlaps(other, Inclusion.Inclusive))
            return null;

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;

        return new DateTimeOffsetRange(start, end);
    }

    /// <summary>
    /// Computes the smallest closed range enclosing both operands, including any gap.
    /// </summary>
    /// <param name="other">The other range to enclose.</param>
    /// <returns>A <see cref="DateTimeOffsetRange"/> encompassing both ranges.</returns>
    public DateTimeOffsetRange Enclose(DateTimeOffsetRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateTimeOffsetRange(start, end);
    }

    /// <summary>
    /// Computes the exact union as an ordered, read-only collection of closed ranges.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns>One range when the operands overlap or are adjacent; otherwise two disjoint ranges in ascending order.</returns>
    public IReadOnlyList<DateTimeOffsetRange> Union(DateTimeOffsetRange other)
    {
        if (Overlaps(other) || IsAdjacentTo(other))
            return Array.AsReadOnly(new[] { Enclose(other) });

        return Start < other.Start
            ? Array.AsReadOnly(new[] { this, other })
            : Array.AsReadOnly(new[] { other, this });
    }

    /// <inheritdoc />
    public bool Equals(DateTimeOffsetRange other)
        => Start == other.Start && End == other.End;

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is DateTimeOffsetRange other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(Start, End);

    /// <inheritdoc />
    public override string ToString()
        => $"{Start:O} - {End:O}";

    /// <summary>
    /// Determines whether two <see cref="DateTimeOffsetRange"/> instances are equal.
    /// </summary>
    public static bool operator ==(DateTimeOffsetRange left, DateTimeOffsetRange right)
        => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="DateTimeOffsetRange"/> instances are not equal.
    /// </summary>
    public static bool operator !=(DateTimeOffsetRange left, DateTimeOffsetRange right)
        => !left.Equals(right);
}
