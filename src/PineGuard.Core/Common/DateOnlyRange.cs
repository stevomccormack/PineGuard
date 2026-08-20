#if NET8_0_OR_GREATER
namespace PineGuard.Common;

/// <summary>
/// Represents an immutable, inclusive date range defined by a start and end <see cref="DateOnly"/>.
/// </summary>
public readonly struct DateOnlyRange : IEquatable<DateOnlyRange>
{
    /// <summary>
    /// Gets the start date of the range.
    /// </summary>
    public DateOnly Start { get; }

    /// <summary>
    /// Gets the end date of the range.
    /// </summary>
    public DateOnly End { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOnlyRange"/> struct.
    /// </summary>
    /// <param name="start">The start date. Must be less than or equal to <paramref name="end"/>.</param>
    /// <param name="end">The end date.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="start"/> is greater than <paramref name="end"/>.</exception>
    public DateOnlyRange(DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new ArgumentException("Start must be less than or equal to End.", nameof(start));

        Start = start;
        End = end;
    }

    /// <summary>
    /// Attempts to create a <see cref="DateOnlyRange"/> from nullable start and end dates.
    /// </summary>
    /// <param name="start">The start date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="end">The end date. If <see langword="null"/>, returns <see langword="false"/>.</param>
    /// <param name="range">When this method returns, contains the created range if successful; otherwise, the default value.</param>
    /// <returns><see langword="true"/> if the range was created successfully; otherwise, <see langword="false"/>.</returns>
    public static bool TryCreate(DateOnly? start, DateOnly? end, out DateOnlyRange range)
    {
        range = default;

        if (start is null || end is null)
            return false;

        var s = start.Value;
        var e = end.Value;

        if (s > e)
            return false;

        range = new DateOnlyRange(s, e);
        return true;
    }

    /// <summary>
    /// Gets the number of days in the range (inclusive).
    /// </summary>
    public int DayCount => End.DayNumber - Start.DayNumber + 1;

    /// <summary>
    /// Gets the duration of the range as a <see cref="TimeSpan"/>.
    /// </summary>
    public TimeSpan Duration => TimeSpan.FromDays(DayCount);

    /// <summary>
    /// Determines whether the specified date falls within this range (inclusive).
    /// </summary>
    /// <param name="value">The date to check.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(DateOnly value) => value >= Start && value <= End;

    /// <summary>
    /// Determines whether this range overlaps with another range (exclusive boundaries).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateOnlyRange other)
        => Start < other.End && other.Start < End;

    /// <summary>
    /// Determines whether this range overlaps with another range using the specified boundary inclusion.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateOnlyRange other, Inclusion inclusion)
    {
        if (inclusion == Inclusion.Exclusive)
            return Start < other.End && other.Start < End;

        return Start <= other.End && other.Start <= End;
    }

    /// <summary>
    /// Determines whether this range is adjacent to another range (no gap, no overlap).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges are adjacent; otherwise, <see langword="false"/>.</returns>
    public bool IsAdjacentTo(DateOnlyRange other)
        => Start.DayNumber == other.End.DayNumber + 1 || End.DayNumber == other.Start.DayNumber - 1;

    /// <summary>
    /// Computes the intersection of this range with another range.
    /// </summary>
    /// <param name="other">The other range to intersect with.</param>
    /// <returns>
    /// The intersecting <see cref="DateOnlyRange"/>, or <see langword="null"/> if the ranges do not overlap.
    /// Because this type represents an inclusive range, two ranges that only touch at a single day
    /// (e.g. one ends on the day the other starts) produce a single-day intersection rather than <see langword="null"/>.
    /// </returns>
    public DateOnlyRange? Intersect(DateOnlyRange other)
    {
        if (!Overlaps(other, Inclusion.Inclusive))
            return null;

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;

        return new DateOnlyRange(start, end);
    }

    /// <summary>
    /// Computes the union of this range with another range (smallest range encompassing both).
    /// </summary>
    /// <param name="other">The other range to union with.</param>
    /// <returns>A <see cref="DateOnlyRange"/> encompassing both ranges.</returns>
    public DateOnlyRange Union(DateOnlyRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateOnlyRange(start, end);
    }

    /// <inheritdoc />
    public bool Equals(DateOnlyRange other)
        => Start == other.Start && End == other.End;

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is DateOnlyRange other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(Start, End);

    /// <inheritdoc />
    public override string ToString()
        => $"{Start:O} - {End:O}";

    /// <summary>
    /// Determines whether two <see cref="DateOnlyRange"/> instances are equal.
    /// </summary>
    public static bool operator ==(DateOnlyRange left, DateOnlyRange right)
        => left.Equals(right);

    /// <summary>
    /// Determines whether two <see cref="DateOnlyRange"/> instances are not equal.
    /// </summary>
    public static bool operator !=(DateOnlyRange left, DateOnlyRange right)
        => !left.Equals(right);
}
#endif
