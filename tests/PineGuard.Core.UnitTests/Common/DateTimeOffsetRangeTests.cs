using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class DateTimeOffsetRangeTests : BaseUnitTest
{
    [Fact]
    public void Equals_ReturnsFalse_WhenDifferent()
    {
        var start = new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
        var range = new DateTimeOffsetRange(start, start.AddDays(10));
        var endDifferent = new DateTimeOffsetRange(start, start.AddDays(11));
        var startDifferent = new DateTimeOffsetRange(start.AddDays(-1), start.AddDays(10));

        Assert.False(range.Equals(endDifferent));
        Assert.False(range.Equals(startDifferent));
    }

    [Fact]
    public void Intersect_AndUnion_CoverBothTernaryBranches()
    {
        var start = new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero);
        var range = new DateTimeOffsetRange(start, start.AddDays(10));

        var otherStartsBefore = new DateTimeOffsetRange(start.AddDays(-9), start.AddDays(5));
        var otherStartsAfter = new DateTimeOffsetRange(start.AddDays(5), start.AddDays(20));

        var intersect1 = range.Intersect(otherStartsBefore);
        Assert.NotNull(intersect1);
        Assert.Equal(start, intersect1.Value.Start);
        Assert.Equal(start.AddDays(5), intersect1.Value.End);

        var intersect2 = range.Intersect(otherStartsAfter);
        Assert.NotNull(intersect2);
        Assert.Equal(start.AddDays(5), intersect2.Value.Start);
        Assert.Equal(start.AddDays(10), intersect2.Value.End);

        var union1 = range.Union(otherStartsBefore);
        Assert.Equal(start.AddDays(-9), union1.Start);
        Assert.Equal(start.AddDays(10), union1.End);

        var union2 = range.Union(otherStartsAfter);
        Assert.Equal(start, union2.Start);
        Assert.Equal(start.AddDays(20), union2.End);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Ctor_SetsStartEnd_AndDuration(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Act
        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.Start, range.Start);
        Assert.Equal(testCase.End, range.End);
        Assert.Equal(testCase.ExpectedDuration, range.Duration);
        Assert.True(range.Contains(testCase.Start));
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.EdgeCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Ctor_EdgeCases_SetsStartEnd_AndDuration(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Act
        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.ExpectedDuration, range.Duration);
        Assert.True(range.Contains(testCase.End));
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.InvalidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Ctor_WhenStartAfterEnd_Throws(DateTimeOffsetRangeTestData.Constructor.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = new DateTimeOffsetRange(invalidCase.Start, invalidCase.End));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Contains_IncludesBounds_AndExcludesOutside(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);

        // Act
        var containsStart = range.Contains(testCase.Start);
        var containsEnd = range.Contains(testCase.End);

        var containsBefore = testCase.Start != DateTimeOffset.MinValue && range.Contains(testCase.Start.AddTicks(-1));
        var containsAfter = testCase.End != DateTimeOffset.MaxValue && range.Contains(testCase.End.AddTicks(1));

        // Assert
        Assert.True(containsStart);
        Assert.True(containsEnd);

        if (testCase.Start != DateTimeOffset.MinValue)
        {
            Assert.False(containsBefore);
        }

        if (testCase.End != DateTimeOffset.MaxValue)
        {
            Assert.False(containsAfter);
        }
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Overlaps_ExclusiveAndInclusive_BehaveAsExpected(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);
        var same = new DateTimeOffsetRange(testCase.Start, testCase.End);

        // Act
        var overlapsExclusive = range.Overlaps(same);
        var overlapsInclusive = range.Overlaps(same, Inclusion.Inclusive);

        // Assert
        Assert.Equal(testCase.Start < testCase.End, overlapsExclusive);
        Assert.True(overlapsInclusive);
        Assert.Equal(range.Overlaps(same, Inclusion.Exclusive), overlapsExclusive);
    }

    [Fact]
    public void Overlaps_ReturnsFalse_WhenRangesDoNotOverlap_OnEitherSide()
    {
        var range = new DateTimeOffsetRange(
            new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 01, 20, 0, 0, 0, TimeSpan.Zero));

        var beforeTouching = new DateTimeOffsetRange(
            new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 01, 10, 0, 0, 0, TimeSpan.Zero));

        var afterTouching = new DateTimeOffsetRange(
            new DateTimeOffset(2024, 01, 20, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 01, 30, 0, 0, 0, TimeSpan.Zero));

        Assert.False(range.Overlaps(beforeTouching));
        Assert.False(range.Overlaps(afterTouching));

        Assert.True(range.Overlaps(beforeTouching, Inclusion.Inclusive));
        Assert.True(range.Overlaps(afterTouching, Inclusion.Inclusive));

        var strictlyBefore = new DateTimeOffsetRange(
            new DateTimeOffset(2024, 01, 01, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 01, 09, 0, 0, 0, TimeSpan.Zero));
        var strictlyAfter = new DateTimeOffsetRange(
            new DateTimeOffset(2024, 01, 21, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2024, 01, 30, 0, 0, 0, TimeSpan.Zero));

        Assert.False(range.Overlaps(strictlyBefore, Inclusion.Inclusive));
        Assert.False(range.Overlaps(strictlyAfter, Inclusion.Inclusive));
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void IsAdjacentTo_True_WhenTouching(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);
        var touchesAtEnd = new DateTimeOffsetRange(testCase.End, testCase.End);
        var touchesAtStart = new DateTimeOffsetRange(testCase.Start, testCase.Start);

        // Act
        var adjacentAtEnd = range.IsAdjacentTo(touchesAtEnd);
        var adjacentAtStart = range.IsAdjacentTo(touchesAtStart);

        // Assert
        Assert.True(adjacentAtEnd);
        Assert.True(adjacentAtStart);
        Assert.Equal(testCase.Start == testCase.End, range.IsAdjacentTo(range));
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Intersect_ReturnsNull_WhenNotOverlapping(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);

        var other = testCase.Start != DateTimeOffset.MinValue
            ? new DateTimeOffsetRange(DateTimeOffset.MinValue, testCase.Start)
            : new DateTimeOffsetRange(testCase.End, testCase.End);

        // Act
        var intersection = range.Intersect(other);

        // Assert
        Assert.Null(intersection);
        Assert.False(range.Overlaps(other, Inclusion.Inclusive) && range.Overlaps(other));
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Intersect_ReturnsIntersection_WhenOverlapping(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        if (testCase.Start >= testCase.End)
        {
            return;
        }

        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);
        var other = new DateTimeOffsetRange(testCase.Start.AddTicks(1), testCase.End);

        // Act
        var intersection = range.Intersect(other);

        // Assert
        Assert.NotNull(intersection);
        Assert.Equal(other.Start, intersection.Value.Start);
        Assert.Equal(other.End, intersection.Value.End);
        Assert.True(range.Overlaps(other));
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Union_ReturnsMinStart_MaxEnd(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeOffsetRange(testCase.Start, testCase.End);
        var other = testCase.End != DateTimeOffset.MaxValue
            ? new DateTimeOffsetRange(testCase.End.AddTicks(1), testCase.End.AddTicks(1))
            : new DateTimeOffsetRange(testCase.Start, testCase.Start);

        // Act
        var union = range.Union(other);

        // Assert
        Assert.Equal(testCase.Start < other.Start ? testCase.Start : other.Start, union.Start);
        Assert.Equal(testCase.End > other.End ? testCase.End : other.End, union.End);
        Assert.True(union.Duration >= range.Duration);
    }

    [Theory]
    [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
    public void Equality_Operators_AndToString_AreConsistent(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range1 = new DateTimeOffsetRange(testCase.Start, testCase.End);
        var range2 = new DateTimeOffsetRange(testCase.Start, testCase.End);
        var different = testCase.End < DateTimeOffset.MaxValue
            ? new DateTimeOffsetRange(testCase.Start, testCase.End.AddTicks(1))
            : testCase.Start > DateTimeOffset.MinValue
                ? new DateTimeOffsetRange(testCase.Start.AddTicks(-1), testCase.End)
                : new DateTimeOffsetRange(testCase.Start, testCase.End);

        // Act
        var equalsTyped = range1.Equals(range2);
        var equalsObject = range1.Equals((object)range2);
        var equalsNullObject = range1.Equals(null);

        // Assert
        Assert.True(equalsTyped);
        Assert.True(equalsObject);
        Assert.False(equalsNullObject);
        Assert.True(range1 == range2);
        Assert.False(range1 != range2);

        Assert.Equal(range1.GetHashCode(), range2.GetHashCode());
        Assert.False(string.IsNullOrWhiteSpace(range1.ToString()));
        Assert.False(range1.Equals(new object()));

        if (different.Start != range1.Start || different.End != range1.End)
        {
            Assert.False(range1.Equals((object)different));
        }
    }
}
