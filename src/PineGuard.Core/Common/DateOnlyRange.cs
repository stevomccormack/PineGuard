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
    /// Gets the calendar-day distance between the endpoints as a <see cref="TimeSpan"/>.
    /// </summary>
    public TimeSpan Duration => TimeSpan.FromDays(End.DayNumber - Start.DayNumber);

    /// <summary>
    /// Determines whether the specified date falls within this range (inclusive).
    /// </summary>
    /// <param name="value">The date to check.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(DateOnly value) => value >= Start && value <= End;

    /// <summary>
    /// Determines whether this range shares at least one included value with another closed range.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateOnlyRange other) => Overlaps(other, Inclusion.Inclusive);

    /// <summary>
    /// Determines whether this range overlaps with another range using the specified boundary inclusion.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <param name="inclusion">Whether to intersect the closed ranges or their open interiors. Open interiors exclude both endpoints; singleton and one-step ranges have empty interiors.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateOnlyRange other, Inclusion inclusion) =>
        TemporalRangeComparison.Overlaps(Start.DayNumber, End.DayNumber, other.Start.DayNumber, other.End.DayNumber, inclusion);

    /// <summary>
    /// Determines whether this range is adjacent to another range (no gap, no overlap).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges are adjacent; otherwise, <see langword="false"/>.</returns>
    public bool IsAdjacentTo(DateOnlyRange other) =>
        TemporalRangeComparison.IsAdjacent(Start.DayNumber, End.DayNumber, other.Start.DayNumber, other.End.DayNumber);

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
    /// Computes the smallest closed range enclosing both operands, including any gap.
    /// </summary>
    /// <param name="other">The other range to enclose.</param>
    /// <returns>A <see cref="DateOnlyRange"/> encompassing both ranges.</returns>
    public DateOnlyRange Enclose(DateOnlyRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateOnlyRange(start, end);
    }

    /// <summary>
    /// Computes the exact union as an ordered, read-only collection of closed ranges.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns>One range when the operands overlap or are adjacent; otherwise two disjoint ranges in ascending order.</returns>
    public IReadOnlyList<DateOnlyRange> Union(DateOnlyRange other)
    {
        if (Overlaps(other) || IsAdjacentTo(other))
            return Array.AsReadOnly(new[] { Enclose(other) });

        return Start < other.Start
            ? Array.AsReadOnly(new[] { this, other })
            : Array.AsReadOnly(new[] { other, this });
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
