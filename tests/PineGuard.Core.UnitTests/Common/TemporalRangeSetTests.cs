using PineGuard.Common;
using PineGuard.Rules;
using PineGuard.Testing.UnitTests.Rules;
using Xunit.Abstractions;

namespace PineGuard.Core.UnitTests.Common;

public sealed class TemporalRangeSetTests(ITestOutputHelper output)
    : BaseRuleUnitTest(output)
{
    [Theory]
    [MemberData(nameof(TemporalRangeSetTestData.DateOnlyOperations.Cases), MemberType = typeof(TemporalRangeSetTestData.DateOnlyOperations))]
    public void DateOnlyOperations_BehaveAsExpected(TemporalRangeSetTestData.SetCase<DateOnlyRange> tc)
    {
        // Act & Assert
        AssertSets(tc, range => (range.Start.DayNumber, range.End.DayNumber),
            (left, right) => left.Overlaps(right), (left, right) => left.Overlaps(right, Inclusion.Exclusive),
            (left, right) => left.IsAdjacentTo(right), (left, right) => left.Intersect(right),
            (left, right) => left.Union(right), (left, right) => left.Enclose(right));
        Assert.Equal(tc.Intersection.Length != 0, DateOnlyRangeRules.IsOverlapping(tc.Value.left, tc.Value.right));
        Assert.Equal(tc.OpenOverlap, DateOnlyRangeRules.IsOverlapping(tc.Value.left, tc.Value.right, Inclusion.Exclusive));
        Assert.Equal(tc.Intersection.Length != 0, DateOnlyRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End));
        Assert.Equal(tc.OpenOverlap, DateOnlyRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End, Inclusion.Exclusive));
    }

    [Theory]
    [MemberData(nameof(TemporalRangeSetTestData.DateTimeOperations.Cases), MemberType = typeof(TemporalRangeSetTestData.DateTimeOperations))]
    public void DateTimeOperations_BehaveAsExpected(TemporalRangeSetTestData.SetCase<DateTimeRange> tc)
    {
        // Act & Assert
        AssertSets(tc, range => (range.Start.Ticks, range.End.Ticks),
            (left, right) => left.Overlaps(right), (left, right) => left.Overlaps(right, Inclusion.Exclusive),
            (left, right) => left.IsAdjacentTo(right), (left, right) => left.Intersect(right),
            (left, right) => left.Union(right), (left, right) => left.Enclose(right));
        Assert.Equal(tc.Intersection.Length != 0, DateTimeRangeRules.IsOverlapping(tc.Value.left, tc.Value.right));
        Assert.Equal(tc.OpenOverlap, DateTimeRangeRules.IsOverlapping(tc.Value.left, tc.Value.right, Inclusion.Exclusive));
        Assert.Equal(tc.Intersection.Length != 0, DateTimeRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End));
        Assert.Equal(tc.OpenOverlap, DateTimeRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End, Inclusion.Exclusive));
    }

    [Theory]
    [MemberData(nameof(TemporalRangeSetTestData.DateTimeOffsetOperations.Cases), MemberType = typeof(TemporalRangeSetTestData.DateTimeOffsetOperations))]
    public void DateTimeOffsetOperations_BehaveAsExpected(TemporalRangeSetTestData.SetCase<DateTimeOffsetRange> tc)
    {
        // Act & Assert
        AssertSets(tc, range => (range.Start.UtcDateTime.Ticks, range.End.UtcDateTime.Ticks),
            (left, right) => left.Overlaps(right), (left, right) => left.Overlaps(right, Inclusion.Exclusive),
            (left, right) => left.IsAdjacentTo(right), (left, right) => left.Intersect(right),
            (left, right) => left.Union(right), (left, right) => left.Enclose(right));
        Assert.Equal(tc.Intersection.Length != 0, DateTimeOffsetRangeRules.IsOverlapping(tc.Value.left, tc.Value.right));
        Assert.Equal(tc.OpenOverlap, DateTimeOffsetRangeRules.IsOverlapping(tc.Value.left, tc.Value.right, Inclusion.Exclusive));
        Assert.Equal(tc.Intersection.Length != 0, DateTimeOffsetRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End));
        Assert.Equal(tc.OpenOverlap, DateTimeOffsetRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End, Inclusion.Exclusive));
    }

    [Theory]
    [MemberData(nameof(TemporalRangeSetTestData.TimeOnlyOperations.Cases), MemberType = typeof(TemporalRangeSetTestData.TimeOnlyOperations))]
    public void TimeOnlyOperations_BehaveAsExpected(TemporalRangeSetTestData.SetCase<TimeOnlyRange> tc)
    {
        // Act & Assert
        AssertSets(tc, range => (range.Start.Ticks, range.End.Ticks),
            (left, right) => left.Overlaps(right), (left, right) => left.Overlaps(right, Inclusion.Exclusive),
            (left, right) => left.IsAdjacentTo(right), (left, right) => left.Intersect(right),
            (left, right) => left.Union(right), (left, right) => left.Enclose(right));
        Assert.Equal(tc.Intersection.Length != 0, TimeOnlyRangeRules.IsOverlapping(tc.Value.left, tc.Value.right));
        Assert.Equal(tc.OpenOverlap, TimeOnlyRangeRules.IsOverlapping(tc.Value.left, tc.Value.right, Inclusion.Exclusive));
        Assert.Equal(tc.Intersection.Length != 0, TimeOnlyRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End));
        Assert.Equal(tc.OpenOverlap, TimeOnlyRules.IsOverlapping(tc.Value.left.Start, tc.Value.left.End, tc.Value.right.Start, tc.Value.right.End, Inclusion.Exclusive));
    }

    private static void AssertSets<T>(TemporalRangeSetTestData.SetCase<T> tc, Func<T, (long start, long end)> bounds,
        Func<T, T, bool> overlaps, Func<T, T, bool> openOverlap, Func<T, T, bool> adjacent,
        Func<T, T, T?> intersect, Func<T, T, IReadOnlyList<T>> union, Func<T, T, T> enclose) where T : struct
    {
        // Arrange
        var (left, right) = tc.Value;

        // Act
        var intersection = intersect(left, right);
        var united = union(left, right);
        var enclosure = enclose(left, right);

        // Assert
        Assert.Equal(tc.Intersection.Length != 0, overlaps(left, right));
        Assert.Equal(tc.OpenOverlap, openOverlap(left, right));
        Assert.Equal(tc.Adjacent, adjacent(left, right));
        Assert.Equal(tc.Intersection, intersection.HasValue ? Expand(intersection.Value, bounds) : []);
        Assert.Equal(tc.Union, united.SelectMany(range => Expand(range, bounds)).ToArray());
        Assert.Equal(tc.Enclosure, Expand(enclosure, bounds));
        Assert.Equal(intersection, intersect(right, left));
        Assert.Equal(united, union(right, left));
        Assert.Equal(enclosure, enclose(right, left));
        Assert.Equal(overlaps(left, right), overlaps(right, left));
        Assert.Equal(openOverlap(left, right), openOverlap(right, left));
        Assert.Equal(adjacent(left, right), adjacent(right, left));
        Assert.InRange(united.Count, 1, 2);
        if (united.Count == 2)
        {
            Assert.False(overlaps(united[0], united[1]));
            Assert.False(adjacent(united[0], united[1]));
            Assert.True(bounds(united[0]).end < bounds(united[1]).start);
        }

        var list = Assert.IsAssignableFrom<IList<T>>(united);
        Assert.True(list.IsReadOnly);
        Assert.Throws<NotSupportedException>(() => list[0] = right);
    }

    private static long[] Expand<T>(T range, Func<T, (long start, long end)> bounds)
    {
        var (start, end) = bounds(range);
        return Enumerable.Range(0, checked((int)(end - start + 1))).Select(index => start + index).ToArray();
    }
}
