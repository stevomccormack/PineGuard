using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class DateOnlyRangeTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Equality.Cases), MemberType = typeof(DateOnlyRangeTestData.Equality))]
    public void Equals_ReturnsExpected(DateOnlyRangeTestData.Equality.Case testCase)
    {
        // Act
        var result = testCase.Left.Equals(testCase.Right);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Intersect.Cases), MemberType = typeof(DateOnlyRangeTestData.Intersect))]
    public void Intersect_ReturnsExpected(DateOnlyRangeTestData.Intersect.Case testCase)
    {
        // Act
        var result = testCase.Base.Intersect(testCase.Other);

        // Assert
        if (testCase.Expected.HasValue)
        {
            Assert.NotNull(result);
            Assert.Equal(testCase.Expected.Value.Start, result.Value.Start);
            Assert.Equal(testCase.Expected.Value.End, result.Value.End);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Union.Cases), MemberType = typeof(DateOnlyRangeTestData.Union))]
    public void Union_ReturnsExpected(DateOnlyRangeTestData.Union.Case testCase)
    {
        // Act
        var result = testCase.Base.Union(testCase.Other);

        // Assert
        Assert.Equal(testCase.Expected.Start, result.Start);
        Assert.Equal(testCase.Expected.End, result.End);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Ctor_SetsStartEnd_AndDayCount(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Act
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.Start, range.Start);
        Assert.Equal(testCase.End, range.End);
        Assert.Equal(testCase.ExpectedDayCount, range.DayCount);
        Assert.Equal(TimeSpan.FromDays(testCase.ExpectedDayCount), range.Duration);
        Assert.True(range.Contains(testCase.Start));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.EdgeCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Ctor_EdgeCases_SetsStartEnd_AndDayCount(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Act
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.Start, range.Start);
        Assert.Equal(testCase.End, range.End);
        Assert.Equal(testCase.ExpectedDayCount, range.DayCount);
        Assert.Equal(TimeSpan.FromDays(testCase.ExpectedDayCount), range.Duration);
        Assert.True(range.Contains(testCase.End));
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.InvalidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Ctor_WhenStartAfterEnd_Throws(DateOnlyRangeTestData.Constructor.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = new DateOnlyRange(invalidCase.Start, invalidCase.End));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
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
    [MemberData(nameof(DateOnlyRangeTestData.TryCreate.ValidCases), MemberType = typeof(DateOnlyRangeTestData.TryCreate))]
    [MemberData(nameof(DateOnlyRangeTestData.TryCreate.EdgeCases), MemberType = typeof(DateOnlyRangeTestData.TryCreate))]
    public void TryCreate_ReturnsExpected(DateOnlyRangeTestData.TryCreate.ValidCase testCase)
    {
        // Act
        var ok = DateOnlyRange.TryCreate(testCase.Input.Start, testCase.Input.End, out var range);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, range);
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

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Overlaps.Cases), MemberType = typeof(DateOnlyRangeTestData.Overlaps))]
    public void Overlaps_ReturnsExpected(DateOnlyRangeTestData.Overlaps.Case testCase)
    {
        // Act
        var result = testCase.Base.Overlaps(testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Adjacency.Cases), MemberType = typeof(DateOnlyRangeTestData.Adjacency))]
    public void IsAdjacentTo_ReturnsExpected(DateOnlyRangeTestData.Adjacency.Case testCase)
    {
        // Act
        var result = testCase.Base.IsAdjacentTo(testCase.Other);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(DateOnlyRangeTestData.Constructor))]
    public void Intersect_ReturnsNull_WhenNotOverlapping(DateOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateOnlyRange(testCase.Start, testCase.End);

        var other = testCase.Start > DateOnly.MinValue
            ? new DateOnlyRange(DateOnly.MinValue, testCase.Start.AddDays(-1))
            : new DateOnlyRange(testCase.End.AddDays(1), testCase.End.AddDays(1));

        // Act
        var intersection = range.Intersect(other);

        // Assert
        Assert.Null(intersection);
        Assert.False(range.Overlaps(other, Inclusion.Inclusive));
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
        var equalsNullObject = range1.Equals(null);

        // Assert
        Assert.True(equalsTyped);
        Assert.True(equalsObject);
        Assert.False(equalsNullObject);
        Assert.True(range1 == range2);
        Assert.False(range1 != range2);

        Assert.Equal(range1.GetHashCode(), range2.GetHashCode());
        Assert.False(string.IsNullOrWhiteSpace(range1.ToString()));

        if (different.Start == range1.Start && different.End == range1.End) return;
        Assert.False(range1.Equals(different));
        Assert.False(range1 == different);
        Assert.True(range1 != different);

        Assert.False(range1.Equals((object)different));
    }
}
