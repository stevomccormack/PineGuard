#if NET8_0_OR_GREATER
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
            throw new ArgumentException("Start must be less than or equal to End.", nameof(start));

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
    /// Determines whether this range shares at least one included value with another closed range.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(TimeOnlyRange other) => Overlaps(other, Inclusion.Inclusive);

    /// <summary>
    /// Determines whether this range overlaps with another range using the specified boundary inclusion.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <param name="inclusion">Whether to intersect the closed ranges or their open interiors. Open interiors exclude both endpoints; singleton and one-step ranges have empty interiors.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(TimeOnlyRange other, Inclusion inclusion) =>
        TemporalRangeComparison.Overlaps(Start.Ticks, End.Ticks, other.Start.Ticks, other.End.Ticks, inclusion);

    /// <summary>
    /// Determines whether the ranges are disjoint with no representable tick between them.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns>Whether the closed ranges are adjacent.</returns>
    public bool IsAdjacentTo(TimeOnlyRange other) =>
        TemporalRangeComparison.IsAdjacent(Start.Ticks, End.Ticks, other.Start.Ticks, other.End.Ticks);

    /// <summary>
    /// Computes the closed intersection; shared endpoints produce a singleton.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns>The intersection, or null for disjoint ranges.</returns>
    public TimeOnlyRange? Intersect(TimeOnlyRange other)
    {
        if (!Overlaps(other))
            return null;

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;
        return new TimeOnlyRange(start, end);
    }

    /// <summary>
    /// Computes the smallest closed linear range enclosing both operands, including any gap.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns>The enclosing range. This operation never wraps across midnight.</returns>
    public TimeOnlyRange Enclose(TimeOnlyRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;
        return new TimeOnlyRange(start, end);
    }

    /// <summary>
    /// Computes the exact union as an ordered, read-only collection of closed ranges.
    /// </summary>
    /// <param name="other">The other range.</param>
    /// <returns>One range when the operands overlap or are adjacent; otherwise two disjoint ranges in ascending order.</returns>
    public IReadOnlyList<TimeOnlyRange> Union(TimeOnlyRange other)
    {
        if (Overlaps(other) || IsAdjacentTo(other))
            return Array.AsReadOnly(new[] { Enclose(other) });

        return Start < other.Start
            ? Array.AsReadOnly(new[] { this, other })
            : Array.AsReadOnly(new[] { other, this });
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
