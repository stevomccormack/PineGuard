using System.Diagnostics.CodeAnalysis;
using PineGuard.Common;
using PineGuard.Testing.UnitTests;

namespace PineGuard.Core.UnitTests.Common;

public sealed class DateTimeOffsetRangeTests : BaseUnitTest
{
    public static class Equal
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Equality.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Equality))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Assertions are inside the delegate invoked by testCase.Value")]
        public static void ReturnsFalse_WhenDifferent(DateTimeOffsetRangeTestData.Equality.ValidCase testCase)
            => testCase.Value.Invoke();
    }

    public static class IntersectAndUnion
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.IntersectAndUnion.EdgeCases), MemberType = typeof(DateTimeOffsetRangeTestData.IntersectAndUnion))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Assertions are inside the delegate invoked by testCase.Value")]
        public static void CoverBothTernaryBranches(DateTimeOffsetRangeTestData.IntersectAndUnion.ValidCase testCase)
            => testCase.Value.Invoke();
    }

    public static class Constructor
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void SetsStartEnd_AndDuration(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
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
        public static void EdgeCases_SetStartEnd_AndDuration(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
        {
            // Act
            var range = new DateTimeOffsetRange(testCase.Start, testCase.End);

            // Assert
            Assert.Equal(testCase.ExpectedDuration, range.Duration);
            Assert.True(range.Contains(testCase.End));
        }

        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.InvalidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void WhenStartAfterEnd_Throws(DateTimeOffsetRangeTestData.Constructor.InvalidCase testCase)
        {
            // Act & Assert
            var ex = Assert.Throws(testCase.ExpectedException.Type, () => _ = new DateTimeOffsetRange(testCase.Start, testCase.End));
            ThrowsCaseAssert.Expected(ex, testCase);
        }
    }

    public static class Contains
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void IncludesBounds_AndExcludesOutside(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
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
    }

    public static class TryCreate
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.TryCreate.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.TryCreate))]
        [MemberData(nameof(DateTimeOffsetRangeTestData.TryCreate.EdgeCases), MemberType = typeof(DateTimeOffsetRangeTestData.TryCreate))]
        public static void ReturnsExpected(DateTimeOffsetRangeTestData.TryCreate.ValidCase testCase)
        {
            // Act
            var ok = DateTimeOffsetRange.TryCreate(testCase.Input.Start, testCase.Input.End, out var range);

            // Assert
            Assert.Equal(testCase.Expected, ok);
            Assert.Equal(testCase.ExpectedOutValue, range);
        }
    }

    public static class Overlaps
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void ExclusiveAndInclusive_BehaveAsExpected(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
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

        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Overlaps.EdgeCases), MemberType = typeof(DateTimeOffsetRangeTestData.Overlaps))]
        [SuppressMessage("Assertion", "S2699:Tests should include assertions", Justification = "Assertions are inside the delegate invoked by testCase.Value")]
        public static void ReturnsFalse_WhenRangesDoNotOverlap_OnEitherSide(DateTimeOffsetRangeTestData.Overlaps.ValidCase testCase)
            => testCase.Value.Invoke();
    }

    public static class IsAdjacentTo
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void True_WhenTouching(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
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
    }

    public static class Intersect
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void ReturnsNull_WhenNotOverlapping(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
        {
            // Arrange
            var range = new DateTimeOffsetRange(testCase.Start, testCase.End);

            var other = testCase.Start != DateTimeOffset.MinValue
                ? new DateTimeOffsetRange(DateTimeOffset.MinValue, testCase.Start.AddTicks(-1))
                : new DateTimeOffsetRange(testCase.End.AddTicks(1), testCase.End.AddTicks(1));

            // Act
            var intersection = range.Intersect(other);

            // Assert
            Assert.Null(intersection);
            Assert.False(range.Overlaps(other, Inclusion.Inclusive));
        }

        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void ReturnsIntersection_WhenOverlapping(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
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
    }

    public static class Union
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void ReturnsMinStart_MaxEnd(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
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
    }

    public static class Equality
    {
        [Theory]
        [MemberData(nameof(DateTimeOffsetRangeTestData.Constructor.ValidCases), MemberType = typeof(DateTimeOffsetRangeTestData.Constructor))]
        public static void Operators_AndToString_AreConsistent(DateTimeOffsetRangeTestData.Constructor.ValidCase testCase)
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
}
