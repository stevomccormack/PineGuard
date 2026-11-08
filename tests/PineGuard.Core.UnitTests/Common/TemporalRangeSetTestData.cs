using PineGuard.Common;
using PineGuard.Testing.UnitTests;
using F = PineGuard.Testing.Fixtures.TemporalRangeSetFixtures;

namespace PineGuard.Core.UnitTests.Common;

public static class TemporalRangeSetTestData
{
    public static class DateOnlyOperations
    {
        public static TheoryData<SetCase<DateOnlyRange>> Cases => Create(F.DateOnlyOffsets,
            (start, end) => new DateOnlyRange(DateOnly.FromDayNumber((int)start), DateOnly.FromDayNumber((int)end)));
    }

    public static class DateTimeOperations
    {
        public static TheoryData<SetCase<DateTimeRange>> Cases => Create(F.DateTimeOffsets,
            (start, end) => new DateTimeRange(new DateTime(start, DateTimeKind.Utc), new DateTime(end, DateTimeKind.Utc)));
    }

    public static class DateTimeOffsetOperations
    {
        public static TheoryData<SetCase<DateTimeOffsetRange>> Cases => Create(F.DateTimeOffsets,
            (start, end) => new DateTimeOffsetRange(new DateTimeOffset(start, TimeSpan.Zero), new DateTimeOffset(end, TimeSpan.Zero)));
    }

    public static class TimeOnlyOperations
    {
        public static TheoryData<SetCase<TimeOnlyRange>> Cases => Create(F.TimeOnlyOffsets,
            (start, end) => new TimeOnlyRange(new TimeOnly(start), new TimeOnly(end)));
    }

    public sealed record SetCase<T>(string Name, (T left, T right) Value, long[] Intersection, long[] Union,
        long[] Enclosure, bool OpenOverlap, bool Adjacent)
        : ValueCase<(T left, T right)>(Name, Value);

    private static TheoryData<SetCase<T>> Create<T>(long[] offsets, Func<long, long, T> create)
    {
        var data = new TheoryData<SetCase<T>>();
        foreach (var offset in offsets)
            foreach (var start in F.Coordinates)
                foreach (var end in F.Coordinates.Where(coordinate => coordinate >= start))
                    foreach (var otherStart in F.Coordinates)
                        foreach (var otherEnd in F.Coordinates.Where(coordinate => coordinate >= otherStart))
                        {
                            var left = F.Coordinates.Where(coordinate => coordinate >= start && coordinate <= end).Select(coordinate => coordinate + offset).ToArray();
                            var right = F.Coordinates.Where(coordinate => coordinate >= otherStart && coordinate <= otherEnd).Select(coordinate => coordinate + offset).ToArray();
                            var intersection = left.Intersect(right).ToArray();
                            var union = left.Union(right).OrderBy(coordinate => coordinate).ToArray();
                            var enclosure = F.Coordinates.Select(coordinate => coordinate + offset).Where(coordinate => coordinate >= union[0] && coordinate <= union[^1]).ToArray();
                            var leftInterior = left.Skip(1).Take(Math.Max(0, left.Length - 2));
                            var rightInterior = right.Skip(1).Take(Math.Max(0, right.Length - 2));
                            data.Add(new SetCase<T>($"{offset}:{start}-{end}/{otherStart}-{otherEnd}",
                                (create(left[0], left[^1]), create(right[0], right[^1])), intersection, union, enclosure,
                                leftInterior.Intersect(rightInterior).Any(), intersection.Length == 0 && union.SequenceEqual(enclosure)));
                        }

        return data;
    }
}
