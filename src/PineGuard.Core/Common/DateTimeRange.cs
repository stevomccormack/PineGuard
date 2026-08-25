using PineGuard.Utils;

namespace PineGuard.Common;

/// <summary>
/// Represents an immutable, inclusive date/time range defined by a start and end <see cref="DateTime"/>.
/// </summary>
/// <remarks>
/// <see cref="Contains(DateTime)"/>, <see cref="Overlaps(DateTimeRange)"/>, <see cref="Overlaps(DateTimeRange, Inclusion)"/>,
/// equality (<see cref="Equals(DateTimeRange)"/> and <see cref="GetHashCode"/>), and the ordering invariant enforced by the
/// constructor and <see cref="TryCreate"/> all normalize both operands to UTC via <see cref="DateTimeUtility.ToUtc(DateTime?)"/>
/// before comparing, so <see cref="DateTimeKind.Utc"/> and <see cref="DateTimeKind.Local"/> values are compared by absolute
/// instant rather than by raw ticks. <see cref="DateTimeKind.Unspecified"/> values are treated as UTC, matching the
/// convention used throughout <c>PineGuard.Core</c>.
/// </remarks>
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
        if (start.Kind != end.Kind &&
            start.Kind != DateTimeKind.Unspecified &&
            end.Kind != DateTimeKind.Unspecified)
            throw new ArgumentException(
                $"DateTime values must have compatible Kind. Start.Kind={start.Kind}, End.Kind={end.Kind}.",
                nameof(start));

        // Validate ordering on the same normalized (UTC) basis that Contains/Overlaps/Equals use, so a range
        // whose raw endpoints look ordered can never be constructed with an inverted normalized instant range
        // (e.g. an Unspecified start paired with a Local end that converts to an earlier UTC instant).
        if (DateTimeUtility.ToUtc(start)!.Value > DateTimeUtility.ToUtc(end)!.Value)
            throw new ArgumentException("Start must be less than or equal to End.", nameof(start));

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

        if (s.Kind != e.Kind && s.Kind != DateTimeKind.Unspecified && e.Kind != DateTimeKind.Unspecified)
            return false;

        // Mirror the constructor: order is validated on the normalized (UTC) endpoints, not raw ticks.
        if (DateTimeUtility.ToUtc(s)!.Value > DateTimeUtility.ToUtc(e)!.Value)
            return false;

        range = new DateTimeRange(s, e);
        return true;
    }

    /// <summary>
    /// Gets the duration of the range as a <see cref="TimeSpan"/>.
    /// </summary>
    public TimeSpan Duration => End - Start;

    /// <summary>
    /// Gets <see cref="Start"/> normalized to UTC (see the type-level remarks).
    /// </summary>
    private DateTime StartUtc => DateTimeUtility.ToUtc(Start)!.Value;

    /// <summary>
    /// Gets <see cref="End"/> normalized to UTC (see the type-level remarks).
    /// </summary>
    private DateTime EndUtc => DateTimeUtility.ToUtc(End)!.Value;

    /// <summary>
    /// Determines whether the specified date/time falls within this range (inclusive).
    /// </summary>
    /// <param name="value">The date/time to check.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is within the range; otherwise, <see langword="false"/>.</returns>
    public bool Contains(DateTime value)
    {
        var valueUtc = DateTimeUtility.ToUtc(value)!.Value;
        return valueUtc >= StartUtc && valueUtc <= EndUtc;
    }

    /// <summary>
    /// Determines whether this range overlaps with another range (exclusive boundaries).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateTimeRange other)
        => StartUtc < other.EndUtc && other.StartUtc < EndUtc;

    /// <summary>
    /// Determines whether this range overlaps with another range using the specified boundary inclusion.
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <param name="inclusion">Whether the boundaries are inclusive or exclusive.</param>
    /// <returns><see langword="true"/> if the ranges overlap; otherwise, <see langword="false"/>.</returns>
    public bool Overlaps(DateTimeRange other, Inclusion inclusion)
    {
        if (inclusion == Inclusion.Exclusive)
            return StartUtc < other.EndUtc && other.StartUtc < EndUtc;

        return StartUtc <= other.EndUtc && other.StartUtc <= EndUtc;
    }

    /// <summary>
    /// Determines whether this range is adjacent to another range (touching at a single point).
    /// </summary>
    /// <param name="other">The other range to compare.</param>
    /// <returns><see langword="true"/> if the ranges are adjacent; otherwise, <see langword="false"/>.</returns>
    public bool IsAdjacentTo(DateTimeRange other)
        => StartUtc == other.EndUtc || EndUtc == other.StartUtc;

    /// <summary>
    /// Computes the intersection of this range with another range.
    /// </summary>
    /// <param name="other">The other range to intersect with.</param>
    /// <returns>
    /// The intersecting <see cref="DateTimeRange"/>, or <see langword="null"/> if the ranges do not overlap.
    /// Because this type represents an inclusive range, two ranges that only touch at a single instant
    /// (e.g. one ends at the instant the other starts) produce a zero-length intersection rather than <see langword="null"/>.
    /// </returns>
    public DateTimeRange? Intersect(DateTimeRange other)
    {
        if (!Overlaps(other, Inclusion.Inclusive))
            return null;

        var start = StartUtc > other.StartUtc ? Start : other.Start;
        var end = EndUtc < other.EndUtc ? End : other.End;

        return CreateNormalized(start, end, this, other);
    }

    /// <summary>
    /// Computes the union of this range with another range (smallest range encompassing both).
    /// </summary>
    /// <param name="other">The other range to union with.</param>
    /// <returns>A <see cref="DateTimeRange"/> encompassing both ranges.</returns>
    public DateTimeRange Union(DateTimeRange other)
    {
        var start = StartUtc < other.StartUtc ? Start : other.Start;
        var end = EndUtc > other.EndUtc ? End : other.End;

        return CreateNormalized(start, end, this, other);
    }

    /// <summary>
    /// Creates a range from endpoints selected across two operands. The result preserves the operands'
    /// <see cref="DateTimeKind"/> only when all four endpoints share it; otherwise both endpoints are normalized
    /// to UTC. Keying the decision on the operands rather than on the selected endpoints keeps the resulting
    /// <see cref="DateTimeKind"/> a function of the inputs' kinds rather than of their values, and guarantees the
    /// endpoints stay ordered and Kind-compatible for the constructor.
    /// </summary>
    private static DateTimeRange CreateNormalized(DateTime start, DateTime end, DateTimeRange first, DateTimeRange second)
    {
        var kind = first.Start.Kind;

        if (first.End.Kind == kind && second.Start.Kind == kind && second.End.Kind == kind)
            return new DateTimeRange(start, end);

        return new DateTimeRange(DateTimeUtility.ToUtc(start)!.Value, DateTimeUtility.ToUtc(end)!.Value);
    }

    /// <inheritdoc />
    public bool Equals(DateTimeRange other)
        => StartUtc == other.StartUtc && EndUtc == other.EndUtc;

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is DateTimeRange other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
        => HashCode.Combine(StartUtc, EndUtc);

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
