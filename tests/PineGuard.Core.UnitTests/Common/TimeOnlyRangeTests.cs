using System.Diagnostics.CodeAnalysis;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class TimeOnlyRangeTests : BaseUnitTest
{
    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Equality.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.Equality))]
    [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Assertions are inside the delegate invoked by testCase.Value")]
    public void Equals_ReturnsFalse_WhenDifferent(TimeOnlyRangeTestData.Equality.ValidCase testCase)
        => testCase.Value();

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Constructor.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.Constructor))]
    public void Ctor_SetsStartEnd_AndDuration(TimeOnlyRangeTestData.Constructor.ValidCase testCase)
    {
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
        // Act & Assert
        var ex = Assert.Throws(testCase.ExpectedException.Type, () => _ = new TimeOnlyRange(testCase.Start, testCase.End));
        ThrowsCaseAssert.Expected(ex, testCase);
    }

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.TryCreate.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.TryCreate))]
    [MemberData(nameof(TimeOnlyRangeTestData.TryCreate.EdgeCases), MemberType = typeof(TimeOnlyRangeTestData.TryCreate))]
    public void TryCreate_ReturnsExpected(TimeOnlyRangeTestData.TryCreate.ValidCase testCase)
    {
        // Act
        var ok = TimeOnlyRange.TryCreate(testCase.Value.Start, testCase.Value.End, out var range);

        // Assert
        Assert.Equal(testCase.Expected, ok);
        Assert.Equal(testCase.ExpectedOutValue, range);
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

    [Theory]
    [MemberData(nameof(TimeOnlyRangeTestData.Overlaps.ValidCases), MemberType = typeof(TimeOnlyRangeTestData.Overlaps))]
    [MemberData(nameof(TimeOnlyRangeTestData.Overlaps.EdgeCases), MemberType = typeof(TimeOnlyRangeTestData.Overlaps))]
    [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Assertions are inside the delegate invoked by testCase.Value")]
    public void Overlaps_ReturnsFalse_WhenRangesDoNotOverlap_OnEitherSide(TimeOnlyRangeTestData.Overlaps.ValidCase testCase)
        => testCase.Value();

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
