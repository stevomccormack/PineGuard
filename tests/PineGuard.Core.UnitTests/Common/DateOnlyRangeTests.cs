using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class DateOnlyRangeTests : BaseUnitTest
{
    [Fact]
    public void Equals_ReturnsFalse_WhenDifferent()
    {
        var range = new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 20));
        var endDifferent = new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 21));
        var startDifferent = new DateOnlyRange(new DateOnly(2024, 01, 09), new DateOnly(2024, 01, 20));

        Assert.False(range.Equals(endDifferent));
        Assert.False(range.Equals(startDifferent));
    }

    [Fact]
    public void Intersect_AndUnion_CoverBothTernaryBranches()
    {
        var range = new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 20));

        var otherStartsBefore = new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 15));
        var otherStartsAfter = new DateOnlyRange(new DateOnly(2024, 01, 15), new DateOnly(2024, 01, 30));

        var intersect1 = range.Intersect(otherStartsBefore);
        Assert.NotNull(intersect1);
        Assert.Equal(new DateOnly(2024, 01, 10), intersect1.Value.Start);
        Assert.Equal(new DateOnly(2024, 01, 15), intersect1.Value.End);

        var intersect2 = range.Intersect(otherStartsAfter);
        Assert.NotNull(intersect2);
        Assert.Equal(new DateOnly(2024, 01, 15), intersect2.Value.Start);
        Assert.Equal(new DateOnly(2024, 01, 20), intersect2.Value.End);

        var union1 = range.Union(otherStartsBefore);
        Assert.Equal(new DateOnly(2024, 01, 01), union1.Start);
        Assert.Equal(new DateOnly(2024, 01, 20), union1.End);

        var union2 = range.Union(otherStartsAfter);
        Assert.Equal(new DateOnly(2024, 01, 10), union2.Start);
        Assert.Equal(new DateOnly(2024, 01, 30), union2.End);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Ctor_SetsStartEnd_AndDayCount(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange

        // Act
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.Start, range.Start);
        Assert.Equal(testCase.End, range.End);
        Assert.Equal(testCase.ExpectedDayCount, range.DayCount);
        Assert.True(range.Contains(testCase.Start));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.EdgeCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Ctor_EdgeCases_SetsStartEnd_AndDayCount(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange

        // Act
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.Start, range.Start);
        Assert.Equal(testCase.End, range.End);
        Assert.Equal(testCase.ExpectedDayCount, range.DayCount);
        Assert.True(range.Contains(testCase.End));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.InvalidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Ctor_WhenStartAfterEnd_Throws(DateOnlyRangeTestData.Constructor.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _ = new DateOnlyRange(testCase.Start, testCase.End));

        // Assert
        Assert.IsType(testCase.ExpectedException.Type, ex);
        if (testCase.ExpectedException.ParamName is not null)
            Assert.Equal(testCase.ExpectedException.ParamName, ex.ParamName);
        if (testCase.ExpectedException.MessageContains is not null)
            Assert.Contains(testCase.ExpectedException.MessageContains, ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Contains_IncludesBounds_AndExcludesOutside(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        // Act
        var containsStart = range.Contains(testCase.Start);
        var containsEnd = range.Contains(testCase.End);

        var containsBefore = testCase.Start > DateOnly.MinValue && range.Contains(testCase.Start.AddDays(-1));
        var containsAfter = testCase.End < DateOnly.MaxValue && range.Contains(testCase.End.AddDays(1));

        // Assert
        Assert.True(containsStart);
        Assert.True(containsEnd);

        if (testCase.Start > DateOnly.MinValue)
        {
            Assert.False(containsBefore);
        }

        if (testCase.End < DateOnly.MaxValue)
        {
            Assert.False(containsAfter);
        }

        Assert.True(testCase.ExpectedDayCount >= 1);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Overlaps_ExclusiveAndInclusive_BehaveAsExpected(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateOnlyRange(testCase.Start, testCase.End);
        var same = new DateOnlyRange(testCase.Start, testCase.End);

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
        var range = new DateOnlyRange(new DateOnly(2024, 01, 10), new DateOnly(2024, 01, 20));

        var beforeTouching = new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 10));
        var afterTouching = new DateOnlyRange(new DateOnly(2024, 01, 20), new DateOnly(2024, 01, 30));

        Assert.False(range.Overlaps(beforeTouching));
        Assert.False(range.Overlaps(afterTouching));

        Assert.True(range.Overlaps(beforeTouching, Inclusion.Inclusive));
        Assert.True(range.Overlaps(afterTouching, Inclusion.Inclusive));

        var strictlyBefore = new DateOnlyRange(new DateOnly(2024, 01, 01), new DateOnly(2024, 01, 09));
        var strictlyAfter = new DateOnlyRange(new DateOnly(2024, 01, 21), new DateOnly(2024, 01, 30));

        Assert.False(range.Overlaps(strictlyBefore, Inclusion.Inclusive));
        Assert.False(range.Overlaps(strictlyAfter, Inclusion.Inclusive));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void IsAdjacentTo_True_WhenTouchingByOneDay(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        // Act
        var adjacentAtEnd = testCase.End < DateOnly.MaxValue
            ? range.IsAdjacentTo(new DateOnlyRange(testCase.End.AddDays(1), testCase.End.AddDays(1)))
            : false;

        var adjacentAtStart = testCase.Start > DateOnly.MinValue
            ? range.IsAdjacentTo(new DateOnlyRange(testCase.Start.AddDays(-1), testCase.Start.AddDays(-1)))
            : false;

        // Assert
        if (testCase.End < DateOnly.MaxValue)
        {
            Assert.True(adjacentAtEnd);
        }

        if (testCase.Start > DateOnly.MinValue)
        {
            Assert.True(adjacentAtStart);
        }

        Assert.False(range.IsAdjacentTo(range));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Intersect_ReturnsNull_WhenNotOverlapping(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        var other = testCase.Start > DateOnly.MinValue
            ? new DateOnlyRange(DateOnly.MinValue, testCase.Start)
            : new DateOnlyRange(testCase.End, testCase.End);

        // Act
        var intersection = range.Intersect(other);

        // Assert
        Assert.Null(intersection);
        Assert.False(range.Overlaps(other, Inclusion.Inclusive) && range.Overlaps(other));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Intersect_ReturnsIntersection_WhenOverlapping(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        if (testCase.Start >= testCase.End || testCase.Start.AddDays(1) >= testCase.End)
        {
            return;
        }

        var range = new DateOnlyRange(testCase.Start, testCase.End);
        var other = new DateOnlyRange(testCase.Start.AddDays(1), testCase.End);

        // Act
        var intersection = range.Intersect(other);

        // Assert
        Assert.NotNull(intersection);
        Assert.Equal(other.Start, intersection.Value.Start);
        Assert.Equal(other.End, intersection.Value.End);
        Assert.True(range.Overlaps(other));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Union_ReturnsMinStart_MaxEnd(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateOnlyRange(testCase.Start, testCase.End);
        var other = testCase.End < DateOnly.MaxValue
            ? new DateOnlyRange(testCase.End.AddDays(1), testCase.End.AddDays(1))
            : new DateOnlyRange(testCase.Start, testCase.Start);

        // Act
        var union = range.Union(other);

        // Assert
        Assert.Equal(testCase.Start < other.Start ? testCase.Start : other.Start, union.Start);
        Assert.Equal(testCase.End > other.End ? testCase.End : other.End, union.End);
        Assert.True(union.DayCount >= range.DayCount);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Equality_Operators_AndToString_AreConsistent(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range1 = new DateOnlyRange(testCase.Start, testCase.End);
        var range2 = new DateOnlyRange(testCase.Start, testCase.End);
        var different = testCase.End < DateOnly.MaxValue
            ? new DateOnlyRange(testCase.Start, testCase.End.AddDays(1))
            : testCase.Start > DateOnly.MinValue
                ? new DateOnlyRange(testCase.Start.AddDays(-1), testCase.End)
                : new DateOnlyRange(testCase.Start, testCase.End);

        // Act
        var equalsTyped = range1.Equals(range2);
        var equalsObject = range1.Equals((object)range2);
        var equalsNullObject = range1.Equals((object?)null);

        // Assert
        Assert.True(equalsTyped);
        Assert.True(equalsObject);
        Assert.False(equalsNullObject);
        Assert.True(range1 == range2);
        Assert.False(range1 != range2);

        Assert.Equal(range1.GetHashCode(), range2.GetHashCode());
        Assert.False(string.IsNullOrWhiteSpace(range1.ToString()));

        if (different.Start != range1.Start || different.End != range1.End)
        {
            Assert.False(range1.Equals(different));
            Assert.False(range1 == different);
            Assert.True(range1 != different);

            Assert.False(range1.Equals((object)different));
        }
    }
}
