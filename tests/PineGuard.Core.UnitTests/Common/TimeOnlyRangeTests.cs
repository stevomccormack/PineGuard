using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class TimeOnlyRangeTests : BaseUnitTest
{
    [Fact]
    public void Equals_ReturnsFalse_WhenDifferent()
    {
        var range = new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 0));
        var endDifferent = new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(11, 1));
        var startDifferent = new TimeOnlyRange(new TimeOnly(10, 1), new TimeOnly(11, 0));

        Assert.False(range.Equals(endDifferent));
        Assert.False(range.Equals(startDifferent));
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.Constructor))]
    public void Ctor_SetsStartEnd_AndDuration(TimeOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange

        // Act
        var range = new TimeOnlyRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.Start, range.Start);
        Assert.Equal(testCase.End, range.End);
        Assert.Equal(testCase.ExpectedDuration, range.Duration);
        Assert.True(range.Contains(testCase.Start));
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Constructor.EdgeCases), MemberType = typeof(TimeOnlyRangeTestData.Constructor))]
    public void Ctor_EdgeCases_SetsStartEnd_AndDuration(TimeOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange

        // Act
        var range = new TimeOnlyRange(testCase.Start, testCase.End);

        // Assert
        Assert.Equal(testCase.ExpectedDuration, range.Duration);
        Assert.True(range.Contains(testCase.End));
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Constructor.InvalidCases), MemberType = typeof(TimeOnlyRangeTestData.Constructor))]
    public void Ctor_WhenStartAfterEnd_Throws(TimeOnlyRangeTestData.Constructor.InvalidCase testCase)
    {
        // Arrange

        // Act
        var ex = Assert.Throws<ArgumentException>(() => _ = new TimeOnlyRange(testCase.Start, testCase.End));

        // Assert
        Assert.IsType(testCase.ExpectedException.Type, ex);
        if (testCase.ExpectedException.ParamName is not null)
            Assert.Equal(testCase.ExpectedException.ParamName, ex.ParamName);
        if (testCase.ExpectedException.MessageContains is not null)
            Assert.Contains(testCase.ExpectedException.MessageContains, ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.Constructor))]
    public void Contains_IncludesBounds_AndExcludesOutside(TimeOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new TimeOnlyRange(testCase.Start, testCase.End);

        // Act
        var containsStart = range.Contains(testCase.Start);
        var containsEnd = range.Contains(testCase.End);

        var containsBefore = testCase.Start != TimeOnly.MinValue && range.Contains(testCase.Start.Add(TimeSpan.FromTicks(-1)));
        var containsAfter = testCase.End != TimeOnly.MaxValue && range.Contains(testCase.End.Add(TimeSpan.FromTicks(1)));

        // Assert
        Assert.True(containsStart);
        Assert.True(containsEnd);

        if (testCase.Start != TimeOnly.MinValue)
        {
            Assert.False(containsBefore);
        }

        if (testCase.End != TimeOnly.MaxValue)
        {
            Assert.False(containsAfter);
        }
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.Constructor))]
    public void Overlaps_ExclusiveAndInclusive_BehaveAsExpected(TimeOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range = new TimeOnlyRange(testCase.Start, testCase.End);
        var same = new TimeOnlyRange(testCase.Start, testCase.End);

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
        var range = new TimeOnlyRange(new TimeOnly(10, 0), new TimeOnly(20, 0));

        var beforeTouching = new TimeOnlyRange(new TimeOnly(1, 0), new TimeOnly(10, 0));
        var afterTouching = new TimeOnlyRange(new TimeOnly(20, 0), new TimeOnly(23, 0));

        Assert.False(range.Overlaps(beforeTouching));
        Assert.False(range.Overlaps(afterTouching));

        Assert.True(range.Overlaps(beforeTouching, Inclusion.Inclusive));
        Assert.True(range.Overlaps(afterTouching, Inclusion.Inclusive));

        var strictlyBefore = new TimeOnlyRange(new TimeOnly(1, 0), new TimeOnly(9, 59));
        var strictlyAfter = new TimeOnlyRange(new TimeOnly(20, 1), new TimeOnly(23, 0));

        Assert.False(range.Overlaps(strictlyBefore, Inclusion.Inclusive));
        Assert.False(range.Overlaps(strictlyAfter, Inclusion.Inclusive));
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.Constructor))]
    public void Equality_Operators_AndToString_AreConsistent(TimeOnlyRangeTestData.Constructor.ValidCase testCase)
    {
        // Arrange
        var range1 = new TimeOnlyRange(testCase.Start, testCase.End);
        var range2 = new TimeOnlyRange(testCase.Start, testCase.End);
        var different = testCase.End != TimeOnly.MaxValue
            ? new TimeOnlyRange(testCase.Start, testCase.End.Add(TimeSpan.FromTicks(1)))
            : testCase.Start != TimeOnly.MinValue
                ? new TimeOnlyRange(testCase.Start.Add(TimeSpan.FromTicks(-1)), testCase.End)
                : new TimeOnlyRange(testCase.Start, testCase.End);

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
        Assert.False(range1.Equals(new object()));

        if (different.Start != range1.Start || different.End != range1.End)
        {
            Assert.False(range1.Equals((object)different));
        }
    }
}
