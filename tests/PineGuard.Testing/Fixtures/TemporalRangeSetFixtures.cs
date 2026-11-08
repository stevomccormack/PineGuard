namespace PineGuard.Testing.Fixtures;

/// <summary>
/// Finite temporal domains for independent, enumerated set-law expectations.
/// </summary>
public static class TemporalRangeSetFixtures
{
    public static readonly int[] Coordinates = [0, 1, 2, 3, 4];
    public static readonly long[] DateOnlyOffsets = [DateOnly.MinValue.DayNumber, DateOnly.MaxValue.DayNumber - Coordinates.Length + 1];
    public static readonly long[] DateTimeOffsets = [DateTime.MinValue.Ticks, DateTime.MaxValue.Ticks - Coordinates.Length + 1];
    public static readonly long[] TimeOnlyOffsets = [TimeOnly.MinValue.Ticks, TimeOnly.MaxValue.Ticks - Coordinates.Length + 1];
}
