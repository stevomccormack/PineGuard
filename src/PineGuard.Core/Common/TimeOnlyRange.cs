namespace PineGuard.Common;

public readonly struct TimeOnlyRange : IEquatable<TimeOnlyRange>
{
    public TimeOnly Start { get; }
    public TimeOnly End { get; }

    public TimeOnlyRange(TimeOnly start, TimeOnly end)
    {
        if (start > end)
            throw new ArgumentException("Start must be less than or equal to End.", nameof(start));

        Start = start;
        End = end;
    }

    public TimeSpan Duration => End.ToTimeSpan() - Start.ToTimeSpan();

    public bool Contains(TimeOnly value) => value >= Start && value <= End;

    public bool Overlaps(TimeOnlyRange other) => Start < other.End && other.Start < End;

    public bool Overlaps(TimeOnlyRange other, RangeInclusion inclusion)
    {
        if (inclusion == RangeInclusion.Exclusive)
            return Start < other.End && other.Start < End;

        return Start <= other.End && other.Start <= End;
    }

    public bool Equals(TimeOnlyRange other) => Start == other.Start && End == other.End;

    public override bool Equals(object? obj) => obj is TimeOnlyRange other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Start, End);

    public override string ToString() => $"{Start:O} - {End:O}";

    public static bool operator ==(TimeOnlyRange left, TimeOnlyRange right) => left.Equals(right);

    public static bool operator !=(TimeOnlyRange left, TimeOnlyRange right) => !left.Equals(right);
}
