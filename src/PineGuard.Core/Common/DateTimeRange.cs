namespace PineGuard.Common;

public readonly struct DateTimeRange : IEquatable<DateTimeRange>
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public DateTimeRange(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("Start must be less than or equal to End.", nameof(start));

        if (start.Kind != end.Kind &&
            start.Kind != DateTimeKind.Unspecified &&
            end.Kind != DateTimeKind.Unspecified)
            throw new ArgumentException(
                $"DateTime values must have compatible Kind. Start.Kind={start.Kind}, End.Kind={end.Kind}.",
                nameof(start));

        Start = start;
        End = end;
    }

    public TimeSpan Duration => End - Start;

    public bool Contains(DateTime value) => value >= Start && value <= End;

    public bool Overlaps(DateTimeRange other)
        => Start < other.End && other.Start < End;

    public bool Overlaps(DateTimeRange other, RangeInclusion inclusion)
    {
        if (inclusion == RangeInclusion.Exclusive)
            return Start < other.End && other.Start < End;

        return Start <= other.End && other.Start <= End;
    }

    public bool IsAdjacentTo(DateTimeRange other)
        => Start == other.End || End == other.Start;

    public DateTimeRange? Intersect(DateTimeRange other)
    {
        if (!Overlaps(other))
            return null;

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;

        return new DateTimeRange(start, end);
    }

    public DateTimeRange Union(DateTimeRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateTimeRange(start, end);
    }

    public bool Equals(DateTimeRange other)
        => Start == other.Start && End == other.End;

    public override bool Equals(object? obj)
        => obj is DateTimeRange other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Start, End);

    public override string ToString()
        => $"{Start:O} - {End:O}";

    public static bool operator ==(DateTimeRange left, DateTimeRange right)
        => left.Equals(right);

    public static bool operator !=(DateTimeRange left, DateTimeRange right)
        => !left.Equals(right);
}
