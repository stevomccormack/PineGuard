using PineGuard.Extensions;

namespace PineGuard.Common;

public readonly struct DateOnlyRange : IEquatable<DateOnlyRange>
{
    public DateOnly Start { get; }
    public DateOnly End { get; }

    public DateOnlyRange(DateOnly start, DateOnly end)
    {
        if (start > end)
            throw new ArgumentException($"{nameof(start).TitleCase()} must be less than or equal to End.", nameof(start));

        Start = start;
        End = end;
    }

    public int DayCount => End.DayNumber - Start.DayNumber + 1;

    public TimeSpan Duration => TimeSpan.FromDays(DayCount);

    public bool Contains(DateOnly value) => value >= Start && value <= End;

    public bool Overlaps(DateOnlyRange other)
        => Start < other.End && other.Start < End;

    public bool Overlaps(DateOnlyRange other, Inclusion inclusion)
    {
        if (inclusion == Inclusion.Exclusive)
            return Start < other.End && other.Start < End;

        return Start <= other.End && other.Start <= End;
    }

    public bool IsAdjacentTo(DateOnlyRange other)
        => Start.DayNumber == other.End.DayNumber + 1 || End.DayNumber == other.Start.DayNumber - 1;

    public DateOnlyRange? Intersect(DateOnlyRange other)
    {
        if (!Overlaps(other))
            return null;

        var start = Start > other.Start ? Start : other.Start;
        var end = End < other.End ? End : other.End;

        return new DateOnlyRange(start, end);
    }

    public DateOnlyRange Union(DateOnlyRange other)
    {
        var start = Start < other.Start ? Start : other.Start;
        var end = End > other.End ? End : other.End;

        return new DateOnlyRange(start, end);
    }

    public bool Equals(DateOnlyRange other)
        => Start == other.Start && End == other.End;

    public override bool Equals(object? obj)
        => obj is DateOnlyRange other && Equals(other);

    public override int GetHashCode()
        => HashCode.Combine(Start, End);

    public override string ToString()
        => $"{Start:O} - {End:O}";

    public static bool operator ==(DateOnlyRange left, DateOnlyRange right)
        => left.Equals(right);

    public static bool operator !=(DateOnlyRange left, DateOnlyRange right)
        => !left.Equals(right);
}
