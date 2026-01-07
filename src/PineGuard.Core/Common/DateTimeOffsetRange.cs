using PineGuard.Extensions;

namespace PineGuard.Common;

public readonly struct DateTimeOffsetRange : IEquatable<DateTimeOffsetRange>
{
    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }

    public DateTimeOffsetRange(DateTimeOffset start, DateTimeOffset end)
    {
        if (start > end)
            throw new ArgumentException($"{nameof(start).TitleCase()} must be less than or equal to {nameof(end).TitleCase()}.", nameof(start));

        Start = start;
        End = end;
    }

    public TimeSpan Duration => End - Start;

    public bool Contains(DateTimeOffset value) => value >= Start && value <= End;

    public bool Overlaps(DateTimeOffsetRange other)
        => Start < other.End && other.Start < End;

    public bool Overlaps(DateTimeOffsetRange other, Inclusion inclusion)
    {
        if (inclusion == Inclusion.Exclusive)
            return Start < other.End && other.Start < End;

        return Start <= other.End && other.Start <= End;
    }

    public bool IsAdjacentTo(DateTimeOffsetRange other)
        => Start == other.End || End == other.Start;

    public DateTimeOffsetRange? Intersect(DateTimeOffsetRange other)
    {
        if (!Overlaps(other))
            return null;

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;

        return new DateTimeOffsetRange(start, end);
    }

    public DateTimeOffsetRange Union(DateTimeOffsetRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateTimeOffsetRange(start, end);
    }

    public bool Equals(DateTimeOffsetRange other)
        => Start == other.Start && End == other.End;

    public override bool Equals(object? obj)
        => obj is DateTimeOffsetRange other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Start, End);

    public override string ToString()
        => $"{Start:O} - {End:O}";

    public static bool operator ==(DateTimeOffsetRange left, DateTimeOffsetRange right)
        => left.Equals(right);

    public static bool operator !=(DateTimeOffsetRange left, DateTimeOffsetRange right)
        => !left.Equals(right);
}
