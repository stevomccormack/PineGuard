using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class DateTimeRangeTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Equality.Cases), MemberType = typeof(DateTimeRangeTestData.Equality))]
    public void Equals_ReturnsExpected(DateTimeRangeTestData.Equality.Case testCase)
    {
        // Act
        var result = testCase.Left.Equals(testCase.Right);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Intersect.Cases), MemberType = typeof(DateTimeRangeTestData.Intersect))]
    public void Intersect_ReturnsExpected(DateTimeRangeTestData.Intersect.Case testCase)
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
    [MemberData(nameof(DateTimeRangeTestData.Union.Cases), MemberType = typeof(DateTimeRangeTestData.Union))]
    public void Union_ReturnsExpected(DateTimeRangeTestData.Union.Case testCase)
    {
        // Act
        var result = testCase.Base.Union(testCase.Other);

        // Assert
        Assert.Equal(testCase.Expected.Start, result.Start);
        Assert.Equal(testCase.Expected.End, result.End);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Ctor_SetsStartEnd_AndDuration(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Act
        var range = new DateTimeRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.Start, range.Start);
        Assert.Equal(testCase.End, range.End);
        Assert.Equal(testCase.ExpectedDuration, range.Duration);
        Assert.True(range.Contains(testCase.Start));
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.EdgeCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Ctor_EdgeCases_AllowsUnspecifiedKind(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Act
        var range = new DateTimeRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.ExpectedDuration, range.Duration);
        Assert.True(range.Contains(testCase.End));
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.InvalidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Ctor_InvalidCases_Throw(DateTimeRangeTestData.Constructor.InvalidCase testCase)
    {
        // Arrange
        var invalidCase = testCase;

        // Act
        var ex = Assert.Throws(invalidCase.ExpectedException.Type, () => _ = new DateTimeRange(invalidCase.Start, invalidCase.End));

        // Assert
        ThrowsCaseAssert.Expected(ex, invalidCase);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.TryCreate.ValidCases), MemberType = typeof(DateTimeRangeTestData.TryCreate))]
    [MemberData(nameof(DateTimeRangeTestData.TryCreate.EdgeCases), MemberType = typeof(DateTimeRangeTestData.TryCreate))]
    public void TryCreate_ReturnsExpected(DateTimeRangeTestData.TryCreate.ValidCase testCase)
    {
        // Act
        var ok = DateTimeRange.TryCreate(testCase.Input.Start, testCase.Input.End, out var range);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, range);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Contains_IncludesBounds_AndExcludesOutside(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeRange(testCase.Start, testCase.End);

        // Act
        var containsStart = range.Contains(testCase.Start);
        var containsEnd = range.Contains(testCase.End);
        var containsBefore = range.Contains(testCase.Start - TimeSpan.FromTicks(1));
        var containsAfter = range.Contains(testCase.End + TimeSpan.FromTicks(1));

        // Assert
        Assert.True(containsStart);
        Assert.True(containsEnd);

        if (testCase.Start != DateTime.MinValue)
        {
            Assert.False(containsBefore);
        }

        if (testCase.End != DateTime.MaxValue)
        {
            Assert.False(containsAfter);
        }
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Overlaps_ExclusiveAndInclusive_BehaveAsExpected(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeRange(testCase.Start, testCase.End);
        var same = new DateTimeRange(testCase.Start, testCase.End);

        // Act
        var overlapsExclusive = range.Overlaps(same);
        var overlapsInclusive = range.Overlaps(same, Inclusion.Inclusive);

        // Assert
        Assert.Equal(testCase.Start < testCase.End, overlapsExclusive);
        Assert.True(overlapsInclusive);
        Assert.Equal(range.Overlaps(same, Inclusion.Exclusive), overlapsExclusive);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Overlaps.Cases), MemberType = typeof(DateTimeRangeTestData.Overlaps))]
    public void Overlaps_ReturnsExpected(DateTimeRangeTestData.Overlaps.Case testCase)
    {
        // Act
        var result = testCase.Base.Overlaps(testCase.Other, testCase.Inclusion);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Adjacency.Cases), MemberType = typeof(DateTimeRangeTestData.Adjacency))]
    public void IsAdjacentTo_ReturnsExpected(DateTimeRangeTestData.Adjacency.Case testCase)
    {
        // Act
        var result = testCase.Base.IsAdjacentTo(testCase.Other);

        // Assert
        Assert.Equal(testCase.Expected, result);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Intersect_ReturnsNull_WhenNotOverlapping(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeRange(testCase.Start, testCase.End);

        var other = testCase.Start != DateTime.MinValue
            ? new DateTimeRange(DateTime.MinValue, testCase.Start)
            : new DateTimeRange(testCase.End, testCase.End);

        // Act
        var intersection = range.Intersect(other);

        // Assert
        Assert.Null(intersection);
        Assert.False(range.Overlaps(other, Inclusion.Inclusive) && range.Overlaps(other));
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Intersect_ReturnsIntersection_WhenOverlapping(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        if (testCase.Start >= testCase.End)
        {
            return;
        }

        var range = new DateTimeRange(testCase.Start, testCase.End);
        var other = new DateTimeRange(testCase.Start + TimeSpan.FromTicks(1), testCase.End);

        // Act
        var intersection = range.Intersect(other);

        // Assert
        Assert.NotNull(intersection);
        Assert.Equal(other.Start, intersection.Value.Start);
        Assert.Equal(other.End, intersection.Value.End);
        Assert.True(range.Overlaps(other));
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Union_ReturnsMinStart_MaxEnd(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new DateTimeRange(testCase.Start, testCase.End);
        var other = testCase.End != DateTime.MaxValue
            ? new DateTimeRange(testCase.End + TimeSpan.FromTicks(1), testCase.End + TimeSpan.FromTicks(1))
            : new DateTimeRange(testCase.Start, testCase.Start);

        // Act
        var union = range.Union(other);

        // Assert
        Assert.Equal(testCase.Start < other.Start ? testCase.Start : other.Start, union.Start);
        Assert.Equal(testCase.End > other.End ? testCase.End : other.End, union.End);
        Assert.True(union.Duration >= range.Duration);
    }

    [Theory]
    [MemberData(nameof(DateTimeRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeRangeTestData.Constructor))]
    public void Equality_Operators_AndToString_AreConsistent(DateTimeRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range1 = new DateTimeRange(testCase.Start, testCase.End);
        var range2 = new DateTimeRange(testCase.Start, testCase.End);
        var different = testCase.End < DateTime.MaxValue
            ? new DateTimeRange(testCase.Start, testCase.End.AddTicks(1))
            : testCase.Start > DateTime.MinValue
                ? new DateTimeRange(testCase.Start.AddTicks(-1), testCase.End)
                : new DateTimeRange(testCase.Start, testCase.End);

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
